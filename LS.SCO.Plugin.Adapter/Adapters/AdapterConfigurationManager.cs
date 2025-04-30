using LS.SCO.Entity.Adapters;
using LS.SCO.Entity.DTO.SCOService;
using LS.SCO.Entity.DTO.SCOService.TerminalSettings;
using LS.SCO.Entity.ExternalServices.Configuration;
using LS.SCO.Helpers.Extensions;
using LS.SCO.Interfaces.Cache;
using LS.SCO.Interfaces.Log;
using LS.SCO.Interfaces.Services.Configuration;
using LS.SCO.Plugin.Adapter.Interfaces;
using LS.SCO.Plugin.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LS.SCO.Plugin.Adapter.Adapters
{
    public class AdapterConfigurationManager : IAdapterConfigurationManager
    {
        private readonly ILogManager _logManager;
        private readonly IServiceConfigurationManager _configuration;
        private readonly ISamplePosServiceDisabled _posService;
        private readonly IBaseCacheService _cacheService;
        private readonly IServiceProvider _services;

        /// <inheritdoc/>
        public List<BaseAdapterConfiguration> Configuration { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AdapterConfigurationManager(ILogManager logManager, IServiceConfigurationManager configuration,
            ISamplePosServiceDisabled posService, IBaseCacheService cacheService, IServiceProvider services)
        {
            this._logManager = logManager;
            this._configuration = configuration;
            this._cacheService = cacheService;
            this.Configuration = new List<BaseAdapterConfiguration>();
            this._services = services;
            this._posService = (ISamplePosServiceDisabled)this._services.GetService<ISamplePosService>();
            this.LoadInstalledDevices();
        }

        /// <summary>
        /// Loads the installed devices from the configuration file.
        /// </summary>
        public void LoadInstalledDevices()
        {
            var appSettings = this._configuration.GetSetupSettings();

            this._logManager.LogServiceInformation(appSettings);

            var settings = _configuration.GetTerminalSettings();

            if (!string.IsNullOrWhiteSpace(settings?.Token))
            {
                var input = new GetTerminalSettingsInputDto
                {
                    Token = settings.Token,
                    StoreId = settings.StoreId,
                    TerminalId = settings.TerminalId
                };

                var result = this._posService.GetTerminalSettingsAsync(input).Result;

                appSettings.ApiClientConfiguration.HardwareStationApiBaseUrl = appSettings.ApiClientConfiguration.HardwareStationApiBaseUrl;
                appSettings.SignalRClientConfiguration.HardwareStationSignalRBaseUrl = appSettings.SignalRClientConfiguration.HardwareStationSignalRBaseUrl;

                _configuration.SaveSetupSettings(appSettings).Wait();

                if (result.ErrorList.Any(e => e.ErrorMessage.Contains("unauthorized")))
                {
                    var defaultConfigs = new BaseAdapterConfiguration();
                    defaultConfigs.StoreId = settings.StoreId;
                    defaultConfigs.Token = settings.Token;

                    this.Configuration.Add(defaultConfigs);
                }

                if ((result?.Token?.Equals(settings.Token) ?? false) && !result.ScoDevices.IsNullOrEmpty())
                {
                    this._cacheService.InitializeCacheServices(result);

                    foreach (var sco in result.ScoDevices)
                    {
                        this.Configuration.Add(this.FillAdapterConfiguration(sco, settings, result.SCOService.StaffID));
                    }

                    this.ValidateConfigurations();
                }
            }
        }

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
            if (this.Configuration.IsNullOrEmpty())
            {
                string message = $"Configuration file Missing: AppSetting.{{{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}}}.json";

                this._logManager.LogError(message);

                throw new Exception(message);
            }
        }
    }
}