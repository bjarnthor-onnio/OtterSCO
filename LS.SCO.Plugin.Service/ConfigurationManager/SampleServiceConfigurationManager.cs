using AutoMapper;
using LS.SCO.Entity.DTO.SCOService.Controller;
using LS.SCO.Entity.ExternalServices.Configuration;
using LS.SCO.Entity.Model.Database;
using LS.SCO.Interfaces.Dal;
using LS.SCO.Interfaces.Services.Configuration;
using Microsoft.Extensions.Configuration;
using LSPosServicesClientConfiguration = LS.SCO.Entity.ExternalServices.Configuration.LSPosServicesClientConfiguration;

namespace LS.SCO.Plugin.Service.ConfigurationManager
{
    public class SampleServiceConfigurationManager : IServiceConfigurationManager
    {
        private readonly IConfiguration _configuration;
        private readonly IAppSettingsRepository _appSettingsRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Default parameter constructor
        /// </summary>
        /// <param name="authConfig"></param>
        public SampleServiceConfigurationManager(IConfiguration configuration, IAppSettingsRepository appSettingsRepository, IMapper mapper)
        {
            this._configuration = configuration;
            this._appSettingsRepository = appSettingsRepository;
            this._mapper = mapper;
        }

        /// <inheritdoc/>
        public AppSettingsJsonDto GetSetupSettings()
        {
            var result = new AppSettingsJsonDto();

            var settings = new Settings();

            this._configuration.Bind(settings);

            result = _mapper.Map<AppSettingsJsonDto>(settings);

            return result;
        }

        /// <inheritdoc/>
        public TerminalSettings GetTerminalSettings()
        {
            var terminalSettings = new TerminalSettings()
            {
                Token = _configuration.GetValue<string>(nameof(Settings.Token)),
                StoreId = _configuration.GetValue<string>(nameof(Settings.StoreId)),
                TerminalId = _configuration.GetValue<string>(nameof(Settings.TerminalId)),
            };

            return terminalSettings;
        }

        /// <inheritdoc/>
        public LSPosServicesClientConfiguration GetLSPosServicesClientConfiguration()
        {
            var config = new LSPosServicesClientConfiguration()
            {
                NetworkCredentials = new NetworkCredentials
                {
                    UserName = _configuration.GetValue<string>(nameof(Settings.UserName)),
                    Password = _configuration.GetValue<string>(nameof(Settings.Password)),
                },
                BaseEndpoint = _configuration.GetValue<string>(nameof(Settings.BaseEndpoint))
            };

            return config;
        }

        /// <inheritdoc/>
        public string GetPropertyConfiguration(string propertyName)
        {
            return _configuration.GetValue<string>(propertyName, null);
        }

        /// <inheritdoc/>
        public string GetLogFilePath()
        {
            return @$"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\LS Retail\LS Self-Checkout Connector"; //TODO: MOVE THIS TO A CONFIGURATION FILE OR OTHER COMMON PROJECT
        }

        /// <inheritdoc/>
        public async Task<(bool success, string message)> SaveSetupSettings(AppSettingsJsonDto appSettings)
        {
            (bool success, string message) result = new(true, string.Empty);

            if (appSettings == null)
                return (false, "Null");

            var destination = new AppSettingsJsonDto();

            this._configuration.Bind(destination);

            if (!destination.Equals(appSettings))
            {
                var settings = _mapper.Map<Settings>(appSettings);

                this.UpdateIConfiguration(settings);

                if (await _appSettingsRepository.UpdateAsync(settings) is null)
                    result.success = false;
            }

            return result;
        }

        /// <summary>
        /// Updates the destination file by reflection
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void UpdateJsonFileByReflection(object source, object destination)
        {
            var sourceProps = source.GetType().GetProperties();

            foreach (var prop in sourceProps)
            {
                var destProp = destination.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase));

                if (prop.PropertyType.Assembly.FullName.Contains("LS.SCO.Entity"))
                {
                    this.UpdateJsonFileByReflection(prop.GetValue(source), destProp.GetValue(destination));
                }
                else
                {
                    var srcValue = prop.GetValue(source);
                    var destValue = destProp.GetValue(destination);

                    if (!srcValue.Equals(destValue))
                        destProp.SetValue(destination, srcValue);
                }
            }
        }

        /// <summary>
        /// Update the IConfigurations values in memory before sending them to the database
        /// </summary>
        /// <param name="settings"></param>
        private void UpdateIConfiguration(Settings settings)
        {
            this._configuration[nameof(settings.Id)] = settings.Id.ToString();
            this._configuration[nameof(settings.UserName)] = settings.UserName;
            this._configuration[nameof(settings.Password)] = settings.Password;
            this._configuration[nameof(settings.Token)] = settings.Token;
            this._configuration[nameof(settings.StoreId)] = settings.StoreId;
            this._configuration[nameof(settings.BaseEndpoint)] = settings.BaseEndpoint;
            this._configuration[nameof(settings.Manufacturer)] = settings.Manufacturer;
            this._configuration[nameof(settings.HardwareStationBaseUrl)] = settings.HardwareStationBaseUrl;
        }
    }
}
