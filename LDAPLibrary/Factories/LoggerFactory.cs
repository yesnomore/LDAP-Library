using LDAPLibrary.Interfarces;
using LDAPLibrary.Logger;

namespace LDAPLibrary.Factories
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger(LoggerType type, string logPath)
        {
            switch (type)
            {
                case LoggerType.File: return new FileLogger(logPath);
                case LoggerType.EventViewer: return new EventViewerLogger();
                default: return new FakeLogger();
            }
        }
    }
}