using Medici.Abstractions.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Medici.OpenProcessors
{
    public static class MediciConfigurationExtensions
    {
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

        public static MediciConfiguration AddOpenPosrProcessor(this MediciConfiguration configuration, Type openProcessType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
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
