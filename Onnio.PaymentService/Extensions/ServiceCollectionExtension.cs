using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Onnio.PaymentService.Interfaces;
using Onnio.PaymentService.Services;

namespace Onnio.PaymentService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentServices(this IServiceCollection services)
        {
            // Register the dependencies
            services.AddScoped<INetgiroPaymentService, NetgiroPaymentService>();
            services.AddScoped<IPeiPaymentService, PeiPaymentService>();
            services.AddScoped<IOnnioPaymentService, OnnioPaymentService>();
            services.AddHttpClient();
            return services;
        }
    }
}
