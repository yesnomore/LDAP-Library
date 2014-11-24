using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace LDAPLibrary
{
    internal class LdapConnector : ILdapConnector
    {
        private const string AdminConnectionErrorMessage =
            "unable to connect with administrator credential, see the config file";

        private const string AdminConnectionErrorMessageBasicMode =
            "unable to connect with administrator in basic mode, see the config file";

        private readonly ILdapConfigRepository _configRepository;
        private readonly ILogger _logger;
        private readonly LdapModeChecker _modeChecker;
        private LdapConnection _ldapConnection;
        private List<ILdapConnectionObserver> observers; 

    public LdapConnector(LdapModeChecker modeChecker, ILdapConfigRepository configRepository, ILogger logger)
        {
            _modeChecker = modeChecker;
            _configRepository = configRepository;
            _logger = logger;
            observers = new List<ILdapConnectionObserver>();
        }

        public LdapState Connect()
        {
            try
            {
                if (_modeChecker.IsCompleteMode())
                {
                    return Connect(
                        new NetworkCredential(_configRepository.GetAdminUser().GetUserDn(),
                            _configRepository.GetAdminUser().GetUserAttribute("userPassword")[0]),
                        _configRepository.GetSecureSocketLayerFlag(),
                        _configRepository.GetTransportSocketLayerFlag(),
                        _configRepository.GetClientCertificateFlag());
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

                _ldapConnection.Bind(credential);
                observers.ForEach(x => x.SetLdapConnection(_ldapConnection));
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
            if (_modeChecker.IsBasicMode())
                _ldapConnection.Dispose();
            _logger.Write(_logger.BuildLogMessage(successConnectionMessage, LdapState.LdapConnectionSuccess));
            return LdapState.LdapConnectionSuccess;
        }

        public void LdapConnectionSubscribe(ILdapConnectionObserver observer)
        {
            observers.Add(observer);
        }

        public void LdapConnectionUnsubscribe(ILdapConnectionObserver observer)
        {
            observers.Remove(observer);
        }
    }
}