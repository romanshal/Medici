using Medici.Abstractions.Contracts.Messaging;

namespace Medici.Abstractions.Pipelines
{
    /// <summary>
    /// Defined a request pre processor for a handler
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    public interface IRequestPreProcessor<in TRequest> where TRequest : notnull, IRequest
    {
        /// <summary>
        /// Method executes before handler
        /// </summary>
        /// <param name="request">Request instance</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An awaitable value task</returns>
        ValueTask Process(TRequest request, CancellationToken cancellationToken = default);
    }
}
