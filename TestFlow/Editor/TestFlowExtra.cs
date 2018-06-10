using System;
using System.Collections;
using System.Diagnostics;

using Flow;
using NUnit.Framework;
using App.Common;

namespace Flow.Test
{
    // TODO: Move to Flow library
    [TestFixture]
    public class TestFlowExtra
    {
        [Test]
        public void TestWhile()
        {
            var k = Flow.Create.Kernel();
            var f = k.Factory;
        }

        [Test]
        public void TestBasicPrint()
        {
            var k = Flow.Create.Kernel();
            var f = k.Factory;

            var t0 = f.Transient().Named("T0");
            var t1 = f.Transient().Named("Trans1");
            var t2 = f.Transient().Named("Trans2");
            var r0 = f.Trigger(t2, t1).Named("Barrier0");
            var b0 = f.Barrier(t0, r0).Named("Barrier1");
            var f0 = f.Future<int>().Named("Future<int>");
            var g0 = f.Group(f0, b0).Named("Group");

            k.Root.Add(g0);

            // allow all objects to be placed
            for (var n = 0; n < 10; ++n)
                k.Step();

            Console.WriteLine(g0);
        }

        private static void Step(Flow.IGenerator gen, int steps)
        {
            for (var n = 0; n < steps; ++n)
                gen.Step();
        }

        [Test]
        public void TestFlowSequence()
        {
            var k = Flow.Create.Kernel();
            var f = k.Factory;
            var r = k.Root;
            r.Add(f.Sequence(
                f.Coroutine(StartGame).Named("GameLoop"),
                f.Coroutine(PlayerTurn, EColor.White),
                f.Coroutine(EndGame))
            );

            k.Step();
            Trace.WriteLine(r);
            k.Step();
            Trace.WriteLine(r);
            k.Step();
            Trace.WriteLine(r);
            k.Step();
            Trace.WriteLine(r);
            k.Step();
            Trace.WriteLine(r);
        }

        static IEnumerator PlayerTurn(IGenerator self, EColor color)
        {
            Trace.WriteLine($"PlayerTurn: {color}");
            yield return null;
            Trace.WriteLine($"PlayerTurn: Again {color}");
            yield return null;
            Trace.WriteLine($"PlayerTurn: Again Again {color}");
        }

        static IEnumerator EndGame(IGenerator self)
        {
            Trace.WriteLine("EndGame");
            yield break;
        }

        static IEnumerator StartGame(IGenerator self)
        {
            Trace.WriteLine("GameLoop");
            yield break;
        }
    }
}
