namespace LDAPLibrary.StaticClasses
{
    internal static class LdapStateUtils
    {
        /// <summary>
        ///     Convert the state to a boolean
        ///     TRUE - success state
        ///     FALSE - error state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool ToBoolean(LdapState state)
        {
            return state == LdapState.LdapConnectionSuccess || state == LdapState.LdapLibraryInitSuccess ||
                   state == LdapState.LdapUserManipulatorSuccess;
        }
    }
}