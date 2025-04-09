using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Onnio.ConfigService.Interface;
using Onnio.ConfigService.Models;
using Onnio.PaymentService.Interfaces;
using Onnio.PaymentService.Models;
using Onnio.PaymentService.Models.App;
using Onnio.PaymentService.Models.Pei;

namespace Onnio.PaymentService.Services
{
    public class PeiPaymentService : IPeiPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationService _configurationService;
        public PeiPaymentService(IHttpClientFactory httpClientFactory, IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _httpClientFactory = httpClientFactory;
        }
        public PeiPaymentResult ProcessPayment()
        {
            return new PeiPaymentResult();
        }

        public async Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto request)
        {
            var peiPaymentRequest = new PeiPaymentRequestDto
            {
                ReceiptNo = request.ReceiptId,
                TotalAmount = request.Amount,
                TenderTypeCode = request.TenderTypeId,
                CurrencyCode = request.CurrencyCode ?? "ISK",
                CardNumber = request.CustomerId
               
            };

            var json = JsonConvert.SerializeObject(peiPaymentRequest, jsonSettings);
            // Call the App payment service
            var response = MakeApiCallAsync(json, "/SCOPaymentRequests").Result;

            // Parse the response
            PaymentResultDto? result = MapResponse(JsonConvert.DeserializeObject<AppPaymentResultDto>(response, jsonSettings));

            return result;
        }
        private async Task<string> MakeApiCallAsync(string json, string endpoint)
        {

            // Make an API call to the App payment service
            var client = _httpClientFactory.CreateClient("AppPaymentService");
            var connection = _configurationService.GetConfigurationAsync<BcConfig>("Config", "BcConfig").Result;

            var url = $"{connection.OdataServiceBaseUrl}{endpoint}";
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{connection.User}:{connection.Password}"));
            request.Headers.Add("Authorization", $"Basic {authorization}");

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;


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
        private PaymentResultDto MapResponse(AppPaymentResultDto result)
        {
            return new PaymentResultDto
            {
                Message = result.AuthorizationMessage,
                Success = result.Authorized
            };
        }
    }
}
