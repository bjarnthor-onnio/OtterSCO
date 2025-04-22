using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class identificationCancelled : Message
    {
        public identificationCancelledParams @params { get; set; }
        public identificationCancelled()
        {
            method = GetType().Name;
        }
    }

    public class identificationCancelledParams : Params
    {
        //  public string barcode { get; set; }

    }

}
