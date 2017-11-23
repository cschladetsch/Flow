// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	public interface ICoroutine : IGenerator
	{
	}

	public interface ICoroutine<out T> : IGenerator<T>
	{
	}
}
