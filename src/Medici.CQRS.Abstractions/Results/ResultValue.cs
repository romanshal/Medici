using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Medici.CQRS.Abstractions.Results
{
    /// <summary>
    /// Represents a result with a value of type <typeparamref name="TValue"/>
    /// </summary>
    /// <typeparam name="TValue">The type of the value</typeparam>
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TValue}"/> class representing a successful result
        /// </summary>
        /// <param name="value">The type of the value</param>
        protected internal Result(TValue value) : base()
        {
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TValue}"/> class representing a failed result
        /// </summary>
        /// <param name="value">The type of the value</param>
        protected internal Result(Error error) : base(error) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TValue}"/> class
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="isSuccess">Success flag</param>
        /// <param name="error">The error</param>
        [JsonConstructor]
        protected internal Result(TValue? value, bool isSuccess, Error? error = null) : base(isSuccess, error) =>
            _value = value;

        /// <summary>
        /// Gets the value of the result
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the result has no value</exception>
        [NotNull]
        public TValue Value => _value! ?? throw new InvalidOperationException("Result has no value");

        /// <summary>
        /// Implicitly converts a value of type <typeparamref name="TValue"/> to a <see cref="Result{TValue}"/> with a successful result
        /// </summary>
        /// <param name="value">The value</param>
        public static implicit operator Result<TValue>(TValue? value) => Create(value);

        /// <summary>
        /// Implicitly converts an <see cref="Error"/> to a <see cref="ResultT{TValue}"/> with a failed result
        /// </summary>
        /// <param name="error">The error</param>
        public static implicit operator Result<TValue>(Error error) => Create<TValue>(error);
    }
}
