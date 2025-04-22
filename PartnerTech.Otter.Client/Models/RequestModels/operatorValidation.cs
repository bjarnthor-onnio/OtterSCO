using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class operatorValidation : Message
    {
        public operatorValidationParams? @params { get; set; }
        public operatorValidation()
        {
            method = GetType().Name;
        }
    }

    public class operatorValidationParams : Params
    {
        public string? operatorId { get; set; }
        public string? password { get; set; }
        public string? key { get; set; }
    }
}
