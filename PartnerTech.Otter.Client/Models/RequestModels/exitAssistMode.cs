using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class exitAssistMode : Message
    {
        public exitAssistModeParams? @params { get; set; }
        public exitAssistMode()
        {
            method = GetType().Name;
        }
    }


    public class exitAssistModeParams : Params
    {
        public string? shutdown { get; set; }
    }

}
