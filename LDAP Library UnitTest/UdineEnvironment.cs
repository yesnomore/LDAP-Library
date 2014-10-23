using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LDAPLibrary;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Net;

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

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class UdineEnvironment
    {
        //Class fields for the test
        ILdapManager _ldapManagerObj;                 //LDAPLibrary

        //READ ONLY USER
        private const string ReadOnlyUserUid = "uptest";
        private const string ReadOnlyUserPwd = "606FSxdklf7q";
        private const string ReadOnlyUserDn = "cn=" + ReadOnlyUserUid + ",ou=servizio,ou=utenti,dc=uniud,dc=it";

        #region LDAP Library Tests - Base

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibrary()
        {
            var authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        ConfigurationManager.AppSettings["LDAPAuthType"]);

            _ldapManagerObj = new LdapManager(new LdapUser(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                new Dictionary<string, List<string>> { { "userPassword", new List<string> { ConfigurationManager.AppSettings["LDAPAdminUserPassword"] } } }),
                                                ConfigurationManager.AppSettings["LDAPServer"],
                                                ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                authType,
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]),
                                                ConfigurationManager.AppSettings["clientCertificatePath"],
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["enableLDAPLibraryLog"]),
                                                ConfigurationManager.AppSettings["LDAPLibraryLogPath"],
                                                ConfigurationManager.AppSettings["LDAPUserObjectClass"],
                                                ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]
                                                );

            Assert.IsFalse(_ldapManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibraryNoAdmin()
        {
            var authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                       ConfigurationManager.AppSettings["LDAPAuthType"]);

            _ldapManagerObj = new LdapManager(null,
                                                ConfigurationManager.AppSettings["LDAPServer"],
                                                ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                authType,
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]),
                                                ConfigurationManager.AppSettings["clientCertificatePath"],
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["enableLDAPLibraryLog"]),
                                                ConfigurationManager.AppSettings["LDAPLibraryLogPath"],
                                                ConfigurationManager.AppSettings["LDAPUserObjectClass"],
                                                ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]
                                                );

            Assert.IsFalse(_ldapManagerObj.Equals(null));

        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestStardardInitLibraryNoAdmin()
        {
            var authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                       ConfigurationManager.AppSettings["LDAPAuthType"]);

            _ldapManagerObj = new LdapManager(null,
                                                    ConfigurationManager.AppSettings["LDAPServer"],
                                                    ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                    authType
                                                    );

            Assert.IsFalse(_ldapManagerObj.Equals(null));

        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestStandardInitLibrary()
        {

            var adminUser = new LdapUser(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                 ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                 ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                 null);
            adminUser.SetUserAttribute("userPassword", ConfigurationManager.AppSettings["LDAPAdminUserPassword"]);


            var authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        ConfigurationManager.AppSettings["LDAPAuthType"]);

            _ldapManagerObj = new LdapManager(adminUser,
                                                ConfigurationManager.AppSettings["LDAPServer"],
                                                ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                authType
                                                );

            Assert.IsFalse(_ldapManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestAdminConnect()
        {
            //Init the DLL
            TestCompleteInitLibrary();

            //Connect with admin user
            Assert.IsTrue(_ldapManagerObj.Connect());
        }

        #endregion
        
        #region LDAP Library Tests - Only Read Permission Required

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void TestSearchUser()
        {
            TestAdminConnect();

            string[] userIdToSearch =
            {
                ReadOnlyUserUid
            };
            var userAttributeToReturnBySearch = new List<string>
            {
				"description","uid"
			};

            List<LdapUser> returnUsers;

            var result = _ldapManagerObj.SearchUsers(userAttributeToReturnBySearch, userIdToSearch, out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers.Count, userIdToSearch.Length);
            Assert.AreEqual(returnUsers[0].GetUserAttribute("uid").First(), ReadOnlyUserUid);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void TestUserConnectWithoutWritePermissions()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
                TestAdminConnect();
            else
                //Init the DLL
                TestStandardInitLibrary();


            var result = _ldapManagerObj.Connect(new NetworkCredential(
                ReadOnlyUserDn, ReadOnlyUserPwd,
                ""),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void TestSearchUserAndConnectWithoutWritePermissions()
        {
            TestAdminConnect();

            var result = _ldapManagerObj.SearchUserAndConnect(ReadOnlyUserUid, ReadOnlyUserPwd);

            Assert.IsTrue(result);
        }

        #endregion
    }
}