using Medici.DependencyInjectionTests.Abstractions;
using Medici.DependencyInjectionTests.Fixtures;

namespace Medici.DependencyInjectionTests
{
    public class MicrosoftServiceCollectionExtensionsShould : BaseServiceCollectionExtensionsShould
    {
        public MicrosoftServiceCollectionExtensionsShould() : base(new MicrosoftServiceProviderFixture()) { }
    }
}
