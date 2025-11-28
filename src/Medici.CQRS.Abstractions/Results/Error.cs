namespace Medici.CQRS.Abstractions.Results
{
    /// <summary>
    /// Represents an error in the application
    /// </summary>
    /// <param name="Code">The error code</param>
    /// <param name="Description">The error description</param>
    public record Error(string Code, string Description, ErrorType ErrorType)
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
        public static readonly Error NullValue = new("Error.NullValue", "A null value was provided.", ErrorType.Invalid);
        public static Error NotFound() => new("Error.NotFound", "The entity was not found", ErrorType.NotFound);
        public static Error NotFound<T>(T id) => new("Error.NotFound", $"The entity with id {id} was not found", ErrorType.NotFound);
        public static readonly Error Forbiden = new("Error.Forbiden", "Access is forbid for this area", ErrorType.Forbidden);
        public static readonly Error Unauthorized = new("Error.Unauthorized", "Unauthorized access", ErrorType.Unauthorized);
        public static readonly Error Invalid = new("Error.Invalid", "Invalid property", ErrorType.Invalid);
        public static readonly Error NoContent = new("Error.NoContent", "No content", ErrorType.NoContent);
        public static readonly Error Critical = new("Error.Critical", "Critical error", ErrorType.Critical);
        public static readonly Error Unavailable = new("Error.Unavailable", "Unavailable error", ErrorType.Unavailable);
    }
}
