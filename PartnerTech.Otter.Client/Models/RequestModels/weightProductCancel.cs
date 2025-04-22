using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class weightProductCancel : Message
    {
        public weightProductCancelParams @params { get; set; }
        public weightProductCancel()
        {
            method = GetType().Name;
        }
    }


    public class weightProductCancelParams : Params
    {
    }

}
