using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.ConfigService.Models
{
    public class OtterConfig
    {
        public string PosId { get; set; } = string.Empty;
        public bool AskForReceipt { get; set; } = true;
        public bool ForceReceiptPrinting { get; set; } = true;
    }
}
