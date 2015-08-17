using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Factories
{
    class LdapConnectionFactory
    {
        public static LdapConnection GetLdapConnection(NetworkCredential credential,
            ILdapConfigRepository ldapConfigRepository)
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
