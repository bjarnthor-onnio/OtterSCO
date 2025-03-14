using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class dataNeeded : Message
    {
        public dataNeededResult result { get; set; }
        public dataNeeded()
        {
            method = this.GetType().Name;
        }
    }


    public class dataNeededResult : Result
    {
        public bool back { get; set; }
        public string data { get; set; }
        public string buttonId { get; set; }
    }

}
