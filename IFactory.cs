// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow
{
    /// <summary>
    /// Creates Flow instances that reside within a Kernel.
    ///
    /// Instances created by a Factory are not automatically added to any process.
    /// Rather, they have to be later added to a Group that will be executed, such
    /// as Kernel.Root or some other Group.
    /// </summary>
    public interface IFactory
    {
        IKernel Kernel { get; set; }

        ITransient Transient();

        IGroup Group(IEnumerable<ITransient> gens);
        IGroup Group(params ITransient[] gens);
        INode Node(IEnumerable<IGenerator> gens);
        INode Node(params IGenerator[] gens);

        IFuture<TR> Timed<TR>(TimeSpan span, ITransient trans);
        ITimer OneShotTimer(TimeSpan interval);
        ITimer OneShotTimer(TimeSpan interval, Action<ITransient> onElapsed);
        IPeriodic PeriodicTimer(TimeSpan interval);

        IBarrier Barrier(params ITransient[] args);
        IBarrier Barrier(IEnumerable<ITransient> args);
        ITimedBarrier TimedBarrier(TimeSpan span, params ITransient[] args);
        ITimedBarrier TimedBarrier(TimeSpan span, IEnumerable<ITransient> args);

        ITrigger Trigger(params ITransient[] args);
        ITimedTrigger TimedTrigger(TimeSpan span, params ITransient[] args);

        IFuture<T> Future<T>();
        IFuture<T> Future<T>(T val);
        ITimedFuture<T> TimedFuture<T>(TimeSpan timeOut);
        ITimedFuture<T> TimedFuture<T>(TimeSpan timeOut, T val);

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

        IGenerator Nop();

        IGenerator Do(Action act);
        IGenerator<T> Value<T>(T val);
        IGenerator<T> Expression<T>(Func<T> act);
        IGenerator If(Func<bool> pred, IGenerator @if);
        IGenerator IfElse(Func<bool> pred, IGenerator @if, IGenerator @else);
        IGenerator While(Func<bool> pred, params IGenerator[] body);
        IGenerator Sequence(params IGenerator[] transients);
        IGenerator Sequence(IEnumerable<IGenerator> transients);

        IGenerator Switch<T>(IGenerator<T> val, params ICase<T>[] cases) where T : IComparable<T>;
        ICase<T> Case<T>(T val, IGenerator statement) where T : IComparable<T>;
        IGenerator Break();

        ITransient Apply(Func<ITransient, ITransient> fun, params ITransient[] transients);
        ITransient Wait(TimeSpan span);
        ITransient WaitFor(ITransient trans, TimeSpan timeOut);

        IGenerator SetDebugLEvel(EDebugLevel level);
        IGenerator Log(string fmt, params object[] objs);
        IGenerator Warn(string fmt, params object[] objs);
        IGenerator Error(string fmt, params object[] objs);

        T Prepare<T>(T obj) where T : ITransient;
    }
}
