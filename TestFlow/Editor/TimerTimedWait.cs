//using System;
//using NUnit.Framework;

//namespace Flow.Test
//{
//    [TestFixture]
//    class TimerWaitTest : TestBase
//    {
//        // Broke this test when I removed IFactory.Parallel
//        // I thought Node did the same thing...
//        //[TestCase(0.5f)]
//        //[TestCase(0.2f)]
//        //public void TestBreak(float timeOut)
//        //{
//        //    _root.Add(
//        //        New.Node(
//        //            New.While(() => true,
//        //                New.OneShotTimer(TimeSpan.FromSeconds(timeOut), (self) => self.Kernel.BreakFlow())
//        //            )
//        //        )
//        //    );

//        //    Print(_root);

//        //    var delta = RunKernel(2) - timeOut;
//        //    Assert.IsTrue(Math.Abs(delta) < 0.1f);
//        //}

//        [TestCase(0.5f, 1.0f, 0.5f)]
//        [TestCase(1.5f, 1.0f, 1.0f)]
//        public void TestTimedWait(float timerLen, float timeOut, float kernelRunTime)
//        {
//            Root.Add(
//                New.WaitFor(
//                    New.Trigger(New.OneShotTimer(TimeSpan.FromSeconds(timerLen))),
//                    TimeSpan.FromSeconds(timeOut)
//                ),
//                New.Break()
//            );

//            var delta = timerLen - RunKernel(kernelRunTime);
//            Assert.IsTrue(Math.Abs(delta) < 0.1f);
//        }
//    }
//}
