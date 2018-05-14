// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Diagnostics;
using Flow.Logger;

namespace Flow.Impl
{
    public class Kernel : Generator<bool>, IKernel
    {
        public EDebugLevel DebugLevel { get; set; }
        public Logger.ILogger Trace { get; set; }
        public INode Root { get; set; }
        public new IFactory Factory { get; internal set; }

        internal Kernel()
        {
            var level = ELogEntryType.Everything;

            Trace = new Logger.Logger(level, "Kernel");
#if UNITY3D
			Trace.AddLogger(new UnityLogger(eLevel));
#endif

#if DOTNET
			Trace.AddLogger(new ConsoleLogger(entryType));
#endif

            _time.Now = DateTime.Now;
            _time.Last = _time.Now;
            _time.Delta = TimeSpan.FromSeconds(0);
        }

        public ITimeFrame Time => _time;

        public void WaitSteps(int numSteps)
        {
            throw new NotImplementedException();
        }

        public bool Break { get; private set; }

        public void BreakFlow()
        {
            Break = true;
        }

        public void ContinueFlow()
        {
            Break = false;
        }

        public void Wait(TimeSpan span)
        {
            if (_waiting)
            {
                _resumeTime += span;
                return;
            }

            _resumeTime = _time.Now + span;
            _waiting = true;
        }

        public void Update(float dt)
        {
            UpdateTime(dt);

            Process();
        }

        private void UpdateTime(float dt)
        {
            var delta = TimeSpan.FromSeconds(dt);
            _time.Last = _time.Now;
            _time.Delta = delta;
            _time.Now = _time.Now + delta;
        }

        public override void Step()
        {
            StepTime();

            if (_waiting)
            {
                if (_time.Now > _resumeTime)
                {
                    _resumeTime = DateTime.MinValue;
                    _waiting = false;
                }
                else
                    return;
            }

            Process();
        }

        private void Process()
        {
            if (Break)
                return;

            if (!IsNullOrInactive(Root))
                Root.Step();

            base.Step();
        }

        public void StepTime()
        {
            var now = DateTime.Now;

            _time.Last = _time.Now;
            _time.Delta = now - _time.Last;
            _time.Now = now;
        }

        private bool _waiting;
        private DateTime _resumeTime;
        private readonly TimeFrame _time = new TimeFrame();
    }
}
