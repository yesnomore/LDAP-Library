using System;
using System.IO;

namespace LDAPLibrary
{
    class FileLogger : ALogger
    {
        private readonly string _logPath;
        private const string LogFileName = "LDAPLog.txt";

        public FileLogger(string logPath)
        {
            if (Directory.Exists(logPath))
                _logPath = logPath;
            else
                throw new ArgumentException("The path for the log is not a directory");
        }

        public override void Write(string message)
        {
            using (var logWriter = new StreamWriter(_logPath+ @"\" + LogFileName, true))
            {
                logWriter.WriteLine(message);
                logWriter.Close();
            }
        }
    }
}
