using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Net;
using LDAPLibrary;

namespace GUI_LDAPUnitTest
{
    public class TestManager
    {
        private readonly string[] _ldapMatchSearchField;
        private ILdapManager _ldapManagerObj;
        private LdapUser _testUser;
        private string _testUserNewDescription;
        private string _testUserNewPassword;
        private string[] _usersToSearch;
        private readonly bool _writePermission = Convert.ToBoolean(ConfigurationManager.AppSettings["writePermissions"]);

        private readonly Dictionary<Tests, TestMethod> _testList;

        public TestManager(ILdapManager lm)
        {
            _ldapManagerObj = lm;

            if (_testUser == null)
            {
                const string testUserCn = "defaultTestUserCN";
                const string testUserSn = "defaultTestUserSN";
                string testUserDn = "no User DN";
                //Cut the DN of Admin User from his CN and add the default CN of testUser
                if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
                    testUserDn = (
                        "cn=" + testUserCn +
                        Config.LDAPLibrary["LDAPAdminUserDN"].Substring(
                            Config.LDAPLibrary["LDAPAdminUserDN"].IndexOf(",", StringComparison.Ordinal))
                        );

                var testUserOtherAttribute = new Dictionary<string, List<string>>
                {
                    //aggiungere inizializzare così il dizionario
                    {"userPassword", new List<string> {"defaultTestUserPassword"}}
                };

                SetupTestUser(testUserDn, testUserCn, testUserSn, testUserOtherAttribute);
            }

            if (string.IsNullOrEmpty(_testUserNewPassword))
                SetupTestUserNewPassword("defaultNewTestUserPassword");

            if (string.IsNullOrEmpty(_testUserNewDescription))
                SetupTestUserNewDescription("defaultNewTestUserDescription");

            if (_usersToSearch == null)
                SetupUsersToSearch(new[] { "defaultNewTestUserCN" });

            _ldapMatchSearchField = new[] { Config.LDAPLibrary["LDAPMatchFieldUsername"] };

            _testList = new Dictionary<Tests, TestMethod>
            {
                {Tests.TestAdminConnection, TestAdminConnect},
                {Tests.TestCreateUser, TestCreateUser},
                {Tests.TestDeleteUser, TestDeleteUser},
                {Tests.TestInitLibrary, TestCompleteInitLibrary},
                {Tests.TestInitLibraryNoAdmin, TestStardardInitLibraryNoAdmin},
                {Tests.TestModifyUserDescription, TestModifyUserAttribute},
                {Tests.TestSearchUsers, TestSearchUser},
                {Tests.TestStandardInitLibraryNoAdmin, TestStardardInitLibraryNoAdmin},
                {Tests.TestUserChangePassword, TestChangeUserPassword},
                {
                    Tests.TestConnectUser, () =>
                    {
                        var testMethod = new TestMethod(TestUserConnectWithoutWritePermissions);
                        if (_writePermission) testMethod = TestUserConnect;
                        return testMethod();
                    }
                },
                {
                    Tests.TestSearchUserAndConnect, () =>
                    {
                        var testMethod = new TestMethod(TestSearchUserAndConnectWithoutWritePermissions);
                        if (_writePermission) testMethod = TestSearchUserAndConnect;
                        return testMethod();
                    }
                }
            };
        }

        #region Method called for configuration

        public void SetupUsersToSearch(string[] list)
        {
            _usersToSearch = list;
        }

        public void SetupTestUserNewPassword(string p)
        {
            _testUserNewPassword = p;
        }

        public void SetupTestUser(string testUserDn, string testUserCn, string testUserSn,
            Dictionary<string, List<string>> testUserOtherAttribute)
        {
            _testUser = new LdapUser(testUserDn, testUserCn, testUserSn, testUserOtherAttribute);
        }

        public void SetupTestUserNewDescription(string p)
        {
            _testUserNewDescription = p;
        }

        #endregion

        #region Unit Tests

        #region LDAP Library Tests - Base

