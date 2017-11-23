// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	public delegate void TriggerHandler(ITrigger trigger, ITransient reason);

	public interface ITrigger : IGroup
	{
		event TriggerHandler Tripped;

		ITransient Reason { get; }
	}
}
