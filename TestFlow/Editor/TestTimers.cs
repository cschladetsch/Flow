using System;
using NUnit.Framework;

namespace Flow.Test
{
    public class TestTimers : TestBase
    {
        [TestCase(0.4f, 0.5f, true)]
        [TestCase(0.4f, 0.2f, false)]
        public void TestOneShot(float span, float runTime, bool shouldBeCompleted)
        {
            var elapsed = false;
            var oneShotTimer = New.OneShotTimer(TimeSpan.FromSeconds(span), (timer) => elapsed = true);
            Root.Add(oneShotTimer);
            RunKernel(runTime);

            if (shouldBeCompleted)
            {
                Assert.IsTrue(!oneShotTimer.Active);
                Assert.IsTrue(elapsed);
            }
            else
            {
                Assert.IsTrue(oneShotTimer.Active);
                Assert.IsFalse(elapsed);
            }
        }

        [TestCase(0.1f, 0.25f, 2)]
        [TestCase(0.1f, 0.45f, 4)]
        [TestCase(0.1f, 0.0f, 0)]
        public void TestPeriodic(float interval, float runTime, int numElapsed)
        {
            var timer = New.PeriodicTimer(TimeSpan.FromSeconds(interval));

            int elapsed = 0;
            timer.Elapsed += (sender) => ++elapsed;

            Root.Add(timer);
            RunKernel(runTime);

            Assert.AreEqual(numElapsed, elapsed);
        }

        [TestCase(0.1f, 0.2f, true)]
        [TestCase(0.2f, 0.1f, false)]
        public void TestTimedFuture(float futureLifetime, float runTime, bool result)
        {
            var span = TimeSpan.FromSeconds(futureLifetime);
            var future = New.TimedFuture<int>(span);

            Assert.IsFalse(future.Available);

            Root.Add(future);
            RunKernel(runTime);

            Step();

            Assert.AreEqual(result, future.HasTimedOut);
        }
    }
}

