using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
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
    public class LocalhostEnvironment
    {
        //Class fields for the test
        private ILdapManager _ldapManagerObj; //LDAPLibrary

        #region Users

        //READ ONLY USER
        private const string ReadOnlyUserCn = "Matteo";
        private const string ReadOnlyUserPwd = "1";
        private const string ReadOnlyUserDn = "cn=" + ReadOnlyUserCn + ",o=ApexNet,ou=People,dc=maxcrc,dc=com";
        //WRITE USER THIS MUST NOT EXIST INITIALLY
        private const string WriteUserCn = "Fabio";
        private const string WriteUserPwd = "1";
        private const string WriteUserDn = "cn=" + WriteUserCn + ",o=ApexNet,ou=People,dc=maxcrc,dc=com";

        #endregion

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

        private static readonly LdapUser AdminUser = new LdapUser(LdapAdminUserDn,
            LdapAdminUserCn,
            LdapAdminUserSn,
            new Dictionary<string, List<string>> {{"userPassword", new List<string> {LdapAdminUserPassword}}});

        private static readonly string LdapLibraryLogPath = string.Format("{0}", AppDomain.CurrentDomain.BaseDirectory);

        #endregion

        #region LDAP Library Tests - Base

        [TestMethod, TestCategory("LDAPLibrary Test Init")]
        public void TestCompleteInitLibrary()
        {
            _ldapManagerObj = new LdapManager(AdminUser,AdminMode,
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
                LdapMatchFieldUsername
                );

            Assert.IsFalse(_ldapManagerObj.Equals(null));
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP LIBRARY INIT SUCCESS");
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
            var existingUser = new LdapUser(ReadOnlyUserDn, ReadOnlyUserCn, "test",
                new Dictionary<string, List<string>> {{"userPassword", new List<string> {ReadOnlyUserPwd}}});

            //Init the DLL and connect the admin
            TestAdminConnect();

            //Create existing user
            bool result = _ldapManagerObj.CreateUser(existingUser);

            //Assert the correct operations
            Assert.IsFalse(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP CREATE USER ERROR: ");

            //Create user
            result = _ldapManagerObj.CreateUser(tempUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
                "LDAP USER MANIPULATION SUCCESS: ");

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
            bool result = _ldapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            //Delete user
            result = _ldapManagerObj.DeleteUser(testLdapUser);

            //Assert the correct operations
            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
                "LDAP USER MANIPULATION SUCCESS: ");

            //Delete user again with error
            result = _ldapManagerObj.DeleteUser(testLdapUser);

            //Assert the correct operations
            Assert.IsFalse(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP DELETE USER ERROR: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestModifyUserAttribute()
        {
            TestAdminConnect();
            var testLdapUser = new LdapUser(WriteUserDn, WriteUserCn, "test",
                new Dictionary<string, List<string>> {{"description", new List<string> {"test"}}});
            bool result = _ldapManagerObj.CreateUser(testLdapUser);

            Assert.IsTrue(result);

            List<ILdapUser> returnUsers;
            const string userAttributeValue = "description Modified";

            result = _ldapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Delete, testLdapUser, "ciccio",
                userAttributeValue);

            Assert.IsFalse(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
                "LDAP MODIFY USER ATTRIBUTE ERROR: ");

            result = _ldapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Replace, testLdapUser,
                "description", userAttributeValue);

            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
                "LDAP USER MANIPULATION SUCCESS: ");

            result = _ldapManagerObj.SearchUsers(
                new List<string> {"description"},
                new[] {WriteUserCn},
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
            var testUser = new LdapUser(WriteUserDn, WriteUserCn, "test",
                new Dictionary<string, List<string>> {{"userPassword", new List<string> {WriteUserPwd}}});
            //Create the user
            bool result = _ldapManagerObj.CreateUser(testUser);

            Assert.IsTrue(result);

            //Perform change of password
            result = _ldapManagerObj.ChangeUserPassword(testUser, newPassword);
            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
                "LDAP USER MANIPULATION SUCCESS: ");

            //Try to connect with the old password
            var testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                WriteUserPwd,
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag);

            Assert.IsFalse(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP CONNECTION ERROR: ");

            //Try to connect with the new password
            testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                newPassword,
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag);

            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP CONNECTION SUCCESS");

            TestAdminConnect();

            result = _ldapManagerObj.DeleteUser(testUser);

            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1),
                "LDAP USER MANIPULATION SUCCESS: ");
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestUserConnect()
        {
            var testUser = new LdapUser(WriteUserDn, WriteUserCn, "test",
                new Dictionary<string, List<string>> {{"userPassword", new List<string> {WriteUserPwd}}});
            var faketestUser = new LdapUser(WriteUserDn, WriteUserCn, "test",
                new Dictionary<string, List<string>> {{"userPassword", new List<string> {"FakePassword"}}});

            TestAdminConnect();


            bool result = _ldapManagerObj.CreateUser(testUser);
            Assert.IsTrue(result);

            var testUserCredential = new NetworkCredential(
                testUser.GetUserDn(),
                testUser.GetUserAttribute("userPassword")[0],
                "");
            var faketestUserCredential = new NetworkCredential(
                faketestUser.GetUserDn(),
                faketestUser.GetUserAttribute("userPassword")[0],
                "");

            result = _ldapManagerObj.Connect(faketestUserCredential,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag);

            Assert.IsFalse(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP CONNECTION ERROR: ");


            result = _ldapManagerObj.Connect(testUserCredential,
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag);

            Assert.IsTrue(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP CONNECTION SUCCESS");

            TestAdminConnect();
            result = _ldapManagerObj.DeleteUser(testUser);
            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Write Permissions")]
        public void TestSearchUserAndConnect()
        {
            TestAdminConnect();
            var testLdapUser = new LdapUser(WriteUserDn, WriteUserCn, "test",
                new Dictionary<string, List<string>> {{"userPassword", new List<string> {WriteUserPwd}}});

            bool result = _ldapManagerObj.CreateUser(testLdapUser);

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
            string[] fakeuserIdToSearch =
            {
                WriteUserCn
            };
            var userAttributeToReturnBySearch = new List<string>
            {
                "description"
            };

            List<ILdapUser> returnUsers;

            bool result = _ldapManagerObj.SearchUsers(userAttributeToReturnBySearch, fakeuserIdToSearch, out returnUsers);

            Assert.IsFalse(result);
            Assert.AreEqual(_ldapManagerObj.GetLdapMessage().Split('-')[1].Substring(1), "LDAP SEARCH USER ERROR: ");

            result = _ldapManagerObj.SearchUsers(null, userIdToSearch, out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers.Count, userIdToSearch.Length);
            Assert.AreEqual(returnUsers[0].GetUserCn(), ReadOnlyUserCn);
            Assert.IsTrue(returnUsers[0].GetUserAttributes().Count == 0);

            result = _ldapManagerObj.SearchUsers(userAttributeToReturnBySearch, userIdToSearch, out returnUsers);

            Assert.IsTrue(result);
            Assert.AreEqual(returnUsers.Count, userIdToSearch.Length);
            Assert.AreEqual(returnUsers[0].GetUserCn(), ReadOnlyUserCn);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void TestUserConnectWithoutWritePermissions()
        {
            if (!string.IsNullOrEmpty(LdapAdminUserDn))
                TestAdminConnect();
            else
                //Init the DLL
                TestStandardInitLibrary();


            bool result = _ldapManagerObj.Connect(new NetworkCredential(
                ReadOnlyUserDn, ReadOnlyUserPwd,
                ""),
                SecureSocketLayerFlag,
                TransportSocketLayerFlag,
                ClientCertificationFlag);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("LDAPLibrary Test Read Permissions")]
        public void TestSearchUserAndConnectWithoutWritePermissions()
        {
            TestAdminConnect();

            bool result = _ldapManagerObj.SearchUserAndConnect(ReadOnlyUserCn, ReadOnlyUserPwd);

            Assert.IsTrue(result);
        }

        #endregion
    }
}