using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.ResponseModels
{
    internal class weightProductException : Message
    {
        public weightProductExceptionParams @params { get; set; }
        public weightProductException()
        {
            method = GetType().Name;
        }
    }

    public class weightProductExceptionParams : Params
    {
        public string message { get; set; }
        public bool? close { get; set; }
    }


}
