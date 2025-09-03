using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<PeiPaymentService> _logger;
        public PeiPaymentService(IHttpClientFactory httpClientFactory, IConfigurationService configurationService, ILogger<PeiPaymentService> logger)
        {
            _configurationService = configurationService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
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
                CardNumber = request.CustomerId,
                ExtraInformation = ""
               
            };

            var json = JsonConvert.SerializeObject(peiPaymentRequest, jsonSettings);
            // Call the App payment service
            var (success, stringResponse) = MakeApiCallAsync(json, "/SCOPaymentRequests").Result;

            _logger.LogInformation("SCOPayment returned from BC {paymentResponse}", stringResponse);

            // Parse the response
            PaymentResultDto? result = MapResponse(JsonConvert.DeserializeObject<AppPaymentResultDto>(stringResponse, jsonSettings));
            if (!success)
            {
                result = new PaymentResultDto
                {
                    Success = false,
                    Message = "An error occurred handling Pei payment"
                };
            }
            

            return result;
        }
        private async Task<Tuple<bool, string>> MakeApiCallAsync(string json, string endpoint)
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
            return new Tuple<bool, string>(response.IsSuccessStatusCode, responseContent);


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
