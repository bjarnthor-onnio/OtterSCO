using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class identification : Message
    {
        public identificationResult result { get; set; }
        public identification() 
        {
            method = this.GetType().Name;
        }
    }

    public class identificationResult : Result
    {
        public string message { get; set; }
        public bool successful { get; set; }
    }


}
