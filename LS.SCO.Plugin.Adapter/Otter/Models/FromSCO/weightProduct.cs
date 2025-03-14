using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class weightProduct : Message
    {
        public weightProductParams @params { get; set; }
        public weightProduct()
        {
            method = this.GetType().Name;
        }
    }


    public class weightProductParams : Params
    {
        public string barcode { get; set; }
    }

}
