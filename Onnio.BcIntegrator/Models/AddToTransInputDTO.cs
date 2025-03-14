using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.BcIntegrator.Models
{
    public class AddToTransInputDTO
    {
        public string barcode { get; set; }
        public string terminalId { get; set; }
        public string receiptId { get; set; }
        public string token { get; set; }
        public string storeId { get; set; }
        public string staffId { get; set; }


    }
}
