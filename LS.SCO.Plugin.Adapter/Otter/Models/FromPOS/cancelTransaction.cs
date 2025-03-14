using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class cancelTransaction : Message
    {
        public cancelTransactionResult result { get; set; }
        public cancelTransaction()
        {
            method = this.GetType().Name;
        }

    }

    public class cancelTransactionResult : Result
    {

    }



}
