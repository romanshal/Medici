using Medici.Abstractions.Contracts.Messaging;
using Medici.DependencyInjectionTests.Contracts.Responses;

namespace Medici.DependencyInjectionTests.Contracts.Requests
{
    public record PrivatePing : IRequest<Pong>
    {
        private class PrivatePingHandler : IRequestHandler<PrivatePing, Pong>
        {
            public Task<Pong> HandleAsync(PrivatePing request, CancellationToken cancellationToken) =>
                Task.FromResult(new Pong());
        }
    }
}
