using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;

namespace LDAPLibrary.Factories
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger(bool enableLogger, string logPath)
        {
            if (enableLogger) return new FileLogger(logPath);
            return new FakeLogger();
        }
    }
}