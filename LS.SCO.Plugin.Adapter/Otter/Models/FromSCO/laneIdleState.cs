using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class laneIdleState : Message
    {
        public laneIdleStateParams @params { get; set; }
        public laneIdleState()
        {
            method = this.GetType().Name;
        }
    }


    public class laneIdleStateParams : Params
    {
    }

}
