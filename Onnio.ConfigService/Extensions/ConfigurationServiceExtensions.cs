
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onnio.ConfigService.Interface;
using Onnio.ConfigService.Models;
using Onnio.ConfigService.Services;

namespace Onnio.ConfigService.Extensions
{
    public static class ConfigurationServiceExtensions
    {
        public static IServiceCollection AddConfigurationService(this IServiceCollection services, Action<ConfigurationServiceOptions> configureOptions = null)
        {
            var options = new ConfigurationServiceOptions();
            configureOptions?.Invoke(options);

            services.AddSingleton<IConfigurationService>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new ConfigurationService(configuration, options);
            });

            return services;
        }
    }
}
