using System.Collections;
using NUnit.Framework;

namespace Flow.Test
{
	public class TestLoops : TestBase
	{
		[Test]
		public void TestDebugLog()
		{
			_root.Add(_flow.Log("Hello World"));
			Step(5);
		}

		[Test]
		public void TestCoro()
		{
			var f = _flow;
			_root.Add(
				f.Coroutine(CountTo, 10)//.SetName("Body")
			);

			count = 0;
			Step(20);
			Assert.AreEqual(count, 10);
		}

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
			var f = _flow;
			count = 0;
			_root.Add(
				f.While(
					() => ++count < 5, 
					f.Do(() => PrintFmt("count={0}", count))
				)
			);

			Step(1);
			Print(count);
			Assert.AreEqual(5, count);
		}

		[Test]
		public void TestWhileEarlyBreak()
		{
			var f = _flow;
			count = 0;
			_root.Add(
				f.While(
					() => true,
					f.Coroutine(BreakEarly)
				),
				f.While(
					() => true,
					f.Coroutine(BreakEarly)
				)
			);

			Step(2);
			Print(count);
			Assert.AreEqual(5, count);
			Assert.AreEqual(2, numBreakEarly);
		}

		private IEnumerator BreakEarly(IGenerator self)
		{
			++numBreakEarly;
			for (var n = 0; n < 3; ++n)
			{
				if (++count == 5)
					yield break;

				yield return self;
			}

			self.Suspend();
		}

		private int count = 0;
		private int numBreakEarly = 0;
	}
}
