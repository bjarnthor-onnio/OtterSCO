using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class relaunch : Message
    {
        public relaunchParams @params { get; set; }
        public relaunch()
        {
            method = this.GetType().Name;
        }
    }


    public class relaunchParams : Params
    {
        public string relaunch { get; set; }
    }

}