        private bool TestCompleteInitLibrary()
        {
            try
            {
                var adminUser = new LdapUser(Config.LDAPLibrary["LDAPAdminUserDN"],
                    Config.LDAPLibrary["LDAPAdminUserCN"],
                    Config.LDAPLibrary["LDAPAdminUserSN"],
                    null);
                adminUser.OverwriteUserAttribute("userPassword", Config.LDAPLibrary["LDAPAdminUserPassword"]);

                var authType = (AuthType)Enum.Parse(typeof(AuthType),
                    Config.LDAPLibrary["LDAPAuthType"]);

                _ldapManagerObj = new LdapManager(adminUser,
                    Config.LDAPLibrary["LDAPServer"],
                    Config.LDAPLibrary["LDAPSearchBaseDN"],
                    authType,
                    Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                    Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                    Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]),
                    Config.LDAPLibrary["clientCertificatePath"],
                    Convert.ToBoolean(Config.LDAPLibrary["enableLDAPLibraryLog"]),
                    Config.LDAPLibrary["LDAPLibraryLogPath"],
                    Config.LDAPLibrary["LDAPUserObjectClass"],
                    Config.LDAPLibrary["LDAPMatchFieldUsername"]
                    );

                if (!_ldapManagerObj.Equals(null))
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TestAdminConnect()
        {
            //Init the DLL
            if (!TestCompleteInitLibrary())
                return false;

            //Connect with admin user
            if (!_ldapManagerObj.Connect())
                return false;

            //Assert the behavior of DLL
            if (!_ldapManagerObj.Equals(null))
                return true;
            return false;
        }

        public bool TestCompleteInitLibraryNoAdmin()
        {
            try
            {
                var authType = (AuthType)Enum.Parse(typeof(AuthType),
                    Config.LDAPLibrary["LDAPAuthType"]);

                _ldapManagerObj = new LdapManager(null,
                    Config.LDAPLibrary["LDAPServer"],
                    Config.LDAPLibrary["LDAPSearchBaseDN"],
                    authType,
                    Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                    Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                    Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]),
                    Config.LDAPLibrary["clientCertificatePath"],
                    Convert.ToBoolean(Config.LDAPLibrary["enableLDAPLibraryLog"]),
                    Config.LDAPLibrary["LDAPLibraryLogPath"],
                    Config.LDAPLibrary["LDAPUserObjectClass"],
                    Config.LDAPLibrary["LDAPMatchFieldUsername"]
                    );

