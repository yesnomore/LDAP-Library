using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    internal static class LdapUserManipulatorFactory
    {
        public static ILdapUserManipulator GetUserManipulator(ILdapConnectionObservable connector, ILogger logger,
            ILdapConfigRepository configRepository)
        {
            var userManipulator = new LdapUserManipulator(logger, configRepository);
            connector.LdapConnectionSubscribe(userManipulator);
            return userManipulator;
        }
    }
}