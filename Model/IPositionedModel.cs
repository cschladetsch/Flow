using Dekuple.Model;
using UniRx;
using UnityEngine;

namespace App.Models
{
    public interface IPositionedModel
        : IModel
    {
        IReactiveProperty<Vector3> Position { get; }
    }
}