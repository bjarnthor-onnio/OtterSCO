using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class cancelTransaction : Message
    {
        public cancelTransactionParams @params { get; set; }
        public cancelTransaction()
        {
            method = GetType().Name;
        }
    }

    public class cancelTransactionParams : Params
    {
        public string transactionId { get; set; }
    }

}
