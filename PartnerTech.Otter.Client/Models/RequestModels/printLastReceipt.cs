using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class printLastReceipt : Message
    {
        public printLastReceiptParams? @params { get; set; }
        public printLastReceipt()
        {
            method = GetType().Name;
        }
    }


    public class printLastReceiptParams : Params
    {

    }
}
