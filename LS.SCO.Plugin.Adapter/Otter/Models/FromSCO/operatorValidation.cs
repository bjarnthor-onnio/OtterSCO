using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class operatorValidation : Message
    {
        public operatorValidationParams @params { get; set; }
        public operatorValidation()
        {
            method = this.GetType().Name;
        }
    }

    public class operatorValidationParams : Params
    {
        public string operatorId { get; set; }
        public string password { get; set; }
        public string key { get; set; }
    }
}
