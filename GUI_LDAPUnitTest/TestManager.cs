using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Net;
using LDAPLibrary;
using LDAPLibrary.Interfarces;

namespace GUI_LDAPUnitTest
{
    public class TestManager
    {
        private readonly string[] _ldapMatchSearchField;
        private readonly Dictionary<Tests, TestMethod> _testList;
        private readonly bool _writePermission = Convert.ToBoolean(ConfigurationManager.AppSettings["writePermissions"]);
        private ILdapManager _ldapManagerObj;


        public TestManager(ILdapManager lm)
        {
            _ldapManagerObj = lm;

            UserRepository = new TestUserRepository();

            _ldapMatchSearchField = new[] {Config.LDAPLibrary["LDAPMatchFieldUsername"]};

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
                adminUser.CreateUserAttribute("userPassword", Config.LDAPLibrary["LDAPAdminUserPassword"]);

                var authType = (AuthType) Enum.Parse(typeof (AuthType),
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
                var authType = (AuthType) Enum.Parse(typeof (AuthType),
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
                var authType = (AuthType) Enum.Parse(typeof (AuthType),
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
            bool result = _ldapManagerObj.CreateUser(UserRepository.TestUser);

            //Assert the correct operations
            if (!result &&
                !_ldapManagerObj.GetLdapMessage()
                    .Contains("LDAP USER MANIPULATION SUCCESS: " + "Create User Operation Success")
                )
                return false;


            result = _ldapManagerObj.DeleteUser(UserRepository.TestUser);

            return result;
        }

        private bool TestDeleteUser()
        {
            //Init the DLL and connect the admin
            if (!TestAdminConnect())
                return false;

            //Create LDAPUser to delete.
            bool result = _ldapManagerObj.CreateUser(UserRepository.TestUser);

            if (!result)
                return false;

            //Delete user
            result = _ldapManagerObj.DeleteUser(UserRepository.TestUser);

            if (
                !result &&
                !_ldapManagerObj.GetLdapMessage()
                    .Contains("LDAP USER MANIPULATION SUCCESS: " + "Delete User Operation Success"))
                return false;
            return true;
        }

        private bool TestModifyUserAttribute()
        {
            string oldDescription;

            if (!TestAdminConnect())
                return false;

            if (!_ldapManagerObj.CreateUser(UserRepository.TestUser))
                return false;

            List<ILdapUser> returnUsers;

            try
            {
                oldDescription = UserRepository.TestUser.GetUserAttribute("description")[0];
            }
            catch (Exception)
            {
                oldDescription = "";
            }
            bool result = _ldapManagerObj.ModifyUserAttribute(DirectoryAttributeOperation.Replace,
                UserRepository.TestUser,
                "description", UserRepository.TestUserNewDescription);

            if (!result)
            {
                _ldapManagerObj.DeleteUser(UserRepository.TestUser);
                UserRepository.TestUser.OverwriteUserAttribute("description", oldDescription);
                return false;
            }
            switch (_ldapMatchSearchField[0])
            {
                case "cn":
                    result = _ldapManagerObj.SearchUsers(new List<string> {"description"},
                        new[] {UserRepository.TestUser.GetUserCn()},
                        out returnUsers);
                    break;
                case "sn":
                    result = _ldapManagerObj.SearchUsers(new List<string> {"description"},
                        new[] {UserRepository.TestUser.GetUserSn()},
                        out returnUsers);
                    break;
                case "dn":
                    result = _ldapManagerObj.SearchUsers(new List<string> {"description"},
                        new[] {UserRepository.TestUser.GetUserDn()},
                        out returnUsers);
                    break;
                default:
                    result = _ldapManagerObj.SearchUsers(new List<string> {"description"},
                        UserRepository.TestUser.GetUserAttribute(_ldapMatchSearchField[0]).ToArray(),
                        out returnUsers);
                    break;
            }

            if (result &&
                returnUsers[0].GetUserCn().Equals(UserRepository.TestUser.GetUserCn()) &&
                returnUsers[0].GetUserAttribute("description")[0].Equals(UserRepository.TestUserNewDescription))
            {
                result = _ldapManagerObj.DeleteUser(UserRepository.TestUser);
                if (result)
                    return true;
                return false;
            }
            _ldapManagerObj.DeleteUser(UserRepository.TestUser);
            UserRepository.TestUser.OverwriteUserAttribute("description", returnUsers[0].GetUserAttribute("description"));
            return false;
        }

        private bool TestChangeUserPassword()
        {
            if (!TestAdminConnect())
                return false;

            //Create the user
            bool result = _ldapManagerObj.CreateUser(UserRepository.TestUser);

            string oldPassword = UserRepository.TestUser.GetUserAttribute("userPassword")[0];

            if (!result)
            {
                return false;
            }

            //Perform change of password
            result = _ldapManagerObj.ChangeUserPassword(UserRepository.TestUser, UserRepository.TestUserNewPassword);

            if (!result)
            {
                _ldapManagerObj.DeleteUser(UserRepository.TestUser);
                UserRepository.TestUser.OverwriteUserAttribute("userPassword", oldPassword);
                return false;
            }

            //Try to connect with the old password
            var testUserCredential = new NetworkCredential(
                UserRepository.TestUser.GetUserDn(),
                oldPassword,
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]));

            if (result)
            {
                _ldapManagerObj.DeleteUser(UserRepository.TestUser);
                UserRepository.TestUser.OverwriteUserAttribute("userPassword", oldPassword);
                return false;
            }

            //Try to connect with the new password
            testUserCredential = new NetworkCredential(
                UserRepository.TestUser.GetUserDn(),
                UserRepository.TestUserNewPassword,
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]));

