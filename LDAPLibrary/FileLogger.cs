using System;
using System.IO;

namespace LDAPLibrary
{
    internal class FileLogger : ALogger
    {
        private const string LogFileName = "LDAPLog.txt";
        private readonly string _logPath;

        public FileLogger(string logPath)
        {
            if (Directory.Exists(logPath))
                _logPath = logPath;
            else
                throw new ArgumentException("The path for the log is not a directory");
        }

        public override void Write(string message)
        {
            using (var logWriter = new StreamWriter(_logPath + @"\" + LogFileName, true))
            {
                logWriter.WriteLine(message);
                logWriter.Close();
            }
        }
    }
}