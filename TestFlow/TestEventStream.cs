using System;
using NUnit.Framework;
using Flow;

namespace TestFlow
{
	[TestFixture()]
	public class TestEventStream
	{
		[Test()]
		public void TestCase ()
		{
			//var kernel = Global.NewKernel();

			//var f = kernel.Factory.NewFuture<int>();

			//var ev = f.NewEventStream(f.Arrived);

		}

		object NewEventStream<A0>(Flow.Action<A0> act)
		{
			return null;
		}
	}
}

