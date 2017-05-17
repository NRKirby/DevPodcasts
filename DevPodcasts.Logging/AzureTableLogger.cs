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
            var storage = CloudStorageAccount
                .Parse(ConfigurationManager.AppSettings["AzureStorageConnectionString"]);

            string tableName = LoggingConstants.AzureLogTableName;

            _log = new LoggerConfiguration()
               .WriteTo.AzureTableStorageWithProperties(storage, storageTableName: tableName)
               .MinimumLevel.Debug()
               .CreateLogger();
        }

        public AzureTableLogger(string connectionString)
        {
            var storage = CloudStorageAccount
                .Parse(connectionString);

            string tableName = LoggingConstants.AzureLogTableName;

            _log = new LoggerConfiguration()
                .WriteTo.AzureTableStorageWithProperties(storage, storageTableName: tableName)
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
            _log.Error($"{msg} {ex?.Message}");
        }
    }
}