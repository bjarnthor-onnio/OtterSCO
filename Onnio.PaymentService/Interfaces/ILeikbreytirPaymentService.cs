using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.PaymentService.Models;
using Onnio.PaymentService.Models.App;

namespace Onnio.PaymentService.Interfaces
{
    public interface ILeikbreytirPaymentService
    {
        Task<PaymentResultDto> ProcessLeikbreytirPaymentAsync(PaymentRequestDto request);
        AppPaymentResultDto ProcessLeikbreitirPayment(PaymentResultDto request);
    }
    
}
