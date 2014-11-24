namespace LDAPLibrary
{
    static class LdapStateUtils
    {
        public static bool ToBoolean(LdapState state)
        {
            return state == LdapState.LdapConnectionSuccess || state == LdapState.LdapLibraryInitSuccess ||
                   state == LdapState.LdapUserManipulatorSuccess;
        }
    }
}
