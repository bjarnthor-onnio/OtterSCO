using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class changeProductQuantity : Message
    {
        public changeProductQuantityResult result { get; set; }
        public changeProductQuantity()
        {
            method = this.GetType().Name;
        }

    }

    public class changeProductQuantityResult : Result
    {

    }



}
