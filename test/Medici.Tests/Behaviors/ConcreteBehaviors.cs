using Medici.Abstractions.Pipelines;
using Medici.Tests.Contracts;
using Medici.Tests.Contracts.Requests;
using Medici.Tests.Contracts.Responses;

namespace Medici.Tests.Behaviors
{
    public class ConcreteInnerBehavior(OutputLogger output) : IPipelineBehavior<Ping, Pong>
    {
        private readonly OutputLogger _output = output;

        public async Task<Pong> Handle(Ping request, RequestHandlerDelegate<Pong> next, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Inner behavior before");
            var response = await next(cancellationToken);
            _output.Messages.Add("Inner behavior after");

            return response;
        }
    }

    public class ConcreteOuterBehavior(OutputLogger output) : IPipelineBehavior<Ping, Pong>
    {
        private readonly OutputLogger _output = output;

        public async Task<Pong> Handle(Ping request, RequestHandlerDelegate<Pong> next, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Outer behavior before");
            var response = await next(cancellationToken);
            _output.Messages.Add("Outer behavior after");

            return response;
        }
    }
}
