using System.Diagnostics;

namespace LDAPLibrary.Logger
{
    internal class EventViewerLogger : ALogger
    {
        private const string EventLogSource = "LDAPLibrary";

        public override void Write(string message)
        {
            using (var e = new EventLog { Source = EventLogSource })
            {
                e.WriteEntry(message);
            }
        }
    }
}