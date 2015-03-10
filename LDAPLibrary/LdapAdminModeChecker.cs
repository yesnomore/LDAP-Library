using System.Runtime.CompilerServices;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;

[assembly: InternalsVisibleTo("LDAP Library UnitTest")]

namespace LDAPLibrary
{
    /// <summary>
    /// Check the ldap library mode.
    /// Simply check the value of AdminMode in the config repository object passed in the constructor.
    /// </summary>
    internal class LdapAdminModeChecker : ILdapAdminModeChecker
    {
        private readonly ILdapConfigRepository _configRepository;

        public LdapAdminModeChecker(ILdapConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public bool IsAdminMode()
        {
            return _configRepository.GetAdminMode() == LDAPAdminMode.Admin;
        }

        public bool IsNoAdminMode()
        {
            return _configRepository.GetAdminMode() == LDAPAdminMode.NoAdmin;
        }

        public bool IsAnonymousMode()
        {
            return _configRepository.GetAdminMode() == LDAPAdminMode.Anonymous;
        }
    }
}