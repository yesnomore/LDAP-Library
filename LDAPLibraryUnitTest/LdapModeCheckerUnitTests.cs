using System.Collections.Generic;
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
        private const LoggerType EnableLog = LoggerType.File;
        private const string LogPath = @"C:\work\LDAPLibrary\Log";


        private static readonly LdapUser AdminUser = new LdapUser(AdminUserDn,
            AdminUserCn,
            AdminUserSn,
            new Dictionary<string, List<string>> {{"userPassword", new List<string> {AdminUserPassword}}});

        #endregion

        private readonly ILdapConfigRepository _configRepository = new LdapConfigRepository();

        [TestMethod, TestCategory("Mode Checker - Admin Mode")]
        public void AdminMode()
        {
            _configRepository.BasicLdapConfig(AdminUser,LDAPAdminMode.Admin, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            var modeCheckerTests = new LdapAdminModeChecker(_configRepository);

            Assert.IsTrue(modeCheckerTests.IsAdminMode());
            Assert.IsFalse(modeCheckerTests.IsNoAdminMode());
            Assert.IsFalse(modeCheckerTests.IsAnonymousMode());
        }

        [TestMethod, TestCategory("Mode Checker - NoAdmin Mode")]
        public void NoAdminMode()
        {
            _configRepository.BasicLdapConfig(AdminUser, LDAPAdminMode.NoAdmin, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            var modeCheckerTests = new LdapAdminModeChecker(_configRepository);

            Assert.IsTrue(modeCheckerTests.IsNoAdminMode());
            Assert.IsFalse(modeCheckerTests.IsAdminMode());
            Assert.IsFalse(modeCheckerTests.IsAnonymousMode());
        }

        [TestMethod, TestCategory("Mode Checker - NoAdmin Mode")]
        public void AnonymousMode()
        {
            _configRepository.BasicLdapConfig(AdminUser, LDAPAdminMode.Anonymous, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            var modeCheckerTests = new LdapAdminModeChecker(_configRepository);

            Assert.IsTrue(modeCheckerTests.IsAnonymousMode());
            Assert.IsFalse(modeCheckerTests.IsAdminMode());
            Assert.IsFalse(modeCheckerTests.IsNoAdminMode());
        }
    }
}