using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class withdraw : Message
    {
        public withdrawParams @params { get; set; }
        public withdraw()
        {
            method = this.GetType().Name;
        }
    }

    public class withdrawParams : Params
    {
        public int amount { get; set; }
    }

}
