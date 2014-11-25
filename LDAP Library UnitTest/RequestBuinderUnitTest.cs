using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using LDAPLibrary;
using LDAPLibrary.Interfarces;
using LDAPLibrary.StaticClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class RequestBuinderUnitTest
    {
        private const string UserCn = "Matteo";
        private const string UserPwd = "1";
        private const string UserDn = "cn=" + UserCn + ",o=ApexNet,ou=People,dc=maxcrc,dc=com";

        private readonly ILdapUser _testUser = new LdapUser(UserDn, UserCn, "test",
            new Dictionary<string, List<string>>
            {
                {"userPassword", new List<string> {UserPwd}},
                {"description", new List<string> {"Test Description"}}
            });

        private const string ObjectClass = "person";


        [TestMethod, TestCategory("RequestBuilder")]
        public void AddRequest()
        {
            AddRequest req = LdapRequestBuilder.GetAddRequest(_testUser, ObjectClass);

            Assert.AreEqual(UserDn, req.DistinguishedName);

            Assert.IsTrue(req.Attributes.Contains(new DirectoryAttribute("objectClass", ObjectClass)));
            Assert.IsTrue(req.Attributes.Contains(new DirectoryAttribute("cn", UserCn)));
            Assert.IsTrue(req.Attributes.Contains(new DirectoryAttribute("sn", "test")));
            Assert.IsTrue(req.Attributes.Contains(new DirectoryAttribute("description", "Test Description")));
            Assert.IsTrue(req.Attributes.Contains(new DirectoryAttribute("userPassword", UserPwd)));
        }

        [TestMethod, TestCategory("RequestBuilder")]
        public void DeleteRequest()
        {
            DeleteRequest req = LdapRequestBuilder.GetDeleteRequest(_testUser);
            Assert.AreEqual(UserDn, req.DistinguishedName);
        }

        [TestMethod, TestCategory("RequestBuilder")]
        public void ModifyRequest()
        {
            ModifyRequest req = LdapRequestBuilder.GetModifyRequest(_testUser, DirectoryAttributeOperation.Replace, "description",
                "Test Description 2");

            Assert.AreEqual(UserDn, req.DistinguishedName);

            var attributeModification = new DirectoryAttributeModification
            {
                Operation = DirectoryAttributeOperation.Replace,
                Name = "description",
            };
            attributeModification.Add("Test Description 2");

            Assert.IsTrue(req.Modifications.Contains(attributeModification));

        }

        [TestMethod, TestCategory("RequestBuilder")]
        public void ModifyPasswordRequest()
        {
            ModifyRequest req = LdapRequestBuilder.GetModifyPasswordRequest(_testUser, "new pwd");

            Assert.AreEqual(UserDn, req.DistinguishedName);

            var attributeModification = new DirectoryAttributeModification
            {
                Operation = DirectoryAttributeOperation.Replace,
                Name = "userPassword",
            };
            attributeModification.Add("new pwd");

            Assert.IsTrue(req.Modifications.Contains(attributeModification));
        }

        public void SearchRequest()
        {
            var ldapSearchFilter = String.Format("(&(objectClass={0})({1}={2}))", ObjectClass, "cn", UserCn);
            const string baseDn = "o=ApexNet,ou=People,dc=maxcrc,dc=com";
            var attributes = new[] {"cn", "sn"};

            SearchRequest req = LdapRequestBuilder.GetSearchPasswordRequest(baseDn, ldapSearchFilter, attributes);

            Assert.AreEqual(baseDn, req.DistinguishedName);
            Assert.AreEqual(ldapSearchFilter, req.Filter);
            Assert.AreEqual(SearchScope.Subtree, req.Scope);
            Assert.AreEqual(attributes, req.Attributes);
        }
    }
}
