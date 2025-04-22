using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class printReservationCode : Message
    {
        public printReservationCodeResult result { get; set; }
        public printReservationCode()
        {
            method = GetType().Name;
        }

    }

    public class printReservationCodeResult : Result
    {

    }
}
