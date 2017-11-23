// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow.Impl
{
	internal class Subroutine<TR> : Generator<TR>, ISubroutine<TR>
	{
		internal Func<IGenerator, TR> Sub;

		public override void Step()
		{
			if (!Active || !Running)
				return;

			if (Sub == null)
			{
				Complete();
				return;
			}

			_value = Sub(this);

			base.Step();
		}
	}
}
