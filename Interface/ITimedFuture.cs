// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	public interface ITimedFuture<T> : IFuture<T>, ITimesOut
	{
	}

	public interface ITimedBarrier : IBarrier, ITimesOut
	{
	}

	public interface ITimedTrigger : ITrigger, ITimesOut
	{
	}

	public interface ITimedNode : INode, ITimesOut
	{
	}
}
