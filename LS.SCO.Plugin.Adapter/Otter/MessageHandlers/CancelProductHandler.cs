using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.VoidItem;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class CancelProductHandler : MessageHandler
    {
        public CancelProductHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter) : base(otterState, otterProtocolHandler, manager, samplePosAdapter)
        {
        }

        public async override void Handle(object message)
        {
            var msg = (Otter.Models.FromSCO.cancelProduct)message;
            _otterState.Api_MessageId = msg.id;

            var cancelProduct = await Task.Run(() => _adapter.VoidItemAsync(msg.@params.barcode,msg.@params.productId.ToString()));
            if (cancelProduct.ErrorList?.Count() > 0)
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.cancelProduct
                {
                    result = new cancelProductResult
                    {
                        message = "Failed to Void Item at LS Central",
                        successful = false
                    },
                    id = _otterState.Api_MessageId
                });
                _otterState.Api_MessageId = null;
            }
            else
            {
                _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.cancelProduct
                {
                    result = new cancelProductResult
                    {
                        successful = true,
                    },
                    id = _otterState.Api_MessageId
                });
                _otterState.Api_MessageId = null;
                _otterEventsManager.sendTotals(cancelProduct.Transaction.BalanceAmountWithTax, cancelProduct.Transaction.NetAmountWithTax);
            }
        }
    }
}