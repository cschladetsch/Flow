using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// A thread-safe channel of values.
	/// </summary>
	/// <typeparam name="TR">The type of objects that travel through the channel</typeparam>
	internal class BlockingChannel<TR> : Subroutine<bool>, IChannel<TR>
	{
		/// <inheritdoc />
		public IFuture<TR> Extract()
		{
			lock (_mutex)
			{
				var future = Factory.NewFuture<TR>();
				_requests.Enqueue(future);
				return future;
			}
		}

		/// <inheritdoc />
		public void Insert(TR val)
		{
			lock (_mutex)
			{
				_values.Enqueue(val);
			}
		}

		/// <inheritdoc />
		public void Flush()
		{
			lock (_mutex)
			{
				while (_values.Count > 0 && _requests.Count > 0)
				{
					_requests.Dequeue().Value = _values.Dequeue();
				}
			}
		}

		internal BlockingChannel(IKernel kernel)
		{
			Sub = StepChannel;
			Completed += tr => Close();
		}

		internal BlockingChannel(IKernel kernel, ITypedGenerator<TR> gen)
			: this(kernel)
		{
			gen.Stepped += g => Insert(gen.Value);
			CompleteAfter(gen);
		}

		internal void Close()
		{
			lock (_mutex)
			{
				Flush();

				foreach (var f in _requests)
					f.Complete();
			}
		}

		bool StepChannel(IGenerator self)
		{
			++StepNumber;

			if (!Active)
				return true;

			Flush();

			return true;
		}

		readonly Queue<TR> _values = new Queue<TR>();

		readonly Queue<IFuture<TR>> _requests = new Queue<IFuture<TR>>();

		private readonly object _mutex = new object();
	}
}