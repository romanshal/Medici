using FluentValidation;
using FluentValidation.Results;
using Medici.Abstractions.Contracts.Messaging;

namespace Medici.Behaviors.Validation
{
    public class ValidationProcess<TRequest>(
        IEnumerable<IValidator<TRequest>> validators) : IValidationProcess<TRequest> where TRequest : notnull, IRequest
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async ValueTask Process(TRequest request, CancellationToken cancellationToken = default)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults
                    .Where(validationResult => !validationResult.IsValid)
                    .SelectMany(validationResult => validationResult.Errors)
                    .Select(validationFailure => new ValidationFailure(
                        validationFailure.PropertyName,
                        validationFailure.ErrorMessage))
                    .ToList();

                if (failures.Count != 0)
                    throw new ValidationException(failures);
            }
        }
    }
}
