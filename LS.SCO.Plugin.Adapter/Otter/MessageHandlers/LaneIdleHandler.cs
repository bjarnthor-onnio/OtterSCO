using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.SCO.Entity.DTO.SCOService.CurrentTransaction;
using LS.SCO.Entity.Extensions;
using LS.SCO.Entity.Model.Toshiba.POS;
using LS.SCO.Interfaces.Services.NCR;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Otter.Extensions;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using LS.SCO.Plugin.Service.Interfaces;
using LS.SCO.Plugin.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LS.SCO.Plugin.Adapter.Otter.MessageHandlers
{
    public class LaneIdleHandler : MessageHandler
    {
        public LaneIdleHandler(OtterState otterState, OtterProtocolHandler otterProtocolHandler, OtterEventsManager manager,SamplePosAdapter samplePosAdapter)
            : base(otterState, otterProtocolHandler, manager, samplePosAdapter) { }

        public override async void Handle(object message)
        {
            var _currTransaction = await _adapter.GetCurrentTransaction();
            if (_currTransaction.IsValid() && _currTransaction.Transaction.NotVoidedAndExists())
            {
                _otterState.Pos_TransactionId = _currTransaction.Transaction.ReceiptId;
                if (_currTransaction.Transaction.NoOfItemLines > 0)
                {
                    var result = ProductHelper.PopulateFullBasket(_currTransaction);
                    _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.fullBasket
                    {
                        @params = result

                    });
                    _otterEventsManager.sendTotals(_currTransaction.Transaction.BalanceAmountWithTax, _currTransaction.Transaction.NetAmountWithTax);
                    _otterState.Api_MessageId = null;
                }
            }
            else
            {
                var trans = _adapter.StartTransactionAsync();
                if (trans != null)
                {
                    _otterState.Pos_TransactionId = trans.Result.Transaction.ReceiptId;
                   
                }
            }
        }
    }
}
