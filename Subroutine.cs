// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow.Impl
{
	internal class Subroutine : Generator, ISubroutine
	{
		internal Action<IGenerator> Sub;

		public override void Step()
		{
			if (!Active || !Running)
				return;

			if (Sub == null)
			{
				Complete();
				return;
			}

			Sub(this);

			base.Step();
		}
	}

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

			Value = Sub(this);

			base.Step();
		}
	}
}
