using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Service.Interfaces;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    public class InitHandler : MessageHandler
    {
        public InitHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager,
            SamplePosAdapter samplePosAdapter): base(otterState, otterProtocolHandler, manager,samplePosAdapter) { }

        public override void Handle(object message)
        {
            _otterProtocolHandler.StopInitialization();
            _otterState.State = OtterState.StatesEnum.Idle;
            
        }
    }
}
