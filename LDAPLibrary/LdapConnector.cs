using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;

namespace LDAPLibrary
{
    /// <summary>
    /// Create an Ldap connection for the admin and for the direct connect of the users.
    /// Implement the pattern observer for passing the admin connection to the operational objects
    /// Can throw exceptions or return a specific message and state if used in wrong way.
    /// </summary>
    internal class LdapConnector : ILdapConnector
    {
        private const string AdminConnectionErrorMessage =
            "unable to connect with administrator credential, see the config file";

        private const string AdminConnectionErrorMessageBasicMode =
            "unable to connect with administrator in basic mode, see the config file";

        private ILdapConfigRepository _configRepository;
        private ILogger _logger;
        private ILdapAdminModeChecker _adminModeChecker;
        private List<ILdapConnectionObserver> _observers;
        private LdapConnection _ldapConnection;

        public LdapConnector(ILdapAdminModeChecker adminModeChecker, ILdapConfigRepository configRepository, ILogger logger)
        {
            _adminModeChecker = adminModeChecker;
            _configRepository = configRepository;
            _logger = logger;
            _observers = new List<ILdapConnectionObserver>();
        }

        public LdapState Connect()
        {
            try
            {
                if (!_adminModeChecker.IsNoAdminMode())
                {
                    LdapState returnState = Connect(
                        new NetworkCredential(_configRepository.GetAdminUser().GetUserDn(),
                            _configRepository.GetAdminUser().GetUserAttribute("userPassword")[0]),
                        _configRepository.GetSecureSocketLayerFlag(),
                        _configRepository.GetTransportSocketLayerFlag(),
                        _configRepository.GetClientCertificateFlag());
                    _observers.ForEach(x => x.SetLdapConnection(_ldapConnection));
                    return returnState;
                }
                _logger.Write(_logger.BuildLogMessage(AdminConnectionErrorMessageBasicMode,
                    LdapState.LdapConnectionError));
                return LdapState.LdapConnectionError;
            }
            catch (Exception)
            {
                _logger.Write(_logger.BuildLogMessage(AdminConnectionErrorMessage, LdapState.LdapConnectionError));
                throw new Exception(AdminConnectionErrorMessage);
            }
        }

        public LdapState Connect(NetworkCredential credential, bool secureSocketLayer, bool transportSocketLayer,
            bool clientCertificate)
        {
            try
            {
                _ldapConnection = new LdapConnection(_configRepository.GetServer())
                {
                    AuthType = _configRepository.GetAuthType()
                };
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
                    clientCertificateFile.Import(_configRepository.GetClientCertificatePath());
                    _ldapConnection.ClientCertificates.Add(clientCertificateFile);
                }

                #endregion

                if (_adminModeChecker.IsAdminMode()) _ldapConnection.Bind(credential);
                if (_adminModeChecker.IsAnonymousMode()) _ldapConnection.Bind(credential);
            }
            catch (Exception e)
            {
                string errorConnectionMessage = String.Format("{0}\n User: {1}\n Pwd: {2}{3}{4}{5}",
                    e.Message,
                    credential.UserName,
                    credential.Password,
                    (secureSocketLayer ? "\n With SSL " : ""),
                    (transportSocketLayer ? "\n With TLS " : ""),
                    (clientCertificate ? "\n With Client Certificate" : ""));
                _logger.Write(_logger.BuildLogMessage(errorConnectionMessage, LdapState.LdapConnectionError));
                return LdapState.LdapConnectionError;
            }
            string successConnectionMessage = String.Format("Connection success\n User: {0}\n Pwd: {1}{2}{3}{4}",
                credential.UserName,
                credential.Password,
                (secureSocketLayer ? "\n With SSL " : ""),
                (transportSocketLayer ? "\n With TLS " : ""),
                (clientCertificate ? "\n With Client Certificate" : ""));
            if (_adminModeChecker.IsNoAdminMode())
                _ldapConnection.Dispose();
            _logger.Write(_logger.BuildLogMessage(successConnectionMessage, LdapState.LdapConnectionSuccess));
            return LdapState.LdapConnectionSuccess;
        }

        public void LdapConnectionSubscribe(ILdapConnectionObserver observer)
        {
            _observers.Add(observer);
        }

        public void LdapConnectionUnsubscribe(ILdapConnectionObserver observer)
        {
            _observers.Remove(observer);
        }

        /// <summary>
        /// Purge the connection and the object reference in the class
        /// </summary>
        public void Dispose()
        {
            _configRepository = null;
            _logger = null;
            _adminModeChecker = null;
            _observers = null;
            _ldapConnection.Dispose();
        }
    }
}