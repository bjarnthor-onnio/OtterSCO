using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class getProductsPrices : Message
    {
        public getProductsPricesResult result { get; set; }

        public getProductsPrices()
        {
            method = GetType().Name;
        }
    }

    public class getProductsPricesResult : Result
    {

    }
}
