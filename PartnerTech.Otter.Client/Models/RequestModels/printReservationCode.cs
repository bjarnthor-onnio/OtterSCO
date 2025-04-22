using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class printReservationCode : Message
    {
        public printReservationCodeParams? @params { get; set; }
        public printReservationCode()
        {
            method = GetType().Name;
        }
    }

    public class printReservationCodeParams : Params
    {
        public string? code { get; set; }
    }

}
