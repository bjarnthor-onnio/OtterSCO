using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class cancelTransaction : Message
    {
        public cancelTransactionResult result { get; set; }
        public cancelTransaction()
        {
            method = GetType().Name;
        }

    }

    public class cancelTransactionResult : Result
    {

    }



}
