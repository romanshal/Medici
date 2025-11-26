using Medici.Abstractions.Contracts.Messaging;
using Medici.CQRS.Abstractions.Results;

namespace Medici.CQRS.Abstractions.Contracts
{
    public interface IQuery : IRequest<Result>;

    public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
}
