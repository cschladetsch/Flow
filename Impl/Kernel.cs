// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl {
    using System;

    public class Kernel
        : Generator<bool>
        , IKernel {
        public EDebugLevel DebugLevel { get; set; }
        public ILogger Log { get; set; }
        public INode Root { get; set; }
        public new IFactory Factory { get; internal set; }
        public bool Break { get; private set; }
        public ITimeFrame Time => _time;

        private bool _waiting;
        private DateTime _resumeTime;
        private readonly TimeFrame _time = new TimeFrame();

        internal Kernel() {
            Log = this;
            Log.LogSubject = this;
            Log.LogPrefix = "Flow";
            Verbosity = 5;
            DebugLevel = EDebugLevel.Medium;
            ShowStack = false;
            ShowSource = true;
            Kernel = this;

            _time.Now = DateTime.Now;
            _time.Last = _time.Now;
            _time.Delta = TimeSpan.FromSeconds(0);
        }

        public void BreakFlow()
            => Break = true;

        public void Wait(TimeSpan span) {
            if (_waiting) {
                _resumeTime += span;
                return;
            }

            _resumeTime = _time.Now + span;
            _waiting = true;
        }

        public void Update(float dt) {
            UpdateTime(dt);

            Process();
        }

        private void UpdateTime(float dt) {
            var delta = TimeSpan.FromSeconds(dt);
            _time.Last = _time.Now;
            _time.Delta = delta;
            _time.Now = _time.Now + delta;
        }

        public override void Step() {
            StepTime();

            if (_waiting) {
                if (_time.Now > _resumeTime) {
                    _resumeTime = DateTime.MinValue;
                    _waiting = false;
                } else
                    return;
            }

            Process();
        }

        private void Process() {
            if (Break)
                return;

            void Step(IGenerator node) {
                if (!IsNullOrInactive(node))
                    node.Step();
            }

            if (Root.Contents.Count > 0)
                Verbose(10, $"Stepping kernel {Root.Contents.Count}");

            Step(Root);

            base.Step();
        }

        public void StepTime() {
            var now = DateTime.Now;

            _time.Last = _time.Now;
            _time.Delta = now - _time.Last;
            _time.Now = now;
        }
    }
}

