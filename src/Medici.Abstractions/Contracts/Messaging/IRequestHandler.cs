namespace Medici.Abstractions.Contracts.Messaging
{
    /// <summary>
    /// Defines a handler for a request
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles a reguest
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task representing response from the request</returns>
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Defines a handler for a request with void response
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IRequestHandler<in TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Handles a reguest
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task representing void response from the request</returns>
        Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
