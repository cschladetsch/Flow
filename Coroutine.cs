// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	/// <inheritdoc />
	internal class Coroutine<TR> : Generator<TR>, ICoroutine<TR>
	{
		/// <inheritdoc />
		public override void Step ()
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

		void CannotStart ()
		{
			throw new Exception("Coroutine cannot start");
		}

		private IEnumerator<TR> _state;

		internal Func<IEnumerator<TR>> Start;
	}
}