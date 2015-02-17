using System.Runtime.CompilerServices;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;

[assembly: InternalsVisibleTo("LDAP Library UnitTest")]

namespace LDAPLibrary
{
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