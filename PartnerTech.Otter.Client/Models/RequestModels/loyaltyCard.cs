using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class loyaltyCard : Message
    {
        public loyaltyCardParams? @params { get; set; }
        public loyaltyCard()
        {
            method = GetType().Name;
        }
    }


    public class loyaltyCardParams : Params
    {
        public string? track1 { get; set; }
        public string? track2 { get; set; }
        public string? track3 { get; set; }
    }

}
