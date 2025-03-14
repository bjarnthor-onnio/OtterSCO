using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class exitAssistMode : Message
    {
        public exitAssistModeParams @params { get; set; }
        public exitAssistMode()
        {
            method = this.GetType().Name;
        }
    }


    public class exitAssistModeParams : Params
    {
        public string shutdown { get; set; }
    }

}
