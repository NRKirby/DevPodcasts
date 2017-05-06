using Microsoft.WindowsAzure.Storage;
using Serilog;
using Serilog.Core;
using System;
using System.Configuration;

namespace DevPodcasts.ServiceLayer.Logging
{
    public class AzureTableLogger : ILogger
    {
        private readonly Logger _log;

        public AzureTableLogger()
        {
            var storage = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureStorageConnectionString"]);
            _log = new LoggerConfiguration()
               .WriteTo.AzureTableStorageWithProperties(storage, storageTableName: "LogDev")
               .MinimumLevel.Debug()
               .CreateLogger();
        }

        public void Error(Exception ex)
        {
            _log.Error(ex.Message);
        }

        public void Info(object msg)
        {
            _log.Information(msg.ToString());
        }

        public void Debug(string msg)
        {
            _log.Debug(msg);
        }

        public void Error(string msg, Exception ex)
        {
            _log.Error(msg + " " + ex.Message);
        }
    }
}
