using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            return StandardAdminConnect();
        }

        protected override void ConnectUser(NetworkCredential credential)
        {
            _ldapConnection = LdapConnectionFactory.GetLdapConnection(_configRepository);
            _ldapConnection.Bind(credential);
        }
    }
}
