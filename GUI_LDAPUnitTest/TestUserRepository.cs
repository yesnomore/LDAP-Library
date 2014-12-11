using System;
using System.Collections.Generic;
using LDAPLibrary;
using LDAPLibrary.Interfarces;

namespace GUI_LDAPUnitTest
{
    public class TestUserRepository
    {
        public string TestUserNewDescription { get; set; }
        public string TestUserNewPassword { get; set; }
        public ILdapUser TestUser { get; private set; }

        const string TestUserDefaultCn = "defaultTestUserCN";
        const string TestUserDefaultSn = "defaultTestUserSN";

        public TestUserRepository()
        {
            string testUserDn = "no User DN";
                //Cut the DN of Admin User from his CN and add the default CN of testUser
                if (!string.IsNullOrEmpty(Config.LDAPLibrary["LDAPAdminUserDN"]))
                    testUserDn = (
                        "cn=" + TestUserDefaultCn +
                        Config.LDAPLibrary["LDAPAdminUserDN"].Substring(
                            Config.LDAPLibrary["LDAPAdminUserDN"].IndexOf(",", StringComparison.Ordinal))
                        );

                var testUserOtherAttribute = new Dictionary<string, List<string>>
                {
                    //aggiungere inizializzare così il dizionario
                    {"userPassword", new List<string> {"defaultTestUserPassword"}},
                    {"description", new List<string> {"test"}}
                };

                SetupTestUser(testUserDn, TestUserDefaultCn, TestUserDefaultSn, testUserOtherAttribute);
            TestUserNewPassword = "defaultNewTestUserPassword";
            TestUserNewDescription = "defaultNewTestUserDescription";
        }

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
    }
}
