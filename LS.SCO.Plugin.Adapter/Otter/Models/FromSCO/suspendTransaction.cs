using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class suspendTransaction : Message
    {
        public suspendTransactionParams @params { get; set; }
        public suspendTransaction()
        {
            method = this.GetType().Name;
        }
    }

    public class suspendTransactionParams : Params
    {
        public string transactionId { get; set; }
    }

}
