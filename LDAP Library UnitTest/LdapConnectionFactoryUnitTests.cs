using LDAPLibrary;
using LDAPLibrary.Factories;
using LDAPLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapConnectionFactoryUnitTests
    {
        [TestMethod, TestCategory("LdapConnectionFactory")]
        public void GetLdapConnector()
        {
            var configRepo = new LdapConfigRepository();
            var connector = LdapConnectorFactory.GetLdapConnector(new LdapModeChecker(configRepo), configRepo,
                new FakeLogger());

            Assert.IsInstanceOfType(connector,typeof(LdapConnector));
        }
    }
}
