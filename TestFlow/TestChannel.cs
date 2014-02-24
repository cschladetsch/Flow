using System;
using System.Collections.Generic;
using NUnit.Framework;
using Flow;

namespace TestFlow
{
	[TestFixture()]
	public class TestChannel
	{
		[Test()]
		public void TestInsertExtract()
		{
			var kernel = Create.NewKernel();
			var chan = kernel.Factory.NewChannel<int>();

			chan.Insert(1);
			chan.Insert(2);
			chan.Insert(3);

			var f0 = chan.Extract();
			var f1 = chan.Extract();
			var f2 = chan.Extract();

			StepKernel(kernel, 5);

			Assert.AreEqual(1, f0.Value);
			Assert.AreEqual(2, f1.Value);
			Assert.AreEqual(3, f2.Value);
		}

		[Test()]
		public void TestExtractInsert()
		{
			var kernel = Create.NewKernel();
			var chan = kernel.Factory.NewChannel<int>();

			var f0 = chan.Extract();
			var f1 = chan.Extract();
			var f2 = chan.Extract();

			chan.Insert(1);
			chan.Insert(2);
			chan.Insert(3);

			StepKernel(kernel, 5);

			Assert.AreEqual(1, f0.Value);
			Assert.AreEqual(2, f1.Value);
			Assert.AreEqual(3, f2.Value);
		}

		void StepKernel (IKernel kernel, int count)
		{
			for (var n = 0; n < count; ++n)
				kernel.Step();
		}

		/// <summary>
		/// Test one consumer using a channel, where the values are manually inserted into the channel.
		/// </summary>
		[Test()]
		public void TestProducerConsumer ()
		{
			var kernel = Create.NewKernel();
			var channel = kernel.Factory.NewChannel<int>();
			var con = new Consumer(kernel, channel);

			channel.Insert(1);
			StepKernel(kernel, 5);
			Assert.AreEqual(1, con.Sum);

			channel.Insert(1);
			StepKernel(kernel, 5);
			Assert.AreEqual(2, con.Sum);

			channel.Insert(1);
			StepKernel(kernel, 5);
			Assert.AreEqual(3, con.Sum);

			StepKernel(kernel, 5);
			Assert.AreEqual(3, con.Sum);
		}

        ///// <summary>
        ///// Test one producer and multiple consumers via a channel.
        ///// <para>The producer in this case is another coroutine</para>
        ///// </summary>
        //[Test()]
        //public void TestCoroProducerMultipleConsumer()
        //{
        //    var kernel = Create.NewKernel();
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

		public Consumer (IKernel kernel, IChannel<int> channel)
		{
			kernel.Factory.NewCoroutine(Step, channel);
		}

		public IEnumerator<bool> Step (IGenerator self, IChannel<int> channel)
		{
			while (true) 
			{
				var next = channel.Extract();
				yield return self.ResumeAfter(next);
				if (!next.Available)
					yield break;
				Sum += next.Value;
			}
		}
	}
}

