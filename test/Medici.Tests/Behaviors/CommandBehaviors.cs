using Medici.Abstractions.Pipelines;
using Medici.CQRS.Abstractions.Results;
using Medici.Tests.Contracts.Requests;
using Medici.Tests.Contracts.Responses;

namespace Medici.Tests.Behaviors
{
    internal class InnerCommandBehaviors(Caller caller) : IPipelineBehavior<CommandPing, Result<Pong>>
    {
        private readonly Caller _caller = caller;
        public async Task<Result<Pong>> Handle(CommandPing request, RequestHandlerDelegate<Result<Pong>> next, CancellationToken cancellationToken = default)
        {
            _caller.Messages.Add("Inner behavior before");
            var response = await next(cancellationToken);
            _caller.Messages.Add("Inner behavior after");

            return response;
        }
    }

    public class OuterCommandBehaviors(Caller caller) : IPipelineBehavior<CommandPing, Result<Pong>>
    {
        private readonly Caller _caller = caller;

        public async Task<Result<Pong>> Handle(CommandPing request, RequestHandlerDelegate<Result<Pong>> next, CancellationToken cancellationToken = default)
        {
            _caller.Messages.Add("Outer behavior before");
            var response = await next(cancellationToken);
            _caller.Messages.Add("Outer behavior after");

            return response;
        }
    }
}
