// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// Subroutine is a Generator, implemented as a direct method call.
	public interface ISubroutine : IGenerator
	{
	}

	public interface ISubroutine<out T> : IGenerator<T>
	{
	}
}
