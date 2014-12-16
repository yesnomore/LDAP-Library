using System.Runtime.CompilerServices;
using LDAPLibrary.Interfarces;

[assembly: InternalsVisibleTo("LDAP Library UnitTest")]

namespace LDAPLibrary
{
    internal class LdapModeChecker : ILdapModeChecker
    {
        private readonly ILdapConfigRepository _configRepository;

        public LdapModeChecker(ILdapConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public bool IsBasicMode()
        {
            return _configRepository.GetAdminUser() == null;
        }

        public bool IsCompleteMode()
        {
            return _configRepository.GetAdminUser() != null;
        }
    }
}