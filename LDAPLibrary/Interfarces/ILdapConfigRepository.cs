using System.DirectoryServices.Protocols;
using LDAPLibrary.Enums;
using LDAPLibrary.Logger;

namespace LDAPLibrary.Interfarces
{
    public interface ILdapConfigRepository
    {

        /// <summary>
        /// Check the validity of parameters
        /// Set the basic Ldap Configuration from parameters
        /// Set the others to the standard values
        /// </summary>
        /// <param name="adminUser">Admin User</param>
        /// <param name="adminMode">Library Admin Mode</param>
        /// <param name="server">Server URL</param>
        /// <param name="searchBaseDn">Root Search Node</param>
        /// <param name="authType"></param>
        /// <param name="loggerType"></param>
        /// <param name="logPath">Path of the log file</param>
        void BasicLdapConfig(ILdapUser adminUser, LDAPAdminMode adminMode, string server, string searchBaseDn, AuthType authType, LoggerType loggerType, string logPath);

        /// <summary>
        /// Check the validity of the parameters
        /// Set addiitonal configuration form the parameters
        /// </summary>
        /// <param name="secureSocketLayerFlag"></param>
        /// <param name="transportSocketLayerFlag"></param>
        /// <param name="clientCertificateFlag"></param>
        /// <param name="clientCertificatePath">Path of the certificate file</param>
        /// <param name="userObjectClass">Object class that rapresent an user</param>
        /// <param name="matchFieldUsername">Attribute that rapresent the username</param>
        void AdditionalLdapConfig(
            bool secureSocketLayerFlag, bool transportSocketLayerFlag, bool clientCertificateFlag,
            string clientCertificatePath,
            string userObjectClass, string matchFieldUsername);

        /*
         * Getters
         */

        ILdapUser GetAdminUser();
        string GetServer();
        string GetSearchBaseDn();
        AuthType GetAuthType();
        bool GetSecureSocketLayerFlag();
        bool GetTransportSocketLayerFlag();
        bool GetClientCertificateFlag();
        string GetClientCertificatePath();
        LoggerType GetWriteLogFlag();
        string GetLogPath();
        string GetUserObjectClass();
        string GetMatchFieldName();
        LDAPAdminMode GetAdminMode();
    }
}