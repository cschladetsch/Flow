namespace Flow.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class TestSequences
        : TestBase
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
            var completed = false;
            var seq = New.Sequence().AddTo(Root);
            seq.Completed += tr => completed = true;

            Step(4);

            Assert.IsTrue(completed);
        }

        [Test]
        public void TestParallel()
        {
            var stepNum = 0;
                New.Node(
                    New.Do(() => stepNum += 1).Named("Do 1"),
                    New.Do(() => stepNum += 1).Named("Do 2"),
                    New.Do(() => stepNum += 1).Named("Do 3")
                ).Named("Parallel Node").AddTo(Root);

            Step(3);

            Assert.AreEqual(3, stepNum);
        }
    }
}
