using LDAPLibrary;
using LDAPLibrary.Enums;
using LDAPLibrary.StaticClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapStateUtilsUnitTests
    {
        [TestMethod, TestCategory("LdapStateUtils")]
        public void ToBoolean()
        {
            Assert.IsTrue(LdapStateUtils.ToBoolean(LdapState.LdapConnectionSuccess));
            Assert.IsTrue(LdapStateUtils.ToBoolean(LdapState.LdapLibraryInitSuccess));
            Assert.IsTrue(LdapStateUtils.ToBoolean(LdapState.LdapUserManipulatorSuccess));

            Assert.IsFalse(LdapStateUtils.ToBoolean(LdapState.LdapChangeUserPasswordError));
            Assert.IsFalse(LdapStateUtils.ToBoolean(LdapState.LdapConnectionError));
            Assert.IsFalse(LdapStateUtils.ToBoolean(LdapState.LdapCreateUserError));
            Assert.IsFalse(LdapStateUtils.ToBoolean(LdapState.LdapDeleteUserError));
            Assert.IsFalse(LdapStateUtils.ToBoolean(LdapState.LdapGenericError));
            Assert.IsFalse(LdapStateUtils.ToBoolean(LdapState.LdapLibraryInitError));
            Assert.IsFalse(LdapStateUtils.ToBoolean(LdapState.LdapModifyUserAttributeError));
            Assert.IsFalse(LdapStateUtils.ToBoolean(LdapState.LdapSearchUserError));
        }
    }
}