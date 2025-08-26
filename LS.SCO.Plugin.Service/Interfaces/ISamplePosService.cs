using LS.SCO.Interfaces.Services.Base;
using Onnio.PaymentService.Models;

namespace LS.SCO.Plugin.Service.Interfaces
{
    /// <summary>
    /// Sample service interface for demonstrating the implementation of a custom POS service
    /// </summary>
    public interface ISamplePosService : IBaseSCOService
    {
        public PaymentResultDto ProcessExternalPayment(PaymentRequestDto request);
        public void CancelExternalPayment(CancellationRequestDto request);
        public bool TriggerLobicoEvent(string transactionId);
    }
}