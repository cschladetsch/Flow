using UniRx;

namespace Dekuple
{
    /// <summary>
    /// Indicates that an object is owned by a player.
    /// </summary>
    public interface IOwned
    {
        IReadOnlyReactiveProperty<IOwner> Owner { get; }
    
        bool SameOwner(IOwned other);
        void SetOwner(IOwner owner);
    }
}
