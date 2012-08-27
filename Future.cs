using System;
using System.Collections.Generic;

namespace Flow
{
	internal class Future<T> : Transient, IFuture<T>
	{
		public event FutureHandler<T> Arrived;

		public ITimer Timer { get; internal set; }

		public bool Available { get; private set; }

		public bool HasTimedOut { get; protected set; }

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