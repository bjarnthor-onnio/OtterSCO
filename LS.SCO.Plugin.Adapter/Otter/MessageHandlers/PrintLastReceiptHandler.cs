using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using Microsoft.VisualBasic;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class PrintLastReceiptHandler : MessageHandler
    {
        public PrintLastReceiptHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter) 
            : base(otterState, otterProtocolHandler, manager, samplePosAdapter){}

        public override void Handle(object message)
        {
            var msg = (Otter.Models.FromSCO.printLastReceipt)message;
            _adapter.PrintPreviousTrans(_otterState.Pos_LastTransactionId);
            _otterState.Pos_LastTransactionId = null;

            _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.printLastReceipt
            {
                result = new printLastReceiptResult
                {
                    message = "",
                    successful = true
                },
                id = msg.id
            });
        }
    }
}