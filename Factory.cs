// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEngine;
#endif

namespace Flow.Impl
{
	public interface IBreak : IGenerator
	{
		
	}

	internal class Break : Generator, IBreak
	{
		public override void Step()
		{
			Kernel.BreakFlow();
		}
	}

	/// <summary>
	///     Makes instances for the Flow library
	/// </summary>
	public class Factory : IFactory
	{
		// TODO: does the Factory really need a referene to a kernel? 
		// Kernel and Factory should be separated? 
		public IKernel Kernel { get; internal set; }

		public bool AutoAdd { get; set; }

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
			return Prepare(new Subroutine() { Sub = (tr) => act() });
		}

		public IGenerator If(Func<bool> pred, IGenerator body)
		{
			return Prepare(Coroutine(IfCoro, pred, body));
		}

		IEnumerator IfCoro(IGenerator self, Func<bool> pred, IGenerator body)
		{
			while (true)
			{
				if (!body.Active)
					yield break;

				if (pred())
					body.Step();

				yield return 0;
			}
		}

		public IGenerator IfElse(Func<bool> pred, IGenerator then, IGenerator elseBody)
		{
			return Prepare(Coroutine(IfElseCoro, pred, then, elseBody));
		}

		IEnumerator IfElseCoro(IGenerator self, Func<bool> pred, IGenerator then, IGenerator elseBody)
		{
			while (true)
			{
				if (pred())
				{
					if (!then.Active)
						yield break;
					then.Step();
				}
				else
				{
					if (!elseBody.Active)
						yield break;
					elseBody.Step();
				}
			}
		}

		public IGenerator While(Func<bool> pred, IGenerator body)
		{
			return Prepare(Coroutine(WhileCoro, pred, body));
		}

		IEnumerator WhileCoro(IGenerator self, Func<bool> pred, IGenerator body)
		{
			while (pred())
			{
				if (!body.Active)
					yield break;

				body.Step();

				yield return self;
			}
		}

		public IGenerator<T> Value<T>(T val)
		{
			return Prepare(new Generator<T>() { Value = val });
		}

		public IGenerator<T> Expression<T>(Func<T> act)
		{
			return Prepare(new Subroutine<T> { Sub = s => act() });
		}

		public IGenerator Switch<T>(IGenerator<T> gen, params ICase<T>[] cases) where T : IComparable<T>
		{
			gen.Step();
			T val = gen.Value;
			var coro = Coroutine(SwitchCoro<T>, val, cases);
			Prepare(coro);
			return coro;
		}

		public ICase<T> Case<T>(T val, IGenerator statement) where T : IComparable<T>
		{
			return new Case<T>(val, statement);
		}

		IEnumerator SwitchCoro<T>(IGenerator self, T val, ICase<T>[] cases) where T : IComparable<T>
		{
			foreach (var c in cases)
			{
				if (c.Matches(val))
				{
					yield return c.Body;
				}
			}
		}

		public ITimer OneShotTimer(TimeSpan interval, Action<ITransient> onElapsed)
		{
			var timer = OneShotTimer(interval);
			timer.Elapsed += (self) => onElapsed(self);
			return timer;
		}

		public ITimer OneShotTimer(TimeSpan interval)
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

		public ITransient Sequence(params IGenerator[] gens)
		{
			return Prepare(Coroutine(SequenceCoro, gens));
		}

		private IEnumerator SequenceCoro(IGenerator self, IGenerator[] gens)
		{
			foreach (var gen in gens)
			{
				gen.Step();

				yield return 0;
			}
		}

		public ITransient Parallel(params IGenerator[] transients)
		{
			var node = Node();
			foreach (var act in transients)
			{
				node.Add(act);
			}
			return Prepare(node);
		}

		public ITransient Parallel(params Action[] actions)
		{
			return Prepare(Coroutine(ParallelCoro, actions));
		}

		private IEnumerator ParallelCoro(IGenerator self, Action[] actions)
		{
			while (true)
			{
				foreach (var act in actions)
				{
					act();
				}

				yield return 0;
			}
		}

		public ITransient Apply(Func<ITransient, ITransient> fun, params ITransient[] transients)
		{
			throw new NotImplementedException();
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

		public IGenerator Break()
		{
			return Prepare(new Break());
		}

		public ITransient SetDebugLEvel(EDebugLevel level)
		{
			return Do(() => { Kernel.DebugLevel = level; });
		}

		public ITransient Log(string fmt, params object[] objs)
		{
			return Do(() => { Kernel.Trace.Log(fmt, objs); });
		}

		public ITransient Warn(string fmt, params object[] objs)
		{
			return Do(() => { Kernel.Trace.Warn(fmt, objs); });
		}

		public ITransient Error(string fmt, params object[] objs)
		{
			return Do(() => { Kernel.Trace.Error(fmt, objs); });
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

		public ITrigger Trigger(params ITransient[] args)
		{
			var trigger = new Trigger();
			trigger.Add(args);
			return Prepare(trigger);
		}

		public ITimedTrigger TimedTrigger(TimeSpan span)
		{
			throw new NotImplementedException("New TimedTrigger");
		}

		public IGenerator Nop()
		{
			return Node();
		}

		public IFuture<T> Future<T>()
		{
			return Prepare(new Future<T>());
		}

		public ITransient Wait(TimeSpan span)
		{
			return Do(() => Kernel.Wait(span));
		}

		public ITransient Wait(ITransient trans, TimeSpan timeOut)
		{
			return Prepare(Trigger(trans, OneShotTimer(timeOut)));
		}

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
