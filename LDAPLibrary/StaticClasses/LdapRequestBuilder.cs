using System.DirectoryServices.Protocols;
using System.Runtime.CompilerServices;
using LDAPLibrary.Interfarces;

[assembly: InternalsVisibleTo("LDAP Library UnitTest")]

namespace LDAPLibrary.StaticClasses
{
    internal static class LdapRequestBuilder
    {
        public static AddRequest GetAddRequest(ILdapUser testUser, string objectClass)
        {
            throw new System.NotImplementedException();
        }

        public static DeleteRequest GetDeleteRequest(ILdapUser user)
        {
            throw new System.NotImplementedException();
        }

        public static ModifyRequest GetModifyRequest(ILdapUser user, DirectoryAttributeOperation attributeOperation, string attributeName, string attributeValue)
        {
            throw new System.NotImplementedException();
        }

        public static ModifyRequest GetModifyPasswordRequest(ILdapUser testUser, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public static SearchRequest GetSearchPasswordRequest(string baseDn, string searchFilter, string[] searchAttributes)
        {
            throw new System.NotImplementedException();
        }
    }
}
