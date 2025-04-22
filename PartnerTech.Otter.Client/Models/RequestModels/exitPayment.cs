using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class exitPayment : Message
    {
        public exitPaymentParams @params { get; set; }
        public exitPayment()
        {
            method = GetType().Name;
        }
    }


    public class exitPaymentParams : Params
    {
    }

}
