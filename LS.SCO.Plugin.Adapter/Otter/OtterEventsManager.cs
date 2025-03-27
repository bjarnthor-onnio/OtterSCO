using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.CurrentTransaction;
using LS.SCO.Entity.DTO.SCOService.GetItemDetails;
using LS.SCO.Entity.DTO.SCOService.StartTransaction;
using LS.SCO.Entity.Extensions;
using LS.SCO.Interfaces.Log;
using LS.SCO.Interfaces.Services.Base;
using LS.SCO.Plugin.Adapter.Adapters;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;
using LS.SCO.Plugin.Service.Interfaces;

namespace LS.SCO.Plugin.Adapter.Otter
{
    public class OtterEventsManager
    {
        protected readonly ILogManager _logService;
        private readonly ILogManager _logManager;
        protected OtterProtocolHandler _otterProtocolHandler;
        protected OtterState _otterState;

        public OtterEventsManager(ILogManager logService, ILogManager logManager, OtterState otterState, OtterProtocolHandler otterProtocolHandler)
        {
            _logService = logService;
            _logManager = logManager;
            _otterState = otterState;
            _otterProtocolHandler = otterProtocolHandler;

        }

        public void sendTotals(decimal balanceAmount, decimal totalAmount)
        {
            _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.totals
            {
                @params = new totalsParams()
                {
                    leftToPay = (int)(balanceAmount * 100),
                    transactionAmount = (int)(totalAmount * 100),
                },
                id = Guid.NewGuid().ToString()
            });

            _otterState.Pos_BalanceAmount = balanceAmount;
            _otterState.Pos_TotalAmount = totalAmount;
        }

        public void sendTransactionFinish()
        {
            _otterProtocolHandler.SendMessage(new Otter.Models.FromPOS.transactionFinish
            {
                @params = new transactionFinishParams
                {
                    transactionId = _otterState.Pos_TransactionId,
                    askForReceipt = false

                },
                id = Guid.NewGuid().ToString()
            });
            _otterState.Pos_LastTransactionId = _otterState.Pos_TransactionId;
            _otterState.Reset();
        }
        public void sendTransactionStart()
        {
            _otterProtocolHandler.SendMessage(new transactionStart
            {
                @params = new transactionStartParams
                {
                    transactionId = _otterState.Pos_TransactionId,
                },
                id = new Guid().ToString()
            });
        }

    }
}
