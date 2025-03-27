using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class ExitPaymentHandler : MessageHandler
    {
        public ExitPaymentHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter) 
            : base(otterState, otterProtocolHandler, manager, samplePosAdapter)
        {
        }

        public override void Handle(object message)
        {
            var msg = message as Otter.Models.FromSCO.exitPayment;
            _otterState.Api_MessageId = msg.id;
            _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.exitPayment
            {
                result = new exitPaymentResult
                {
                    successful = true,
                },
                id = _otterState.Api_MessageId    
            });
            _otterState.Api_MessageId = null;
        }
    }
}