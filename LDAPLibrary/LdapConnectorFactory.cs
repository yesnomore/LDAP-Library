namespace LDAPLibrary
{
    static class LdapConnectorFactory
    {
        public static ILdapConnector GetLdapConnector(LdapModeChecker modeChecker,ILdapConfigRepository configRepository, ILogger logger)
        {
            return new LdapConnector(modeChecker,configRepository,logger);
        }
    }
}
