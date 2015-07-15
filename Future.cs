// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	internal class Future<T> : Transient, IFuture<T>
	{
		private T _value;

		/// <inheritdoc />
		public event FutureHandler<T> Arrived;

		/// <inheritdoc />
		public bool Available { get; private set; }

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

				Complete();
			}
		}
	}
}
