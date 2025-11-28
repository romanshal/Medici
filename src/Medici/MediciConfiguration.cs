using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Medici
{
    public class MediciConfiguration
    {
        /// <summary>
        /// Service lifetime to register services under. Default value is <see cref="ServiceLifetime.Transient"/>
        /// </summary>
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

        /// <summary>
        /// Asseblies to register
        /// </summary>
        public List<Assembly> Assemblies { get; } = [];

        /// <summary>
        /// List of request pre processors to register in queue order
        /// </summary>
        public Queue<ServiceDescriptor> RequestPreProcessors { get; } = new();

        /// <summary>
        /// List of request post processors to register in specific order
        /// </summary>
        public Queue<ServiceDescriptor> RequestPostProcessors { get; } = new();

        public bool OnlyGenericHandlers { get; set; } = false;

        /// <summary>
        /// Mediator implementation type to register. Default is <see cref="Medici"/>
        /// </summary>
        public Type MediatorImplementationType { get; set; } = typeof(Medici);

        public MediciConfiguration RegisterServicesFromAssembly(Assembly assembly)
        {
            Assemblies.Add(assembly);

            return this;
        }

        public MediciConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
        {
            Assemblies.AddRange(assemblies);

            return this;
        }

        public MediciConfiguration RegisterServicesFromAssemblyWithType(Type type) =>
            RegisterServicesFromAssembly(type.Assembly);

        public MediciConfiguration RegisterServicesFromAssemblyWithType<T>()
            => RegisterServicesFromAssemblyWithType(typeof(T));

        public MediciConfiguration RegisterServicesFromAssemblyeWithTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                RegisterServicesFromAssembly(type.Assembly);
            }

            return this;
        }

        public MediciConfiguration AddRequestPreProcessor(Type interfaceType, Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            RequestPreProcessors.Enqueue(new ServiceDescriptor(interfaceType, implementationType, serviceLifetime));

            return this;
        }

        public MediciConfiguration AddRequestPreProcessor<TInterfaceType, TImplementationType>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient) =>
            AddRequestPreProcessor(typeof(TInterfaceType), typeof(TImplementationType), serviceLifetime);

        public MediciConfiguration AddRequestPostProcessor(Type interfaceType, Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            RequestPostProcessors.Enqueue(new ServiceDescriptor(interfaceType, implementationType, serviceLifetime));

            return this;
        }

        public MediciConfiguration AddRequestPostProcessor<TInterfaceType, TImplementationType>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient) =>
            AddRequestPostProcessor(typeof(TInterfaceType), typeof(TImplementationType), serviceLifetime);
    }
}
