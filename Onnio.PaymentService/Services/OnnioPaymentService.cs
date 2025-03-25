using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Onnio.PaymentService.Interfaces;
using Onnio.PaymentService.Models;

namespace Onnio.PaymentService.Services
{
    public class OnnioPaymentService : IOnnioPaymentService
    {
        public IAppPaymentService AppPaymentService { get; }
        public INetgiroPaymentService NetgiroPaymentService { get; }
        public IPeiPaymentService PeiPaymentService { get; }
        public ILeikbreytirPaymentService LeikbreytirPaymentService { get; }

        public OnnioPaymentService(INetgiroPaymentService netgiroPaymentService, 
            IPeiPaymentService peiPaymentService, 
            IAppPaymentService appPaymentService, 
            ILeikbreytirPaymentService leikbreytirPaymentService)
        {
            NetgiroPaymentService = netgiroPaymentService;
            PeiPaymentService = peiPaymentService;
            AppPaymentService = appPaymentService;
            LeikbreytirPaymentService = leikbreytirPaymentService;
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
                    case PaymentServiceType.App:
                        return await AppPaymentService.ProcessAppPaymentAsync(request);
                    case PaymentServiceType.Leikbreytir:
                        return await LeikbreytirPaymentService.ProcessLeikbreytirPaymentAsync(request);

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
