// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// A Node is a Group that steps all referenced Generators when it itself is Stepped, and similarly for Pre and Post.
	public interface INode : IGroup
	{
		void Add(params IGenerator[] gens);
	}
}
