using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using LDAPLibrary.Factories;
using LDAPLibrary.Interfarces;
using LDAPLibrary.StaticClasses;

namespace LDAPLibrary
{
    public class LdapManager : ILdapManager
    {
        #region Class Variables

        private readonly ILdapConfigRepository _configRepository;
        private readonly ILdapConnector _connector;
        private readonly ILogger _logger;
        private readonly LdapUserManipulator _manageLdapUser;

        private readonly LdapModeChecker _modeChecker;
        private LdapState _ldapCurrentState;

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
            _configRepository = LdapConfigRepositoryFactory.GetConfigRepository();
            try
            {
                _configRepository.BasicLdapConfig(adminUser, ldapServer, ldapSearchBaseDn, authType);
                _logger = LoggerFactory.GetLogger(false, null);
            }
            catch (ArgumentNullException)
            {
                _ldapCurrentState = LdapState.LdapLibraryInitError;
                throw;
            }

            _modeChecker = new LdapModeChecker(_configRepository);

            _connector = LdapConnectorFactory.GetLdapConnector(_modeChecker, _configRepository, _logger);
            _manageLdapUser = LdapUserManipulatorFactory.GetUserManipulator(_connector,_logger,_configRepository);
            _ldapCurrentState = LdapState.LdapLibraryInitSuccess;
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
            try
            {
                _logger = LoggerFactory.GetLogger(writeLogFlag, logPath);
                _configRepository.AdditionalLdapConfig(secureSocketLayerFlag, transportSocketLayerFlag,
                    clientCertificateFlag, clientCertificatePath, writeLogFlag, logPath, userObjectClass,
                    matchFieldUsername);
            }
            catch (ArgumentNullException e)
            {
                _ldapCurrentState = LdapState.LdapLibraryInitError;
                _logger.Write(_logger.BuildLogMessage(e.Message, _ldapCurrentState));
                throw;
            }

            _connector = LdapConnectorFactory.GetLdapConnector(_modeChecker, _configRepository, _logger);
            _manageLdapUser = LdapUserManipulatorFactory.GetUserManipulator(_connector,_logger,_configRepository);
            _ldapCurrentState = LdapState.LdapLibraryInitSuccess;
            _logger.Write(_logger.BuildLogMessage("", _ldapCurrentState));
        }

        #region Methods from LDAPUserManipulator Class

        /// <summary>
        ///     Create a new LDAP User
        /// </summary>
        /// <param name="newUser"> The LDAPUser object that contain all the details of the new user to create</param>
        /// <returns>Boolean that comunicate the result of creation</returns>
        public bool CreateUser(ILdapUser newUser)
        {
            _ldapCurrentState = _manageLdapUser.CreateUser(newUser);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }

        /// <summary>
        ///     delete the specified  LdapUser
        /// </summary>
        /// <param name="user">LDAPUser to delete</param>
        /// <returns>the result of operation</returns>
        public bool DeleteUser(ILdapUser user)
        {
            _ldapCurrentState = _manageLdapUser.DeleteUser(user);       
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
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
            _ldapCurrentState = _manageLdapUser.ModifyUserAttribute(operationType, user, attributeName, attributeValue);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }

        /// <summary>
        ///     Change the user Password
        /// </summary>
        /// <param name="user">LDAPUser to change the password</param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public bool ChangeUserPassword(ILdapUser user, string newPwd)
        {
            _ldapCurrentState = _manageLdapUser.ChangeUserPassword(user, newPwd);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
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
            _ldapCurrentState = _manageLdapUser.SearchUsers(otherReturnedAttributes, searchedUsers, out searchResult);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }

        #endregion

        /// <summary>
        ///     Return the Error Message of an occurred LDAP Exception
        /// </summary>
        /// <returns>Message</returns>
        public string GetLdapMessage()
        {
            return _logger.BuildLogMessage("", _ldapCurrentState);
        }

        /// <summary>
        ///     Instance the Ldap connection with admin config credential
        /// </summary>
        /// <returns>Success or Failed</returns>
        public bool Connect()
        {
            _ldapCurrentState = _connector.Connect();
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
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
            _ldapCurrentState = _connector.Connect(credential, secureSocketLayer, transportSocketLayer,
                clientCertificate);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
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
            bool searchResult = SearchUsers(null, new[] {user}, out searchReturn);

            //if the previous search goes try to connect all the users
            return searchResult &&
                   searchReturn.Select(
                       searchedUser =>
                           Connect(new NetworkCredential(searchedUser.GetUserDn(), password),
                               _configRepository.GetSecureSocketLayerFlag(),
                               _configRepository.GetTransportSocketLayerFlag(),
                               _configRepository.GetClientCertificateFlag()))
                       .Any(connectResult => connectResult);
        }
    }
}