using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Authentication;
using LDAPLibrary.Enums;
using LDAPLibrary.Factories;
using LDAPLibrary.Interfarces;
using LDAPLibrary.StaticClasses;

namespace LDAPLibrary.Connectors
{
    /// <summary>
    /// Create an Ldap connection for the admin and for the direct connect of the users.
    /// Implement the pattern observer for passing the admin connection to the operational objects
    /// Can throw exceptions or return a specific message and state if used in wrong way.
    /// </summary>
    internal abstract class ALdapConnector : ILdapConnector
    {
        private const string AdminConnectionErrorMessage =
            "unable to connect with administrator credential, see the config file";

        protected const string AdminConnectionErrorMessageBasicMode =
            "unable to connect with administrator in basic mode, see the config file";

        protected string SuccessConnectionMessage(NetworkCredential credential)
        {
            return String.Format("Connection success\n User: {0}\n Pwd: {1}{2}{3}{4}", credential.UserName,
                credential.Password,
                (_configRepository.GetSecureSocketLayerFlag() ? "\n With SSL " : ""),
                (_configRepository.GetTransportSocketLayerFlag() ? "\n With TLS " : ""),
                (_configRepository.GetClientCertificateFlag() ? "\n With Client Certificate" : ""));
        }

        protected string ErrorConnectionMessage(NetworkCredential credential, string exceptionMessage){ 
            return String.Format("{0}\n User: {1}\n Pwd: {2}{3}{4}{5}",
            exceptionMessage,
            credential.UserName,
            credential.Password,
            (_configRepository.GetSecureSocketLayerFlag() ? "\n With SSL " : ""),
            (_configRepository.GetTransportSocketLayerFlag() ? "\n With TLS " : ""),
            (_configRepository.GetClientCertificateFlag() ? "\n With Client Certificate" : ""));
        }

    protected ILdapConfigRepository _configRepository;
        protected ILogger _logger;
        protected List<ILdapConnectionObserver> _observers;
        protected LdapConnection _ldapConnection;

        protected ALdapConnector(ILdapConfigRepository configRepository, ILogger logger)
        {
            _configRepository = configRepository;
            _logger = logger;
            _observers = new List<ILdapConnectionObserver>();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~Abstract Methods~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        protected abstract LdapState ConnectAdmin();
        protected abstract void ConnectUser(NetworkCredential credential);

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        protected void StandardConnect(NetworkCredential credential)
        {
            if (LdapParameterChecker.ParametersIsNullOrEmpty(new []{credential.UserName})) throw new InvalidCredentialException("Username cannot be null or empty");
            if (LdapParameterChecker.ParametersIsNullOrEmpty(new []{credential.Password})) throw new InvalidCredentialException("Password cannot be null or empty");

            _ldapConnection = LdapConnectionFactory.GetLdapConnection(_configRepository);
            _ldapConnection.Bind(credential);
        }

        public LdapState Connect()
        {
            try
            {
                return ConnectAdmin();
            }
            catch (Exception)
            {
                _logger.Write(_logger.BuildLogMessage(AdminConnectionErrorMessage, LdapState.LdapConnectionError));
                throw new Exception(AdminConnectionErrorMessage);
            }
        }
        
        public LdapState Connect(NetworkCredential credential)
        {
            try
            {
                ConnectUser(credential);
            }
            catch (Exception e)
            {
                _logger.Write(_logger.BuildLogMessage(ErrorConnectionMessage(credential,e.Message), LdapState.LdapConnectionError));
                return LdapState.LdapConnectionError;
            }
            _logger.Write(_logger.BuildLogMessage(SuccessConnectionMessage(credential), LdapState.LdapConnectionSuccess));
            return LdapState.LdapConnectionSuccess;
        }

        //~~~~~~~~~~~~~~~~~~~~~~Observer Pattern~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public void LdapConnectionSubscribe(ILdapConnectionObserver observer)
        {
            _observers.Add(observer);
        }

        public void LdapConnectionUnsubscribe(ILdapConnectionObserver observer)
        {
            _observers.Remove(observer);
        }

        //~~~~~~~~~~~~~~~~~~~~~~Disposable~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        /// <summary>
        /// Purge the connection and the object reference in the class
        /// </summary>
        public void Dispose()
        {
            _configRepository = null;
            _logger = null;
            _observers = null;
            _ldapConnection.Dispose();
        }
    }
}