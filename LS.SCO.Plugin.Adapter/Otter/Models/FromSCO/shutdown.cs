using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class shutdown : Message
    {
        public shutdownParams @params { get; set; }
        public shutdown()
        {
            method = this.GetType().Name;
        }
    }


    public class shutdownParams : Params
    {
        public string shutdown { get; set; }
    }

}
