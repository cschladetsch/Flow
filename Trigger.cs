// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	/// A trigger is a group that deletes itself when any of its children are deleted
	/// </summary>
	internal class Trigger : Group, ITrigger
	{
		/// <inheritdoc />
		public event TriggerHandler Tripped;

		/// <inheritdoc />
		public ITransient Reason { get; private set; }

		internal Trigger()
		{
			// when an element is removed from this trigger, it fires
			Removed += Trip;
		}

		void Trip(IGroup self, ITransient other)
		{
			Reason = other;

 			if (Tripped != null)
				Tripped(this, other);

			Removed -= Trip;

			Complete();
		}
	}
}