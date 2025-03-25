using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Onnio.PaymentService.Models.Netgiro
{
    public class NetgiroParameters
    {
        [JsonProperty("BaseUrl")]
        public string BaseUrl { get; set; } = "";

        [JsonProperty("AppKey")]
        public string AppKey { get; set; } = "";

        [JsonProperty("Secret")]
        public string Secret { get; set; } = "";
    }
}
