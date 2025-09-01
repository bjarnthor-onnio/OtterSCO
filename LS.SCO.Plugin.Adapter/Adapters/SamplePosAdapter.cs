using AutoMapper;
using LS.SCO.Entity.Adapters;
using LS.SCO.Entity.DTO.DieboldNixdorf.Pos;
using LS.SCO.Entity.DTO.HardwareStation;
using LS.SCO.Entity.DTO.HardwareStation.Output;
using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.CurrentTransaction;
using LS.SCO.Entity.DTO.SCOService.GetItemDetails;
using LS.SCO.Entity.DTO.SCOService.StartTransaction;
using LS.SCO.Entity.DTO.SCOService.TerminalSettings;
using LS.SCO.Entity.Enums;
using LS.SCO.Entity.Extensions;
using LS.SCO.Helpers.Extensions;
using System.Linq;
using LS.SCO.Interfaces.Adapter;
using LS.SCO.Interfaces.Cache;
using LS.SCO.Interfaces.Log;
using LS.SCO.Interfaces.Services.Configuration;
using LS.SCO.Interfaces.Services.Validation;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;
using LS.SCO.Plugin.Adapter.Controllers.Models;
using LS.SCO.Plugin.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using GetItemDetailsOutputDto = LS.SCO.Entity.DTO.SCOService.GetItemDetails.GetItemDetailsOutputDto;
using LS.SCO.Entity.DTO.SCOService.VoidTransaction;
using LS.SCO.Entity.DTO.SCOService.CancelActiveTransaction;
using LS.SCO.Entity.DTO.SCOService.Items;
using LS.SCO.Entity.DTO.SCOService.StaffLogon;
using LS.SCO.Entity.DTO.SCOService.VoidItem;
using Onnio.PaymentService.Models;
using LS.SCO.Entity.ErrorManagement;
using LS.SCO.Entity.Base;
using Onnio.PaymentService.Services;
using Onnio.ConfigService.Interface;
using LS.SCO.Entity.DTO.SCOService.PrintPreviousTransaction;
using LS.SCO.Entity.DTO.SCOService.CalculateBasket;
using LS.SCO.Plugin.Adapter.Interfaces;
using Onnio.ConfigService.Models;

namespace LS.SCO.Plugin.Adapter.Adapters
{
    /// <summary>
    /// This adapter class is responsible for creating and handling a connection to the SCO Device.
    /// </summary>
    public partial class SamplePosAdapter : ISamplePosAdapter
    {
        #region Infra

        protected readonly ILogManager _logService;
        protected ISamplePosServiceDisabled _posService;
        private readonly IAdapterConfigurationManager _adapterConfigurationManager;
        protected CancellationTokenSource _cancellationTokenSource;
        protected readonly IAdapterValidationService _validationService;
        protected readonly ISCOCacheService _cacheService;
        private readonly IServiceProvider _services;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationService _configService;

        public BaseAdapterConfiguration AdapterConfiguration { get; set; }

        public string TerminalId { get => AdapterConfiguration.TerminalId; }
        public IServiceScope Scope { get; set; }

        /// <summary>
        /// Default parameter constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="logService"></param>
        /// <param name="posService"></param>
        /// <param name="validationService"></param>
        /// <param name="cacheService"></param>
        public SamplePosAdapter(
            ILogManager logService,
            IAdapterConfigurationManager adapterConfigurationManager,
            IAdapterValidationService validationService,
            ISCOCacheService cacheService,
            IMapper mapper,
            IServiceProvider services,
            IHttpClientFactory httpClientFactory,
            IConfigurationService configService)
        {
            this._logService = logService;
            this._services = services;
            this._posService = (ISamplePosServiceDisabled)this._services.GetService<ISamplePosService>();
            this._adapterConfigurationManager = adapterConfigurationManager;
            this._cancellationTokenSource = new CancellationTokenSource();
            this._validationService = validationService;
            this._cacheService = cacheService;
            _mapper = mapper;
            //Onnio addons
            this._httpClientFactory = httpClientFactory;
            _configService = configService;
        }

        /// <summary>
        /// Used to configure multiple terminals connected to the system.
        /// </summary>
        /// <param name="position">Position of the Adapters array in case of searching for adapter configurations in separate confoguration providers.</param>
        public void SetUpServices(int position = 0)
        {
            if (this._adapterConfigurationManager != null)
            {
                this.AdapterConfiguration = this._adapterConfigurationManager.Configuration[position];
            }
        }

        /// <summary>
        /// Loads the installed devices from the configuration file.
        /// </summary>
        

