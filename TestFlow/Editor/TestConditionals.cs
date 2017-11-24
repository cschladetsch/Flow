using NUnit.Framework;

namespace Flow.Test
{								 
	class TestConditionals : TestBase
	{
		[Test]
		public void TestIf()
		{
			var worked = false;
			var exp = _factory.If(
				() => true, 
				_factory.Do(() => worked = true)
			);
			_root.Add(exp);
			Step(2);
			Assert.IsTrue(worked);

			worked = true;
			_root.Remove(exp);
			var exp2 = _factory.If(
				() => false, 
				_factory.Do(() => worked = true)
			);
			_root.Add(exp2);
			Step(2);
			Assert.IsFalse(worked);
		}
	}
}
