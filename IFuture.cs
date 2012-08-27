using System;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// Delegate for events that deal with futures.
	/// </summary>
	public delegate void FutureHandler<T>(IFuture<T> future);

	/// <summary>
	/// A future value. When set, it fires its Arrived event and Delete()'s itself.
	/// </summary>
	public interface IFuture<T> : ITransient
	{
		/// <summary>
		/// Occurs when the value has been set for the first time. It is an exception to set the value more than once.
		/// </summary>
		event FutureHandler<T> Arrived;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Flow.IFuture`1"/> is available.
		/// </summary>
		/// <value>
		/// <c>true</c> if available; otherwise, <c>false</c>.
		/// </value>
		bool Available { get; }

		/// <summary>
		/// Gets or sets the value of the future. It is an error to set this twice.
		/// </summary>
		/// <value>
		/// The value of the future.
		/// </value>
		T Value { get; set; }
	}

	/// <summary>
	/// A timed future.
	/// </summary>
	public interface ITimedFuture<T> : IFuture<T>, ITimesOut
	{
	}
}
