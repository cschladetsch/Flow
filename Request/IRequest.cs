namespace Dekuple
{
    /// <summary>
    /// An action proposed by a player.
    /// </summary>
    public interface IRequest
        : IHasId
    {
        IOwner Owner { get; }
    }
}
