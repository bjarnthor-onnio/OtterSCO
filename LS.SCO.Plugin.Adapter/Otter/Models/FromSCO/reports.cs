using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class reports : Message
    {
        public ReportsParams @params { get; set; }
        public reports() 
        {
            method = this.GetType().Name;
        }
    }


    public class ReportsParams : Params
    {
    }

}
