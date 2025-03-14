using LS.SCO.Entity.DTO.SCOService;
using LS.SCO.Entity.DTO.SCOService.TerminalSettings;
using LS.SCO.Entity.Enums;
using LS.SCO.Interfaces.ErrorHandling;
using LS.SCO.Interfaces.Factories;
using LS.SCO.Interfaces.Services.Configuration;
using LS.SCO.Plugin.Service.Interfaces;
using LS.SCO.Services.Constants.Services;
using LS.SCO.Services.Implementations.ScoService;
using LS.SCO.WorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LS.SCO.Plugin.Service.ServiceController
{
    /// <summary>
    /// Class responsible for managing the Sample service configuration and execution.
    /// </summary>
    public class SampleServiceController : ScoControllerService
    {
        /// <summary>
        /// Default parameter constructor.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="errorManager"></param>
        /// <param name="configuration"></param>
        /// <param name="scoService"></param>
        /// <param name="adapterFactory"></param>
        public SampleServiceController(
            IServiceProvider serviceProvider,
            IErrorManager errorManager,
            IServiceConfigurationManager configuration,
            ISamplePosService scoService,
            IAdapterFactory adapterFactory) : base(serviceProvider, errorManager, configuration, scoService, adapterFactory)
        {
        }

        /// <inheritdoc />
        public async override Task<IEnumerable<(ScoDeviceDto Terminal, bool Status)>> GetServiceTerminals(GetTerminalSettingsInputDto input)
        {
            var terminalSettings = await this._scoService.GetTerminalSettingsAsync(input);

            List<ScoDeviceDto> terminals = terminalSettings.ScoDevices;

            var joinedList = _adapterFactory.Adapters.Select(adapter =>
                {
                    return (Terminal: terminals[0], Status: true);
                });

            return joinedList;
        }

        /// <inheritdoc />
        protected override void InitializeServiceConfiguration(IServiceProvider serviceProvider)
        {
            this._service = serviceProvider?.GetServices<IHostedService>()?.OfType<BaseStarterService>()?.FirstOrDefault();

            if (this._service is null)
                throw new Exception(ScoServiceControllerErrorMessages.SCOServiceNotFound);

            this._token = new CancellationTokenSource();
        }

        /// <inheritdoc />
        protected override async Task<bool> ReloadConfiguration()
        {
            var result = false;

            var status = (await this.StopScoServiceAsync())?.ServiceStatus ?? this.GetServiceStatus();

            if (status == ScoServiceStatus.STOPPED)
            {
                if (this._service.RefreshAdapters())
                {
                    status = (await this.StartScoServiceAsync())?.ServiceStatus ?? this.GetServiceStatus();

                    result = status == ScoServiceStatus.RUNNING;
                }
            }

            return result;
        }
    }
}