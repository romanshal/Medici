using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;

namespace Medici.Behaviours
{
    public class RequestPreProcessorBehavior<TRequest, TResponse>(
        IEnumerable<IRequestPreProcessor<TRequest>> preProcessors) : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : notnull, IRequest
    {
        private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors = preProcessors;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            foreach (var processor in _preProcessors)
            {
                await processor.Process(request, cancellationToken).ConfigureAwait(false);
            }

            return await next(cancellationToken).ConfigureAwait(false);
        }
    }
}
