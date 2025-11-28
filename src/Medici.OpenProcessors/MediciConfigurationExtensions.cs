using Medici.Abstractions.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Medici.OpenProcessors
{
    public static class MediciConfigurationExtensions
    {
        /// <summary>
        /// Registers an open pre processor type against the <see cref="IPipelineBehavior{TRequest,TResponse}"/> open generic interface type
        /// </summary>
        /// <param name="configuration">Configuration options</param>
        /// <param name="openProcessType">An open generic processor type</param>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/></param>
        /// <returns>Configuration options</returns>
        public static MediciConfiguration AddOpenPreProcessor(this MediciConfiguration configuration, Type openProcessType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            if (!openProcessType.IsGenericType)
            {
                throw new InvalidOperationException($"{openProcessType.Name} must be generic");
            }

            var genericInterfaces = openProcessType.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition());
            var openProcessInterfaces = new HashSet<Type>(genericInterfaces.Where(i => i == typeof(IRequestPreProcessor<>)));

            if (openProcessInterfaces.Count == 0)
            {
                throw new InvalidOperationException($"{openProcessType.Name} must implement {typeof(IRequestPreProcessor<>).FullName}");
            }

            foreach (var openBehaviorInterface in openProcessInterfaces)
            {
                configuration.RequestPreProcessors.Enqueue(new ServiceDescriptor(openBehaviorInterface, openProcessType, serviceLifetime));
            }

            return configuration;
        }

        /// <summary>
        /// Registers an open post processor type against the <see cref="IPipelineBehavior{TRequest,TResponse}"/> open generic interface type
        /// </summary>
        /// <param name="configuration">Configuration options</param>
        /// <param name="openProcessType">An open generic processor type</param>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/></param>
        /// <returns>Configuration options</returns>
        public static MediciConfiguration AddOpenPostProcessor(this MediciConfiguration configuration, Type openProcessType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            if (!openProcessType.IsGenericType)
            {
                throw new InvalidOperationException($"{openProcessType.Name} must be generic");
            }

            var genericInterfaces = openProcessType.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition());
            var openProcessInterfaces = new HashSet<Type>(genericInterfaces.Where(i => i == typeof(IRequestPostProcessor<,>)));

            if (openProcessInterfaces.Count == 0)
            {
                throw new InvalidOperationException($"{openProcessType.Name} must implement {typeof(IRequestPostProcessor<,>).FullName}");
            }

            foreach (var openBehaviorInterface in openProcessInterfaces)
            {
                configuration.RequestPreProcessors.Enqueue(new ServiceDescriptor(openBehaviorInterface, openProcessType, serviceLifetime));
            }

            return configuration;
        }
    }
}
