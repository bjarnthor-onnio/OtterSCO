using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class enterAssistMode : Message
    {
        public enterAssistModeParams @params { get; set; }
        public enterAssistMode()
        {
            method = GetType().Name;
        }
    }


    public class enterAssistModeParams : Params
    {
        public string shutdown { get; set; }
    }

}
