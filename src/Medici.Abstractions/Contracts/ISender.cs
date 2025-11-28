using Medici.Abstractions.Contracts.Messaging;

namespace Medici.Abstractions.Contracts
{
    /// <summary>
    /// Mediator instance for sending requests, queries, commands
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Asynchronously send a request to a single handler with no response
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request">Request object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Awaitable task that represents the send operation. The task result contains the handler response</returns>
        Task SendAsync(IRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send asynchronously request
        /// </summary>
        /// <typeparam name="TResponse">Response type</typeparam>
        /// <param name="request">Request object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Awaitable task that represents the send operation. The task result contains the handler response</returns>
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
