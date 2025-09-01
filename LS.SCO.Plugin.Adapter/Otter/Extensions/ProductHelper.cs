using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.SCO.Entity.DTO.SCOService.CurrentTransaction;
using LS.SCO.Entity.DTO.SCOService.GetItemDetails;
using LS.SCO.Entity.DTO.SCOService.Items;
using LS.SCO.Entity.Model.Toshiba.POS;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

namespace LS.SCO.Plugin.Adapter.Otter.Extensions
{
    public static class ProductHelper
    {
        public static product PopulateProduct(SaleItemDto saleItem)
        {
            product p = new product();
            p.result = new Otter.Models.FromPOS.ProductResult();
            p.result.barcode = saleItem.Barcode;
            p.result.productId = saleItem.LineNr;
            p.result.linkedProductId = null;                                    //TODO - NEED SOLUTION - feature flag??
            p.result.name = saleItem.Description;
            p.result.quantity = (saleItem.Quantity % 1 != 0) ? 1 : (int?)saleItem.Quantity;
            p.result.weight = (saleItem.Quantity % 1 != 0) ? (int?)(saleItem.Quantity * 1000) : 0;
            p.result.department = null;
            p.result.price = (int)(saleItem.PriceWithTax * 100);
            p.result.totalPrice = (int)((saleItem.NetAmount + saleItem.TaxAmount) * 100);
           
           
            p.result.successful = true;
           
            var productDiscounts = new List<Otter.Models.FromPOS.Discounts>();
            //TODO GET discounts from LsCentral for product
            if (saleItem.PeriodicDiscountAmount > 0)
            {
                productDiscounts.Add(new Discounts()
                {
                    discountAmount = (int)saleItem.PeriodicDiscountAmount * 100,
                    discountText = saleItem.PeriodicDiscountDescription
                });
            }
            else if (saleItem.PriceReductions.Count > 0)
            {
                var discount = saleItem.PriceReductions.First();
                productDiscounts.Add(new Discounts()
                {
                    discountAmount = Math.Abs((int)discount.Amount * 10),
                    discountText = discount.PromotionText
                });
            }
            p.result.discount = productDiscounts;
            return p;
        }
        public static addProduct PopulateAddProduct(SaleItemDto saleItem)
        {
            addProduct addProduct = new addProduct();
            addProduct.@params = new Otter.Models.FromPOS.addProductParams();
            addProduct.@params.successful = true;
            addProduct.@params.quantity = (saleItem.Quantity % 1 != 0) ? 1 : (int?)saleItem.Quantity;
            addProduct.@params.weight = (saleItem.Quantity % 1 != 0) ? (int?)(saleItem.Quantity * 1000) : 0;
            addProduct.@params.name = saleItem.Description;
            addProduct.@params.securityMode = "SkipBagging";
            addProduct.@params.productId = saleItem.LineNr;
            addProduct.@params.price = (int)saleItem.PriceWithTax * 100;
            addProduct.@params.totalPrice = (int)((saleItem.NetAmount + saleItem.TaxAmount) * 100);
            addProduct.@params.discount = new List<Otter.Models.FromPOS.Discount_addProduct>();
            addProduct.@params.barcode = saleItem.Barcode;
            if (saleItem.PeriodicDiscountAmount > 0)
            {
                addProduct.@params.discount.Add(new Otter.Models.FromPOS.Discount_addProduct()
                {
                    discountAmount = (int)saleItem.PeriodicDiscountAmount * 100,
                    discountText = saleItem.PeriodicDiscountDescription
                });
            }
            else if (saleItem.PriceReductions.Count > 0)
            {
                var discount = saleItem.PriceReductions.First();
                addProduct.@params.discount.Add(new Otter.Models.FromPOS.Discount_addProduct()
                {
                    discountAmount = Math.Abs((int)discount.Amount * 10),
                    discountText = discount.PromotionText
                });
            }
            return addProduct;
        }
        public static fullBasketResult PopulateFullBasket(GetCurrentTransactionOutputDto Transaction)
        {
            fullBasketResult res = new fullBasketResult();

            res.products = new List<Products_fullBasket>();
            foreach (var saleItem in Transaction.Transaction.SaleItems.Where(x => x.Voided == false))
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
                else if (saleItem.PriceReductions.Count > 0)
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
            return res;
        }
    }
}
