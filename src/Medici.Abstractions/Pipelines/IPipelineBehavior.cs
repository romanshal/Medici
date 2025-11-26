using Medici.Abstractions.Contracts.Messaging;

namespace Medici.Abstractions.Pipelines
{
    /// <summary>
    /// Represents an async delegate for the next task in the pipeline
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Awaitable task returning a <typeparamref name="TResponse"/></returns>
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken cancellationToken = default);

    /// <summary>
    /// Pipeline behavior to inject pre/post processes
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : notnull, IRequest
    {
        Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default);
    }
}
