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

        public IEnumerable<LogItemViewModel> GetAllLogs()
        {
            var query = new TableQuery<LogEntity>();
            var logs = _table.ExecuteQuery(query);
            return logs
                .Select(i => new LogItemViewModel
                {
                    Level = i.Level,
                    TimeStamp = i.Timestamp.DateTime,
                    LogMessage = i.RenderedMessage
                });
        }

        public LogsViewModel GetLogs(int pageIndex, int itemsPerPage, string levelFilter)
        {
            var query = new TableQuery<LogEntity>();

            if (levelFilter != null)
            {
                query = query
                    .Where(TableQuery.GenerateFilterCondition("Level", QueryComparisons.Equal, levelFilter));
            }

            var tableQueryResult = _table
                .ExecuteQuery(query)
                .ToList();

            var logs = tableQueryResult
                .OrderByDescending(i =>i.Timestamp.DateTime)
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
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    TotalItems = allLogs.Count,
                    TotalPages = int.Parse(Math.Ceiling((decimal)allLogs.Count / itemsPerPage).ToString())
                }
            };

            viewModel.PaginationInfo.Next = viewModel.PaginationInfo.ActualPage == viewModel.PaginationInfo.TotalPages - 1 ? "is-disabled" : "";
            viewModel.PaginationInfo.Previous = viewModel.PaginationInfo.ActualPage == 0 ? "is-disabled" : "";

            return viewModel;
        }
    }
}
