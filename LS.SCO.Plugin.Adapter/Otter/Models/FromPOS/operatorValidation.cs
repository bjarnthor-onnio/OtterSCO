using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class operatorValidation : Message
    {
        public operatorValidationResult result { get; set; }

        public operatorValidation() 
        {
            method = this.GetType().Name;
        }
    }

    public class operatorValidationResult : Result
    {
        public string operatorId { get; set; }
        public string level { get; set; }
    }
}
