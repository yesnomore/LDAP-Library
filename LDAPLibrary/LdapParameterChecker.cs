using System;
using System.Collections.Generic;
using System.Linq;

namespace LDAPLibrary
{
    static internal class LdapParameterChecker
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