using DevPodcasts.ViewModels;
using DevPodcasts.ViewModels.Logs;
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
            var tableName = Constants.AzureLogTableName;
            var tableClient = storage.CreateCloudTableClient();
            _table = tableClient.GetTableReference(tableName);
        }

        public LogsViewModel GetIndexViewModel(int pageIndex, int itemsPerPage, string filter)
        {
            var query = new TableQuery<LogEntity>();

            if (filter != null)
            {
                query = query
                    .Where(TableQuery.GenerateFilterCondition("Level", QueryComparisons.Equal, filter));
            }

            var tableQueryResult = _table
                .ExecuteQuery(query)
                .ToList();

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

        public IEnumerable<LogItemViewModel> GetLogs(int pageIndex, int itemsPerPage, string filter)
        {
            var query = new TableQuery<LogEntity>();

            if (filter != null)
            {
                query = query
                    .Where(TableQuery.GenerateFilterCondition("Level", QueryComparisons.Equal, filter));
            }

            var tableQueryResult = _table
                .ExecuteQuery(query)
                .ToList();

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
