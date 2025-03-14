using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class loyalty : Message
    {
        public LoyaltyParams @params { get; set; }
        public loyalty()
        {
            method = this.GetType().Name;
        }
    }


    public class LoyaltyParams : Params
    {
        public string identifier { get; set; }
    }

}
