using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    internal class Init : Message
    {
        public InitResult result { get; set; }
        public Init() 
        {
            method = this.GetType().Name;
        }
    }

    public class InitResult : Result
    {

    }
}
