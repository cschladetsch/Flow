// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	/// An instance of IKernel contains a top-level root node, which contains all the transients in the IKernel.
	/// <para>
	/// When the Kernel is Stepped, it Steps the top-level Root node.
	/// </para>
	/// <para>
	/// After that, it calls Post on the top-level Root node.
	/// </para>
	/// </summary>
	public interface IKernel : IGenerator
	{
		/// <summary>
		/// Gets or sets the root of the kernel. When the Kernel is stepped, it will first Step() every generator reachable from Root, 
		/// then call Stepped() on all nodes reachable from the Root.
		/// </summary>
		/// <value>
		/// The root group.
		/// </value>
		INode Root { get; set; }

		/// <summary>
		/// Gets the time to use for this update.
		/// </summary>
		/// <value>
		/// The time to use for this udpate
		/// </value>
		ITimeFrame Time { get; }
	}
}