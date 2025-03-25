using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.PaymentService.Models.App
{
    public class AppPaymentResultDto
    {
        public string CardNumber { get; set; }
        public string AuthorizationStatus { get; set; } = "";
        public bool Authorized { get; set; } = false;
        public string AuthorizationMessage { get; set; } = "";
        public AppError? Error { get; set; } = null;
    }
    public class AppError
    {
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
