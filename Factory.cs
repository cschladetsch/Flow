// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow.Impl
{
	/// <summary>
	///     Makes instances for the Flow library
	/// </summary>
	public class Factory : IFactory
	{
		public IKernel Kernel { get; internal set; }

		public INode Node()
		{
			return Prepare(new Node());
		}

		public IGroup Group()
		{
			return Prepare(new Group());
		}

		public ITransient Transient()
		{
			return Prepare(new Transient());
		}

		public IGenerator Do(Action act)
		{
			return Prepare(new detail.EveryTime(act));
		}

		public IGenerator<bool> If(Func<bool> fun)
		{
			return Prepare(Expression(fun));
		}

		public ITransient If(Func<bool> test, IGenerator @if)
		{
			return Prepare(new Conditional(Expression(test), @if));
		}

		public ITransient IfElse(Func<bool> pred, ITransient @if, ITransient @else)
		{
			throw new NotImplementedException();
		}

		public ITransient While(Func<bool> pred, params ITransient[] body)
		{
			throw new NotImplementedException();
		}

		public IGenerator<T> Value<T>(Func<T> act)
		{
			throw new NotImplementedException();
		}

		public IGenerator<T> Expression<T>(Func<T> act)
		{
			return Prepare(new Subroutine<T> {Sub = s => act()});
		}

		public ITransient DebugException(string fmt, Exception ex)
		{
			throw new NotImplementedException();
		}

		public ITimer Timer(TimeSpan interval)
		{
			return Prepare(new Timer(Kernel, interval));
		}

		public IPeriodic PeriodicTimer(TimeSpan interval)
		{
			return Prepare(new Periodic(Kernel, interval));
		}

		public IBarrier Barrier()
		{
			return Prepare(new Barrier());
		}

		public ITransient Parallel(params ITransient[] transients)
		{
			var gr = Node();
			foreach (var act in transients)
			{
				gr.Add(act);
			}
			return Prepare(gr);
		}

		public ITransient Apply(Func<ITransient, ITransient> fun, params ITransient[] transients)
		{
			throw new NotImplementedException();
		}

		public ITransient Parallel(params Action[] actions)
		{
			var node = Node();
			foreach (var act in actions)
			{
				node.Add(Do(act));
			}
			return Prepare(node);
		}

		IEnumerator<bool> ConditionCoro(IGenerator self, Func<bool> pred)
		{
			while (pred())
			{
				yield return true;
			}

			yield return false;
			self.Complete();
		}

		public ITransient DebugLog(string fmt, params object[] objs)
		{
			return Do(() =>
			{
				#if UNITY
				Debug.LogFormat(fmt, objs);
				#endif
			});
		}

		public ITransient DebugWarning(string fmt, params object[] objs)
		{
			throw new NotImplementedException();
		}

		public ITransient DebugError(string fmt, params object[] objs)
		{
			throw new NotImplementedException();
		}

		public ITransient Loop(params ITransient[] trans)
		{
			var node = Node();
			foreach (var other in trans)
			{
				node.Add(other);
			}
			return Prepare(node);
		}

		public ITransient ActionSequence(params Action[] actions)
		{
			INode seq = Node();
			IGenerator prev = null;

			foreach (var act in actions)
			{
				var tr = Do(act);
				if (prev != null)
					tr.ResumeAfter(prev);

				seq.Add(tr);
				prev = tr;
			}

			return Prepare(seq);
		}

		public ITransient ActionParallel(params Action[] actions)
		{
			var node = Node();
			foreach (var act in actions)
				node.Add(Do(act));
			return Prepare(node);
		}

		public ITransient Sequence(params ITransient[] transients)
		{
			ITransient next = Node();
			var first = next;
			foreach (var trans in transients)
			{
				if (next != null)
					trans.Completed += (tr) => Sequence(trans);
				next = Prepare(trans);
			}

			return Prepare(first);
		}

		public IBarrier Barrier(params ITransient[] args)
		{
			var barrier = Barrier();
			foreach (var tr in args)
			{
				barrier.Add(tr);
			}
			return Prepare(barrier);
		}

		public IBarrier TimedBarrier(TimeSpan span, params ITransient[] args)
		{
			throw new NotImplementedException();
		}

		public ITrigger Trigger()
		{
			return Prepare(new Trigger());
		}

		public ITimedTrigger TimedTrigger(TimeSpan span)
		{
			throw new NotImplementedException();
		}

		public IFuture<T> Future<T>()
		{
			return Prepare(new Future<T>());
		}

		public ITransient WaitFor(TimeSpan span)
		{
			var start = Kernel.Time.Now;
			var end = Kernel.Time.Now + span;
			var next = Node();
			var test = If(() => end > start);
			return next;
		}

		//public IGenerator<bool> While(Func<bool> act, ITransient body)
		//{
		//	return Do(act);
		//}

		public ITimedFuture<T> TimedFuture<T>(TimeSpan interval)
		{
			return Prepare(new TimedFuture<T>(Kernel, interval));
		}

		public ISubroutine<TR> Subroutine<TR>(Func<IGenerator, TR> fun)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = tr => fun(sub);
			return Prepare(sub);
		}

		public ISubroutine<TR> Subroutine<TR, T0>(Func<IGenerator, T0, TR> fun, T0 t0)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = tr => fun(sub, t0);
			return Prepare(sub);
		}

		public ISubroutine<TR> Subroutine<TR, T0, T1>(Func<IGenerator, T0, T1, TR> fun, T0 t0, T1 t1)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = tr => fun(sub, t0, t1);
			return Prepare(sub);
		}

		public ISubroutine<TR> Subroutine<TR, T0, T1, T2>(Func<IGenerator, T0, T1, T2, TR> fun, T0 t0, T1 t1, T2 t2)
		{
			var sub = new Subroutine<TR>();
			sub.Sub = tr => fun(sub, t0, t1, t2);
			return Prepare(sub);
		}

		public ICoroutine<TR> Coroutine<TR>(Func<IGenerator, IEnumerator<TR>> fun)
		{
			var coro = new Coroutine<TR>();
			coro.Start = () => fun(coro);
			return Prepare(coro);
		}

		public ICoroutine<TR> Coroutine<TR, T0>(Func<IGenerator, T0, IEnumerator<TR>> fun, T0 t0)
		{
			var coro = new Coroutine<TR>();
			coro.Start = () => fun(coro, t0);
			return Prepare(coro);
		}

		public ICoroutine<TR> TypedCoroutine<TR, T0, T1>(Func<IGenerator, T0, T1, IEnumerator<TR>> fun, T0 t0, T1 t1)
		{
			var coro = new Coroutine<TR>();
			coro.Start = () => fun(coro, t0, t1);
			return Prepare(coro);
		}

		public ICoroutine<TR> TypedCoroutine<TR, T0, T1, T2>(Func<IGenerator, T0, T1, T2, IEnumerator<TR>> fun, T0 t0,
			T1 t1, T2 t2)
		{
			var coro = new Coroutine<TR>();
			coro.Start = () => fun(coro, t0, t1, t2);
			return Prepare(coro);
		}

		public ICoroutine Coroutine(Func<IGenerator, IEnumerator> fun)
		{
			var coro = new Coroutine();
			coro.Start = () => fun(coro);
			return Prepare(coro);
		}

		public ICoroutine Coroutine<T0>(Func<IGenerator, T0, IEnumerator> fun, T0 t0)
		{
			var coro = new Coroutine();
			coro.Start = () => fun(coro, t0);
			return Prepare(coro);
		}

		public ICoroutine Coroutine<T0, T1>(Func<IGenerator, T0, T1, IEnumerator> fun, T0 t0, T1 t1)
		{
			var coro = new Coroutine();
			coro.Start = () => fun(coro, t0, t1);
			return Prepare(coro);
		}

		public ICoroutine Coroutine<T0, T1, T2>(Func<IGenerator, T0, T1, T2, IEnumerator> fun, T0 t0, T1 t1, T2 t2)
		{
			var coro = new Coroutine();
			coro.Start = () => fun(coro, t0, t1, t2);
			return Prepare(coro);
		}

		public IChannel<TR> Channel<TR>(IGenerator<TR> gen)
		{
			return Prepare(new Channel<TR>(Kernel, gen));
		}

		public IChannel<TR> Channel<TR>()
		{
			return Prepare(new Channel<TR>(Kernel));
		}

		public T Prepare<T>(T obj, bool add) where T : ITransient
		{
			var tr = Prepare(obj);
			Kernel.Root.Add(tr);
			return tr;
		}

		public T Prepare<T>(T obj) where T : ITransient
		{
			obj.Kernel = Kernel;

			var gen = obj as IGenerator;
			if (gen != null)
				gen.Resume();

			//Kernel.Root.Add(obj);

			return obj;
		}

		///// <inheritdoc />
		//public ICoroutine<TR> TypedCoroutine<TR, T0, T1, T2, T3>(
		//	Func<IGenerator, T0, T1, T2, T3, IEnumerator<TR>> fun, T0 t0, T1 t1, T2 t2, T3 t3)
		//{
		//	var coro = new Coroutine<TR>();
		//	coro.Start = () => fun(coro, t0, t1, t2, t3);
		//	return Prepare(coro);
		//}
	}

	namespace detail
	{
		class EveryTime : Generator
		{
			public EveryTime(Action act)
			{
				_act = act;
			}

			public override void Step()
			{
				if (!Active)
					return;

				_act();
			}

			readonly Action _act;
		}

		class OneTime : EveryTime
		{
			public OneTime(Action act) : base(act) { }

			public override void Step()
			{
				base.Step();
				Complete();
			}
		}
	}
}
