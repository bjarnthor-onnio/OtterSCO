using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromSCO
{
    public class intervention : Message
    {
        public interventionResult result { get; set; }
        public intervention()
        {
            method = this.GetType().Name;
        }
    }
    public class interventionResult 
    {
        public string buttonId { get; set; }
    }

}
