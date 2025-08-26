namespace Onnio.PaymentService.Models.Netgiro
{
    public class ConfirmCartRequest
    {
        public string TransactionId { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
    }
}