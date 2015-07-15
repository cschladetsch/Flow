using System;
using System.Collections;

namespace Flow
{
	internal class Coroutine : Generator, ICoroutine
	{
		internal Func<IEnumerator> Start;

		private IEnumerator _state;

		public object Value { get; private set; }

		public override void Step()
		{
			if (!Running || !Active)
				return;

			if (_state == null)
			{
				if (Start == null)
					CannotStart();

				_state = Start();
				if (_state == null)
					CannotStart();
			}

			if (!_state.MoveNext())
			{
				Complete();
				return;
			}

			Value = _state.Current;
			base.Step();
		}

		private void CannotStart()
		{
			throw new Exception("TypedCoroutine cannot start");
		}
	}
}
