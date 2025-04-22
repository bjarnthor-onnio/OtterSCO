using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class addProduct : Message
    {
        public addProductParams @params { get; set; }
        public addProduct()
        {
            method = GetType().Name;
        }
    }

    public class addProductParams : Params
    {
        public bool successful { get; set; }
        public string barcode { get; set; }
        public int productId { get; set; }
        public int? linkedProductId { get; set; }
        public string name { get; set; }
        public int? quantity { get; set; }
        public int? weight { get; set; }
        public string department { get; set; }
        public int? price { get; set; }
        public int? totalPrice { get; set; }
        public List<Discount_addProduct> discount { get; set; }
        public string discountText { get; set; }
        public int? customerAge { get; set; }
        public string securityMode { get; set; }
        public bool? visualVerify { get; set; }

        public bool? isCoupon { get; set; }

        public LoyaltyCard loyaltyCard { get; set; }
    }

    public class Discount_addProduct
    {
        public int discountAmount { get; set; }
        public string discountText { get; set; }
    }

}
