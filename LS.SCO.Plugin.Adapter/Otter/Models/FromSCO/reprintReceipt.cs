using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class reprintReceipt : Message
    {
        public reprintReceiptParams @params { get; set; }
        public reprintReceipt()
        {
            method = this.GetType().Name;
        }
    }


    public class reprintReceiptParams : Params
    {
    }

}
