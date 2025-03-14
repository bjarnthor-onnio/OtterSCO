using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class restoreTransaction : Message
    {
        public restoreTransactionResult result { get; set; }
        public restoreTransaction()
        {
            method = this.GetType().Name; 
        }

    }

    public class restoreTransactionResult : Result
    {

    }
}
