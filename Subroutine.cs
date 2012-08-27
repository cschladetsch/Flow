// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	internal class Subroutine<TR> : Generator<TR>, ISubroutine<TR>
	{
		/// <inheritdoc />
		public override bool Step ()
		{
			if (!Exists || !Running)
				return false;

			if (Sub == null) 
			{
				Delete();
				return false;
			}

			Value = Sub(this);

			return base.Step();
		}

		internal Func<IGenerator, TR> Sub;
	}
}