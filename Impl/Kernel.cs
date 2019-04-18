<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow.Impl
{
<<<<<<< HEAD
    public class Kernel
        : Generator<bool>
        , IKernel
=======
    public class Kernel : Generator<bool>, IKernel
>>>>>>> 2156678... Updated to .Net4.5
    {
        public EDebugLevel DebugLevel { get; set; }
        public ILogger Log { get; set; }
        public INode Root { get; set; }
        public new IFactory Factory { get; internal set; }
<<<<<<< HEAD
        public ITimeFrame Time => _time;

        private bool _waiting;
        private DateTime _resumeTime;
        private readonly TimeFrame _time = new TimeFrame();

        public bool Break { get; private set; }
=======
>>>>>>> 2156678... Updated to .Net4.5

        internal Kernel()
        {
            Log = this;
            Log.LogSubject = this;
            Log.LogPrefix = "Flow";
<<<<<<< HEAD
            Verbosity = 5;
            DebugLevel = EDebugLevel.Medium;
=======
            Verbosity = 999;
>>>>>>> 2156678... Updated to .Net4.5
            ShowStack = false;
            ShowSource = true;
            Kernel = this;

#if UNITY3D
			Log.AddLogger(new UnityLogger(eLevel));
#else
			//Log.AddLogger(new Logger.ConsoleLogger(entryType));
#endif

            _time.Now = DateTime.Now;
            _time.Last = _time.Now;
            _time.Delta = TimeSpan.FromSeconds(0);
        }

<<<<<<< HEAD
=======
        public ITimeFrame Time => _time;

        public void WaitSteps(int numSteps)
        {
            throw new NotImplementedException();
        }

        public bool Break { get; private set; }

>>>>>>> 2156678... Updated to .Net4.5
        public void BreakFlow()
        {
            Break = true;
        }

<<<<<<< HEAD
=======
        public void ContinueFlow()
        {
            Break = false;
        }

>>>>>>> 2156678... Updated to .Net4.5
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
<<<<<<< HEAD
=======

        private bool _waiting;
        private DateTime _resumeTime;
        private readonly TimeFrame _time = new TimeFrame();
>>>>>>> 2156678... Updated to .Net4.5
    }
}