        /// <summary>
        /// Starts the operation of the connected device.
        /// </summary>
        public void StartMonitoringAsync(CancellationToken cancellationToken)
        {
            this._posService = this._services.GetService<ISamplePosService>() as ISamplePosServiceDisabled;
            ConnectoToOtterSCO();
        }

        /// <summary>
        /// Requests the terminal's reboot
        /// </summary>
        public void StopMonitoringAsync()
        {
            this._posService = this._services.GetService<ISamplePosServiceDisabled>();
        }

        /// <summary>
        /// Disposes any unecessary resources.
        /// </summary>
        public void Dispose()
        {

        }

        #endregion Infra

        /// <summary>
        /// Asks LS Central to start a new transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<StartTransactionOutputDto> StartTransactionAsync()
        {
            _logService.MethodStart();

            var input = new StartTransactionInputDto();

            input.ConfigureBaseInputProperties(this);

            var startTransactionResult = await _posService.StartTransactionAsync(input);

            if (startTransactionResult.IsValid())
                _logService.LogInfo(message: $"Success, TransactionId : {startTransactionResult.Transaction?.TransactionId}");

            else
                _logService.LogError(errorMessages: startTransactionResult.ErrorList);

            return startTransactionResult;
        }

        /// <summary>
        /// Requests the item details from LS Central.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<GetItemDetailsOutputDto> GetItemDetailAsync(GetItemDetailsInputDto item)
        {
            var input = new GetItemDetailsInputDto { ItemCode = item.ItemCode };

            input.ConfigureBaseInputProperties(this);

            var result = await this._posService.GetItemDetailAsync(input);

            return result;
        }
        public async Task<AddToTransOutputDto> AddToTransaction(AddToTransInputDto input)
        {
            var result =  await _posService.AddToTransactionAsync(input);
            return result;
            
        }
        /// <summary>
        /// Adds an item to the current transaction.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<AddToTransOutputDto> AddItemToTransaction(string barCode, string itemId, string receiptId = "", decimal qty = 0, bool isCoupon = false)
        {
            var input = new AddToTransInputDto { Data = new AddToTransInputDataDto { } };
            input.Data.Code = barCode;
            input.Data.TransactionId = receiptId;
            input.Data.Id = itemId;
            input.Data.Quantity = qty;

            input.ConfigureBaseInputProperties(this);
            AddToTransOutputDto result = null;
            if(isCoupon)
            {
                input.Data.EntryType = 6;
                result = await _posService.AddToTransactionAsync(input);
                
            }
            else
            {
                result = await this._posService.AddItemAsync(input);
            }

            if (result != null && result.Result == "IFC_OK")
            {
                _logService.MethodEndsWithSuccess();
            }

            return result;
        }

