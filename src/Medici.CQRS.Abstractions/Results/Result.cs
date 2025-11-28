using System.Text.Json.Serialization;

namespace Medici.CQRS.Abstractions.Results
{
    /// <summary>
    /// Represents the result of an operation that can either be successful or contain an error
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets a value indicating whether the result is successful
        /// </summary>
        public bool IsSuccess { get; } = true;

        /// <summary>
        /// Gets a value indicating whether the result is failure
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the error associated with the result, if any
        /// </summary>
        public Error? Error { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class representing a successful result
        /// </summary>
        [JsonConstructor]
        protected Result() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class representing a failed result with an error
        /// </summary>
        /// <param name="error">The error associated with the failed result</param>
        [JsonConstructor]
        protected Result(Error error)
        {
            IsSuccess = false;
            Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class
        /// </summary>
        /// <param name="isSuccess">Success flag</param>
        /// <param name="error">The error</param>
        [JsonConstructor]
        protected Result(bool isSuccess, Error? error = null)
        {
            if (isSuccess && error != null || !isSuccess && (error == null || error == Error.None))
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Result"/> representing a successful result
        /// </summary>
        /// <returns>A new instance of <see cref="Result"/> representing a successful result</returns>
        public static Result Success() => new();

        /// <summary>
        /// Creates a new instance of <see cref="Result"/> representing a failed result
        /// </summary>
        /// <param name="error">The error associated with the failed result</param>
        /// <returns>A new instance of <see cref="Result"/> representing a failed result</returns>
        public static Result Failure(Error error) => new(error);

        /// <summary>
        /// Creates a new instance of <see cref="Result{TValue}"/> representing a successful result
        /// </summary>
        /// <returns>A new instance of <see cref="Result{TValue}"/> representing a successful result</returns>
        public static Result<TValue> Success<TValue>(TValue value) => new(value);

        /// <summary>
        /// Creates a new instance of <see cref="Result{TValue}"/> representing a failed result
        /// </summary>
        /// <param name="error">The error associated with the failed result</param>
        /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result</returns>
        public static Result<TValue> Failure<TValue>(Error error) => new(error);

        /// <summary>
        /// Implicitly converts an <see cref="Error"/> to a <see cref="Result"/> representing a failed result.
        /// </summary>
        /// <param name="error">The error to convert.</param>
        /// <returns>A new instance of <see cref="Result"/> representing a failed result with the specified error.</returns>
        public static implicit operator Result(Error error) =>
            new(error);

        protected static Result<TValue> Create<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

        protected static Result<TValue> Create<TValue>(Error error) =>
            Failure<TValue>(error);
    }
}
