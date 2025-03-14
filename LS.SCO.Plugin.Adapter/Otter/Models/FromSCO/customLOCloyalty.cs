using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class customLOCloyalty : Message
    {
        public customLOCloyaltyParams @params { get; set; }
        public customLOCloyalty()
        {
            method = this.GetType().Name;
        }
    }


    public class customLOCloyaltyParams : Params
    {
    }

}
