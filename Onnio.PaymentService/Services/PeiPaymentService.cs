using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.PaymentService.Interfaces;
using Onnio.PaymentService.Models.Pei;

namespace Onnio.PaymentService.Services
{
    public class PeiPaymentService : IPeiPaymentService
    {
        public PeiPaymentResult ProcessPayment()
        {
            return new PeiPaymentResult();
        }

        public async Task<PeiPaymentResult> ProcessPaymentAsync()
        {
            return await Task.FromResult(new PeiPaymentResult());
        }
    }
}