        /// <summary>
        /// Requests the current transaction from LS Central.
        /// </summary>
        /// <returns></returns>
        public async Task<GetCurrentTransactionOutputDto> GetCurrentTransaction()
        {
            var input = new GetCurrentTransactionInputDto();

            input.ConfigureBaseInputProperties(this);

            var result = await this._posService.GetCurrentTransactionAsync(input);

            if (result.IsValid() && result.Transaction != null)
            {
                _logService.MethodEndsWithSuccess();
            }
            else
            {
                _logService.MethodEndsWithError(errorMessages: result.ErrorList);
            }

            return result;
        }
        public async Task<AddToTransOutputDto> PayForCurrentTransactionExternal(string tenderType, decimal? amount, string customerId = "", bool skipPaymentLine = true, string confirmationCode = "")
        {
            //Dont need this here but we need to get the current transaction to get the receiptId, have to figure out a better way to do this
            GetCurrentTransactionOutputDto currentTransaction = await GetCurrentTransaction();


            PaymentRequestDto request = new PaymentRequestDto
            {
                CurrencyCode = "ISK",
                Amount = Convert.ToInt32(amount),
                Reference = "Vörur",
                CustomerId = customerId,
                ReceiptId = currentTransaction.Transaction.ReceiptId,
                TenderTypeId = tenderType

            };

            //TODO: handle different payment services better
            //Maybe create a swithc case for the different payment services
            
            if (tenderType == "23")
            {
                request.paymentService = PaymentServiceType.App;
            }
            if (tenderType == "40")
            {
                request.paymentService = PaymentServiceType.Leikbreytir;
            }
            if(tenderType == "18"|| tenderType.ToLower().Contains("netgiro"))
            {
                request.paymentService = PaymentServiceType.Netgiro;
                request.ConfirmationCode = confirmationCode;
                request.CustomerId = customerId;
            }
            if(tenderType == "22")
            {
                request.paymentService = PaymentServiceType.Pei;
            }

            var result = _posService.ProcessExternalPayment(request);


            AddToTransOutputDto output = new AddToTransOutputDto();
            output.ConfigureBaseInputProperties(this);

           
            if (!result.Success)
            {
                Error error = new Error();
                error.ErrorMessage = result.Message;
                List<Error> errorEnumerable = new List<Error>();
                errorEnumerable.Add(error);

                output.ErrorList = errorEnumerable;
                
                if (result.ConfirmationNeeded)
                {
                    _otterState.External_PaymentTransactionId = result.PaymentReference;
                    _otterState.External_PaymentAuthenticationType = (int)PaymentAuthenticationType.Code;
                    
                }

                return output;

            }


            if (!skipPaymentLine)
            {
                var input = new EFTRequestInputDto(EFTRequestType.Purchase)
                {
                    AmountBreakdown = new Entity.Model.HardwareStation.AmountBreakdown
                    {
                        TotalAmount = Convert.ToInt32(amount)
                    },
                    TenderType = tenderType
                };
                input.ConfigureBaseInputProperties(this);

                var addTenderOutput = await AddPaymentLineToTransaction(tenderType, input);

                if (!addTenderOutput.IsValid())
                {
                    output.ErrorList = output.ErrorList.Concat(addTenderOutput.ErrorList).ToList();
                    return output;
                }

                return addTenderOutput;

            }
            output.Success = true;
            return output;

        }
        public async void CancelExternalPayment(string tenderType)
        {
            if (!string.IsNullOrEmpty(_otterState.External_PaymentTransactionId))
            {

                CancellationRequestDto request = new CancellationRequestDto();
                if (tenderType == "18" || tenderType.ToLower().Contains("netgiro"))
                {
                    request.paymentService = PaymentServiceType.Netgiro;
                    request.TransactionId = _otterState.External_PaymentTransactionId;
                }

                _posService.CancelExternalPayment(request);
                _otterState.External_PaymentTransactionId = string.Empty;
            }
        }

        /// <summary>
        /// Pays for current transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseOutputEntity> PayForCurrentTransaction(decimal amount, string tenderType)
        {
               /* var serviceInput = new EFTRequestInputDto(EFTRequestType.Purchase)
                {
                    AmountBreakdown = new Entity.Model.HardwareStation.AmountBreakdown
                    {
                        TotalAmount = Convert.ToDecimal(amount),
                    },
                    TenderType = tenderType
                };
                serviceInput.ManualEntry = true;
                serviceInput.ConfigureBaseInputProperties(this);

                var result = await _posService.CallPaymentService(serviceInput);
            
                _logService.WebserviceCallSuccessful(webservice: "CallPaymentService");

                if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                {
                    _logService.MethodEndsWithSuccess();
                    return new BaseOutputEntity();
                }

                _logService.MethodEndsWithError(errorMessages: result.Message);

                return new BaseOutputEntity
                {
                    ErrorList = new List<Error> {
                        new Error
                        {
                            ErrorMessage = result.Message
                        }
                    }
                };*/
            EFTRequestOutputDto sessionResult = null;
            try
            {

                var input = new EFTRequestInputDto(EFTRequestType.Purchase)
                {
                    AmountBreakdown = new Entity.Model.HardwareStation.AmountBreakdown
                    {
                        TotalAmount = amount//Convert.ToDecimal(amount) / 100,
                    },
                    TenderType = tenderType
                };

                input.ConfigureBaseInputProperties(this);

                var purchaseResult = await _posService.PurchaseAsync(input, this._cancellationTokenSource?.Token);

                _logService.WebserviceCallSuccessful(webservice: "PurchaseAsync");

                _logService.LogInfo(message: $"ResultCode: {purchaseResult.ResultCode} - VerificationMethod: {purchaseResult.VerificationMethod}");

                if (!CanAddPaymentLineToTransaction(purchaseResult))
                {
                    if (purchaseResult.AuthorizationStatus == AuthorizationStatus.Declined)
                    {
                        purchaseResult.AddError(0, "Ekki heimilað", ErrorType.ExternalService);
                    }
                    if (purchaseResult.AuthorizationStatus == AuthorizationStatus.UserRejected)
                    {
                        purchaseResult.AddError(0, "Notandi hætti við", ErrorType.ExternalService);
                    }
                    if (purchaseResult.AuthorizationStatus == AuthorizationStatus.Cancelled)
                    {
                        purchaseResult.AddError(0, "Hætt við færslu", ErrorType.ExternalService);
                    }
                    return purchaseResult;
                }
                
                var addTenderOutput = await AddPaymentLineToTransaction(tenderType, input);

                if (!addTenderOutput.IsValid())
                {
                    purchaseResult.ErrorList = purchaseResult.ErrorList.Concat(addTenderOutput.ErrorList).ToList();

                    return purchaseResult;
                }

                var setCardEntryInput = CreateSetCardEntryInputDto(purchaseResult);
                setCardEntryInput.ReceiptID = input.Connection.RetailTransaction.ReceiptId;

                var cardEntryOutput = await _posService.SetCardEntryAsync(setCardEntryInput);

                if (!cardEntryOutput.IsValid())
                {
                    purchaseResult.ErrorList = purchaseResult.ErrorList.Concat(addTenderOutput.ErrorList).ToList();

                    return purchaseResult;
                }

                return purchaseResult;
            }
            finally
            {
                if (sessionResult != null)
                {
                    sessionResult = await DoPaymentSessions(EFTRequestType.FinishSession);
                    //Currently we don't care if the FinishSession didn't work once the payment has gone through
                }

                if (this._cancellationTokenSource?.Token.IsCancellationRequested ?? false)
                    this._cancellationTokenSource.TryReset();
            }
        }
       
