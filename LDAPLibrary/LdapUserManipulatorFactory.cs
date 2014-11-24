namespace LDAPLibrary
{
    static class LdapUserManipulatorFactory
    {
        public static LdapUserManipulator GetUserManipulator(ILdapConnectionObservable observable)
        {
            var userManipulator = new LdapUserManipulator();
            observable.LdapConnectionSubscribe(userManipulator);
            return userManipulator;
        }
    }
}
