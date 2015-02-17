using System;
using System.Collections.Generic;
using System.Globalization;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary.Logger
{
    internal abstract class ALogger : ILogger
    {
        protected const string DateFormat = "dd/MM/yyyy HH:mm:ss tt";

        private readonly Dictionary<LdapState, LdapError> _ldapErrors = new Dictionary<LdapState, LdapError>
        {
            {
                LdapState.LdapChangeUserPasswordError,
                m => "LDAP CHANGE USER PASSWORD ERROR: " + m
            },
            {
                LdapState.LdapConnectionError,
                m => "LDAP CONNECTION ERROR: " + m
            },
            {
                LdapState.LdapConnectionSuccess,
                m => "LDAP CONNECTION SUCCESS"
            },
            {
                LdapState.LdapCreateUserError,
                m => "LDAP CREATE USER ERROR: " + m
            },
            {
                LdapState.LdapDeleteUserError,
                m => "LDAP DELETE USER ERROR: " + m
            },
            {
                LdapState.LdapGenericError,
                m => "LDAP GENERIC ERROR"
            },
            {
                LdapState.LdapLibraryInitError,
                m => "LDAP LIBRARY INIT ERROR: " + m
            },
            {
                LdapState.LdapLibraryInitSuccess,
                m => "LDAP LIBRARY INIT SUCCESS"
            },
            {
                LdapState.LdapModifyUserAttributeError,
                m => "LDAP MODIFY USER ATTRIBUTE ERROR: " + m
            },
            {
                LdapState.LdapSearchUserError,
                m => "LDAP SEARCH USER ERROR: " + m
            },
            {
                LdapState.LdapUserManipulatorSuccess,
                m => "LDAP USER MANIPULATION SUCCESS: " + m
            }
        };

        public string BuildLogMessage(string message, LdapState state)
        {
            return String.Format("{0} - {1}", DateTime.Now.ToString(DateFormat, CultureInfo.InvariantCulture),
                _ldapErrors[state](message));
        }

        public abstract void Write(string message);

        private delegate string LdapError(string message);
    }
}