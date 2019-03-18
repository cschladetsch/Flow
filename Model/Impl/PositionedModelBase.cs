using Dekuple;
using Dekuple.Model;
using UniRx;
using UnityEngine;

namespace App.Models
{
    public class PositionedModelBase
        : ModelBase
        , IPositionedModel
    {
        public IReactiveProperty<Vector3> Position => _position;

        private readonly ReactiveProperty<Vector3> _position = new ReactiveProperty<Vector3>();

        public PositionedModelBase(IOwner owner) : base(owner)
        {
        }
    }
}