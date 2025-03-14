﻿using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.GetItemDetails;
using LS.SCO.Entity.DTO.SCOService.Items;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class ProductHandler : MessageHandler
    {
        public ProductHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager,
            SamplePosAdapter samplePosAdapter) : base(otterState, otterProtocolHandler, manager, samplePosAdapter) { }

        public async override void Handle(object message)
        {
            var msg = (Otter.Models.FromSCO.product)message;
            _otterState.Api_MessageId_Product = msg.id;

            if (string.IsNullOrEmpty(_otterState.Pos_TransactionId))
            {
                var _currTransaction = await _adapter.GetCurrentTransaction();
                if (_currTransaction.Transaction != null && _currTransaction.Transaction.TransactionId != null)
                {
                    _otterState.Pos_TransactionId = _currTransaction.Transaction.ReceiptId;
                }
            }

            if (string.IsNullOrEmpty(_otterState.Pos_TransactionId))
            {
                await _adapter.StartTransactionAsync();
            }

            var input = new GetItemDetailsInputDto();

            input.ConfigureBaseInputProperties(_adapter);
            input.ItemCode = msg.@params.barcode;

            var itemDetails = await _adapter.GetItemDetailAsync(input);
            bool isCoupon = false;

            if (itemDetails.ErrorList?.Count() > 0)
            {
                if (itemDetails.ErrorList.First().ErrorMessage == "Error Code: 2002 - Coupon")
                {
                    isCoupon = true;
                }
                else
                {
                    Console.WriteLine("################   ERROR DURING PRODUCT");
                    //TODO check error code and map to reponse
                    _otterProtocolHandler.SendMessage(OtterMessages.ProductError(_otterState.Api_MessageId_Product, itemDetails.ErrorList.First().ErrorMessage));
                    return;
                }
            }
            if (!isCoupon)
            {
                //Check if item is available for sale
                if (itemDetails.NotForSale || itemDetails.NotForScoSale)
                {
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.product
                    {
                        result = new Otter.Models.FromPOS.ProductResult()

                        {
                            message = "Not for sale",
                            successful = false,
                            productExceptions = new Otter.Models.FromPOS.ProductExceptions()
                            {
                                message = "Item is not for sale"
                            }
                        },
                        id = _otterState.Api_MessageId_Product
                    });

                    return;
                }

                //Check if quantity is required
                if (itemDetails.QuantityEntryRequired && msg.@params.quantity == null)
                {
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.product
                    {
                        result = new Otter.Models.FromPOS.ProductResult()
                        {
                            message = "",
                            successful = false,
                            productExceptions = new Otter.Models.FromPOS.ProductExceptions()
                            {
                                message = "",
                                quantity = true
                            }
                        },
                        id = _otterState.Api_MessageId_Product
                    });

                    return;
                }

                //Check if weight is required
                if (itemDetails.WeighingRequired && msg.@params.weight == null)
                {
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.product
                    {
                        result = new Otter.Models.FromPOS.ProductResult()

                        {
                            message = "",
                            successful = false,
                            productExceptions = new Otter.Models.FromPOS.ProductExceptions()
                            {
                                message = "",
                                weight = true
                            }
                        },
                        id = _otterState.Api_MessageId_Product
                    });

                    return;
                }

                //Check if price is required
                if (itemDetails.PriceEntryRequired && (msg.@params.price == null || msg.@params.price == 0))
                {
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.product
                    {
                        result = new Otter.Models.FromPOS.ProductResult()

                        {
                            message = "",
                            successful = false,
                            productExceptions = new Otter.Models.FromPOS.ProductExceptions()
                            {
                                message = "",
                                price = true
                            }
                        },
                        id = _otterState.Api_MessageId_Product
                    });

                    return;
                }

                //Check if quantity limit reached
                if (itemDetails.QuantityDisallowed)
                {
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.product
                    {
                        result = new Otter.Models.FromPOS.ProductResult()

                        {
                            message = "",
                            successful = false,
                            productExceptions = new Otter.Models.FromPOS.ProductExceptions()
                            {
                                message = "",
                                quantityLimit = true
                            }
                        },
                        id = _otterState.Api_MessageId_Product
                    });

                    return;
                }
            }

            //TODO Check if time restricted
            decimal weight = msg.@params.weight > 0 ? (decimal)msg.@params.weight : 0;

            //Add item to transaction
            decimal qty = itemDetails.WeighingRequired ? (weight / 1000) : 1;

            int ageLimit = 0;
            int.TryParse(itemDetails.FeatureFlags?.Find(x => x.Flag == "AgeLimit")?.Value, out ageLimit);

            bool visualVerify = false;
            bool.TryParse(itemDetails.FeatureFlags?.Find(x => x.Flag == "VisualVerify")?.Value, out visualVerify);

            int customerAge = 0;
            int.TryParse(itemDetails.FeatureFlags?.Find(x => x.Flag == "CustomerAge")?.Value, out customerAge);

            string securityMode = itemDetails.FeatureFlags?.Find(x => x.Flag == "SecurityMode")?.Value ?? "Default";

            Console.WriteLine("################   SECURITY MODE: " + securityMode);

            // TODO - call LsCentral web service directly for extra features
            // var clnt = new LsClient();
            // clnt.AddToTransAsync(msg.@params.barcode, qty);

            var addItem = _adapter.AddItemToTransaction(msg.@params.barcode, _otterState.Pos_TransactionId, qty).Result;
            bool discountAdded = false;
            if (addItem.ErrorList?.Count() > 0)
            {


                Console.WriteLine("################   ERROR DURING Add Item To Transaction at LsCentral");
                _otterProtocolHandler.SendMessage(OtterMessages.ProductError(_otterState.Api_MessageId_Product, addItem.ErrorList.First().ErrorMessage));
            }
            else
            {
                SaleItemDto saleItem = addItem.Transaction.SaleItems?.OrderBy(x => x.LineNr).Last();
                var productDiscounts = new List<Otter.Models.FromPOS.Discounts>();
                //TODO GET discounts from LsCentral for product
                if (saleItem.PeriodicDiscountAmount > 0)
                {
                    discountAdded = true;

                    productDiscounts.Add(new Discounts()
                    {
                        discountAmount = (int)saleItem.PeriodicDiscountAmount * 100,
                        discountText = saleItem.PeriodicDiscountDescription
                    });
                }
                product p = new product();
                p.result = new Otter.Models.FromPOS.ProductResult();

                p.result.barcode = itemDetails.ItemId;
                p.result.productId = saleItem.LineNr;
                p.result.linkedProductId = null;                                    //TODO - NEED SOLUTION - feature flag??
                p.result.name = saleItem.Description;
                p.result.quantity = (saleItem.Quantity % 1 != 0) ? 1 : (int?)saleItem.Quantity;
                p.result.weight = (saleItem.Quantity % 1 != 0) ? (int?)(saleItem.Quantity * 1000) : 0;
                p.result.department = null;                                          //TODO - anything to map here?
                p.result.price = (int)(saleItem.PriceWithTax * 100);
                p.result.totalPrice = (int)((saleItem.NetAmount + saleItem.TaxAmount) * 100);
                p.result.discount = productDiscounts;
                p.result.discountText = productDiscounts.Count() > 0 ? productDiscounts.Last()?.discountText : "";       //TODO - test
                p.result.customerAge = ageLimit;                                     //TODO - test
                p.result.visualVerify = visualVerify;                                //TODO - test
                p.result.securityMode = securityMode;
                p.result.successful = true;
                p.id = _otterState.Api_MessageId_Product;
                _otterProtocolHandler.SendMessage(p);

                if (discountAdded)
                {
                    var discountItems = addItem.Transaction.SaleItems.Where(x => (x.HasLineDiscount || x.HasPeriodicDiscount) && x.LineNr != saleItem.LineNr);
                    foreach (var item in discountItems)
                    {
                        addProduct addProduct = new addProduct();
                        addProduct.@params = new Otter.Models.FromPOS.addProductParams();
                        addProduct.@params.successful = true;
                        addProduct.@params.quantity = (saleItem.Quantity % 1 != 0) ? 1 : (int?)saleItem.Quantity;
                        addProduct.@params.weight = (saleItem.Quantity % 1 != 0) ? (int?)(saleItem.Quantity * 1000) : 0;
                        addProduct.@params.name = item.Description;
                        addProduct.@params.securityMode = "SkipBagging";
                        addProduct.@params.productId = item.LineNr;
                        addProduct.@params.price = (int)item.PriceWithTax * 100;
                        addProduct.@params.totalPrice = (int)((saleItem.NetAmount + saleItem.TaxAmount) * 100);
                        addProduct.@params.discount = new List<Otter.Models.FromPOS.Discount_addProduct>();
                        addProduct.@params.barcode = itemDetails.ItemId;
                        addProduct.@params.discount.Add(new Otter.Models.FromPOS.Discount_addProduct()
                        {
                            discountAmount = (int)item.PeriodicDiscountAmount * 100,
                            discountText = item.PeriodicDiscountDescription
                        });
                        addProduct.id = _otterState.Api_MessageId_Product;
                        _otterProtocolHandler.SendMessage(addProduct);

                    }
                }

                _otterEventsManager.sendTotals(addItem.Transaction.BalanceAmountWithTax, addItem.Transaction.NetAmountWithTax);
            }
        }
    }
}