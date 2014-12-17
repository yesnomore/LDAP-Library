using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using LDAPLibrary.Factories;
using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;
using LDAPLibrary.StaticClasses;

namespace LDAPLibrary
{
    public class LdapManager : ILdapManager
    {
        #region Class Variables

        private readonly ILdapConfigRepository _configRepository;
        private readonly ILdapConnector _connector;
        private readonly ILogger _logger;
        private readonly ILdapUserManipulator _manageLdapUser;

        private readonly ILdapModeChecker _modeChecker;
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
                _logger = LoggerFactory.GetLogger(_configRepository.GetWriteLogFlag(), null);
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
            LoggerType writeLogFlag,
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

        public bool CreateUser(ILdapUser newUser)
        {
            _ldapCurrentState = _manageLdapUser.CreateUser(newUser);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }

        public bool DeleteUser(ILdapUser user)
        {
            _ldapCurrentState = _manageLdapUser.DeleteUser(user);       
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }


        public bool ModifyUserAttribute(DirectoryAttributeOperation operationType, ILdapUser user, string attributeName,
            string attributeValue)
        {
            _ldapCurrentState = _manageLdapUser.ModifyUserAttribute(operationType, user, attributeName, attributeValue);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }

        public bool ChangeUserPassword(ILdapUser user, string newPwd)
        {
            _ldapCurrentState = _manageLdapUser.ChangeUserPassword(user, newPwd);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }


        public bool SearchUsers(List<string> otherReturnedAttributes, string[] searchedUsers,
            out List<ILdapUser> searchResult)
        {
            _ldapCurrentState = _manageLdapUser.SearchUsers(otherReturnedAttributes, searchedUsers, out searchResult);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }

        #endregion

        public string GetLdapMessage()
        {
            return _logger.BuildLogMessage("", _ldapCurrentState);
        }

        public bool Connect()
        {
            _ldapCurrentState = _connector.Connect();
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }

        public bool Connect(NetworkCredential credential, bool secureSocketLayer, bool transportSocketLayer,
            bool clientCertificate)
        {
            _ldapCurrentState = _connector.Connect(credential, secureSocketLayer, transportSocketLayer,
                clientCertificate);
            return LdapStateUtils.ToBoolean(_ldapCurrentState);
        }


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