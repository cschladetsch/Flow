// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

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
			var level = ELogLevel.Verbose;

			Trace = new Logger.Logger(level, "Kernel");
#if UNITY
			Trace.AddLogger(new UnityLogger(level));
#endif

#if DOTNET
			Trace.AddLogger(new ConsoleLogger(level));
#endif

			_time.Now = DateTime.Now;
			_time.Last = _time.Now;
			_time.Delta = TimeSpan.FromSeconds(0);
		}

		public ITimeFrame Time
		{
			get { return _time; }
		}

		public void WaitSteps(int numSteps)
		{
			throw new NotImplementedException();
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
			if (!IsNullOrInactive(Root))
				Root.Step();

			base.Step();
		}

		private void StepTime()
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
