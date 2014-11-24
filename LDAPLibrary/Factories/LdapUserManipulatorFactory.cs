using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    internal static class LdapUserManipulatorFactory
    {
        public static LdapUserManipulator GetUserManipulator(ILdapConnectionObservable observable)
        {
            var userManipulator = new LdapUserManipulator();
            observable.LdapConnectionSubscribe(userManipulator);
            return userManipulator;
        }
    }
}