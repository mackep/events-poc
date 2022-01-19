using Microsoft.Extensions.DependencyInjection;

namespace Events.Handling
{
    public static class SetupExtensions
    {
        public static IServiceCollection AddEventHandler<THandler>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton) where THandler : IEventHandler
        {
            services.Add(new ServiceDescriptor(typeof(THandler), typeof(THandler), lifetime));
            services.Add(new ServiceDescriptor(typeof(IEventHandler), f => f.GetRequiredService(typeof(THandler)), lifetime));

            return services;
        }
    }
}
