using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Onnio.PaymentService.Extensions;
using Onnio.PaymentService.Interfaces;
using Onnio.PaymentService.Models;
using Onnio.PaymentService.Models.App;


namespace Onnio.PaymentService.Services
{
    internal class LeikbreytirPaymentService : ILeikbreytirPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public LeikbreytirPaymentService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PaymentResultDto> ProcessLeikbreytirPaymentAsync(PaymentRequestDto request)
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
            var response = await MakeApiCallAsync(json, "/SCOPaymentRequests");
            // Parse the response
            var result = MapResponse(JsonConvert.DeserializeObject<AppPaymentResultDto>(response, jsonSettings));
            
            return result;
        }

        public AppPaymentResultDto ProcessLeikbreitirPayment(PaymentResultDto request)
        {
            // Call the Leikbreytir payment service
            return new AppPaymentResultDto { Authorized = false };
        }

        private async Task<string> MakeApiCallAsync(string json, string endpoint)
        {
            var client = _httpClientFactory.CreateClient("AppPaymentService");
            var connection = ScoBcConnection.GetBcConnection();
            var url = $"{connection.Url}/{endpoint}";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{connection.AppKey}:{connection.Password}"));
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