        /// <summary>
        /// Finishes the current transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<FinishTransactionOutputDto> FinishTransactionAsync()
        {
            var input = new FinishTransactionInputDto();

            input.ConfigureBaseInputProperties(this);

            var result = await this._posService.FinishTransactionAsync(input);

            if (result.IsValid())
            {
                var otterConfig = _configService.GetConfigurationAsync<OtterConfig>("Config", "OtterConfig").Result;
                
                if(!otterConfig.AskForReceipt && otterConfig.ForceReceiptPrinting)
                {
                    this.PrintPreviousTrans("0");
                }

                _logService.MethodEndsWithSuccess();
            }
            else
            {
                _logService.MethodEndsWithError(errorMessages: result.ErrorList);
            }

            return result;
        }

        /// <summary>
        /// Voids the current transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<VoidTransactionOutputDto> VoidTransactionAsync()
        {
            var input = new VoidTransactionInputDto();

            input.ConfigureBaseInputProperties(this);

            var result = await this._posService.VoidTransactionAsync(input);

            if (result.IsValid())
            {
                _logService.MethodEndsWithSuccess();
            }
            else
            {
                _logService.MethodEndsWithError(errorMessages: result.ErrorList);
            }

            return result;
        }

        /// <summary>
        /// Voids an item from the current transaction.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<VoidItemOutputDto> VoidItemAsync(string itemCode, string lineNo)
        {
            var input = new VoidItemInputDto();
            input.ConfigureBaseInputProperties(this);
            input.Data = new AddToTransInputDataDto();
            input.Data.LineNo = lineNo;
            input.Data.Id = itemCode;
            
            var result = await _posService.VoidItemAsync(input);
            if (result.IsValid())
            {
                _logService.MethodEndsWithSuccess();
            }
            else
            {
                _logService.MethodEndsWithError(errorMessages: result.ErrorList);
            }
            return result;
        }

        /// <summary>
        /// Cancels the active transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<CancelActiveTransactionOutputDto> CancelActiveTransactionAsync()
        {
            var input = new CancelActiveTransactionInputDto();

            input.ConfigureBaseInputProperties(this);

            var result = await this._posService.CancelActiveTransactionAsync(input);

            if (result.IsValid())
            {
                _logService.MethodEndsWithSuccess();
            }
            else
            {
                _logService.MethodEndsWithError(errorMessages: result.ErrorList);
            }

            return result;
        }
       
        public StaffLogonOutputDto StaffLogon(string operatorId, string password)
        {
            var input = new StaffLogonInputDto();
            input.ConfigureBaseInputProperties(this);
            input.StaffId = operatorId;
            input.StaffID = operatorId;
            input.Password = password;
          
            var res = _posService.StaffLogonAsync(input).Result;
            return res;
        }
        public bool LobicoTrigger(string receptId)
        {
            return _posService.TriggerLobicoEvent(receptId);

        }
        public async void PrintPreviousTrans(string transactionId)
        {
            var input = new PrintPreviousTranInputDto();
            input.ConfigureBaseInputProperties(this);
            input.TransactionId = 0;
            var response = await _posService.PrintPreviousTransactionAsync(input);
        }
        public async Task<CalculateBasketOutputDto> CalculateTotals()
        {
            var input = new CalculateBasketInputDto();
            input.ConfigureBaseInputProperties(this);
            var response = await _posService.CalculateBasketAsync(input);
            return response;
        }
    }
}