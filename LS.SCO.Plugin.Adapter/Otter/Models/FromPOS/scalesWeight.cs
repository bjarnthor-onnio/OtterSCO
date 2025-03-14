using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    internal class scalesWeight : Message
    {
        public scalesWeightParams @params { get; set; }
        public scalesWeight()
        {
            method = this.GetType().Name;
        }
    }

    public class scalesWeightParams : Params
    {
        public int weight { get; set; }
    }


}
