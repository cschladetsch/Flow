using System.Linq;
using NUnit.Framework;

namespace Flow.Test
{								 
	class TestConditionals : TestBase
	{
		[Test]
		public void TestIf()
		{
			_kernel.DebugLevel = Flow.EDebugLevel.Verbose;
			var executed = false;
			var exp = _flow.If(
				() => true,
				_flow.Do(() => executed = true)
			).Named("IF1");

			_root.Add(exp);
			Step(2);
			Assert.IsTrue(executed);

			executed = false;
			var exp2 = _flow.If(
				() => false, 
				_flow.Do(() => executed = true)
			).Named("If2");

			_root.Remove(exp);
			_root.Add(exp2);

			Step(2);
			Assert.IsFalse(executed);
		}
	}
}
