using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class laneIdleState : Message
    {
        public laneIdleStateParams @params { get; set; }
        public laneIdleState()
        {
            method = GetType().Name;
        }
    }


    public class laneIdleStateParams : Params
    {
    }

}
