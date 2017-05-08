using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DevPodcasts.ServiceLayer.Logging
{
    public class LogService
    {
        private readonly CloudTable _table;

        public LogService()
        {
            var storage = CloudStorageAccount
                .Parse(ConfigurationManager.AppSettings["AzureStorageConnectionString"]);

            string tableName = Constants.AzureLogTableName;

            CloudTableClient tableClient = storage.CreateCloudTableClient();

            _table = tableClient.GetTableReference(tableName);
        }

        public IEnumerable<LogEntity> GetAllLogs()
        {
            var query = new TableQuery<LogEntity>();
            return _table.ExecuteQuery(query).ToList();
        }
    }
}
