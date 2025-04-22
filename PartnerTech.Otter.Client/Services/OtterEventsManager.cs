

using PartnerTech.Otter.Client.Models.ResponseModels;

namespace PartnerTech.Otter.Client.Services
{
    public class OtterEventsManager
    {
        
        protected OtterService _otterService;
        protected OtterState _otterState;

        public OtterEventsManager(OtterState otterState, OtterService otterService)
        {
            _otterState = otterState;
            _otterService = otterService;
        }

        public void sendTotals(decimal balanceAmount, decimal totalAmount)
        {
            _otterService.SendMessage(new Models.ResponseModels.totals
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
            _otterService.SendMessage(new Models.ResponseModels.transactionFinish
            {
                @params = new transactionFinishParams
                {
                    transactionId = _otterState.Pos_TransactionId,
                    askForReceipt = true

                },
                id = Guid.NewGuid().ToString()
            });
            _otterState.Pos_LastTransactionId = _otterState.Pos_TransactionId;
            _otterState.Reset();
        }
        public void sendTransactionStart()
        {
            _otterService.SendMessage(new transactionStart
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
