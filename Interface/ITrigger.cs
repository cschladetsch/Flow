// (C) 2012 christian.schladetsch@gmail.com

namespace Flow
{
	/// <summary>
	/// Trigger handler.
	/// </summary>
	public delegate void TriggerHandler(ITrigger trigger, ITransient reason);

	/// <summary>
	/// A trigger Deletes itself when any of the objects in it are deleted
	/// </summary>
	public interface ITrigger : IGroup
	{
		/// <summary>
		/// Occurs when any of the objects added to the trigger are deleted.
		/// </summary>
		event TriggerHandler Tripped;
	}
}