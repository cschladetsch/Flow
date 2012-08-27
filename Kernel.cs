using System;

namespace Flow
{
    internal class Kernel : Generator<bool>, IKernel
    {
        public INode Root { get; set; }

		public new IFactory Factory { get; internal set; }

		public ITimeFrame Time { get { return _time; } }

		internal Kernel()
		{
			_time.Now = DateTime.Now;
			_time.Last = _time.Now;
			_time.Delta = TimeSpan.FromSeconds(0);
		}

        public override bool Step()
        {
			StepTime();

			if (Root == null)
				return false;

			var result = Root.Step();

			Root.Post();

			return result;
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