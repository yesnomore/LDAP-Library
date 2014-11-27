using System;
using System.Collections.Generic;
using LDAPLibrary;
using LDAPLibrary.StaticClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class FilterBuilderTest
    {
        private const string ObjectClass = "person";
        private const string FieldUsername = "cn";
        private const string UserCn = "Manager";

        [TestMethod,TestCategory("FilterBuilder Search")]
        public void SearchFilterBuilder()
        {
            var searchFilterExpected = String.Format("(&(objectClass={0})({1}={2}))", ObjectClass, FieldUsername, UserCn);
            var searchFilter = LdapFilterBuilder.GetSearchFilter(ObjectClass,FieldUsername,UserCn);

            Assert.AreEqual(searchFilterExpected,searchFilter);
        }
    }
}
