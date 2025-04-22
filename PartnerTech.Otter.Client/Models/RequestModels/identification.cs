using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class identification : Message
    {
        public identificationParams? @params { get; set; }
        public identification()
        {
            method = GetType().Name;
        }
    }

    public class identificationParams : Params
    {
        public string? barcode { get; set; }

    }

}
