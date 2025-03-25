using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Onnio.PaymentService.Models.App
{
    public class SCOPaymentStateTriggerDto
    {
        [JsonProperty("@odata.context")]
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonProperty("@odata.etag")]
        [JsonPropertyName("@odata.etag")]
        public string ODataEtag { get; set; }

        [JsonProperty("ReceiptNo")]
        [JsonPropertyName("ReceiptNo")]
        public string ReceiptNo { get; set; }

        [JsonProperty("RecalculationNeeded")]
        [JsonPropertyName("RecalculationNeeded")]
        public bool RecalculationNeeded { get; set; }
    }
}
