using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class Init : Message
    {
        public InitResult? result { get; set; }
        public Init() 
        {
            method = GetType().Name;
        }
    }

    public class InitResult : Result
    {

    }
}
