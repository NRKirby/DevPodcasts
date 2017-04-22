using System;
using System.IO;
using System.Web.Configuration;

namespace DevPodcasts.ServiceLayer.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath = WebConfigurationManager.AppSettings["LogFilePath"];

        public void Error(Exception ex)
        {
            using (var sw = new StreamWriter(_logFilePath, true))
            {
                sw.WriteLine(DateTime.Now + " Error: " + ex.Message);
            }
        }

        public void Info(object msg)
        {
            using (var sw = new StreamWriter(_logFilePath, true))
            {
                sw.WriteLine(DateTime.Now + " Info: " + msg);
            }
        }

        public void Debug(string msg)
        {
            using (var sw = new StreamWriter(_logFilePath, true))
            {
                sw.WriteLine(DateTime.Now + " Debug: " + msg);
            }
        }

        public void Error(string msg, Exception ex)
        {
            using (var sw = new StreamWriter(_logFilePath, true))
            {
                sw.WriteLine(DateTime.Now + " Error: "+ msg + ex.Message);
            }
        }
    }
}
