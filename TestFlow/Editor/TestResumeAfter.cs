using System.Collections;
using NUnit.Framework;

namespace Flow.Test
{
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

            ICoroutine coro = New.Coroutine(Coro); // create a coro
            coro.AddTo(Root); // add it to the root
            coro.ResumeAfter(() => Kernel.StepNumber > activateStep); // wait for the kernel to step x times
            Assert.IsFalse(hasRun); // we will not be running because the kernel hasnt been stepped
            Step(numSteps); // step x times
            Print(Kernel.StepNumber);
            Print(hasRun);
            Assert.AreEqual(hasRun, ran);
        }
    }
}
