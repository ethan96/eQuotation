using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;

namespace Advantech.Myadvantech.DataAccess
{
    public class eQuotationDAL
    {

        //public static decimal get_exchangerate(string CurrencyFrom, string CurrencyTo)
        //{
        //    if (string.Equals(CurrencyFrom, CurrencyTo)) return 1;

        //    object rate = SqlProvider.dbExecuteScalar("MY", string.Format("select top 1 UKURS from SAP_EXCHANGERATE where fCURR='{0}' and TCURR='{1}' order by exch_date desc", CurrencyFrom, CurrencyTo));
        //    if (rate == null) return 1;
        //    decimal _rate = 1;
        //    decimal.TryParse(rate.ToString(), out _rate);
        //    return _rate;
        //}
        public static string getTimeZoneName(string ORG)
        {
            object timezone = SqlProvider.dbExecuteScalar("MY", string.Format("select top 1 isnull(timezonename,'') as timezonename from TIMEZONE where org like '%{0}'", ORG));
            if (timezone == null) return string.Empty;
            return timezone.ToString();
        }
        public static DataTable getGP_Parameter(string RBU)
        {
            DataTable dt = SqlProvider.dbGetDataTable("EQ", string.Format("SELECT [RBU] ,isnull(GP_Percent,0) as [GP_Percent] ,isnull(Approver,'') as [Approver]  FROM [dbo].[GP_Parameters] WHERE [RBU]='{0}' Order by GP_Percent Desc, SEQ", RBU));
            return dt;
        }

        public static DataTable getGP_ParameterByMargin(string RBU,decimal margin)
        {
            DataTable dt = SqlProvider.dbGetDataTable("EQ", string.Format("SELECT [RBU] ,isnull(GP_Percent,0) as [GP_Percent] ,isnull(Approver,'') as [Approver]  FROM [dbo].[GP_Parameters] WHERE [RBU]='{0}' and GP_Percent>={1} Order by GP_Percent Desc, SEQ", RBU, margin));
            return dt;
        }


        /// <summary>
        /// Get AKR's GP approver by sales eamil
        /// </summary>
        /// <param name="Sales_email"></param>
        /// <returns></returns>
        public static String getAKRGP_Approver(string Sales_email)
        {
            DataTable dt = SqlProvider.dbGetDataTable("EQ", string.Format("SELECT Sales_email,isnull(Report_to,'') as [Approver] FROM [dbo].[AKR_Sales_List] WHERE [Sales_email]='{0}'", Sales_email));
            String approver = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                approver = dt.Rows[0]["Approver"].ToString();
            }

            return approver;
        }
        
        
        
