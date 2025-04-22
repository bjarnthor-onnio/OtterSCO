using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class product : Message
    {
        public ProductResult result { get; set; }

        public product()
        {
            method = GetType().Name;
        }

        //   public ProductException ProductExceptions { get; set; }
    }

    //public class ProductException : ProductExceptions
    //{
    //    public bool weight { get; set; }
    //    public bool quantity { get; set; }
    //    public bool quantityLimit { get; set; }
    //    public bool notFound { get; set; }
    //    public bool timeBlock { get; set; }
    //}

    public class ProductResult : Result
    {
        public string barcode { get; set; }
        public int? productId { get; set; }
        public int? linkedProductId { get; set; }
        public string name { get; set; }
        public int? quantity { get; set; }
        public int? weight { get; set; }
        public string department { get; set; }
        public int? price { get; set; }
        public int? totalPrice { get; set; }
        public List<Discounts> discount { get; set; }
        public string discountText { get; set; }
        public int? customerAge { get; set; }
        public bool? visualVerify { get; set; }
        public string securityMode { get; set; }

        public ProductExceptions productExceptions { get; set; }

        public LoyaltyCard loyaltyCard { get; set; }

        public bool? isCoupon { get; set; }
    }

    public class Discounts
    {
        public int discountAmount { get; set; }
        public string discountText { get; set; }
    }

    public class ProductExceptions
    {
        public bool? weight { get; set; }
        public bool? quantity { get; set; }
        public bool? quantityLimit { get; set; }
        public bool? notFound { get; set; }
        public bool? timeBlock { get; set; }
        public string message { get; set; }
        public bool? price { get; set; }
    }

    public class LoyaltyCard
    {
        public bool successful { get; set; }
        public string cardData { get; set; }
        public string language { get; set; }
        public string greetingText { get; set; }
        public string message { get; set; }
    }



}
