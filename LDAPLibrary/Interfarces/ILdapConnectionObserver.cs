using System.DirectoryServices.Protocols;

namespace LDAPLibrary.Interfarces
{
    interface ILdapConnectionObserver
    {
        void SetLdapConnection(LdapConnection ldapConnection);
    }
}
