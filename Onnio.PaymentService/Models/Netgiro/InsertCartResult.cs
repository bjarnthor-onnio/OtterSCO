using System.Transactions;

namespace Onnio.PaymentService.Models.Netgiro
{
    public class InsertCartResult
    {
        public string Signature { get; set; } = "";
        public bool IsValidSignature { get; set; } = false;
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public int ResultCode { get; set; } = 0;
        public string TransactionId { get; set; } = "";
        public bool RemoteConfirmationAvailable { get; set; } = false;
        public string AnnouncementMessage { get; set; } = "";
        public int ProcessCartCheckIntervalMiliseconds { get; set; } = 0;




    }
}