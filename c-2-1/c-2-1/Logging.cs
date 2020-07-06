using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace c_2_1
{
    public static class Logger
    {
        public static Action<string> WriteMessage;

        public static void LogMessage(string msg)
        {
            WriteMessage(msg);
        }
    }

    public static class LoggingMethods
    {
        public static void LogToConsole(string message)
        {
            Console.Error.WriteLine(message);
        }

        public class FileLogger
        {
            private readonly string logPath;
            public FileLogger(string path)
            {
                logPath = path;
                Logger.WriteMessage += LogMessage;
            }

            public void DetachLog() => Logger.WriteMessage -= LogMessage;
            // make sure this can't throw.
            private void LogMessage(string msg)
            {
                try
                {
                    using (var log = File.AppendText(logPath))
                    {
                        log.WriteLine(DateTime.Now + " [INFO] " + msg);
                        log.Flush();
                    }
                }
                catch (Exception)
                {
                    // Hmm. We caught an exception while
                    // logging. We can't really log the
                    // problem (since it's the log that's failing).
                    // So, while normally, catching an exception
                    // and doing nothing isn't wise, it's really the
                    // only reasonable option here.
                }
            }
        }
    }
}

