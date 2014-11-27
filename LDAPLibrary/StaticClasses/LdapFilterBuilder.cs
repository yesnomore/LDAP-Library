using System;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("LDAP Library UnitTest")]

namespace LDAPLibrary.StaticClasses
{
    static class LdapFilterBuilder
    {
        private const string SearchFilterTemplate = "(&(objectClass={0})({1}={2}))";

        public static string GetSearchFilter(string objectClass, string fieldUsername, string user)
        {
            return String.Format(SearchFilterTemplate, objectClass, fieldUsername, user);
        }
    }
}
