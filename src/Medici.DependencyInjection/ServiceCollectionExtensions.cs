using Microsoft.Extensions.DependencyInjection;

namespace Medici.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMedici(
            this IServiceCollection services,
            MediciConfiguration configuration)
        {
            if (configuration.Assemblies.Count == 0)
            {
                throw new ArgumentException("No assemblies found. Make sure that at least one assembly exist.");
            }

            return new MediatorBuilder(configuration, services).Build();
        }

        public static IServiceCollection AddMedici(
            this IServiceCollection services,
            Action<MediciConfiguration> configuration)
        {
            var mediciConfiguration = new MediciConfiguration();

            configuration.Invoke(mediciConfiguration);

            return AddMedici(services, mediciConfiguration);
        }
    }
}
