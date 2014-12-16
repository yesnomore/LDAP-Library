using System;
using System.Globalization;
using System.IO;
using LDAPLibrary;
using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class FileLoggerTest
    {
        private const string Test = "Test Log Message";
        private static readonly string FilePath = string.Format("{0}", AppDomain.CurrentDomain.BaseDirectory);
        private readonly ILogger _logger = new FileLogger(FilePath);

        [TestMethod, TestCategory("FileLogger")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the file Logger with path empty throw an exception")]
        public void FileLoggerInitEmptyPath()
        {
            new FileLogger("");
        }

        [TestMethod, TestCategory("FileLogger")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the file logger with path null throw an exception")]
        public void FileLoggerInitNullPath()
        {
            new FileLogger(null);
        }

        [TestMethod, TestCategory("FileLogger")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the file logger with unexisting path throw an exception")]
        public void FileLoggerNotExistPath()
        {
            new FileLogger(@"C:\CICCIOBAFFONENONESISTE");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapChangeUserPasswordError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapChangeUserPasswordError),
                "LDAP CHANGE USER PASSWORD ERROR");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapConnectionError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapConnectionError),
                "LDAP CONNECTION ERROR");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapConnectionSuccess()
        {
            AssertLogMessage(_logger.BuildLogMessage(Test, LdapState.LdapConnectionSuccess),
                "LDAP CONNECTION SUCCESS");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapCreateUserError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapCreateUserError),
                "LDAP CREATE USER ERROR");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapDeleteUserError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapDeleteUserError),
                "LDAP DELETE USER ERROR");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapGenericError()
        {
            AssertLogMessage(_logger.BuildLogMessage(Test, LdapState.LdapGenericError),
                "LDAP GENERIC ERROR");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapLibraryInitError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapLibraryInitError),
                "LDAP LIBRARY INIT ERROR");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapLibraryInitSuccess()
        {
            AssertLogMessage(_logger.BuildLogMessage(Test, LdapState.LdapLibraryInitSuccess),
                "LDAP LIBRARY INIT SUCCESS");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapModifyUserAttributeError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapModifyUserAttributeError),
                "LDAP MODIFY USER ATTRIBUTE ERROR");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapSearchUserError()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapSearchUserError),
                "LDAP SEARCH USER ERROR");
        }

        [TestMethod,TestCategory("FileLogger")]
        public void LdapUserManipulatorSuccess()
        {
            AssertLogMessageWithAddictionMessage(_logger.BuildLogMessage(Test, LdapState.LdapUserManipulatorSuccess),
                "LDAP USER MANIPULATION SUCCESS");
        }

        [TestMethod, TestCategory("FileLogger")]
        public void TestFileLogWrite()
        {
            var fileLogPath = AppDomain.CurrentDomain.BaseDirectory + @"\LDAPLog.txt";
            if (File.Exists(fileLogPath)) File.Delete(fileLogPath);

            _logger.Write(Test);

            Assert.IsTrue(File.Exists(fileLogPath));
        }

        private static void AssertLogMessageWithAddictionMessage(string logMessage,string logMessageAssertEqual)
        {
            var dateSplit = logMessage.Split('-');

            DateTime dateparse;
            if (!DateTime.TryParseExact(dateSplit[0].Remove(dateSplit[0].Length - 1),"dd/MM/yyyy HH:mm:ss tt",CultureInfo.InvariantCulture,DateTimeStyles.None,out dateparse)) 
                Assert.Fail();

            var stateSplit  = dateSplit[1].Substring(1).Split(':');

            Assert.AreEqual(stateSplit[1].Substring(1),Test);
            Assert.AreEqual(stateSplit[0],logMessageAssertEqual);
        }

         private static void AssertLogMessage(string logMessage,string logMessageAssertEqual)
        {
            var dateSplit = logMessage.Split('-');

            DateTime dateparse;
            if (!DateTime.TryParseExact(dateSplit[0].Remove(dateSplit[0].Length - 1), "dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateparse)) 
                Assert.Fail();

            Assert.AreEqual(dateSplit[1].Substring(1), logMessageAssertEqual);
        }

    }
}
