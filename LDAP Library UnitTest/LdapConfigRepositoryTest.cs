using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LDAPLibrary;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapConfigRepositoryTest
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


        [TestMethod, TestCategory("configRepository no Exception")]
        public void BasicConfig()
        {
            _configRepository.BasicLdapConfig(AdminUser, Server, SearchBaseDn, AuthType);

            Assert.AreEqual(AdminUser, _configRepository.GetAdminUser());
            Assert.AreEqual(Server, _configRepository.GetServer());
            Assert.AreEqual(SearchBaseDn, _configRepository.GetSearchBaseDn());
            Assert.AreEqual(AuthType, _configRepository.GetAuthType());

            //Standard configs

            Assert.AreEqual(StandardSecureSocketLayer, _configRepository.GetSecureSocketLayerFlag());
            Assert.AreEqual(StandardTransportSocketLayer, _configRepository.GetTransportSocketLayerFlag());
            Assert.AreEqual(StandardClientCertificate, _configRepository.GetClientCertificateFlag());
            Assert.AreEqual(StandardClientCertificatePath, _configRepository.GetClientCertificatePath());
            Assert.AreEqual(StandardEnableLog, _configRepository.GetWriteLogFlag());
            Assert.AreEqual(StandardLogPath, _configRepository.GetLogPath());
            Assert.AreEqual(StandardUserObjectClass, _configRepository.GetUserObjectClass());
            Assert.AreEqual(StandardMatchFieldUsername, _configRepository.GetMatchFieldName());

        }
        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with Server null or empty throw an exception")]
        public void BasicConfigNoServer()
        {
            _configRepository.BasicLdapConfig(AdminUser, "", SearchBaseDn, AuthType);
        }

        [TestMethod, TestCategory("configRepository no Exception")]
        public void BasicConfigNoSearchBaseDn()
        {
            _configRepository.BasicLdapConfig(AdminUser, Server, "", AuthType);
        }

        [TestMethod, TestCategory("configRepository no Exception")]
        public void BasicConfigNoAdmin()
        {
            _configRepository.BasicLdapConfig(null, Server, SearchBaseDn, AuthType);
        }

        [TestMethod, TestCategory("configRepository no Exception")]
        public void CompleteConfig()
        {
            _configRepository.CompleteLdapConfig(AdminUser, Server, SearchBaseDn, AuthType, SecureSocketLayer,
                TransportSocketLayer, ClientCertificate, ClientCertificatePath, EnableLog, LogPath, UserObjectClass, MatchFieldUsername);

            Assert.AreEqual(AdminUser, _configRepository.GetAdminUser());
            Assert.AreEqual(Server, _configRepository.GetServer());
            Assert.AreEqual(SearchBaseDn, _configRepository.GetSearchBaseDn());
            Assert.AreEqual(AuthType, _configRepository.GetAuthType());
            Assert.AreEqual(SecureSocketLayer, _configRepository.GetSecureSocketLayerFlag());
            Assert.AreEqual(TransportSocketLayer, _configRepository.GetTransportSocketLayerFlag());
            Assert.AreEqual(ClientCertificate, _configRepository.GetClientCertificateFlag());
            Assert.AreEqual(ClientCertificatePath, _configRepository.GetClientCertificatePath());
            Assert.AreEqual(EnableLog, _configRepository.GetWriteLogFlag());
            Assert.AreEqual(LogPath, _configRepository.GetLogPath());
            Assert.AreEqual(UserObjectClass, _configRepository.GetUserObjectClass());
            Assert.AreEqual(MatchFieldUsername, _configRepository.GetMatchFieldName());
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with Server null or empty throw an exception")]
        public void CompleteConfigNoServer()
        {
            _configRepository.CompleteLdapConfig(AdminUser,
                "",
                                                SearchBaseDn,
                                                AuthType,
                                                SecureSocketLayer,
                                                TransportSocketLayer,
                                                ClientCertificate,
                                                ClientCertificatePath,
                                                EnableLog,
                                                LogPath,
                                                UserObjectClass,
                                                MatchFieldUsername
                                                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with Certificate path null or empty throw an exception")]
        public void CompleteConfigNoCertificatePath()
        {
            _configRepository.CompleteLdapConfig(AdminUser,
                Server,
                                                SearchBaseDn,
                                                AuthType,
                                                SecureSocketLayer,
                                                TransportSocketLayer,
                                                ClientCertificate,
                                                "",
                                                EnableLog,
                                                LogPath,
                                                UserObjectClass,
                                                MatchFieldUsername
                                                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with log path null or empty throw an exception")]
        public void CompleteConfigNoLogPath()
        {
            _configRepository.CompleteLdapConfig(AdminUser,
                Server,
                                                SearchBaseDn,
                                                AuthType,
                                                SecureSocketLayer,
                                                TransportSocketLayer,
                                                ClientCertificate,
                                                ClientCertificatePath,
                                                EnableLog,
                                                "",
                                                UserObjectClass,
                                                MatchFieldUsername
                                                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with user class null or empty throw an exception")]
        public void CompleteConfigNoUserClass()
        {
            _configRepository.CompleteLdapConfig(AdminUser,
                Server,
                                                SearchBaseDn,
                                                AuthType,
                                                SecureSocketLayer,
                                                TransportSocketLayer,
                                                ClientCertificate,
                                                ClientCertificatePath,
                                                EnableLog,
                                                LogPath,
                                                "",
                                                MatchFieldUsername
                                                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with matchFieldUsername null or empty throw an exception")]
        public void CompleteConfigNoMatchFieldUsername()
        {
            _configRepository.CompleteLdapConfig(AdminUser,
                Server,
                                                SearchBaseDn,
                                                AuthType,
                                                SecureSocketLayer,
                                                TransportSocketLayer,
                                                ClientCertificate,
                                                ClientCertificatePath,
                                                EnableLog,
                                                LogPath,
                                                UserObjectClass,
                                                ""
                                                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with admin user null or empty throw an exception")]
        public void CompleteConfigNoAdmin()
        {
            _configRepository.CompleteLdapConfig(null,
                Server,
                                                SearchBaseDn,
                                                AuthType,
                                                SecureSocketLayer,
                                                TransportSocketLayer,
                                                ClientCertificate,
                                                ClientCertificatePath,
                                                EnableLog,
                                                LogPath,
                                                UserObjectClass,
                                                MatchFieldUsername
                                                );
        }
    }
}
