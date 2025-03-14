using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class cancelTransaction : Message
    {
        public cancelTransactionParams @params { get; set; }
        public cancelTransaction()
        {
            method = this.GetType().Name;
        }
    }

    public class cancelTransactionParams : Params
    {
        public string transactionId { get; set; }
    }

}
