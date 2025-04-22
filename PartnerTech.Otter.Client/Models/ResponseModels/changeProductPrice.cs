using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class changeProductPrice : Message
    {
        public changeProductPriceResult result { get; set; }
        public changeProductPrice()
        {
            method = GetType().Name;
        }

    }

    public class changeProductPriceResult : Result
    {

    }



}
