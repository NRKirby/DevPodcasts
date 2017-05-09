using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DevPodcasts.ViewModels.Logs
{
    public class LogsViewModel
    {
        public IEnumerable<LogItemViewModel> Items { get; set; }

        public PaginationInfo PaginationInfo { get; set; }

        public SelectList ErrorLevels { get; set; }

        public string SelectedErrorLevel { get; set; }
    }

    public class LogItemViewModel
    {
        public DateTime TimeStamp { get; set; }

        public string LogMessage { get; set; }

        public string Level { get; set; }
    }
}
