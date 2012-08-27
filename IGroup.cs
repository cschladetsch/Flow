using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// Group handler.
	/// </summary>
	public delegate void GroupHandler(IGroup node, ITransient child);

	/// <summary>
	/// A flow group contains a collection of other transients, and fires events when the contents of the group changes.
	/// <para>
	/// TODO: When the group is suspnded, all child generators are suspended, and conversely.
	/// </para>
	/// </summary>
    public interface IGroup : IGenerator
    {
		/// <summary>
		/// Occurs when a transient is added to this group.
		/// </summary>
		event GroupHandler Added;

		/// <summary>
		/// Occurs when a transient is removed from this group.
		/// </summary>
		event GroupHandler Removed;
     
		/// <summary>
		/// Gets the contents of this group.
		/// </summary>
		/// <value>
		/// An enumerable over the contents of this group.
		/// </value>
		IEnumerable<ITransient> Contents { get; }

		/// <summary>
		/// Add the specified transient to this group if it is not already a member of this group.
		/// </summary>
		/// <param name='trans'>
		/// The transient to add to this group
		/// </param>
		void Add(ITransient trans);
        
		/// <summary>
		/// Remove the specified transient from this group.
		/// </summary>
		/// <param name='trans'>
		/// The transient to remove from this group
		/// </param>
        void Remove(ITransient trans);
    }
}
