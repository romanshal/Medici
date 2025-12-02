using Medici.Abstractions.Pipelines;
using Medici.Extensions;
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
        /// List of request post processors to register in queue order
        /// </summary>
        public Queue<ServiceDescriptor> RequestPostProcessors { get; } = new();

        /// <summary>
        /// Mediator implementation type to register. Default is <see cref="Medici"/>
        /// </summary>
        public Type MediatorImplementationType { get; set; } = typeof(Medici);

        /// <summary>
        /// Register various handlers from assembly
        /// </summary>
        /// <param name="assembly">Assembly to scan</param>
        /// <returns>This</returns>
        public MediciConfiguration RegisterServicesFromAssembly(Assembly assembly)
        {
            Assemblies.Add(assembly);

            return this;
        }

        /// <summary>
        /// Register various handlers from assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to scan</param>
        /// <returns>This</returns>
        public MediciConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
        {
            Assemblies.AddRange(assemblies);

            return this;
        }

        /// <summary>
        /// Register various handlers from assembly containing given type
        /// </summary>
        /// <param name="type">Type from assembly to scan</param>
        /// <returns>This</returns>
        public MediciConfiguration RegisterServicesFromAssemblyWithType(Type type) =>
            RegisterServicesFromAssembly(type.Assembly);

        /// <summary>
        /// Register various handlers from assembly containing given type
        /// </summary>
        /// <typeparam name="T">Type from assembly to scan</typeparam>
        /// <returns>This</returns>
        public MediciConfiguration RegisterServicesFromAssemblyWithType<T>()
            => RegisterServicesFromAssemblyWithType(typeof(T));

        /// <summary>
        /// Register various handlers from assembly containing given types
        /// </summary>
        /// <param name="types">Types from assembly to scan</param>
        /// <returns>This</returns>
        public MediciConfiguration RegisterServicesFromAssemblyeWithTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                RegisterServicesFromAssembly(type.Assembly);
            }

            return this;
        }

        /// <summary>
        /// Register a request pre processor type
        /// </summary>
        /// <param name="interfaceType">Request pre processor interface type</param>
        /// <param name="implementationType">Request pre processor implementation type</param>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public MediciConfiguration AddRequestPreProcessor(Type interfaceType, Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            RequestPreProcessors.Enqueue(new ServiceDescriptor(interfaceType, implementationType, serviceLifetime));

            return this;
        }

        /// <summary>
        /// Register a request pre processor type
        /// </summary>
        /// <typeparam name="TInterfaceType">Request pre processor interface type</typeparam>
        /// <typeparam name="TImplementationType">Request pre processor implementation type</typeparam>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public MediciConfiguration AddRequestPreProcessor<TInterfaceType, TImplementationType>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient) =>
            AddRequestPreProcessor(typeof(TInterfaceType), typeof(TImplementationType), serviceLifetime);

        /// <summary>
        /// Register a request pre processor type against all <see cref="IRequestPreProcessor{TRequest}"/> implementations
        /// </summary>
        /// <typeparam name="TImplementationType">Request pre processor implementation type</typeparam>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public MediciConfiguration AddRequestPreProcessor<TImplementationType>(
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            => AddRequestPreProcessor(typeof(TImplementationType), serviceLifetime);

        /// <summary>
        /// Register a closed request pre processor type against all <see cref="IRequestPreProcessor{TRequest}"/> implementations
        /// </summary>
        /// <param name="implementationType">Closed request pre processor implementation type</param>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public MediciConfiguration AddRequestPreProcessor(Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var implementedGenericInterfaces = implementationType.GetImplementableInterfaces(typeof(IRequestPreProcessor<>)).ToList();

            if (implementedGenericInterfaces.Count == 0)
            {
                throw new InvalidOperationException($"{implementationType.Name} must implement {typeof(IRequestPreProcessor<>).FullName}");
            }

            foreach (var implementedPreProcessorType in implementedGenericInterfaces)
            {
                RequestPreProcessors.Enqueue(new ServiceDescriptor(implementedPreProcessorType, implementationType, serviceLifetime));
            }

            return this;
        }
        /// <summary>
        /// Register a request post processor type
        /// </summary>
        /// <param name="interfaceType">Request post processor interface type</param>
        /// <param name="implementationType">Request post processor implementation type</param>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public MediciConfiguration AddRequestPostProcessor(Type interfaceType, Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            RequestPostProcessors.Enqueue(new ServiceDescriptor(interfaceType, implementationType, serviceLifetime));

            return this;
        }

        /// <summary>
        /// Register a request post processor type
        /// </summary>
        /// <typeparam name="TInterfaceType">Request post processor interface type</typeparam>
        /// <typeparam name="TImplementationType">Request post processor implementation type</typeparam>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public MediciConfiguration AddRequestPostProcessor<TInterfaceType, TImplementationType>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient) =>
            AddRequestPostProcessor(typeof(TInterfaceType), typeof(TImplementationType), serviceLifetime);

        /// <summary>
        /// Register a request post processor type against all <see cref="IRequestPostProcessor{TRequest,TResponse}"/> implementations
        /// </summary>
        /// <typeparam name="TImplementationType">Request post processor implementation type</typeparam>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public MediciConfiguration AddRequestPostProcessor<TImplementationType>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            => AddRequestPostProcessor(typeof(TImplementationType), serviceLifetime);

        /// <summary>
        /// Register a request post processor type against all <see cref="IRequestPostProcessor{TRequest,TResponse}"/> implementations
        /// </summary>
        /// <param name="implementationType">Request post processor implementation type</param>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public MediciConfiguration AddRequestPostProcessor(Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var implementedGenericInterfaces = implementationType.GetImplementableInterfaces(typeof(IRequestPostProcessor<,>)).ToList();

            if (implementedGenericInterfaces.Count == 0)
            {
                throw new InvalidOperationException($"{implementationType.Name} must implement {typeof(IRequestPostProcessor<,>).FullName}");
            }

            foreach (var implementedPostProcessorType in implementedGenericInterfaces)
            {
                RequestPostProcessors.Enqueue(new ServiceDescriptor(implementedPostProcessorType, implementationType, serviceLifetime));
            }
            return this;
        }
    }
}
