using UniRx;
using UnityEngine;

namespace Dekuple.Model
{
    /// <summary>
    /// Base for models which require a tracked scale in local space.
    /// </summary>
    public interface ILocalScaledModel
        : IModel
    {
        IReactiveProperty<Vector3> LocalScale { get; }
    }
}