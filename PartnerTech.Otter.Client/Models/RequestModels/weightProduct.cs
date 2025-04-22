using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class weightProduct : Message
    {
        public weightProductParams @params { get; set; }
        public weightProduct()
        {
            method = GetType().Name;
        }
    }


    public class weightProductParams : Params
    {
        public string barcode { get; set; }
    }

}
