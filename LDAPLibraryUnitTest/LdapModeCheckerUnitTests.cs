using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using LDAPLibrary;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapModeCheckerUnitTests
    {
        #region Localhost Configuration

        private const AuthType AuthType = System.DirectoryServices.Protocols.AuthType.Basic;
        private const string Server = "127.0.0.1:389";

        private const string AdminUserDn = "cn=Manager,dc=maxcrc,dc=com";
        private const string AdminUserCn = "Manager";
        private const string AdminUserSn = "test";
        private const string AdminUserPassword = "secret";

        private const string SearchBaseDn = "o=ApexNet,ou=People,dc=maxcrc,dc=com";
        private const string UserObjectClass = "person";
        private const string MatchFieldUsername = "cn";
        private const LoggerType EnableLog = LoggerType.File;
        private const string LogPath = @"C:\work\LDAPLibrary\Log";
        private const bool SecureSocketLayer = false;
        private const bool TransportSocketLayer = false;
        private const bool ClientCertificate = false;
        private const string ClientCertificatePath = "null";
        private const LDAPAdminMode AdminMode = LDAPAdminMode.Admin;

        private static readonly LdapUser AdminUser = new LdapUser(AdminUserDn,
            AdminUserCn,
            AdminUserSn,
            new Dictionary<string, List<string>> {{"userPassword", new List<string> {AdminUserPassword}}});

        #endregion

        private readonly ILdapConfigRepository _configRepository = new LdapConfigRepository();

        [TestMethod, TestCategory("Mode Checker - Basic Mode")]
        public void BasicModeNoAdmin()
        {
            _configRepository.BasicLdapConfig(null,LDAPAdminMode.NoAdmin, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            var modeCheckerTests = new LdapModeChecker(_configRepository);

            Assert.IsTrue(modeCheckerTests.IsBasicMode());
            Assert.IsFalse(modeCheckerTests.IsCompleteMode());
        }

        [TestMethod, TestCategory("Mode Checker - Complete Mode")]
        public void CompleteMode()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(SecureSocketLayer,
                TransportSocketLayer, ClientCertificate, ClientCertificatePath, UserObjectClass, MatchFieldUsername);

            var modeCheckerTests = new LdapModeChecker(_configRepository);

            Assert.IsFalse(modeCheckerTests.IsBasicMode());
            Assert.IsTrue(modeCheckerTests.IsCompleteMode());
        }
    }
}