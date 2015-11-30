using System;
using System.Collections.Generic;
using System.Configuration;
using LDAPLibrary.Interfarces;

namespace GUI_LDAPUnitTest.Tests.BusinessLogic
{
    internal class TestRepository
    {
        private readonly TestImplementation.TestImplementation _testImplementation;
        private readonly bool _writePermission = Convert.ToBoolean(ConfigurationManager.AppSettings["writePermissions"]);

        public TestRepository(ILdapManager ldapManagerObj, TestUserRepository testUserRepository)
        {
            _testImplementation = new TestImplementation.TestImplementation(testUserRepository, ldapManagerObj);

            TestList = new Dictionary<TestType, TestMethod>
            {
                {TestType.TestAdminConnection, _testImplementation.TestAdminConnect},
                {TestType.TestCreateUser, _testImplementation.TestCreateUser},
                {TestType.TestDeleteUser, _testImplementation.TestDeleteUser},
                {TestType.TestInitLibrary, _testImplementation.TestCompleteInitLibrary},
                {TestType.TestInitLibraryNoAdmin, _testImplementation.TestStardardInitLibraryNoAdmin},
                {TestType.TestModifyUserDescription, _testImplementation.TestModifyUserAttribute},
                {TestType.TestSearchUsers, _testImplementation.TestSearchUser},
                {TestType.TestStandardInitLibraryNoAdmin, _testImplementation.TestStardardInitLibraryNoAdmin},
                {TestType.TestUserChangePassword, _testImplementation.TestChangeUserPassword},
                {
                    TestType.TestConnectUser, () =>
                    {
                        var testMethod = new TestMethod(_testImplementation.TestUserConnectWithoutWritePermissions);
                        if (_writePermission) testMethod = _testImplementation.TestUserConnect;
                        return testMethod();
                    }
                },
                {
                    TestType.TestSearchUserAndConnect, () =>
                    {
                        var testMethod =
                            new TestMethod(_testImplementation.TestSearchUserAndConnectWithoutWritePermissions);
                        if (_writePermission) testMethod = _testImplementation.TestSearchUserAndConnect;
                        return testMethod();
                    }
                }
            };
        }

        public Dictionary<TestType, TestMethod> TestList { get; set; }

        internal delegate bool TestMethod();
    }
}