using System.DirectoryServices.Protocols;
using System.Security.Cryptography.X509Certificates;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    /// <summary>
    /// Factory Class used to create teh LDAP connection Object
    /// </summary>
    public static class LdapConnectionFactory
    {
        /// <summary>
        /// Static Method used to create an LDAP connection object
        /// </summary>
        /// <param name="ldapConfigRepository">Repository of all LDAP configuration</param>
        /// <returns></returns>
        public static LdapConnection GetLdapConnection(ILdapConfigRepository ldapConfigRepository)
        {
            var ldapConnection = new LdapConnection(ldapConfigRepository.GetServer())
            {
                AuthType = ldapConfigRepository.GetAuthType()
            };
            ldapConnection.SessionOptions.ProtocolVersion = 3;

            if (ldapConfigRepository.GetSecureSocketLayerFlag())
                ldapConnection.SessionOptions.SecureSocketLayer = true;

            if (ldapConfigRepository.GetTransportSocketLayerFlag())
                ldapConnection.SessionOptions.StartTransportLayerSecurity(null);

            if (ldapConfigRepository.GetClientCertificateFlag())
            {
                var clientCertificateFile = new X509Certificate();
                clientCertificateFile.Import(ldapConfigRepository.GetClientCertificatePath());
                ldapConnection.ClientCertificates.Add(clientCertificateFile);
                ldapConnection.SessionOptions.VerifyServerCertificate += (conn, cert) => true;
            }

            return ldapConnection;
        }
    }
}
