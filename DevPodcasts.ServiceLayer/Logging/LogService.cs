using Microsoft.WindowsAzure.Storage;
using Serilog;
using Serilog.Core;
using System.Configuration;

namespace DevPodcasts.ServiceLayer.Logging
{
    public class LogService
    {
        private readonly Logger _log;

        public LogService()
        {
            var storage = CloudStorageAccount
                .Parse(ConfigurationManager.AppSettings["AzureStorageConnectionString"]);

            string tableName = Constants.AzureLogTableName;

            _log = new LoggerConfiguration()
                .WriteTo.AzureTableStorageWithProperties(storage, storageTableName: tableName)
                .MinimumLevel.Debug()
                .CreateLogger();
        }
    }
}
