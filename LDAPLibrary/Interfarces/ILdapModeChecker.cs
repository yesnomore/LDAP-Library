namespace LDAPLibrary.Interfarces
{
    internal interface ILdapModeChecker
    {
        bool IsBasicMode();
        bool IsCompleteMode();
    }
}