using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Entity.DTO.SCOService.CurrentTransaction;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using Onnio.PaymentService.Services;
using WSSCOGetCurrentTransaction;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class GoPaymentHandler : MessageHandler
    {
        public GoPaymentHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter) : base(otterState, otterProtocolHandler, manager, samplePosAdapter)
        {
        }

        public override void Handle(object message)
        {
            GetCurrentTransactionOutputDto currentTransaction = null;
            
            bool recalcNeeded = _adapter.LobicoTrigger(_otterState.Pos_TransactionId);
            var msg = (Otter.Models.FromSCO.goPayment)message;
            
            if (recalcNeeded)
            {
                currentTransaction = _adapter.GetCurrentTransaction().Result;

                var discountItems = currentTransaction.Transaction.SaleItems.Where(x => (x.HasLineDiscount || x.HasPeriodicDiscount) );
                foreach (var item in discountItems)
                {
                    addProduct addProduct = new addProduct();
                    addProduct.@params = new Otter.Models.FromPOS.addProductParams();
                    addProduct.@params.successful = true;
                    addProduct.@params.quantity = (item.Quantity % 1 != 0) ? 1 : (int?)item.Quantity;
                    addProduct.@params.weight = (item.Quantity % 1 != 0) ? (int?)(item.Quantity * 1000) : 0;
                    addProduct.@params.name = item.Description;
                    addProduct.@params.securityMode = "SkipBagging";
                    addProduct.@params.productId = item.LineNr;
                    addProduct.@params.price = (int)item.PriceWithTax * 100;
                    addProduct.@params.totalPrice = (int)((item.NetAmount + item.TaxAmount) * 100);
                    addProduct.@params.discount = new List<Otter.Models.FromPOS.Discount_addProduct>();
                    
                    addProduct.@params.discount.Add(new Otter.Models.FromPOS.Discount_addProduct()
                    {
                        discountAmount = (int)item.PeriodicDiscountAmount * 100,
                        discountText = item.PeriodicDiscountDescription
                    });

                    addProduct.id = new Guid().ToString();
                    _otterProtocolHandler.SendMessage(addProduct);

                }

                _otterEventsManager.sendTotals(currentTransaction.Transaction.BalanceAmountWithTax, currentTransaction.Transaction.NetAmountWithTax);

            }
            

            _otterProtocolHandler.SendMessage(new goPayment
            {
                id = msg.id,
                result = new goPaymentResult
                {
                    successful = true,

                }
            });
            _otterState.Api_MessageId = null;
            
            
        }
    }
}