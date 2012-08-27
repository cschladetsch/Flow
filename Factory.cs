using System;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// Makes instances for the Flow library
	/// </summary>
    public class Factory : IFactory
    {
        public IKernel Kernel { get; internal set; }

		public ITimer NewTimer (TimeSpan interval)
		{
			var timer = new Timer(Kernel, interval);
			return Prepare(timer);
		}

		public IPeriodicTimer NewPeriodicTimer (TimeSpan interval)
		{
			var timer = new PeriodicTimer(Kernel, interval);
			return Prepare(timer);
		}

		public IBarrier NewBarrier()
		{
			return Prepare(new Barrier());
		}

		public ITrigger NewTrigger()
		{
			return Prepare(new Trigger());
		}

		public ITimedTrigger NewTimedTrigger(TimeSpan span)
		{
			throw new NotImplementedException();
		}

		public IFuture<T> NewFuture<T>()
		{
			return Prepare(new Future<T>());
		}

		public ITimedFuture<T> NewTimedFuture<T> (TimeSpan interval)
		{
			return Prepare(new TimedFuture<T>(Kernel, interval));
		}

		public ISubroutine<TR> NewSubroutine<TR>(Func<IGenerator, TR> fun)
		{
            var sub = new Subroutine<TR>();
			sub.Sub = (tr) => fun(sub);
            return Prepare(sub);
		}

		public ISubroutine<TR> NewSubroutine<TR, T0> (Func<IGenerator, T0, TR> fun, T0 t0)
		{
            var sub = new Subroutine<TR>();
			sub.Sub = (tr) => fun(sub, t0);
            return Prepare(sub);
		}

        public ICoroutine<TR> NewCoroutine<TR>(Func<IGenerator, IEnumerator<TR>> fun)
        {
            var coro = new Coroutine<TR>();
            coro.Start = () => fun(coro);
            return Prepare(coro);
        }

        public ICoroutine<TR> NewCoroutine<TR, T0>(Func<IGenerator, T0, IEnumerator<TR>> fun, T0 t0)
        {
            var coro = new Coroutine<TR>();
            coro.Start = () => fun(coro, t0);
            return Prepare(coro);
        }

        public ICoroutine<TR> NewCoroutine<TR, T0, T1>(Func<IGenerator, T0, T1, IEnumerator<TR>> fun, T0 t0, T1 t1)
        {
            var coro = new Coroutine<TR>();
            coro.Start = () => fun(coro, t0, t1);
            return Prepare(coro);
        }

		public IChannel<TR> NewChannel<TR>(ITypedGenerator<TR> gen)
		{
			return Prepare(new Channel<TR>(Kernel, gen));
		}

		public IChannel<TR> NewChannel<TR>()
		{
			return Prepare(new Channel<TR>(Kernel));
		}

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