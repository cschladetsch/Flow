using UniRx;
using UnityEngine;

namespace Dekuple.Model
{
    /// <summary>
    /// Base for models which require a tracked world position.
    /// </summary>
    public interface IPositionedModel
        : IModel
    {
        IReactiveProperty<Vector3> Position { get; }
    }
}