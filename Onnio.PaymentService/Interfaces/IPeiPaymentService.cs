using Onnio.PaymentService.Models.Pei;

namespace Onnio.PaymentService.Interfaces
{
    public interface IPeiPaymentService
    {
        Task<PeiPaymentResult> ProcessPaymentAsync();
        PeiPaymentResult ProcessPayment();
    }
}