        public static bool LogQuote2Order(string OrderID, string QuoteID, ref string Msg)
        {

            if (string.IsNullOrEmpty(OrderID) || string.IsNullOrEmpty(QuoteID)) return false;
            StringBuilder sql = new StringBuilder();
            sql.Append("  select   top 1 ORDER_ID, ORDER_NO,PO_NO,CREATED_DATE,CREATED_BY ");
            sql.Append(" from  ORDER_MASTER where ORDER_ID =@ORDER_ID ");
            IDbDataParameter dp = null;
            IDbConnection connMY = DatabaceFactory.CreateConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString, DatabaseType.SQLServer);
            IDbCommand cmd = DatabaceFactory.CreateCommand(sql.ToString(), DatabaseType.SQLServer, connMY);
            dp = cmd.CreateParameter();
            dp.ParameterName = "ORDER_ID";
            dp.Value = OrderID;
            cmd.Parameters.Add(dp);
            DbDataAdapter da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                sql.Clear();
                sql.Append(" INSERT INTO [QUOTE_TO_ORDER_LOG] ([QUOTEID] ,[SO_NO],[PO_NO] ,[ORDER_DATE] ,[ORDER_BY]) ");
                sql.Append(" VALUES ( @QUOTEID ,@SO_NO,@PO_NO ,@ORDER_DATE ,@ORDER_BY)  ");
                IDbConnection connEQ = DatabaceFactory.CreateConnection(ConfigurationManager.ConnectionStrings["EQ"].ConnectionString, DatabaseType.SQLServer);
                cmd = DatabaceFactory.CreateCommand(sql.ToString(), DatabaseType.SQLServer, connEQ);
                dp = cmd.CreateParameter(); dp.ParameterName = "QUOTEID"; dp.Value = QuoteID; cmd.Parameters.Add(dp);
                dp = cmd.CreateParameter(); dp.ParameterName = "SO_NO"; dp.Value = dt.Rows[0]["ORDER_NO"] ?? ""; cmd.Parameters.Add(dp);
                dp = cmd.CreateParameter(); dp.ParameterName = "PO_NO"; dp.Value = dt.Rows[0]["PO_NO"] ?? ""; cmd.Parameters.Add(dp);
                dp = cmd.CreateParameter(); dp.ParameterName = "ORDER_DATE"; dp.Value = dt.Rows[0]["CREATED_DATE"] ?? ""; cmd.Parameters.Add(dp);
                dp = cmd.CreateParameter(); dp.ParameterName = "ORDER_BY"; dp.Value = dt.Rows[0]["CREATED_BY"] ?? ""; cmd.Parameters.Add(dp);
                int retInt = -1;
                connEQ.Open();
                try
                {
                    retInt = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Msg = ex.ToString();
                }
                finally
                {
                    connEQ.Close();
                    connMY.Close();
                }
                if (retInt > 0) return true;

            }
            return false;
        }
        public string getProductType(string PartNO)
        {
            IDbConnection connMY = DatabaceFactory.CreateConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString, DatabaseType.SQLServer);
            string sql = string.Format("select top 1 isnull(PRODUCT_TYPE,'')  as PRODUCT_TYPE  from  SAP_PRODUCT where PART_NO =@PART_NO");
            IDbCommand cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
            IDbDataParameter dp = cmd.CreateParameter(); dp.ParameterName = "PART_NO"; dp.Value = PartNO; cmd.Parameters.Add(dp);
            connMY.Open();
            object retObj = null;
            try
            {
                retObj = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            { }
            finally
            {
                connMY.Close();
            }
            if (retObj != null && !string.IsNullOrEmpty(retObj.ToString()))
            {
                return retObj.ToString().Trim();
            }
            return string.Empty;
        }
        public void getOfficeGroupByEripIdOrg(string AccountRowId, string EripId, string org, out string officecode, out string groupcode, ref string Msg)
        {
            officecode = string.Empty; groupcode = string.Empty; EripId = EripId.Trim().ToUpper();
            DataTable dt = new DataTable();
            IDbConnection connMY = DatabaceFactory.CreateConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString, DatabaseType.SQLServer);
            IDbConnection connEQ = DatabaceFactory.CreateConnection(ConfigurationManager.ConnectionStrings["EQ"].ConnectionString, DatabaseType.SQLServer);
            if (!string.IsNullOrEmpty(EripId))
            {
                string sql = string.Format("SELECT top 1 isnull(SalesOffice,'') as OfficeCode,  isnull(SalesGroup,'') as GroupCode from sap_dimcompany where org_ID='{0}' AND company_id='{1}'", org, EripId);
                IDbCommand cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
                DbDataAdapter da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
                connMY.Open();
                da.Fill(dt);
                if (dt != null && dt.Rows.Count == 1)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["OfficeCode"].ToString()))
                    {
                        officecode = dt.Rows[0]["OfficeCode"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["GroupCode"].ToString()))
                    {
                        groupcode = dt.Rows[0]["GroupCode"].ToString();
                    }

                }
                connMY.Close();
            }

            if (string.IsNullOrEmpty(officecode) || string.IsNullOrEmpty(groupcode))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT  TOP 1 a.ROW_ID, ISNULL(b.ATTRIB_05, N'') AS ERP_ID, a.NAME AS ACCOUNT_NAME, ");
                sb.Append(" a.CUST_STAT_CD AS ACCOUNT_STATUS, ISNULL(a.MAIN_FAX_PH_NUM, N'') AS FAX_NUM, ");
                sb.Append(" ISNULL(a.MAIN_PH_NUM, N'') AS PHONE_NUM, ISNULL(a.OU_TYPE_CD, N'') AS OU_TYPE_CD, ISNULL(a.URL, N'') ");
                sb.Append(" AS URL, ISNULL(b.ATTRIB_34, N'') AS BusinessGroup, ISNULL(a.OU_TYPE_CD, N'') AS ACCOUNT_TYPE, ");
                sb.Append(" ISNULL(c.NAME, N'') AS RBU, ISNULL");
                sb.Append(" ((SELECT    EMAIL_ADDR");
                sb.Append(" FROM  S_CONTACT");
                sb.Append(" WHERE  (ROW_ID IN");
                sb.Append(" (SELECT   PR_EMP_ID");
                sb.Append(" FROM   S_POSTN");
                sb.Append(" WHERE  (ROW_ID IN");
                sb.Append(" (SELECT PR_POSTN_ID");
                sb.Append(" FROM   S_ORG_EXT");
                sb.Append(" WHERE   (ROW_ID = a.ROW_ID)))))), N'') AS PRIMARY_SALES_EMAIL, ");
                sb.Append(" a.PAR_OU_ID AS PARENT_ROW_ID, ISNULL(b.ATTRIB_09, N'N') AS MAJORACCOUNT_FLAG, ISNULL(a.CMPT_FLG, ");
                sb.Append("  N'N') AS COMPETITOR_FLAG, ISNULL(a.PRTNR_FLG, N'N') AS PARTNER_FLAG, ISNULL(d.COUNTRY, N'') ");
                sb.Append(" AS COUNTRY, ISNULL(d.CITY, N'') AS CITY, ISNULL(d.ADDR, N'') AS ADDRESS, ISNULL(d.STATE, N'') AS STATE, ");
                sb.Append(" ISNULL(d.ZIPCODE, N'') AS ZIPCODE, ISNULL(d.PROVINCE, N'') AS PROVINCE, ISNULL");
                sb.Append(" ((SELECT  TOP (1) NAME");
                sb.Append(" FROM  S_INDUST");
                sb.Append(" WHERE   (ROW_ID = a.X_ANNIE_PR_INDUST_ID)), N'N/A') AS BAA, b.CREATED, ");
                sb.Append(" b.LAST_UPD AS LAST_UPDATED, ISNULL");
                sb.Append(" ((SELECT TOP (1) e.NAME");
                sb.Append(" FROM   S_PARTY AS e INNER JOIN");
                sb.Append(" S_POSTN AS f ON e.ROW_ID = f.OU_ID");
                sb.Append(" WHERE  (f.ROW_ID IN");
                sb.Append(" (SELECT   PR_POSTN_ID");
                sb.Append(" FROM   S_ORG_EXT AS S_ORG_EXT_2");
                sb.Append(" WHERE   (ROW_ID = a.ROW_ID)))), N'') AS PriOwnerDivision, ");
                sb.Append(" a.PR_POSTN_ID AS PriOwnerRowId, ISNULL");
                sb.Append(" ((SELECT   TOP (1) NAME");
                sb.Append(" FROM   S_POSTN AS f");
                sb.Append(" WHERE (ROW_ID IN");
                sb.Append(" (SELECT PR_POSTN_ID");
                sb.Append(" FROM S_ORG_EXT AS S_ORG_EXT_1");
                sb.Append(" WHERE   (ROW_ID = a.ROW_ID)))), N'') AS PriOwnerPosition, CAST('' AS nvarchar(10)) ");
                sb.Append(" AS LOCATION, CAST('' AS nvarchar(10)) AS ACCOUNT_TEAM, ISNULL(d.ADDR_LINE_2, N'') AS ADDRESS2, ");
                sb.Append(" ISNULL(b.ATTRIB_36, N'') AS ACCOUNT_CC_GRADE, ISNULL(a.BASE_CURCY_CD, N'') AS CURRENCY");
                sb.Append(" FROM  S_ORG_EXT AS a LEFT OUTER JOIN");
                sb.Append(" S_ORG_EXT_X AS b ON a.ROW_ID = b.ROW_ID LEFT OUTER JOIN");
                sb.Append(" S_PARTY AS c ON a.BU_ID = c.ROW_ID LEFT OUTER JOIN");
                sb.Append(" S_ADDR_ORG AS d ON a.PR_ADDR_ID = d.ROW_ID");
                sb.Append(" WHERE          (a.ROW_ID = @RID)");
                IDbDataParameter dp = null;
                IDbConnection connCRM = DatabaceFactory.CreateConnection(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString, DatabaseType.SQLServer);
                IDbCommand cmd = DatabaceFactory.CreateCommand(sb.ToString(), DatabaseType.SQLServer, connCRM);
                dp = cmd.CreateParameter(); dp.ParameterName = "RID"; dp.Value = AccountRowId; cmd.Parameters.Add(dp);
                connCRM.Open();
                DbDataAdapter da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
                dt.Clear();
                da.Fill(dt);
                if (dt != null && dt.Rows.Count == 1)
                {
                    //'JJ 2014/2/13：Company_id如果空白時需要Office和Sales Group因為gpblock_logic這個table需要知道office和sales Group才能知道要appove給誰
                    //'由Sales的Emal去SIEBEL_POSITION的PRIMARY_POSITION_NAME查出字串如：AFR/Embedded/AOnline-eSales-SEU-Agnes_Iglesias，PRIMARY_POSITION_NAME 鎖定Embedded或iA
                    //'再由字串split(1)分出來Sales Group是EC Aonline(Embedded是EC Aonline、iA是iA Aonline)
                    //'由SIEBEL_POSITION  join SIEBEL_CONTACT，再由SIEBEL_CONTACT的OrgID就能知道office
                    //'如果查不到就由317(EC Aonline)的Fei.Khong@advantech.com和Ween.Niu@advantech.com來appove
                    string UserEmail = HttpContext.Current.User.Identity.Name;
                    string sql = string.Format("SELECT TOP 1 a.PRIMARY_POSITION_NAME + ',' + b.OrgID FROM SIEBEL_POSITION a left join SIEBEL_CONTACT b on a.CONTACT_ID = b.ROW_ID WHERE a.EMAIL_ADDR =@UserEmail and(a.PRIMARY_POSITION_NAME like '%Embedded%' or a.PRIMARY_POSITION_NAME like '%iA%')");
                    cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
                    dp = cmd.CreateParameter(); dp.ParameterName = "UserEmail"; dp.Value = UserEmail; cmd.Parameters.Add(dp);
                    connMY.Open();
                    object retObj = null;
                    try
                    {
                        retObj = cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    { Msg += ex.ToString(); }
                    finally
                    {
                        connMY.Close();
                    }
                    //例：ABN/iA/AOnline-Manager-NEU-Wenyu_Lai,AUK
                    if ((retObj != null) && !string.IsNullOrEmpty(retObj.ToString()))
                    {
                        string sOffice = retObj.ToString().Split(new char[] { ',' })[1];
                        //例：AUK
                        string sGroup = retObj.ToString().Split(new char[] { ',' })[0].Split(new char[] { '/' })[1];
                        //例：iA
                        //轉換Sales Group Name
                        if (sGroup == "Embedded")
                        {
                            sGroup = "EC Aonline";
                        }
                        else if (sGroup == "iA")
                        {
                            sGroup = "iA Aonline";
                        }

                        //找出office code

                        sql = "SELECT TOP 1 OFFICE_CODE from gpblock_logic where OFFICE_NAME=@sOffice and active=1 AND TYPE='GP' and group_name=@sGroup";
                        cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connEQ);
                        dp = cmd.CreateParameter(); dp.ParameterName = "sOffice"; dp.Value = sOffice; cmd.Parameters.Add(dp);
                        IDbDataParameter dpgroup = cmd.CreateParameter(); dpgroup.ParameterName = "sGroup"; dpgroup.Value = sGroup; cmd.Parameters.Add(dpgroup);
                        connEQ.Open();
                        retObj = null;
                        try
                        {
                            retObj = cmd.ExecuteScalar();
                        }
                        catch (Exception ex)
                        { Msg += ex.ToString(); }
                        finally
                        {
                            connEQ.Close();
                        }
                        if ((retObj != null) && !string.IsNullOrEmpty(retObj.ToString()))
                        {
                            officecode = retObj.ToString();

                            //找出Group code
                            sql = "SELECT TOP 1 GROUP_CODE from gpblock_logic where OFFICE_NAME=@sOffice and active=1 AND TYPE='GP' and group_name=@sGroup";
                            cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connEQ);
                            dp = cmd.CreateParameter(); dp.ParameterName = "sOffice"; dp.Value = sOffice; cmd.Parameters.Add(dp);
                            dpgroup = cmd.CreateParameter(); dpgroup.ParameterName = "sGroup"; dpgroup.Value = sGroup; cmd.Parameters.Add(dpgroup);
                            connEQ.Open();
                            retObj = null;
                            try
                            {
                                retObj = cmd.ExecuteScalar();
                            }
                            catch (Exception ex)
                            { Msg += ex.ToString(); }
                            finally
                            {
                                connEQ.Close();
                            }
                            ///////  
                            if ((retObj != null) && !string.IsNullOrEmpty(retObj.ToString()))
                            {
                                groupcode = retObj.ToString();
                            }
                            else
                            {
                                groupcode = "317";
                                //找不到就都帶317 (EC Aonline)
                            }
                        }
                        else
                        {
                            //都沒有就由Account RBU去查office code，而group code就固定是317 (EC Aonline)
                            object R = new object();
                            sql = "SELECT TOP 1 OFFICE_CODE from gpblock_logic where OFFICE_NAME=@OFFICE_NAME and active=1 AND TYPE='GP' and group_name='317'";
                            cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connEQ);
                            dp = cmd.CreateParameter(); dp.ParameterName = "OFFICE_NAME"; dp.Value = dt.Rows[0]["RBU"]; cmd.Parameters.Add(dp);
                            connEQ.Open();
                            try
                            {
                                R = cmd.ExecuteScalar();
                            }
                            catch (Exception ex)
                            { }
                            finally
                            {
                                connEQ.Close();
                            }

                            if ((R != null) && !string.IsNullOrEmpty(R.ToString()))
                            {
                                officecode = R.ToString();
                                groupcode = "317";  //找不到就都帶317 (EC Aonline)
                            }
                        }

                    }

                }
                connCRM.Close();
            }

            //JJ 2014/2/25 都找不到預設就帶ADL

            if (string.IsNullOrEmpty(officecode))
            {
                officecode = "3000";
            }

            //JJ 2014/2/25 都找不到預設就帶EC Aonline
            if (string.IsNullOrEmpty(groupcode))
            {
                groupcode = "317";
            }



        }

        public IEnumerable<GPBLOCK_LOGIC> getApproverList(string officecode, string groupcode, ref string Msg)
        {
            ///get Approver List
            IQueryable<GPBLOCK_LOGIC> _ApprovalList = eQuotationContext.Current.GPBLOCK_LOGIC.Where(p => p.group_code == groupcode && p.Office_code == officecode && p.Active == 1 && p.Type == "GP").OrderByDescending(p => p.gp_level);
            return _ApprovalList.AsEnumerable();
            ///
        }
        public bool InitQuotationApprovalExpirationFlow(string QuoteID, ref string msg)
        {
            QuotationMaster _QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            if (_QuotationMaster.QuoteExtensionX.ApprovalFlowTypeX == eQApprovalFlowType.ThirtyDaysExpiration)
            {

                string officecode; string groupcode;
                eQuotationDAL _eQuotationDAL = new eQuotationDAL();
                getOfficeGroupByEripIdOrg(_QuotationMaster.quoteToRowId, _QuotationMaster.quoteToErpId, _QuotationMaster.org, out officecode, out groupcode, ref msg);
                IEnumerable<GPBLOCK_LOGIC> _ApproverList = getApproverList(officecode, groupcode, ref msg);
                if (_ApproverList != null)
                {
                    eQuotationContext.Current.Database.ExecuteSqlCommand(string.Format("delete from  Quotation_Approval_Expiration where quoteID='{0}'", QuoteID));
                    //eQuotationContext.Current.SaveChanges();
                    int i = 1;
                    foreach (GPBLOCK_LOGIC gp in _ApproverList)
                    {
                        Quotation_Approval_Expiration qae = new Quotation_Approval_Expiration();
                        qae.QuoteID = QuoteID;
                        qae.GPlever = gp.gp_level;
                        qae.Approver = gp.approver;
                        qae.ApprovalLevel = i;
                        qae.Status = 0;
                        qae.MobileApproveYes = System.Guid.NewGuid().ToString();
                        qae.MobileApproveNo = System.Guid.NewGuid().ToString();
                        qae.ApprovalDate = null;
                        qae.ApprovalFrom = "";
                        qae.ApprovalFlowType = "ThirtyDaysExpiration";
                        qae.CreatedDate = DateTime.Now;
                        i++;
                        eQuotationContext.Current.Entry(qae).State = System.Data.Entity.EntityState.Added;
                    }
                    eQuotationContext.Current.SaveChanges();
                }


            }
            return true;
        }
        public IEnumerable<Quotation_Approval_Expiration> getQuotationApprovalExpiration(string QuoteID)
        {
            IEnumerable<Quotation_Approval_Expiration> _QAEList = eQuotationContext.Current.Quotation_Approval_Expiration.Where(p => p.QuoteID == QuoteID).OrderBy(p => p.ApprovalLevel.Value);
            return _QAEList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="QuoteID"></param>
        /// <returns></returns>
        public static bool DeleteQuotationDetail_Extension_ABR(string QuoteID)
        {
            try
            {
                eQuotationContext.Current.Database.ExecuteSqlCommand(string.Format("DELETE FROM  QuotationDetail_Extension_ABR WHERE quoteId ='{0}'", QuoteID));
                return true;
            }
            catch (Exception)
            {


            }
            return false;

        }


        public static bool updateQuoteSiebelQuote(string QuoteID, string SiebleQuoteRowID)
        {
            try
            {
                eQuotationContext.Current.Database.ExecuteSqlCommand(string.Format("DELETE FROM  quoteSiebelQuote WHERE quoteId ='{0}'; insert quoteSiebelQuote (quoteId,siebelQuoteId) values (N'{0}',N'{1}')", QuoteID, SiebleQuoteRowID));
                return true;
            }
            catch (Exception)
            {


            }
            return false;
        }
        /// <summary>
        /// updateOptyQuote
        /// </summary>
        /// <param name="QuoteID">QuoteID</param>
        /// <param name="optyId">optyId</param>
        /// <param name="optyName">optyName</param>
        /// <param name="optyStage">optyStage</param>
        /// <param name="msg">return msg</param>
        /// <returns></returns>
        public static bool updateOptyQuote(string QuoteID, string optyId, string optyName, string optyStage, ref string msg)
        {
            bool retbool = false;
            optyQuote _optyQuote = eQuotationContext.Current.optyQuote.FirstOrDefault(p => p.quoteId == QuoteID);
            if (_optyQuote != null)
            {
                _optyQuote.optyId = optyId;
                _optyQuote.optyName = optyName;
                _optyQuote.optyStage = optyStage;
                try
                {
                    eQuotationContext.Current.SaveChanges();
                    retbool = true;
                }
                catch (Exception ex)
                {
                    msg = ex.ToString();
                }
            }
            else
            {
                msg = "Did't find the QuoteID";
            }
            return retbool;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quote_id"></param>
        /// <returns></returns>
        public List<EQPARTNER> GetEQPartnerByQuoteID(String quote_id)
        {
            return eQuotationContext.Current.EQPARTNER.Where(d => d.QUOTEID == quote_id).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quote_id"></param>
        /// <returns></returns>
        public List<QuotationMaster> GetQuoteMasterByQuoteID(String quote_id)
        {
            return eQuotationContext.Current.QuotationMaster.Where(d => d.quoteId == quote_id).ToList();
        }

        public List<QuotationDetail> GetQuoteDetailByQuoteID(String quote_id)
        {
            return eQuotationContext.Current.QuotationDetail.Where(d => d.quoteId == quote_id).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quote_id"></param>
        /// <returns></returns>
        public static QuotationExtension GetQuoteMasterExtendionByQuoteID(String quote_id)
        {
            return eQuotationContext.Current.QuotationExtension.Where(d => d.QuoteID == quote_id).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quote_id"></param>
        /// <returns></returns>
        public static List<EQPARTNER> GetQuotePartnerByQuoteID(String quote_id)
        {
            return eQuotationContext.Current.EQPARTNER.Where(d => d.QUOTEID == quote_id).ToList();
        }



        /// <summary>
        /// updateQuoteSiebelQuote
        /// </summary>
        /// <param name="QuoteID"></param>
        /// <param name="optyId"></param>
        /// <param name="optyName"></param>
        /// <param name="optyStage"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        //public static bool updateQuoteSiebelQuote(string QuoteID, string SiebleQuoteID,  ref string msg)
        //{
        //    bool retbool = false;
        //    quoteSiebelQuote _quoteSiebelQuote = eQuotationContext.Current.quoteSiebelQuote.FirstOrDefault(p => p.quoteId == QuoteID);
        //    if (_quoteSiebelQuote != null)
        //    {
        //        _quoteSiebelQuote.siebelQuoteId = SiebleQuoteID;
        //    }
        //    else
        //    {
        //        _quoteSiebelQuote = new quoteSiebelQuote();
        //        _quoteSiebelQuote.quoteId = QuoteID;
        //        _quoteSiebelQuote.siebelQuoteId = SiebleQuoteID;
        //        eQuotationContext.Current.quoteSiebelQuote.Add(_quoteSiebelQuote);
        //    }
        //    try
        //    {
        //        eQuotationContext.Current.SaveChanges();
        //        retbool = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.ToString();
        //    }

        //    return retbool;
        //}

        public static AJP_ConfiguratorTerms Get_AJPTermsRecord(String QuoteID)
        {
            return eQuotationContext.Current.AJP_ConfiguratorTerms.Where(d => d.QuoteID == QuoteID).FirstOrDefault();
        }

        public static List<EQPARTNER> GetEQPartner(String QuoteID)
        {
            return eQuotationContext.Current.EQPARTNER.Where(d => d.QUOTEID == QuoteID).ToList();
        }

        public static string GetRegionParameterValue(string region, string sector, string siteParameter, string defaultValue)
        {
            var parameter =  eQuotationContext.Current.RegionSectorParameter.Where(p => p.Region == region && p.Sector == sector && p.SiteParameter == siteParameter ).FirstOrDefault();
            if (parameter != null)
                return parameter.ParaValue;
            else
                return defaultValue;

        }

        public static List<ACN_EQ_Sales> GetACNSalesApproversBySector(string sector)
        {

            var salesApprovers = new List<ACN_EQ_Sales>();


            salesApprovers = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.Sector == sector).ToList();


            return salesApprovers;
        }

        public static bool DeleteACNSalesApproversBySalesCode(string salesCode, ref string msg)
        {
            bool retbool = false;
            var salesApprovers = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesCode == salesCode);
            
            try
            {
                eQuotationContext.Current.ACN_EQ_Sales.RemoveRange(salesApprovers);
                eQuotationContext.Current.SaveChanges();
                retbool = true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return retbool;
        }

        public static bool CreateOrUpdateACNSalesApprovers(List<ACN_EQ_Sales> salesApprovers, ref string msg)
        {
            bool retbool = false;
            var salesCode = salesApprovers.FirstOrDefault().SalesCode;
            var existedSalesApprovers = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesCode == salesCode);

            try
            {
                
                //Delete 
                foreach (var existedApprover in existedSalesApprovers)
                {
                    if (!salesApprovers.Any(a => a.Id == existedApprover.Id))
                    {
                        eQuotationContext.Current.ACN_EQ_Sales.Remove(existedApprover);
                    }
                }


                foreach (var approver in salesApprovers)
                {


                    //var existedSalesApprover = existedSalesApprovers.FirstOrDefault(s => s.Level == approver.Level);
                    if (approver.Id == 0)
                    {
                        eQuotationContext.Current.ACN_EQ_Sales.Add(approver);
                    }
                    else
                    {
                        var existedSalesApprover = eQuotationContext.Current.ACN_EQ_Sales.FirstOrDefault(s => s.Id == approver.Id);
                        if (existedSalesApprover != null)
                        {
                            existedSalesApprover.SalesCode = approver.SalesCode;
                            existedSalesApprover.IdSBU = approver.IdSBU;
                            existedSalesApprover.SalesEmail = approver.SalesEmail;
                            existedSalesApprover.Sector = approver.Sector;
                            existedSalesApprover.ApproverEmail = approver.ApproverEmail;
                            existedSalesApprover.Level = approver.Level;
                        }
                    }
                    

                }

                eQuotationContext.Current.SaveChanges();

                retbool = true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return retbool;
        }

        public static bool CreateOrUpdateACNPSMs(ACN_EQ_PSM psm, ref string msg)
        {
            bool retbool = false;
            //var salesCode = salesApprovers.FirstOrDefault().SalesCode;

            try
            {


                //foreach (var psm in psms)
                //{

                //    if (psm.Id == 0)
                //    {
                //        eQuotationContext.Current.ACN_EQ_PSM.Add(psm);
                //    }
                //    else
                //    {
                //        var existedPSM = eQuotationContext.Current.ACN_EQ_PSM.FirstOrDefault(s => s.Id == psm.Id);
                //        if (existedPSM != null)
                //        {
                //            existedPSM.PG = psm.PG;
                //            existedPSM.ProductDivision = psm.ProductDivision;
                //            existedPSM.Sector = psm.Sector;
                //            existedPSM.PSM = psm.PSM;
                //            existedPSM.Level = psm.Level;
                //        }
                //    }


                //}

                if (psm.Id == 0)
                {
                    eQuotationContext.Current.ACN_EQ_PSM.Add(psm);
                }
                else
                {
                    var existedPSM = eQuotationContext.Current.ACN_EQ_PSM.FirstOrDefault(s => s.Id == psm.Id);
                    if (existedPSM != null)
                    {
                        existedPSM.PG = psm.PG;
                        existedPSM.ProductDivision = psm.ProductDivision;
                        existedPSM.Sector = psm.Sector;
                        existedPSM.PSM = psm.PSM;
                        existedPSM.Level = psm.Level;
                    }
                }

                eQuotationContext.Current.SaveChanges();

                retbool = true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return retbool;
        }

        public static bool DeleteACNPSMById(int id, ref string msg)
        {
            bool retbool = false;
            var psm = eQuotationContext.Current.ACN_EQ_PSM.FirstOrDefault(s => s.Id == id);

            try
            {
                eQuotationContext.Current.ACN_EQ_PSM.Remove(psm);
                eQuotationContext.Current.SaveChanges();
                retbool = true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return retbool;
        }
    }
}
