// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// Makes instances for the Flow library
	/// </summary>
	public class Factory : IFactory
	{
		/// <inheritdoc />
		public IKernel Kernel { get; internal set; }

		/// <inheritdoc />
		public ITimer NewTimer (TimeSpan interval)
		{
			var timer = new Timer(Kernel, interval);
			return Prepare(timer);
		}

		/// <inheritdoc />
		public IPeriodic NewPeriodicTimer (TimeSpan interval)
		{
			var timer = new Periodic(Kernel, interval);
			return Prepare(timer);
		}

		/// <inheritdoc />
		public IBarrier NewBarrier()
		{
			return Prepare(new Barrier());
		}

		/// <inheritdoc />
		public ITrigger NewTrigger()
		{
			return Prepare(new Trigger());
		}

		/// <inheritdoc />
		public ITimedTrigger NewTimedTrigger(TimeSpan span)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public IFuture<T> NewFuture<T>()
		{
			return Prepare(new Future<T>());
		}

		/// <inheritdoc />
		public ITimedFuture<T> NewTimedFuture<T> (TimeSpan interval)
		{
			return Prepare(new TimedFuture<T>(Kernel, interval));
		}

		/// <inheritdoc />
		public ISubroutine<TR> NewSubroutine<TR>(Func<IGenerator, TR> fun)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = (tr) => fun(sub);
			return Prepare(sub);
		}

		/// <inheritdoc />
		public ISubroutine<TR> NewSubroutine<TR, T0> (Func<IGenerator, T0, TR> fun, T0 t0)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = (tr) => fun(sub, t0);
			return Prepare(sub);
		}

		/// <inheritdoc />
		public ICoroutine<TR> NewCoroutine<TR>(Func<IGenerator, IEnumerator<TR>> fun)
		{
			var coro = new Coroutine<TR>();
			coro.Start = () => fun(coro);
			return Prepare(coro);
		}

		/// <inheritdoc />
		public ICoroutine<TR> NewCoroutine<TR, T0>(Func<IGenerator, T0, IEnumerator<TR>> fun, T0 t0)
		{
			var coro = new Coroutine<TR>();
			coro.Start = () => fun(coro, t0);
			return Prepare(coro);
		}

		/// <inheritdoc />
		public ICoroutine<TR> NewCoroutine<TR, T0, T1>(Func<IGenerator, T0, T1, IEnumerator<TR>> fun, T0 t0, T1 t1)
		{
			var coro = new Coroutine<TR>();
			coro.Start = () => fun(coro, t0, t1);
			return Prepare(coro);
		}

		/// <inheritdoc />
		public IChannel<TR> NewChannel<TR>(ITypedGenerator<TR> gen)
		{
			return Prepare(new Channel<TR>(Kernel, gen));
		}

		/// <inheritdoc />
		public IChannel<TR> NewChannel<TR>()
		{
			return Prepare(new Channel<TR>(Kernel));
		}

		/// <inheritdoc />
		public T Prepare<T>(T obj) where T : ITransient
		{
			obj.Kernel = Kernel;

			var gen = obj as IGenerator;
			if (gen != null)
				gen.Resume();

			Kernel.Root.Add(obj);

			return obj;
		}
	}
}