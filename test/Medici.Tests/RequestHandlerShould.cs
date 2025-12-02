using FluentAssertions;
using Medici.Abstractions.Contracts;
using Medici.DependencyInjection;
using Medici.Tests.Contracts.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Medici.Tests
{
    public class RequestHandlerShould
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMedici _medici;
        private readonly Caller _caller;

        public RequestHandlerShould()
        {
            var services = new ServiceCollection();
            _caller = new Caller();
            services.AddMedici(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(Ping).Assembly, typeof(NilPing).Assembly);
            });
            services.AddSingleton<Caller>(_caller);
            _serviceProvider = services.BuildServiceProvider();
            _medici = _serviceProvider.GetService<IMedici>()!;
        }

        [Fact]
        public async Task ResolveMainHandler()
        {
            var response = await _medici.SendAsync(new Ping("Ping"));

            response.Message.Should().Be("Ping Pong");
        }

        [Fact]
        public async Task ResolveNilHandler()
        {
            await _medici.SendAsync(new NilPing("Ping"));

            _caller.CallCount.Should().Be(1);
        }
    }
}
