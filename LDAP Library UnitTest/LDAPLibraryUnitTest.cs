using System;
using LDAPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Net;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LDAPLibraryUnitTest
    {
        //Class fields for the test
        ILdapManager LdapManagerObj;                 //LDAPLibrary
        string[] LDAPMatchSearchField;				 //Field to search

        #region LDAP Library Tests - Base

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testCompleteInitLibrary()
        {

            LdapUser adminUser = new LdapUser(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                null);
            adminUser.CreateUserAttribute("userPassword", ConfigurationManager.AppSettings["LDAPAdminUserPassword"]);

            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        ConfigurationManager.AppSettings["LDAPAuthType"]);

            LdapManagerObj = new LdapManager(adminUser,
                                                ConfigurationManager.AppSettings["LDAPServer"],
                                                ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                authType,
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]),
                                                ConfigurationManager.AppSettings["clientCertificatePath"],
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["enableLDAPLibraryLog"]),
                                                ConfigurationManager.AppSettings["LDAPLibraryLogPath"],
                                                ConfigurationManager.AppSettings["LdapUserObjectClass"],
                                                ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]
                                                );

            Assert.IsFalse(LdapManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testCompleteInitLibraryNoAdmin()
        {
            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                       ConfigurationManager.AppSettings["LDAPAuthType"]);

            LdapManagerObj = new LdapManager(null,
                                                ConfigurationManager.AppSettings["LDAPServer"],
                                                ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                authType,
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]),
                                                ConfigurationManager.AppSettings["clientCertificatePath"],
                                                Convert.ToBoolean(ConfigurationManager.AppSettings["enableLDAPLibraryLog"]),
                                                ConfigurationManager.AppSettings["LDAPLibraryLogPath"],
                                                ConfigurationManager.AppSettings["LdapUserObjectClass"],
                                                ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]
                                                );

            Assert.IsFalse(LdapManagerObj.Equals(null));

        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testStardardInitLibraryNoAdmin()
        {
            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                       ConfigurationManager.AppSettings["LDAPAuthType"]);

            LdapManagerObj = new LdapManager(null,
                                                    ConfigurationManager.AppSettings["LDAPServer"],
                                                    ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                    authType
                                                    );

            Assert.IsFalse(LdapManagerObj.Equals(null));

        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testStandardInitLibrary()
        {

            LdapUser adminUser = new LdapUser(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                 ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                 ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                 null);
            adminUser.CreateUserAttribute("userPassword", ConfigurationManager.AppSettings["LDAPAdminUserPassword"]);


            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        ConfigurationManager.AppSettings["LDAPAuthType"]);

            LdapManagerObj = new LdapManager(adminUser,
                                                ConfigurationManager.AppSettings["LDAPServer"],
                                                ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                authType
                                                );

            Assert.IsFalse(LdapManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testAdminConnect()
        {
            //Init the DLL
            testCompleteInitLibrary();

            //Connect with admin user
            Assert.IsTrue(LdapManagerObj.Connect());
        }

        #endregion

        #region LDAP Library Tests - Write Permission Required

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testCreateUser()
        {
            LdapUser testLdapUser = setupTestUser();
            //Init the DLL and connect the admin
            testAdminConnect();

            //Create user
            bool result = LdapManagerObj.CreateUser(testLdapUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(LdapManagerObj.GetLdapMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Create User Operation Success");

            result = LdapManagerObj.DeleteUser(testLdapUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testDeleteUser()
        {
            //Set the test user
            LdapUser testLdapUser = setupTestUser();

            //Init the DLL and connect the admin
            testAdminConnect();

            //Create LDAPuser to delete.
            bool result = LdapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            //Delete user
            result = LdapManagerObj.DeleteUser(testLdapUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(LdapManagerObj.GetLdapMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Delete User Operation Success");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testModifyUserAttribute()
        {
            bool result;
            testAdminConnect();
            LdapUser testLdapUser = setupTestUser();
            result = LdapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            List<LdapUser> returnUsers = new List<LdapUser>();
            const string userAttributeValue = "description Modified";

            result = LdapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Replace, testLdapUser, "description", userAttributeValue);

            Assert.IsTrue(result);

            result = LdapManagerObj.SearchUsers(
                new List<string> { "description" },
                LDAPMatchSearchField,
                out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers[0].GetUserCn(), testLdapUser.GetUserCn());
            Assert.AreEqual(returnUsers[0].GetUserAttribute("description")[0], userAttributeValue);

            result = LdapManagerObj.DeleteUser(testLdapUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testChangeUserPassword()
        {
            testAdminConnect();
            const string newPassword = "pippo";
            LdapUser testUser = setupTestUser();
            string oldPassword = testUser.GetUserAttribute("userPassword")[0];
            //Create the user
            bool result = LdapManagerObj.CreateUser(testUser);

            Assert.IsTrue(result);

            //Perform change of password
            result = LdapManagerObj.ChangeUserPassword(testUser, newPassword);
            Assert.IsTrue(result);

            //Try to connect with the old password
            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                oldPassword,
                "");

            result = LdapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsFalse(result);

            //Try to connect with the new password
            testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                newPassword,
                "");

            result = LdapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);

            testAdminConnect();

            result = LdapManagerObj.DeleteUser(testUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testUserConnect()
        {
            bool result;
            LdapUser testUser = setupTestUser();
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
            {
                testAdminConnect();
                result = LdapManagerObj.CreateUser(testUser);
                Assert.IsTrue(result);
            }
            else
                //Init the DLL
                testStandardInitLibrary();


            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                testUser.GetUserAttribute("userPassword")[0],
                "");

            result = LdapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
            {
                testAdminConnect();
                result = LdapManagerObj.DeleteUser(testUser);
                Assert.IsTrue(result);
            }
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testSearchUserAndConnect()
        {

            testAdminConnect();
            LdapUser testLdapUser = setupTestUser();

            bool result = LdapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            result = LdapManagerObj.SearchUserAndConnect(LDAPMatchSearchField[0], testLdapUser.GetUserAttribute("userPassword")[0]);

            Assert.IsTrue(result);

            testAdminConnect();

            result = LdapManagerObj.DeleteUser(testLdapUser);

            Assert.IsTrue(result);
        }

        #endregion

        #region LDAP Library Tests - Only Read Permission Required

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void testSearchUser()
        {
            testAdminConnect();

            string[] userIDToSearch = new string[1]
			{
				"Matteo"
			};
            List<string> userAttributeToReturnBySearch = new List<string>()
			{
				"description"
			};

            List<LdapUser> returnUsers = new List<LdapUser>();

            bool result = LdapManagerObj.SearchUsers(userAttributeToReturnBySearch, userIDToSearch, out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers.Count, userIDToSearch.Length);
            Assert.AreEqual(returnUsers[0].GetUserCn(), "Matteo");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void testUserConnectWithoutWritePermissions()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
                testAdminConnect();
            else
                //Init the DLL
                testStandardInitLibrary();

            LdapUser testUser = setupReadOnlyTestUser();

            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                testUser.GetUserAttribute("userPassword")[0],
                "");

            bool result = LdapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);

        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void testSearchUserAndConnectWithoutWritePermissions()
        {
            testAdminConnect();
            LdapUser testLdapUser = setupReadOnlyTestUser();

            bool result = LdapManagerObj.SearchUserAndConnect(LDAPMatchSearchField[0], testLdapUser.GetUserAttribute("userPassword")[0]);

            Assert.IsTrue(result);
        }

        #endregion

        private LdapUser setupTestUser()
        {
            string userDN = "cn=Fabio2,o=ApexNet,ou=People,dc=maxcrc,dc=com";
            string userCN = "Fabio";
            string userSN = "Vassura";

            LdapUser testLdapUser = new LdapUser(userDN, userCN, userSN, null);

            testLdapUser.CreateUserAttribute("userPassword", "1");
            testLdapUser.CreateUserAttribute("description", "test");

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]))
            {
                if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("cn"))
                    LDAPMatchSearchField = new string[1] { testLdapUser.GetUserCn() };
                else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("dn"))
                    LDAPMatchSearchField = new string[1] { testLdapUser.GetUserDn() };
                else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("sn"))
                    LDAPMatchSearchField = new string[1] { testLdapUser.GetUserSn() };
                else
                    LDAPMatchSearchField = new string[1] {
						testLdapUser.GetUserAttribute( ConfigurationManager.AppSettings["LDAPMatchFieldUsername"] )[0]
					};
            }

            //Set the test user
            return testLdapUser;
        }

        private LdapUser setupReadOnlyTestUser()
        {
            string userDN = "cn=Matteo,o=ApexNet,ou=People,dc=maxcrc,dc=com";
            string userCN = "Matteo";
            string userSN = "Paci";

            LdapUser testLdapUser = new LdapUser(userDN, userCN, userSN, null);

            testLdapUser.CreateUserAttribute("userPassword", "1");
            testLdapUser.CreateUserAttribute("description", "test");

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]))
            {
                if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("cn"))
                    LDAPMatchSearchField = new string[1] { testLdapUser.GetUserCn() };
                else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("dn"))
                    LDAPMatchSearchField = new string[1] { testLdapUser.GetUserDn() };
                else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("sn"))
                    LDAPMatchSearchField = new string[1] { testLdapUser.GetUserSn() };
                else
                    LDAPMatchSearchField = new string[1] {
						testLdapUser.GetUserAttribute( ConfigurationManager.AppSettings["LDAPMatchFieldUsername"] )[0]
					};
            }

            //Set the test user
            return testLdapUser;
        }
    }
}