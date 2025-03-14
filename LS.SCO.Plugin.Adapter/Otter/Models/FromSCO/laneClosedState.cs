using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class laneClosedState : Message
    {
        public laneClosedStateParams @params { get; set; }
        public laneClosedState()
        {
            method = this.GetType().Name;
        }
    }


    public class laneClosedStateParams : Params
    {
    }

}
