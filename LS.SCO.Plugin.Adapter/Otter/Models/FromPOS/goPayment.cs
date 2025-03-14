using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class goPayment : Message
    {
        public goPaymentResult result { get; set; }

        public goPayment()
        {
            method = this.GetType().Name;
        }
    }

    public class goPaymentResult : Result
    {

    }
}
