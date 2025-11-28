using Medici.Abstractions.Contracts;
using Medici.Abstractions.Pipelines;
using Medici.Tests.Contracts;
using Medici.Tests.Contracts.Requests;

namespace Medici.Tests.Behaviors
{
    public class NilInnerBehavior(
        Caller caller) : IPipelineBehavior<NilPing, Nil>
    {
        private readonly Caller _caller = caller;

        public async Task<Nil> Handle(NilPing request, RequestHandlerDelegate<Nil> next, CancellationToken cancellationToken = default)
        {
            _caller.Messages.Add("Inner nil behavior before");
            var response = await next(cancellationToken);
            _caller.Messages.Add("Inner nil behavior after");

            return response;
        }
    }

    public class NilOuterBehavior(
        Caller caller) : IPipelineBehavior<NilPing, Nil>
    {
        private readonly Caller _caller = caller;

        public async Task<Nil> Handle(NilPing request, RequestHandlerDelegate<Nil> next, CancellationToken cancellationToken = default)
        {
            _caller.Messages.Add("Outer nil behavior before");
            var response = await next(cancellationToken);
            _caller.Messages.Add("Outer nil behavior after");

            return response;
        }
    }
}
