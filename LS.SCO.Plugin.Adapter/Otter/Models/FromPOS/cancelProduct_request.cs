using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class cancelProduct_request : Message
    {
        public cancelProduct_request_Params @params { get; set; }
        public cancelProduct_request()
        {
            method = this.GetType().Name;
        }
    }

    public class cancelProduct_request_Params : Params
    {
        public string barcode { get; set; }
        public int productId { get; set; }
    }



}
