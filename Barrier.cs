// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <inheritdoc />
	internal class Barrier : Group, IBarrier
	{
		/// <inheritdoc />
		public override void Post()
		{
			base.Post();

			// do nothing if we have any contents
			foreach (var elem in Contents) 
				return;

			// if there is nothing pending to add, we are done
			if (_adds.Count == 0)
				Complete();
		}
	}
}