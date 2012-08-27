// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// A channel is a buffered input/output stream.
	/// <para>If a channel is created with a TypedGenerator, that is used as the source of the channel</para>
	/// </summary>
	public interface IChannel<TR> : ITransient
	{
		/// <summary>
		/// Gets a generator used to insert values into the stream. Only valid if the channel was created using IFactory.NewChannel(ITypedGenerator).
		/// <para>When the generator is stepped, its new value is inserted into this channel</para>
		/// </summary>
		/// <value>
		/// The generator.
		/// </value>
		ITypedGenerator<TR> Generator { get; }

		/// <summary>
		/// Gets the next future value from the channel
		/// </summary>
		/// <value>
		/// A future output of the channel
		/// </value>
		IFuture<TR> Extract { get; }

		/// <summary>
		/// Insert the specified value into the channel
		/// </summary>
		/// <param name='val'>
		/// The value to insert into the channel
		/// </param>
		void Insert(TR val);

		/// <summary>
		/// Service as many pending read requests as possible
		/// </summary>
		void Flush();
	}
}