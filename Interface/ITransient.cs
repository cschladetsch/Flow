// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	///     General delegate for dealing with events created by an ITransient instance.
	/// </summary>
	public delegate void TransientHandler(ITransient sender);

	/// <summary>
	///     Transient handler reason.
	/// </summary>
	public delegate void TransientHandlerReason(ITransient sender, ITransient reason);

	/// <summary>
	///     A Transient object notifies observers when it has been Completed. When a Transient is Completed,
	///     it has no more work to do and its internal state will not change without external influence.
	///     flow-control.
	/// </summary>
	public interface ITransient : INamed
	{
		/// <summary>
		///     Gets the kernel that stores this Transient.
		/// </summary>
		/// <value>
		///     The kernel.
		/// </value>
		IKernel Kernel { get; /*internal*/ set; }

		/// <summary>
		///     Gets the factory that made this Transient
		/// </summary>
		/// <value>
		///     The factory.
		/// </value>
		IFactory Factory { get; }

		/// <summary>
		///     Gets a value indicating whether this <see cref="Flow.ITransient" /> is still active and has not been Completed.
		/// </summary>
		/// <value>
		///     True if this ITransient instance has not been Complete()'d.
		/// </value>
		bool Active { get; }

		/// <summary>
		///     Occurs when the Complete method is first called. Successive calls to Complete will do nothing.
		/// </summary>
		event TransientHandler Completed;

		/// <summary>
		///     Occurs when completed, with a reason why.
		/// </summary>
		event TransientHandlerReason WhyCompleted;

		/// <summary>
		///     Complete this instance and fire the Completed event iff it has not already been Completed.
		/// </summary>
		void Complete();

		/// <summary>
		///     Ensure that this instance is Complete()'d after the given other transient is Completed.
		/// </summary>
		/// <param name='other'>
		///     Another transient that is stopping this transient from being Complete()'d.
		/// </param>
		void CompleteAfter(ITransient other);

		/// <summary>
		///     Completes this Transient after a period of time
		/// </summary>
		/// <param name='span'>
		///     The time to wait before deleting this Transient.
		/// </param>
		void CompleteAfter(TimeSpan span);
	}
}
