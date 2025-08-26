using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class printLastReceipt : Message
    {
        public printLastReceiptResult result { get; set; }
        public printLastReceipt()
        {
            method = this.GetType().Name;
        }

    }

    public class printLastReceiptResult : Result
    {

    }
}
