using System;

namespace Flow
{
	/// <summary>
	/// General delegate for dealing with events created by an ITransient instance.
	/// </summary>
    public delegate void TransientHandler(ITransient sender);

	/// <summary>
	/// A transient object notifies observers when it has been Delete()'d. Note that this is 
	/// distinct from the notion of garbage collection in C#: Specifically, the object can still exist
	/// and be indirected in the runtime after it has had its Delete() method called. Delete() is purely used for 
	/// flow-control.
	/// </summary>
    public interface ITransient : INamed
    {
		/// <summary>
		/// Occurs when the Delete() method is first called. Successive calls to Delete() will do nothing.
		/// </summary>
        event TransientHandler Deleted;

		/// <summary>
		/// Gets the kernel that stores this generator.
		/// </summary>
		/// <value>
		/// The kernel.
		/// </value>
		IKernel Kernel { get; }

		/// <summary>
		/// Gets the factory.
		/// </summary>
		/// <value>
		/// The factory.
		/// </value>
		IFactory Factory { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Flow.ITransient"/> exists.
		/// </summary>
		/// <value>
		/// True if this ITransient instance has not been Delete()'d.
		/// </value>
        bool Exists { get; }

		/// <summary>
		/// Delete this instance iand fire the Deleted event iff it has not already been Delete()'d.
		/// </summary>
        void Delete();

		/// <summary>
		/// Ensure that this instance is Delete()'d after the given other transient is Delete()'d.
		/// </summary>
		/// <param name='other'>
		/// Another transient that is stopping this transient from being Delete()'d.
		/// </param>
        void DeleteAfter(ITransient other);

    }
}