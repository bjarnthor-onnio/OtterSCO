using LS.SCO.Entity.CustomAttributes;
using LS.SCO.Helpers.Extensions;
using LS.SCO.Interfaces.Services.Configuration;
using LS.SCO.Interfaces.Services.ScoService;
using LS.SCO.Interfaces.Services.Validation;
using LS.SCO.Plugin.Service.ConfigurationManager;
using LS.SCO.Plugin.Service.Facade;
using LS.SCO.Plugin.Service.Interfaces;
using LS.SCO.Plugin.Service.ServiceController;
using LS.SCO.Plugin.Service.Services;
using LS.SCO.Plugin.Service.Validation;
using LS.SCO.Services.ServiceRegisterers;
using LS.SCO.WorkerService;
using Microsoft.Extensions.DependencyInjection;
using Onnio.PaymentService.Extensions;


namespace LS.SCO.Plugin.Service.ServiceRegisterers
{
    /// <summary>
    /// Sample service registerer for demonstrating the implementation of a custom POS service registerer
    /// </summary>
    [PluginService]
    public class SampleServiceRegisterer : BaseRegisterer
    {
        /// <summary>
        /// All the custom dependencies will be registered here to be injected by the DI container at runtime.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="contract"></param>
        /// <param name="implementation"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void RegisterGlobalServices(IServiceCollection services, Type contract, Type implementation)
        {
            if (contract == null || implementation == null) return;

            services.AddSingleton(contract, implementation);
        }

        public override void RegisterServices(IServiceCollection services = null)
        {
            Thread.Sleep(5000);
            base.RegisterServices(services);

            services.AddSingleton<ISamplePosService, SamplePosService>();
            services.AddSingleton<ISamplePosFacade, SamplePosFacade>();
            services.AddSingleton<ISampleValidationService, SampleValidationService>();
            services.AddSingleton<IServiceConfigurationManager, SampleServiceConfigurationManager>();
            services.AddSingleton<IAdapterValidationService, SampleAdapterValidationService>();
            services.AddSingleton<IScoControllerService, SampleServiceController>();
            services?.AddHostedService<BaseStarterService>();
            services.AddHttpClient();
            services.AddPaymentServices();
        }

        public override void CleanUpServices(IServiceCollection services)
        {
            //var serviceDescriptor = services?.Where(descriptor => descriptor.ServiceType.FullName.Contains(typeof(ISamplePosService).FullName)).ToList();

            //if (!serviceDescriptor.IsNullOrEmpty())
            //{
            //    foreach (var item in serviceDescriptor)
            //    {
            //        if ((item.ImplementationType?.Name ?? string.Empty) == typeof(SamplePosService).Name)
            //            services.Remove(item);
            //    }
            //}
        }

        /// <inheritdoc/>
        protected override string GetManufacturerName()
        {
            return "Sample";
        }

        public void RemoveService<TService>(IServiceCollection services)
        {
            var serviceDescriptor = services?.Where(descriptor => descriptor?.ServiceType == typeof(TService));

            if (!serviceDescriptor.IsNullOrEmpty())
            {
                foreach (var item in serviceDescriptor)
                {
                    services.Remove(item);
                }
            }
        }
    }
}