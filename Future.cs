// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow.Impl
{
	internal class Future<T> : Transient, IFuture<T>
	{
		private T _value;

		public event FutureHandler<T> Arrived;

		public bool Available { get; private set; }

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

				Complete();
			}
		}
	}
}
