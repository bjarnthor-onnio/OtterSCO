using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Onnio.ConfigService.Interface;
using Onnio.ConfigService.Models;
using Onnio.PaymentService.Extensions;
using Onnio.PaymentService.Interfaces;
using Onnio.PaymentService.Models;
using Onnio.PaymentService.Models.App;
using static System.Net.WebRequestMethods;

namespace Onnio.PaymentService.Services
{
    public class AppPaymentService : IAppPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationService _configurationService;
        public AppPaymentService(IHttpClientFactory httpClientFactory, IConfigurationService configService)
        {
            _httpClientFactory = httpClientFactory;
            _configurationService = configService;
        }
        public async Task<bool> TriggerAppPaymentStateChangeAsync(string receiptId)
        {
            try
            {
                var stateChangeRequest = new AppStateChangeRequest
                {
                    ReceiptNo = receiptId,
                };

                var json = JsonConvert.SerializeObject(stateChangeRequest, jsonSettings);
                
                var response = await MakeApiCallAsync(json, "/SCOPaymentStateTriggers");
                SCOPaymentStateTriggerDto? result = JsonConvert.DeserializeObject<SCOPaymentStateTriggerDto>(response, jsonSettings);

                return result?.RecalculationNeeded ?? false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<PaymentResultDto> ProcessAppPaymentAsync(PaymentRequestDto request)
        {
            var appPaymentRequest = new AppPaymentRequestDto
            {
                ReceiptNo = request.ReceiptId,
                TotalAmount = request.Amount,
                TenderTypeCode = request.TenderTypeId,
                CurrencyCode = request.CurrencyCode ?? "ISK",
                CardNumber = request.CustomerId,
            };

            var json = JsonConvert.SerializeObject(appPaymentRequest, jsonSettings);
            // Call the App payment service
            var response = MakeApiCallAsync(json, "/SCOPaymentRequests").Result;
            
            // Parse the response
            PaymentResultDto? result = MapResponse(JsonConvert.DeserializeObject<AppPaymentResultDto>(response, jsonSettings));

            return result;
        }
        public PaymentResultDto ProcessAppPayment(PaymentRequestDto request)
        {
            return new PaymentResultDto { Message = "Method not implemented", Success = false };
        }
        private async Task<string> MakeApiCallAsync(string json, string endpoint)
        {

            // Make an API call to the App payment service
            var client = _httpClientFactory.CreateClient("AppPaymentService");
            var connection = _configurationService.GetConfigurationAsync<BcConfig>("Config","BcConfig").Result;

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
