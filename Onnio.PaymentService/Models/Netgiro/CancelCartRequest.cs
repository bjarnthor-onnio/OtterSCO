using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.PaymentService.Models.Netgiro
{
    public class CancelCartRequest
    {
        public string TransactionId { get; set; } = string.Empty;
    }
}
