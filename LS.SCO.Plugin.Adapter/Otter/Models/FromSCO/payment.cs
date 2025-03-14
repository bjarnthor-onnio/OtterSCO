using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class payment : Message
    {
        public paymentParams @params { get; set; }
        public payment()
        {
            method = this.GetType().Name;
        }
    }


    public class paymentParams : Params
    {
        public string type { get; set; }
        public int amount { get; set; }

    }

}
