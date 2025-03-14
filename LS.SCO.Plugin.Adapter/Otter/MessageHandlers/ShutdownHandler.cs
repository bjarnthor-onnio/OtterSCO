using LS.SCO.Plugin.Adapter.Adapters;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class ShutdownHandler : MessageHandler
    {
        public ShutdownHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, 
            SamplePosAdapter samplePosAdapter) : base(otterState, otterProtocolHandler, manager, samplePosAdapter){}

        public override void Handle(object message)
        {
            
        }
    }
}