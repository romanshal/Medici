namespace Medici.Abstractions.Contracts.Messaging
{
    /// <summary>
    /// Main interface for mediator requests.
    /// </summary>
    public interface IRequest { }

    /// <summary>
    /// Generic interface for represents a reguest with response.
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequest<out TResponse> : IRequest { }
}
