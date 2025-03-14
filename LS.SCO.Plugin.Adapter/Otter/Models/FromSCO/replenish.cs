using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class replenish : Message
    {
        public replenishParams @params { get; set; }
        public replenish()
        {
            method = this.GetType().Name;
        }
    }

    public class replenishParams : Params
    {
        public int amount { get; set; }
    }

}
