using System;
using System.Net;
using LDAPLibrary.Enums;

namespace LDAPLibrary.Interfarces
{
    internal interface ILdapConnector : ILdapConnectionObservable, IDisposable
    {
        /// <summary>
        /// Connect the Admin user
        /// Send the LDAPConnection to the observers.
        /// </summary>
        /// <returns>
        /// * LdapConnectionSuccess if the connection is successfull
        /// * LdapConnectionError if the mode is in NoAdmin
        /// * Exception if there's an error in the connection
        /// </returns>
        LdapState Connect();

        /// <summary>
        /// Connect to the LDAP using the credential in input and the parameters of TSL,SSL and certificate.
        /// </summary>
        /// <param name="credential">Credential to connect</param>
        /// <returns>
        /// * LdapConnectionSuccess
        /// * LdapConnectionError
        /// </returns>
        LdapState Connect(NetworkCredential credential);
    }
}