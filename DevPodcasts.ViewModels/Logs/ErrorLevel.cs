using System.ComponentModel;

namespace DevPodcasts.ViewModels.Logs
{
    public enum ErrorLevel
    {
        [Description("Debug")]
        Debug = 0,
        [Description("Error")]
        Error = 1,
        [Description("Info")]
        Info = 2
    }
}
