using System;
using App.Model;
using UniRx;

namespace Dekuple.Agent
{
    using Registry;
    using Model;

    /// <summary>
    /// Common for all agents that manage models in the system.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class AgentBase<TModel>
        : AgentLogger
        , IAgent<TModel>
        where TModel : class, IModel
    {
        public event Action<IAgent> OnDestroyed;
        public IRegistry<IAgent> Registry { get; set; }
        public Guid Id { get; /*private*/ set; }
        public IModel BaseModel { get; }
        public TModel Model => BaseModel as TModel;
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public IReadOnlyReactiveProperty<IOwner> Owner => Model.Owner;
        public IPlayerModel PlayerModel => Owner.Value as IPlayerModel;

        private readonly BoolReactiveProperty _destroyed = new BoolReactiveProperty(false);
        private bool _started = false;

        public virtual bool IsValid
        {
            get
            {
                if (Id == Guid.Empty) return false;
                if (Registry == null) return false;
                return BaseModel != null && Model.IsValid;
            }
        }

        protected AgentBase(TModel model)
        {
            if (model == null)
            {
                Error("Model cannot be null");
                return;
            }

            Assert.IsNotNull(model);
            BaseModel = model;
        }

        public bool SameOwner(IEntity other)
        {
            if (other == null)
                return Owner.Value == null;

            return other.Owner.Value == Owner.Value;
        }

        public void SetOwner(IOwner owner)
        {
            Model.SetOwner(owner);
        }

        public bool SameOwner(IOwned other)
        {
            if (other == null)
                return Owner.Value == null;

            return other.Owner.Value == Owner.Value;
        }

        public virtual void StartGame()
        {
            Assert.IsFalse(_started);
            _started = true;
        }

        public virtual void EndGame()
        {
            _started = false;
        }

        public void Destroy()
        {
            TransientCompleted();

            if (!_destroyed.Value)
                _destroyed.Value = true;

            OnDestroyed?.Invoke(this);
        }
    }
}
