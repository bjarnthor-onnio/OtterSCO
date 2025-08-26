using Onnio.PaymentService.Models;
using Onnio.PaymentService.Models.Netgiro;

namespace Onnio.PaymentService.Interfaces
{
    public interface INetgiroPaymentService
    {
        Task<PaymentResultDto>ProcessCartAsync(PaymentRequestDto request);
        Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto request);
        Task<PaymentResultDto> ConfirmCartAsync(string transactionId, string confirmationCode);
        Task<PaymentResultDto> CancelCartAsync(string transactionId);
    }
}