// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow
{
    public interface ICase<T> : IGenerator<IGenerator> where T : IComparable<T>
    {
        bool Matches(T val);
    }

    /// <summary>
	/// Creates Flow instances that reside within a Kernel.
	/// </summary>
	public interface IFactory
	{
		IKernel Kernel { get; }

		ITransient Transient();
		IGroup Group();
		INode Node();

		IGenerator Do(Action act);
		IGenerator<T> Value<T>(T act);

		IGenerator<T> Expression<T>(Func<T> act);
		IGenerator If(Func<bool> pred, IGenerator @if);
		IGenerator IfElse(Func<bool> pred, IGenerator @if, IGenerator @else);
		ITransient While(Func<bool> pred, IGenerator body);
		ITransient Sequence(params IGenerator[] transients);
		ITransient Parallel(params IGenerator[] transients);

        IGenerator Switch<T>(IGenerator<T> val, params ICase<T>[] cases) where T : IComparable<T>;
        ICase<T> Case<T>(T val, IGenerator statement) where T : IComparable<T>;

		ITransient Apply(Func<ITransient, ITransient> fun, params ITransient[] transients);
		ITransient Wait(TimeSpan span);

		ITransient SetDebugLEvel(EDebugLevel level);
		ITransient Log(string fmt, params object[] objs);
		ITransient Warn(string fmt, params object[] objs);
		ITransient Error(string fmt, params object[] objs);

		ITimer Timer(TimeSpan interval);
		IPeriodic PeriodicTimer(TimeSpan interval);

		IBarrier Barrier(params ITransient[] args);
		IBarrier TimedBarrier(TimeSpan span, params ITransient[] args);

		ITrigger Trigger(params ITransient[] args);
		ITimedTrigger TimedTrigger(TimeSpan span);

		IGenerator Nop();
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
