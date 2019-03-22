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
            Root.Add(
                New.Switch(New.Value(val),
                    New.Case(1, New.Do(() => val *= 2)),
                    New.Case(2, New.Do(() => val *= 3)),
                    New.Case(3, New.Do(() => val *= 4))
                )
            );

            Step();

            Assert.AreEqual(expect, val);
        }
    }
}
