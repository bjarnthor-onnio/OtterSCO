using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class replenish : Message
    {
        public replenishResult result { get; set; }
        public replenish()
        {
            method = GetType().Name;
        }
    }

    public class replenishResult : Result
    {
        public string message { get; set; }
        public bool successful { get; set; }

        public int amount { get; set; }
    }


}
