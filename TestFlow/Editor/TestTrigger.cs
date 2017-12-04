using Flow;
using Flow.Test;
using NUnit.Framework;

namespace Flow.Test
{
	[TestFixture]
	public class TestTrigger : TestBase
	{
		[Test]
		public void TriggerTest()
		{
			var trans1 = _flow.Transient();
			var trans2 = _flow.Transient();
			var trigger = _flow.Trigger(trans1, trans2);

			_root.Add(trigger);

			Step();

			Assert.IsTrue(trigger.Active);
			Assert.IsTrue(trans1.Active);
			Assert.IsTrue(trans2.Active);

			trans1.Complete();

			Step();

			Assert.IsFalse(trigger.Active);
			Assert.IsFalse(trans1.Active);
			Assert.IsTrue(trans2.Active);
		}

		[Test]
		public void TriggerFutureTest()
		{
			var trigger = _flow.Trigger();
			var future1 = _flow.Future<int>();
			var future2 = _flow.Future<string>();

			trigger.Add(future1);
			trigger.Add(future2);

			_root.Add(trigger, future1, future2);
			Step(2);

			Assert.IsTrue(trigger.Active);
			Assert.IsTrue(future1.Active);
			Assert.IsTrue(future2.Active);

			Step();

			future1.Value = 123;

			Step();

			Assert.AreEqual(123, future1.Value);
			Assert.IsFalse(trigger.Active);
			Assert.IsFalse(future1.Active);
			Assert.IsTrue(future2.Active);

			future2.Value = "foo";
			Step();
			Assert.IsFalse(future2.Active);
			Assert.AreEqual("foo", future2.Value);
		}
	}
}
