using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.PaymentService.Extensions
{
    public static class ScoBcConnection
    {
        const string _url = "http://localhost:7148/lscentralnavuserpass/api/Onnio/PTSCO/v1.0/companies(41c550b2-8cf3-ef11-ba8e-005056aed32f)";
        const string _appKey = "WebServices";
        const string _password = "SuperSafePassword1!";

        public static ConnectionInfo GetBcConnection()
        {
            return new ConnectionInfo
            {
                Url = _url,
                AppKey = _appKey,
                Password = _password
            };
        }
    }
    public class ConnectionInfo
    {
        public string Url { get; set; }
        public string AppKey { get; set; }
        public string Password { get; set; }
    }
}

