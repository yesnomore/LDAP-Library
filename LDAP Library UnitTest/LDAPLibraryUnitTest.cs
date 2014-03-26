using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LDAPLibrary;
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
        ILDAPManager LDAPManagerObj;                 //LDAPLibrary
        string[] LDAPMatchSearchField;				 //Field to search

        #region LDAP Library Tests - Base

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testCompleteInitLibrary()
        {

            LDAPUser adminUser = new LDAPUser(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                null);
            adminUser.setUserAttribute("userPassword", ConfigurationManager.AppSettings["LDAPAdminUserPassword"]);

            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        ConfigurationManager.AppSettings["LDAPAuthType"]);

            LDAPManagerObj = new LDAPManager(adminUser,
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

            Assert.IsFalse(LDAPManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testCompleteInitLibraryNoAdmin()
        {
            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                       ConfigurationManager.AppSettings["LDAPAuthType"]);

            LDAPManagerObj = new LDAPManager(null,
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

            Assert.IsFalse(LDAPManagerObj.Equals(null));

        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testStardardInitLibraryNoAdmin()
        {
            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                       ConfigurationManager.AppSettings["LDAPAuthType"]);

            LDAPManagerObj = new LDAPManager(null,
                                                    ConfigurationManager.AppSettings["LDAPServer"],
                                                    ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                    authType
                                                    );

            Assert.IsFalse(LDAPManagerObj.Equals(null));

        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testStandardInitLibrary()
        {

            LDAPUser adminUser = new LDAPUser(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                 ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                 ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                 null);
            adminUser.setUserAttribute("userPassword", ConfigurationManager.AppSettings["LDAPAdminUserPassword"]);


            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        ConfigurationManager.AppSettings["LDAPAuthType"]);

            LDAPManagerObj = new LDAPManager(adminUser,
                                                ConfigurationManager.AppSettings["LDAPServer"],
                                                ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                authType
                                                );

            Assert.IsFalse(LDAPManagerObj.Equals(null));
        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void testAdminConnect()
        {
            //Init the DLL
            testCompleteInitLibrary();

            //Connect with admin user
            Assert.IsTrue(LDAPManagerObj.connect());
        }

        #endregion

        #region LDAP Library Tests - Write Permission Required

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testCreateUser()
        {
            LDAPUser testLDAPUser = setupTestUser();
            //Init the DLL and connect the admin
            testAdminConnect();

            //Create user
            bool result = LDAPManagerObj.createUser(testLDAPUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(LDAPManagerObj.getLDAPMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Create User Operation Success");

            result = LDAPManagerObj.deleteUser(testLDAPUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testDeleteUser()
        {
            //Set the test user
            LDAPUser testLDAPUser = setupTestUser();

            //Init the DLL and connect the admin
            testAdminConnect();

            //Create LDAPuser to delete.
            bool result = LDAPManagerObj.createUser(testLDAPUser);

            Assert.IsTrue(result);

            //Delete user
            result = LDAPManagerObj.deleteUser(testLDAPUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(LDAPManagerObj.getLDAPMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Delete User Operation Success");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testModifyUserAttribute()
        {
            bool result;
            testAdminConnect();
            LDAPUser testLDAPUser = setupTestUser();
            result = LDAPManagerObj.createUser(testLDAPUser);

            Assert.IsTrue(result);

            List<LDAPUser> returnUsers = new List<LDAPUser>();
            const string userAttributeValue = "description Modified";

            result = LDAPManagerObj.modifyUserAttribute(DirectoryAttributeOperation.Replace, testLDAPUser, "description", userAttributeValue);

            Assert.IsTrue(result);

            result = LDAPManagerObj.searchUsers(
                new List<string> { "description" },
                LDAPMatchSearchField,
                out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers[0].getUserCn(), testLDAPUser.getUserCn());
            Assert.AreEqual(returnUsers[0].getUserAttribute("description")[0], userAttributeValue);

            result = LDAPManagerObj.deleteUser(testLDAPUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testChangeUserPassword()
        {
            testAdminConnect();
            const string newPassword = "pippo";
            LDAPUser testUser = setupTestUser();
            string oldPassword = testUser.getUserAttribute("userPassword")[0];
            //Create the user
            bool result = LDAPManagerObj.createUser(testUser);

            Assert.IsTrue(result);

            //Perform change of password
            result = LDAPManagerObj.changeUserPassword(testUser, newPassword);
            Assert.IsTrue(result);

            //Try to connect with the old password
            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.getUserDn(),
                oldPassword,
                "");

            result = LDAPManagerObj.connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsFalse(result);

            //Try to connect with the new password
            testUserCredential = new NetworkCredential(
                testUser.getUserDn(),
                newPassword,
                "");

            result = LDAPManagerObj.connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);

            testAdminConnect();

            result = LDAPManagerObj.deleteUser(testUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testUserConnect()
        {
            bool result;
            LDAPUser testUser = setupTestUser();
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
            {
                testAdminConnect();
                result = LDAPManagerObj.createUser(testUser);
                Assert.IsTrue(result);
            }
            else
                //Init the DLL
                testStandardInitLibrary();


            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.getUserDn(),
                testUser.getUserAttribute("userPassword")[0],
                "");

            result = LDAPManagerObj.connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
            {
                testAdminConnect();
                result = LDAPManagerObj.deleteUser(testUser);
                Assert.IsTrue(result);
            }
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void testSearchUserAndConnect()
        {

            testAdminConnect();
            LDAPUser testLDAPUser = setupTestUser();

            bool result = LDAPManagerObj.createUser(testLDAPUser);

            Assert.IsTrue(result);

            result = LDAPManagerObj.searchUserAndConnect(LDAPMatchSearchField[0], testLDAPUser.getUserAttribute("userPassword")[0]);

            Assert.IsTrue(result);

            testAdminConnect();

            result = LDAPManagerObj.deleteUser(testLDAPUser);

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

            List<LDAPUser> returnUsers = new List<LDAPUser>();

            bool result = LDAPManagerObj.searchUsers(userAttributeToReturnBySearch, userIDToSearch, out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers.Count, userIDToSearch.Length);
            Assert.AreEqual(returnUsers[0].getUserCn(), "Matteo");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void testUserConnectWithoutWritePermissions()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
                testAdminConnect();
            else
                //Init the DLL
                testStandardInitLibrary();

            LDAPUser testUser = setupTestUser();

            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.getUserDn(),
                testUser.getUserAttribute("userPassword")[0],
                "");

            bool result = LDAPManagerObj.connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);

        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void testSearchUserAndConnectWithoutWritePermissions()
        {
            testAdminConnect();
            LDAPUser testLDAPUser = setupTestUser();

            bool result = LDAPManagerObj.searchUserAndConnect(LDAPMatchSearchField[0], testLDAPUser.getUserAttribute("userPassword")[0]);

            Assert.IsTrue(result);
        }

        #endregion

        private LDAPUser setupTestUser()
        {
            string userDN = "cn=Fabio2,o=ApexNet,ou=People,dc=maxcrc,dc=com";
            string userCN = "Fabio";
            string userSN = "Vassura";

            LDAPUser testLDAPUser = new LDAPUser(userDN, userCN, userSN, null);

            testLDAPUser.setUserAttribute("userPassword", "1");

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]))
            {
                if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("cn"))
                    LDAPMatchSearchField = new string[1] { testLDAPUser.getUserCn() };
                else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("dn"))
                    LDAPMatchSearchField = new string[1] { testLDAPUser.getUserDn() };
                else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("sn"))
                    LDAPMatchSearchField = new string[1] { testLDAPUser.getUserSn() };
                else
                    LDAPMatchSearchField = new string[1] {
						testLDAPUser.getUserAttribute( ConfigurationManager.AppSettings["LDAPMatchFieldUsername"] )[0]
					};
            }

            //Set the test user
            return testLDAPUser;
        }
    }
}