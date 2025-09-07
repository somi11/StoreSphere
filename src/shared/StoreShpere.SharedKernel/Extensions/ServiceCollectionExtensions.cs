using Microsoft.Extensions.DependencyInjection;
using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedKernelEvents(this IServiceCollection services)
        {
            services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
            return services;
        }
    }
}
