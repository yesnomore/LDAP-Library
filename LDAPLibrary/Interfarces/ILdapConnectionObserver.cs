using System.DirectoryServices.Protocols;

namespace LDAPLibrary.Interfarces
{
    internal interface ILdapConnectionObserver
    {
        void SetLdapConnection(LdapConnection ldapConnection);
    }
}