using Medici.Abstractions.Contracts.Messaging;
using Medici.Tests.Contracts.Responses;

namespace Medici.Tests.Contracts.Requests
{
    public record Ping(string? Message) : IRequest<Pong>;

    public class PingHandler(Caller caller) : IRequestHandler<Ping, Pong>
    {
        private readonly Caller _caller = caller;

        public Task<Pong> HandleAsync(Ping request, CancellationToken cancellationToken = default)
        {
            _caller.Call();
            _caller.Messages.Add("Handler");
            return Task.FromResult(new Pong(request.Message + " Pong"));
        }
    }
}
