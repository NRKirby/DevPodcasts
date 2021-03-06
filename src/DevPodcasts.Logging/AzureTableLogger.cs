﻿using Microsoft.WindowsAzure.Storage;
using Serilog;
using Serilog.Core;
using System;
using System.Configuration;

namespace DevPodcasts.Logging
{
    public class AzureTableLogger : ILogger
    {
        private readonly Logger _log;

        public AzureTableLogger()
        {
            var azureStorageConnectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];

            if (string.IsNullOrEmpty(azureStorageConnectionString)) return;

            var storage = CloudStorageAccount
                .Parse(ConfigurationManager.AppSettings["AzureStorageConnectionString"]);

            var tableName = LoggingConstants.AzureLogTableName;

            _log = new LoggerConfiguration()
                .WriteTo.AzureTableStorageWithProperties(storage, storageTableName: tableName)
                .MinimumLevel.Debug()
                .CreateLogger();
        }

        public AzureTableLogger(string connectionString, string tableName)
        {
            var storage = CloudStorageAccount
                .Parse(connectionString);

            _log = new LoggerConfiguration()
                .WriteTo.AzureTableStorageWithProperties(storage, storageTableName: tableName)
                .MinimumLevel.Debug()
                .CreateLogger();
        }

        public void Error(Exception ex)
        {
            _log?.Error(ex.Message);
        }

        public void Info(object msg)
        {
            _log?.Information(msg.ToString());
        }

        public void Debug(string msg)
        {
            _log?.Debug(msg);
        }

        public void Error(string msg, Exception ex)
        {
            _log?.Error($"{msg} {ex?.Message}");
        }
    }
}
