using System.Linq;
using NUnit.Framework;

namespace Flow.Test
{
    class TestConditionals : TestBase
    {
        [Test]
        public void TestIf()
        {
            Kernel.DebugLevel = Flow.EDebugLevel.Verbose;
            var executed = false;
            var exp = New.If(
                () => true,
                New.Do(() => executed = true)
            );//.Named("IF1");

            Root.Add(exp);
            Step(2);
            Assert.IsTrue(executed);

            executed = false;
            var exp2 = New.If(
                () => false,
                New.Do(() => executed = true)
            );//.Named("If2");

            Root.Remove(exp);
            Root.Add(exp2);

            Step(2);
            Assert.IsFalse(executed);
        }
    }
}
