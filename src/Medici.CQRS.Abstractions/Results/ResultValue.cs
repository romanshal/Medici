using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Medici.CQRS.Abstractions.Results
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        [JsonConstructor]
        protected internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error) =>
            _value = value;

        [NotNull]
        public T Value => _value! ?? throw new InvalidOperationException("Result has no value.");

        public static implicit operator Result<T>(T? value) => Create(value);
    }
}
