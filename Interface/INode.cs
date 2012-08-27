// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	/// A flow Node is a Group that steps all referenced Generators when it itself is Stepped
	/// </summary>
	public interface INode : IGroup
	{
	}
}
