using Medici.Abstractions.Contracts.Messaging;
using Medici.Abstractions.Pipelines;

namespace Medici.Behaviors.Validation
{
    public interface IValidationProcess<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull, IRequest;
}
