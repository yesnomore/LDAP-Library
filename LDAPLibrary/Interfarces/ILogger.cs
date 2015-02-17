using LDAPLibrary.Enums;

namespace LDAPLibrary.Interfarces
{
    public interface ILogger
    {
        string BuildLogMessage(string message, LdapState state);

        void Write(string message);
    }
}