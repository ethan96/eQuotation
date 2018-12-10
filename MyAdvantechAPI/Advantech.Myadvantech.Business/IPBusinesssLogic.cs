using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Numerics;
using Advantech.Myadvantech.DataAccess;

namespace Advantech.Myadvantech.Business
{
    public class IPBusinesssLogic
    {
        private static int switchtoLocal = 0;
        public static String IPtoNation(String sourceIP)
        {
            String nation = "";
            if (switchtoLocal == 0)
            {
                nation = IPtoNationGeoIPCDatabase(sourceIP);
                if (nation == "ERROR" || nation == "XX")
                {
                    switchtoLocal = 50;
                    nation = IPtoNationwipmania(sourceIP);
                }
            }
            else
            {
                nation = IPtoNationwipmania(sourceIP);
                switchtoLocal--;
            }
            return nation;
        }


        private static String IPtoNationwipmania(String sourceIP)
        {
            String url = "http://api.wipmania.com/" + sourceIP.Trim() + "?k=NoA-k2UCnQvC0tSmTMIR7Ji23A1";
            String nation = "";

            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = 2000;
                WebResponse webResponse = webRequest.GetResponse();

                StreamReader responseStream = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding(1252));

                nation = responseStream.ReadToEnd();
            }
            catch (Exception ex)
            {
                //eStore.Utilities.eStoreLoger.Warn("IPToNation failed", sourceIP, "", "", ex);
                nation = "ERROR";
            }

