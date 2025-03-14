using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class Init : Message
    {
        public InitParams @params { get; set; }
        public Init() 
        { 
            method = this.GetType().Name;
        }
    }

    public class InitParams : Params
    {
        public string laneNumber { get; set; }
        public string storeNumber { get; set; }
        public string posVersion { get; set; }
    }
}
