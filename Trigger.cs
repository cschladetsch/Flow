// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow.Impl
{
	internal class Trigger : Group, ITrigger
	{
		public event TriggerHandler Tripped;

		public ITransient Reason { get; private set; }

		internal Trigger()
		{
			Removed += Trip;
		}

		private void Trip(IGroup self, ITransient other)
		{
			Reason = other;

			if (Tripped != null)
				Tripped(this, other);

			Complete();
		}
	}
}