            return nation;
        }

        private static String IPtoNationwipmaniaFromLoacl(String sourceIP)
        {
            String url = "http://172.21.128.41:7080/default.aspx?ip=" + sourceIP.Trim();
            String nation = "";

            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = 2000;
                WebResponse webResponse = webRequest.GetResponse();

                StreamReader responseStream = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding(1252));

                nation = responseStream.ReadToEnd();
            }
            catch (Exception ex)
            {
                //eStore.Utilities.eStoreLoger.Warn("IPToNation failed", sourceIP, "", "", ex);
                nation = "ERROR";
            }

            return nation;
        }


        public static String IPtoNationGeoIPCDatabase(String sourceIP)
        {
            Int64 ip = convertIPtoInteger(sourceIP);
            
            if (ip == 0)
            {
                BigInteger iPV6 = convertIPtoBigInt(sourceIP);
                if (iPV6 == 0)
                    return "XX";
                else
                    return getCountryShortFromGeoIPV6CDatabase(iPV6.ToString());
            }
            else
            {
                return getCountryshortfromGeoIPCDatabase(ip);

            }
        }


        public static String IPtoCountryNameByIP(String sourceIP)
        {
            Int64 ip = convertIPtoInteger(sourceIP);

            if (ip == 0)
            {
                BigInteger iPV6 = convertIPtoBigInt(sourceIP);
                if (iPV6 == 0)
                    return "XX";
                else
                    return getCountryNameByIPV6(iPV6.ToString());
            }
            else
            {
                return getCountryNameByIPV4(ip);

            }
        }

        private static string getCountryshortfromGeoIPCDatabase(Int64 ip)
        {
            //string CommText = string.Format("select shorts from GeoLoc where   {0} between ipfromN and   iptoN ", ip);
            //Change to new IP tables.
            string table = DateTime.Now.AddDays(-3).Month % 2 > 0 ? "Ip2Location_Ip2Country_A" : "Ip2Location_Ip2Country_B";
            string CommText = string.Format("select shorts from  {0}  where  {1}  between  IpFrom  and  IpTo ", table, ip);

            string connString = ConfigurationManager.ConnectionStrings["MY"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();

                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand(CommText, con);
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteScalar() != null ? (string)cmd.ExecuteScalar() : "XX";
                }
            }
            catch (Exception)
            {
                return "ERROR";
            }

        }

        private static string getCountryNameByIPV4(Int64 ip)
        {
            //string CommText = string.Format("select shorts from GeoLoc where   {0} between ipfromN and   iptoN ", ip);
            //Change to new IP tables.
            string table = DateTime.Now.AddDays(-3).Month % 2 > 0 ? "Ip2Location_Ip2Country_A" : "Ip2Location_Ip2Country_B";
            string CommText = string.Format("SELECT top 1 CountryName from  {0}  where  {1}  between  IpFrom  and  IpTo ", table, ip);

            
            //string connString = ConfigurationManager.ConnectionStrings["MY"].ToString();
            try
            {
                object countryName = SqlProvider.dbExecuteScalar("MY", CommText);
                return countryName != null ? countryName.ToString() : "XX" ;
                //using (SqlConnection con = new SqlConnection(connString))
                //{
                //    con.Open();

                //    if (con.State == System.Data.ConnectionState.Closed)
                //        con.Open();

                //    SqlCommand cmd = new SqlCommand(CommText, con);
                //    cmd.CommandType = CommandType.Text;
                //    return cmd.ExecuteScalar() != null ? (string)cmd.ExecuteScalar() : "XX";
                //}
            }
            catch (Exception)
            {
                return "ERROR";
            }

        }

        private static string getCountryShortFromGeoIPV6CDatabase(string iPV6)
        {
            string table = DateTime.Now.AddDays(-3).Month % 2 > 0 ? "Ip2Location_Ip2Country_IPV6_A" : "Ip2Location_Ip2Country_IPV6_B";
            string CommText = string.Format("select top 1 shorts from  {0} where '{1}' <= IpTo ", table, iPV6);

            string connString = ConfigurationManager.ConnectionStrings["MY"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();

                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand(CommText, con);
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteScalar() != null ? (string)cmd.ExecuteScalar() : "XX";
                }
            }
            catch (Exception)
            {
                return "ERROR";
            }

        }

        private static string getCountryNameByIPV6(string iPV6)
        {
            string table = DateTime.Now.AddDays(-3).Month % 2 > 0 ? "Ip2Location_Ip2Country_IPV6_A" : "Ip2Location_Ip2Country_IPV6_B";
            string CommText = string.Format("SELECT top 1 CountryName From  {0} where '{1}' <= IpTo ", table, iPV6);

            //string connString = ConfigurationManager.ConnectionStrings["MY"].ToString();
            try
            {
                object countryName = SqlProvider.dbExecuteScalar("MY", CommText);
                return countryName != null ? countryName.ToString() : "XX";
                //using (SqlConnection con = new SqlConnection(connString))
                //{
                //    con.Open();

                //    if (con.State == System.Data.ConnectionState.Closed)
                //        con.Open();

                //    SqlCommand cmd = new SqlCommand(CommText, con);
                //    cmd.CommandType = CommandType.Text;
                //    return cmd.ExecuteScalar() != null ? (string)cmd.ExecuteScalar() : "XX";
                //}
            }
            catch (Exception)
            {
                return "ERROR";
            }

        }

        private static Int64 convertIPtoInteger(string ip)
        {
            Int64 rlt = 0;
            string[] ips = ip.Split('.');
            Int64 a, b, c, d;
            if (ips.Length == 4)
            {
                if (Int64.TryParse(ips[0], out a) &&
                    Int64.TryParse(ips[1], out b) &&
                    Int64.TryParse(ips[2], out c) &&
                    Int64.TryParse(ips[3], out d))
                {
                    rlt = 16777216 * a + 65536 * b + 256 * c + d;
                }
            }
            return rlt;
        }

        private static BigInteger convertIPtoBigInt(string ipv6)
        {
            System.Net.IPAddress address;
            BigInteger ipnum = 0; //BigInteger 可用來處理超大整數

            if (System.Net.IPAddress.TryParse(ipv6, out address))
            {
                byte[] addrBytes = address.GetAddressBytes();

                if (System.BitConverter.IsLittleEndian)
                {
                    System.Collections.Generic.List<byte> byteList = new System.Collections.Generic.List<byte>(addrBytes);
                    byteList.Reverse();
                    addrBytes = byteList.ToArray();
                }

                if (addrBytes.Length > 8)
                {
                    //IPv6
                    ipnum = System.BitConverter.ToUInt64(addrBytes, 8);
                    ipnum <<= 64;
                    ipnum += System.BitConverter.ToUInt64(addrBytes, 0);
                }
                else
                {
                    //IPv4
                    ipnum = System.BitConverter.ToUInt32(addrBytes, 0);
                }
                return ipnum;
            }
            return ipnum;
        }
    }
}
