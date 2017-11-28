using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow.Impl
{
	internal class Coroutine : Generator, ICoroutine
	{
		public override object Value {
			get { return _value; }
		}

		private object _value;

		public override void Step()
		{
			if (!Running || !Active)
				return;

			if (_state == null)
			{
				if (Start == null)
					CannotStart();
				else
					_state = Start();

				if (_state == null)
					CannotStart();
			}

			if (_state == null || !_state.MoveNext())
			{
				Complete();
				return;
			}

			_value = _state.Current;

			base.Step();
		}

		protected static void CannotStart()
		{
			throw new Exception("Coroutine cannot start");
		}

		internal Func<IEnumerator> Start;

		protected IEnumerator _state;
	}

	internal class Coroutine<T> : Generator<T>, ICoroutine<T>
	{
		public override void Step()
		{
			if (!Running || !Active)
				return;

			if (_state == null)
			{
				if (Start == null)
					CannotStart();
				else
					_state = Start();

				if (_state == null)
					CannotStart();
			}

			if (_state == null || !_state.MoveNext())
			{
				Complete();
				return;
			}

			Value = _state.Current;

			base.Step();
		}

		internal Func<IEnumerator<T>> Start;
		private IEnumerator<T> _state;
	}
}
