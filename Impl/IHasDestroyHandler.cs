using System;
using UniRx;

namespace Dekuple
{
    public interface IHasDestroyHandler<out T>
    {
        IReadOnlyReactiveProperty<bool> Destroyed { get; }
        event Action<T> OnDestroyed;
        void Destroy();
    }
}
