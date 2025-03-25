using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.ConfigService.Interface;
using Onnio.PaymentService.Models.Netgiro;

namespace Onnio.PaymentService.Extensions
{
    public class NetgiroConnectionInfo
    {
        private readonly IConfigurationService _service;
        const string BaseUrl = "https://api.test.netgiro.is/v1";
        const string AppKey = "881E674F-7891-4C20-AFD8-56FE2624C4B5";
        const string Secret = "YCFd6hiA8lUjZejVcIf/LhRXO4wTDxY0JhOXvQZwnMSiNynSxmNIMjMf1HHwdV6cMN48NX3ZipA9q9hLPb9C1ZIzMH5dvELPAHceiu7LbZzmIAGeOf/OUaDrk2Zq2dbGacIAzU6yyk4KmOXRaSLi8KW8t3krdQSX7Ecm8Qunc/A=";

        public NetgiroConnectionInfo(IConfigurationService service)
        {
            _service = service;
        }
        public static string CalculateSignature(params string[] args)
        {
            string input = string.Join("", args);
            var sha = System.Security.Cryptography.SHA256.Create();
            var hashArray = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            string calculatedSignature = string.Empty;

            foreach (byte b in hashArray)
            {
                calculatedSignature += b.ToString("x2");
            }

            return calculatedSignature;

        }
        public NetgiroParameters GetParameters()
        {
            var config = _service.GetConfigurationAsync<NetgiroParameters>("Config", "NetgiroConfig").Result;
            return new NetgiroParameters {
               BaseUrl = BaseUrl,
               AppKey = AppKey,
               Secret = Secret
           };
        }

    }
    
}
