using System.Diagnostics;

namespace LDAPLibrary.Logger
{
    class EventViewerLogger : ALogger
    {
        private readonly EventLog _eventlogger;
        private const string EventLogSource = "LDAPLibrary";
        public EventViewerLogger()
        {
            _eventlogger = new EventLog{ Source = EventLogSource };
        }

        public override void Write(string message)
        {
            _eventlogger.WriteEntry(message);
        }
    }
}
