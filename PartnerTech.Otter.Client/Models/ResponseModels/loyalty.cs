using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class loyalty : Message
    {
        public loyaltyResult result { get; set; }

        public loyalty()
        {
            method = GetType().Name;
        }
    }

    public class loyaltyResult : Result
    {
        public string message { get; set; }
        public bool successful { get; set; }
    }


}
