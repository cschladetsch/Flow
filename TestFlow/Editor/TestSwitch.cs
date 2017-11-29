using System;
using NUnit.Framework;

namespace Flow.Test
{
    [TestFixture]
    public class TestSwitch : TestBase
    {
        [Test]
        public void TestSwitchStatement()
        {
            int val = 0;
            _flow.Switch(_flow.Value(1),
                 _flow.Case(1, _flow.Do(() => val = 1)),
                 _flow.Case(2, _flow.Do(() => val = 2)),
                 _flow.Case(3, _flow.Do(() => val = 3))
            );

            Assert.AreEqual(1, val);
        }
    }
}
