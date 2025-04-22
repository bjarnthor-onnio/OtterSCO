using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    internal class transactionStart : Message
    {
        public transactionStartParams @params { get; set; }
        public transactionStart()
        {
            method = GetType().Name;
        }
    }

    public class transactionStartParams : Params
    {
        public string transactionId { get; set; }
    }


}
