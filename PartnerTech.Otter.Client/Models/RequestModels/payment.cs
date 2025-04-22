using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class payment : Message
    {
        public paymentParams? @params { get; set; }
        public payment()
        {
            method = GetType().Name;
        }
    }


    public class paymentParams : Params
    {
        public string? type { get; set; }
        public int amount { get; set; }

    }

}
