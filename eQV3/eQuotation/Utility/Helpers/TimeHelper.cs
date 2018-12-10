using eQuotation.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Helpers;

namespace eQuotation.Utility
{
    public static class TimeHelper
    {
        public static DateTime ConvertToLocalTime(DateTime time, string region)
        {
            var localtime = time;


            Dictionary<string, string> dicTZ = (Dictionary<string, string>)HttpContext.Current.Cache["TZ"];
            if (dicTZ == null)
            {
                dicTZ = new Dictionary<string, string>();
                HttpContext.Current.Cache.Add("TZ", dicTZ, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

            }
            if (!dicTZ.ContainsKey(region))
            {

                var tz = DBUtil.dbExecuteScalar("MY", String.Format("select top 1 isnull(timezonename,'') as timezonename from TIMEZONE where org like '%{0}'", region));
                if (tz != null)
                {
                    dicTZ.Add(region, tz.ToString());
                }
            }

            if (!string.IsNullOrEmpty(dicTZ[region]))
            {
                var utcTime = time.ToUniversalTime();
                string timezone = dicTZ[region];
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                var ts = tzi.GetUtcOffset(utcTime);
                localtime = utcTime.Add(ts);
            }


            return localtime;
        }

        public static DateTime ConvertToSystemTime(DateTime sourceTime, string region)
        {


 
            var taipeiTime = sourceTime;
            try
            {
                var userTimezone = DBUtil.dbExecuteScalar("MY", String.Format("select top 1 isnull(timezonename,'') as timezonename from TIMEZONE where org like '%{0}'", region));
                if (userTimezone != null && !string.IsNullOrEmpty(userTimezone.ToString()))
                {
                    string userTZ = userTimezone.ToString();
                    var userTZi = TimeZoneInfo.FindSystemTimeZoneById(userTZ);

                    var utcTime = TimeZoneInfo.ConvertTimeToUtc(sourceTime, userTZi);

                    var tzi = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                    var ts = tzi.GetUtcOffset(utcTime);
                    taipeiTime = utcTime.Add(ts);
                }
            }
            catch
            {
            }
            

         

            return taipeiTime;
        }

    }



}