using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.SCO.Entity.DTO.SCOService.CurrentTransaction;
using LS.SCO.Entity.Extensions;
using LS.SCO.Entity.Model.Toshiba.POS;
using LS.SCO.Interfaces.Services.NCR;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using LS.SCO.Plugin.Service.Interfaces;
using LS.SCO.Plugin.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    public class LaneIdleHandler : MessageHandler
    {
        public LaneIdleHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager,SamplePosAdapter samplePosAdapter)
            : base(otterState, otterProtocolHandler, manager, samplePosAdapter) { }

        public override async void Handle(object message)
        {
            var _currTransaction = await _adapter.GetCurrentTransaction();
            if (_currTransaction.IsValid() && _currTransaction.Transaction.NotVoidedAndExists())
            {
                _otterState.Pos_TransactionId = _currTransaction.Transaction.ReceiptId;
                if (_currTransaction.Transaction.NoOfItemLines > 0)
                {
                    fullBasketResult res = new fullBasketResult();

                    res.products = new List<Products_fullBasket>();
                    foreach (var saleItem in _currTransaction.Transaction.SaleItems.Where(x => x.Voided == false))
                    {
                        Products_fullBasket p = new Products_fullBasket();
                        var productDiscounts = new List<Otter.Models.FromPOS.Discounts_fullBasket>();
                        if (saleItem.PeriodicDiscountAmount > 0)
                        {
                            productDiscounts.Add(new Discounts_fullBasket()
                            {
                                discountAmount = (int)saleItem.PeriodicDiscountAmount * 100,
                                discountText = saleItem.PeriodicDiscountDescription
                            });
                        }
                        if(saleItem.PriceReductions.Count > 0)
                        {
                            var discount = saleItem.PriceReductions.First();
                            productDiscounts.Add(new Discounts_fullBasket()
                            {
                                discountAmount = Math.Abs((int)discount.Amount * 10),
                                discountText = discount.PromotionText
                            });
                        }
                        p.barcode = saleItem.ID;
                        p.productId = saleItem.LineNr;
                        p.linkedProductId = null;                                    //TODO - NEED SOLUTION - feature flag??
                        p.name = saleItem.Description;
                        p.quantity = !saleItem.ScaleItem ? (int)saleItem.Quantity : null;
                        p.weight = saleItem.ScaleItem ? (int)saleItem.Quantity : null;
                        p.department = null;                                          //TODO - anything to map here?
                        p.price = (int)(saleItem.PriceWithTax * 100);
                        p.totalPrice = (int)((saleItem.NetAmount + saleItem.TaxAmount) * 100);
                        p.customerAge = saleItem.MinimumCustomerAge;                                     //TODO - test
                        p.visualVerify = false;
                        p.securityMode = "SkipBagging";
                        p.discount = productDiscounts;
                        p.discountText = productDiscounts.Count() > 0 ? productDiscounts.Last()?.discountText : "";


                        res.products.Add(p);
                    }
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.fullBasket
                    {
                        @params = res

                    });
                    _otterEventsManager.sendTotals(_currTransaction.Transaction.BalanceAmountWithTax, _currTransaction.Transaction.NetAmountWithTax);
                    
                    
                    _otterState.Api_MessageId = null;
                }
            }
            else
            {
                var trans = _adapter.StartTransactionAsync();
                if (trans != null)
                {
                    _otterState.Pos_TransactionId = trans.Result.Transaction.ReceiptId;
                }
            }
        }
    }
}
