using System;
using System.Collections.Generic;
using System.Linq;
using LDAPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapUserUnitTests
    {
        private const string LdapUserDn = "cn=Manager,dc=maxcrc,dc=com";
        private const string LdapUserCn = "Manager";
        private const string LdapUserSn = "test";
        private static readonly Dictionary<string, List<string>> LdapUserAttributes = 
            new Dictionary<string, List<string>>
            {
                { "userPassword", new List<string> { "secret" } },
                { "description", new List<string> { "test description" } },
                { "telephoneNumber", new List<string> { "555-54321" } }
            };

        private LdapUser TestUser = new LdapUser(LdapUserDn,
            LdapUserCn,
            LdapUserSn,
            LdapUserAttributes);

        #region Init Unit test
        [TestMethod, TestCategory("LDAPUser Init")]
        public void LdapUserInstanceNullOtherAttribute()
        {
            var user = new LdapUser(LdapUserDn,
            LdapUserCn,
            LdapUserSn,
            null);

            Assert.IsNotNull(user);
        }

        [TestMethod, TestCategory("LDAPUser Init")]
        public void LdapUserInstance()
        {
            var user = new LdapUser(LdapUserDn,
            LdapUserCn,
            LdapUserSn,
            LdapUserAttributes);

            Assert.IsNotNull(user);
        }

        [TestMethod, TestCategory("LDAPUser Init")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the user with dn null don't throw an exception")]
        public void NullDn()
        {
            new LdapUser(null,
                LdapUserCn,
                LdapUserSn,
                null);
        }
        [TestMethod, TestCategory("LDAPUser Init")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the user with cn null don't throw an exception")]
        public void NullCn()
        {
            new LdapUser(LdapUserDn,
                null,
                LdapUserSn,
                null);
        }
        [TestMethod, TestCategory("LDAPUser Init")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the user with sn null don't throw an exception")]
        public void NullSn()
        {
            new LdapUser(LdapUserDn,
                LdapUserCn,
                null,
                null);
        } 
        #endregion

        #region Getter Tests

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserAttribute()
        {
            Assert.AreEqual(TestUser.GetUserAttribute("userPassword")[0], "secret");
            Assert.IsInstanceOfType(TestUser.GetUserAttribute("userPassword"), typeof(List<string>));

            Assert.AreEqual(TestUser.GetUserAttribute("description")[0], "test description");
            Assert.IsInstanceOfType(TestUser.GetUserAttribute("description"), typeof(List<string>));

            Assert.AreEqual(TestUser.GetUserAttribute("telephoneNumber")[0], "555-54321");
            Assert.IsInstanceOfType(TestUser.GetUserAttribute("telephoneNumber"), typeof(List<string>));

        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        [ExpectedException(typeof (ArgumentException),
            "if the key requested is not in che dictionary an exception is throw")]
        public void GetUserAttributeNoKey()
        {
            TestUser.GetUserAttribute("NoKey");
        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserAttributes()
        {
            Assert.AreEqual(TestUser.GetUserAttributes(),LdapUserAttributes);
        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserAttributesKeys()
        {
            CollectionAssert.AreEqual(TestUser.GetUserAttributeKeys(), LdapUserAttributes.Keys.ToArray());
        }

        #endregion

        #region Setter Tests
        #endregion
    }
}
