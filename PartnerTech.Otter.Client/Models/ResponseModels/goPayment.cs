using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class goPayment : Message
    {
        public goPaymentResult result { get; set; }

        public goPayment()
        {
            method = GetType().Name;
        }
    }

    public class goPaymentResult : Result
    {

    }
}
