using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class goPayment : Message
    {
        public goPaymentParams @params { get; set; }
        public goPayment()
        {
            method = this.GetType().Name;
        }
    }


    public class goPaymentParams : Params
    {
    }

}
