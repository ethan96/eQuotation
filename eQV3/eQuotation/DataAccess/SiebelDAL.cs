using Advantech.Myadvantech.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eQuotation.DataAccess
{
    public class SiebelDAL
    {
        static int _SiebelWSTimeOut = 60000;

        public static DataTable GetSiebelAccountList(SiebelAccountSearch search, List<string> SiebelRBU)
        {
            // Ryan 20171113 Add to select with RBU parameter
            //ICC 2018/2/9 Change parameter. Use new class to search Siebel DB (Country, City, Zip code, State)
            string _sql = " select TOP 500 a.ROW_ID AS ROW_ID, a.NAME as COMPANYNAME, IsNull(b.ATTRIB_05,'') as ERPID, ";
            _sql += " IsNull(d.COUNTRY,'') as COUNTRY, IsNull(d.CITY,'') as CITY, Isnull(a.LOC,'') as LOCATION, ";
            _sql += " IsNull(c.NAME, '') as RBU, IsNull(d.STATE,'') as STATE,IsNull(d.PROVINCE,'') as PROVINCE, ";
            _sql += " IsNull(a.CUST_STAT_CD,'') as STATUS ,IsNull(d.ADDR,'') as ADDRESS, IsNull(d.ZIPCODE,'') as ZIPCODE, IsNull(d.ADDR_LINE_2,'') as ADDRESS2,";
            _sql += " (SELECT TOP 1 isnull(EMAIL_ADDR,'') FROM S_CONTACT WHERE ROW_ID=(SELECT TOP 1 PR_EMP_ID from S_POSTN where ROW_ID = (SELECT TOP 1 PR_POSTN_ID FROM S_ORG_EXT WHERE ROW_ID=a.ROW_ID)) and EMAIL_ADDR is not null) AS priSales, ";
            _sql += " IsNull(a.BASE_CURCY_CD,'') as CURRENCY ";
            _sql += " from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID ";
            _sql += " left join S_PARTY c on a.BU_ID=c.ROW_ID ";
            _sql += " left join S_ADDR_PER d on a.PR_ADDR_ID=d.ROW_ID where 1 = 1 ";

            if (SiebelRBU != null && SiebelRBU.Count > 0)
            {
                SiebelRBU = SiebelRBU.Select(d => d = "'" + d + "'").ToList();
                _sql += " and c.NAME in (" + String.Join(",", SiebelRBU) + ") ";
            }
            if (!string.IsNullOrEmpty(search.AccountName))
            {
                _sql += string.Format(" and Upper(ISNULL(a.NAME,'')) like Upper(N'%{0}%') ", search.AccountName.Trim().ToUpper().Replace("'", "''").Replace("*", "%"));
            }
            if (!string.IsNullOrEmpty(search.ERPID))
            {
                _sql += string.Format(" and Upper(ISNULL(b.ATTRIB_05,'')) like Upper(N'%{0}%') ", search.ERPID.Trim().ToUpper().Replace("'", "''").Replace("*", "%"));
            }
            if (!string.IsNullOrEmpty(search.PrimarySales))
            {
                _sql += " AND a.PR_POSTN_ID IN (SELECT ROW_ID from S_POSTN WHERE S_POSTN.PR_EMP_ID IN(SELECT ROW_ID FROM S_CONTACT WHERE Upper(S_CONTACT.EMAIL_ADDR) like '%" + search.PrimarySales.ToUpper() + "%')) ";
            }
            if (!string.IsNullOrEmpty(search.AccountStatus))
            {
                _sql += string.Format(" and Upper(a.CUST_STAT_CD) = Upper(N'{0}') ", search.AccountStatus.Trim().ToUpper().Replace("'", "''").Replace("*", "%"));
            }

            if (!string.IsNullOrEmpty(search.Country))
                _sql += string.Format(" and Upper(d.COUNTRY) like '%{0}%' ", search.Country.Trim().ToUpper().Replace("'", "''").Replace("*", "%"));

            if (!string.IsNullOrEmpty(search.City))
                _sql += string.Format(" and Upper(d.CITY) like '%{0}%' ", search.City.Trim().ToUpper().Replace("'", "''").Replace("*", "%"));

            if (!string.IsNullOrEmpty(search.ZipCode))
                _sql += string.Format(" and Upper(d.ZIPCODE) like '%{0}%' ", search.ZipCode.Trim().ToUpper().Replace("'", "''").Replace("*", "%"));

            if (!string.IsNullOrEmpty(search.State))
                _sql += string.Format(" and Upper(d.STATE) like '%{0}%' ", search.State.Trim().ToUpper().Replace("'", "''").Replace("*", "%"));

            if (!string.IsNullOrEmpty(search.AccountRowId))
            {
                _sql += string.Format(" and Upper(ISNULL(a.ROW_ID,'')) like Upper(N'%{0}%') ", search.AccountRowId.Trim().ToUpper().Replace("'", "''").Replace("*", "%"));
            }
            _sql += "order by a.ROW_ID ";

            DataTable SiebelAccount = DBUtil.dbGetDataTable("CRM", _sql);

            return SiebelAccount;
        }

        public static DataTable GetSalesInfo(string Email)
        {
            var DT = new DataTable();
            if (!string.IsNullOrEmpty(Email))
            {
                string _sql = "  select top 1 (isnull(FirstName,'') + ' ' + isnull(LastName,'')) as FULL_NAME,isnull(CellPhone,'') as TEL,isnull(EMAIL_ADDRESS,'') as EMAIL from SIEBEL_CONTACT ";
                _sql += string.Format(" where EMAIL_ADDRESS='{0}' ", Email.Trim().Replace("'", "''"));
                DT = DBUtil.dbGetDataTable("MY", _sql);
            }

            return DT;
        }

        public static DataTable GetEndCustomerOpty(string Account_ID,string _OptyName,string userEmail)
        {
            //String str = String.Format(@"select a.ROW_ID,a.NAME,b.ACCOUNT_NAME,a.CREATED,
            //                isnull((select USER_LOGIN from SIEBEL_POSITION where ROW_ID=a.PR_POSTN_ID),'') as [PRIMARY], 
            //                a.STAGE_NAME,a.CONTACT from SIEBEL_OPPORTUNITY a (nolock)
            //                left join SIEBEL_ACCOUNT b on a.ACCOUNT_ROW_ID=b.ROW_ID
            //                where a.ACCOUNT_ROW_ID in (
            //                select ROW_ID from SIEBEL_ACCOUNT (nolock)
            //                where PARENT_ROW_ID='{0}' and ERP_ID='{1}')", Account_ID, EndCustomerERPID);
            String str = String.Format(@"select distinct case a.CURR_STG_ID WHEN '1-VXVAID' THEN 'c' WHEN '1-VXVAIE' THEN 'b' ELSE 'a' END AS orderbystr, 
							a.ROW_ID,a.NAME,b.ACCOUNT_NAME,a.CREATED, 
                            isnull((select top 1 USER_LOGIN from SIEBEL_POSITION where CONTACT_ID=e.ROW_ID),'') as [PRIMARY], 
                            a.STAGE_NAME,a.CONTACT from SIEBEL_OPPORTUNITY a (nolock) 
                            left join SIEBEL_ACCOUNT b on a.ACCOUNT_ROW_ID=b.ROW_ID 
                            inner join SIEBEL_ACCOUNT_OWNER c on c.ACCOUNT_ROW_ID = a.ACCOUNT_ROW_ID 
                            inner join SIEBEL_POSITION d on d.ROW_ID = c.POSITION_ID 
							inner join SIEBEL_CONTACT e on d.CONTACT_ID = e.ROW_ID 
                            where a.ACCOUNT_ROW_ID in (
                            select ROW_ID from SIEBEL_ACCOUNT (nolock)
                            where PARENT_ROW_ID='{0}' AND (a.NAME LIKE N'%{1}%')) 
                            and e.EMAIL_ADDRESS = '{2}' 
							order by orderbystr, a.CREATED desc", Account_ID, _OptyName, userEmail);
            DataTable dt = SqlProvider.dbGetDataTable("MY", str);
            return dt;
        }

        public static List<string> GetAccountOwners(string accountRowId)
        {
            List<String> result = new List<String>();
            String str = String.Format(@"SELECT distinct(b.EMAIL_ADDR) as AccountOwner FROM [SIEBEL_ACCOUNT_OWNER] a inner join SIEBEL_POSITION b on a.POSITION_ID = b.ROW_ID
                            where a.ACCOUNT_ROW_ID = '{0}'", accountRowId);
            DataTable dt = SqlProvider.dbGetDataTable("MY", str);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow d in dt.Rows)
                {
                    if (d["AccountOwner"] != null && !String.IsNullOrEmpty(d["AccountOwner"].ToString()))
                        result.Add(d["AccountOwner"].ToString());
                }
            }

            return result;

        }
        //public static DataTable getContactList(string Account)
        //{
        //    var Contact = new DataTable();
        //    if (!string.IsNullOrEmpty(Account))
        //    {
        //        string _sql = "select a.ROW_ID,(isnull(FirstName,'') + ' ' + isnull(LastName,'')) as Name from Siebel_Contact a ";
        //        _sql += " inner join SIEBEL_ACCOUNT b on a.ACCOUNT_ROW_ID = b.ROW_ID ";
        //        _sql += string.Format(" where b.ROW_ID = '{0}' ", Account.Trim().Replace("'", "''"));
        //        Contact = DBUtil.dbGetDataTable("MY", _sql);
        //    }
        //    return Contact;
        //}

        public static List<String> GetRBUList(String _MasterRBU)
        {
            List<String> result = new List<String>();

            DataTable dt = SqlProvider.dbGetDataTable("EQ", String.Format("select distinct SIEBEL_RBU from LEADSFLASHRBU_SIEBELRBU where FLASHLEADS_RBU = '{0}'", _MasterRBU));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow d in dt.Rows)
                {
                    if (d["SIEBEL_RBU"] != null && !String.IsNullOrEmpty(d["SIEBEL_RBU"].ToString()))
                        result.Add(d["SIEBEL_RBU"].ToString());
                }
            }
            else
            {
                result.Add(_MasterRBU);
            }

            return result;
        }

        public static String GetMasterRBU(String _SiebelRBU)
        {

            DataTable dt = SqlProvider.dbGetDataTable("EQ", String.Format("select TOP 1 * from LEADSFLASHRBU_SIEBELRBU where SIEBEL_RBU = '{0}'", _SiebelRBU));
            if (dt != null && dt.Rows.Count > 0)
                    if (dt.Rows[0]["FLASHLEADS_RBU"] != null && !String.IsNullOrEmpty(dt.Rows[0]["FLASHLEADS_RBU"].ToString()))
                        return dt.Rows[0]["FLASHLEADS_RBU"].ToString();
            return "";
        }


        public static String GetCorrectCurrency(String _Currency)
        {
            String result = String.Empty;
            switch (_Currency.ToUpper().Trim())
            {
                case "EURO":
                case "FRF":
                case "PTE":
                case "EUR":
                    result = "EUR";
                    break;

                case "ILS":
                case "INR":
                case "USD":
                    result = "USD";
                    break;

                case "NTD":
                case "TWD":
                    result = "TWD";
                    break;

                case "RMB(￥)":
                case "RMB":
                case "CNY":
                    result = "CNY";
                    break;

                case "YEN":
                case "JPY":
                    result = "JPY";
                    break;

                default:           
                    result = "USD";
                    break;
            }

            return result;
        }

    }
}