using Medici.Abstractions.Contracts.Messaging;
using Medici.CQRS.Abstractions.Results;

namespace Medici.CQRS.Abstractions.Contracts
{
    public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, Result> where TQuery : IQuery;

    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;
}
