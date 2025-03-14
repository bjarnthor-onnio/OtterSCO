using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class useVoucherForTender : Message
    {
        public useVoucherForTenderParams @params { get; set; }
        public useVoucherForTender()
        {
            method = this.GetType().Name;
        }
    }


    public class useVoucherForTenderParams : Params
    {
    }

}
