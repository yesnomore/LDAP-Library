namespace LDAPLibrary
{
    public interface ILogger
    {
        string BuildLogMessage(string message, LdapState state);

        void Write(string message);
    }
}