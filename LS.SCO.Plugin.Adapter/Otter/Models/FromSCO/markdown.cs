using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class markdown : Message
    {
        public markdownParams @params { get; set; }
        public markdown()
        {
            method = this.GetType().Name;
        }
    }

    public class markdownParams : Params
    {
        public string barcode { get; set; }
        public int productId { get; set; }
    }

}
