// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using Flow.Logger;

namespace Flow
{
	/// <summary>
	///     A Kernel contains a top-level root Node, and a local TimeFrame.
	///     <para>
	///         When the Kernel is Stepped, it updates its Time property, Steps the top-level Root node, then calls Post on the
	///         top-level Root node.
	///     </para>
	/// </summary>
	public interface IKernel : IGenerator
	{
		EDebugLevel DebugLevel { get; set; }

		ILogger Trace { get; set; }

		/// <summary>
		///     Gets or sets the root of the kernel. When the Kernel is stepped, it will first Step() every generator reachable
		///     from Root,
		///     then call Stepped() on all nodes reachable from the Root.
		/// </summary>
		/// <value>
		///     The root group.
		/// </value>
		INode Root { get; set; }

		IFactory Factory { get; }

		/// <summary>
		///     Gets the time to use for this update.
		/// </summary>
		/// <value>
		///     The time to use for this update
		/// </value>
		ITimeFrame Time { get; }

		void Update(float deltaSeconds);

		void Wait(TimeSpan end);
		void WaitSteps(int numSteps);
	}

	public enum EDebugLevel
	{
		None,
		Low,
		Medium,
		High,
		Verbose,
	}
}
