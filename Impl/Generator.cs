using System;

namespace Flow.Impl {
    public class Generator
        : Transient
        , IGenerator {
        public event GeneratorHandler Suspended;
        public event GeneratorHandler Resumed;
        public event GeneratorHandler Stepped;

        public virtual object Value { get; protected set; }

        public bool Running { get; protected set; }

        public int StepNumber { get; protected set; }

        public Generator() =>
            Completed += tr => Suspend();

        public new IGenerator AddTo(IGroup group) {
            group?.Add(this);
            return this;
        }

        public new IGenerator Named(string name) {
            Name = name;
            return this;
        }

        public virtual void Step() {
            if (!Active)
                return;

            ++StepNumber;
            Stepped?.Invoke(this);
        }

        public virtual void Pre() {
        }

        public virtual void Post() {
        }

        public void Suspend() {
            if (!Running)
                return;

            Running = false;
            Suspended?.Invoke(this);
        }

        public void Resume() {
            if (Running || !Active)
                return;

            Running = true;
            Resumed?.Invoke(this);
        }

        public IGenerator SuspendAfter(ITransient other) {
            if (IsNullOrInactive(other)) {
                Suspend();
                return this;
            }

            Resume();

            void SuspendThis(ITransient tr) {
                other.Completed -= SuspendThis;
                Suspend();
            }

            other.Completed += SuspendThis;

            return this;
        }

        public IGenerator ResumeAfter(Func<bool> pred, string name) {
            ITransient transient = Factory.While(() => !pred()).AddTo(Kernel.Root).Named(name);
            return ResumeAfter(transient);
        }

        public IGenerator ResumeAfter(Func<bool> pred) {
            return ResumeAfter(pred, pred.ToString());
        }

        public IGenerator ResumeAfter(ITransient other) {
            //Verbosity = 100;
            if (IsNullOrInactive(other)) {
                Verbose(10, $"<color=blue>Gen: </color><b>{other.Name}</b> already complete, resuming <b>{Name}</b>.");
                Resume();
                return this;
            }

            Verbose(10, $"<color=blue>Gen: </color>Suspending <b>{Name}</b> until after <b>{other.Name}</b>.");
            Suspend();

            // thanks to https://github.com/innostory for reporting an issue
            // where a dangling reference to 'other' resulted in memory leaks.
            void OnCompleted(ITransient tr) {
                other.Completed -= OnCompleted;
                Verbose(10, $"<color=blue>Gen: </color><b>{other.Name}</b> completed, resuming <b>{Name}</b>.");
                Resume();
            }

            other.Completed += OnCompleted;

            return this;
        }

        public IGenerator ResumeAfter(TimeSpan span) {
            if (!Active)
                return this;

            var timer = Factory.OneShotTimer(span);
            timer.Name = $"TimeSpan ({span})";
            Kernel.Root.Add(timer);
            return ResumeAfter(timer);
        }

        public IGenerator SuspendAfter(TimeSpan span) {
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
        , IGenerator<TResult> {
        public new TResult Value
        {
            get => (TResult)base.Value;
            set => base.Value = value;
        }

        protected static void CannotStart() {
            throw new Exception("Can't start typed gen");
        }
    }
}
