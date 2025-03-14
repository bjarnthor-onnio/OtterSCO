using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class cancelProduct : Message
    {
        public cancelProductResult result { get; set; }
        public cancelProduct()
        {
            method = this.GetType().Name;
        }

    }

    public class cancelProductResult : Result
    {

    }



}
