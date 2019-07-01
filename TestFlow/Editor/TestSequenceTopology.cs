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
                    New.Do(() => stepNum += Kernel.StepNumber),
                    New.Do(() => stepNum += Kernel.StepNumber),
                    New.Do(() => stepNum += Kernel.StepNumber)
                )
            );
            Step(5);
            Assert.AreEqual(stepNum, 0 + 1 + 2 + 3 + 4);
        }

        [Test]
        public void TestEmptySequenceCompletes()
        {
            bool completed = false;
            var seq = New.Sequence().AddTo(Root);
            seq.Completed += tr => completed = true;
            Step(4);
            Assert.IsTrue(completed);
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
