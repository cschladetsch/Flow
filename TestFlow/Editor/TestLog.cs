using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Flow.Logger
{
    [TestFixture]
    public class TestLog
    {
        [Test]
        public void Test()
        {
            var log = new Logger(ELogLevel.Verbose, "test");
#if UNITY
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
