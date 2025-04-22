using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class loyalty : Message
    {
        public LoyaltyParams @params { get; set; }
        public loyalty()
        {
            method = GetType().Name;
        }
    }


    public class LoyaltyParams : Params
    {
        public string identifier { get; set; }
    }

}
