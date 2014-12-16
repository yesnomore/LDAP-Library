using System;
using System.Diagnostics;
using System.Globalization;
using LDAPLibrary;
using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class EventViewerLoggerTest
    {
        private const string Test = "Test Log Message";
        private const string EventLogEntrySource = "LDAPLibrary";
        private readonly ILogger _logger = new EventViewerLogger();

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapChangeUserPasswordError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapChangeUserPasswordError),
                "LDAP CHANGE USER PASSWORD ERROR");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapConnectionError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapConnectionError),
                "LDAP CONNECTION ERROR");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapConnectionSuccess()
        {
            AssertLogMessage(_logger.BuildLogMessage(Test, LdapState.LdapConnectionSuccess),
                "LDAP CONNECTION SUCCESS");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapCreateUserError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapCreateUserError),
                "LDAP CREATE USER ERROR");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapDeleteUserError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapDeleteUserError),
                "LDAP DELETE USER ERROR");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapGenericError()
        {
            AssertLogMessage(_logger.BuildLogMessage(Test, LdapState.LdapGenericError),
                "LDAP GENERIC ERROR");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapLibraryInitError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapLibraryInitError),
                "LDAP LIBRARY INIT ERROR");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapLibraryInitSuccess()
        {
            AssertLogMessage(_logger.BuildLogMessage(Test, LdapState.LdapLibraryInitSuccess),
                "LDAP LIBRARY INIT SUCCESS");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapModifyUserAttributeError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapModifyUserAttributeError),
                "LDAP MODIFY USER ATTRIBUTE ERROR");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapSearchUserError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapSearchUserError),
                "LDAP SEARCH USER ERROR");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void LdapUserManipulatorSuccess()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapUserManipulatorSuccess),
                "LDAP USER MANIPULATION SUCCESS");
        }

        [TestMethod, TestCategory("EventViewerLogger")]
        public void TestEventViewerLoggerWrite()
        {
            _logger.Write(Test);
            var eventlog = new EventLog("Application");
            Assert.IsTrue(eventlog.Entries[eventlog.Entries.Count - 1].Message.Equals(Test));
            Assert.IsTrue(eventlog.Entries[eventlog.Entries.Count - 1].Source.Equals(EventLogEntrySource));
        }

        private static void AssertLogMessageWithAddictionMessage(string logMessage, string logMessageAssertEqual)
        {
            var dateSplit = logMessage.Split('-');

            DateTime dateparse;
            if (!DateTime.TryParseExact(dateSplit[0].Remove(dateSplit[0].Length - 1), "dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateparse))
                Assert.Fail();

            var stateSplit = dateSplit[1].Substring(1).Split(':');

            Assert.AreEqual(stateSplit[1].Substring(1), Test);
            Assert.AreEqual(stateSplit[0], logMessageAssertEqual);
        }

        private static void AssertLogMessage(string logMessage, string logMessageAssertEqual)
        {
            var dateSplit = logMessage.Split('-');

            DateTime dateparse;
            if (!DateTime.TryParseExact(dateSplit[0].Remove(dateSplit[0].Length - 1), "dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateparse))
                Assert.Fail();

            Assert.AreEqual(dateSplit[1].Substring(1), logMessageAssertEqual);
        }
    }
}
