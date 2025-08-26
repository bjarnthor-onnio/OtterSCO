using Newtonsoft.Json;

namespace Onnio.PaymentService.Models
{
    public class PaymentRequestDto
    {
        [JsonIgnore]
        public PaymentServiceType paymentService { get; set; }
        public int Amount { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string TenderTypeId { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "ISK";
        public string ReceiptId { get; set; } = string.Empty;
        public string ConfirmationCode { get; set; } = string.Empty;
    }
}