using LS.SCO.Entity.Adapters;
using LS.SCO.Entity.DTO.HardwareStation;
using LS.SCO.Entity.DTO.HardwareStation.Input;
using LS.SCO.Entity.DTO.HardwareStation.Output;
using LS.SCO.Entity.DTO.SCOService;
using LS.SCO.Entity.DTO.SCOService.AddToTransaction;
using LS.SCO.Entity.DTO.SCOService.PrintPreviousTransaction;
using LS.SCO.Entity.DTO.SCOService.SetCardEntry;
using LS.SCO.Entity.Enums;
using LS.SCO.Entity.Extensions;
using LS.SCO.Entity.ExternalServices.Configuration;
using LS.SCO.Helpers.Extensions;
using LS.SCO.Plugin.Adapter.Adapters.Extensions;

namespace LS.SCO.Plugin.Adapter.Adapters
{
    /// <summary>
    /// This adapter class is responsible for creating and handling a connection to the SCO Device.
    /// </summary>
    public partial class SamplePosAdapter
    {
        /// <summary>
        /// Fills an object
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private BaseAdapterConfiguration FillAdapterConfiguration(ScoDeviceDto service, TerminalSettings settings, string staffId = "")
        {
            var device = new BaseAdapterConfiguration();

            foreach (var property in device.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                    property.SetValue(device, service?.FeatureFlags?.GetFeatureFlagValueByName(property.Name));

                if (property.PropertyType == typeof(int))
                    property.SetValue(device, int.TryParse(service?.FeatureFlags?.GetFeatureFlagValueByName(property.Name) ?? "0", out int number) ? number : number);
            }

            device.TerminalId = service.TerminalId; //device
            device.StaffId = staffId; //from get terminal settings
            device.StoreId = settings.StoreId; //config file
            device.Token = settings.Token; //config file

            return device;
        }

        /// <summary>
        /// Checks if the configurations were properly filled
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ValidateConfigurations()
        {
            if (this.AdapterConfiguration.IsNullOrEmpty())
            {
                string message = $"Configuration file Missing: AppSetting.{{{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}}}.json";

                this._logManager.LogError(message);

                throw new Exception(message);
            }
        }

        /// <summary>
        /// Creates SetCardEntryInputDto
        /// </summary>
        /// <param name="sessionResult"></param>
        /// <returns></returns>
        private SetCardEntryInputDto CreateSetCardEntryInputDto(EFTRequestOutputDto sessionResult)
        {
            SetCardEntryInputDto setCardEntryInputDto = new SetCardEntryInputDto
            {
                CardEntryHeader = new List<CardEntryHeaderDto>()
            };

            CardEntryHeaderDto cardEntryDto = _mapper.Map<CardEntryHeaderDto>(sessionResult);

            setCardEntryInputDto.CardEntryHeader.Add(cardEntryDto);

            setCardEntryInputDto = _mapper.Map<EFTRequestOutputDto, SetCardEntryInputDto>(sessionResult, setCardEntryInputDto);

            setCardEntryInputDto.ConfigureBaseInputProperties(this);
            

            return setCardEntryInputDto;
        }

        /// <summary>
        /// Initiates a payment session
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private async Task<EFTRequestOutputDto> DoPaymentSessions(EFTRequestType session)
        {
            _logService.MethodStart();

            var input = new SessionInputDto(session);
            input.ConfigureBaseInputProperties(this);

            _logService.LogInfo(message: $"Payment session: {input.SessionLogString()}");
            var sessionResult = await _posService.PaymentSessionAsync(input);

            return await _posService.HandlePostPaymentSession(sessionResult, input);
        }

        /// <summary>
        /// Adds a payment line to the transaction
        /// </summary>
        /// <param name="tenderType"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<AddToTransOutputDto> AddPaymentLineToTransaction(string tenderType, EFTRequestInputDto input)
        {
            var tenderInput = this._mapper.Map<EFTRequestInputDto, AddToTransInputDto>(input);

            tenderInput.Data ??= new AddToTransInputDataDto();
            tenderInput.Data.Code = tenderType;

            tenderInput.CopyBaseIdentification(input);
            
            var tenderOutput = await _posService.AddTenderAsync(tenderInput);

            return tenderOutput;
        }

        /// <summary>
        /// True if the payment line can be added to the transaction
        /// </summary>
        /// <param name="purchaseResult"></param>
        /// <returns></returns>
        private bool CanAddPaymentLineToTransaction(EFTRequestOutputDto purchaseResult)
        {
            return purchaseResult.IsValid() && purchaseResult.ResultCode == ResultCode.Success && purchaseResult.AuthorizationStatus == AuthorizationStatus.Approved;
        }

        private void PrintLastTransactionReceipt()
        {
            _logService.MethodStart();
            var input = new PrintPreviousTranInputDto();
            input.ConfigureBaseInputProperties(this);
            _posService.PrintPreviousTransactionAsync(input);
        }
    }
}