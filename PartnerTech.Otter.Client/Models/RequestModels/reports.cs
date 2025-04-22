using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class reports : Message
    {
        public ReportsParams @params { get; set; }
        public reports() 
        {
            method = GetType().Name;
        }
    }


    public class ReportsParams : Params
    {
    }

}
