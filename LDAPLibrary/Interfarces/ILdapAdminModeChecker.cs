namespace LDAPLibrary.Interfarces
{
    internal interface ILdapAdminModeChecker
    {
        bool IsAdminMode();
        bool IsNoAdminMode();
        bool IsAnonymousMode();
    }
}