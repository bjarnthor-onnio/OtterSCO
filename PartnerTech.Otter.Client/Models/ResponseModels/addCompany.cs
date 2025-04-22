using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class addCompany : Message
    {
        public addCompanyParams @params { get; set; }
        public addCompany()
        {
            method = GetType().Name;
        }
    }

    public class addCompanyParams : Params
    {
        public bool successful { get; set; }
        public string Identifier { get; set; }
        public companyExceptions companyExceptions { get; set; }

    }

    public class companyExceptions
    {
        public bool invalidCode { get; set; }
    }

}
