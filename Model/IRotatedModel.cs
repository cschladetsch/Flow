using Dekuple.Model;
using UniRx;
using UnityEngine;

namespace Dekuple.Model
{
    /// <summary>
    /// Base for models which require a tracked rotation.
    /// </summary>
    public interface IRotatedModel
        : IModel
    {
        IReactiveProperty<Quaternion> Rotation { get; }
    }
}