using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class payment : Message
    {
        public paymentResult result { get; set; }
        public payment()
        {
            method = GetType().Name;
        }
    }

    public class paymentResult : Result
    {

        public string type { get; set; }
        public int amount { get; set; }


    }

}
