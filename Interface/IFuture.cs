// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	public delegate void FutureHandler<T>(IFuture<T> future);

	/// <summary>
	///     When its Value property is first set, a Future fires its Arrived event, sets its Available property to true, and
	///     Completes itself.
	/// </summary>
	public interface IFuture<T> : ITransient
	{
		bool Available { get; }
		T Value { get; set; }
		event FutureHandler<T> Arrived;
	}
}
