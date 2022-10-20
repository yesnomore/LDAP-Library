using System.Net;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Connectors
{
    class AdminLdapConnector : ALdapConnector
    {
        public AdminLdapConnector(ILdapConfigRepository configRepository, ILogger logger) : base(configRepository, logger)
        {
        }

        protected override LdapState ConnectAdmin()
        {
            var username = $"cn={_configRepository.GetAdminUser().GetUserCn()},{_configRepository.GetAdminUser().GetUserDn()}";

            var returnState = Connect(
                        new NetworkCredential(username,
                                                _configRepository.GetAdminUser().GetUserAttribute("userPassword")[0]));
            _observers.ForEach(x => x.SetLdapConnection(_ldapConnection));
            return returnState;
        }

        protected override void ConnectUser(NetworkCredential credential)
        {
            StandardConnect(credential);
        }
    }
}
