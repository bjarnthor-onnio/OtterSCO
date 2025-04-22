using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class enterPOSCode : Message
    {
        public enterPOSCodeParams @params { get; set; }
        public enterPOSCode()
        {
            method = GetType().Name;
        }
    }


    public class enterPOSCodeParams : Params
    {
    }

}
