using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace LDAPLibrary
{
    public class LDAPManager : ILDAPManager
    {
        #region Class Variables


        private LdapConnection ldapConnection;

        //Variables passed in the constructor MUST
        private string LDAPServer;			//Server LDAP with port
        private LDAPUser loginUser;			//LDAP User Admin
        private string LDAPSearchBaseDN;	//LDAP base search DN

        //Attioctional parameter class that have default values
        private string defaultUserSn = "Default Surname";//Sn used in library for the user than don't have that( almost always required in OpenLDAP)
        private string defaultUserCn = "Default CommonName";//Cn used in library for the user than don't have that(almost always required in OpenLDAP)
        private string logPath;							//Log file path
        private string UserObjectClass;					//The attribute ObjectClass used to indentify an user
        private string MatchFieldUsername;				//Field used in search filter to know what is the LDAP attribute to match with username
        private bool writeLogFlag;						//Flag that specify if enable the log
        private bool secureSocketLayerConnection;		//Flag that specify if enable SSL protocol
        private bool transportSocketLayerConnection;	//Flag that specify if enable TSL protocol
        private bool clientCertificate;					//Flag that specify if enable Client Certificate
        private string clientCertificatePath;			//Client Certificate Path

        //Error Description from LDAP connection
        private LDAPState LDAPCurrentState;

        private string LDAPConnectionErrorDescription;

        private string LDAPInitLibraryErrorDescription;

        //External Class to delegate the jobs
        private LDAPUserManipulator ManageLDAPUser;


        #endregion

        /// <summary>
        /// LDAP library constructior where all the class variables are initialized
        /// The variables not specified in definition will be set at default values.
        /// </summary>
        /// <param name="adminUserDN">DN of admin user</param>
        /// <param name="adminUserCN">CN of admin user</param>
        /// <param name="adminUserSN">SN of admin user</param>
        /// <param name="LDAPServer">LDAP Server with port</param>
        /// <param name="LDAPSearchBaseDN">Base DN where start the search.</param>
        public LDAPManager(string adminUserDN,
                            string adminUserCN,
                            string adminUserSN,
                            Dictionary<string, string[]> adminUserAttributes,
                            string LDAPServer,
                            string LDAPSearchBaseDN
                           )
        {
            if (checkLibraryParameters(new string[] { LDAPServer }))
            {
                this.LDAPServer = LDAPServer;
                if (checkLibraryParameters(new string[] { adminUserDN, adminUserCN, adminUserSN, LDAPSearchBaseDN }))
                {
                    this.loginUser = new LDAPUser(adminUserDN, adminUserCN, adminUserSN, adminUserAttributes);

                    this.LDAPSearchBaseDN = LDAPSearchBaseDN;

                }
                standardLDAPInformation();

                LDAPCurrentState = LDAPState.LDAPLibraryInitSuccess;
                writeLog(getLDAPMessage());
            }
            else
            {
                LDAPInitLibraryErrorDescription = "Standard Init LDAPLibrary - One or more standard required string parameter is null or empty. Check the config file.";
                LDAPCurrentState = LDAPState.LDAPLibraryInitError;
                writeLog(getLDAPMessage());
                throw new Exception(LDAPInitLibraryErrorDescription);
            }
        }

        /// <summary>
        /// More detailed contructor that user the default constructor and the addictionalLDAPInformation method
        /// </summary>
        public LDAPManager(string adminUserDN,
                            string adminUserCN,
                            string adminUserSN,
                            Dictionary<string, string[]> adminUserAttributes,
                            string LDAPServer,
                            string LDAPSearchBaseDN,
                            bool secureSocketLayerFlag,
                            bool transportSocketLayerFlag,
                            bool clientCertificateFlag,
                            string clientCertificatePath,
                            bool writeLogFlag,
                            string logPath,
                            string UserObjectClass,
                            string MatchFieldUsername
            )
            : this(adminUserDN,
                    adminUserCN,
                    adminUserSN,
                    adminUserAttributes,
                    LDAPServer,
                    LDAPSearchBaseDN)
        {
            if (checkLibraryParameters(new string[] { clientCertificatePath, logPath, UserObjectClass, MatchFieldUsername }))
            {
                addictionalLDAPInformation(secureSocketLayerFlag,
                                            transportSocketLayerFlag,
                                            clientCertificateFlag,
                                            clientCertificatePath,
                                            writeLogFlag,
                                            logPath,
                                            UserObjectClass,
                                            MatchFieldUsername
                );

                LDAPCurrentState = LDAPState.LDAPLibraryInitSuccess;
                writeLog(getLDAPMessage());
            }
            else
            {
                LDAPInitLibraryErrorDescription = "Complete Init LDAPLibrary - One or more Addictional string parameter is null or empty. Check the config file.";
                LDAPCurrentState = LDAPState.LDAPLibraryInitError;
                writeLog(getLDAPMessage());
                throw new Exception(LDAPInitLibraryErrorDescription);
            }
        }

        #region Methods from LDAPUserManipulator Class

        /// <summary>
        /// Create a new LDAP User
        /// </summary>
        /// <param name="newUser"> The LDAPUser object that contain all the details of the new user to create</param>
        /// <returns>Boolean that comunicate the result of creation</returns>
        public bool createUser(LDAPUser newUser)
        {
            bool operationResult = ManageLDAPUser.createUser(newUser, out LDAPCurrentState, UserObjectClass);
            writeLog(getLDAPMessage());
            return operationResult;
        }
        /// <summary>
        /// delete the specified  LDAPUser
        /// </summary>
        /// <param name="user">LDAPUser to delete</param>
        /// <returns>the result of operation</returns>
        public bool deleteUser(LDAPUser user)
        {
            bool operationResult = ManageLDAPUser.deleteUser(user, out LDAPCurrentState);
            writeLog(getLDAPMessage());
            return operationResult;
        }

        /// <summary>
        /// Modify an LDAPUser Attribute
        /// </summary>
        /// <param name="operationType">Choose the operation to do, it's an enum</param>
        /// <param name="user">The User to Modify the attribute</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="attributeValue">Value of the attribute</param>
        /// <returns></returns>
        public bool modifyUserAttribute(DirectoryAttributeOperation operationType, LDAPUser user, string attributeName, string attributeValue)
        {
            bool operationResult = ManageLDAPUser.modifyUserAttribute(operationType, user, attributeName, attributeValue, out LDAPCurrentState);
            writeLog(getLDAPMessage());
            return operationResult;
        }

        /// <summary>
        /// Change the user Password
        /// </summary>
        /// <param name="user">LDAPUser to change the password</param>
        /// <returns></returns>
        public bool changeUserPassword(LDAPUser user, string newPwd)
        {
            bool operationResult = ManageLDAPUser.changeUserPassword(user, newPwd, out LDAPCurrentState);
            writeLog(getLDAPMessage());
            return operationResult;
        }

        /// <summary>
        /// Search Users in the LDAP system
        /// </summary>
        /// <param name="otherReturnedAttributes">Addictional attributes added to the results LDAPUsers objects</param>
        /// <param name="searchedUsers">Credential for the search</param>
        /// <param name="searchResult">LDAPUsers object returned in the search</param>
        /// <returns>Boolean that comunicate the result of search</returns>
        public bool searchUsers(List<string> otherReturnedAttributes, string[] searchedUsers, out List<LDAPUser> searchResult)
        {
            bool operationResult = ManageLDAPUser.searchUsers(LDAPSearchBaseDN, UserObjectClass, MatchFieldUsername, otherReturnedAttributes, searchedUsers, out searchResult, out LDAPCurrentState);
            writeLog(getLDAPMessage());
            return operationResult;
        }

        #endregion

        /// <summary>
        /// Private method used in detail constructor for specify all the addictional information
        /// </summary>
        /// <param name="adminUserAttributes">Attributes for Admin user</param>
        /// <param name="domain">Domain of the server LDAP</param>
        /// <param name="secureSocketLayerFlag">Flag to specify if use SSL protocol</param>
        /// <param name="transportSocketLayerFlag">Flag to specify if use TSL protocol</param>
        /// <param name="clientCertificateFlag">Flag to specify the Client Certificate</param>
        /// <param name="clientCertificatePath">Path of the Client Certificate</param>
        /// <param name="writeLogFlag">Flag to specify if write the log</param>
        /// <param name="logPath">Path of the log file</param>
        /// <param name="UserObjectClass">Attribute that rappresent the user in LDAP</param>
        /// <param name="MatchFieldUsername">Attribute to match in user search</param>
        /// <param name="defaultUserSn">Default Surname for new users.</param>
        private void addictionalLDAPInformation(
                bool secureSocketLayerFlag,
                bool transportSocketLayerFlag,
                bool clientCertificateFlag,
                string clientCertificatePath,
                bool writeLogFlag,
                string logPath,
                string UserObjectClass,
                string MatchFieldUsername
            )
        {

            this.secureSocketLayerConnection = secureSocketLayerFlag;
            this.transportSocketLayerConnection = transportSocketLayerFlag;
            this.clientCertificate = clientCertificateFlag;
            this.clientCertificatePath = clientCertificatePath;
            this.writeLogFlag = writeLogFlag;
            this.logPath = logPath;
            this.UserObjectClass = UserObjectClass;
            this.MatchFieldUsername = MatchFieldUsername;
        }

        /// <summary>
        /// Set LDAP Information To standard values.
        /// </summary>
        private void standardLDAPInformation()
        {

            //Default class variables information
            this.secureSocketLayerConnection = false;
            this.transportSocketLayerConnection = false;
            this.clientCertificate = false;
            this.clientCertificatePath = "";
            this.writeLogFlag = false;
            this.logPath = "";
            this.UserObjectClass = "person";
            this.MatchFieldUsername = "cn";

        }

        /// <summary>
        /// Return the Error Message of an occurred LDAP Exception
        /// </summary>
        /// <returns>Message</returns>
        public string getLDAPMessage()
        {
            switch (LDAPCurrentState)
            {
                case LDAPState.LDAPConnectionError: return "LDAP CONNECTION ERROR: " + LDAPConnectionErrorDescription;
                case LDAPState.LDAPChangeUserPasswordError: return "LDAP CHANGE USER ERROR: " + ManageLDAPUser.getLDAPUserManipulationMessage();
                case LDAPState.LDAPCreateUserError: return "LDAP CREATE USER ERROR: " + ManageLDAPUser.getLDAPUserManipulationMessage();
                case LDAPState.LDAPDeleteUserError: return "LDAP DELETE USER ERROR: " + ManageLDAPUser.getLDAPUserManipulationMessage();
                case LDAPState.LDAPModifyUserAttributeError: return "LDAP MODIFY ATTRIBUTE USER ERROR: " + ManageLDAPUser.getLDAPUserManipulationMessage();
                case LDAPState.LDAPSearchUserError: return "LDAP SEARCH USER ERROR: " + ManageLDAPUser.getLDAPUserManipulationMessage();
                case LDAPState.LDAPLibraryInitError: return "LDAP LIBRARY INIT ERROR: " + LDAPInitLibraryErrorDescription;
                case LDAPState.LDAPGenericError: return "LDAP GENERIC ERROR";
                case LDAPState.LDAPConnectionSuccess: return "LDAP CONNECTION SUCCESS";
                case LDAPState.LDAPUserManipulatorSuccess: return "LDAP USER MANIPULATION SUCCESS: " + ManageLDAPUser.getLDAPUserManipulationMessage();
                case LDAPState.LDAPLibraryInitSuccess: return "LDAP LIBRARY INIT SUCCESS";
                default: return "NO LDAP MESSAGE!";
            }

        }

        /// <summary>
        /// Instance the Ldap connection with admin config credential
        /// </summary>
        /// <returns>Success or Failed</returns>
        public bool connect()
        {
            try
            {
                if (loginUser == null)
                {
                    bool temp = connect(new NetworkCredential(loginUser.getUserDn(), loginUser.getUserAttribute("userPassword")[0]),
                                                              secureSocketLayerConnection,
                                                              transportSocketLayerConnection,
                                                              this.clientCertificate);
                    return temp;
                }
                return false;
            }
            catch (Exception e)
            {
                LDAPConnectionErrorDescription = "LDAP CONNECTION WITH ADMIN WS-CONFIG CREDENTIAL DENIED: unable to connect with administrator credential, see the config file";
                LDAPCurrentState = LDAPState.LDAPConnectionError;
                writeLog(getLDAPMessage());
                throw new Exception(LDAPConnectionErrorDescription);
            }
        }

        /// <summary>
        /// Connect to LDAP with the specified credential
        /// </summary>
        /// <param name="credential">user Credential</param>
        /// <param name="secureSocketLayer">Flag that specify if we want to use SSL for connection.</param>
        /// <returns>Success or Failed</returns>
        public bool connect(NetworkCredential credential, bool secureSocketLayer, bool transportSocketLayer, bool clientCertificate)
        {
            try
            {
                ldapConnection = new LdapConnection(LDAPServer) { Credential = credential, AuthType = AuthType.Basic };
                ldapConnection.SessionOptions.ProtocolVersion = 3;

                #region secure Layer Options

                if (secureSocketLayer)
                    ldapConnection.SessionOptions.SecureSocketLayer = secureSocketLayer;

                if (transportSocketLayer)
                {
                    LdapSessionOptions options = ldapConnection.SessionOptions;
                    options.StartTransportLayerSecurity(null);
                }

                if (clientCertificate)
                {
                    X509Certificate ClientCertificateFile = new X509Certificate();
                    ClientCertificateFile.Import(clientCertificatePath);
                    ldapConnection.ClientCertificates.Add(ClientCertificateFile);
                }

                #endregion

                ldapConnection.Bind();
                ManageLDAPUser = new LDAPUserManipulator(ldapConnection, defaultUserCn, defaultUserSn);
            }
            catch (Exception e)
            {
                LDAPConnectionErrorDescription += e.Message +
                                                    "\n User: " + credential.UserName +
                                                    "\n Pwd: " + credential.Password +
                                                    (secureSocketLayer ? "\n With SSL " : "") +
                                                    (transportSocketLayer ? "\n With TLS " : "") +
                                                    (clientCertificate ? "\n With Client Certificate" : "");
                LDAPCurrentState = LDAPState.LDAPConnectionError;
                writeLog(getLDAPMessage());
                return false;
            }
            LDAPConnectionErrorDescription += "Connection success" +
                                                    "\n User: " + credential.UserName +
                                                    "\n Pwd: " + credential.Password +
                                                    (secureSocketLayer ? "\n With SSL " : "") +
                                                    (transportSocketLayer ? "\n With TLS " : "") +
                                                    (clientCertificate ? "\n With Client Certificate" : "");
            if (loginUser == null)
                ldapConnection.Dispose();
            LDAPCurrentState = LDAPState.LDAPConnectionSuccess;
            writeLog(getLDAPMessage());
            return true;
        }

        /// <summary>
        /// Write to log the message incoming
        /// </summary>
        /// <param name="messageToLog">Message to Log</param>
        protected void writeLog(string messageToLog)
        {
            if (writeLogFlag == true && !String.IsNullOrEmpty(logPath))
            {
                StreamWriter logWriter = new StreamWriter(logPath + "LDAPLog.txt", true);
                logWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt") + " - " + messageToLog);
                logWriter.Close();
            }
        }

        /// <summary>
        /// Search the user and try to connect to LDAP
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="password">Password</param>
        /// <returns>
        /// TRUE: connected
        /// FALSE: not connected
        /// </returns>
        public bool searchUserAndConnect(string user, string password)
        {
            string[] tempUser = new string[1];
            tempUser[0] = user;
            List<LDAPUser> searchReturn = new List<LDAPUser>();
            bool connectResult = false;

            //Do the search and check the result 
            bool searchResult = searchUsers(null, tempUser, out searchReturn);
            if (searchResult == false)
                return false;

            //try to connect for all the results
            foreach (LDAPUser searchedUser in searchReturn)
            {
                connectResult = connect(new NetworkCredential(searchedUser.getUserDn(), password),
                                        secureSocketLayerConnection,
                                        transportSocketLayerConnection,
                                        this.clientCertificate);
                if (connectResult == true)
                    return true;
            }
            return connectResult;
        }

        /// <summary>
        /// Check all the string parameters
        /// </summary>
        /// <returns>true if all is set, false otherwise</returns>
        public static bool checkLibraryParameters(string[] parameters)
        {
            foreach (string s in parameters)
                if (checkNullParameter(s))
                    return false;
            return true;
        }

        private static bool checkNullParameter(string parameter)
        {
            return string.IsNullOrEmpty(parameter);
        }
    }
}