                if (!_ldapManagerObj.Equals(null))
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool TestStardardInitLibraryNoAdmin()
        {
            try
            {
                var authType = (AuthType)Enum.Parse(typeof(AuthType),
                    Config.LDAPLibrary["LDAPAuthType"]);

                _ldapManagerObj = new LdapManager(null,
                    Config.LDAPLibrary["LDAPServer"],
                    Config.LDAPLibrary["LDAPSearchBaseDN"],
                    authType
                    );

                if (!_ldapManagerObj.Equals(null))
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region LDAP Library Tests - Write Permission Required

        private bool TestCreateUser()
        {
            if (!TestAdminConnect())
                return false;

            //Create user
            bool result = _ldapManagerObj.CreateUser(_testUser);

            //Assert the correct operations
            if (!result &&
                !_ldapManagerObj.GetLdapMessage()
                    .Equals("LDAP USER MANIPULATION SUCCESS: " + "Create User Operation Success")
                )
                return false;


            result = _ldapManagerObj.DeleteUser(_testUser);

            return result;
        }

        private bool TestDeleteUser()
        {
            //Init the DLL and connect the admin
            if (!TestAdminConnect())
                return false;

            //Create LDAPUser to delete.
            bool result = _ldapManagerObj.CreateUser(_testUser);

            if (!result)
                return false;

            //Delete user
            result = _ldapManagerObj.DeleteUser(_testUser);

            if (
                !result &&
                !_ldapManagerObj.GetLdapMessage()
                    .Equals("LDAP USER MANIPULATION SUCCESS: " + "Delete User Operation Success"))
                return false;
            return true;
        }

        private bool TestModifyUserAttribute()
        {
            string oldDescription;

            if (!TestAdminConnect())
                return false;

            if (!_ldapManagerObj.CreateUser(_testUser))
                return false;

            List<ILdapUser> returnUsers;

            try
            {
                oldDescription = _testUser.GetUserAttribute("description")[0];
            }
            catch (Exception)
            {
                oldDescription = "";
            }
            bool result = _ldapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Replace, _testUser,
                "description", _testUserNewDescription);

            if (!result)
            {
                _ldapManagerObj.DeleteUser(_testUser);
                _testUser.OverwriteUserAttribute("description", oldDescription);
                return false;
            }
            switch (_ldapMatchSearchField[0])
            {
                case "cn":
                    result = _ldapManagerObj.SearchUsers(new List<string> { "description" },
                        new[] { _testUser.GetUserCn() },
                        out returnUsers);
                    break;
                case "sn":
                    result = _ldapManagerObj.SearchUsers(new List<string> { "description" },
                        new[] { _testUser.GetUserSn() },
                        out returnUsers);
                    break;
                case "dn":
                    result = _ldapManagerObj.SearchUsers(new List<string> { "description" },
                        new[] { _testUser.GetUserDn() },
                        out returnUsers);
                    break;
                default:
                    result = _ldapManagerObj.SearchUsers(new List<string> { "description" },
                        _testUser.GetUserAttribute(_ldapMatchSearchField[0]).ToArray(),
                        out returnUsers);
                    break;
            }

            if (result &&
                returnUsers[0].GetUserCn().Equals(_testUser.GetUserCn()) &&
                returnUsers[0].GetUserAttribute("description")[0].Equals(_testUserNewDescription))
            {
                result = _ldapManagerObj.DeleteUser(_testUser);
                if (result)
                    return true;
                return false;
            }
            _ldapManagerObj.DeleteUser(_testUser);
            _testUser.OverwriteUserAttribute("description", returnUsers[0].GetUserAttribute("description"));
            return false;
        }

        private bool TestChangeUserPassword()
        {
            if (!TestAdminConnect())
                return false;

            //Create the user
            bool result = _ldapManagerObj.CreateUser(_testUser);

            string oldPassword = _testUser.GetUserAttribute("userPassword")[0];

            if (!result)
            {
                return false;
            }

            //Perform change of password
            result = _ldapManagerObj.ChangeUserPassword(_testUser, _testUserNewPassword);

            if (!result)
            {
                _ldapManagerObj.DeleteUser(_testUser);
                _testUser.OverwriteUserAttribute("userPassword", oldPassword);
                return false;
            }

            //Try to connect with the old password
            var testUserCredential = new NetworkCredential(
                _testUser.GetUserDn(),
                oldPassword,
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]));

            if (result)
            {
                _ldapManagerObj.DeleteUser(_testUser);
                _testUser.OverwriteUserAttribute("userPassword", oldPassword);
                return false;
            }

