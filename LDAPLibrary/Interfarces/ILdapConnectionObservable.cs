namespace LDAPLibrary.Interfarces
{
    interface ILdapConnectionObservable
    {
        void LdapConnectionSubscribe(ILdapConnectionObserver observer);
        void LdapConnectionUnsubscribe(ILdapConnectionObserver observer);
    }
}