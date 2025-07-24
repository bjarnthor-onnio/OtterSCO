using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class intervention : Message
    {
        public interventionParams @params { get; set; }

        public intervention()
        {
            method = GetType().Name;
        }

    }
    public class interventionParams : Params
    {
        public string code { get; set; }
        public string text { get; set; }
        public string instructionsText { get; set; }
        public List<InterventionButton> buttons { get; set; }
        public string type { get; set; }
    }
    public class InterventionButton
    {
        public string buttonText { get; set; }
        public string buttonId { get; set; }
    }
}
