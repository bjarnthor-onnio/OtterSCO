using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class exitPayment : Message
    {
        public exitPaymentResult result { get; set; }
        public exitPayment()
        {
            method = this.GetType().Name;
        }

    }

    public class exitPaymentResult : Result
    {

    }



}
