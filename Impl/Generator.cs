using System;

namespace Flow.Impl
{
    public class Generator : Transient, IGenerator
    {
        public event GeneratorHandler Suspended;
        public event GeneratorHandler Resumed;
        public event GeneratorHandler Stepped;

        public virtual object Value { get; protected set; }

        internal Generator()
        {
            Completed += tr => Suspend();
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
            Kernel.Log.Verbose(90, $"{Name}:{GetType().Name} Stepped #{StepNumber}");

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
            TransientHandler action = null;
            action = tr =>
            {
                other.Completed -= action;
                Suspend();
            };

            other.Completed += action;

            return this;
        }

        public IGenerator After(ITransient other)
        {
            return ResumeAfter(other);
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
            TransientHandler onCompleted = null;
            onCompleted = tr =>
            {
                other.Completed -= onCompleted;
                Resume();
            };

            other.Completed += onCompleted;

            return this;
        }

        public IGenerator ResumeAfter(TimeSpan span)
        {
            return !Active ? this : ResumeAfter(Factory.OneShotTimer(span));
        }

        public IGenerator SuspendAfter(TimeSpan span)
        {
            return !Active ? this : SuspendAfter(Factory.OneShotTimer(span));
        }
    }

    public delegate void WhyTypedGeneratorCompleted<TR>(Generator<TR> self);

    public class Generator<TR> : Generator, IGenerator<TR>
    {
        public new TR Value
        {
            get { return (TR)base.Value; }
            set { base.Value = value; }
        }

        public event WhyTypedGeneratorCompleted<TR> TypedCompleted;

        protected static void CannotStart()
        {
            throw new Exception("Can't start typed gen");
        }

        protected void InvokeTypedCompleted()
        {
            TypedCompleted?.Invoke(this);
        }
    }
}
