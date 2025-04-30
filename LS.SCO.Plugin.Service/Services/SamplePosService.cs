using AutoMapper;
using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Interfaces.Cache;
using LS.SCO.Interfaces.ErrorHandling;
using LS.SCO.Interfaces.Log;
using LS.SCO.Plugin.Service.Interfaces;
using LS.SCO.Services.Implementations.Base;
using Onnio.PaymentService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Onnio.PaymentService;
using Onnio.PaymentService.Models;

using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.HardwareStation.Output;
using LS.SCO.Entity.DTO.HardwareStation;


namespace LS.SCO.Plugin.Service.Services
{
    /// <summary>
    /// Sample service for demonstrating the implementation of a custom POS service
    /// </summary>
    public class SamplePosService : BaseSCOService<ISamplePosFacade, ISampleValidationService>, ISamplePosServiceDisabled, ISamplePosService
    {
        private readonly IOnnioPaymentService _onnioPaymentService;
        /// <summary>
        /// The services will be injected by the DI container at runtime
        /// </summary>
        /// <param name="logService"></param>
        /// <param name="errorManager"></param>
        /// <param name="mapper"></param>
        /// <param name="posFacade"></param>
        /// <param name="cacheService"></param>
        /// <param name="validationService"></param>
        public SamplePosService(ILogManager logService,
            IErrorManager errorManager,
            IMapper mapper,
            ISamplePosFacade posFacade,
            ISCOCacheService cacheService,
            IOnnioPaymentService onnioPaymentService,
            ISampleValidationService validationService) : base(logService, errorManager, mapper, posFacade, cacheService, validationService)
        {
            _onnioPaymentService = onnioPaymentService;
        }

        protected override async Task<bool> AskCustomerForPrintingReceipt(FinishTransactionInputDto input)
        {
            return true;
        }
        protected override Task ModifyPaymentResult(EFTRequestInputDto input, EFTRequestOutputDto purchaseResult, string tenderType)
        {
            return base.ModifyPaymentResult(input, purchaseResult, tenderType);
        }

        public PaymentResultDto ProcessExternalPayment(PaymentRequestDto request)
        {
            
            var response  = _onnioPaymentService.ProcessPaymentAsync(request).Result as PaymentResultDto;
            return response;
        }

        public bool TriggerLobicoEvent(string receiptId)
        {
            return _onnioPaymentService.AppPaymentService.TriggerAppPaymentStateChangeAsync(receiptId).Result;
            
        }
    }
}