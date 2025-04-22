using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class laneClosedState : Message
    {
        public laneClosedStateParams? @params { get; set; }
        public laneClosedState()
        {
            method = GetType().Name;
        }
    }


    public class laneClosedStateParams : Params
    {
    }

}
