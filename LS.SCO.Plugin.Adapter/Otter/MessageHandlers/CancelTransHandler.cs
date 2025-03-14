using LS.SCO.Entity.DTO.SCOService.VoidTransaction;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class CancelTransHandler : MessageHandler
    {
        public CancelTransHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter) 
            : base(otterState, otterProtocolHandler, manager, samplePosAdapter){}

        public override void Handle(object message)
        {
            var voidTransaction = _adapter.VoidTransactionAsync().Result;


            if (voidTransaction.ErrorList.Count() > 0)
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.cancelTransaction
                {
                    result = new cancelTransactionResult()
                    {
                        successful = false,
                        message = "Failed to Void Transaction at LS Central"
                    },
                    id = _otterState.Api_MessageId
                });
                _otterState.Api_MessageId = null;
            }
            else
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.cancelTransaction
                {
                    result = new cancelTransactionResult()
                    {
                        successful = true,
                    },
                    id = _otterState.Api_MessageId
                });

                _otterEventsManager.sendTransactionFinish();

            }
        }
    }
}