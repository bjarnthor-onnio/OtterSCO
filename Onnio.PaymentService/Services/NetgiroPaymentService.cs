using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Onnio.ConfigService.Interface;
using Onnio.PaymentService.Extensions;
using Onnio.PaymentService.Interfaces;
using Onnio.PaymentService.Models;
using Onnio.PaymentService.Models.Netgiro;

namespace Onnio.PaymentService.Services
{
    public class NetgiroPaymentService : INetgiroPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationService _configurationService;
        public NetgiroPaymentService(IHttpClientFactory httpClientFactory, IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _httpClientFactory = httpClientFactory;
        }

        public PaymentResultDto ProcessPayment(int amount, string receiptId, string ssn)
        {
            return new PaymentResultDto();
        }

        public async Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto paymentRequest)
        {
            InsertCartRequest request = new InsertCartRequest
            {
                Amount = paymentRequest.Amount,
                Reference = paymentRequest.Reference,
                CustomerId = paymentRequest.CustomerId,
                ConfirmationType = 0,
                Description = ""
            };

            //Call the Netgiro API
            var response = await MakeApiCallAsync("InsertCart", request);
            
            //Parse the response
            InsertCartResult? result = JsonConvert.DeserializeObject<InsertCartResult>(response.Content.ReadAsStringAsync().Result, jsonSettings);

            if (!result.Success || !response.IsSuccessStatusCode)
            {
                return new PaymentResultDto { Success = false, Message = result.Message };
            }
            
            int RetryInterval = result.ProcessCartCheckIntervalMiliseconds;
            var checkCartRequest = new CheckCartRequest { TransactionId = result.TransactionId };
            Stopwatch stopwatch = new Stopwatch();
            TimeSpan timeLimit = TimeSpan.FromMinutes(2);

            do
            {
                stopwatch.Start();
                CheckCartResult? checkCartResult = null;
                //Call the Netgiro API
                response = await MakeApiCallAsync("CheckCart", checkCartRequest);
                checkCartResult = JsonConvert.DeserializeObject<CheckCartResult>(response.Content.ReadAsStringAsync().Result, jsonSettings);

                if (checkCartResult.PaymentSuccessful)
                {
                    return new PaymentResultDto { 
                        Success = true, 
                        Message = "Payment successful",
                        PaymentAuthorization = checkCartResult.PaymentInfo.InvoiceNumber.ToString(),

                    };
                    
                }
                else
                {
                   Thread.Sleep(RetryInterval);
                }
                
            }
            while (stopwatch.Elapsed < timeLimit );
            
            stopwatch.Stop();
            var cancelCartResponse = await MakeApiCallAsync("CancelCart", result.TransactionId);
            return new PaymentResultDto { Success = false, Message = "Hætt við greiðslu, samþykki barst ekki tímanlega." };

        }


        private async Task<HttpResponseMessage> MakeApiCallAsync(string endpoint, object payload)
        {
            // Get HttpClient from factory
            var client = _httpClientFactory.CreateClient();

            var parameters = _configurationService.GetConfigurationAsync<NetgiroParameters>("Config", "NetgiroConfig").Result;
            // Construct full URL
            string url = $"{parameters.BaseUrl}/checkout/{endpoint}";

            // Create timestamp in ISO format
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            // Create signature string: ApplicationId + Timestamp + SecretKey
            string signatureString = $"{parameters.AppKey}{timestamp}{parameters.Secret}";

            string json = JsonConvert.SerializeObject(payload, jsonSettings);
           
            // Generate signature hash with SHA256
            string signature = NetgiroConnectionInfo.CalculateSignature(parameters.Secret, timestamp, url, json);

            // Set up headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("netgiro_appkey", parameters.AppKey);
            client.DefaultRequestHeaders.Add("netgiro_signature", signature);
            client.DefaultRequestHeaders.Add("netgiro_nonce", timestamp);

            // Make the API request
            var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            return response;
        }
        private static JsonSerializerSettings jsonSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
            }
        }
    }
}

