using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class changeProductPrice : Message
    {
        public changeProductPriceParams @params { get; set; }
        public changeProductPrice()
        {
            method = GetType().Name;
        }
    }

    public class changeProductPriceParams : Params
    {
        public string barcode { get; set; }
        public int productId { get; set; }
        public int price { get; set; }
    }

}
