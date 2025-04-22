using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class setMode : Message
    {
        public setModeParams @params { get; set; }
        public setMode()
        {
            method = GetType().Name;
        }
    }


    public class setModeParams : Params
    {
        public string mode { get; set; }
    }

}
