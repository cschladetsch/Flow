using System;
using System.Collections.Generic;

namespace Flow.Impl
{
	/// <summary>
	/// A thread-safe channel of values.
	/// </summary>
	internal class BlockingChannel<TR> : Subroutine<bool>, IChannel<TR>
	{
		private readonly object _mutex = new object();

		private readonly Queue<IFuture<TR>> _requests = new Queue<IFuture<TR>>();

		private readonly Queue<TR> _values = new Queue<TR>();

		internal BlockingChannel(IKernel kernel)
		{
			Sub = StepChannel;
			Completed += tr => Close();
		}

		internal BlockingChannel(IKernel kernel, IGenerator<TR> gen)
			: this(kernel)
		{
			gen.Stepped += g => Insert(gen.Value);
			CompleteAfter(gen);
		}

		public IFuture<TR> Extract()
		{
			lock (_mutex)
			{
				IFuture<TR> future = Factory.Future<TR>();
				_requests.Enqueue(future);
				return future;
			}
		}

		public List<TR> ExtractAll()
		{
			throw new NotImplementedException();
		}

		public void Insert(TR val)
		{
			lock (_mutex)
			{
				_values.Enqueue(val);
			}
		}

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

		internal void Close()
		{
			lock (_mutex)
			{
				Flush();

				foreach (var f in _requests)
				{
					f.Complete();
				}
			}
		}

		private bool StepChannel(IGenerator self)
		{
			++StepNumber;

			if (!Active)
				return true;

			Flush();

			return true;
		}
	}
}
