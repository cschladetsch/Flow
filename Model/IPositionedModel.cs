using Dekuple.Model;
using UniRx;
using UnityEngine;

namespace Dekuple.Model
{
    public interface IPositionedModel
        : IModel
    {
        IReactiveProperty<Vector3> Position { get; }
    }
}