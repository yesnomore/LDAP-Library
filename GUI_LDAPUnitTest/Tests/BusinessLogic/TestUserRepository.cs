using System;
using System.Collections.Generic;
using LDAPLibrary;
using LDAPLibrary.Interfarces;

namespace GUI_LDAPUnitTest.Tests.BusinessLogic
{
    public class TestUserRepository
    {
        private string[] _usersToSearch;

        public TestUserRepository()
        {
            string testUserDn = string.IsNullOrEmpty(Config.LdapLibrary["LDAPAdminUserDN"])
                ? "no User DN"
                : (
                    "cn=" + Constants.TestUserDefaultCn +
                    Config.LdapLibrary["LDAPAdminUserDN"].Substring(
                        Config.LdapLibrary["LDAPAdminUserDN"].IndexOf(",", StringComparison.Ordinal))
                    );

            var testUserOtherAttribute = new Dictionary<string, List<string>>
            {
                //aggiungere inizializzare così il dizionario
                {"userPassword", new List<string> {Constants.TestUserDefaultPassword}},
                {"description", new List<string> {Constants.TestUserDefaultDescription}}
            };

            SetupTestUser(testUserDn, Constants.TestUserDefaultCn, Constants.TestUserDefaultSn, testUserOtherAttribute);
            TestUserNewPassword = Constants.TestUserDefaultNewPassword;
            TestUserNewDescription = Constants.TestUserDefaultNewDescription;
            SetupUsersToSearch(new[] {Constants.TestSearchUserDefaultCn});
        }

        public string TestUserNewDescription { get; set; }
        public string TestUserNewPassword { get; set; }
        public ILdapUser TestUser { get; private set; }
        public int ExpectedSearchNoFilterResultNumber { get; set; }
        public int ExpectedSearchAllNodesResultNumber { get; set; }

        public void SetupTestUser(string testUserDn, string testUserCn, string testUserSn,
            Dictionary<string, List<string>> testUserOtherAttribute)
        {
            TestUser = new LdapUser(testUserDn, testUserCn, testUserSn, testUserOtherAttribute);
        }

        public string GetTestUserCn()
        {
            return TestUser.GetUserCn();
        }

        public string GetTestUserDn()
        {
            return TestUser.GetUserDn();
        }

        public string GetTestUserSn()
        {
            return TestUser.GetUserSn();
        }

        public List<string> GetTestUserOtherAttributes(string attributeKey)
        {
            return TestUser.GetUserAttribute(attributeKey);
        }

        public string[] GetTestUserOtherAttributesKeys()
        {
            return TestUser.GetUserAttributeKeys();
        }

        public void SetupUsersToSearch(string[] list)
        {
            _usersToSearch = list;
        }

        public string[] GetUserToSearch()
        {
            return _usersToSearch;
        }
    }
}