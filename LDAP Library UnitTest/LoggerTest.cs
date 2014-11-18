using System;
using LDAPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LoggerTest
    {

        private const string test = "Test Log Message";
        private const string filePath = "C:\work\LDAPLibrary\Log\";
        private ILogger logger = new FileLogger(filePath);

        [TestMethod, TestCategory("Logger")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the logger with path empty throw an exception")]
        public void FileLoggerInitEmptyPath()
        {
            ILogger logger = new FileLogger("");
        }

        [TestMethod, TestCategory("Logger")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the logger with path null throw an exception")]
        public void FileLoggerInitNullPath()
        {
            ILogger logger = new FileLogger(null);
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapChangeUserPasswordError()
        {
            AssertLogMessageWithAddictionMessage(logger.BuildLogMessage(test, LdapState.LdapChangeUserPasswordError),
                "LDAP CHANGE USER PASSWORD ERROR");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapConnectionError()
        {
            AssertLogMessageWithAddictionMessage(logger.BuildLogMessage(test, LdapState.LdapConnectionError),
                "LDAP CONNECTION ERROR");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapConnectionSuccess()
        {
            AssertLogMessage(logger.BuildLogMessage(test, LdapState.LdapConnectionSuccess),
                "LDAP CONNECTION SUCCESS");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapCreateUserError()
        {
            AssertLogMessageWithAddictionMessage(logger.BuildLogMessage(test, LdapState.LdapCreateUserError),
                "LDAP CREATE USER ERROR");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapDeleteUserError()
        {
            AssertLogMessageWithAddictionMessage(logger.BuildLogMessage(test, LdapState.LdapDeleteUserError),
                "LDAP DELETE USER ERROR");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapGenericError()
        {
            AssertLogMessage(logger.BuildLogMessage(test, LdapState.LdapGenericError),
                "LDAP GENERIC ERROR");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapLibraryInitError()
        {
            AssertLogMessageWithAddictionMessage(logger.BuildLogMessage(test, LdapState.LdapLibraryInitError),
                "LDAP LIBRARY INIT ERROR");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapLibraryInitSuccess()
        {
            AssertLogMessage(logger.BuildLogMessage(test, LdapState.LdapLibraryInitSuccess),
                "LDAP LIBRARY INIT SUCCESS");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapModifyUserAttributeError()
        {
            AssertLogMessageWithAddictionMessage(logger.BuildLogMessage(test, LdapState.LdapModifyUserAttributeError),
                "LDAP MODIFY USER ATTRIBUTE ERROR");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapSearchUserError()
        {
            AssertLogMessageWithAddictionMessage(logger.BuildLogMessage(test, LdapState.LdapSearchUserError),
                "LDAP SEARCH USER ERROR");
        }

        [TestMethod,TestCategory("Logger")]
        public void LdapUserManipulatorSuccess()
        {
            AssertLogMessageWithAddictionMessage(logger.BuildLogMessage(test, LdapState.LdapUserManipulatorSuccess),
                "LDAP USER MANIPULATION SUCCESS");
        }

        private static void AssertLogMessageWithAddictionMessage(string logMessage,string logMessageAssertEqual)
        {
            var dateSplit = logMessage.Split('-');

            DateTime dateparse;
            if (!DateTime.TryParse(dateSplit[0],out dateparse)) Assert.Fail();

            var stateSplit  = dateSplit[1].Split(':');

            Assert.AreEqual(stateSplit[1],test);
            Assert.AreEqual(stateSplit[0],logMessageAssertEqual);
        }

         private static void AssertLogMessage(string logMessage,string logMessageAssertEqual)
        {
            var dateSplit = logMessage.Split('-');

            DateTime dateparse;
            if (!DateTime.TryParse(dateSplit[0],out dateparse)) Assert.Fail();

            Assert.AreEqual(dateSplit[1],logMessageAssertEqual);
        }

    }
}
