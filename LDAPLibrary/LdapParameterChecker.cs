using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LDAP Library UnitTest")]

namespace LDAPLibrary
{
    internal static class LdapParameterChecker
    {
        /// <summary>
        ///     Check all the string parameters
        /// </summary>
        /// <returns>true if all is set, false otherwise</returns>
        public static bool ParametersIsNullOrEmpty(IEnumerable<string> parameters)
        {
            return parameters.Any(String.IsNullOrEmpty);
        }
    }
}