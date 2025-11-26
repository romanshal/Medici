using Medici.Abstractions.Contracts.Messaging;

namespace Medici.Tests.Contracts.Requests
{
    public record NilPing(string? Message) : IRequest;

    public class NilPingHandler(OutputLogger output) : IRequestHandler<NilPing>
    {
        private readonly OutputLogger _output = output;

        public Task HandleAsync(NilPing request, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Handler");
            return Task.CompletedTask;
        }
    }
}
