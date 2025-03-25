using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Onnio.PaymentService.Models.App
{
    /// <summary>
    /// Represents a payment request in the self-checkout system.
    /// </summary>
    public class AppPaymentRequestDto
    {
       
        [Required]
        public string ReceiptNo { get; set; }
       
        [Required]
        public decimal TotalAmount { get; set; }
       
        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        public string TenderTypeCode { get; set; }

        public string CustomerIdentifier { get; set; }

        public string CardNumber { get; set; }

       
    }
}