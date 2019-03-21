using UniRx;
using UnityEngine;

namespace Dekuple.Model
{
    /// <summary>
    /// Base for models which require a tracked scale in world space.
    /// </summary>
    public interface IWorldScaledModel
        : IModel
    {
        IReactiveProperty<Vector3> Scale { get; }
    }
}