using Medici.CQRS.Abstractions.Contracts;
using Medici.CQRS.Abstractions.Results;

namespace Medici.Tests.Contracts.Requests
{
    public record NilQueryPing(string? Message) : IQuery;

    public class NilQueryPingHandler(Caller caller) : IQueryHandler<NilQueryPing>
    {
        private readonly Caller _caller = caller;
        public async Task<Result> HandleAsync(NilQueryPing request, CancellationToken cancellationToken = default)
        {
            _caller.Call();
            _caller.Messages.Add("Handler");
            return Result.Success();
        }
    }
}
