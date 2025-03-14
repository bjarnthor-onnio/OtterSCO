using System;
using System.Collections.Generic;
using System.Text;

namespace LS.SCO.Plugin.Adapter.Otter.Models.FromPOS
{
    public class dataNeeded : Message
    {
        public dataNeededParams @params { get; set; }
        public dataNeeded()
        {
            method = this.GetType().Name;
        }
    }

    public class dataNeededParams : Params
    {
        public string titleText { get; set; }
        public string instructionsText { get; set; }
        public bool? operatorMode { get; set; }
        public string imageFilePath { get; set; }
        public string soundFilePath { get; set; }
        public bool? scannerEnabled { get; set; }
        public string[] expectedCodeTypes { get; set; }
        public bool? keyPad { get; set; }
        public int? keyPadInputMask { get; set; }
        public int? minimalInputLength { get; set; }
        public string keyPadPattern { get; set; }
        public bool? keyBoard { get; set; }
        public int? exitButton { get; set; }
        public string lightsColor { get; set; }

        public List<Button> buttons { get; set; }

        public bool? clearScreen { get; set; }
        public bool? deviceError { get; set; }

    }


    public class Button
    {
        public string buttonId { get; set; }
        public string buttonText { get; set; }
        public string buttonImage { get; set; }
    }

}
