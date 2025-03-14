using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class getProductsPrices : Message
    {
        public getProductsPricesParams @params { get; set; }
        public getProductsPrices()
        {
            method = this.GetType().Name;
        }
    }


    public class getProductsPricesParams : Params
    {
        public List<string> barcodes { get; set; }
    }

}
