using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Net;

namespace LDAPLibrary
{
    public class LDAPManager : ILDAPManager
    {
        #region Class Variables

        
        private LdapConnection ldapConnection;

        //Variables passed in the constructor
        private string LDAPServer;
        private LDAPUser loginUser;
        private string defaultUserSn;
        private string domain;
        private string logPath;
        private string UserObjectClass;
        private string MatchFieldUsername;
        private bool writeLogFlag;
        private string LDAPSearchBaseDN;
        
        //Error Description from LDAP connection
        private LDAPState LDAPCurrentState;
        private string LDAPConnectionErrorDescription;

        //External Class to delegate the jobs
        private LDAPUserManipulator ManageLDAPUser;

        
        #endregion

        /// <summary>
        /// Ldap Manager Contructor with all the configuration needed
        /// </summary>
        /// <param name="adminUserDN">User Admin DN</param>
        /// <param name="adminUserCN">User Admin CN</param>
        /// <param name="adminUserSN">User Admin SN</param>
        /// <param name="adminUserAttributes">User Admin other attributes</param>
        /// <param name="LDAPServer">Name of the server LDAP</param>
        /// <param name="domain">Domain of the server LDAP</param>
        /// <param name="baseDN">Base DN for the Admin User</param>
        /// <param name="writeLogFlag">Flag that secified if write the log file</param>
        /// <param name="logPath">Path of the log file</param>
        /// <param name="UserObjectClass">Class in LDAP user to identify the User</param>
        /// <param name="MatchFieldUsername">Field user to identify the username</param>
        /// <param name="LDAPSearchBaseDN">Base DN where start the search</param>
        /// <param name="defaultUserSn">Default value of user Sn</param>
        public LDAPManager(string adminUserDN, 
                           string adminUserCN,
                           string adminUserSN,
                           Dictionary<string,string[]> adminUserAttributes,
                           string LDAPServer,
                           string domain,
                           bool writeLogFlag,
                           string logPath,
                           string UserObjectClass,
                           string MatchFieldUsername,
                           string LDAPSearchBaseDN,
                           string defaultUserSn)
        {

            this.loginUser = new LDAPUser(adminUserDN, adminUserCN, adminUserSN, adminUserAttributes);
            this.LDAPServer = LDAPServer;
            this.domain = domain;
            this.writeLogFlag = writeLogFlag;
            this.logPath = logPath;
            this.UserObjectClass = UserObjectClass;
            this.MatchFieldUsername = MatchFieldUsername;
            this.LDAPSearchBaseDN = LDAPSearchBaseDN;
            this.defaultUserSn = defaultUserSn;

            if (!connect())
            {
                writeLog("LDAP CONNECTION WITH ADMIN WS-CONFIG CREDENTIAL DENIED: unable to connect with administrator credential, see the config file");
                throw new Exception("LDAP CONNECTION WITH ADMIN WS-CONFIG CREDENTIAL DENIED: unable to connect with administrator credential, see the config file");
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
            bool operationResult =  ManageLDAPUser.createUser(newUser, out LDAPCurrentState);
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
        public bool searchUsers(List<string> otherReturnedAttributes, string [] searchedUsers, out List<LDAPUser> searchResult)
        {
            bool operationResult = ManageLDAPUser.searchUsers(LDAPSearchBaseDN, UserObjectClass, MatchFieldUsername, otherReturnedAttributes ,searchedUsers, out searchResult, out LDAPCurrentState);
            writeLog(getLDAPMessage());
            return operationResult;
        }

        #endregion


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
                case LDAPState.LDAPGenericError: return "LDAP GENERIC ERROR";
                case LDAPState.LDAPConnectionSuccess: return "LDAP CONNECTION SUCCESS";
                case LDAPState.LDAPUserManipulatorSuccess: return "LDAP USER MANIPULATION SUCCESS: " + ManageLDAPUser.getLDAPUserManipulationMessage();
                default: return "NO LDAP MESSAGE!";
            }

        }

        /// <summary>
        /// Instance the Ldap connection with admin config credential
        /// </summary>
        /// <returns>Success or Failed</returns>
        public bool connect()
        {
           bool temp = connect(new NetworkCredential(loginUser.getUserDn(), loginUser.getUserAttribute("userPassword")[0], domain));
           return temp;
        }

        /// <summary>
        /// Connect to LDAP with the specified credential
        /// </summary>
        /// <param name="credential">user Credential</param>
        /// <returns>Success or Failed</returns>
        public bool connect(NetworkCredential credential) 
        {
            try
            {
                ldapConnection = new LdapConnection(LDAPServer) { Credential = credential, AuthType = AuthType.Basic };
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                ldapConnection.Bind();
                ManageLDAPUser = new LDAPUserManipulator(ldapConnection, defaultUserSn);
            }
            catch (Exception e)
            {
                LDAPConnectionErrorDescription += e.Message + "User: " + credential.UserName + " Pwd: " + credential.Password;
                LDAPCurrentState = LDAPState.LDAPConnectionError;
                writeLog(getLDAPMessage());
                return false;
            }
            LDAPCurrentState = LDAPState.LDAPConnectionSuccess;
            writeLog(getLDAPMessage());
            return true;
        }

        /// <summary>
        /// Write to log the message incoming
        /// </summary>
        /// <param name="messageToLog">Message to Log</param>
        protected void writeLog( string messageToLog) 
        {
            if (writeLogFlag == true && (!logPath.Equals("") || logPath == null) ) 
            {
                StreamWriter logWriter = new StreamWriter(logPath + "LDAPLog.txt",true);
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
            string [] tempUser = new string[1];
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
                connectResult = connect(new NetworkCredential(searchedUser.getUserDn(), password, domain));
                if (connectResult == true)
                    return true;
            }
            return connectResult;
        }
    }
}
