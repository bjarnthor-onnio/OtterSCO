using Onnio.PaymentService.Models;
using Onnio.PaymentService.Models.Netgiro;

namespace Onnio.PaymentService.Interfaces
{
    public interface INetgiroPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto request);
        PaymentResultDto ProcessPayment(int amount, string receiptId, string ssn);
    }
}