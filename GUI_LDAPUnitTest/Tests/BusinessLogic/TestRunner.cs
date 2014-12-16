using LDAPLibrary.Interfarces;

namespace GUI_LDAPUnitTest.Tests.BusinessLogic
{
    public class TestRunner
    {
        private readonly TestRepository _testRepo;

        public TestRunner(ILdapManager lm)
        {
            UserRepository = new TestUserRepository();

            _testRepo = new TestRepository(lm, UserRepository);
        }

        public TestUserRepository UserRepository { get; private set; }

        public bool RunTest(TestType testTypeType)
        {
            return _testRepo.TestList[testTypeType]();
        }
    }
}