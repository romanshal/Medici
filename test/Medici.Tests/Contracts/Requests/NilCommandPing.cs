using Medici.CQRS.Abstractions.Contracts;
using Medici.CQRS.Abstractions.Results;

namespace Medici.Tests.Contracts.Requests
{
    public record NilCommandPing(string? Message) : ICommand;

    public class NilCommandPingHandler(Caller caller) : ICommandHandler<NilCommandPing>
    {
        private readonly Caller _caller = caller;

        public async Task<Result> HandleAsync(NilCommandPing request, CancellationToken cancellationToken = default)
        {
            _caller.Call();
            _caller.Messages.Add("Handler");
            return Result.Success();
        }
    }
}
