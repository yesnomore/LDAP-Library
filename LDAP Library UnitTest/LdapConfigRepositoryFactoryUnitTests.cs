using System;
using LDAPLibrary;
using LDAPLibrary.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapConfigRepositoryFactoryUnitTests
    {
        [TestMethod, TestCategory("LdapConfigRepositoryFactory")]
        public void GetLdapConfigRepository()
        {
            var configRepo = LdapConfigRepositoryFactory.GetConfigRepository();

            Assert.IsInstanceOfType(configRepo,typeof(LdapConfigRepository));
        }
    }
}
