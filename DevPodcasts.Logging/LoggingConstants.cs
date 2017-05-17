namespace DevPodcasts.Logging
{
    public class LoggingConstants
    {
#if DEBUG
            public static string AzureLogTableName = "LogDev";
#else
        public static string AzureLogTableName = "LogProd";
#endif
       
    }
}
