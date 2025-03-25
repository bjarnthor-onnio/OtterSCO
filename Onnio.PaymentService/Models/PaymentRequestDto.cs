using Newtonsoft.Json;

namespace Onnio.PaymentService.Models
{
    public class PaymentRequestDto
    {
        [JsonIgnore]
        public PaymentServiceType paymentService { get; set; }
        public int Amount { get; set; }
        public string Reference { get; set; } = "";
        public string CustomerId { get; set; } = "";
        public string TenderTypeId { get; set; } = "";
        public string CurrencyCode { get; set; } = "ISK";
        public string ReceiptId { get; set; }
    }
}