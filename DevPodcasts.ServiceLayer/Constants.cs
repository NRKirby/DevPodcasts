namespace DevPodcasts.ServiceLayer
{
    public class Constants
    {
#if DEBUG
        public static string AzureLogTableName = "LogDev";
#else
        public static string AzureLogTableName = "LogProd";
#endif
    }
}
