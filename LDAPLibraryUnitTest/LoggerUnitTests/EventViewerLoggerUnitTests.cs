using System.Diagnostics;
using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest.LoggerUnitTests
{
    [TestClass]
    public class EventViewerLoggerUnitTests
    {
        private const string Test = "Test Log Message";
        private const string EventLogEntrySource = "LDAPLibrary";
        private readonly ILogger _logger = new EventViewerLogger();

        /**
         * THE ALOGGER CLASS IS ALREADY TESTED BY THE FileLoggerAndALoggerUnitTests file
         * see the report generator
         */

        [TestMethod, TestCategory("EventViewerLogger")]
        public void TestEventViewerLoggerWrite()
        {
            _logger.Write(Test);
            var eventlog = new EventLog("Application");
            Assert.IsTrue(eventlog.Entries[eventlog.Entries.Count - 1].Message.Equals(Test));
            Assert.IsTrue(eventlog.Entries[eventlog.Entries.Count - 1].Source.Equals(EventLogEntrySource));
        }
    }
}