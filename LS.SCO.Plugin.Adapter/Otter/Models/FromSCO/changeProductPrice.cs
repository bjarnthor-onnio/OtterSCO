using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class changeProductPrice : Message
    {
        public changeProductPriceParams @params { get; set; }
        public changeProductPrice()
        {
            method = this.GetType().Name;
        }
    }

    public class changeProductPriceParams : Params
    {
        public string barcode { get; set; }
        public int productId { get; set; }
        public int price { get; set; }
    }

}
