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
using Onnio.BcIntegrator;
using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using Onnio.BcIntegrator.Models;

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

        public PaymentResultDto ProcessExternalPayment(PaymentRequestDto request)
        {
            var response  = _onnioPaymentService.ProcessPaymentAsync(request).Result as PaymentResultDto;
            return response;

        }
        /*public override async Task<AddToTransOutputDto> AddItemAsync(AddToTransInputDto input)
        {
            AddToTransInputDTO addToTransInputDTO = new AddToTransInputDTO();
            addToTransInputDTO.terminalId = input.TerminalId;
            addToTransInputDTO.storeId = input.StoreId;
            addToTransInputDTO.staffId = input.StaffId;
            addToTransInputDTO.token = input.Token;
            addToTransInputDTO.barcode = input.Data.Id;
            addToTransInputDTO.receiptId = input.Data.TransactionId;
            Transaction transaction = new Transaction();
            transaction.AddToTransaction(addToTransInputDTO);
            return new AddToTransOutputDto();
        }*/
    }
}