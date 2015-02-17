using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    internal static class LdapConnectorFactory
    {
        public static ILdapConnector GetLdapConnector(ILdapAdminModeChecker adminModeChecker,
            ILdapConfigRepository configRepository, ILogger logger)
        {
            return new LdapConnector(adminModeChecker, configRepository, logger);
        }
    }
}