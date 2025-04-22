using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class suspendTransaction : Message
    {
        public suspendTransactionParams @params { get; set; }
        public suspendTransaction()
        {
            method = GetType().Name;
        }
    }

    public class suspendTransactionParams : Params
    {
        public string transactionId { get; set; }
    }

}
