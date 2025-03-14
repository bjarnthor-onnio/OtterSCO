using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class enterPOSCode : Message
    {
        public enterPOSCodeParams @params { get; set; }
        public enterPOSCode()
        {
            method = this.GetType().Name;
        }
    }


    public class enterPOSCodeParams : Params
    {
    }

}
