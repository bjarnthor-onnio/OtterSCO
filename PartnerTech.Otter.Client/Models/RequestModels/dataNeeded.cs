using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class dataNeeded : Message
    {
        public dataNeededResult? result { get; set; }
        public dataNeeded()
        {
            method = GetType().Name;
        }
    }


    public class dataNeededResult : Result
    {
        public bool back { get; set; }
        public string? data { get; set; }
        public string? buttonId { get; set; }
    }

}
