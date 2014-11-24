namespace LDAPLibrary.Interfarces
{
    internal interface ILdapConnectionObservable
    {
        void LdapConnectionSubscribe(ILdapConnectionObserver observer);
        void LdapConnectionUnsubscribe(ILdapConnectionObserver observer);
    }
}