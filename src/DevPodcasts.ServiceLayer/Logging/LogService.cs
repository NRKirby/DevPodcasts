using DevPodcasts.Logging;
using DevPodcasts.ViewModels;
using DevPodcasts.ViewModels.Logs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DevPodcasts.ServiceLayer.Logging
{
    public class LogService
    {
        private readonly CloudTable _table;
        private const int NumberOfRowsToRetrieveFromTableStorage = 500;

        public LogService()
        {
            var storage = CloudStorageAccount
                .Parse(ConfigurationManager.AppSettings["AzureStorageConnectionString"]);
            var tableName = Constants.AzureLogTableName;
            var tableClient = storage.CreateCloudTableClient();
            _table = tableClient.GetTableReference(tableName);
        }

        public LogsViewModel GetIndexViewModel(int pageIndex, int itemsPerPage, string filter)
        {
            var tableQueryResult = ExecuteTableQuery(filter, NumberOfRowsToRetrieveFromTableStorage);

            var logs = tableQueryResult
                .OrderByDescending(i => i.Timestamp.DateTime)
                .Skip(itemsPerPage * pageIndex)
                .Take(itemsPerPage);

            var viewModel = new LogsViewModel
            {
                Items = logs
                    .Select(i => new LogItemViewModel
                    {
                        Level = i.Level,
                        TimeStamp = i.Timestamp.DateTime.ToLocalTime(),
                        LogMessage = i.RenderedMessage
                    }),
                PaginationInfo = new PaginationInfo(pageIndex, itemsPerPage, tableQueryResult.Count),
                Filter = filter
            };

            return viewModel;
        }

        private List<LogEntity> ExecuteTableQuery(string filter, int top)
        {
            var query = new TableQuery<LogEntity>();

            if (filter != null)
            {
                query = query
                    .Where(TableQuery.GenerateFilterCondition("Level", QueryComparisons.Equal, filter));
            }

            query.Take(top);

            return _table
                .ExecuteQuery(query)
                .ToList();
        }

        public IEnumerable<LogItemViewModel> GetLogs(int pageIndex, int itemsPerPage, string filter)
        {
            var tableQueryResult = ExecuteTableQuery(filter, NumberOfRowsToRetrieveFromTableStorage);

            return tableQueryResult
                .OrderByDescending(i => i.Timestamp.DateTime)
                .Skip(itemsPerPage * pageIndex)
                .Take(itemsPerPage)
                .Select(i => new LogItemViewModel
                {
                    Level = i.Level,
                    TimeStamp = i.Timestamp.DateTime.ToLocalTime(),
                    LogMessage = i.RenderedMessage
                });
        }
    }
}
