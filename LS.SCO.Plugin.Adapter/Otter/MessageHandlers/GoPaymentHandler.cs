using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Entity.DTO.SCOService.CurrentTransaction;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Extensions;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using LS.SCO.WfcServices.Model.Common;
using Onnio.PaymentService.Services;
using WSSCOGetCurrentTransaction;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    internal class GoPaymentHandler : MessageHandler
    {
        public GoPaymentHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager, SamplePosAdapter samplePosAdapter) 
            : base(otterState, otterProtocolHandler, manager, samplePosAdapter){}

        public override void Handle(object message)
        {

            GetCurrentTransactionOutputDto currentTransaction = null;
            
            var msg = (Otter.Models.FromSCO.goPayment)message;
            _otterState.Api_MessageId = msg.id;
            
            bool recalcNeeded = _adapter.LobicoTrigger(_otterState.Pos_TransactionId);

            if (recalcNeeded)
            {
                var calculatedBasket = _adapter.CalculateTotals().Result;

                //var discountItems = currentTransaction.Transaction.SaleItems.Where(x => (x.HasLineDiscount || x.HasPeriodicDiscount) );
                var discountItems = calculatedBasket.Transaction.SaleItems.Where(x => (x.HasLineDiscount || x.HasPeriodicDiscount || x.PriceReductions.Count > 0));
                
                foreach (var item in discountItems)
                {
                    var addProduct = ProductHelper.PopulateAddProduct(item); 

                    addProduct.id = new Guid().ToString();
                    _otterProtocolHandler.SendMessage(addProduct);

                }

                _otterEventsManager.sendTotals(calculatedBasket.Transaction.BalanceAmountWithTax, calculatedBasket.Transaction.NetAmountWithTax);

            }
            

            _otterProtocolHandler.SendMessage(new goPayment
            {
                id = _otterState.Api_MessageId,
                result = new goPaymentResult
                {
                    successful = true,

                }
            });
            _otterState.Api_MessageId = null;
            
            
        }
    }
}