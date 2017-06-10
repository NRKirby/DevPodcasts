using Microsoft.WindowsAzure.Storage.Table;

namespace DevPodcasts.Logging
{
    public class LogEntity : TableEntity
    {
        public string MessageTemplate { get; set; }

        public string Level { get; set; }

        public string RenderedMessage { get; set; }
    }
}
