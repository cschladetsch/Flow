using System;
using App.View;
using UnityEngine;

using UniRx;
using CoLib;

namespace Dekuple.View.Impl
{
    using Registry;
    using Model;
    using Agent;

    /// <inheritdoc cref="LoggingBehavior" />
    /// <summary>
    /// Common for all Views in the game. This is to replace MonoBehavior and make it more rational, as well as to
    /// conform with Flow.ITransient.
    /// </summary>
    public abstract class ViewBase
        : LoggingBehavior
        , IHasName
        , IViewBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IRegistry<IViewBase> Registry { get; set; }
        public IViewRegistry ViewRegistry => Registry as IViewRegistry;
        public IReadOnlyReactiveProperty<IOwner> Owner => AgentBase.Owner;
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public event Action<IViewBase> OnDestroyed;
        public IAgent AgentBase { get; set; }
        public IViewBase OwnerView { get; set; }
        public IModel OwnerModel => Owner.Value as IModel;
        public GameObject GameObject => gameObject;

        // lazy create because most views won't need a queue or audio source
        protected CommandQueue _Queue => _queue ?? (_queue = new CommandQueue());
        protected AudioSource _AudioSource => _audioSource ? _audioSource : (_audioSource = GameObject.AddComponent<AudioSource>());

        private bool _paused;
        private bool _created;
        private float _localTime;
        private CommandQueue _queue;
        private AudioSource _audioSource;
        private readonly BoolReactiveProperty _destroyed = new BoolReactiveProperty(false);

        public virtual bool IsValid
        {
            get
            {
                if (Id == Guid.Empty) return false;
                if (Registry == null) return false;
                if (ViewRegistry == null) return false;
                if (GameObject == null) return false;
                if (AgentBase == null) return false;
                return AgentBase.IsValid && AgentBase.BaseModel.IsValid;
            }
        }

        public bool SameOwner(IEntity other)
        {
            if (other == null)
                return Owner.Value == null;

            return other.Owner.Value == Owner.Value;
        }

        public virtual void SetAgent(IViewBase player, IAgent agent)
        {
            OwnerView = player;
            Assert.IsNotNull(agent);
            AgentBase = agent;
        }

        private bool Is<T>()
        {
            return typeof(T).IsAssignableFrom(GetType());
        }

        private void Awake()
        {
            Create();
        }

        private void Start()
        {
            Begin();
        }

        private void Update()
        {
            if (_paused)
                return;

            Step();
            _localTime += Time.deltaTime;
        }

        public virtual void Create()
        {
            Assert.IsFalse(_created);
            _created = true;
        }

        protected virtual void Begin()
        {
        }

        protected virtual void Step()
        {
            _queue?.Update(Time.deltaTime);
        }

        public void Pause(bool pause = true)
        {
            _paused = pause;
        }

        public float LifeTime()
        {
            return _localTime;
        }

        public bool SameOwner(IOwned other)
        {
            return ReferenceEquals(Owner.Value, other);
        }

        public void SetOwner(IOwner owner)
        {
            Verbose(20, $"New owner if {this} is {owner}");
            AgentBase.SetOwner(owner);
        }

        public virtual void Destroy()
        {
            Verbose(10, $"Destroy {this}");
            if (Destroyed.Value)
            {
                Warn($"Object {Id} of type {GetType()} already destoyed");
                return;
            }

            OnDestroyed?.Invoke(this);
            GameObject.transform.SetParent(null);
            _destroyed.Value = true;

            Destroy(GameObject);
        }

        public override string ToString()
        {
            return $"View {name} of type {GetType()}, Id={Id}";
        }
    }

    public class ViewBase<TIAgent>
        : ViewBase
        , IView<TIAgent>
        where TIAgent 
            : class, IAgent
    {
        public TIAgent Agent => AgentBase as TIAgent;

        // !NOTE! To override this, you ***must*** declare the typed signature
        // in the overridden interface. Otherwise it will fall back to this
        // default implementation. This is a trap you will fall into, so
        // sorry you had to read this comment after debugging for 5 minutes.
        //
        // Specifically, it is not enough to just override this in an
        // implementation. The signature must also be in the in the interface.
        //
        // Unsure if this is a bug in C# or intended behavior.
        //
        // !!NOTE!! Update 2019 with .Net 4.7 - this seems to've been fixed?
        //public virtual void SetAgent(IViewBase player, TIAgent agent)
        //{
        //    base.SetAgent(player, agent);
        //}
    }
}
