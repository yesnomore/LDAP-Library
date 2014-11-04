using System;
using System.Collections.Generic;
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
        private readonly Dictionary<string, List<string>> LdapUserAttributes = 
            new Dictionary<string, List<string>>
            {
                { "userPassword", new List<string> { "secret" } },
                { "description", new List<string> { "test description" } },
                { "telephoneNumber", new List<string> { "555-54321" } }
            };

        #region Init Unit test
        [TestMethod, TestCategory("LDAPUser Init")]
        public void LdapUserCreationNullOtherAttribute()
        {
            var user = new LdapUser(LdapUserDn,
            LdapUserCn,
            LdapUserSn,
            null);

            Assert.IsNotNull(user);
        }

        [TestMethod, TestCategory("LDAPUser Init")]
        public void LdapUserCreation()
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
        public void LdapUserCreationNullDn()
        {
            new LdapUser(null,
                LdapUserCn,
                LdapUserSn,
                null);
        }

        [TestMethod, TestCategory("LDAPUser Init")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the user with cn null don't throw an exception")]
        public void LdapUserCreationNullCn()
        {
            new LdapUser(LdapUserDn,
                null,
                LdapUserSn,
                null);
        }
        [TestMethod, TestCategory("LDAPUser Init")]
        [ExpectedException(typeof(ArgumentException),
    "The creation of the user with sn null don't throw an exception")]
        public void LdapUserCreationNullSn()
        {
            new LdapUser(LdapUserDn,
                LdapUserCn,
                null,
                null);
        } 
        #endregion


    }
}
