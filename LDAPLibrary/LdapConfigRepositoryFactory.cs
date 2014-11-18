namespace LDAPLibrary
{
    public static class LdapConfigRepositoryFactory
    {
        public static ILdapConfigRepository GetConfigRepository()
        {
            return new LdapConfigRepository();
        }
    }
}