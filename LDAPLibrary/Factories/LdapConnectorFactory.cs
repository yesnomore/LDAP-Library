using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    internal static class LdapConnectorFactory
    {
        public static ILdapConnector GetLdapConnector(ILdapModeChecker modeChecker,
            ILdapConfigRepository configRepository, ILogger logger)
        {
            return new LdapConnector(modeChecker, configRepository, logger);
        }
    }
}