using System;
using System.Globalization;

namespace ZeroGravity.Mobile.Contract.Helper
{
    public static class DateTimeHelper
    {
        public static string ToUniversalControllerDate(DateTime dateTime, bool isLocalBeginOfDayInUtcRequired = true)
        {
            string dateAsString;
            if (isLocalBeginOfDayInUtcRequired)
            {
                var localBeginOfDayInUtc = ConvertLocalBeginOfDayToUtc(dateTime);

                dateAsString = localBeginOfDayInUtc.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                dateAsString = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return dateAsString;
        }

        /// <summary>
        /// Converts a local <see cref="DateTime"/> to the local's day start time in UTC.
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <returns>The local's day start time as <see cref="DateTime"/> in UTC.</returns>
        public static DateTime ConvertLocalBeginOfDayToUtc(DateTime localDateTime)
        {
            var localBeginOfDay = new DateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day);
            var localBeginOfDayInUtc = localBeginOfDay.ToUniversalTime();

            return localBeginOfDayInUtc;
        }

        /// <summary>
        /// Converts universal date to local date.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/>The universal date</param>
        /// <returns>The local date</returns>
        public static DateTime ToLocalTime(DateTime dateTime)
        {
            return dateTime.ToLocalTime();
        }

        /// <summary>
        /// Converts local date to universal date.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/>The local date</param>
        /// <returns>The universal date</returns>
        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            return dateTime.ToUniversalTime();
        }

        /// <summary>
        /// Converts universal date to today of local time.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/>The universal date</param>
        /// <returns>The today of local date.</returns>
        public static DateTime ToLocalCurrentDate(DateTime dateTime)
        {
            var localTime = ToLocalTime(dateTime);
            return new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);
        }

        /// <summary>
        /// Converts universal date to today of local time. Format: 15. October 2020
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/>The universal date</param>
        /// <returns>The today of local date in Zero-Gravity format.</returns>
        public static string ToLocalDateZeroGravityFormat(DateTime dateTime)
        {
            var localTime = dateTime.ToLocalTime();
            return localTime.ToString("d MMMM yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adds a provided <see cref="TimeSpan"/> to a provided <see cref="DateTime"/>.
        /// Example: 22.01.2021 00:00:00 + 11:38:00 = 22.01.2021 11:38:00
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeSpan"></param>
        /// <returns>Return the provided <see cref="DateTime"/> with the added <see cref="TimeSpan"/></returns>
        public static DateTime AddTimeSpanToDateTime(DateTime dateTime, TimeSpan timeSpan)
        {
            var newDateTime = dateTime.Date + timeSpan;
            return newDateTime;
        }


        /// <summary>
        /// Converts given date time to localized time format (currently only HH:mm)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetTimeFormat(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }
        /// <summary>
        /// returns date with suffix for ex: 25th April
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetDateWithSuffix(DateTime dateTime)
        {
            string dateWithSuffix = string.Format(new DayNumberFormatInfo(), "{0:mx}", dateTime);
            return dateWithSuffix.TrimStart(new Char[] { '0' });
        }

    }
}