using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    internal static class LdapUserManipulatorFactory
    {
        public static LdapUserManipulator GetUserManipulator(ILdapConnectionObservable observable, ILogger logger)
        {
            var userManipulator = new LdapUserManipulator(logger);
            observable.LdapConnectionSubscribe(userManipulator);
            return userManipulator;
        }
    }
}