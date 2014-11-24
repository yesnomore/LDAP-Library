namespace LDAPLibrary
{
    interface ILdapConnectionObservable
    {
        void LdapConnectionSubscribe(ILdapConnectionObserver observer);
        void LdapConnectionUnsubscribe(ILdapConnectionObserver observer);
    }
}