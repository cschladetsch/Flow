// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	internal class Subroutine<TR> : Generator<TR>, ISubroutine<TR>
	{
		/// <inheritdoc />
		public override void Step ()
		{
			if (!Active || !Running)
				return;

			if (Sub == null) 
			{
				Complete();
				return;
			}

			Value = Sub(this);

			base.Step();
		}

		internal Func<IGenerator, TR> Sub;
	}
}