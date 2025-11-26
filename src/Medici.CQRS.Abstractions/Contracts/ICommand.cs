using Medici.Abstractions.Contracts.Messaging;
using Medici.CQRS.Abstractions.Results;

namespace Medici.CQRS.Abstractions.Contracts
{
    public interface ICommand : IRequest<Result>;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
}
