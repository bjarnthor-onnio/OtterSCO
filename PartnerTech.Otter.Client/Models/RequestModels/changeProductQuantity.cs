using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class changeProductQuantity : Message
    {
        public changeProductQuantityParams? @params { get; set; }
        public changeProductQuantity()
        {
            method = GetType().Name;
        }
    }

    public class changeProductQuantityParams : Params
    {
        public string? barcode { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public int currentQuantity { get; set; }
    }

}
