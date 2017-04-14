using System;
using System.IO;

namespace DevPodcasts.ServiceLayer.Logging
{
    public class FileLogger : ILogger
    {
        public void Error(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Info(object msg)
        {
            throw new NotImplementedException();
        }

        public void Debug(string msg)
        {
            throw new NotImplementedException();
        }

        public void Error(string msg, Exception ex)
        {
            using (var sw = new StreamWriter(@"C:\Users\Nick\Desktop\projects\DevPodcasts\DevPodcasts.ServiceLayer\Logging\Data\Log.txt", true))
            {
                sw.WriteLine(DateTime.Now + " : "+ msg + " : " + ex.Message);
            }
        }
    }
}
