using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    internal class scalesWeight : Message
    {
        public scalesWeightParams @params { get; set; }
        public scalesWeight()
        {
            method = GetType().Name;
        }
    }

    public class scalesWeightParams : Params
    {
        public int weight { get; set; }
    }


}
