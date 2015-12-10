using LDAPLibrary.Connectors;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    internal static class LdapConnectorFactory
    {
        public static ILdapConnector GetLdapConnector(ILdapAdminModeChecker adminModeChecker,ILdapConfigRepository configRepository, ILogger logger)
        {
            if (adminModeChecker.IsAnonymousMode()) return new AnonymousLdapConnector(configRepository,logger);
            if (adminModeChecker.IsNoAdminMode()) return new NoAdminLdapConnector(configRepository, logger);
            return new AdminLdapConnector(configRepository, logger);
        }
    }
}