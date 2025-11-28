using Medici.Abstractions.Contracts;
using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;
using Medici.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Medici
{
    public class MediatorBuilder(MediciConfiguration configuration, IServiceCollection services)
    {
        private readonly MediciConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        private readonly IServiceCollection _services = services ?? throw new ArgumentNullException(nameof(services));

        public IServiceCollection Build()
        {
            RegisterMediator();
            RegisterHandlers();
            RegisterPipeline();

            _services.TryAddSingleton(_configuration);

            return _services;
        }

        private void RegisterMediator()
        {
            _services.TryAdd(new ServiceDescriptor(
                typeof(IMedici),
                _configuration.MediatorImplementationType,
                _configuration.Lifetime));

            _services.TryAdd(new ServiceDescriptor(
                typeof(ISender),
                sp => sp.GetRequiredService<IMedici>(),
                _configuration.Lifetime));
        }

        private void RegisterHandlers()
        {
            _services
                .Scan(scan => scan
                    .FromAssemblies(_configuration.Assemblies)
                    .AddClasses(c => c.
                        Where(type =>
                            type.GetInterfaces().Any(i =>
                                i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                    .AddClasses(c => c
                        .Where(type =>
                            type.GetInterfaces().Any(i =>
                                i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IRequestHandler<>))))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }

        private void RegisterPipeline()
        {
            if (_configuration.RequestPreProcessors.Count > 0)
            {
                _services.TryAddEnumerable(ServiceDescriptor.Transient(
                    typeof(IPipelineBehavior<,>),
                    typeof(RequestPreProcessorBehavior<,>)));

                _services.TryAddEnumerable(_configuration.RequestPreProcessors);
            }

            if (_configuration.RequestPostProcessors.Count > 0)
            {
                _services.TryAddEnumerable(ServiceDescriptor.Transient(
                    typeof(IPipelineBehavior<,>),
                    typeof(RequestPostProcessorBehavior<,>)));

                _services.TryAddEnumerable(_configuration.RequestPostProcessors);
            }
        }
    }
}
