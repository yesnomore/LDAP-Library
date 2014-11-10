using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace LDAPLibrary
{
    public class LdapManager : ILdapManager
    {
        #region Class Variables

        //Attioctional parameter class that have default values
        private const string DefaultUserSn = "Default Surname";
        //Sn used in library for the user than don't have that( almost always required in OpenLDAP)

        private const string DefaultUserCn = "Default CommonName";
        //Cn used in library for the user than don't have that(almost always required in OpenLDAP)

        private readonly AuthType _authType;
        //Set the authentication Type of ldapConnection http://msdn.microsoft.com/it-it/library/system.directoryservices.protocols.authtype(v=vs.110).aspx

        //Error Description from LDAP connection
        private readonly Dictionary<LdapState, LdapError> _ldapErrors;

        private readonly string _ldapInitLibraryErrorDescription;
        private readonly string _ldapSearchBaseDn; //LDAP base search DN
        private readonly string _ldapServer; //Server LDAP with port
        private readonly ILdapUser _loginUser; //LDAP User Admin
        private bool _clientCertificate; //Flag that specify if enable Client Certificate
        private string _clientCertificatePath; //Client Certificate Path
        private LdapConnection _ldapConnection;
        private string _ldapConnectionErrorDescription;
        private LdapState _ldapCurrentState;
        private string _logPath; //Log file path

        //External Class to delegate the jobs
        private LdapUserManipulator _manageLdapUser;

        private string _matchFieldUsername;
        //Field used in search filter to know what is the LDAP attribute to match with username

        private bool _secureSocketLayerConnection; //Flag that specify if enable SSL protocol
        private bool _transportSocketLayerConnection; //Flag that specify if enable TSL protocol
        private string _userObjectClass; //The attribute ObjectClass used to indentify an user
        private bool _writeLogFlag; //Flag that specify if enable the log

        private delegate string LdapError();

        #endregion

        /// <summary>
        ///     LDAP library constructior where all the class variables are initialized
        ///     The variables not specified in definition will be set at default values.
        /// </summary>
        /// <param name="adminUser"></param>
        /// <param name="ldapServer">LDAP Server with port</param>
        /// <param name="ldapSearchBaseDn">Base DN where start the search.</param>
        /// <param name="authType"></param>
        public LdapManager(ILdapUser adminUser,
            string ldapServer,
            string ldapSearchBaseDn,
            AuthType authType
            )
        {
            if (CheckLibraryParameters(new[] { ldapServer }))
            {
                _ldapServer = ldapServer;
                _authType = authType;
                if (adminUser != null &&
                    CheckLibraryParameters(new[] { adminUser.GetUserDn(), adminUser.GetUserCn(), adminUser.GetUserSn(), ldapSearchBaseDn }))
                {
                    _loginUser = adminUser;

                    _ldapSearchBaseDn = ldapSearchBaseDn;
                }
                StandardLdapInformation();

                _ldapErrors = new Dictionary<LdapState, LdapError>
                {
                    {
                        LdapState.LdapChangeUserPasswordError,
                        () => "LDAP CHANGE USER PASSWORD ERROR: " + _manageLdapUser.GetLdapUserManipulationMessage()
                    },
                    {LdapState.LdapConnectionError, () => "LDAP CONNECTION ERROR: " + _ldapConnectionErrorDescription},
                    {LdapState.LdapConnectionSuccess, () => "LDAP CONNECTION SUCCESS"},
                    {
                        LdapState.LdapCreateUserError,
                        () => "LDAP CREATE USER ERROR: " + _manageLdapUser.GetLdapUserManipulationMessage()
                    },
                    {
                        LdapState.LdapDeleteUserError,
                        () => "LDAP DELETE USER ERROR: " + _manageLdapUser.GetLdapUserManipulationMessage()
                    },
                    {LdapState.LdapGenericError, () => "LDAP GENERIC ERROR"},
                    {
                        LdapState.LdapLibraryInitError,
                        () => "LDAP LIBRARY INIT ERROR: " + _ldapInitLibraryErrorDescription
                    },
                    {LdapState.LdapLibraryInitSuccess, () => "LDAP LIBRARY INIT SUCCESS"},
                    {
                        LdapState.LdapModifyUserAttributeError,
                        () => "LDAP MODIFY USER ATTRIBUTE ERROR: " + _manageLdapUser.GetLdapUserManipulationMessage()
                    },
                    {
                        LdapState.LdapSearchUserError,
                        () => "LDAP SEARCH USER ERROR: " + _manageLdapUser.GetLdapUserManipulationMessage()
                    },
                    {
                        LdapState.LdapUserManipulatorSuccess,
                        () => "LDAP USER MANIPULATION SUCCESS: " + _manageLdapUser.GetLdapUserManipulationMessage()
                    }
                };

                _ldapCurrentState = LdapState.LdapLibraryInitSuccess;
                WriteLog(GetLdapMessage());
            }
            else
            {
                _ldapInitLibraryErrorDescription =
                    "Standard Init LDAPLibrary - One or more standard required string parameter is null or empty. Check the config file.";
                _ldapCurrentState = LdapState.LdapLibraryInitError;
                WriteLog(GetLdapMessage());
                throw new Exception(_ldapInitLibraryErrorDescription);
            }
        }

        /// <summary>
        ///     More detailed contructor that user the default constructor and the addictionalLDAPInformation method
        /// </summary>
        public LdapManager(ILdapUser adminUser,
            string ldapServer,
            string ldapSearchBaseDn,
            AuthType authType,
            bool secureSocketLayerFlag,
            bool transportSocketLayerFlag,
            bool clientCertificateFlag,
            string clientCertificatePath,
            bool writeLogFlag,
            string logPath,
            string userObjectClass,
            string matchFieldUsername
            )
            : this(adminUser,
                ldapServer,
                ldapSearchBaseDn,
                authType)
        {
            if (CheckLibraryParameters(new[] { clientCertificatePath, logPath, userObjectClass, matchFieldUsername }))
            {
                AddictionalLdapInformation(secureSocketLayerFlag,
                    transportSocketLayerFlag,
                    clientCertificateFlag,
                    clientCertificatePath,
                    writeLogFlag,
                    logPath,
                    userObjectClass,
                    matchFieldUsername
                    );

                _ldapCurrentState = LdapState.LdapLibraryInitSuccess;
                WriteLog(GetLdapMessage());
            }
            else
            {
                _ldapInitLibraryErrorDescription =
                    "Complete Init LDAPLibrary - One or more Addictional string parameter is null or empty. Check the config file.";
                _ldapCurrentState = LdapState.LdapLibraryInitError;
                WriteLog(GetLdapMessage());
                throw new Exception(_ldapInitLibraryErrorDescription);
            }
        }

        #region Methods from LDAPUserManipulator Class

        /// <summary>
        ///     Create a new LDAP User
        /// </summary>
        /// <param name="newUser"> The LDAPUser object that contain all the details of the new user to create</param>
        /// <returns>Boolean that comunicate the result of creation</returns>
        public bool CreateUser(ILdapUser newUser)
        {
            bool operationResult = _manageLdapUser.CreateUser(newUser, out _ldapCurrentState, _userObjectClass);
            WriteLog(GetLdapMessage());
            return operationResult;
        }

        /// <summary>
        ///     delete the specified  LdapUser
        /// </summary>
        /// <param name="user">LDAPUser to delete</param>
        /// <returns>the result of operation</returns>
        public bool DeleteUser(ILdapUser user)
        {
            bool operationResult = _manageLdapUser.DeleteUser(user, out _ldapCurrentState);
            WriteLog(GetLdapMessage());
            return operationResult;
        }

        /// <summary>
        ///     Modify an LDAPUser Attribute
        /// </summary>
        /// <param name="operationType">Choose the operation to do, it's an enum</param>
        /// <param name="user">The User to Modify the attribute</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="attributeValue">Value of the attribute</param>
        /// <returns></returns>
        public bool ModifyUserAttribute(DirectoryAttributeOperation operationType, ILdapUser user, string attributeName,
            string attributeValue)
        {
            bool operationResult = _manageLdapUser.ModifyUserAttribute(operationType, user, attributeName,
                attributeValue, out _ldapCurrentState);
            WriteLog(GetLdapMessage());
            return operationResult;
        }

        /// <summary>
        ///     Change the user Password
        /// </summary>
        /// <param name="user">LDAPUser to change the password</param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public bool ChangeUserPassword(ILdapUser user, string newPwd)
        {
            bool operationResult = _manageLdapUser.ChangeUserPassword(user, newPwd, out _ldapCurrentState);
            WriteLog(GetLdapMessage());
            return operationResult;
        }

        /// <summary>
        ///     Search Users in the LDAP system
        /// </summary>
        /// <param name="otherReturnedAttributes">Addictional attributes added to the results LDAPUsers objects</param>
        /// <param name="searchedUsers">Credential for the search</param>
        /// <param name="searchResult">LDAPUsers object returned in the search</param>
        /// <returns>Boolean that comunicate the result of search</returns>
        public bool SearchUsers(List<string> otherReturnedAttributes, string[] searchedUsers,
            out List<ILdapUser> searchResult)
        {
            bool operationResult = _manageLdapUser.SearchUsers(_ldapSearchBaseDn, _userObjectClass, _matchFieldUsername,
                otherReturnedAttributes, searchedUsers, out searchResult, out _ldapCurrentState);
            WriteLog(GetLdapMessage());
            return operationResult;
        }

        #endregion

        /// <summary>
        ///     Return the Error Message of an occurred LDAP Exception
        /// </summary>
        /// <returns>Message</returns>
        public string GetLdapMessage()
        {
            return _ldapErrors[_ldapCurrentState]();
        }

        /// <summary>
        ///     Instance the Ldap connection with admin config credential
        /// </summary>
        /// <returns>Success or Failed</returns>
        public bool Connect()
        {
            try
            {
                if (_loginUser != null)
                {
                    bool temp =
                        Connect(
                            new NetworkCredential(_loginUser.GetUserDn(), _loginUser.GetUserAttribute("userPassword")[0]),
                            _secureSocketLayerConnection,
                            _transportSocketLayerConnection,
                            _clientCertificate);
                    return temp;
                }
                return false;
            }
            catch (Exception)
            {
                _ldapConnectionErrorDescription =
                    "LDAP CONNECTION WITH ADMIN WS-CONFIG CREDENTIAL DENIED: unable to connect with administrator credential, see the config file";
                _ldapCurrentState = LdapState.LdapConnectionError;
                WriteLog(GetLdapMessage());
                throw new Exception(_ldapConnectionErrorDescription);
            }
        }

        /// <summary>
        ///     Connect to LDAP with the specified credential
        /// </summary>
        /// <param name="credential">user Credential</param>
        /// <param name="secureSocketLayer">Flag that specify if we want to use SSL for connection.</param>
        /// <param name="transportSocketLayer"></param>
        /// <param name="clientCertificate"></param>
        /// <returns>Success or Failed</returns>
        public bool Connect(NetworkCredential credential, bool secureSocketLayer, bool transportSocketLayer,
            bool clientCertificate)
        {
            try
            {
                _ldapConnection = new LdapConnection(_ldapServer) { AuthType = _authType };
                _ldapConnection.SessionOptions.ProtocolVersion = 3;

                #region secure Layer Options

                if (secureSocketLayer)
                    _ldapConnection.SessionOptions.SecureSocketLayer = true;

                if (transportSocketLayer)
                {
                    LdapSessionOptions options = _ldapConnection.SessionOptions;
                    options.StartTransportLayerSecurity(null);
                }

                if (clientCertificate)
                {
                    var clientCertificateFile = new X509Certificate();
                    clientCertificateFile.Import(_clientCertificatePath);
                    _ldapConnection.ClientCertificates.Add(clientCertificateFile);
                }

                #endregion

                _ldapConnection.Bind(credential);
                //ldapConnection.SendRequest(new SearchRequest(LDAPServer, "(objectClass=*)", SearchScope.Subtree, null));
                _manageLdapUser = new LdapUserManipulator(_ldapConnection, DefaultUserCn, DefaultUserSn);
            }
            catch (Exception e)
            {
                _ldapConnectionErrorDescription += String.Format("{0}\n User: {1}\n Pwd: {2}{3}{4}{5}",
                    e.Message,
                    credential.UserName,
                    credential.Password,
                    (secureSocketLayer ? "\n With SSL " : ""),
                    (transportSocketLayer ? "\n With TLS " : ""),
                    (clientCertificate ? "\n With Client Certificate" : ""));
                _ldapCurrentState = LdapState.LdapConnectionError;
                WriteLog(GetLdapMessage());
                return false;
            }
            _ldapConnectionErrorDescription += String.Format("Connection success\n User: {0}\n Pwd: {1}{2}{3}{4}",
                credential.UserName,
                credential.Password,
                (secureSocketLayer ? "\n With SSL " : ""),
                (transportSocketLayer ? "\n With TLS " : ""),
                (clientCertificate ? "\n With Client Certificate" : ""));
            if (_loginUser == null)
                _ldapConnection.Dispose();
            _ldapCurrentState = LdapState.LdapConnectionSuccess;
            WriteLog(GetLdapMessage());
            return true;
        }

        /// <summary>
        ///     Search the user and try to connect to LDAP
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="password">Password</param>
        /// <returns>
        ///     TRUE: connected
        ///     FALSE: not connected
        /// </returns>
        public bool SearchUserAndConnect(string user, string password)
        {
            List<ILdapUser> searchReturn;

            //Do the search and check the result 
            bool searchResult = SearchUsers(null, new[] { user }, out searchReturn);

            //if the previous search goes try to connect all the users
            return searchResult &&
                   searchReturn.Select(
                       searchedUser =>
                           Connect(new NetworkCredential(searchedUser.GetUserDn(), password),
                               _secureSocketLayerConnection, _transportSocketLayerConnection, _clientCertificate))
                       .Any(connectResult => connectResult);
        }

        /// <summary>
        ///     Private method used in detail constructor for specify all the addictional information
        /// </summary>
        /// <param name="secureSocketLayerFlag">Flag to specify if use SSL protocol</param>
        /// <param name="transportSocketLayerFlag">Flag to specify if use TSL protocol</param>
        /// <param name="clientCertificateFlag">Flag to specify the Client Certificate</param>
        /// <param name="clientCertificatePath">Path of the Client Certificate</param>
        /// <param name="writeLogFlag">Flag to specify if write the log</param>
        /// <param name="logPath">Path of the log file</param>
        /// <param name="userObjectClass">Attribute that rappresent the user in LDAP</param>
        /// <param name="matchFieldUsername">Attribute to match in user search</param>
        private void AddictionalLdapInformation(
            bool secureSocketLayerFlag,
            bool transportSocketLayerFlag,
            bool clientCertificateFlag,
            string clientCertificatePath,
            bool writeLogFlag,
            string logPath,
            string userObjectClass,
            string matchFieldUsername
            )
        {
            _secureSocketLayerConnection = secureSocketLayerFlag;
            _transportSocketLayerConnection = transportSocketLayerFlag;
            _clientCertificate = clientCertificateFlag;
            _clientCertificatePath = clientCertificatePath;
            _writeLogFlag = writeLogFlag;
            _logPath = logPath;
            _userObjectClass = userObjectClass;
            _matchFieldUsername = matchFieldUsername;
        }

        /// <summary>
        ///     Set LDAP Information To standard values.
        /// </summary>
        private void StandardLdapInformation()
        {
            //Default class variables information
            _secureSocketLayerConnection = false;
            _transportSocketLayerConnection = false;
            _clientCertificate = false;
            _clientCertificatePath = "";
            _writeLogFlag = false;
            _logPath = "";
            _userObjectClass = "person";
            _matchFieldUsername = "cn";
        }

        /// <summary>
        ///     Write to log the message incoming
        /// </summary>
        /// <param name="messageToLog">Message to Log</param>
        protected void WriteLog(string messageToLog)
        {
            if (_writeLogFlag && !String.IsNullOrEmpty(_logPath))
            {
                using (var logWriter = new StreamWriter(_logPath + "LDAPLog.txt", true))
                {
                    logWriter.WriteLine("{0:dd/MM/yyyy HH:mm:ss tt} - {1}", DateTime.Now, messageToLog);
                    logWriter.Close();
                }
            }
        }

        /// <summary>
        ///     Check all the string parameters
        /// </summary>
        /// <returns>true if all is set, false otherwise</returns>
        public static bool CheckLibraryParameters(string[] parameters)
        {
            return parameters.All(s => !CheckNullParameter(s));
        }

        private static bool CheckNullParameter(string parameter)
        {
            return string.IsNullOrEmpty(parameter);
        }
    }
}