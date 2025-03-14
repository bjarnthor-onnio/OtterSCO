using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class GoPaymentHandler : MessageHandler
    {
        public GoPaymentHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter) : base(otterState, otterProtocolHandler, manager, samplePosAdapter)
        {
        }

        public async override void Handle(object message)
        {
            var msg = (Otter.Models.FromSCO.goPayment)message;
            _otterProtocolHandler.SendMessage(new goPayment
            {
                id = msg.id,
                result = new goPaymentResult
                {
                    successful = true
                }
            });


        }
    }
}