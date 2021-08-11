using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Utilities
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
        public static int ToInt(this object obj)
        {
            if (obj==null)
            {
                return 0;
            }
            try
            {
                return int.Parse(obj.ToString());
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
            catch (Exception)
            {

                return new DateTime();
            }
        }
        public static string ToFormatedString(this DateTime value)
        {
            try
            {

                return value.ToString("yyyy/MM/dd");
            }
            catch (Exception)
            {

                return "";
            }
        }
        public static string ToFormatedString(this DateTime? value,string errorText="")
        {
            try
            {
                var date = DateTime.Parse(value.ToString());
                return date.ToString("yyyy/MM/dd");
            }
            catch (Exception)
            {

                return errorText;
            }
        }
    }
}