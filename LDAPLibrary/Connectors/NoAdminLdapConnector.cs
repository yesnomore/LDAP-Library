using System;
using System.Net;
using System.Security.Authentication;
using LDAPLibrary.Enums;
using LDAPLibrary.Factories;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Connectors
{
    class NoAdminLdapConnector: ALdapConnector
    {
        public NoAdminLdapConnector(ILdapConfigRepository configRepository, ILogger logger) : base(configRepository, logger)
        {
        }

        protected override LdapState ConnectAdmin()
        {
            _logger.Write(  _logger.BuildLogMessage(AdminConnectionErrorMessageBasicMode,
                            LdapState.LdapConnectionError));
            return LdapState.LdapConnectionError;
        }

        protected override void ConnectUser(NetworkCredential credential)
        {
            StandardConnect(credential);
            _ldapConnection.Dispose();
        }
    }
}
