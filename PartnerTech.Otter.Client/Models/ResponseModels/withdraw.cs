using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class withdraw : Message
    {
        public withdrawResult result { get; set; }
        public withdraw()
        {
            method = GetType().Name;
        }
    }

    public class withdrawResult : Result
    {
        public string message { get; set; }
        public bool successful { get; set; }
        public int amount { get; set; }
    }


}
