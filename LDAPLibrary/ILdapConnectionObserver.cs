using System.DirectoryServices.Protocols;

namespace LDAPLibrary
{
    interface ILdapConnectionObserver
    {
        void SetLdapConnection(LdapConnection ldapConnection);
    }
}
