using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    internal class transactionStart : Message
    {
        public transactionStartParams @params { get; set; }
        public transactionStart()
        {
            method = this.GetType().Name;
        }
    }

    public class transactionStartParams : Params
    {
        public string transactionId { get; set; }
    }


}
