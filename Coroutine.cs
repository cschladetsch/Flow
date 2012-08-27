// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	/// <inheritdoc />
	internal class Coroutine<TR> : Generator<TR>, ICoroutine<TR>
	{
		internal Coroutine()
		{
			// ensure that we transition through suspended state before deleting
			Deleted += tr => { if (Running) Suspend(); };
		}

		/// <inheritdoc />
		public override void Step ()
		{
			if (!Running || !Exists)
				return;

			if (_enumerator == null) 
			{
				if (Start == null)
					CannotStart();
			
				_enumerator = Start();

				if (_enumerator == null)
					CannotStart();
			}

			if (!_enumerator.MoveNext())
			{
				Delete();
				return;
			}

			Value = _enumerator.Current;

			base.Step();
		}

		void CannotStart ()
		{
			throw new Exception("Coroutine cannot start");
		}

		private IEnumerator<TR> _enumerator;

		internal Func<IEnumerator<TR>> Start;
	}
}