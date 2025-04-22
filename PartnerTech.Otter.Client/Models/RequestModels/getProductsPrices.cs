using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class getProductsPrices : Message
    {
        public getProductsPricesParams @params { get; set; }
        public getProductsPrices()
        {
            method = GetType().Name;
        }
    }


    public class getProductsPricesParams : Params
    {
        public List<string> barcodes { get; set; }
    }

}
