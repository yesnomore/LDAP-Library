using System;
using System.Net;
using System.Security.Authentication;
using LDAPLibrary.Enums;
using LDAPLibrary.Factories;
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
            return StandardAdminConnect();
        }

        protected override void ConnectUser(NetworkCredential credential)
        {
            if (String.IsNullOrEmpty(credential.UserName)) throw new InvalidCredentialException("Username cannot be null or empty");
            if (String.IsNullOrEmpty(credential.Password)) throw new InvalidCredentialException("Password cannot be null or empty");

            _ldapConnection = LdapConnectionFactory.GetLdapConnection(credential, _configRepository);
            _ldapConnection.Bind(credential);
        }
    }
}
