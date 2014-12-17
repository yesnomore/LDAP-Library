using System;
using LDAPLibrary.StaticClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class FilterBuilderUnitTests
    {
        private const string ObjectClass = "person";
        private const string FieldUsername = "cn";
        private const string UserCn = "Manager";

        [TestMethod, TestCategory("FilterBuilder Search")]
        public void SearchFilterBuilder()
        {
            string searchFilterExpected = String.Format("(&(objectClass={0})({1}={2}))", ObjectClass, FieldUsername,
                UserCn);
            string searchFilter = LdapFilterBuilder.GetSearchFilter(ObjectClass, FieldUsername, UserCn);

            Assert.AreEqual(searchFilterExpected, searchFilter);
        }
    }
}