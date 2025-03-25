using LS.SCO.Plugin.Adapter.Adapters;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class PrintLastReceiptHandler : ShutdownHandler
    {
        public PrintLastReceiptHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter) 
            : base(otterState, otterProtocolHandler, manager, samplePosAdapter)
        {
            _adapter.PrintPreviousTrans(_otterState.Pos_LastTransactionId);
            _otterState.Pos_LastTransactionId = null;
        }
    }
}