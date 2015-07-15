// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Linq;

namespace Flow
{
	/// <inheritdoc />
	internal class Barrier : Group, IBarrier
	{
		/// <inheritdoc />
		public override void Post()
		{
			base.Post();

			if (Contents.Any(t => t.Active))
				return;

			if (Additions.Count == 0)
				Complete();
		}
	}
}
