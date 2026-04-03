using AutoMapper;

namespace Sparq.WebApi.Infrastructure
{
    /// <summary>
    /// Dependency injection extension methods for WebApi services.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers AutoMapper with the application's service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddAutomapper(this IServiceCollection services)
        {
            // 16.x helyes forma: Action<IMapperConfigurationExpression> lambda
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            return services;
        }
    }
}