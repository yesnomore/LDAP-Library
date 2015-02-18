using System.DirectoryServices.Protocols;

namespace LDAPLibrary.Interfarces
{
    public interface ILdapConnectionObserver
    {
        void SetLdapConnection(LdapConnection ldapConnection);
    }
}