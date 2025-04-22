using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class exitPayment : Message
    {
        public exitPaymentResult result { get; set; }
        public exitPayment()
        {
            method = GetType().Name;
        }

    }

    public class exitPaymentResult : Result
    {

    }



}
