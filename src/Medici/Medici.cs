using Medici.Abstractions.Contracts;
using Medici.Abstractions.Contracts.Messaging;
using System.Collections.Concurrent;

namespace Medici
{
    public class Medici(IServiceProvider serviceProvider) : IMedici
    {   
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ConcurrentDictionary<Type, RequestHandlerBase> _requestHandlers = new();

        public Task<TResponse> SendAsync<TResponse>(
            IRequest<TResponse> request, 
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var handler = (RequestHandler<TResponse>)_requestHandlers.GetOrAdd(request.GetType(), static requestHandlerType =>
            {
                var wrapperType = typeof(RequestHandlerWrapper<,>).MakeGenericType(requestHandlerType, typeof(TResponse));
                var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestHandlerType}");
                return (RequestHandlerBase)wrapper;
            });

            return handler.HandleAsync(request, _serviceProvider, cancellationToken);
        }

        public Task SendAsync(
            IRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var handler = (RequestHandler)_requestHandlers.GetOrAdd(request.GetType(), static requestType =>
            {
                var wrapperType = typeof(RequestHandlerWrapper<>).MakeGenericType(requestType);
                var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
                return (RequestHandlerBase)wrapper;
            });

            return handler.HandleAsync(request, _serviceProvider, cancellationToken);
        }
    }
}
