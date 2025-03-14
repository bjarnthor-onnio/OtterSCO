using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class receipt : Message
    {
        public receiptParams @params { get; set; }
        public receipt()
        {
            method = this.GetType().Name;
        }
    }

    public class receiptParams : Params
    {
        public List<printerData> print { get; set; }

    }



    public class printerData
    {
        public string name { get; set; }
        public string ftype { get; set; }
        public string data { get; set; }
    }
}
