using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Access.Primitives.Orleans.Streaming
{
    public static class StreamExtensions
    {
        public static IServiceCollection AddStreamProviderRefs(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(p => !p.IsAbstract)
                .Where(p => typeof(StreamProviderReference).IsAssignableFrom(p));

            foreach (var type in types)
            {
                services.TryAddSingleton(type);
            }
            return services;
        }
    }
}
