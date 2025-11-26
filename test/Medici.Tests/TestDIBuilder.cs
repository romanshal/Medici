using Microsoft.Extensions.DependencyInjection;

namespace Medici.Tests
{
    public static class TestDIBuilder
    {
        public static IServiceProvider Build(Action<ServiceCollection> config)
        {
            var services = new ServiceCollection();

            ConfigAction(services);

            var container = services.BuildServiceProvider();

            return container;

            void ConfigAction(ServiceCollection cfg)
            {
                new MediatorBuilder(new MediciConfiguration(), cfg).Build();

                config(cfg);
            }
        }
    }
}
