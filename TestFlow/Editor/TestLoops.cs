using System.Collections;
using NUnit.Framework;

namespace Flow.Test
{
	public class TestLoops : TestBase
	{
		[Test]
		public void TestDebugLog()
		{
			_root.Add(_factory.Log("Hello World"));
			Step(5);
		}

		[Test]
		public void TestCoro()
		{
			var f = _factory;
			_root.Add(
				f.Coroutine(CountTo, 10).Named("Body")
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
		public void TestWhile()
		{
			var f = _factory;
			var count = 0;
			_root.Add(
				f.While(
					() => ++count < 5, 
					f.Do(() => _kernel.Trace.Log("count={0}", count))
				)
			);

			Step(2);
			Print(count);
			Assert.AreEqual(5, count);
		}
	}
}
