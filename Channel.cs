// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	internal class Channel<TR> : Subroutine<bool>, IChannel<TR>
	{
		/// <inheritdoc />
		public IFuture<TR> Extract()
		{
			var future = Factory.NewFuture<TR>();
			_requests.Enqueue(future);
			return future;
		}

		/// <inheritdoc />
		public void Insert(TR val)
		{
			_values.Enqueue(val);
		}

		/// <inheritdoc />
		public void Flush()
		{
			while (_values.Count > 0 && _requests.Count > 0)
			{
				_requests.Dequeue().Value = _values.Dequeue();
			}
		}

		internal Channel(IKernel kernel)
		{
			Sub = StepChannel;
			Completed += (tr) => Close();
		}

		internal void Close()
		{
			Flush();

			foreach (var f in _requests)
				f.Complete();
		}

		internal Channel(IKernel kernel, ITypedGenerator<TR> gen)
			: this(kernel)
		{
			gen.Stepped += g => Insert(gen.Value);
			CompleteAfter(gen);
		}

		bool StepChannel(IGenerator self)
		{
			if (!Active)
				return true;

			Flush();

			return true;
		}

		readonly Queue<TR> _values = new Queue<TR>();

		readonly Queue<IFuture<TR>> _requests = new Queue<IFuture<TR>>();
	}
}