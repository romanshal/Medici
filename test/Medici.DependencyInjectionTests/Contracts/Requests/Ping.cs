using Medici.Abstractions.Contracts.Messaging;
using Medici.DependencyInjectionTests.Contracts.Responses;

namespace Medici.DependencyInjectionTests.Contracts.Requests
{
    public record Ping : IRequest<Pong>;

    public class PingHandler : IRequestHandler<Ping, Pong>
    {
        public Task<Pong> HandleAsync(Ping request, CancellationToken cancellationToken = default) =>
            Task.FromResult(new Pong());
    }
}
