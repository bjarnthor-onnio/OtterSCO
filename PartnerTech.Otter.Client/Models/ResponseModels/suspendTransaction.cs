using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class suspendTransaction : Message
    {
        public suspendTransactionResult result { get; set; }
        public suspendTransaction()
        {
            method = GetType().Name;
        }

    }

    public class suspendTransactionResult : Result
    {

    }



}
