// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	internal class Future<T> : Transient, IFuture<T>
	{
		/// <inheritdoc />
		public event FutureHandler<T> Arrived;

		/// <inheritdoc />
		public ITimer Timer { get; internal set; }

		/// <inheritdoc />
		public bool Available { get; private set; }

		/// <inheritdoc />
		public bool HasTimedOut { get; protected set; }

		/// <inheritdoc />
		public T Value 
		{
			get 
			{
				if (!Available)
					throw new FutureNotSetException();
				return _value;
			}
			set 
			{
				if (Available)
					throw new FutureAlreadySetException();
				_value = value;
				Available = true;
				if (Arrived != null)
					Arrived(this);
				Delete();
			}
		}

		private T _value;
	}
}