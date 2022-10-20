using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using LDAPLibrary;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
 * 
 * 
 * TEST TRAMITE LDAP LOCALE ( tramite ldap for windows vedi oneNote)
 * IMPORTANTE:
 * - AVERE UN A STRUTTURA COME QUESTA: o=ApexNet,ou=People,dc=maxcrc,dc=com
 * - AVERE ALL'INTERNO UN UTENTE DI NOME "MATTEO", VEDI COSTANTI DI CLASSE (ReadOnlyUserCn....)
 *   L'UTENTE WRITE INVECE SI CREA E SI CANCELLA DA SOLO A MENO DI FALLIMENTI NEI TEST.
 * 
 * 
 */

namespace LDAP_Library_UnitTest.localhost
{
    [TestClass]
    public class LocalhostEnvironmentInitTest
    {
        //Class fields for the test
        private ILdapManager _ldapManagerObj; //LDAPLibrary

        #region Localhost Configuration

        private const AuthType LdapAuthType = AuthType.Basic;
        private const string LdapServer = "127.0.0.1:389";

        private const string LdapAdminUserDn = "cn=Manager,dc=maxcrc,dc=com";
        private const string LdapAdminUserCn = "Manager";
        private const string LdapAdminUserSn = "test";
        private const string LdapAdminUserPassword = "secret";

        private const string LdapSearchBaseDn = "o=ApexNet,ou=People,dc=maxcrc,dc=com";
        private const string LdapUserObjectClass = "person";
        private const string LdapMatchFieldUsername = "cn";
        private const LoggerType EnableLdapLibraryLog = LoggerType.File;
        private const bool SecureSocketLayerFlag = false;
        private const bool TransportSocketLayerFlag = false;
        private const bool ClientCertificationFlag = false;
        private const string ClientCertificatePath = "null";
        private const LDAPAdminMode AdminMode = LDAPAdminMode.Admin;
        private static readonly TimeSpan connectionTimeout = new TimeSpan(0, 0, 30, 0);

        private static readonly LdapUser AdminUser = new LdapUser(LdapAdminUserDn,
            LdapAdminUserCn,
            LdapAdminUserSn,
            new Dictionary<string, List<string>> { { "userPassword", new List<string> { LdapAdminUserPassword } } });

        private static readonly string LdapLibraryLogPath = string.Format("{0}", AppDomain.CurrentDomain.BaseDirectory);

        #endregion

        #region LDAP Library Tests - Base

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibrary()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );

            Assert.IsFalse(_ldapManagerObj.Equals(null));
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT SUCCESS");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with Server null or empty throw an exception")]
        public void TestCompleteInitLibraryNoServer()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                "",
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with Certificate path null or empty throw an exception")]
        public void TestCompleteInitLibraryNoCertificatePathRequired()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                true,
                "",
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibraryNoCertificatePathNotRequired()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                "",
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
            Assert.IsFalse(_ldapManagerObj.Equals(null));
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT SUCCESS");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with log path null or empty throw an exception")]
        public void TestCompleteInitLibraryNoLogPath_File()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                "",
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibraryNoLogPath_EventViewer()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                LoggerType.EventViewer,
                "",
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibraryNoLogPath_None()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                LoggerType.None,
                "",
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with user class null or empty throw an exception")]
        public void TestCompleteInitLibraryNoUserClass()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                "",
                LdapMatchFieldUsername,
                connectionTimeout
                );
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with matchFieldUsername null or empty throw an exception")]
        public void TestCompleteInitLibraryNoMatchFieldUsername()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                "",
                connectionTimeout
                );
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with SearchBaseDn null or empty throw an exception")]
        public void TestCompleteInitLibraryNoSearchBaseDn()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                "",
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with Admin null or empty and LDAPAdminMode.Admin throw an exception")]
        public void TestCompleteInitLibraryNoAdmin_LDAPAdminMode_Admin()
        {
            _ldapManagerObj = new LdapManager(null, LDAPAdminMode.Admin,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibraryNoAdmin_LDAPAdminMode_NoAdmin()
        {
            _ldapManagerObj = new LdapManager(null, LDAPAdminMode.NoAdmin,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibraryNoAdmin_LDAPAdminMode_Anonymous()
        {
            _ldapManagerObj = new LdapManager(null, LDAPAdminMode.Anonymous,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibraryNoAdmin_SearchBaseDN_Null()
        {
            _ldapManagerObj = new LdapManager(null, LDAPAdminMode.NoAdmin,
                LdapServer,
                "",
                LdapAuthType,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag,
                ClientCertificatePath,
                EnableLdapLibraryLog,
                LdapLibraryLogPath,
                LdapUserObjectClass,
                LdapMatchFieldUsername,
                connectionTimeout
                );
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with Admin null or empty and LDAPAdminMode.Admin throw an exception")]
        public void TestStardardInitLibraryNoAdmin()
        {
            _ldapManagerObj = new LdapManager(null, LDAPAdminMode.Admin,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType, EnableLdapLibraryLog, LdapLibraryLogPath
                );

            Assert.IsFalse(_ldapManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestStandardInitLibrary()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode,
                LdapServer,
                LdapSearchBaseDn,
                LdapAuthType, EnableLdapLibraryLog, LdapLibraryLogPath
                );

            Assert.IsFalse(_ldapManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestStandardInitLibraryEventViewerLog()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode, LdapServer, LdapSearchBaseDn, LdapAuthType,
                LoggerType.EventViewer, LdapLibraryLogPath);
            Assert.IsFalse(_ldapManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestStandardInitLibraryEventViewerLogNoPath()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode, LdapServer, LdapSearchBaseDn, LdapAuthType,
                LoggerType.EventViewer, null);
            Assert.IsFalse(_ldapManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with Server null or empty throw an exception")]
        public void TestStandardInitLibraryNoServer()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode, "", LdapSearchBaseDn, LdapAuthType, EnableLdapLibraryLog,
                LdapLibraryLogPath);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with SearchBaseDn null or empty throw an exception")]
        public void TestStandardInitLibraryNoSearchBaseDn()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode, LdapServer, "", LdapAuthType, EnableLdapLibraryLog,
                LdapLibraryLogPath);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        [ExpectedException(typeof(ArgumentNullException),
            "The creation of the library with log type null or empty throw an exception")]
        public void TestStandardInitLibraryNologPath_File()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode, LdapServer, LdapSearchBaseDn, LdapAuthType,
                EnableLdapLibraryLog, null);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestStandardInitLibraryNologPath_EventViewer()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode, LdapServer, LdapSearchBaseDn, LdapAuthType,
                LoggerType.EventViewer, null);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestStandardInitLibraryNologPath_None()
        {
            _ldapManagerObj = new LdapManager(AdminUser, AdminMode, LdapServer, LdapSearchBaseDn, LdapAuthType,
                LoggerType.None, null);
        }
        #endregion
    }
}