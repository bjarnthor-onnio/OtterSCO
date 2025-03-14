using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class loyaltyCard : Message
    {
        public loyaltyCardParams @params { get; set; }
        public loyaltyCard()
        {
            method = this.GetType().Name;
        }
    }


    public class loyaltyCardParams : Params
    {
        public string track1 { get; set; }
        public string track2 { get; set; }
        public string track3 { get; set; }
    }

}
