using System.Collections.Generic;
using NUnit.Framework;

namespace Flow.Test
{
    public class TestChannel : TestBase
    {
        [Test()]
        public void TestInsertExtract()
        {
            var chan = New.Channel<int>();

            chan.Insert(1);
            chan.Insert(2);
            chan.Insert(3);

            var f0 = chan.Extract();
            var f1 = chan.Extract();
            var f2 = chan.Extract();
            var f3 = chan.Extract();

            Root.Add(chan);
            Step(5);

            Assert.IsTrue(f0.Available);
            Assert.IsTrue(f1.Available);
            Assert.IsTrue(f2.Available);
            Assert.IsFalse(f3.Available);

            Assert.AreEqual(1, f0.Value);
            Assert.AreEqual(2, f1.Value);
            Assert.AreEqual(3, f2.Value);
        }

        [Test()]
        public void TestExtractInsert()
        {
            var chan = New.Channel<int>();

            var f0 = chan.Extract();
            var f1 = chan.Extract();
            var f2 = chan.Extract();

            chan.Insert(1);
            chan.Insert(2);
            chan.Insert(3);

            Root.Add(chan);
            Step(5);

            Assert.AreEqual(1, f0.Value);
            Assert.AreEqual(2, f1.Value);
            Assert.AreEqual(3, f2.Value);
        }

        /// <summary>
        /// Test one consumer using a channel, where the values are manually inserted into the channel.
        /// </summary>
        [Test()]
        public void TestProducerConsumer()
        {
            var channel = Kernel.Factory.Channel<int>();
            var con = new Consumer(Kernel, channel);

            Root.Add(channel);
            channel.Insert(1);
            Step(5);
            Assert.AreEqual(1, con.Sum);

            channel.Insert(1);
            Step(5);
            Assert.AreEqual(2, con.Sum);

            channel.Insert(1);
            Step(5);
            Assert.AreEqual(3, con.Sum);

            Step(5);
            Assert.AreEqual(3, con.Sum);
        }

        ///// <summary>
        ///// Test one producer and multiple consumers via a channel.
        ///// <para>The producer in this case is another coroutine</para>
        ///// </summary>
        //[Test()]
        //public void TestCoroProducerMultipleConsumer()
        //{
        //    var kernel = Create.Kernel();
        //    var prod = kernel.Factory.NewCoroutine(Producer);
        //    var channel = kernel.Factory.NewChannel<int>(prod);

        //    var con1 = new Consumer(kernel, channel);
        //    var con2 = new Consumer(kernel, channel);

        //    StepKernel(kernel, 50);

        //    Assert.AreEqual(15, con1.Sum + con2.Sum);
        //}

        IEnumerator<int> Producer(IGenerator self)
        {
            yield return 0;
            yield return 1;
            yield return 2;
            yield return 4;
            yield return 8;
        }
    }

    class Consumer
    {
        public int Sum;

        public Consumer(IKernel kernel, IChannel<int> channel)
        {
            // this is ambiguous, but it shouldn;t be....
            //kernel.Root.Add(kernel.Factory.Coroutine(Step, channel));
            kernel.Root.Add(kernel.Factory.Coroutine<bool, IChannel<int>>(Step, channel));
        }

        public IEnumerator<bool> Step(IGenerator self, IChannel<int> channel)
        {
            while (true)
            {
                var next = channel.Extract();
                yield return self.ResumeAfter(next) != null;
                if (!next.Available)
                    yield break;
                Sum += next.Value;
            }
        }
    }
}

