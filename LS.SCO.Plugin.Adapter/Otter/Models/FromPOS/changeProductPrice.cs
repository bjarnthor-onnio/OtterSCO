using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class changeProductPrice : Message
    {
        public changeProductPriceResult result { get; set; }
        public changeProductPrice()
        {
            method = this.GetType().Name;
        }

    }

    public class changeProductPriceResult : Result
    {

    }



}
