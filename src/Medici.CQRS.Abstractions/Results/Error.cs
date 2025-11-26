namespace Medici.CQRS.Abstractions.Results
{
    public record Error(string Code, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static Error NullValue = new("Error.NullValue", "A null value was provided.");
    }
}
