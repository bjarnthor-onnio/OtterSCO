using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class identification : Message
    {
        public identificationResult result { get; set; }
        public identification() 
        {
            method = GetType().Name;
        }
    }

    public class identificationResult : Result
    {
        public string message { get; set; }
        public bool successful { get; set; }
    }


}
