using Medici.Abstractions.Contracts;
using Medici.Abstractions.Pipelines;
using Medici.Tests.Contracts;
using Medici.Tests.Contracts.Requests;

namespace Medici.Tests.Behaviors
{
    public class NilInnerBehavior(
        OutputLogger output) : IPipelineBehavior<NilPing, Nil>
    {
        private readonly OutputLogger _output = output;

        public async Task<Nil> Handle(NilPing request, RequestHandlerDelegate<Nil> next, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Inner nil behavior before");
            var response = await next(cancellationToken);
            _output.Messages.Add("Inner nil behavior after");

            return response;
        }
    }

    public class NilOuterBehavior(
        OutputLogger output) : IPipelineBehavior<NilPing, Nil>
    {
        private readonly OutputLogger _output = output;

        public async Task<Nil> Handle(NilPing request, RequestHandlerDelegate<Nil> next, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Outer nil behavior before");
            var response = await next(cancellationToken);
            _output.Messages.Add("Outer nil behavior after");

            return response;
        }
    }
}
