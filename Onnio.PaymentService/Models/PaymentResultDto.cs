namespace Onnio.PaymentService.Models
{
    public class PaymentResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = String.Empty;
        public string PaymentId { get; set; } = String.Empty;
        public string PaymentAuthorization { get; set; } = String.Empty;
    }
}