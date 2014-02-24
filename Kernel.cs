// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	internal class Kernel : TypedGenerator<bool>, IKernel
	{
		/// <inheritdoc />
		public INode Root { get; set; }

		/// <inheritdoc />
		public new IFactory Factory { get; internal set; }

		/// <inheritdoc />
		public ITimeFrame Time { get { return _time; } }

		internal Kernel()
		{
			_time.Now = DateTime.Now;
			_time.Last = _time.Now;
			_time.Delta = TimeSpan.FromSeconds(0);
		}

		/// <inheritdoc />
		public override void Step()
		{
			StepTime();

			if (IsNullOrEmpty(Root))
				return;

			Root.Step();
			Root.Post();
		}

		void StepTime()
		{
			var now = DateTime.Now;

			_time.Last = _time.Now;
			_time.Delta = now - _time.Last;
			_time.Now = now;
		}

		private readonly TimeFrame _time = new TimeFrame();
	}
}