            TestAdminConnect();

            if (result)
            {
                result = _ldapManagerObj.DeleteUser(UserRepository.TestUser);
                UserRepository.TestUser.OverwriteUserAttribute("userPassword", oldPassword);
                return result;
            }
            _ldapManagerObj.DeleteUser(UserRepository.TestUser);
            UserRepository.TestUser.OverwriteUserAttribute("userPassword", oldPassword);
            return false;
        }

        private bool TestUserConnect()
        {
            bool result;

            if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
            {
                if (!TestAdminConnect())
                    return false;

                result = _ldapManagerObj.CreateUser(UserRepository.TestUser);

                if (!result)
                    return false;
            }

            var testUserCredential = new NetworkCredential(
                UserRepository.TestUser.GetUserDn(),
                UserRepository.TestUser.GetUserAttribute("userPassword")[0],
                "");

            result = _ldapManagerObj.Connect(testUserCredential,
                Convert.ToBoolean(Config.LDAPLibrary["secureSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["transportSocketLayerFlag"]),
                Convert.ToBoolean(Config.LDAPLibrary["ClientCertificationFlag"]));

            if (!result)
            {
                _ldapManagerObj.DeleteUser(UserRepository.TestUser);
                return false;
            }

            if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
            {
                if (!TestAdminConnect())
                    return false;
            }
            result = _ldapManagerObj.DeleteUser(UserRepository.TestUser);
            return result;
        }

        private bool TestSearchUserAndConnect()
        {
            if (!TestAdminConnect())
                return false;

            bool result = _ldapManagerObj.CreateUser(UserRepository.TestUser);

            if (!result)
                return false;

            switch (_ldapMatchSearchField[0])
            {
                case "cn":
                    result = _ldapManagerObj.SearchUserAndConnect(UserRepository.TestUser.GetUserCn(),
                        UserRepository.TestUser.GetUserAttribute("userPassword")[0]);
                    break;
                case "sn":
                    result = _ldapManagerObj.SearchUserAndConnect(UserRepository.TestUser.GetUserSn(),
                        UserRepository.TestUser.GetUserAttribute("userPassword")[0]);
                    break;
                case "dn":
                    result = _ldapManagerObj.SearchUserAndConnect(UserRepository.TestUser.GetUserDn(),
                        UserRepository.TestUser.GetUserAttribute("userPassword")[0]);
                    break;
                default:
                    result = _ldapManagerObj.SearchUserAndConnect(
                        UserRepository.TestUser.GetUserAttribute(_ldapMatchSearchField[0])[0],
                        UserRepository.TestUser.GetUserAttribute("userPassword")[0]);
                    break;
            }

            if (!result)
            {
                _ldapManagerObj.DeleteUser(UserRepository.TestUser);
                return false;
            }

            if (!TestAdminConnect())
            {
                _ldapManagerObj.DeleteUser(UserRepository.TestUser);
                return false;
            }

            result = _ldapManagerObj.DeleteUser(UserRepository.TestUser);

            return result;
        }

        #endregion

        #region LDAP Library Tests - Only Read Permission Required

        private bool TestSearchUser()
        {
            if (!TestAdminConnect())
                return false;

            List<ILdapUser> returnUsers;

            bool result = _ldapManagerObj.SearchUsers(null, UserRepository.GetUserToSearch(), out returnUsers);

            if (result &&
                returnUsers.Count.Equals(UserRepository.GetUserToSearch().Length))
                return true;
            return false;
        }

        private bool TestUserConnectWithoutWritePermissions()
        {
            if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
                if (!TestAdminConnect())
                    return false;

            var testUserCredential = new NetworkCredential(
                UserRepository.TestUser.GetUserDn(),
                UserRepository.TestUser.GetUserAttribute("userPassword")[0],
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
                        _ldapManagerObj.SearchUserAndConnect(UserRepository.TestUser.GetUserCn(),
                            UserRepository.TestUser.GetUserAttribute("userPassword")[0]);
                case "sn":
                    return
                        _ldapManagerObj.SearchUserAndConnect(UserRepository.TestUser.GetUserSn(),
                            UserRepository.TestUser.GetUserAttribute("userPassword")[0]);
                case "dn":
                    return
                        _ldapManagerObj.SearchUserAndConnect(UserRepository.TestUser.GetUserDn(),
                            UserRepository.TestUser.GetUserAttribute("userPassword")[0]);
                default:
                    return
                        _ldapManagerObj.SearchUserAndConnect(
                            UserRepository.TestUser.GetUserAttribute(_ldapMatchSearchField[0])[0],
                            UserRepository.TestUser.GetUserAttribute("userPassword")[0]);
            }
        }

        #endregion

        #endregion

        public TestUserRepository UserRepository { get; private set; }

        public bool RunTest(Tests testType)
        {
            return _testList[testType]();
        }

        private delegate bool TestMethod();
    }
}