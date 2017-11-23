// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// A channel is a buffered input/output stream.
	/// <para>If a channel is created with a TypedGenerator, that is used as the source of the channel</para>
	/// </summary>
	public interface IChannel<T> : ITransient
	{
		void Insert(T val);
		IFuture<T> Extract();
		List<T> ExtractAll();
		void Flush();
	}
}
