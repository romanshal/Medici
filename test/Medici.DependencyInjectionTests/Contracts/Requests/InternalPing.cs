using Medici.Abstractions.Contracts.Messaging;
using Medici.DependencyInjectionTests.Contracts.Responses;

namespace Medici.DependencyInjectionTests.Contracts.Requests
{
    internal record InternalPing : IRequest<Pong>;

    internal class InternalPingHandler : IRequestHandler<InternalPing, Pong>
    {
        public Task<Pong> HandleAsync(InternalPing request, CancellationToken cancellationToken = default) =>
            Task.FromResult(new Pong());
    }
}
