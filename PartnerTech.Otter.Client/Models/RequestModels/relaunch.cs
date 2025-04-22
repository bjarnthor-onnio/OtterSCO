using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class relaunch : Message
    {
        public relaunchParams? @params { get; set; }
        public relaunch()
        {
            method = GetType().Name;
        }
    }


    public class relaunchParams : Params
    {
        public string? relaunch { get; set; }
    }

}
