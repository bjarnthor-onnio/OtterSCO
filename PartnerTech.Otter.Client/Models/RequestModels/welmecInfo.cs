using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class welmecInfo : Message
    {
        public welmecInfoParams @params { get; set; }
        public welmecInfo()
        {
            method = GetType().Name;
        }
    }


    public class welmecInfoParams : Params
    {
        public string shutdown { get; set; }
    }

}
