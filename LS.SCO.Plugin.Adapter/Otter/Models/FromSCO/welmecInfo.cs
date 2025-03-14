using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class welmecInfo : Message
    {
        public welmecInfoParams @params { get; set; }
        public welmecInfo()
        {
            method = this.GetType().Name;
        }
    }


    public class welmecInfoParams : Params
    {
        public string shutdown { get; set; }
    }

}
