// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEngine;
#endif

namespace Flow.Impl
{
    /// <inheritdoc />
    /// <summary>
    /// Makes instances for the Flow library
    /// </summary>
    public class Factory : IFactory
    {
        // TODO: does the Factory really need a referene to a kernel?
        // Kernel and Factory should be separated?
        public IKernel Kernel { get; set; }

        public bool AutoAdd { get; set; }

        public INode Node(params IGenerator[] gens)
        {
            return Node(gens.ToList());
        }

        public INode Node(IEnumerable<IGenerator> gens)
        {
            var node = Prepare(new Node());
            node.Add(gens);
            return node;
        }

        public IGroup Group(params ITransient[] trans)
        {
            var group = Prepare(new Group());
            group.Add(trans);
            return group;
        }

        public IGroup Group(IEnumerable<ITransient> trans)
        {
            var group = Prepare(new Group());
            group.Add(trans);
            return group;
        }

        public ITransient Transient()
        {
            return Prepare(new Transient());
        }

        public IGenerator Do(Action act)
        {
            return Prepare(new Subroutine() { Sub = (tr) => act() });
        }

        public IFuture<TR> Timed<TR>(TimeSpan span, ITransient trans)
        {
            var timed = TimedFuture<TR>(span);
            timed.TimedOut += (tr) => trans.Complete();
            return Prepare(timed);
        }

        public IGenerator If(Func<bool> pred, IGenerator body)
        {
            return Prepare(Coroutine(IfCoro, pred, body));
        }

        private static IEnumerator IfCoro(IGenerator self, Func<bool> pred, IGenerator body)
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

        private static IEnumerator IfElseCoro(IGenerator self, Func<bool> pred, IGenerator then, IGenerator elseBody)
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

        public IGenerator While(Func<bool> pred, params IGenerator[] body)
        {
            return Prepare(Coroutine(WhileCoro, pred, body));
        }

        private IEnumerator WhileCoro(IGenerator self, Func<bool> pred, IGenerator[] gens)
        {
            var node = Prepare(Node(gens));
            while (pred())
            {
                node.Step();
                if (!node.Active || node.Empty)
                    yield break;

                yield return null;
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

        public IGenerator Sequence(params IGenerator[] gens)
        {
            return Sequence(gens.ToList());
        }

        public IGenerator Sequence(IEnumerable<IGenerator> gens)
        {
            var seq = new Sequence();
            seq.Add(gens);
            return Prepare(seq);
        }

        public IGenerator Switch<T>(IGenerator<T> gen, params ICase<T>[] cases) where T : IComparable<T>
        {
            gen.Step();
            var val = gen.Value;
            var coro = Coroutine(SwitchCoro, val, cases);
            Prepare(coro);
            return coro;
        }

        public ICase<T> Case<T>(T val, IGenerator statement) where T : IComparable<T>
        {
            return new Case<T>(val, statement);
        }

        private static IEnumerator SwitchCoro<T>(IGenerator self, T val, ICase<T>[] cases) where T : IComparable<T>
        {
            return (from c in cases where c.Matches(val) select c.Body).GetEnumerator();
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

        public IGenerator Sequence(params ITransient[] gens)
        {
            var seq = new Sequence();
            seq.Add(gens);
            return Prepare(seq);
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

        public IGenerator SetDebugLEvel(EDebugLevel level)
        {
            return Do(() => { Kernel.DebugLevel = level; });
        }

        public IGenerator Log(string fmt, params object[] objs)
        {
            return Do(() => { Kernel.Log.Info(fmt, objs); });
        }

        public IGenerator Warn(string fmt, params object[] objs)
        {
            return Do(() => { Kernel.Log.Warn(fmt, objs); });
        }

        public IGenerator Error(string fmt, params object[] objs)
        {
            return Do(() => { Kernel.Log.Error(fmt, objs); });
        }

        public ITransient ActionSequence(params Action[] actions)
        {
            var seq = Node();
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
            return Barrier(args.ToList());
        }

        public IBarrier Barrier(IEnumerable<ITransient> args)
        {
            var barrier = Barrier();
            foreach (var tr in args)
            {
                if (tr == null)
                    continue;
                barrier.Add(tr);
            }
            return Prepare(barrier);
        }

        public ITimedBarrier TimedBarrier(TimeSpan span, params ITransient[] args)
        {
            return TimedBarrier(span, args.ToList());
        }

        public ITimedBarrier TimedBarrier(TimeSpan span, IEnumerable<ITransient> args)
        {
            var barrier = new TimedBarrier(Kernel, span, args);
            return Prepare(barrier);
        }

        public ITrigger Trigger(params ITransient[] args)
        {
            var trigger = new Trigger();
            trigger.Add(args);
            return Prepare(trigger);
        }

        public ITimedTrigger TimedTrigger(TimeSpan span, params ITransient[] args)
        {
            var timedTrigger = new TimedTrigger(Kernel, span);
            timedTrigger.Add(args);
            return Prepare(timedTrigger);
        }

        public IGenerator Nop()
        {
            return Node();
        }

        public IFuture<T> Future<T>()
        {
            return Prepare(new Future<T>());
        }

        public IFuture<T> Future<T>(T val)
        {
            var future = Prepare(new Future<T>());
            future.Value = val;
            return future;
        }

        public ITransient Wait(TimeSpan span)
        {
            return Do(() => Kernel.Wait(span));
        }

        public ITransient WaitFor(ITransient trans, TimeSpan timeOut)
        {
            return Prepare(Trigger(trans, OneShotTimer(timeOut)));
        }

        public ITimedFuture<T> TimedFuture<T>(TimeSpan interval)
        {
            return Prepare(new TimedFuture<T>(Kernel, interval));
        }

        public ITimedFuture<T> TimedFuture<T>(TimeSpan timeOut, T val)
        {
            var future = TimedFuture<T>(timeOut);
            future.Value = val;
            return future;
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
            (obj as IGenerator)?.Resume();
            return obj;
        }
    }
}
