using FluentAssertions;
using Medici.Tests.Contracts.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Medici.Tests
{
    public class ServiceCollectionShould
    {
        [Fact]
        public async Task ThrowInvalidException_WhenNoAnyHandlers()
        {
            var services = new ServiceCollection();
            new MediatorBuilder(new MediciConfiguration(), services).Build();

            var serviceProvider = services.BuildServiceProvider();

            var medici = new Medici(serviceProvider);

            Func<Task> act = async () => { await medici.SendAsync(new Ping("Ping")); };
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
