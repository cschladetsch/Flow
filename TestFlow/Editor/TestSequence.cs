using System.Collections.Generic;
using NUnit.Framework;

namespace Flow.Test
{
	class TestSequence : TestBase
	{
		[Test]
		public void TestSequenceTransients()
		{
			var f = _kernel.Factory;
			var seq = f.Sequence(f.Coroutine<int>(First), f.Coroutine<int>(Second));
			_root.Add(seq);
			Step(5);
		}

		IEnumerator<int> First(IGenerator self)
		{
			yield return 1;
			yield return 2;
		}

		IEnumerator<int> Second(IGenerator self)
		{
			yield return 3;
			yield return 4;
		}
	}
}
