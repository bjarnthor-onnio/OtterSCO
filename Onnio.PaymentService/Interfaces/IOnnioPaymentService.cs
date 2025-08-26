using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.PaymentService.Models;

namespace Onnio.PaymentService.Interfaces
{
    public interface IOnnioPaymentService
    {
        public INetgiroPaymentService NetgiroPaymentService { get; }
        public IPeiPaymentService PeiPaymentService { get; }
        public IAppPaymentService AppPaymentService { get; }
        public ILeikbreytirPaymentService LeikbreytirPaymentService { get; }

        Task<object> ProcessPaymentAsync(PaymentRequestDto request);
        Task<object> ProcessCancellationAsync(CancellationRequestDto request);
    }
}
