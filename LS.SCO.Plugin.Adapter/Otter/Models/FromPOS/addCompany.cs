using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class addCompany : Message
    {
        public addCompanyParams @params { get; set; }
        public addCompany()
        {
            method = this.GetType().Name;
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
