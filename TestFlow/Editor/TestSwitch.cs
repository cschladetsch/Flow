using NUnit.Framework;

namespace Flow.Test
{
	[TestFixture]
	public class TestSwitch : TestBase
	{
		[TestCase(1, 2)]
		[TestCase(2, 6)]
		[TestCase(3, 12)]
		public void TestSwitchStatement(int val, int expect)
		{
			_root.Add(
				_flow.Switch(_flow.Value(val),
					_flow.Case(1, _flow.Do(() => val *= 2)),
					_flow.Case(2, _flow.Do(() => val *= 3)),
					_flow.Case(3, _flow.Do(() => val *= 4))
				)
			);

			Step();

			Assert.AreEqual(expect, val);
		}
	}
}
