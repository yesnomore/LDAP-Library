using System.Diagnostics;

namespace LDAPLibrary.Logger
{
    internal class EventViewerLogger : ALogger
    {
        private const string EventLogSource = "LDAPLibrary";
        private readonly EventLog _eventlogger;

        public EventViewerLogger()
        {
            _eventlogger = new EventLog {Source = EventLogSource};
        }

        public override void Write(string message)
        {
            _eventlogger.WriteEntry(message);
        }
    }
}