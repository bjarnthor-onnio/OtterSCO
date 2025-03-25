using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    internal class transactionFinish : Message
    {
        public transactionFinishParams @params { get; set; }
        public transactionFinish()
        {
            method = this.GetType().Name;
        }
    }

    public class transactionFinishParams : Params
    {
        public string transactionId { get; set; }
        public string additionalText { get; set; }
        public bool askForReceipt { get; set; }
    }


}
