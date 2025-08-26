using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Onnio.PaymentService.Models
{
    public class CancellationRequestDto
    {
        [JsonIgnore]
        public PaymentServiceType paymentService { get; set; }
        public string TransactionId { get; set; } = string.Empty;
    }
}
