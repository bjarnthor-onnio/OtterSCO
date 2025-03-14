using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class setMode : Message
    {
        public setModeParams @params { get; set; }
        public setMode()
        {
            method = this.GetType().Name;
        }
    }


    public class setModeParams : Params
    {
        public string mode { get; set; }
    }

}
