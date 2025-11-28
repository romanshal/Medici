using Medici.Abstractions.Contracts.Messaging;

namespace Medici.Tests.Contracts.Requests
{
    public class Caller
    {
        public int CallCount { get; private set; } = 0;

        public IList<string> Messages { get; } = [];

        public void Call() => CallCount++;
    }

    public record NilPing(string? Message) : IRequest;

    public class NilPingHandler(Caller caller) : IRequestHandler<NilPing>
    {
        private readonly Caller _caller = caller;

        public Task HandleAsync(NilPing request, CancellationToken cancellationToken = default)
        {
            _caller.Call();
            _caller.Messages.Add("Handler");
            return Task.CompletedTask;
        }
    }
}
