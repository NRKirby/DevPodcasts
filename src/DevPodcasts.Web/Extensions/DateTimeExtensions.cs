using System;

namespace DevPodcasts.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortUkDate(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        public static string ToUkDateAndTime(this DateTime dateTime)
        {
            var britishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, britishZone).ToString("G");
        }
    }
}