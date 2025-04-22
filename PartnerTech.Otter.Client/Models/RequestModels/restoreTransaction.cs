using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class restoreTransaction : Message
    {
        public restoreTransactionParams @params { get; set; }
        public restoreTransaction()
        {
            method = GetType().Name;
        }
    }


    public class restoreTransactionParams : Params
    {
    }

}
