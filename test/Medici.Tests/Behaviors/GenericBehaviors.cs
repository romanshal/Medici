using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;
using Medici.Tests.Contracts;

namespace Medici.Tests.Behaviors
{
    public class GenericInnerBehavior<TRequest, TResponse>(
        OutputLogger output) : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : notnull, IRequest
    {
        private readonly OutputLogger _output = output;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Inner behavior before");
            var response = await next(cancellationToken);
            _output.Messages.Add("Inner behavior after");

            return response;
        }
    }

    public class GenericOuterBehavior<TRequest, TResponse>(
        OutputLogger output) : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : notnull, IRequest
    {
        private readonly OutputLogger _output = output;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            _output.Messages.Add("Outer behavior before");
            var response = await next(cancellationToken);
            _output.Messages.Add("Outer behavior after");

            return response;
        }
    }
}
