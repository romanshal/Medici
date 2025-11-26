using Medici.DependencyInjection;
using Medici.DependencyInjectionTests.Abstractions;
using Medici.DependencyInjectionTests.Contracts.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Medici.DependencyInjectionTests.Fixtures
{
    internal class MicrosoftServiceProviderFixture : BaseServiceProviderFixture
    {
        public override IServiceProvider Provider => new ServiceCollection()
            .AddFakeLogging()
            .AddMedici(cfg => cfg.RegisterServicesFromAssembly(typeof(Ping).Assembly))
            .BuildServiceProvider();
    }
}
