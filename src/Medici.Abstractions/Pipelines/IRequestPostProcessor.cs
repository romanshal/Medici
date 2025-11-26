using Medici.Abstractions.Contracts.Messaging;

namespace Medici.Abstractions.Pipelines
{
    /// <summary>
    /// Defined a request post processor for a handler
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequestPostProcessor<in TRequest, in TResponse> where TRequest : notnull, IRequest
    {
        /// <summary>
        /// Method executes after handler
        /// </summary>
        /// <param name="request">Request instance</param>
        /// <param name="response">Response instance</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An awaitable value task</returns>
        ValueTask Process(TRequest request, TResponse response, CancellationToken cancellationToken = default);
    }
}
