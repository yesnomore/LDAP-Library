using System.DirectoryServices.Protocols;

namespace LDAPLibrary
{
    public interface ILdapConfigRepository
    {

        //Configurations Patterns

        void BasicLdapConfig(ILdapUser adminUser, string server, string searchBaseDn, AuthType authType);

        void AdditionalLdapConfig(
            bool secureSocketLayerFlag, bool transportSocketLayerFlag, bool clientCertificateFlag, string clientCertificatePath,
            bool writeLogFlag, string logPath, string userObjectClass, string matchFieldUsername);

        // Getters

        ILdapUser GetAdminUser();
        string GetServer();
        string GetSearchBaseDn();
        AuthType GetAuthType();
        bool GetSecureSocketLayerFlag();
        bool GetTransportSocketLayerFlag();
        bool GetClientCertificateFlag();
        string GetClientCertificatePath();
        bool GetWriteLogFlag();
        string GetLogPath();
        string GetUserObjectClass();
        string GetMatchFieldName();



    }
}
