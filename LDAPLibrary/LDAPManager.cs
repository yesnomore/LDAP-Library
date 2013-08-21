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
        private string domain;
        private string baseDN;
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

        public LDAPManager(string adminUserDN, 
                           string adminUserCN,
                           string adminUserSN,
                           Dictionary<string,string[]> adminUserAttributes,
                           string LDAPServer,
                           string domain,
                           string baseDN,
                           bool writeLog,
                           string logPath,
                           string UserObjectClass,
                           string MatchFieldUsername,
                           string LDAPSearchBaseDN)
        {

            this.loginUser = new LDAPUser(adminUserDN, adminUserCN, adminUserSN, adminUserAttributes);
            this.LDAPServer = LDAPServer;
            this.domain = domain;
            this.baseDN = baseDN;
            this.writeLogFlag = writeLog;
            this.logPath = logPath;
            this.UserObjectClass = UserObjectClass;
            this.MatchFieldUsername = MatchFieldUsername;
            this.LDAPSearchBaseDN = LDAPSearchBaseDN;

            if (!connect())
                throw new Exception("LDAP CONNECTION WITH ADMIN WS-CONFIG CREDENTIAL DENIED: view the log or call 'getLDAPMessage()' for information ");
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
        /// <param name="baseDN">Starting DN of the search</param>
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
        /// <returns></returns>
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
        /// Instance the Ldap connection
        /// </summary>
        /// <returns></returns>
        public bool connect()
        {
           bool temp = connect(new NetworkCredential(loginUser.getUserDn(), loginUser.getUserAttribute("userPassword")[0], domain));
           return temp;
        }
        public bool connect(NetworkCredential credential) 
        {
            try
            {
                ldapConnection = new LdapConnection(LDAPServer);
                ldapConnection.Credential = credential;
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                ldapConnection.Bind();
                ManageLDAPUser = new LDAPUserManipulator(ldapConnection);
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

        private void writeLog( string messageToLog) 
        {
            if (writeLogFlag == true && (!logPath.Equals("") || logPath == null) ) 
            {
                StreamWriter logWriter = new StreamWriter(logPath + "LDAPLog.txt",true);
                logWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt") + " - " + messageToLog);
                logWriter.Close();
            }
        }
        /// <summary>
        /// Method that wrap SearchUsers and Connect in one operation
        /// </summary>
        /// <param name="baseDN">The nodo where the search starts</param>
        /// <param name="user">The Username to search and Connect</param>
        /// <param name="password">The passwords of the User</param>
        /// <returns>the operation result</returns>
        public bool searchUserAndConnect(string user, string password) 
        {
            string [] tempUser = new string[1];
            tempUser[0] = user; 
            List<LDAPUser> searchReturn = new List<LDAPUser>();
            bool connectResult = false,searchResult;

            //Do the search and check the result 
            searchResult = searchUsers(null, tempUser, out searchReturn);
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
