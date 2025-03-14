using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class withdraw : Message
    {
        public withdrawResult result { get; set; }
        public withdraw()
        {
            method = this.GetType().Name;
        }
    }

    public class withdrawResult : Result
    {
        public string message { get; set; }
        public bool successful { get; set; }
        public int amount { get; set; }
    }


}
