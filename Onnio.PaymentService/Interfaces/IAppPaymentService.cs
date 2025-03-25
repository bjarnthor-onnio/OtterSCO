using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.PaymentService.Models;

namespace Onnio.PaymentService.Interfaces
{
    public interface IAppPaymentService
    {
        Task<PaymentResultDto> ProcessAppPaymentAsync(PaymentRequestDto request);
        PaymentResultDto ProcessAppPayment(PaymentRequestDto request);
        Task<bool> TriggerAppPaymentStateChangeAsync(string receiptId);
    }
}
