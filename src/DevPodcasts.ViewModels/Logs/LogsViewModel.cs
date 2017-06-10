using System;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Logs
{
    public class LogsViewModel
    {
        public IEnumerable<LogItemViewModel> Items { get; set; }

        public PaginationInfo PaginationInfo { get; set; }

        public ErrorLevel Level { get; set; }

        public string Filter { get; set; }
    }

    public class LogItemViewModel
    {
        public DateTime TimeStamp { get; set; }

        public string LogMessage { get; set; }

        public string Level { get; set; }
    }
}
