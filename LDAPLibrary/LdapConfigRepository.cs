using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;

namespace LDAPLibrary
{
    public class LdapConfigRepository : ILdapConfigRepository
    {
        private const string BasicConfigNullParametersErrorMessage =
            "One param are null or empty: Server: {0},Search Base DN: {1},Admin User: {2}";
        private const string CompleteConfigNullParametersErrorMessage =
            "One param are null or empty: Server: {0},Search Base DN: {1},Admin User: {2},clientCertificatePath: {3}, logPath: {4}, userObjectClass: {5},matchFieldUsername: {6}";

        #region Configuration Parameters

        private ILdapUser _adminUser;
        private AuthType _authType;
        private bool _clientCertificateFlag;
        private string _clientCertificatePath;
        private string _logPath;
        private string _matchFieldUsername;
        private string _searchBaseDn;
        private bool _secureSocketLayerFlag;
        private string _server;
        private bool _transportSocketLayerFlag;
        private string _userObjectClass;
        private bool _writeLogFlag;

        #endregion

        #region Configuration Parameters Getters

        public ILdapUser GetAdminUser()
        {
            return _adminUser;
        }

        public string GetServer()
        {
            return _server;
        }

        public string GetSearchBaseDn()
        {
            return _searchBaseDn;
        }

        public AuthType GetAuthType()
        {
            return _authType;
        }

        public bool GetSecureSocketLayerFlag()
        {
            return _secureSocketLayerFlag;
        }

        public bool GetTransportSocketLayerFlag()
        {
            return _transportSocketLayerFlag;
        }

        public bool GetClientCertificateFlag()
        {
            return _clientCertificateFlag;
        }

        public string GetClientCertificatePath()
        {
            return _clientCertificatePath;
        }

        public bool GetWriteLogFlag()
        {
            return _writeLogFlag;
        }

        public string GetLogPath()
        {
            return _logPath;
        }

        public string GetUserObjectClass()
        {
            return _userObjectClass;
        }

        public string GetMatchFieldName()
        {
            return _matchFieldUsername;
        }

        #endregion

        public void BasicLdapConfig(ILdapUser adminUser, string server, string searchBaseDn, AuthType authType)
        {
            if (ParametersIsNullOrEmpty(new[] { searchBaseDn, server }) || adminUser == null)
                throw new ArgumentNullException(String.Format(BasicConfigNullParametersErrorMessage, server, searchBaseDn, adminUser));

            _authType = authType;
            _searchBaseDn = searchBaseDn;
            _server = server;
            _adminUser = adminUser;

            StandardLdapInformation();
        }

        public void CompleteLdapConfig(ILdapUser adminUser, string server, string searchBaseDn, AuthType authType,
            bool secureSocketLayerFlag, bool transportSocketLayerFlag, bool clientCertificateFlag,
            string clientCertificatePath, bool writeLogFlag, string logPath, string userObjectClass,
            string matchFieldUsername)
        {
            if (ParametersIsNullOrEmpty(new[] { searchBaseDn, server, clientCertificatePath, logPath, userObjectClass, matchFieldUsername }) 
                || adminUser == null)
                throw new ArgumentNullException(String.Format(CompleteConfigNullParametersErrorMessage, server, searchBaseDn,
                    adminUser, clientCertificatePath, logPath, userObjectClass, matchFieldUsername));


            _matchFieldUsername = matchFieldUsername;
            _userObjectClass = userObjectClass;
            _logPath = logPath;
            _writeLogFlag = writeLogFlag;
            _clientCertificatePath = clientCertificatePath;
            _clientCertificateFlag = clientCertificateFlag;
            _transportSocketLayerFlag = transportSocketLayerFlag;
            _secureSocketLayerFlag = secureSocketLayerFlag;
            _authType = authType;
            _searchBaseDn = searchBaseDn;
            _server = server;
            _adminUser = adminUser;
        }

        /// <summary>
        ///     Check all the string parameters
        /// </summary>
        /// <returns>true if all is set, false otherwise</returns>
        private static bool ParametersIsNullOrEmpty(IEnumerable<string> parameters)
        {
            return parameters.Any(String.IsNullOrEmpty);
        }

        /// <summary>
        ///     Set LDAP Information To standard values.
        /// </summary>
        private void StandardLdapInformation()
        {
            //Default class variables information
            _secureSocketLayerFlag = false;
            _transportSocketLayerFlag = false;
            _clientCertificateFlag = false;
            _clientCertificatePath = "";
            _writeLogFlag = false;
            _logPath = "";
            _userObjectClass = "person";
            _matchFieldUsername = "cn";
        }

    }
}