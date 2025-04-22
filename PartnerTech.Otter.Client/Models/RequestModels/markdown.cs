using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class markdown : Message
    {
        public markdownParams @params { get; set; }
        public markdown()
        {
            method = GetType().Name;
        }
    }

    public class markdownParams : Params
    {
        public string barcode { get; set; }
        public int productId { get; set; }
    }

}
