using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    public static class LdapConfigRepositoryFactory
    {
        public static ILdapConfigRepository GetConfigRepository()
        {
            return new LdapConfigRepository();
        }
    }
}