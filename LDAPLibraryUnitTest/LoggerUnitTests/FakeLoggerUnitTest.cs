using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest.LoggerUnitTests
{
    [TestClass]
    public class FakeLoggerUnitTest
    {
        private readonly ILogger _logger = new FakeLogger();

        [TestMethod, TestCategory("FakeLogger")]
        public void TestFakeLoggerWrite()
        {
            _logger.Write(null);
        }
    }
}
