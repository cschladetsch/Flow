// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	internal class Subroutine<TR> : TypedGenerator<TR>, ISubroutine<TR>
	{
		internal Func<IGenerator, TR> Sub;

		/// <inheritdoc />
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
