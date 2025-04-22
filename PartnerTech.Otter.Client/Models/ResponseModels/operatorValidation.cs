using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class operatorValidation : Message
    {
        public operatorValidationResult result { get; set; }

        public operatorValidation() 
        {
            method = GetType().Name;
        }
    }

    public class operatorValidationResult : Result
    {
        public string operatorId { get; set; }
        public string level { get; set; }
    }
}
