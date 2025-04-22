using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class company : Message
    {
        public companyParams @params { get; set; }
        public company()
        {
            method = GetType().Name;
        }
    }


    public class companyParams : Params
    {
        public string identifier { get; set; }
    }

}
