using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class replenish : Message
    {
        public replenishResult result { get; set; }
        public replenish()
        {
            method = this.GetType().Name;
        }
    }

    public class replenishResult : Result
    {
        public string message { get; set; }
        public bool successful { get; set; }

        public int amount { get; set; }
    }


}
