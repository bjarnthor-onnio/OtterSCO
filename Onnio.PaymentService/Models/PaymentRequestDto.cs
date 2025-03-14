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
        

    }
}