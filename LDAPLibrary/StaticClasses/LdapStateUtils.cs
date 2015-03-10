using System.Runtime.CompilerServices;
using LDAPLibrary.Enums;

[assembly: InternalsVisibleTo("LDAPLibraryUnitTest")]

namespace LDAPLibrary.StaticClasses
{
    /// <summary>
    /// Function utils to manage the LDAP State Enum
    /// </summary>
    internal static class LdapStateUtils
    {
        /// <summary>
        /// Convert the state to a boolean
        /// </summary>
        /// <param name="state">Input State</param>
        /// <returns>        
        /// TRUE - success state
        /// FALSE - error state
        /// </returns>
        public static bool ToBoolean(LdapState state)
        {
            return state == LdapState.LdapConnectionSuccess || state == LdapState.LdapLibraryInitSuccess ||
                   state == LdapState.LdapUserManipulatorSuccess;
        }
    }
}