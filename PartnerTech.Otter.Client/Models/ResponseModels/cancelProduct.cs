using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class cancelProduct : Message
    {
        public cancelProductResult result { get; set; }
        public cancelProduct()
        {
            method = GetType().Name;
        }

    }

    public class cancelProductResult : Result
    {

    }



}
