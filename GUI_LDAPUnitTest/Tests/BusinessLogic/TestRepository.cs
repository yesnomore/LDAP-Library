using System;
using System.Collections.Generic;
using System.Configuration;

using LDAPLibrary.Interfarces;

namespace GUI_LDAPUnitTest.Tests.BusinessLogic
{
    class TestRepository
    {
        private readonly bool WritePermission = Convert.ToBoolean(ConfigurationManager.AppSettings["writePermissions"]);
        private delegate bool TestMethod();
        private  TestImplementation testImplementation;

        public TestRepository(ILdapManager ldapManagerObj,TestUserRepository testUserRepository)
        {
            testImplementation = new TestImplementation(testUserRepository, ldapManagerObj);

            _testList = new Dictionary<TestType, TestMethod>
            {
                {TestType.TestAdminConnection, () => TestImplementation.TestAdminConnect()},
                {TestType.TestCreateUser, TestCreateUser},
                {TestType.TestDeleteUser, TestDeleteUser},
                {TestType.TestInitLibrary, TestCompleteInitLibrary},
                {TestType.TestInitLibraryNoAdmin, TestStardardInitLibraryNoAdmin},
                {TestType.TestModifyUserDescription, TestModifyUserAttribute},
                {TestType.TestSearchUsers, TestSearchUser},
                {TestType.TestStandardInitLibraryNoAdmin, TestStardardInitLibraryNoAdmin},
                {TestType.TestUserChangePassword, TestChangeUserPassword},
                {
                    TestType.TestConnectUser, () =>
                    {
                        var testMethod = new TestMethod(TestUserConnectWithoutWritePermissions);
                        if (WritePermission) testMethod = TestUserConnect;
                        return testMethod();
                    }
                },
                {
                    TestType.TestSearchUserAndConnect, () =>
                    {
                        var testMethod = new TestMethod(TestSearchUserAndConnectWithoutWritePermissions);
                        if (WritePermission) testMethod = TestSearchUserAndConnect;
                        return testMethod();
                    }
                }
            };
        }

        private Dictionary<TestType, TestMethod> _testList;
    }
}
