using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class cancelProduct_request : Message
    {
        public cancelProduct_request_Params @params { get; set; }
        public cancelProduct_request()
        {
            method = GetType().Name;
        }
    }

    public class cancelProduct_request_Params : Params
    {
        public string barcode { get; set; }
        public int productId { get; set; }
    }



}
