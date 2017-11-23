// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// Creates Flow instances that reside within a Kernel.
	/// </summary>
	public interface IFactory
	{
		IKernel Kernel { get; }

		INode Node();
		IGroup Group();
		ITransient Transient();

		//IGenerator Once(Action act);
		IGenerator Do(Action act);
		//IGenerator<T> Expression<T>(Func<T> act);
		//IGenerator<T> Once<T>(Func<T> act);
		//IGenerator<T> Do<T>(Func<T> act);
		IGenerator<T> Value<T>(Func<T> act);
		IGenerator<T> Expression<T>(Func<T> act);
		ITransient If(Func<bool> pred, IGenerator @if);
		ITransient IfElse(Func<bool> pred, ITransient @if, ITransient @else);
		ITransient While(Func<bool> pred, params ITransient[] body);
		ITransient Loop(params ITransient[] trans);
		ITransient Sequence(params ITransient[] transients);
		ITransient Parallel(params ITransient[] transients);
		ITransient Apply(Func<ITransient, ITransient> fun, params ITransient[] transients);
		ITransient WaitFor(TimeSpan span);

		ITransient DebugLog(string fmt, params object[] objs);
		ITransient DebugWarning(string fmt, params object[] objs);
		ITransient DebugError(string fmt, params object[] objs);
		ITransient DebugException(string fmt, Exception ex);

		ITimer Timer(TimeSpan interval);
		IPeriodic PeriodicTimer(TimeSpan interval);

		IBarrier Barrier(params ITransient[] args);
		IBarrier TimedBarrier(TimeSpan span, params ITransient[] args);

		ITrigger Trigger();
		ITimedTrigger TimedTrigger(TimeSpan span);

		IFuture<T> Future<T>();
		ITimedFuture<T> TimedFuture<T>(TimeSpan timeOut);

		ICoroutine Coroutine(Func<IGenerator, IEnumerator> fun);
		ICoroutine Coroutine<T0>(Func<IGenerator, T0, IEnumerator> fun, T0 t0);
		ICoroutine Coroutine<T0, T1>(Func<IGenerator, T0, T1, IEnumerator> fun, T0 t0, T1 t1);
		ICoroutine Coroutine<T0, T1, T2>(Func<IGenerator, T0, T1, T2, IEnumerator> fun, T0 t0, T1 t1, T2 t2);
		ICoroutine<TR> Coroutine<TR>(Func<IGenerator, IEnumerator<TR>> fun);
		ICoroutine<TR> Coroutine<TR, T0>(Func<IGenerator, T0, IEnumerator<TR>> fun, T0 t0);

		ISubroutine<TR> Subroutine<TR>(Func<IGenerator, TR> fun);
		ISubroutine<TR> Subroutine<TR, T0>(Func<IGenerator, T0, TR> fun, T0 t0);
		ISubroutine<TR> Subroutine<TR, T0, T1>(Func<IGenerator, T0, T1, TR> fun, T0 t0, T1 t1);
		ISubroutine<TR> Subroutine<TR, T0, T1, T2>(Func<IGenerator, T0, T1, T2, TR> fun, T0 t0, T1 t1, T2 t2);

		IChannel<TR> Channel<TR>();
		IChannel<TR> Channel<TR>(IGenerator<TR> gen);

		T Prepare<T>(T obj) where T : ITransient;
	}
}
