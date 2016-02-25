// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	///     Creates Flow instances that reside within a Kernel.
	/// </summary>
	public interface IFactory
	{
		/// <summary>
		///     Gets the kernel.
		/// </summary>
		/// <value>
		///     The kernel.
		/// </value>
		IKernel Kernel { get; }

//		ITransient NewParallel(params Action<IGenerator> actions);

		ITransient NewSequence(params Action[] actions);

		ITransient NewParallel(params Action[] actions);

		INode NewNode();

		/// <summary>
		///     News the timer.
		/// </summary>
		/// <returns>
		///     The timer.
		/// </returns>
		/// <param name='interval'>
		///     Interval.
		/// </param>
		ITimer NewTimer(TimeSpan interval);

		/// <summary>
		///     News the periodic timer.
		/// </summary>
		/// <returns>
		///     The periodic timer.
		/// </returns>
		/// <param name='interval'>
		///     Interval.
		/// </param>
		IPeriodic NewPeriodicTimer(TimeSpan interval);

		/// <summary>
		///     News the barrier.
		/// </summary>
		/// <returns>
		///     The barrier.
		/// </returns>
		IBarrier NewBarrier();

		/// <summary>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		IBarrier NewBarrier(string name, params ITransient[] args);

		/// <summary>
		///     Make a new trigger.
		/// </summary>
		/// <returns>
		///     The trigger.
		/// </returns>
		ITrigger NewTrigger();

		/// <summary>
		///     Make a new timed trigger.
		/// </summary>
		/// <returns>
		///     The timed trigger.
		/// </returns>
		/// <param name='span'>
		///     The trigger will be deleted after this time internal.
		/// </param>
		ITimedTrigger NewTimedTrigger(TimeSpan span);

		/// <summary>
		///     News the future.
		/// </summary>
		/// <returns>
		///     The future.
		/// </returns>
		/// <typeparam name='T'>
		///     The 1st type parameter.
		/// </typeparam>
		IFuture<T> NewFuture<T>();

		/// <summary>
		///     Make a new timed future.
		/// </summary>
		/// <returns>
		///     The timed future.
		/// </returns>
		/// <param name='timeOut'>
		///     Time out.
		/// </param>
		/// <typeparam name='T'>
		///     The 1st type parameter.
		/// </typeparam>
		ITimedFuture<T> NewTimedFuture<T>(TimeSpan timeOut);

		/// <summary>
		///     News the coroutine.
		/// </summary>
		/// <returns>
		///     The coroutine.
		/// </returns>
		/// <param name='fun'>
		///     Fun.
		/// </param>
		/// <typeparam name='TR'>
		///     The 1st type parameter.
		/// </typeparam>
		ITypedCoroutine<TR> NewTypedCoroutine<TR>(Func<IGenerator, IEnumerator<TR>> fun);

		IGenerator NewAction(Action act);

		/// <summary>
		///     News the coroutine.
		/// </summary>
		/// <returns>
		///     The coroutine.
		/// </returns>
		/// <param name='fun'>
		///     Fun.
		/// </param>
		/// <param name='t0'>
		///     T0.
		/// </param>
		/// <typeparam name='TR'>
		///     The 1st type parameter.
		/// </typeparam>
		/// <typeparam name='T0'>
		///     The 2nd type parameter.
		/// </typeparam>
		ITypedCoroutine<TR> NewTypedCoroutine<TR, T0>(Func<IGenerator, T0, IEnumerator<TR>> fun, T0 t0);

		/// <summary>
		///     News the coroutine.
		/// </summary>
		/// <returns>
		///     The coroutine.
		/// </returns>
		/// <param name='fun'>
		///     Fun.
		/// </param>
		/// <param name='t0'>
		///     T0.
		/// </param>
		/// <param name='t1'>
		///     T1.
		/// </param>
		/// <typeparam name='TR'>
		///     The 1st type parameter.
		/// </typeparam>
		/// <typeparam name='T0'>
		///     The 2nd type parameter.
		/// </typeparam>
		/// <typeparam name='T1'>
		///     The 3rd type parameter.
		/// </typeparam>
		ITypedCoroutine<TR> NewTypedCoroutine<TR, T0, T1>(Func<IGenerator, T0, T1, IEnumerator<TR>> fun, T0 t0, T1 t1);

		ITypedCoroutine<TR> NewTypedCoroutine<TR, T0, T1, T2>(Func<IGenerator, T0, T1, T2, IEnumerator<TR>> fun, T0 t0, T1 t1,
			T2 t2);

		/// <summary>
		/// </summary>
		/// <param name="fun"></param>
		/// <returns></returns>
		ICoroutine NewCoroutine(Func<IGenerator, IEnumerator> fun);

		/// <summary>
		/// </summary>
		/// <typeparam name="T0"></typeparam>
		/// <param name="fun"></param>
		/// <param name="t0"></param>
		/// <returns></returns>
		ICoroutine NewCoroutine<T0>(Func<IGenerator, T0, IEnumerator> fun, T0 t0);

		ICoroutine NewCoroutine<T0, T1>(Func<IGenerator, T0, T1, IEnumerator> fun, T0 t0, T1 t1);

		ICoroutine NewCoroutine<T0, T1, T2>(Func<IGenerator, T0, T1, T2, IEnumerator> fun, T0 t0, T1 t1, T2 t2);

		/// <summary>
		///     News the subroutine.
		/// </summary>
		/// <returns>
		///     The subroutine.
		/// </returns>
		/// <param name='fun'>
		///     Fun.
		/// </param>
		/// <typeparam name='TR'>
		///     The 1st type parameter.
		/// </typeparam>
		ISubroutine<TR> NewSubroutine<TR>(Func<IGenerator, TR> fun);

		/// <summary>
		///     News the subroutine.
		/// </summary>
		/// <returns>
		///     The subroutine.
		/// </returns>
		/// <param name='fun'>
		///     Fun.
		/// </param>
		/// <param name='t0'>
		///     T0.
		/// </param>
		/// <typeparam name='TR'>
		///     The 1st type parameter.
		/// </typeparam>
		/// <typeparam name='T0'>
		///     The 2nd type parameter.
		/// </typeparam>
		ISubroutine<TR> NewSubroutine<TR, T0>(Func<IGenerator, T0, TR> fun, T0 t0);

		/// <summary>
		///     News the subroutine.
		/// </summary>
		/// <returns>
		///     The subroutine.
		/// </returns>
		/// <param name='fun'>
		///     Fun.
		/// </param>
		/// <param name='t0'>
		///     T0.
		/// </param>
		/// <param name='t1'>
		///     T1.
		/// </param>
		/// <typeparam name='TR'>
		///     The 1st type parameter.
		/// </typeparam>
		/// <typeparam name='T0'>
		///     The 2nd type parameter.
		/// </typeparam>
		/// <typeparam name='T1'>
		///     The 3rd type parameter.
		/// </typeparam>
		ISubroutine<TR> NewSubroutine<TR, T0, T1>(Func<IGenerator, T0, T1, TR> fun, T0 t0, T1 t1);

		/// <summary>
		///     News the subroutine.
		/// </summary>
		/// <returns>
		///     The subroutine.
		/// </returns>
		/// <param name='fun'>
		///     Fun.
		/// </param>
		/// <param name='t0'>
		///     T0.
		/// </param>
		/// <param name='t1'>
		///     T1.
		/// </param>
		/// <param name='t2'>
		///     T2.
		/// </param>
		/// <typeparam name='TR'>
		///     The 1st type parameter.
		/// </typeparam>
		/// <typeparam name='T0'>
		///     The 2nd type parameter.
		/// </typeparam>
		/// <typeparam name='T1'>
		///     The 3rd type parameter.
		/// </typeparam>
		/// <typeparam name='T2'>
		///     The 4th type parameter.
		/// </typeparam>
		ISubroutine<TR> NewSubroutine<TR, T0, T1, T2>(Func<IGenerator, T0, T1, T2, TR> fun, T0 t0, T1 t1, T2 t2);

		/// <summary>
		///     News the channel.
		/// </summary>
		/// <returns>
		///     The channel.
		/// </returns>
		/// <param name='gen'>
		///     Gen.
		/// </param>
		IChannel<TR> NewChannel<TR>(ITypedGenerator<TR> gen);

		/// <summary>
		///     News the channel.
		/// </summary>
		/// <returns>
		///     The channel.
		/// </returns>
		/// <typeparam name='TR'>
		///     The 1st type parameter.
		/// </typeparam>
		IChannel<TR> NewChannel<TR>();

		/// <summary>
		///     Prepare the specified transient for use with the kernel associated with this factory.
		///     <para>This can be used to prepare custom implementations of objects used by the flow library</para>
		/// </summary>
		/// <param name='obj'>
		///     The transient object to prepare
		/// </param>
		/// <typeparam name='T'>
		///     The 1st type parameter.
		/// </typeparam>
		T Prepare<T>(T obj) where T : ITransient;
	}
}
