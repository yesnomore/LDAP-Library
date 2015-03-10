namespace LDAPLibrary.Interfarces
{
    internal interface ILdapConnectionObservable
    {
        /// <summary>
        /// Add the observer to the list of observers
        /// </summary>
        /// <param name="observer">Observer to add</param>
        void LdapConnectionSubscribe(ILdapConnectionObserver observer);

        /// <summary>
        /// Remove the observer to the list of observers
        /// </summary>
        /// <param name="observer">Observer to add</param>
        void LdapConnectionUnsubscribe(ILdapConnectionObserver observer);
    }
}