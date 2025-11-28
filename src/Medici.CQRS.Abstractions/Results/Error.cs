namespace Medici.CQRS.Abstractions.Results
{
    /// <summary>
    /// Represents an error in the application
    /// </summary>
    /// <param name="Code">The error code</param>
    /// <param name="Description">The error description</param>
    public record Error(string Description, ErrorType ErrorType)
    {
        public static readonly Error None = new(string.Empty, ErrorType.None);
        public static Error NullValue() => new("A null value was provided", ErrorType.Invalid);
        public static Error NullValue<T>(T value) => new($"A null value was provided for {nameof(T)}", ErrorType.Invalid);
        public static Error NotFound() => new("The entity was not found", ErrorType.NotFound);
        public static Error NotFound<T>(T id) => new($"The entity with id {id} was not found", ErrorType.NotFound);
        public static readonly Error Forbiden = new("Access is forbid for this area", ErrorType.Forbidden);
        public static readonly Error Unauthorized = new("Unauthorized access", ErrorType.Unauthorized);
        public static Error Invalid() => new("Invalid property", ErrorType.Invalid);
        public static Error Invalid<T>(T value) => new($"Invalid property {nameof(T)}. Value: {value}", ErrorType.Invalid);
        public static readonly Error NoContent = new("No content", ErrorType.NoContent);
        public static readonly Error Critical = new("Critical error", ErrorType.Critical);
        public static readonly Error Unavailable = new("Unavailable error", ErrorType.Unavailable);
    }
}
