using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    static class LdapConnectorFactory
    {
        public static ILdapConnector GetLdapConnector(LdapModeChecker modeChecker,ILdapConfigRepository configRepository, ILogger logger)
        {
            return new LdapConnector(modeChecker,configRepository,logger);
        }
    }
}
