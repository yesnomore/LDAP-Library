using LDAPLibrary.StaticClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class ParameterCheckerUnitTests
    {
        private const string Test = "ciccio";

        [TestMethod, TestCategory("Parameter Checker")]
        public void ParametersNotNull()
        {
            Assert.IsFalse(LdapParameterChecker.ParametersIsNullOrEmpty(new[] {Test, Test}));
        }

        [TestMethod, TestCategory("Parameter Checker")]
        public void ParametersNull()
        {
            Assert.IsTrue(LdapParameterChecker.ParametersIsNullOrEmpty(new[] {null, Test}));
        }

        [TestMethod, TestCategory("Parameter Checker")]
        public void ParametersEmpty()
        {
            Assert.IsTrue(LdapParameterChecker.ParametersIsNullOrEmpty(new[] {"", Test}));
        }

        [TestMethod, TestCategory("Parameter Checker")]
        public void ParametersNullAndEmpty()
        {
            Assert.IsTrue(LdapParameterChecker.ParametersIsNullOrEmpty(new[] {"", null}));
        }
    }
}