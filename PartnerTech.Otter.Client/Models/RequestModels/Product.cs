using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class product : Message
    {
        public ProductParams? @params { get; set; }
        public product()
        {
            method = GetType().Name;
        }
    }

    public class ProductParams : Params
    {
        public string? barcode { get; set; }
        public bool? scanned { get; set; }
        public int? quantity { get; set; }
        public int? weight { get; set; }
        public decimal? price { get; set; }
    }

}
