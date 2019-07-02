namespace Flow.Test
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    public class TestResumeAfter
        : TestBase
    {
        [TestCase(10, 15, true)]
        [TestCase(15, 3, false)]
        [TestCase(3, 3, false)]
        public void TestResumeAfterPred(int activateStep, int numSteps, bool ran)
        {
            var hasRun = false;
            IEnumerator Coro(IGenerator self)
            {
                yield return null;
                hasRun = true;
            }

            var coro = New.Coroutine(Coro);
            coro.AddTo(Root);
            coro.ResumeAfter(() => Kernel.StepNumber > activateStep);
            Assert.IsFalse(hasRun);

            Step(numSteps);
            Print(Kernel.StepNumber);
            Print(hasRun);

            Assert.AreEqual(hasRun, ran);
        }
    }
}
