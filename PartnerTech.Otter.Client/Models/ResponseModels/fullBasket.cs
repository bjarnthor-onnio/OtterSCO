using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class fullBasket : Message
    {
        public fullBasketResult @params { get; set; }
        public fullBasket()
        {
            method = GetType().Name;
        }
    }


    public class fullBasketResult : Params
    {
        public List<Products_fullBasket> products { get; set; }
    }

    public class Products_fullBasket
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
        public List<Discounts_fullBasket> discount { get; set; }
        public string discountText { get; set; }
        public int? customerAge { get; set; }
        public bool? visualVerify { get; set; }
        public string securityMode { get; set; }

        public ProductExceptions_fullBasket productExceptions { get; set; }

        public LoyaltyCard_fullBasket loyaltyCard { get; set; }
    }

    public class Discounts_fullBasket
    {
        public int discountAmount { get; set; }
        public string discountText { get; set; }
    }

    public class ProductExceptions_fullBasket
    {
        public bool? weight { get; set; }
        public bool? quantity { get; set; }
        public bool? quantityLimit { get; set; }
        public bool? notFound { get; set; }
        public bool? timeBlock { get; set; }
        public string message { get; set; }
        public bool? price { get; set; }
    }

    public class LoyaltyCard_fullBasket
    {
        public bool successful { get; set; }
        public string cardData { get; set; }
        public string language { get; set; }
        public string greetingText { get; set; }
        public string message { get; set; }
    }



}
