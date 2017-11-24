using System.Collections;
using NUnit.Framework;

namespace Flow.Test
{
	public class TestLoops : TestBase
	{
		[Test]
		public void TestDebugLog()
		{
			_root.Add(_factory.DebugLog("Hello World"));
			Step(5);
		}

		[Test]
		public void TestLoop()
		{
			var f = _factory;
			_root.Add(
				f.Do(() => count = 0),
				f.Loop(
					f.Coroutine(CountTo, 10)
				)
			);

			count = 0;
			Step(20);
			Assert.AreEqual(count, 10);
		}

		private int count = 0;

		IEnumerator CountTo(IGenerator self, int max)
		{
			while (++count != max)
			{
				yield return 0;
			}
		}

		[Test]
		public void TestConditionalLoop()
		{
			var f = _factory;
			var count = 0;
			_root.Add(f.While(() => ++count < 5, f.DebugLog("count={0}", count)));
			Step(10);
			Print(count);
			Assert.AreEqual(count, 5);
		}
	}
}
