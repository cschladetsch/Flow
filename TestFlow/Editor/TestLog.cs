/*
using System;
using NUnit.Framework;
using Flow.Logger;

namespace Flow.Test
{
    [TestFixture]
    public class TestLog : TestBase
    {
        [Test]
        public void Test()
        {
            var log = new Logger.Logger(ELogEntryType.Everything, "test");
#if UNITY_EDITOR
            log.AddLogger(new UnityLogger());
#endif
            //log.AddFile("testlog.txt");
			log.Log("log1");
			log.Warn("warn1");
			log.Error("Error1");
			//log.Flush();
		}
	}
}

*/
