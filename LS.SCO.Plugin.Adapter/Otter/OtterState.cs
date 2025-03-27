using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.SCO.Plugin.Adapter.Otter
{
    public class OtterState
    {
        public enum StatesEnum
        {
            Idle,
            Sale,
            Payment
        }

        public OtterState()
        {
            Reset();
        }

        public StatesEnum State { get; set; }
        public string? Api_MessageId { get; set; }
        public string? Api_MessageId_Product { get; set; }
        public string? Api_MessageId_Operator { get; set; }
        public string? Api_MessageId_Payment { get; set; } 
        public string? Api_Active_Payment_Method { get; set; } = String.Empty;
        public bool Pos_TransactionStarted { get; set; } = false;
        public string? Pos_TransactionId { get; set; } = null;
        public string? Pos_LastTransactionId { get; set; } = null;
        public decimal? Pos_BalanceAmount { get; set; } = null;
        public decimal? Pos_TotalAmount { get; set; } = null;
        public void Reset()
        {
            State = StatesEnum.Idle;
            Api_MessageId = Api_MessageId_Product = Api_MessageId_Operator = Api_MessageId_Payment = null;
            Pos_TransactionId = null;
            Pos_TransactionStarted = false;
            Api_Active_Payment_Method = "";
        }
    }
}
