using System;
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
    public class LocalhostEnvironment
    {
        //Class fields for the test
        ILdapManager _ldapManagerObj;                 //LDAPLibrary

        //READ ONLY USER
        private const string ReadOnlyUserCn = "Matteo";
        private const string ReadOnlyUserPwd = "1";
        private const string ReadOnlyUserDn = "cn=" + ReadOnlyUserCn + ",o=ApexNet,ou=People,dc=maxcrc,dc=com";
        //WRITE USER THIS MUST NOT EXIST INITIALLY
        private const string WriteUserCn = "Fabio";
        private const string WriteUserPwd = "1";
        private const string WriteUserDn = "cn=" + WriteUserCn + ",o=ApexNet,ou=People,dc=maxcrc,dc=com";



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

        #region LDAP Library Tests - Write Permission Required

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestCreateUser()
        {
            var tempUser = new LdapUser(WriteUserDn, WriteUserCn, "test", null);

            //Init the DLL and connect the admin
            TestAdminConnect();

            //Create user
            var result = _ldapManagerObj.CreateUser(tempUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Create User Operation Success");

            result = _ldapManagerObj.DeleteUser(tempUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestDeleteUser()
        {
            //Set the test user
            var testLdapUser = new LdapUser(WriteUserDn, WriteUserCn, "test", null);

            //Init the DLL and connect the admin
            TestAdminConnect();

            //Create LDAPUser to delete.
            var result = _ldapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            //Delete user
            result = _ldapManagerObj.DeleteUser(testLdapUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Delete User Operation Success");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestModifyUserAttribute()
        {
            TestAdminConnect();
            var testLdapUser = new LdapUser(WriteUserDn, WriteUserCn, "test", new Dictionary<string, List<string>> { { "description", new List<string> { "test" } } });
            var result = _ldapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            List<LdapUser> returnUsers;
            const string userAttributeValue = "description Modified";

            result = _ldapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Replace, testLdapUser, "description", userAttributeValue);

            Assert.IsTrue(result);

            result = _ldapManagerObj.SearchUsers(
                new List<string> { "description" },
                new[] { WriteUserCn },
                out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers[0].GetUserCn(), testLdapUser.GetUserCn());
            Assert.AreEqual(returnUsers[0].GetUserAttribute("description")[0], userAttributeValue);

            result = _ldapManagerObj.DeleteUser(testLdapUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestChangeUserPassword()
        {
            TestAdminConnect();
            const string newPassword = "pippo";
            var testUser = new LdapUser(WriteUserDn, WriteUserCn, "test", new Dictionary<string, List<string>> { { "userPassword", new List<string> { WriteUserPwd } } });
            //Create the user
            var result = _ldapManagerObj.CreateUser(testUser);

            Assert.IsTrue(result);

            //Perform change of password
            result = _ldapManagerObj.ChangeUserPassword(testUser, newPassword);
            Assert.IsTrue(result);

            //Try to connect with the old password
            var testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                WriteUserPwd,
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsFalse(result);

            //Try to connect with the new password
            testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                newPassword,
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);

            TestAdminConnect();

            result = _ldapManagerObj.DeleteUser(testUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestUserConnect()
        {
            var testUser = new LdapUser(WriteUserDn, WriteUserCn, "test", new Dictionary<string, List<string>> { { "userPassword", new List<string> { WriteUserPwd } } });

            TestAdminConnect();
            bool result = _ldapManagerObj.CreateUser(testUser);
            Assert.IsTrue(result);



            var testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                testUser.GetUserAttribute("userPassword")[0],
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);

            TestAdminConnect();
            result = _ldapManagerObj.DeleteUser(testUser);
            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestSearchUserAndConnect()
        {

            TestAdminConnect();
            var testLdapUser = new LdapUser(WriteUserDn, WriteUserCn, "test", new Dictionary<string, List<string>> { { "userPassword", new List<string> { WriteUserPwd } } });

            var result = _ldapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            result = _ldapManagerObj.SearchUserAndConnect(WriteUserCn, WriteUserPwd);

            Assert.IsTrue(result);

            TestAdminConnect();

            result = _ldapManagerObj.DeleteUser(testLdapUser);

            Assert.IsTrue(result);
        }

        #endregion

        #region LDAP Library Tests - Only Read Permission Required

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void TestSearchUser()
        {
            TestAdminConnect();

            string[] userIdToSearch =
            {
                ReadOnlyUserCn
            };
            var userAttributeToReturnBySearch = new List<string>
            {
				"description"
			};

            List<LdapUser> returnUsers;

            var result = _ldapManagerObj.SearchUsers(userAttributeToReturnBySearch, userIdToSearch, out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers.Count, userIdToSearch.Length);
            Assert.AreEqual(returnUsers[0].GetUserCn(), ReadOnlyUserCn);
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

            var result = _ldapManagerObj.SearchUserAndConnect(ReadOnlyUserCn, ReadOnlyUserPwd);

            Assert.IsTrue(result);
        }

        #endregion
    }
}