using Medici.CQRS.Abstractions.Contracts;
using Medici.CQRS.Abstractions.Results;
using Medici.Tests.Contracts.Responses;

namespace Medici.Tests.Contracts.Requests
{
    public record QueryPing(string? Message) : ICommand<Pong>;

    public class QueryPingHandler(Caller caller) : ICommandHandler<QueryPing, Pong>
    {
        private readonly Caller _caller = caller;

        public Task<Result<Pong>> HandleAsync(QueryPing request, CancellationToken cancellationToken = default)
        {
            _caller.Call();
            _caller.Messages.Add("Handler");
            return Task.FromResult(Result.Success(new Pong(request.Message + " Pong")));
        }
    }
}
