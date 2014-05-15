namespace LDAPLibrary
{
    public enum LDAPState
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
