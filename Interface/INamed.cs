// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	public delegate void NamedHandler(INamed named, string newName, string oldName);

	/// Fires its NewName event when its Name property is changed.
	public interface INamed
	{
		event NamedHandler NewName;
		string Name { get; set; }
	}
}
