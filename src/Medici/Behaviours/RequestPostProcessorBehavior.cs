using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;

namespace Medici.Behaviours
{
    public class RequestPostProcessorBehavior<TRequest, TResponse>(
        IEnumerable<IRequestPostProcessor<TRequest, TResponse>> postProcessors) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest
    {
        private readonly IEnumerable<IRequestPostProcessor<TRequest, TResponse>> _postProcessors = postProcessors;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            var response = await next(cancellationToken).ConfigureAwait(false);

            foreach (var processor in _postProcessors)
            {
                await processor.Process(request, response, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }
    }
}
