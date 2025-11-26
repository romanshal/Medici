using Medici.Abstractions.Contracts.Messaging;
using Medici.Tests.Contracts.Responses;

namespace Medici.Tests.Contracts.Requests
{
    public record Ping(string? Message) : IRequest<Pong>;

    public class PingHandler(OutputLogger output) : IRequestHandler<Ping, Pong>
    {
        private readonly OutputLogger _output = output;

        public Task<Pong> HandleAsync(Ping request, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Handler");
            return Task.FromResult(new Pong (request.Message + " Pong"));
        }
    }
}
