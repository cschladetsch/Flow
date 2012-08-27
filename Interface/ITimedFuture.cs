// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// A timed future.
	/// </summary>
	public interface ITimedFuture<T> : IFuture<T>, ITimesOut
	{
	}

	/// <summary>
	/// A timed future.
	/// </summary>
	public interface ITimedBarrier : IBarrier, ITimesOut
	{
	}

	/// <summary>
	/// A timed Trigger.
	/// </summary>
	public interface ITimedTrigger : ITrigger, ITimesOut
	{
	}

	/// <summary>
	/// A timed Node.
	/// </summary>
	public interface ITimedNode : INode, ITimesOut
	{
	}
}
