using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.PaymentService.Models.Netgiro
{
    public class NetgiroParameters
    {
        public string BaseUrl { get; set; }
        public string AppKey { get; set; }
        public string Secret { get; set; }
    }
}
