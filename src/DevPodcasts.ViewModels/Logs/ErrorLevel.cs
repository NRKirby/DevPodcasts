using System.ComponentModel;

namespace DevPodcasts.ViewModels.Logs
{
    public enum ErrorLevel
    {
        [Description("All")]
        All = 0,

        [Description("Debug")]
        Debug = 1,

        [Description("Error")]
        Error = 2,

        [Description("Info")]
        Info = 3
    }
}
