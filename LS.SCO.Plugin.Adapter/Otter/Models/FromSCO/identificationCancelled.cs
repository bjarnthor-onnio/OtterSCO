using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class identificationCancelled : Message
    {
        public identificationCancelledParams @params { get; set; }
        public identificationCancelled()
        {
            method = this.GetType().Name;
        }
    }

    public class identificationCancelledParams : Params
    {
        //  public string barcode { get; set; }

    }

}
