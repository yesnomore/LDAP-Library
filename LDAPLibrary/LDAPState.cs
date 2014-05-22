namespace LDAPLibrary
{
    public enum LdapState
    {
        LdapLibraryInitSuccess,
        LdapConnectionSuccess,
        LdapUserManipulatorSuccess,
        LdapConnectionError,
        LdapCreateUserError,
        LdapDeleteUserError,
        LdapModifyUserAttributeError,
        LdapChangeUserPasswordError,
        LdapSearchUserError,
        LdapLibraryInitError,
        LdapGenericError
    }
}
