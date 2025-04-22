using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    public class changeProductQuantity : Message
    {
        public changeProductQuantityResult result { get; set; }
        public changeProductQuantity()
        {
            method = GetType().Name;
        }

    }

    public class changeProductQuantityResult : Result
    {

    }



}
