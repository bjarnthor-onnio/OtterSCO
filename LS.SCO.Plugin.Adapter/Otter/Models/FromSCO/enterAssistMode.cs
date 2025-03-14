using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class enterAssistMode : Message
    {
        public enterAssistModeParams @params { get; set; }
        public enterAssistMode()
        {
            method = this.GetType().Name;
        }
    }


    public class enterAssistModeParams : Params
    {
        public string shutdown { get; set; }
    }

}
