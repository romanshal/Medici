using Medici.Abstractions.Pipelines;
using Medici.Tests.Contracts.Requests;
using Medici.Tests.Contracts.Responses;

namespace Medici.Tests.Behaviors
{
    public class ConcreteInnerBehavior(Caller caller) : IPipelineBehavior<Ping, Pong>
    {
        private readonly Caller _caller = caller;

        public async Task<Pong> Handle(Ping request, RequestHandlerDelegate<Pong> next, CancellationToken cancellationToken = default)
        {
            _caller.Messages.Add("Inner behavior before");
            var response = await next(cancellationToken);
            _caller.Messages.Add("Inner behavior after");

            return response;
        }
    }

    public class ConcreteOuterBehavior(Caller caller) : IPipelineBehavior<Ping, Pong>
    {
        private readonly Caller _caller = caller;

        public async Task<Pong> Handle(Ping request, RequestHandlerDelegate<Pong> next, CancellationToken cancellationToken = default)
        {
            _caller.Messages.Add("Outer behavior before");
            var response = await next(cancellationToken);
            _caller.Messages.Add("Outer behavior after");

            return response;
        }
    }
}
