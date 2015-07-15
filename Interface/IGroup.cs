// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	///     Delegate for events related to a group and a child of the group.
	/// </summary>
	public delegate void GroupHandler(IGroup node, ITransient child);

	/// <summary>
	///     A Group contains a collection of other Transients, and fires events when the contents of the group changes.
	///     <para>When a Group is suspended, all child generators are also suspended.</para>
	///     <para>When a Group is resumed, all child generators are also resumed</para>
	/// </summary>
	public interface IGroup : IGenerator
	{
		/// <summary>
		///     Gets the contents of this group.
		/// </summary>
		/// <value>
		///     An enumerable over the contents of this group.
		/// </value>
		IEnumerable<ITransient> Contents { get; }

		/// <summary>
		///     Occurs when a transient is added to this group.
		/// </summary>
		event GroupHandler Added;

		/// <summary>
		///     Occurs when a transient is removed from this group.
		/// </summary>
		event GroupHandler Removed;

		/// <summary>
		///     Add the specified transient to this group if it is not already a member of this group.
		/// </summary>
		/// <param name='trans'>
		///     The transient to add to this group
		/// </param>
		void Add(ITransient trans);

		/// <summary>
		///     Remove the specified transient from this group.
		/// </summary>
		/// <param name='trans'>
		///     The transient to remove from this group
		/// </param>
		void Remove(ITransient trans);
	}
}
