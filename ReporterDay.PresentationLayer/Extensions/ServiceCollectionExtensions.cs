using Microsoft.Extensions.DependencyInjection;
using ReporterDay.PresentationLayer.Security;

namespace ReporterDay.PresentationLayer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddSingleton<IdProtector, ArticleIdProtector>();
            return services;
        }
    }
}
