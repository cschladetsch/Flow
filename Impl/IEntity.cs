namespace Dekuple
{
    /// <summary>
    /// Something that exists in the game. Can be a Model, an Agent, a View or
    /// anything else.
    /// </summary>
    public interface IEntity
        : IHasId
        , IOwned
    {
        bool IsValid { get; }
    }
}
