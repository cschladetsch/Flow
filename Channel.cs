// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Collections.Generic;

namespace Flow.Impl
{
	internal class Channel<TR> : Subroutine<bool>, IChannel<TR>
	{
		private readonly Queue<IFuture<TR>> _requests = new Queue<IFuture<TR>>();

		private readonly Queue<TR> _values = new Queue<TR>();

		internal Channel(IKernel kernel)
		{
			Sub = StepChannel;
			Completed += tr => Close();
		}

		internal Channel(IKernel kernel, IGenerator<TR> gen)
			: this(kernel)
		{
			gen.Stepped += g => Insert(gen.Value);
			CompleteAfter(gen);
		}

		public IFuture<TR> Extract()
		{
			IFuture<TR> future = Factory.Future<TR>();
			_requests.Enqueue(future);
			return future;
		}

		public List<TR> ExtractAll()
		{
			Flush();

			var list = new List<TR>();
			while (_values.Count > 0)
			{
				list.Add(_values.Dequeue());
			}

			return list;
		}

		public void Insert(TR val)
		{
			_values.Enqueue(val);
		}

		public void Flush()
		{
			while (_values.Count > 0 && _requests.Count > 0)
			{
				_requests.Dequeue().Value = _values.Dequeue();
			}
		}

		internal void Close()
		{
			Flush();

			foreach (var f in _requests)
			{
				f.Complete();
			}
		}

		private bool StepChannel(IGenerator self)
		{
			Flush();

			return true;
		}
	}
}
