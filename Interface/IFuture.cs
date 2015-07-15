// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	///     Delegate for events that deal with Future values.
	/// </summary>
	public delegate void FutureHandler<T>(IFuture<T> future);

	/// <summary>
	///     When its Value property is first set, a Future fires its Arrived event, sets its Available property to true, and
	///     Completes itself.
	/// </summary>
	public interface IFuture<T> : ITransient
	{
		/// <summary>
		///     Gets a value indicating whether this Future is available.
		/// </summary>
		/// <value>
		///     <c>true</c> if available; otherwise, <c>false</c>.
		/// </value>
		bool Available { get; }

		/// <summary>
		///     Gets or sets the value of the future. It is an error to set this twice.
		/// </summary>
		/// <value>
		///     The value of the future.
		/// </value>
		T Value { get; set; }

		/// <summary>
		///     Occurs after the Value has been set
		/// </summary>
		event FutureHandler<T> Arrived;
	}
}
