using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class cancelProduct : Message
    {
        public cancelProductParams @params { get; set; }
        public cancelProduct()
        {
            method = this.GetType().Name;
        }
    }

    public class cancelProductParams : Params
    {
        public string barcode { get; set; }
        public int productId { get; set; }
    }

}
