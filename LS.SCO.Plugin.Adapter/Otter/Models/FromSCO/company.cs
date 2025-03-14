using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class company : Message
    {
        public companyParams @params { get; set; }
        public company()
        {
            method = this.GetType().Name;
        }
    }


    public class companyParams : Params
    {
        public string identifier { get; set; }
    }

}
