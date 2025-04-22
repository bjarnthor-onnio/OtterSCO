using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class goPayment : Message
    {
        public goPaymentParams? @params { get; set; }
        public goPayment()
        {
            method = GetType().Name;
        }
    }


    public class goPaymentParams : Params
    {
    }

}
