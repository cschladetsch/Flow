namespace Flow
{
	/// <summary>
	/// A trigger is a group that deletes itself when any of its children are deleted
	/// </summary>
	internal class Trigger : Group, ITrigger
	{
		public event TriggerHandler Tripped;

		internal Trigger()
		{
			Removed += Trip;
		}

		void Trip(IGroup self, ITransient other)
		{
 			if (Tripped != null)
				Tripped(this, other);

			Delete();
		}
	}
}