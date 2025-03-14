using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.PaymentService.Models.Netgiro
{
    public class InsertCartRequest
    {
        public int Amount { get; set; } = 0;
        public string Reference { get; set; } = "";
        public string CustomerId { get; set; } = "";
        public string Description { get; set; } = "";
        public string CallbackUrl { get; set; } = "";
        public string CallbackCancelUrl { get; set; } = "";
        public int ConfirmationType { get; set; } = 0;
        public string LocationId { get; set; } = "";
        public string RegisterId { get; set; } = "";
        public string ClientInfo { get; set; } = "";
        public IEnumerable<CartItemRequests>? CartItems { get; set; }

    }
    public class CartItemRequests
    {
        public string ProductNo { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Amount { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public int UnitPrice { get; set; } = 0;

    }
}
