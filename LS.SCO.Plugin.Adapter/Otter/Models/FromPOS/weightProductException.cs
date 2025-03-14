using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    internal class weightProductException : Message
    {
        public weightProductExceptionParams @params { get; set; }
        public weightProductException()
        {
            method = this.GetType().Name;
        }
    }

    public class weightProductExceptionParams : Params
    {
        public string message { get; set; }
        public bool? close { get; set; }
    }


}
