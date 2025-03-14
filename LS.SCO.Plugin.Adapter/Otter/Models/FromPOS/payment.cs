using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class payment : Message
    {
        public paymentResult result { get; set; }
        public payment()
        {
            method = this.GetType().Name;
        }
    }

    public class paymentResult : Result
    {

        public string type { get; set; }
        public int amount { get; set; }


    }

}
