using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class shutdown : Message
    {
        public shutdownParams @params { get; set; }
        public shutdown()
        {
            method = GetType().Name;
        }
    }


    public class shutdownParams : Params
    {
        public string shutdown { get; set; }
    }

}
