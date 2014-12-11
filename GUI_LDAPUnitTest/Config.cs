using System.Collections.Specialized;
using System.Configuration;

namespace GUI_LDAPUnitTest
{
    /// <summary>
    ///     Helper class when reading from separate configuration files
    /// </summary>
    public static class Config
    {
        private static NameValueCollection ldapLibrary =
            (NameValueCollection) ConfigurationManager.GetSection("LDAPLibrary");

        /// <summary>
        ///     Constants section
        /// </summary>
        public static NameValueCollection LDAPLibrary
        {
            get { return ldapLibrary; }
            set { ldapLibrary = value; }
        }
    }
}