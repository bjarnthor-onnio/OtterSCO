using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class reprintReceipt : Message
    {
        public reprintReceiptParams @params { get; set; }
        public reprintReceipt()
        {
            method = GetType().Name;
        }
    }


    public class reprintReceiptParams : Params
    {
    }

}
