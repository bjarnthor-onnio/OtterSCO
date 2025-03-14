using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class changeProductQuantity : Message
    {
        public changeProductQuantityParams @params { get; set; }
        public changeProductQuantity()
        {
            method = this.GetType().Name;
        }
    }

    public class changeProductQuantityParams : Params
    {
        public string barcode { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public int currentQuantity { get; set; }
    }

}
