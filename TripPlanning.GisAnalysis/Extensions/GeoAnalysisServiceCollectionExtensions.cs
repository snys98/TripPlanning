using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TripPlanning.GisAnalysis.Extensions
{
    public static class GeoAnalysisServiceCollectionExtensions
    {
        public static IServiceCollection AddPathResolver(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException("services");
            services.TryAdd(ServiceDescriptor.Singleton<IPathResolver, PathResolver>());
            GeoAnalysisConfig.Build(configuration);
            return services;
        }
    }
}