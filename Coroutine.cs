// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	internal class Coroutine<TR> : Generator<TR>, ICoroutine<TR>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Flow.Coroutine`1"/> class.
		/// </summary>
		public Coroutine()
		{
			// ensure that we transition through suspended state before deleting
			Deleted += tr => { if (Running) Suspend(); };
		}

		/// <summary>
		/// Execute the coroutine until it yields or ends.
		/// </summary>
		public override bool Step ()
		{
			if (!Running || !Exists)
				return false;

			if (_enumerator == null) 
			{
				if (Start == null)
					CannotStart();
			
				_enumerator = Start();

				if (_enumerator == null)
					CannotStart();
			}

			var stepped = _enumerator.MoveNext();
			if (!stepped)
			{
				Delete();
				return false;
			}

			Value = _enumerator.Current;

			return base.Step();
		}

		void CannotStart ()
		{
			throw new Exception("Coroutine cannot start");
		}

		private IEnumerator<TR> _enumerator;

		internal Func<IEnumerator<TR>> Start;
	}
}