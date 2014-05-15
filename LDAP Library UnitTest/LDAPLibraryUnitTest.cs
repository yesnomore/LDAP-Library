﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LDAPLibrary;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Net;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapLibraryUnitTest
    {
        //Class fields for the test
        ILDAPManager _ldapManagerObj;                 //LDAPLibrary
        string[] _ldapMatchSearchField;				 //Field to search

        #region LDAP Library Tests - Base

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibrary()
        {

            LDAPUser adminUser = new LDAPUser(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                null);
            adminUser.SetUserAttribute("userPassword", ConfigurationManager.AppSettings["LDAPAdminUserPassword"]);

            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        ConfigurationManager.AppSettings["LDAPAuthType"]);

            _ldapManagerObj = new LDAPManager(adminUser,
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
            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                       ConfigurationManager.AppSettings["LDAPAuthType"]);

            _ldapManagerObj = new LDAPManager(null,
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
            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                       ConfigurationManager.AppSettings["LDAPAuthType"]);

            _ldapManagerObj = new LDAPManager(null,
                                                    ConfigurationManager.AppSettings["LDAPServer"],
                                                    ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
                                                    authType
                                                    );

            Assert.IsFalse(_ldapManagerObj.Equals(null));

        }

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestStandardInitLibrary()
        {

            LDAPUser adminUser = new LDAPUser(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                 ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                 ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                 null);
            adminUser.SetUserAttribute("userPassword", ConfigurationManager.AppSettings["LDAPAdminUserPassword"]);


            AuthType authType = (AuthType)Enum.Parse(typeof(AuthType),
                                                        ConfigurationManager.AppSettings["LDAPAuthType"]);

            _ldapManagerObj = new LDAPManager(adminUser,
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
            LDAPUser testLdapUser = SetupTestUser();
            //Init the DLL and connect the admin
            TestAdminConnect();

            //Create user
            bool result = _ldapManagerObj.CreateUser(testLdapUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Create User Operation Success");

            result = _ldapManagerObj.DeleteUser(testLdapUser);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestDeleteUser()
        {
            //Set the test user
            LDAPUser testLdapUser = SetupTestUser();

            //Init the DLL and connect the admin
            TestAdminConnect();

            //Create LDAPUser to delete.
            bool result = _ldapManagerObj.CreateUser(testLdapUser);

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
            bool result;
            TestAdminConnect();
            LDAPUser testLdapUser = SetupTestUser();
            result = _ldapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            List<LDAPUser> returnUsers;
            const string userAttributeValue = "description Modified";

            result = _ldapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Replace, testLdapUser, "description", userAttributeValue);

            Assert.IsTrue(result);

            result = _ldapManagerObj.SearchUsers(
                new List<string> { "description" },
                _ldapMatchSearchField,
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
            LDAPUser testUser = SetupTestUser();
            string oldPassword = testUser.GetUserAttribute("userPassword")[0];
            //Create the user
            bool result = _ldapManagerObj.CreateUser(testUser);

            Assert.IsTrue(result);

            //Perform change of password
            result = _ldapManagerObj.ChangeUserPassword(testUser, newPassword);
            Assert.IsTrue(result);

            //Try to connect with the old password
            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                oldPassword,
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
            bool result;
            LDAPUser testUser = SetupTestUser();
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
            {
                TestAdminConnect();
                result = _ldapManagerObj.CreateUser(testUser);
                Assert.IsTrue(result);
            }
            else
                //Init the DLL
                TestStandardInitLibrary();


            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                testUser.GetUserAttribute("userPassword")[0],
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
            {
                TestAdminConnect();
                result = _ldapManagerObj.DeleteUser(testUser);
                Assert.IsTrue(result);
            }
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestSearchUserAndConnect()
        {

            TestAdminConnect();
            LDAPUser testLdapUser = SetupTestUser();

            bool result = _ldapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            result = _ldapManagerObj.SearchUserAndConnect(_ldapMatchSearchField[0], testLdapUser.GetUserAttribute("userPassword")[0]);

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
                "Matteo"
            };
            List<string> userAttributeToReturnBySearch = new List<string>
            {
				"description"
			};

            List<LDAPUser> returnUsers;

            bool result = _ldapManagerObj.SearchUsers(userAttributeToReturnBySearch, userIdToSearch, out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers.Count, userIdToSearch.Length);
            Assert.AreEqual(returnUsers[0].GetUserCn(), "Matteo");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void TestUserConnectWithoutWritePermissions()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPAdminUserDN"]))
                TestAdminConnect();
            else
                //Init the DLL
                TestStandardInitLibrary();

            LDAPUser testUser = SetupTestUser();

            NetworkCredential testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                testUser.GetUserAttribute("userPassword")[0],
                "");

            bool result = _ldapManagerObj.Connect(testUserCredential,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
                        Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

            Assert.IsTrue(result);

        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void TestSearchUserAndConnectWithoutWritePermissions()
        {
            TestAdminConnect();
            LDAPUser testLdapUser = SetupTestUser();

            bool result = _ldapManagerObj.SearchUserAndConnect(_ldapMatchSearchField[0], testLdapUser.GetUserAttribute("userPassword")[0]);

            Assert.IsTrue(result);
        }

        #endregion

        private LDAPUser SetupTestUser()
        {
            string userDN = "cn=Fabio2,o=ApexNet,ou=People,dc=maxcrc,dc=com";
            string userCN = "Fabio";
            string userSN = "Vassura";

            LDAPUser testLdapUser = new LDAPUser(userDN, userCN, userSN, null);

            testLdapUser.SetUserAttribute("userPassword", "1");

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]))
            {
                if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("cn"))
                    _ldapMatchSearchField = new[] { testLdapUser.GetUserCn() };
                else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("dn"))
                    _ldapMatchSearchField = new[] { testLdapUser.GetUserDn() };
                else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("sn"))
                    _ldapMatchSearchField = new[] { testLdapUser.GetUserSn() };
                else
                    _ldapMatchSearchField = new[] {
						testLdapUser.GetUserAttribute( ConfigurationManager.AppSettings["LDAPMatchFieldUsername"] )[0]
					};
            }

            //Set the test user
            return testLdapUser;
        }
    }
}