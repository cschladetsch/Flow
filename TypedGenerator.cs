// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <inheritdoc />
	internal abstract class TypedGenerator<TR> : Generator, ITypedGenerator<TR>
	{
		public TR Value { get; protected set; }
	}
}
