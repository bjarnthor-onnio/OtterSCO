using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class useVoucherForTender : Message
    {
        public useVoucherForTenderParams @params { get; set; }
        public useVoucherForTender()
        {
            method = GetType().Name;
        }
    }


    public class useVoucherForTenderParams : Params
    {
    }

}
