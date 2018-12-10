using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;

namespace eQuotation.Utility
{
    public static class ObjectExtentions
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }


        public static string ToDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] memInfo = type.GetMember(enumeration.ToString());

            if (null != memInfo && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDisplay), false);
                if (null != attrs && attrs.Length > 0)
                    return ((EnumDisplay)attrs[0]).Text;
            }

            return enumeration.ToString();
        }

        public static bool ToExclude(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] memInfo = type.GetMember(enumeration.ToString());

            if (null != memInfo && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDisplay), false);
                if (null != attrs && attrs.Length > 0)
                    return ((EnumDisplay)attrs[0]).IsExclude;
            }

            return false;
        }

        public static bool HasValue<T>(string text)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("TProperty must be an enumerated type");

            foreach (var c in Enum.GetValues(typeof(T)).Cast<Enum>())
            {
                if (text == c.ToDescription())
                    return true;
                    //return (T) Enum.Parse(typeof(T), c.ToString(), true);
            }

            return false;
        }

        public static T GetEnum<T>(string text)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("TProperty must be an enumerated type");

            foreach (var c in Enum.GetValues(typeof(T)).Cast<Enum>())
            {
                if (text == c.ToDescription())
                    return (T) Enum.Parse(typeof(T), c.ToString(), true);
            }

            return default(T);
        }

        public static DateTime FirstDateOfWeek(this DateTime sourceDateTime)
        {
            var daysAhead = (DayOfWeek.Sunday - (int)sourceDateTime.DayOfWeek);

            sourceDateTime = sourceDateTime.AddDays((int)daysAhead);

            return sourceDateTime;
        }

        public static DateTime LastDateOfWeek(this DateTime sourceDateTime)
        {
            var daysAhead = DayOfWeek.Saturday - (int)sourceDateTime.DayOfWeek;

            sourceDateTime = sourceDateTime.AddDays((int)daysAhead);

            return sourceDateTime;
        }

        public static int Week(this DateTime? date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear((DateTime) date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

        }

        public static string[] SplitTextNum(this string text)
        {
            var retval = new string[3];

            var prefix = string.Empty;
            var suffix = string.Empty;

            if (string.IsNullOrEmpty(text)) return null;

            var len = text.Length;
            var regex = new Regex("^[0-9]+$");

            for (var idx = len - 1; idx >= 0; idx--)
            {
                var str = text.Substring(idx, 1);

                if (regex.IsMatch(str))
                    suffix = string.Format("{0}{1}", str, suffix);
                else
                    break;
            }

            prefix = text.Substring(0, text.Length - suffix.Length);

            //text
            retval[0] = prefix;

            //numeric
            retval[1] = suffix;

            //length of numeric
            retval[2] = suffix.Length.ToString();

            return retval;
        }



    }
}