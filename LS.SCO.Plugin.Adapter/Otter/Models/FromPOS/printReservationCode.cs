using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class printReservationCode : Message
    {
        public printReservationCodeResult result { get; set; }
        public printReservationCode()
        {
            method = this.GetType().Name;
        }

    }

    public class printReservationCodeResult : Result
    {

    }
}
