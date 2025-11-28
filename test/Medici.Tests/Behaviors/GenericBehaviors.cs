using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;
using Medici.Tests.Contracts.Requests;

namespace Medici.Tests.Behaviors
{
    public class GenericInnerBehavior<TRequest, TResponse>(
        Caller caller) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest
    {
        private readonly Caller _caller = caller;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            _caller.Messages.Add("Inner behavior before");
            var response = await next(cancellationToken);
            _caller.Messages.Add("Inner behavior after");

            return response;
        }
    }

    public class GenericOuterBehavior<TRequest, TResponse>(
        Caller caller) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest
    {
        private readonly Caller _caller = caller;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            _caller.Messages.Add("Outer behavior before");
            var response = await next(cancellationToken);
            _caller.Messages.Add("Outer behavior after");

            return response;
        }
    }
}
