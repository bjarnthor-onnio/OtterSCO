using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class getProductsPrices : Message
    {
        public getProductsPricesResult result { get; set; }

        public getProductsPrices()
        {
            method = this.GetType().Name;
        }
    }

    public class getProductsPricesResult : Result
    {

    }
}
