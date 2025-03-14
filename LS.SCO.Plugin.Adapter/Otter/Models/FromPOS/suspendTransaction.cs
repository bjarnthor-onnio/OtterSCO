using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class suspendTransaction : Message
    {
        public suspendTransactionResult result { get; set; }
        public suspendTransaction()
        {
            method = this.GetType().Name;
        }

    }

    public class suspendTransactionResult : Result
    {

    }



}
