using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class printLastReceipt : Message
    {
        public printLastReceiptParams @params { get; set; }
        public printLastReceipt()
        {
            method = this.GetType().Name;
        }
    }


    public class printLastReceiptParams : Params
    {

    }
}
