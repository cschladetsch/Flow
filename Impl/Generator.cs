using System;

namespace Flow.Impl
{
    public class Generator
        : Transient
        , IGenerator
    {
        public event GeneratorHandler Suspended;
        public event GeneratorHandler Resumed;
        public event GeneratorHandler Stepped;

        public virtual object Value { get; protected set; }

        public Generator()
        {
            OnDisposed += tr => Suspend();
        }

        public bool Running { get; private set; }

        public int StepNumber { get; protected set; }

        public new IGenerator Named(string name)
        {
            Name = name;
            return this;
        }

        public virtual void Step()
        {
            if (!Active)
                return;

            ++StepNumber;
            Stepped?.Invoke(this);
        }

        public virtual void Pre()
        {
        }

        public virtual void Post()
        {
        }

        public void Suspend()
        {
            Running = false;
            Suspended?.Invoke(this);
        }

        public void Resume()
        {
            if (Running || !Active)
                return;

            Running = true;
            Resumed?.Invoke(this);
        }

        public IGenerator SuspendAfter(ITransient other)
        {
            if (IsNullOrInactive(other))
            {
                Suspend();
                return this;
            }

            Resume();

            // thanks to https://github.com/innostory for reporting an issue
            // where a dangling reference to 'other' resulted in memory leaks.
            void Action(ITransient tr)
            {
                other.OnDisposed -= Action;
                Suspend();
            }

            other.OnDisposed += Action;

            return this;
        }

        public IGenerator ResumeAfter(Func<bool> pred)
        {
            // resume after pred is true
            var @while = Factory.While(() => !pred()).AddTo(Kernel.Root);
            return ResumeAfter(@while);
        }

        public IGenerator ResumeAfter(ITransient other)
        {
            if (IsNullOrInactive(other))
            {
                Resume();
                return this;
            }

            Suspend();

            // thanks to https://github.com/innostory for reporting an issue
            // where a dangling reference to 'other' resulted in memory leaks.
            void OnCompleted(ITransient tr)
            {
                other.OnDisposed -= OnCompleted;
                Resume();
            }

            other.OnDisposed += OnCompleted;

            return this;
        }

        public IGenerator ResumeAfter(TimeSpan span)
        {
            if (!Active)
                return this;

            var timer = Factory.OneShotTimer(span);
            Kernel.Root.Add(timer);
            return ResumeAfter(timer);
        }

        public IGenerator SuspendAfter(TimeSpan span)
        {
            if (!Active)
                return this;

            var timer = Factory.OneShotTimer(span);
            Kernel.Root.Add(timer);
            return ResumeAfter(timer);
        }
    }

    public delegate void WhyTypedGeneratorCompleted<in TResult>(IGenerator<TResult> self);

    public class Generator<TResult>
        : Generator
        , IGenerator<TResult>
    {
        public new TResult Value
        {
            get => (TResult)base.Value;
            set => base.Value = value;
        }

        protected static void CannotStart()
        {
            throw new Exception("Can't start typed gen");
        }
    }
}
