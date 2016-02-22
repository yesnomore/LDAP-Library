using System;
using System.DirectoryServices.Protocols;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;
using LDAPLibrary.StaticClasses;

namespace LDAPLibrary
{

    /// <summary>
    /// Repository for all the configuration and input of the library.
    /// Created at startup.
    /// Also Check the validity of the Parameters and, in case, throw an ArgumentNullException, with specific messages.
    /// </summary>
    public class LdapConfigRepository : ILdapConfigRepository
    {
        private const string BasicConfigNullParametersErrorMessage =
            "Server or SearchBaseDn parameter cannot be null or empty and the file log path cannot be null if the logType is 'File' ";

        private const string CompleteConfigNullParametersErrorMessage =
            "One param are null or empty:Admin User: {0},clientCertificatePath: {1}, userObjectClass: {2},matchFieldUsername: {3}";

        #region Configuration Parameters

        private ILdapUser _adminUser;
        private AuthType _authType;
        private bool _clientCertificateFlag;
        private string _clientCertificatePath;
        private string _logPath;
        private LoggerType _loggerType;
        private string _matchFieldUsername;
        private string _searchBaseDn;
        private bool _secureSocketLayerFlag;
        private string _server;
        private bool _transportSocketLayerFlag;
        private string _userObjectClass;
        private LDAPAdminMode _adminMode;
        private TimeSpan _connectionTimeout;


        #endregion

        #region Configuration Parameters Getters

        public ILdapUser GetAdminUser()
        {
            return _adminUser ?? new FakeLdapUser();
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

        public LoggerType GetWriteLogFlag()
        {
            return _loggerType;
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

        public LDAPAdminMode GetAdminMode()
        {
            return _adminMode;
        }

        public TimeSpan GetConnectionTimeout()
        {
            return _connectionTimeout;
        }

        #endregion

        public void BasicLdapConfig(ILdapUser adminUser, LDAPAdminMode adminMode, string server, string searchBaseDn, AuthType authType, LoggerType loggerType, string logPath)
        {
            _adminMode = adminMode;
            _loggerType = loggerType;
            _authType = authType;

            BasicLdapConfigValidator(server,logPath,searchBaseDn,adminUser);

            _searchBaseDn = searchBaseDn;
            _server = server;
            _adminUser = adminUser;
            _logPath = logPath;
            

            StandardLdapInformation();
        }

        public void AdditionalLdapConfig(
            bool secureSocketLayerFlag, bool transportSocketLayerFlag, bool clientCertificateFlag,
            string clientCertificatePath, string userObjectClass,
            string matchFieldUsername, TimeSpan connectionTimeout)
        {
            _clientCertificateFlag = clientCertificateFlag;
            _transportSocketLayerFlag = transportSocketLayerFlag;
            _secureSocketLayerFlag = secureSocketLayerFlag;

            AddictionalLdapConfigValidator(clientCertificatePath, userObjectClass, matchFieldUsername);

            _matchFieldUsername = matchFieldUsername;
            _userObjectClass = userObjectClass;
            _clientCertificatePath = clientCertificatePath;
            _connectionTimeout = connectionTimeout;
        }

        /// <summary>
        /// Set LDAP Information To standard values.
        /// </summary>
        private void StandardLdapInformation()
        {
            //Default class variables information
            _secureSocketLayerFlag = false;
            _transportSocketLayerFlag = false;
            _clientCertificateFlag = false;
            _clientCertificatePath = "";
            _userObjectClass = "person";
            _matchFieldUsername = "cn";
        }

        /// <summary>
        /// Check the validity of parameters
        /// 
        /// * Server and searchBaseDn cannot be null or empty
        /// * logPath cannot be null or empty if the loggerType is set to File
        /// * Admin cannot be null if there's admin mode
        /// 
        /// Used in the BasicLdapConfig method
        /// 
        /// Can throw an ArgumentNullException
        /// </summary>
        /// <param name="server">Server URL</param>
        /// <param name="loggerType"></param>
        /// <param name="logPath">Path of the log file</param>
        /// <param name="searchBaseDn">Search Root Node</param>
        /// <param name="admin">Admin User</param>
        /// <param name="adminMode">Library Admin Mode</param>
        private void BasicLdapConfigValidator(string server, string logPath, string searchBaseDn, ILdapUser admin)
        {
            if (     String.IsNullOrEmpty(server) 
                ||  (String.IsNullOrEmpty(logPath) && GetWriteLogFlag() == LoggerType.File)
                ||  (GetAdminMode() == LDAPAdminMode.Admin && admin == null)
                ||  (String.IsNullOrEmpty(searchBaseDn) && GetAdminMode() != LDAPAdminMode.NoAdmin))
                throw new ArgumentNullException(String.Format(BasicConfigNullParametersErrorMessage));
        }

        /// <summary>
        /// Check the validity of parameters
        /// 
        /// * Check if clientCertificatePath, userObjectClass, matchFieldUsername are null or empty
        /// 
        /// Used in AdditionalLdapConfig
        /// 
        /// Can throw an ArgumentNullException
        /// </summary>
        /// <param name="clientCertificatePath"></param>
        /// <param name="userObjectClass"></param>
        /// <param name="matchFieldUsername"></param>
        private void AddictionalLdapConfigValidator(string clientCertificatePath, string userObjectClass,string matchFieldUsername)
        {
            if (LdapParameterChecker.ParametersIsNullOrEmpty(new[] { userObjectClass, matchFieldUsername }) || String.IsNullOrEmpty(clientCertificatePath) && GetClientCertificateFlag() == true)
                throw new ArgumentNullException(String.Format(CompleteConfigNullParametersErrorMessage,
                    _adminUser, clientCertificatePath, userObjectClass, matchFieldUsername));
        }
    }
}