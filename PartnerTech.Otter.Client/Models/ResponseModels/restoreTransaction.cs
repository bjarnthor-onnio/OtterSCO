using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class restoreTransaction : Message
    {
        public restoreTransactionResult result { get; set; }
        public restoreTransaction()
        {
            method = GetType().Name; 
        }

    }

    public class restoreTransactionResult : Result
    {

    }
}
