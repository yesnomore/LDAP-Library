using System.Net;
using LDAPLibrary.Enums;
using LDAPLibrary.Factories;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Connectors
{
    class AnonymousLdapConnector : ALdapConnector
    {
        public AnonymousLdapConnector(ILdapConfigRepository configRepository, ILogger logger) : base(configRepository, logger)
        {
        }

        protected override LdapState ConnectAdmin()
        {
            _ldapConnection = LdapConnectionFactory.GetLdapConnection(_configRepository);
            var adminCredential = new NetworkCredential(_configRepository.GetAdminUser().GetUserDn(),
                _configRepository.GetAdminUser().GetUserAttribute("userPassword")[0]);
            
            _ldapConnection.Bind(adminCredential);
            
            
            _observers.ForEach(x => x.SetLdapConnection(_ldapConnection));
            
            
            _logger.Write(_logger.BuildLogMessage(SuccessConnectionMessage(adminCredential), LdapState.LdapConnectionSuccess));
            return LdapState.LdapConnectionSuccess;
        }

        protected override void ConnectUser(NetworkCredential credential)
        {
            StandardConnect(credential);
        }
    }
}
