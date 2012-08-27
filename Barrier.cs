namespace Flow
{
	/// <summary>
	/// A barrier is a group that deletes itself when all added elements have been removed from it.
	/// <para>
	/// This is useful to block execution until one of several external events or conditions change.
	/// </para>
	/// </summary>
	internal class Barrier : Group, IBarrier
	{
		public override void Post()
		{
			base.Post();

			// do nothing if we have any contents
			foreach (var elem in Contents) 
				return;

			// if there is nothing pending to add, we are done
			if (_pendingAdds.Count == 0)
				Delete();
		}
	}
}