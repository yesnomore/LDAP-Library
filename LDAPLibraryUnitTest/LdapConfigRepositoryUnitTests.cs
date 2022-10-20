﻿using System;
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
    public class LdapConfigRepositoryUnitTests
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
        private const string LogPath = @"C:\work\LDAPLibrary\Log\";
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

        #region StandardConfigValues

        private const bool StandardSecureSocketLayer = false;
        private const bool StandardTransportSocketLayer = false;
        private const bool StandardClientCertificate = false;
        private const string StandardClientCertificatePath = "";
        private const LoggerType StandardEnableLog = LoggerType.EventViewer;
        private const string StandardLogPath = "";
        private const string StandardUserObjectClass = "person";
        private const string StandardMatchFieldUsername = "cn";
        private static readonly TimeSpan connectionTimeout = new TimeSpan(0, 0, 30, 0);

        #endregion

        private readonly ILdapConfigRepository _configRepository = new LdapConfigRepository();


        [TestMethod, TestCategory("configRepository no Exception")]
        public void BasicConfig()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, Server, SearchBaseDn, AuthType, StandardEnableLog, StandardLogPath);

            Assert.AreEqual(AdminUser, _configRepository.GetAdminUser());
            Assert.AreEqual(AdminMode, _configRepository.GetAdminMode());
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
        [ExpectedException(typeof (ArgumentNullException),
            "The creation of the configRepository with Server null or empty throw an exception")]
        public void BasicConfigNoServer()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, "", SearchBaseDn, AuthType, StandardEnableLog, LogPath);
        }

        [TestMethod, TestCategory("configRepository no Exception")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with Server null or empty throw an exception")]
        public void BasicConfigNoSearchBaseDn()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, Server, "", AuthType, StandardEnableLog, LogPath);
        }

        [TestMethod, TestCategory("configRepository no Exception")]
        public void BasicConfigNoAdmin_LDAPAdminMode_NoAdmin()
        {
            _configRepository.BasicLdapConfig(null, LDAPAdminMode.NoAdmin, Server, SearchBaseDn, AuthType, StandardEnableLog, LogPath);
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with Server null or empty throw an exception")]
        public void BasicConfigNoAdmin_LDAPAdminMode_Admin()
        {
            _configRepository.BasicLdapConfig(null, LDAPAdminMode.Admin, Server, SearchBaseDn, AuthType, StandardEnableLog, LogPath);
        }

        [TestMethod, TestCategory("configRepository no Exception")]
        public void BasicConfigNoAdmin_LDAPAdminMode_Anonymous()
        {
            _configRepository.BasicLdapConfig(null, LDAPAdminMode.Anonymous, Server, SearchBaseDn, AuthType, StandardEnableLog, LogPath);
        }

        [TestMethod, TestCategory("configRepository no Exception")]
        public void CompleteConfig()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(SecureSocketLayer,
                TransportSocketLayer, ClientCertificate, ClientCertificatePath, UserObjectClass, MatchFieldUsername,connectionTimeout);

            Assert.AreEqual(AdminUser, _configRepository.GetAdminUser());
            Assert.AreEqual(AdminMode, _configRepository.GetAdminMode());
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
            Assert.AreEqual(connectionTimeout, _configRepository.GetConnectionTimeout());
        }

        [TestMethod, TestCategory("configRepository no Exceptions")]
        public void CompleteConfigNoConnectionTimeout()
        {
            _configRepository.BasicLdapConfig(AdminUser, AdminMode, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(
                SecureSocketLayer,
                TransportSocketLayer,
                ClientCertificate,
                ClientCertificatePath,
                UserObjectClass,
                MatchFieldUsername,
                null
                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof (ArgumentNullException),
            "The creation of the configRepository with Certificate path null or empty throw an exception")]
        public void CompleteConfigNoCertificatePath()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(
                SecureSocketLayer,
                TransportSocketLayer,
                true,
                "",
                UserObjectClass,
                MatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof (ArgumentNullException),
            "The creation of the configRepository with log path null or empty throw an exception")]
        public void CompleteConfigNoLogPath()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, Server, SearchBaseDn, AuthType, EnableLog, "");
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof (ArgumentNullException),
            "The creation of the configRepository with user class null or empty throw an exception")]
        public void CompleteConfigNoUserClass()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(
                SecureSocketLayer,
                TransportSocketLayer,
                ClientCertificate,
                ClientCertificatePath,
                "",
                MatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the configRepository with SearchBaseDn null or empty throw an exception")]
        public void CompleteConfigNoSearchBaseDn()
        {
            _configRepository.BasicLdapConfig(AdminUser, AdminMode, Server, "", AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(
                SecureSocketLayer,
                TransportSocketLayer,
                ClientCertificate,
                ClientCertificatePath,
                UserObjectClass,
                MatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof (ArgumentNullException),
            "The creation of the configRepository with matchFieldUsername null or empty throw an exception")]
        public void CompleteConfigNoMatchFieldUsername()
        {
            _configRepository.BasicLdapConfig(AdminUser,AdminMode, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(
                SecureSocketLayer,
                TransportSocketLayer,
                ClientCertificate,
                ClientCertificatePath,
                UserObjectClass,
                "",
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        [ExpectedException(typeof (ArgumentNullException),
            "The creation of the configRepository with admin user null or empty throw an exception")]
        public void CompleteConfigNoAdmin_LDAPAdminMode_Admin()
        {
            _configRepository.BasicLdapConfig(null,LDAPAdminMode.Admin, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(
                SecureSocketLayer,
                TransportSocketLayer,
                ClientCertificate,
                ClientCertificatePath,
                UserObjectClass,
                MatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        public void CompleteConfigNoAdmin_LDAPAdminMode_NoAdmin()
        {
            _configRepository.BasicLdapConfig(null, LDAPAdminMode.NoAdmin, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(
                SecureSocketLayer,
                TransportSocketLayer,
                ClientCertificate,
                ClientCertificatePath,
                UserObjectClass,
                MatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("configRepository Exceptions")]
        public void CompleteConfigNoAdmin_LDAPAdminMode_Anonymous()
        {
            _configRepository.BasicLdapConfig(null,LDAPAdminMode.Anonymous, Server, SearchBaseDn, AuthType, EnableLog, LogPath);

            _configRepository.AdditionalLdapConfig(
                SecureSocketLayer,
                TransportSocketLayer,
                ClientCertificate,
                ClientCertificatePath,
                UserObjectClass,
                MatchFieldUsername,
                connectionTimeout
                );
        }
    }
}