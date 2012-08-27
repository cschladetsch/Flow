// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	/// <inheritdoc />
	internal class Channel<TR> : Subroutine<bool>, IChannel<TR>
	{
		public ITypedGenerator<TR> Generator { get; private set; }

		public IFuture<TR> Extract 
		{
			get 
			{
				var future = Factory.NewFuture<TR>();
				_requests.Enqueue(future);
				return future;
			}
		}

		internal Channel(IKernel kernel)
		{
			Sub = StepChannel;
			Deleted += (tr) => Flush();
		}

		internal Channel(IKernel kernel, ITypedGenerator<TR> gen)
			: this(kernel)
		{
			Generator = gen;
			Generator.Stepped += GenStepped;

			DeleteAfter(Generator);
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

		void GenStepped(IGenerator gen)
		{
			Insert(Generator.Value);
		}

		bool StepChannel(IGenerator self)
		{
			Flush();
			return true;
		}

		readonly Queue<TR> _values = new Queue<TR>();

		readonly Queue<IFuture<TR>> _requests = new Queue<IFuture<TR>>();
	}
}