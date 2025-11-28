using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;
using Medici.Extensions;
using Medici.OpenProcessors;

namespace Medici.Behaviors.Validation
{
    public static class MediciConfigurationExtensions
    {
        /// <summary>
        /// Registers an open validation request pre processor type against the <see cref="IRequestPreProcessor{TRequest}"/> open generic interface type with default <see cref="ValidationProcess{TRequest}"/> validator
        /// </summary>
        /// <param name="configuration">Configuration options</param>
        /// <returns>Configuration options</returns>
        public static MediciConfiguration AddValidationBehavior(this MediciConfiguration configuration)
        {
            configuration.AddOpenPreProcessor(typeof(ValidationProcess<>));

            return configuration;
        }

        /// <summary>
        /// Registers an open validation request pre processor type against the <see cref="IRequestPreProcessor{TRequest}"/> open generic interface type with default custom validator
        /// </summary>
        /// <param name="configuration">Configuration options</param>
        /// <param name="validationType">Validation processor implementation type</param>
        /// <returns>Configuration options</returns>
        public static MediciConfiguration AddValidationBehavior(this MediciConfiguration configuration, Type validationType)
        {
            var implementedGenericInterfaces = validationType.GetImplementableInterfaces(typeof(IPipelineBehavior<,>)).ToList();
            if (implementedGenericInterfaces.Count == 0)
            {
                throw new InvalidOperationException($"{validationType.Name} must implement {typeof(IValidationProcess<>).FullName}");
            }

            foreach (var implementedValidatonType in implementedGenericInterfaces)
            {
                configuration.AddOpenPreProcessor(implementedValidatonType);
            }

            return configuration;
        }

        /// <summary>
        /// Registers an open validation request pre processor type against the <see cref="IRequestPreProcessor{TRequest}"/> open generic interface type with default custom validator
        /// </summary>
        /// <typeparam name="TValidationType">Validation processor implementation type</typeparam>
        /// <param name="configuration">Configuration options</param>
        /// <returns>Configuration options</returns>
        public static MediciConfiguration AddValidationBehavior<TValidationType>(this MediciConfiguration configuration) where TValidationType : IValidationProcess<IRequest>
        {
            configuration.AddOpenPreProcessor(typeof(TValidationType));

            return configuration;
        }
    }
}
