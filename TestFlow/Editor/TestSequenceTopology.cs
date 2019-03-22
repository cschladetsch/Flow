using NUnit.Framework;

namespace Flow.Test
{
    [TestFixture]
    public class TestSequenceTopology : TestBase
    {
        [Test()]
        public void TestSequence()
        {
            var stepNum = 0;
            Root.Add(
                New.Sequence(
                    New.Do(() => stepNum += Kernel.StepNumber),
                    New.Do(() => stepNum += Kernel.StepNumber),
                    New.Do(() => stepNum += Kernel.StepNumber)
                )
            );
            Step(3);
            Assert.AreEqual(stepNum, 0 + 1 + 2);
        }

        [Test()]
        public void TestParallel()
        {
            var stepNum = 0;
            Root.Add(
                New.Node(
                    New.Do(() => stepNum += Kernel.StepNumber),
                    New.Do(() => stepNum += Kernel.StepNumber),
                    New.Do(() => stepNum += Kernel.StepNumber)
                )
            );
            Step(3);
            Assert.AreEqual(stepNum, 0 * 3 + 1 * 3 + 2 * 3);
        }
    }
}
