using LDAPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class ParameterChecker
    {
        private const string test = "ciccio";

        [TestMethod,TestCategory("Parameter Checker")]
        public void ParametersNotNull()
        {
            Assert.IsFalse(LdapParameterChecker.ParametersIsNullOrEmpty(new []{test,test}));
        }

        [TestMethod, TestCategory("Parameter Checker")]
        public void ParametersNull()
        {
            Assert.IsTrue(LdapParameterChecker.ParametersIsNullOrEmpty(new[] { null, test }));
        }

        [TestMethod, TestCategory("Parameter Checker")]
        public void ParametersEmpty()
        {
            Assert.IsTrue(LdapParameterChecker.ParametersIsNullOrEmpty(new[] { "", test }));
        }

        [TestMethod, TestCategory("Parameter Checker")]
        public void ParametersNullAndEmpty()
        {
            Assert.IsTrue(LdapParameterChecker.ParametersIsNullOrEmpty(new[] { "", null }));
        }
    }
}
