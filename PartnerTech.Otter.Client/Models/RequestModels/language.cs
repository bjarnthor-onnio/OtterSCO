using System;
using System.Collections.Generic;
using System.Text;
using PartnerTech.Otter.Client.Models.BaseModels;

namespace PartnerTech.Otter.Client.Models.RequestModels
{
    public class language : Message
    {
        public LanguageParams? @params { get; set; }
        public language()
        {
            method = GetType().Name;
        }
    }


    public class LanguageParams : Params
    {
        public string? language { get; set; }
    }

}
