using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    internal class transactionFinish : Message
    {
        public transactionFinishParams @params { get; set; }
        public transactionFinish()
        {
            method = GetType().Name;
        }
    }

    public class transactionFinishParams : Params
    {
        public string transactionId { get; set; }
        public string additionalText { get; set; }
        public bool askForReceipt { get; set; }
    }


}
