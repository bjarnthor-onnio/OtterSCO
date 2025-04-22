using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class withdraw : Message
    {
        public withdrawParams @params { get; set; }
        public withdraw()
        {
            method = GetType().Name;
        }
    }

    public class withdrawParams : Params
    {
        public int amount { get; set; }
    }

}
