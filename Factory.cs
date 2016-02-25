// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	///     Makes instances for the Flow library
	/// </summary>
	public class Factory : IFactory
	{
		/// <inheritdoc />
		public IKernel Kernel { get; internal set; }

		/// <inheritdoc />
		public ITimer NewTimer(TimeSpan interval)
		{
			return Prepare(new Timer(Kernel, interval));
		}

		/// <inheritdoc />
		public IPeriodic NewPeriodicTimer(TimeSpan interval)
		{
			return Prepare(new Periodic(Kernel, interval));
		}

		/// <inheritdoc />
		public IBarrier NewBarrier()
		{
			return Prepare(new Barrier());
		}

		public IGenerator NewAction(Action act)
		{
			return new DoAction(act);
		}

		public INode NewNode()
		{
			return Prepare(new Node());
		}

		public ITransient NewParallel(params Action[] actions)
		{
			var gr = NewNode();

			foreach (var act in actions)
			{
				gr.Add(NewAction(act)); 
			}

			return gr;
		}

		public ITransient NewSequence(params Action[] actions)
		{
			INode seq = NewNode();
			IGenerator prev = null;

			foreach (var act in actions)
			{
				var tr = NewAction(act);
				if (prev != null)
					tr.ResumeAfter(prev);

				seq.Add(tr);
				prev = tr;
			}

			return Prepare(seq);
		}

		class DoAction : Generator
		{
			public DoAction(Action act)
			{
				_act = act;
			}

			public override void Step()
			{
				if (!Active)
					return;
				
				_act();

				Console.WriteLine ("Completed");

				Complete();
			}

			Action _act;
		}

		public IBarrier NewBarrier(string name, params ITransient[] args)
		{
			IBarrier barrier = NewBarrier();
			barrier.Name = name;
			foreach (ITransient tr in args)
			{
				barrier.Add(tr);
			}
			return barrier;
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
		public ITimedFuture<T> NewTimedFuture<T>(TimeSpan interval)
		{
			return Prepare(new TimedFuture<T>(Kernel, interval));
		}

		/// <inheritdoc />
		public ISubroutine<TR> NewSubroutine<TR>(Func<IGenerator, TR> fun)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = tr => fun(sub);
			return Prepare(sub);
		}

		/// <inheritdoc />
		public ISubroutine<TR> NewSubroutine<TR, T0>(Func<IGenerator, T0, TR> fun, T0 t0)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = tr => fun(sub, t0);
			return Prepare(sub);
		}

		/// <inheritdoc />
		public ISubroutine<TR> NewSubroutine<TR, T0, T1>(Func<IGenerator, T0, T1, TR> fun, T0 t0, T1 t1)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = tr => fun(sub, t0, t1);
			return Prepare(sub);
		}

		/// <inheritdoc />
		public ISubroutine<TR> NewSubroutine<TR, T0, T1, T2>(Func<IGenerator, T0, T1, T2, TR> fun, T0 t0, T1 t1, T2 t2)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = tr => fun(sub, t0, t1, t2);
			return Prepare(sub);
		}

		/// <inheritdoc />
		public ITypedCoroutine<TR> NewTypedCoroutine<TR>(Func<IGenerator, IEnumerator<TR>> fun)
		{
			var coro = new TypedCoroutine<TR>();
			coro.Start = () => fun(coro);
			return Prepare(coro);
		}

		/// <inheritdoc />
		public ITypedCoroutine<TR> NewTypedCoroutine<TR, T0>(Func<IGenerator, T0, IEnumerator<TR>> fun, T0 t0)
		{
			var coro = new TypedCoroutine<TR>();
			coro.Start = () => fun(coro, t0);
			return Prepare(coro);
		}

		/// <inheritdoc />
		public ITypedCoroutine<TR> NewTypedCoroutine<TR, T0, T1>(Func<IGenerator, T0, T1, IEnumerator<TR>> fun, T0 t0, T1 t1)
		{
			var coro = new TypedCoroutine<TR>();
			coro.Start = () => fun(coro, t0, t1);
			return Prepare(coro);
		}

		/// <inheritdoc />
		public ITypedCoroutine<TR> NewTypedCoroutine<TR, T0, T1, T2>(Func<IGenerator, T0, T1, T2, IEnumerator<TR>> fun, T0 t0,
			T1 t1, T2 t2)
		{
			var coro = new TypedCoroutine<TR>();
			coro.Start = () => fun(coro, t0, t1, t2);
			return Prepare(coro);
		}

		public ICoroutine NewCoroutine(Func<IGenerator, IEnumerator> fun)
		{
			var coro = new Coroutine();
			coro.Start = () => fun(coro);
			return Prepare(coro);
		}

		public ICoroutine NewCoroutine<T0>(Func<IGenerator, T0, IEnumerator> fun, T0 t0)
		{
			var coro = new Coroutine();
			coro.Start = () => fun(coro, t0);
			return Prepare(coro);
		}

		public ICoroutine NewCoroutine<T0, T1>(Func<IGenerator, T0, T1, IEnumerator> fun, T0 t0, T1 t1)
		{
			var coro = new Coroutine();
			coro.Start = () => fun(coro, t0, t1);
			return Prepare(coro);
		}

		public ICoroutine NewCoroutine<T0, T1, T2>(Func<IGenerator, T0, T1, T2, IEnumerator> fun, T0 t0, T1 t1, T2 t2)
		{
			var coro = new Coroutine();
			coro.Start = () => fun(coro, t0, t1, t2);
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

		/// <inheritdoc />
		public ITypedCoroutine<TR> NewTypedCoroutine<TR, T0, T1, T2, T3>(
			Func<IGenerator, T0, T1, T2, T3, IEnumerator<TR>> fun, T0 t0, T1 t1, T2 t2, T3 t3)
		{
			var coro = new TypedCoroutine<TR>();
			coro.Start = () => fun(coro, t0, t1, t2, t3);
			return Prepare(coro);
		}
	}
}
