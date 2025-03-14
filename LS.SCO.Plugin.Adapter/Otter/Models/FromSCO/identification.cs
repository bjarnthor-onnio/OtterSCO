using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class identification : Message
    {
        public identificationParams @params { get; set; }
        public identification()
        {
            method = this.GetType().Name;
        }
    }

    public class identificationParams : Params
    {
        public string barcode { get; set; }

    }

}
