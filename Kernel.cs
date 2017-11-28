// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using Flow.Logger;

namespace Flow.Impl
{
	public class Kernel : Generator<bool>, IKernel
	{
		private readonly TimeFrame _time = new TimeFrame();

		internal Kernel()
		{
			Trace = new Logger.Logger(ELogLevel.Verbose, "Kernel");
			#if UNITY
			Trace.AddLogger(new UnityLogger(ELogLevel.Verbose));
			#endif // using UnityEngine;
			
			_time.Now = DateTime.Now;
			_time.Last = _time.Now;
			_time.Delta = TimeSpan.FromSeconds(0);
		}

		public EDebugLevel DebugLevel { get; set; }
		public Logger.ILogger Trace { get; set; }
		
		public INode Root { get; set; }

		public new IFactory Factory { get; internal set; }

		public ITimeFrame Time
		{
			get { return _time; }
		}

		public void Wait(DateTime end)
		{
			throw new NotImplementedException("Kernel.Wait");
		}

		public void Update(float dt)
		{
			var delta = TimeSpan.FromSeconds(dt);
			_time.Last = _time.Now;
			_time.Delta = delta;
			_time.Now = _time.Now + delta;

			Process();
		}

		public override void Step()
		{
			StepTime();

			if (IsNullOrInactive(Root))
				return;

			Process();
		}

		private void Process()
		{
			Root.Step();
		}

		private void StepTime()
		{
			var now = DateTime.Now;

			_time.Last = _time.Now;
			_time.Delta = now - _time.Last;
			_time.Now = now;
		}
	}
}
