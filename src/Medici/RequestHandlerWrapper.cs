using Medici.Abstractions.Contracts;
using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Medici
{
    public abstract class RequestHandlerBase
    {
        public abstract Task<object?> HandleAsync(
            object request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default);
    }

    public abstract class RequestHandler : RequestHandlerBase
    {
        public abstract Task<Nil> HandleAsync(
            IRequest request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default);
    }

    public abstract class RequestHandler<TResponse> : RequestHandlerBase
    {
        public abstract Task<TResponse> HandleAsync(
            IRequest<TResponse> request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default);
    }

    public class RequestHandlerWrapper<TRequest> : RequestHandler where TRequest : IRequest
    {
        public override async Task<object?> HandleAsync(
            object request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default) =>
            await HandleAsync((IRequest)request, serviceProvider, cancellationToken).ConfigureAwait(false);

        public override Task<Nil> HandleAsync(
            IRequest request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default)
        {
            async Task<Nil> InnerHandler(CancellationToken token = default)
            {
                await serviceProvider
                    .GetRequiredService<IRequestHandler<TRequest>>()
                    .HandleAsync((TRequest)request, token == default ? cancellationToken : token);

                return Nil.Value;
            }

            return serviceProvider
                .GetServices<IPipelineBehavior<TRequest, Nil>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<Nil>)InnerHandler,
                    (next, pipeline) => (token) => pipeline.Handle((TRequest)request, next, token == default ? cancellationToken : token))(cancellationToken);
        }
    }

    public class RequestHandlerWrapper<TRequest, TResponse> : RequestHandler<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override async Task<object?> HandleAsync(
            object request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default) =>
            await HandleAsync((IRequest<TResponse>)request, serviceProvider, cancellationToken).ConfigureAwait(false);

        public override async Task<TResponse> HandleAsync(
            IRequest<TResponse> request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default)
        {
            Task<TResponse> InnerHandler(CancellationToken token = default) =>
                serviceProvider
                    .GetRequiredService<IRequestHandler<TRequest, TResponse>>()
                    .HandleAsync((TRequest)request, token == default ? cancellationToken : token);

            return await serviceProvider
                .GetServices<IPipelineBehavior<TRequest, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)InnerHandler,
                    (next, pipeline) => (token) => pipeline.Handle((TRequest)request, next, token == default ? cancellationToken : token))(cancellationToken);
        }
    }
}
