using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class exitPayment : Message
    {
        public exitPaymentParams @params { get; set; }
        public exitPayment()
        {
            method = this.GetType().Name;
        }
    }


    public class exitPaymentParams : Params
    {
    }

}
