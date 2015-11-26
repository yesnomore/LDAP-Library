using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
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
            if (String.IsNullOrEmpty(credential.UserName)) throw new InvalidCredentialException("Username cannot be null or empty");
            if (String.IsNullOrEmpty(credential.Password)) throw new InvalidCredentialException("Password cannot be null or empty");

            _ldapConnection = LdapConnectionFactory.GetLdapConnection(credential, _configRepository);
            _ldapConnection.Dispose();
        }
    }
}
