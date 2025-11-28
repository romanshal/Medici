using FluentAssertions;
using Medici.Abstractions.Contracts;
using Medici.Abstractions.Contracts.Messaging;
using Medici.DependencyInjectionTests.Contracts.Requests;
using Medici.DependencyInjectionTests.Contracts.Responses;
using Microsoft.Extensions.DependencyInjection;

namespace Medici.DependencyInjectionTests.Abstractions
{
    public abstract class BaseServiceCollectionExtensionsShould(BaseServiceProviderFixture fixture) : IClassFixture<BaseServiceProviderFixture>
    {
        private readonly IServiceProvider _provider = fixture.Provider;

        [Fact]
        public void ResolveMedici_WhenServiceExist() =>
            _provider.GetService<IMedici>()
            .Should()
            .NotBeNull();

        [Fact]
        public void ResolveRequestHandler_WhenHandlerExist() =>
            _provider.GetService<IRequestHandler<Ping, Pong>>()
            .Should()
            .NotBeNull();
    }
}
