using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class loyalty : Message
    {
        public loyaltyResult result { get; set; }

        public loyalty()
        {
            method = this.GetType().Name;
        }
    }

    public class loyaltyResult : Result
    {
        public string message { get; set; }
        public bool successful { get; set; }
    }


}
