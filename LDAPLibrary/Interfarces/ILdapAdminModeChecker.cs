namespace LDAPLibrary.Interfarces
{
    internal interface ILdapAdminModeChecker
    {
        /// <summary>
        /// Check if is in Admin Mode
        /// </summary>
        /// <returns>
        /// * True 
        /// * False
        /// </returns>
        bool IsAdminMode();

        /// <summary>
        /// Check if is in NaAdmin Mode
        /// </summary>
        /// <returns>
        /// * True 
        /// * False
        /// </returns>
        bool IsNoAdminMode();

        /// <summary>
        /// Check if is in Anonymouse Mode
        /// </summary>
        /// <returns>
        /// * True 
        /// * False
        /// </returns>
        bool IsAnonymousMode();
    }
}