// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	internal class Kernel : TypedGenerator<bool>, IKernel
	{
		private readonly TimeFrame _time = new TimeFrame();

		internal Kernel()
		{
			_time.Now = DateTime.Now;
			_time.Last = _time.Now;
			_time.Delta = TimeSpan.FromSeconds(0);
		}

		/// <inheritdoc />
		public INode Root { get; set; }

		/// <inheritdoc />
		public new IFactory Factory { get; internal set; }

		/// <inheritdoc />
		public ITimeFrame Time
		{
			get { return _time; }
		}

		public void Update(float dt)
		{
			var delta = TimeSpan.FromSeconds(dt);
			_time.Last = _time.Now;
			_time.Delta = delta;
			_time.Now = _time.Now + delta;

			Process();
		}

		/// <inheritdoc />
		public override void Step()
		{
			StepTime();

			if (IsNullOrInactive(Root))
				return;

			Process();
		}

		void Process()
		{
			Root.Step();
			Root.Post();
		}

		private void StepTime()
		{
			DateTime now = DateTime.Now;

			_time.Last = _time.Now;
			_time.Delta = now - _time.Last;
			_time.Now = now;
		}
	}
}
