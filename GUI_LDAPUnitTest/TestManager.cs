﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using LDAPLibrary;

namespace GUI_LDAPUnitTest
{
    public class TestManager
    {
        private readonly string[] _ldapMatchSearchField;
        private ILDAPManager _ldapManagerObj;
        private LDAPUser _testUser;
        private string _testUserNewDescription;
        private string _testUserNewPassword;
        private string[] _usersToSearch;

        public TestManager(ILDAPManager lm)
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
            _testUser = new LDAPUser(testUserDn, testUserCn, testUserSn, testUserOtherAttribute);
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
                var adminUser = new LDAPUser(Config.LDAPLibrary["LDAPAdminUserDN"],
                    Config.LDAPLibrary["LDAPAdminUserCN"],
                    Config.LDAPLibrary["LDAPAdminUserSN"],
                    null);
                adminUser.SetUserAttribute("userPassword", Config.LDAPLibrary["LDAPAdminUserPassword"]);

                var authType = (AuthType)Enum.Parse(typeof(AuthType),
                    Config.LDAPLibrary["LDAPAuthType"]);

                _ldapManagerObj = new LDAPManager(adminUser,
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

                _ldapManagerObj = new LDAPManager(null,
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

                _ldapManagerObj = new LDAPManager(null,
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

            List<LDAPUser> returnUsers;

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
                _testUser.SetUserAttribute("description", oldDescription);
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
            _testUser.SetUserAttributes("description", returnUsers[0].GetUserAttribute("description"));
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
                _testUser.SetUserAttribute("userPassword", oldPassword);
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
                _testUser.SetUserAttribute("userPassword", oldPassword);
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
                _testUser.SetUserAttribute("userPassword", oldPassword);
                return result;
            }
            _ldapManagerObj.DeleteUser(_testUser);
            _testUser.SetUserAttribute("userPassword", oldPassword);
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

            List<LDAPUser> returnUsers;

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

        public bool RunTest(Tests testType, bool writePermission)
        {
            switch (testType)
            {
                case Tests.TestInitLibrary:
                    return TestCompleteInitLibrary();
                case Tests.TestAdminConnection:
                    return TestAdminConnect();
                case Tests.TestCreateUser:
                    return TestCreateUser();
                case Tests.TestModifyUserDescription:
                    return TestModifyUserAttribute();
                case Tests.TestSearchUsers:
                    return TestSearchUser();
                case Tests.TestConnectUser:
                    if (writePermission) return TestUserConnect();
                    return TestUserConnectWithoutWritePermissions();
                case Tests.TestSearchUserAndConnect:
                    if (writePermission) return TestSearchUserAndConnect();
                    return TestSearchUserAndConnectWithoutWritePermissions();
                case Tests.TestUserChangePassword:
                    return TestChangeUserPassword();
                case Tests.TestDeleteUser:
                    return TestDeleteUser();
                case Tests.TestStandardInitLibraryNoAdmin:
                    return TestStardardInitLibraryNoAdmin();
                case Tests.TestInitLibraryNoAdmin:
                    return TestCompleteInitLibraryNoAdmin();
                default:
                    return false;
            }
        }
    }
}