            //Try to connect with the new password
            testUserCredential = new NetworkCredential(
                _testUser.GetUserDn(),
                _testUserNewPassword,
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]));

            TestAdminConnect();

            if (result)
            {
                result = _ldapManagerObj.DeleteUser(_testUser);
                _testUser.OverwriteUserAttribute("userPassword", oldPassword);
                return result;
            }
            _ldapManagerObj.DeleteUser(_testUser);
            _testUser.OverwriteUserAttribute("userPassword", oldPassword);
            return false;
        }

        private bool TestUserConnect()
        {
            bool result;

            if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
            {
                if (!TestAdminConnect())
                    return false;

                result = _ldapManagerObj.CreateUser(_testUser);

                if (!result)
                    return false;
            }

            var testUserCredential = new NetworkCredential(
                _testUser.GetUserDn(),
                _testUser.GetUserAttribute("userPassword")[0],
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]));

            if (!result)
            {
                _ldapManagerObj.DeleteUser(_testUser);
                return false;
            }

            if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
            {
                if (!TestAdminConnect())
                    return false;
            }
            result = _ldapManagerObj.DeleteUser(_testUser);
            return result;
        }

        private bool TestSearchUserAndConnect()
        {
            if (!TestAdminConnect())
                return false;

            bool result = _ldapManagerObj.CreateUser(_testUser);

            if (!result)
                return false;

            switch (_ldapMatchSearchField[0])
            {
                case "cn":
                    result = _ldapManagerObj.SearchUserAndConnect(_testUser.GetUserCn(),
                        _testUser.GetUserAttribute("userPassword")[0]);
                    break;
                case "sn":
                    result = _ldapManagerObj.SearchUserAndConnect(_testUser.GetUserSn(),
                        _testUser.GetUserAttribute("userPassword")[0]);
                    break;
                case "dn":
                    result = _ldapManagerObj.SearchUserAndConnect(_testUser.GetUserDn(),
                        _testUser.GetUserAttribute("userPassword")[0]);
                    break;
                default:
                    result = _ldapManagerObj.SearchUserAndConnect(
                        _testUser.GetUserAttribute(_ldapMatchSearchField[0])[0],
                        _testUser.GetUserAttribute("userPassword")[0]);
                    break;
            }

            if (!result)
            {
                _ldapManagerObj.DeleteUser(_testUser);
                return false;
            }

            if (!TestAdminConnect())
            {
                _ldapManagerObj.DeleteUser(_testUser);
                return false;
            }

            result = _ldapManagerObj.DeleteUser(_testUser);

            return result;
        }

        #endregion

        #region LDAP Library Tests - Only Read Permission Required

        private bool TestSearchUser()
        {
            if (!TestAdminConnect())
                return false;

            List<ILdapUser> returnUsers;

            bool result = _ldapManagerObj.SearchUsers(null, _usersToSearch, out returnUsers);

            if (result &&
                returnUsers.Count.Equals(_usersToSearch.Length))
                return true;
            return false;
        }

        private bool TestUserConnectWithoutWritePermissions()
        {
            if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
                if (!TestAdminConnect())
                    return false;

            var testUserCredential = new NetworkCredential(
                _testUser.GetUserDn(),
                _testUser.GetUserAttribute("userPassword")[0],
                "");

            bool result = _ldapManagerObj.Connect(testUserCredential,
                Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]));

            return result;
        }

        private bool TestSearchUserAndConnectWithoutWritePermissions()
        {
            if (!TestAdminConnect() || _ldapManagerObj == null)
                return false;

            switch (_ldapMatchSearchField[0])
            {
                case "cn":
                    return
                        _ldapManagerObj.SearchUserAndConnect(_testUser.GetUserCn(),
                            _testUser.GetUserAttribute("userPassword")[0]);
                case "sn":
                    return
                        _ldapManagerObj.SearchUserAndConnect(_testUser.GetUserSn(),
                            _testUser.GetUserAttribute("userPassword")[0]);
                case "dn":
                    return
                        _ldapManagerObj.SearchUserAndConnect(_testUser.GetUserDn(),
                            _testUser.GetUserAttribute("userPassword")[0]);
                default:
                    return
                        _ldapManagerObj.SearchUserAndConnect(_testUser.GetUserAttribute(_ldapMatchSearchField[0])[0],
                            _testUser.GetUserAttribute("userPassword")[0]);
            }
        }

        #endregion

        #endregion

        public string GetTestUserCn()
        {
            return _testUser.GetUserCn();
        }

        public string GetTestUserDn()
        {
            return _testUser.GetUserDn();
        }

        public string GetTestUserSn()
        {
            return _testUser.GetUserSn();
        }

        public List<string> GetTestUserOtherAttributes(string attributeKey)
        {
            return _testUser.GetUserAttribute(attributeKey);
        }

        public string[] GetTestUserOtherAttributesKeys()
        {
            return _testUser.GetUserAttributeKeys();
        }

        public string[] GetUserToSearch()
        {
            return _usersToSearch;
        }

        public string GetTestUserNewPassword()
        {
            return _testUserNewPassword;
        }

        public string GetTestUserNewDescription()
        {
            return _testUserNewDescription;
        }

        public bool RunTest(Tests testType)
        {
            return _testList[testType]();
        }

        private delegate bool TestMethod();
    }
}