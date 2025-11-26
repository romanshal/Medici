using Medici.CQRS.Abstractions.Contracts;
using Medici.CQRS.Abstractions.Results;
using Medici.Tests.Contracts.Responses;

namespace Medici.Tests.Contracts.Requests
{
    public record CommandPing(string? Message) : ICommand<Pong>;

    public class CommandPingHandler(OutputLogger output) : ICommandHandler<CommandPing, Pong>
    {
        private readonly OutputLogger _output = output;

        public Task<Result<Pong>> HandleAsync(CommandPing request, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Handler");
            return Task.FromResult(Result.Success(new Pong(request.Message + " Pong")));
        }
    }
}
