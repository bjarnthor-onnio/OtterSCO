using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.PaymentService.Interfaces;
using Onnio.PaymentService.Models;

namespace Onnio.PaymentService.Services
{
    public class OnnioPaymentService : IOnnioPaymentService
    {
        public INetgiroPaymentService NetgiroPaymentService { get; }
        public IPeiPaymentService PeiPaymentService { get; }
        public OnnioPaymentService(INetgiroPaymentService netgiroPaymentService, IPeiPaymentService peiPaymentService)
        {
            NetgiroPaymentService = netgiroPaymentService;
            PeiPaymentService = peiPaymentService;
        }

        public async Task<object> ProcessPaymentAsync(PaymentRequestDto request)
        {
            try
            {
                switch (request.paymentService)
                {
                    case PaymentServiceType.Netgiro:
                        return await NetgiroPaymentService.ProcessPaymentAsync(request);

                    case PaymentServiceType.Pei:
                        return await PeiPaymentService.ProcessPaymentAsync();

                    default:
                        return new PaymentResultDto { Message = "Payment method not supported", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new PaymentResultDto { Success = false, Message = ex.Message };
            }
        }
    }
}
