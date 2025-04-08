using Onnio.PaymentService.Models;
using Onnio.PaymentService.Models.Pei;

namespace Onnio.PaymentService.Interfaces
{
    public interface IPeiPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto request);
        PeiPaymentResult ProcessPayment();
    }
}