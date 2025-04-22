using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class Init : Message
    {
        public InitParams @params { get; set; }
        public Init() 
        { 
            method = GetType().Name;
        }
    }

    public class InitParams : Params
    {
        public string laneNumber { get; set; }
        public string storeNumber { get; set; }
        public string posVersion { get; set; }
        public string posId { get; set; }
    }
}
