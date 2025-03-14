using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class language : Message
    {
        public LanguageParams @params { get; set; }
        public language()
        {
            method = this.GetType().Name;
        }
    }


    public class LanguageParams : Params
    {
        public string language { get; set; }
    }

}
