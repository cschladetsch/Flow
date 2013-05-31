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

			// if there is nothing pending to add, we are done
			if (Additions.Count == 0 && _contents.Count == 0)
				Complete(); 
			
		}
	}
}