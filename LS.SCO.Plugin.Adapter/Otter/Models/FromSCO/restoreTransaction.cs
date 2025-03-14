using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class restoreTransaction : Message
    {
        public restoreTransactionParams @params { get; set; }
        public restoreTransaction()
        {
            method = this.GetType().Name;
        }
    }


    public class restoreTransactionParams : Params
    {
    }

}
