using System;
using System.Globalization;

namespace Riddhasoft.Globals.Conversion
{
    public static class ConversionExtension
    {
        public static string ToString(this TimeSpan? obj, string format)
        {
            try
            {

                return TimeSpan.Parse(obj.ToString()).ToString(format);
            }
            catch
            {

                return new TimeSpan().ToString(format);
            }
        }
        public static TimeSpan? ToNullableTimeSpan(this string obj)
        {
            try
            {
                return TimeSpan.Parse(obj);
            }
            catch
            {

                return null;
            }
        }
        public static TimeSpan ToTimeSpan(this string obj)
        {
            try
            {
                return TimeSpan.Parse(obj);
            }
            catch
            {

                return new TimeSpan(0);
            }
        }
        public static int? ToNullableInt(this object obj)
        {
            try
            {
                return int.Parse(obj.ToString());
            }
            catch
            {

                return null;
            }
        }
        public static string ConvertFromToTime(this string timeHour, string inputFormat, string outputFormat)
        {
            var timeFromInput = DateTime.ParseExact(timeHour, inputFormat, null, DateTimeStyles.None);
            string timeOutput = timeFromInput.ToString(
                outputFormat,
                CultureInfo.InvariantCulture);
            return timeOutput;
        }
        public static int ToInt(this object obj)
        {
            try
            {
                return int.Parse(obj.ToString());
            }
            catch
            {

                return 0;
            }
        }
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                return decimal.Parse(obj.ToString());
            }
            catch
            {

                return 0;
            }
        }
        public static DateTime ToDateTime(this DateTime? obj)
        {
            try
            {
                return DateTime.Parse(obj.ToString());
            }
            catch
            {

                return new DateTime();
            }
        }
        public static string ToString(this object obj)
        {
            return obj.ToString();
        }
        public static DateTime? ToNullableDatetime(this string value)
        {
            try
            {
                return DateTime.Parse(value);
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static DateTime ToDateTime(this string value)
        {
            try
            {

                return DateTime.Parse(value);
            }
            catch (Exception ex)
            {

                return new DateTime();
            }
        }
        
       
       
    }
}
