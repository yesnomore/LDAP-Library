using System;
using System.DirectoryServices.Protocols;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary
{
    public class LdapConfigRepository : ILdapConfigRepository
    {
        private const string BasicConfigNullParametersErrorMessage =
            "Server parameter cannot be null or empty";

        private const string CompleteConfigNullParametersErrorMessage =
            "One param are null or empty:Search Base DN: {0},Admin User: {1},clientCertificatePath: {2}, logPath: {3}, userObjectClass: {4},matchFieldUsername: {5}";

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
            if (LdapParameterChecker.ParametersIsNullOrEmpty(new[] {server}))
                throw new ArgumentNullException(String.Format(BasicConfigNullParametersErrorMessage));

            _authType = authType;
            _searchBaseDn = searchBaseDn;
            _server = server;
            _adminUser = adminUser;

            StandardLdapInformation();
        }

        public void AdditionalLdapConfig(
            bool secureSocketLayerFlag, bool transportSocketLayerFlag, bool clientCertificateFlag,
            string clientCertificatePath, bool writeLogFlag, string logPath, string userObjectClass,
            string matchFieldUsername)
        {
            if (LdapParameterChecker.ParametersIsNullOrEmpty(new[]
            {_searchBaseDn, clientCertificatePath, logPath, userObjectClass, matchFieldUsername})
                || _adminUser == null)
                throw new ArgumentNullException(String.Format(CompleteConfigNullParametersErrorMessage, _searchBaseDn,
                    _adminUser, clientCertificatePath, logPath, userObjectClass, matchFieldUsername));

            _matchFieldUsername = matchFieldUsername;
            _userObjectClass = userObjectClass;
            _logPath = logPath;
            _writeLogFlag = writeLogFlag;
            _clientCertificatePath = clientCertificatePath;
            _clientCertificateFlag = clientCertificateFlag;
            _transportSocketLayerFlag = transportSocketLayerFlag;
            _secureSocketLayerFlag = secureSocketLayerFlag;
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