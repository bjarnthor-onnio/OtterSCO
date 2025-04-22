using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class replenish : Message
    {
        public replenishParams? @params { get; set; }
        public replenish()
        {
            method = GetType().Name;
        }
    }

    public class replenishParams : Params
    {
        public int? amount { get; set; }
    }

}
