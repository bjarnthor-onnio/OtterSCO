using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Service.Interfaces;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    public class DefaultHandler : MessageHandler
    {
        public DefaultHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager,SamplePosAdapter samplePosAdapter)
            : base(otterState, otterProtocolHandler, manager,samplePosAdapter) { }

        public override void Handle(object message)
        {
            // Implementation of CheckIfTransHasItems
        }
    }
}
