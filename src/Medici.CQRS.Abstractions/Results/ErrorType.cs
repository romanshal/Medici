namespace Medici.CQRS.Abstractions.Results
{
    public enum ErrorType
    {
        None,
        Forbidden,
        Unauthorized,
        Invalid,
        NotFound,
        NoContent,
        Conflict,
        Critical,
        Unavailable
    }
}
