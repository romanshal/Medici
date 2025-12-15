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

            _caller.CallCount.Should().Be(1);
        }

        [Fact]
        public async Task ResolveNilHandler()
        {
            await _medici.SendAsync(new NilPing("Ping"));

            _caller.CallCount.Should().Be(1);
        }

        [Fact]
        public async Task ResolveCommandTypes()
        {
            var response = await _medici.SendAsync(new CommandPing("Ping"));

            response.IsSuccess.Should().BeTrue();
            response.Error.Should().BeNull();
            response.Value.Should().NotBeNull();
            response.Value.Message.Should().Be("Ping Pong");

            response.IsSuccess.Should().BeTrue();
            response.Value.Should().NotBeNull();
            response.Value.Message.Should().Be("Ping Pong");

            _caller.CallCount.Should().Be(1);
        }

        [Fact]
        public async Task ResolveNilCommandTypes()
        {
            var response = await _medici.SendAsync(new NilCommandPing("Ping"));

            response.IsSuccess.Should().BeTrue();
            response.Error.Should().BeNull();

            _caller.CallCount.Should().Be(1);
        }

        [Fact]
        public async Task ResolveQueryTypes()
        {
            var response = await _medici.SendAsync(new QueryPing("Ping"));

            response.IsSuccess.Should().BeTrue();
            response.Error.Should().BeNull();
            response.Value.Should().NotBeNull();
            response.Value.Message.Should().Be("Ping Pong");

            response.IsSuccess.Should().BeTrue();
            response.Value.Should().NotBeNull();
            response.Value.Message.Should().Be("Ping Pong");

            _caller.CallCount.Should().Be(1);
        }

        [Fact]
        public async Task ResolveNilQueryTypes()
        {
            var response = await _medici.SendAsync(new NilQueryPing("Ping"));

            response.IsSuccess.Should().BeTrue();
            response.Error.Should().BeNull();

            _caller.CallCount.Should().Be(1);
        }
    }
}
