using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Runtime.InteropServices;
using LDAPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapModeCheckerTests
    {

        #region Localhost Configuration

        private const AuthType AuthType = System.DirectoryServices.Protocols.AuthType.Basic;
        private const string Server = "127.0.0.1:389";

        private const string AdminUserDn = "cn=Manager,dc=maxcrc,dc=com";
        private const string AdminUserCn = "Manager";
        private const string AdminUserSn = "test";
        private const string AdminUserPassword = "secret";
        private static readonly LdapUser AdminUser = new LdapUser(AdminUserDn,
            AdminUserCn,
            AdminUserSn,
            new Dictionary<string, List<string>> { { "userPassword", new List<string> { AdminUserPassword } } });

        private const string SearchBaseDn = "o=ApexNet,ou=People,dc=maxcrc,dc=com";
        private const string UserObjectClass = "person";
        private const string MatchFieldUsername = "cn";
        private const bool EnableLog = true;
        private const string LogPath = @"C:\work\LDAPLibrary\Log\";
        private const bool SecureSocketLayer = false;
        private const bool TransportSocketLayer = false;
        private const bool ClientCertificate = false;
        private const string ClientCertificatePath = "null";

        #endregion

        #region StandardConfigValues

        private const bool StandardSecureSocketLayer = false;
        private const bool StandardTransportSocketLayer = false;
        private const bool StandardClientCertificate = false;
        private const string StandardClientCertificatePath = "";
        private const bool StandardEnableLog = false;
        private const string StandardLogPath = "";
        private const string StandardUserObjectClass = "person";
        private const string StandardMatchFieldUsername = "cn";

        #endregion

        private readonly ILdapConfigRepository _configRepository = new LdapConfigRepository();

        [TestMethod, TestCategory("Mode Checker - Basic Mode")]
        public void BasicMode()
        {
            _configRepository.BasicLdapConfig(null, Server, SearchBaseDn, AuthType);

            var modeCheckerTests = new LdapModeChecker(_configRepository);

            Assert.IsTrue(modeCheckerTests.IsBasicMode());
            Assert.IsFalse(modeCheckerTests.IsCompleteMode());
        }

        [TestMethod, TestCategory("Mode Checker - Complete Mode")]
        public void CompleteMode()
        {
            _configRepository.BasicLdapConfig(AdminUser, Server, SearchBaseDn, AuthType);

            _configRepository.AdditionalLdapConfig(SecureSocketLayer,
                TransportSocketLayer, ClientCertificate, ClientCertificatePath, EnableLog, LogPath, UserObjectClass, MatchFieldUsername);

            var modeCheckerTests = new LdapModeChecker(_configRepository);

            Assert.IsFalse(modeCheckerTests.IsBasicMode());
            Assert.IsTrue(modeCheckerTests.IsCompleteMode());
        }
    }
}
