using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.PaymentService.Models.Pei
{
    public class PeiPaymentRequestDto
    {
        [Required]
        public string ReceiptNo { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public string CurrencyCode { get; set; } 
        [Required]
        public string TenderTypeCode { get; set; }
        public string CustomerIdentifier { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExtraInformation { get; set; } = string.Empty;
    }
}
