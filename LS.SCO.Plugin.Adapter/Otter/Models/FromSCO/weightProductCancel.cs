using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class weightProductCancel : Message
    {
        public weightProductCancelParams @params { get; set; }
        public weightProductCancel()
        {
            method = this.GetType().Name;
        }
    }


    public class weightProductCancelParams : Params
    {
    }

}
