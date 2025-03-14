using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class product : Message
    {
        public ProductParams @params { get; set; }
        public product()
        {
            method = this.GetType().Name;
        }
    }

    public class ProductParams : Params
    {
        public string barcode { get; set; }
        public bool? scanned { get; set; }
        public int? quantity { get; set; }
        public int? weight { get; set; }
        public decimal? price { get; set; }
    }

}
