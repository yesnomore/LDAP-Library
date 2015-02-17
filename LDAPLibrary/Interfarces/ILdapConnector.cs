using System.Net;
using LDAPLibrary.Enums;

namespace LDAPLibrary.Interfarces
{
    internal interface ILdapConnector : ILdapConnectionObservable
    {
        /// <summary>
        ///     Instance the Ldap connection with admin config credential
        /// </summary>
        /// <returns>Success or Failed</returns>
        LdapState Connect();

        /// <summary>
        ///     Connect to LDAP with the specified credential
        /// </summary>
        /// <param name="credential">user Credential</param>
        /// <param name="secureSocketLayer">Flag that specify if we want to use SSL for connection.</param>
        /// <param name="transportSocketLayer"></param>
        /// <param name="clientCertificate"></param>
        /// <returns>Success or Failed</returns>
        LdapState Connect(NetworkCredential credential, bool secureSocketLayer, bool transportSocketLayer,
            bool clientCertificate);
    }
}