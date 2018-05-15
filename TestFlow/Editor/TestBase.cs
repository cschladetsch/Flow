using System;
using NUnit.Framework;

namespace Flow.Test
{
    [TestFixture]
    public class TestBase
    {
        protected IKernel Kernel;
        protected INode Root;
        protected IFactory New;

        [SetUp]
        public void Pre()
        {
            Kernel = Create.Kernel();
            New = Kernel.Factory;
            Root = Kernel.Root;
        }

        protected void Print(object q)
        {
            PrintFmt("{0}={1}", SymbolName.Get(() => q), q);
        }

        protected void PrintFmt(string fmt, params object[] args)
        {
            Kernel.Log.Info(fmt, args);
        }

        protected void Step(int steps = 1)
        {
            for (var n = 0; n < steps; ++n)
            {
                Kernel.Step();
            }
        }

        protected float RunKernel(float seconds)
        {
            Kernel.Step();
            var start = Kernel.Time.Now;
            var end = start + TimeSpan.FromSeconds(seconds);
            while (Kernel.Time.Now < end)
            {
                Kernel.Step();
                if (Kernel.Break)
                    break;
            }
            return (float)(Kernel.Time.Now - start).TotalSeconds;
        }
    }
}
