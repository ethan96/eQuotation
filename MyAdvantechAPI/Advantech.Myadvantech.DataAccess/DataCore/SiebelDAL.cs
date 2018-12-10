using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Caching;

namespace Advantech.Myadvantech.DataAccess
{
    public class SiebelDAL
    {
        static int _SiebelWSTimeOut = 60000;

        //        Shared Function getSiebelLoginNameByRowid(ByVal rowId As String) As String
        //    Dim Str As String = String.Format("select TOP 1 LOGIN from S_USER WHERE ROW_ID='{0}' AND UPPER(LOGIN) not like 'DELETE%'", rowId)
        //    'Try
        //    Dim LOGIN As String = tbOPBase.dbExecuteScalar("CRM", Str)

        //    'Frank 2013/10/17
        //    If String.IsNullOrEmpty(LOGIN) Then Return ""

        //    Return LOGIN.ToString
        //    'Catch ex As Exception

        //    'End Try
        //    Return "MYADVANTECH"
        //End Function

        public static string GetSiebelLoginNameByRowid(String rowId)
        {
            string _sql = string.Format("select TOP 1 LOGIN from S_USER WHERE ROW_ID='{0}' AND UPPER(LOGIN) not like 'DELETE%'", rowId);
            object LOGIN = SqlProvider.dbExecuteScalar("CRM", _sql);
            if (LOGIN == null) { return ""; }
            if (string.IsNullOrEmpty((string)LOGIN)) { return ""; }
            return (string)LOGIN;
            //return "MYADVANTECH";
        }

        public static string GetSiebelPositionNameByPostnid(String PostnId)
        {
            string _sql = string.Format("select TOP 1 NAME from S_POSTN WHERE ROW_ID='{0}'", PostnId);
            object NAME = SqlProvider.dbExecuteScalar("CRM", _sql);
            if (NAME == null) { return ""; }
            if (string.IsNullOrEmpty((string)NAME)) { return ""; }
            return (string)NAME;
            //return "MYADVANTECH";
        }

        #region Siebel Account
        /// <summary>
        /// Update Siebel account web service
        /// </summary>
        /// <param name="accountRowID"></param>
        /// <param name="ErpID"></param>
        /// <returns></returns>
        public static string UpdateAccountErpID(string accountRowID, string ErpID)
        {
            //ICC 2016/5/27 Update Siebel account ERP ID
            //ICC 2016/12/23 Update Siebel account Web service
            string smtp = ConfigurationManager.AppSettings["SMTPServer"] ?? "172.20.0.76";
            SmtpClient smtpClient1 = new SmtpClient(smtp);
            MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,YL.Huang@advantech.com.tw");
            mail.Subject = "Add Siebel Account Failed in Adding Account";
            mail.IsBodyHtml = true;

            var strMeth = new WSSiebel_UpdAccount.ADVWebServiceUpdAccount();
            var strAccount = new WSSiebel_UpdAccount.ACC();
            var AccInput = new WSSiebel_UpdAccount.UpdAccount_Input();
            var AccOutput = new WSSiebel_UpdAccount.UpdAccount_Output();

            strAccount.ROW_ID = accountRowID; //It's Required.
            strAccount.ERPID = ErpID;

            AccInput.ACC = strAccount;
            AccInput.SOURCE = "MTL";  //It's Required.

            try
            {
                AccOutput = strMeth.UpdAccount(AccInput);
                string strStatus = AccOutput.STATUS;
                if (strStatus == "SUCCESS")
                {
                    mail.Subject = "Add Siebel Account Success in Adding Account";
                    mail.Body = string.Format("Update Siebel account success! Row ID: {0}", AccOutput.ROW_ID);
                    smtpClient1.Send(mail);
                    return AccOutput.ROW_ID;
                }
                else
                    mail.Body = string.Format("Update Siebel account failed! Error message: {0}, {1}", AccOutput.Error_spcCode, AccOutput.Error_spcMessage);

            }
            catch (Exception ex)
            {
                mail.Body = string.Format("Message: {0}<br />", ex.ToString());
            }
            smtpClient1.Send(mail);
            return string.Empty;
        }
        public static string CreateAccount(ref SiebelWebService.ACCOUNT account)
        {
            var ws = new SiebelWebService.WSSiebel();
            var emp = new SiebelWebService.EMPLOYEE();
            emp.USER_ID = ConfigurationManager.AppSettings["CRMHQId"];
            emp.PASSWORD = ConfigurationManager.AppSettings["CRMHQPwd"];
            var res = new SiebelWebService.RESULT();
            try
            {
                res = ws.AddAccount(emp, account);
                return res.ROW_ID;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        /// <summary>
        ///  根据appid创建Account
        /// </summary>
        /// <param name="ApplicationID"></param>
        /// <returns></returns>
        public static string CreateAccount(int ApplicationID)
        {
            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient("172.20.0.76");
            smtpClient1.Send("myadvantech@advantech.com", "ming.zhao@advantech.com.cn", "create : " + "SiebleAccount", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            var acc = new SiebelWebService.ACCOUNT();
            string strOwnerPriPositionName = string.Empty;
            string strRBU = string.Empty;
            // strOwnerPriPositionName = "ATW Marcom - Ada.Tang";

            MyAdminDAL _MyAdminDAL = new MyAdminDAL();
            SA_APPLICATION ap = _MyAdminDAL.getApplicationByID(ApplicationID);

            int _type = (int)companyType.SholdTo;
            SA_APPLICATION2COMPANY A2C = ap.SA_APPLICATION2COMPANY.FirstOrDefault(p => p.CompanyType == _type);
            strRBU = A2C.SiebelRBU;
            SA_KNA1 KNA1 = A2C.SA_KNA1.FirstOrDefault();
            SA_KNVV KNVV = A2C.SA_KNVV.FirstOrDefault();
            acc.ADDR_LINE1 = A2C.SA_BAPIADDR1.FirstOrDefault().Addr_No;
            acc.BAA = "Metals/Mining";
            acc.CITY = KNA1.Ort01;
            //  使用DatabaceFactory查找country_name
            IDbConnection connMY = DatabaceFactory.CreateConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString, DatabaseType.SQLServer);
            string sql = string.Format("select TOP 1 isnull(country_name,'') as  country_name  from SAP_DIMCOMPANY where COUNTRY =@COUNTRY");
            IDbCommand cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
            IDbDataParameter dp = cmd.CreateParameter(); dp.ParameterName = "COUNTRY"; dp.Value = KNA1.Land1; cmd.Parameters.Add(dp);
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
                acc.COUNTRY = retObj.ToString().Trim();
            }
            //
            string email = string.Empty;

            sql = string.Format("select top 1 isnull(EMAIL,'') as email  from dbo.SAP_EMPLOYEE where SALES_CODE =@SALES_CODE");
            cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
            dp = cmd.CreateParameter(); dp.ParameterName = "SALES_CODE"; dp.Value = A2C.SalesCode; cmd.Parameters.Add(dp);
            connMY.Open();
            retObj = null;
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
                email = retObj.ToString().Trim();
            }
            if (!string.IsNullOrEmpty(email))
            {

                sql = "select top 1 PRIMARY_POSITION_NAME from SIEBEL_POSITION  where EMAIL_ADDR=@EMAIL_ADDR and PRIMARY_POSITION_NAME is not null order by CREATED desc";
                cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
                dp = cmd.CreateParameter(); dp.ParameterName = "EMAIL_ADDR"; dp.Value = email; cmd.Parameters.Add(dp);
                connMY.Open();
                retObj = null;
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
                    strOwnerPriPositionName = retObj.ToString().Trim();
                }
            }
            if (string.IsNullOrEmpty(strOwnerPriPositionName))
            {
                sql = "select top 1 PRIMARY_POSITION_NAME from SIEBEL_POSITION where ROW_ID='1-2SUR45'";
                cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
                connMY.Open();
                retObj = null;
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
                    strOwnerPriPositionName = retObj.ToString().Trim();
                }
            }

            //
            acc.CURRENCY = KNVV.Waers;
            acc.MAIN_PHONE = KNA1.Telf1;
            acc.NAME = KNA1.Name1;
            acc.PRI_POS_NAME = strOwnerPriPositionName;
            acc.ORG = strRBU;
            acc.STATUS = "10-Sales Contact";
            acc.ERPID = KNA1.Kunnr;
            acc.IS_PARTNER = false;
            acc.URL = KNA1.Knurl;
            acc.MAIN_FAX = KNA1.Telfx;
            acc.ZIP = KNA1.Pstlz;
            acc.SITE = "";
            acc.TYPE = "";
            acc.STATUS = "07-General Account";
            acc.STATE = "";
            acc.INDUSTRY = new string[] { "" };
            return CreateAccount(ref acc);
        }

        public static string CreateAccount2(int ApplicationID)
        {
            try
            {
                System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient("172.20.0.76");
                smtpClient1.Send("myadvantech@advantech.com", "YL.Huang@advantech.com.tw", "create : " + "SiebleAccount", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                WSSiebel_AddAccount.ACC acc = new WSSiebel_AddAccount.ACC();
                string strOwnerPriPositionName = string.Empty;
                string strRBU = string.Empty;
                // strOwnerPriPositionName = "ATW Marcom - Ada.Tang";

                MyAdminDAL _MyAdminDAL = new MyAdminDAL();
                SA_APPLICATION ap = _MyAdminDAL.getApplicationByID(ApplicationID);

                int _type = (int)companyType.SholdTo;
                SA_APPLICATION2COMPANY A2C = ap.SA_APPLICATION2COMPANY.FirstOrDefault(p => p.CompanyType == _type);
                strRBU = A2C.SiebelRBU;
                SA_KNA1 KNA1 = A2C.SA_KNA1.FirstOrDefault();
                SA_KNVV KNVV = A2C.SA_KNVV.FirstOrDefault();

                //  使用DatabaceFactory查找country_name
                IDbConnection connMY = DatabaceFactory.CreateConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString, DatabaseType.SQLServer);
                string sql = string.Format("select TOP 1 isnull(country_name,'') as  country_name  from SAP_DIMCOMPANY where COUNTRY =@COUNTRY");
                IDbCommand cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
                IDbDataParameter dp = cmd.CreateParameter(); dp.ParameterName = "COUNTRY"; dp.Value = KNA1.Land1; cmd.Parameters.Add(dp);
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
                    WSSiebel_AddAccount.ADDR objADDR = new WSSiebel_AddAccount.ADDR();
                    objADDR.CITY = KNA1.Ort01;
                    objADDR.COUNTRY = retObj.ToString().Trim();
                    objADDR.LINE1 = A2C.SA_BAPIADDR1.FirstOrDefault().Addr_No;
                    objADDR.STATE = "";
                    objADDR.ZIP = KNA1.Pstlz;
                    acc.ADDR = (new List<WSSiebel_AddAccount.ADDR> { objADDR }).ToArray();
                }
                //
                string email = string.Empty;

                sql = string.Format("select top 1 isnull(EMAIL,'') as email  from dbo.SAP_EMPLOYEE where SALES_CODE =@SALES_CODE");
                cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
                dp = cmd.CreateParameter(); dp.ParameterName = "SALES_CODE"; dp.Value = A2C.SalesCode; cmd.Parameters.Add(dp);
                connMY.Open();
                retObj = null;
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
                    email = retObj.ToString().Trim();
                }
                if (!string.IsNullOrEmpty(email))
                {

                    sql = "select top 1 PRIMARY_POSITION_NAME from SIEBEL_POSITION  where EMAIL_ADDR=@EMAIL_ADDR and PRIMARY_POSITION_NAME is not null order by CREATED desc";
                    cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
                    dp = cmd.CreateParameter(); dp.ParameterName = "EMAIL_ADDR"; dp.Value = email; cmd.Parameters.Add(dp);
                    connMY.Open();
                    retObj = null;
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
                        strOwnerPriPositionName = retObj.ToString().Trim();
                    }
                }
                if (string.IsNullOrEmpty(strOwnerPriPositionName))
                {
                    sql = "select top 1 PRIMARY_POSITION_NAME from SIEBEL_POSITION where ROW_ID='1-2SUR45'";
                    cmd = DatabaceFactory.CreateCommand(sql, DatabaseType.SQLServer, connMY);
                    connMY.Open();
                    retObj = null;
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
                        strOwnerPriPositionName = retObj.ToString().Trim();
                    }
                }

                //
                acc.NAME = KNA1.Name1;
                acc.SITE = "";
                acc.STATUS = "07-General Account";
                acc.CURRENCY = KNVV.Waers;
                acc.DESC = "";
                acc.ERPID = KNA1.Kunnr;
                acc.URL = KNA1.Knurl;
                acc.MAIN_FAX = KNA1.Telfx;
                acc.MAIN_PHONE = KNA1.Telf1;
                acc.IS_PARTNER = "N";
                acc.TYPE = "";
                //acc.BUS_GROUP = "";
                //acc.TAXID = "";

                WSSiebel_AddAccount.BAA objBAA = new WSSiebel_AddAccount.BAA();
                objBAA.NAME = "Metals/Mining";
                acc.BAA = (new List<WSSiebel_AddAccount.BAA> { objBAA }).ToArray();

                WSSiebel_AddAccount.ORG objOrg = new WSSiebel_AddAccount.ORG();
                objOrg.NAME = strRBU;
                acc.ORG = (new List<WSSiebel_AddAccount.ORG> { objOrg }).ToArray();

                //WSSiebel_AddAccount.INDUSTRY objIndustry = new WSSiebel_AddAccount.INDUSTRY();
                //objIndustry.CODE = string.Empty;
                //objIndustry.IS_PRIMARY = string.Empty;
                //objIndustry.NAME = string.Empty;
                //acc.INDUSTRY = (new List<WSSiebel_AddAccount.INDUSTRY> { objIndustry }).ToArray();

                WSSiebel_AddAccount.POSITION objPosition = new WSSiebel_AddAccount.POSITION();
                objPosition.NAME = strOwnerPriPositionName;
                acc.POSITION = (new List<WSSiebel_AddAccount.POSITION> { objPosition }).ToArray();

                return CreateAccount2(ref acc);
            }
            catch (Exception ex)
            {
                string smtp = ConfigurationManager.AppSettings["SMTPServer"] != null ? ConfigurationManager.AppSettings["SMTPServer"] : "172.20.0.76";
                SmtpClient smtpClient1 = new SmtpClient(smtp);
                MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "MyAdvantech@advantech.com");
                mail.Subject = "Add Siebel Account Failed in Value Setting - exception";
                mail.IsBodyHtml = true;
                mail.Body = string.Format("Message: {0}<br />Exception: {1}", ex.Message, ex.ToString());
                smtpClient1.Send(mail);
                return "";
            }
        }

        public static string CreateAccount2(ref WSSiebel_AddAccount.ACC objACC)
        {
            try
            {
                var ws = new WSSiebel_AddAccount.ADVWebServiceAddAccount();
                var accInput = new WSSiebel_AddAccount.AddAccount_Input();
                accInput.ACC = objACC;
                accInput.SOURCE = "MTL";

                WSSiebel_AddAccount.AddAccount_Output accOutput = ws.AddAccount(accInput);
                //ICC 2016/5/27 Add error email
                if (accOutput.STATUS != "SUCCESS")
                {
                    string smtp = ConfigurationManager.AppSettings["SMTPServer"] != null ? ConfigurationManager.AppSettings["SMTPServer"] : "172.20.0.76";
                    SmtpClient smtpClient1 = new SmtpClient(smtp);
                    MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,YL.Huang@advantech.com.tw,rudy.wang@advantech.com.tw");
                    mail.Subject = "Add Siebel Account Failed in Adding Account";
                    mail.IsBodyHtml = true;
                    mail.Body = string.Format("Message: {0}, {1}, {2}<br />", accOutput.STATUS, accOutput.Error_spcCode, accOutput.Error_spcMessage);
                    smtpClient1.Send(mail);
                }
                return accOutput.ROW_ID;
            }
            catch (Exception ex)
            {
                string smtp = ConfigurationManager.AppSettings["SMTPServer"] != null ? ConfigurationManager.AppSettings["SMTPServer"] : "172.20.0.76";
                SmtpClient smtpClient1 = new SmtpClient(smtp);
                MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "MyAdvantech@advantech.com");
                mail.Subject = "Add Siebel Account Failed in Adding Account - exception";
                mail.IsBodyHtml = true;
                mail.Body = string.Format("Message: {0}<br />Exception: {1}", ex.Message, ex.ToString());
                smtpClient1.Send(mail);

                return "";
            }
        }
        public static SIEBEL_ACCOUNT getSiebelAccount(string account_row_id)
        {
            return MyAdvantechContext.Current.SIEBEL_ACCOUNT.Find(account_row_id);
        }

        /// <summary>
        /// Get Siebel Position (ID, Name) from SAP Company ID
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns>Distionary(Position ID, Position Name)</returns>
        public static Dictionary<string, string> GetSiebelPositionBySAPCompanyID(string CompanyID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("1-SPMFBX", "HQ Digital Marketing - Olivia.Chen");//預設給Olivia
            System.Data.SqlClient.SqlConnection connMy = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
            connMy.Open();
            try
            {

                var apt = new System.Data.SqlClient.SqlDataAdapter(
                                        @" with tmp_Employee as (
                                         select a.EMAIL,a.SALES_CODE,(select z.Data from dbo.Split(a.EMAIL, '@') z where z.ID = 1) as EMAIL_ID from SAP_EMPLOYEE a (nolock)
                                     ),
                                     POSTN as (
                                         select a.EMAIL_ADDR, (select z.Data from dbo.Split(a.EMAIL_ADDR, '@') z where z.ID = 1) as EMAIL_ID, a.ROW_ID, a.PRIMARY_POSITION_NAME from SIEBEL_POSITION a (nolock)
                                     )
                                     select top 1 d.ROW_ID,d.PRIMARY_POSITION_NAME from SAP_DIMCOMPANY a (nolock)
                                     inner join SAP_COMPANY_EMPLOYEE b(nolock) on a.COMPANY_ID = b.COMPANY_ID
                                     inner join tmp_Employee c on b.SALES_CODE = c.SALES_CODE
                                     inner join POSTN d on c.EMAIL_ID = d.EMAIL_ID 
                                     where a.COMPANY_ID = @ERPID order by b.PARTNER_FUNCTION", connMy);
                apt.SelectCommand.Parameters.AddWithValue("ERPID", CompanyID);
                var dt = new System.Data.DataTable();
                apt.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dic.Clear();
                    dic.Add(dt.Rows[0]["ROW_ID"].ToString(), dt.Rows[0]["PRIMARY_POSITION_NAME"].ToString());
                }
                connMy.Close();
            }
            catch (Exception)
            {
                connMy.Close();
            }
            return dic;
        }
        public static void LogAssociateSiebelSAPAccountContact(string ContactEmail, string SAPSoldToId, bool IsABB, string ContactRowID, string AccountRowID, bool Is_New_Contact, bool Is_New_Account, string Msg)
        {
            System.Data.SqlClient.SqlConnection connMyLocal = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MYLOCAL"].ConnectionString);
            connMyLocal.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(@"
                    insert into AssociateSiebelSAPAccountContact_LOG (CONTACT_ID,ACCOUNT_ID,NEW_CONTACT,NEW_ACCOUNT,MSG,EMAIL,COMPANY_ID,IS_ABB) values 
                    (@CONTACT_ID,@ACCOUNT_ID,@NEW_CONTACT,@NEW_ACCOUNT,@MSG,@EMAIL,@COMPANY_ID,@IS_ABB)", connMyLocal);
            cmd.Parameters.AddWithValue("CONTACT_ID", ContactRowID);
            cmd.Parameters.AddWithValue("ACCOUNT_ID", AccountRowID);
            cmd.Parameters.AddWithValue("NEW_CONTACT", Is_New_Contact);
            cmd.Parameters.AddWithValue("NEW_ACCOUNT", Is_New_Account);
            cmd.Parameters.AddWithValue("MSG", Msg);
            cmd.Parameters.AddWithValue("EMAIL", ContactEmail);
            cmd.Parameters.AddWithValue("COMPANY_ID", SAPSoldToId);
            cmd.Parameters.AddWithValue("IS_ABB", IsABB);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string smtp = ConfigurationManager.AppSettings["SMTPServer"] != null ? ConfigurationManager.AppSettings["SMTPServer"] : "172.20.0.76";
                SmtpClient smtpClient1 = new SmtpClient(smtp);
                MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "MyAdvantech@advantech.com");
                mail.Subject = "Log AssociateSiebelSAPAccountContact Error";
                mail.IsBodyHtml = true;
                mail.Body = string.Format("Message: {0}<br />Exception: {1}", ex.Message, ex.ToString());
                smtpClient1.Send(mail);
            }
        }

        public static bool CreateAccountBySAPSoldToId(string KUNNR)
        {
            var dtSAPCompany = new DataTable();
            var aptMy = new System.Data.SqlClient.SqlDataAdapter(string.Format(@"
            select a.COMPANY_NAME, a.TEL_NO, a.FAX_NO, a.SALESGROUP, a.SALESOFFICE
            , a.ADDRESS, a.CITY, a.COUNTRY_NAME, a.ZIP_CODE 
            from SAP_DIMCOMPANY a (nolock) 
            where a.COMPANY_ID='{0}'
            ", KUNNR), ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
            aptMy.Fill(dtSAPCompany); aptMy.SelectCommand.Connection.Close();
            if (dtSAPCompany.Rows.Count == 0) return false;

            WSSiebel_AddAccount.ACC acc = new WSSiebel_AddAccount.ACC();
            acc.NAME = dtSAPCompany.Rows[0]["COMPANY_NAME"].ToString();
            acc.ERPID = KUNNR;
            acc.MAIN_PHONE = dtSAPCompany.Rows[0]["TEL_NO"].ToString();
            acc.MAIN_FAX = dtSAPCompany.Rows[0]["FAX_NO"].ToString();
            acc.STATUS = "07-General Account";
            switch (dtSAPCompany.Rows[0]["SALESGROUP"].ToString())
            {
                case "290":
                    acc.STATUS = "07-General Account";
                    break;
                case "291":
                    acc.STATUS = "01- Channel Partner";
                    break;
                case "292":
                    acc.STATUS = "06-Key Account";
                    break;
            }
            //Address
            var objAddr = new WSSiebel_AddAccount.ADDR();
            objAddr.LINE1 = dtSAPCompany.Rows[0]["ADDRESS"].ToString();
            objAddr.CITY = dtSAPCompany.Rows[0]["CITY"].ToString();
            objAddr.COUNTRY = dtSAPCompany.Rows[0]["COUNTRY_NAME"].ToString();
            objAddr.ZIP = dtSAPCompany.Rows[0]["ZIP_CODE"].ToString();
            objAddr.IS_PRIMARY = "Y";
            acc.ADDR = new WSSiebel_AddAccount.ADDR[] { objAddr };

            //Position
            var owner = GetSiebelPositionBySAPCompanyID(KUNNR);
            var objPos = new WSSiebel_AddAccount.POSITION();
            objPos.NAME = string.IsNullOrEmpty(owner.First().Value) ? "System Use - MyAdvantech" : owner.First().Value;
            objPos.IS_PRIMARY = "Y";
            acc.POSITION = new WSSiebel_AddAccount.POSITION[] { objPos };

            //Org
            var objOrg = new WSSiebel_AddAccount.ORG();
            objOrg.NAME = "HQDC";
            if (objAddr.COUNTRY == "Taiwan") objOrg.NAME = "ATW";
            objOrg.IS_PRIMARY = "Y";
            acc.ORG = new WSSiebel_AddAccount.ORG[] { objOrg };

            var AccountRowId = CreateAccount2(ref acc);
            return !string.IsNullOrEmpty(AccountRowId);
        }

        public static bool AssociateSiebelSAPAccountContact(string ContactEmail, string FirstName, string LastName, string SAPSoldToId, string VKORG, bool IsToSAPPRD, bool IsABB)
        {
            //if contact email not yet exists on Siebel, create contact first
            //if contact already associates to a Siebel Account, but account is without ERPID, update ERPID to that Siebel Account
            //if contact not yet associates to a Siebel Account, create Siebel Account with ERPID=SAPSoldToId, then associate contact to that account
            //Sync Siebel account/contact from Siebel db to MyA local db
            var SiebelApt = new System.Data.SqlClient.SqlDataAdapter(
                @"
                select A.ROW_ID as CONTACT_ROW_ID, A.EMAIL_ADDR, B.ROW_ID as ACCOUNT_ROW_ID, D.ATTRIB_05 as ERPID, IsNull(A.SUPPRESS_EMAIL_FLG, 'N') as NeverEmail
                FROM S_CONTACT A inner join S_PARTY z on z.ROW_ID=A.BU_ID 
                LEFT JOIN S_CONTACT_X E ON A.ROW_ID = E.ROW_ID LEFT JOIN S_ORG_EXT B ON A.PR_DEPT_OU_ID = B.PAR_ROW_ID 
                LEFT JOIN S_ORG_EXT_X D ON B.ROW_ID = D.ROW_ID
                where z.NAME='ABB' and LOWER(A.EMAIL_ADDR)=lower(@EMAIL)
                ",
                ConfigurationManager.ConnectionStrings["CRMDB75"].ConnectionString);
            SiebelApt.SelectCommand.Parameters.AddWithValue("EMAIL", ContactEmail);
            var dtAccountContact = new System.Data.DataTable();
            SiebelApt.Fill(dtAccountContact);
            SiebelApt.SelectCommand.Parameters.Clear();
            SiebelApt.SelectCommand.CommandText =
                @"select B.ROW_ID
                from S_ORG_EXT B inner JOIN S_ORG_EXT_X D ON B.ROW_ID = D.ROW_ID
                inner join S_PARTY c on B.BU_ID=c.ROW_ID
                where D.ATTRIB_05=@ERPID and c.NAME='ABB'";
            SiebelApt.SelectCommand.Parameters.AddWithValue("ERPID", SAPSoldToId);
            var dtAccountWithERPID = new System.Data.DataTable();
            SiebelApt.Fill(dtAccountWithERPID);
            SiebelApt.SelectCommand.Connection.Close();
            bool isAssocicateContactToAccount = false; var ContactRowId = ""; var AccountRowId = ""; var Msg = ""; bool NewContact = false; bool NewAccount = false;

            if (dtAccountWithERPID.Rows.Count == 0)
            {
                //Create Account, then hook contact to the newly created account
                try
                {
                    var SAPApt = new System.Data.SqlClient.SqlDataAdapter(
                    @"
                    select top 1 a.COMPANY_NAME, a.ADDRESS, a.TEL_NO, a.FAX_NO, a.ZIP_CODE, a.CITY, a.COUNTRY_NAME, a.SALESGROUP, isnull(a.REGION_CODE,'') as REGION_CODE 
                    from SAP_DIMCOMPANY a where COMPANY_ID=@ERPID
                    ",
                    ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
                    SAPApt.SelectCommand.Parameters.AddWithValue("ERPID", SAPSoldToId);
                    var dtSAPCompany = new System.Data.DataTable();
                    SAPApt.Fill(dtSAPCompany);

                    if (dtSAPCompany.Rows.Count > 0)
                    {
                        NewAccount = true;
                        //Check if the same account name in Siebel
                        SiebelApt = new System.Data.SqlClient.SqlDataAdapter(
                        @"
                        select a.ROW_ID 
                        from S_ORG_EXT a inner join S_PARTY b on a.BU_ID=b.ROW_ID 
                        where Lower(a.NAME) = @NAME and b.NAME='ABB'
                        ",
                        ConfigurationManager.ConnectionStrings["CRMDB75"].ConnectionString);
                        SiebelApt.SelectCommand.Parameters.AddWithValue("NAME", dtSAPCompany.Rows[0]["COMPANY_NAME"].ToString().ToLower());
                        var dtSameAccountName = new System.Data.DataTable();
                        SiebelApt.Fill(dtSameAccountName);
                        SiebelApt.SelectCommand.Connection.Close();
                        if (dtSameAccountName.Rows.Count > 0)
                        {
                            AccountRowId = dtSameAccountName.Rows[0]["ROW_ID"].ToString();
                        }
                        else
                        {
                            WSSiebel_AddAccount.ACC acc = new WSSiebel_AddAccount.ACC();
                            acc.NAME = dtSAPCompany.Rows[0]["COMPANY_NAME"].ToString();
                            acc.ERPID = SAPSoldToId;
                            acc.MAIN_PHONE = dtSAPCompany.Rows[0]["TEL_NO"].ToString();
                            acc.MAIN_FAX = dtSAPCompany.Rows[0]["FAX_NO"].ToString();
                            switch (dtSAPCompany.Rows[0]["SALESGROUP"].ToString())
                            {
                                case "290":
                                    acc.STATUS = "07-General Account";
                                    break;
                                case "291":
                                    acc.STATUS = "01- Channel Partner";
                                    break;
                                case "292":
                                    acc.STATUS = "06-Key Account";
                                    break;
                            }

                            //Address
                            var objAddr = new WSSiebel_AddAccount.ADDR();
                            objAddr.LINE1 = dtSAPCompany.Rows[0]["ADDRESS"].ToString();
                            objAddr.CITY = dtSAPCompany.Rows[0]["CITY"].ToString();

                            //Ryan 20180605 Siebel takes country field as "United States" instead of "US" & "USA"
                            string strCountry = dtSAPCompany.Rows[0]["COUNTRY_NAME"].ToString();
                            if (strCountry.ToUpper().Equals("US") || strCountry.ToUpper().Equals("USA")) strCountry = "United States";
                            objAddr.COUNTRY = strCountry;

                            objAddr.ZIP = dtSAPCompany.Rows[0]["ZIP_CODE"].ToString();
                            objAddr.STATE = dtSAPCompany.Rows[0]["REGION_CODE"].ToString();
                            objAddr.IS_PRIMARY = "Y";
                            acc.ADDR = new WSSiebel_AddAccount.ADDR[] { objAddr };

                            //Position
                            var owner = GetSiebelPositionBySAPCompanyID(SAPSoldToId);
                            var objPos = new WSSiebel_AddAccount.POSITION();
                            objPos.NAME = string.IsNullOrEmpty(owner.First().Value) ? "System Use - MyAdvantech" : owner.First().Value;
                            objPos.IS_PRIMARY = "Y";
                            acc.POSITION = new WSSiebel_AddAccount.POSITION[] { objPos };

                            //Org
                            var objOrg = new WSSiebel_AddAccount.ORG();
                            objOrg.NAME = "ABB";
                            objOrg.IS_PRIMARY = "Y";
                            acc.ORG = new WSSiebel_AddAccount.ORG[] { objOrg };

                            AccountRowId = CreateAccount2(ref acc);

                            if (AccountRowId == "")
                            {
                                Msg = "Create Account Failed.";
                                //LogAssociateSiebelSAPAccountContact(ContactEmail, SAPSoldToId, IsABB, ContactRowId, AccountRowId, NewContact, NewAccount, Msg);
                                //If Siebel web service timeout, directly update local table
                                string tmpID = "tmp" + SAPSoldToId;
                                System.Data.SqlClient.SqlConnection connMY = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
                                connMY.Open();
                                System.Data.SqlClient.SqlCommand cmdUpdAcc = new System.Data.SqlClient.SqlCommand("update SIEBEL_CONTACT set ERPID='" + SAPSoldToId + "', ACCOUNT_ROW_ID='" + tmpID + "' where ROW_ID='" + ContactRowId + "';insert into SIEBEL_ACCOUNT (ROW_ID,ACCOUNT_NAME,RBU,ERP_ID) values ('" + tmpID + "',N'" + dtSAPCompany.Rows[0]["COMPANY_NAME"].ToString().Replace("'", "''") + "','ABB','" + SAPSoldToId + "')", connMY);
                                connMY.Close();
                            }
                        }
                    }
                    else
                    {
                        Msg = "No SAP Company Data.";
                        LogAssociateSiebelSAPAccountContact(ContactEmail, SAPSoldToId, IsABB, ContactRowId, AccountRowId, NewContact, NewAccount, Msg);
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    Msg = "Create Account Failed2. " + ex.ToString();
                    LogAssociateSiebelSAPAccountContact(ContactEmail, SAPSoldToId, IsABB, ContactRowId, AccountRowId, NewContact, NewAccount, Msg);
                    return false;
                }
            }
            else
            {
                AccountRowId = dtAccountWithERPID.Rows[0]["ROW_ID"].ToString();
            }

            if (dtAccountContact.Rows.Count == 0)
            {
                //create contact, and hook to an account
                NewContact = true;
                try
                {
                    SIEBEL_CONTACT con = new SIEBEL_CONTACT();
                    con.ACCOUNT_ROW_ID = AccountRowId;
                    con.EMAIL_ADDRESS = ContactEmail;
                    con.FirstName = FirstName; con.LastName = LastName; con.OrgID = "ABB";
                    con.OwnerId = GetSiebelPositionBySAPCompanyID(SAPSoldToId).First().Key;

                    ContactRowId = CreateSiebelContactByWS(con);
                    if (!string.IsNullOrEmpty(ContactRowId))
                    {
                        isAssocicateContactToAccount = true;
                    }
                    else
                    {
                        Msg = "Create Contact Failed.";
                        LogAssociateSiebelSAPAccountContact(ContactEmail, SAPSoldToId, IsABB, ContactRowId, AccountRowId, NewContact, NewAccount, Msg);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Msg = "Create Contact Failed. " + ex.ToString();
                    LogAssociateSiebelSAPAccountContact(ContactEmail, SAPSoldToId, IsABB, ContactRowId, AccountRowId, NewContact, NewAccount, Msg);
                    return false;
                }
            }
            else
            {
                ContactRowId = dtAccountContact.Rows[0]["CONTACT_ROW_ID"].ToString();
                if (dtAccountContact.Rows[0]["ACCOUNT_ROW_ID"] == DBNull.Value || dtAccountContact.Rows[0]["ACCOUNT_ROW_ID"].ToString() == "1-1CK7KL5") //If no account or B+B dummy account, associate an new account
                {
                    SIEBEL_CONTACT updContact = new SIEBEL_CONTACT();
                    updContact.ROW_ID = ContactRowId;
                    updContact.ACCOUNT_ROW_ID = AccountRowId;
                    updContact.NeverEmail = dtAccountContact.Rows[0]["NeverEmail"].ToString();
                    UpdateSiebelContactByWS(updContact);
                }
                else
                {

                }
            }

            LogAssociateSiebelSAPAccountContact(ContactEmail, SAPSoldToId, IsABB, ContactRowId, AccountRowId, NewContact, NewAccount, Msg);

            //Sync to MyAdvantechGlobal Local DB
            if (AccountRowId != "") SyncSiebelAccount(new List<string> { AccountRowId });
            if (ContactRowId != "") SyncSiebelContact(new List<string> { ContactRowId });
            return true;
        }

        public static bool SyncSiebelAccount(List<string> arrAccId)
        {
            int retry = 0;
            try
            {
                var dt = new System.Data.DataTable();
                string sql = "";
                while (retry < 6)
                {
                    sql = " select a.ROW_ID, IsNull(b.ATTRIB_05, '') as ERP_ID, a.NAME as ACCOUNT_NAME, a.CUST_STAT_CD as ACCOUNT_STATUS, " +
                    " IsNull(a.MAIN_FAX_PH_NUM, '') as FAX_NUM, IsNull(a.MAIN_PH_NUM, '') as PHONE_NUM, " +
                    " IsNull(a.OU_TYPE_CD, '') as OU_TYPE_CD,IsNull(a.URL, '') as URL,IsNull(b.ATTRIB_34, '') as BusinessGroup,  " +
                    " IsNull(a.OU_TYPE_CD, '') as ACCOUNT_TYPE, IsNull(c.NAME, '') as RBU,   " +
                    " IsNull((select EMAIL_ADDR from S_CONTACT where ROW_ID in (select PR_EMP_ID from S_POSTN where ROW_ID in (select PR_POSTN_ID from S_ORG_EXT where ROW_ID=a.ROW_ID))),'') as PRIMARY_SALES_EMAIL,  " +
                    " a.PAR_OU_ID as PARENT_ROW_ID,IsNull(b.ATTRIB_09,'N') as MAJORACCOUNT_FLAG,IsNull(a.CMPT_FLG,'N') as COMPETITOR_FLAG, " +
                    " IsNull(a.PRTNR_FLG,'N') as PARTNER_FLAG,IsNull(d.COUNTRY,'') as COUNTRY,IsNull(d.CITY,'') as CITY, " +
                    " IsNull(d.ADDR,'') as ADDRESS,IsNull(d.STATE,'') as STATE, IsNull(d.ZIPCODE,'') as ZIPCODE, IsNull(d.PROVINCE,'') as PROVINCE,  " +
                    " IsNull((select top 1 NAME from S_INDUST where ROW_ID=a.X_ANNIE_PR_INDUST_ID),'N/A') as BAA,a.CREATED, a.LAST_UPD as LAST_UPDATED,  " +
                    " IsNull((select top 1 e.NAME from S_PARTY e inner join S_POSTN f on e.ROW_ID=f.OU_ID where f.ROW_ID in (select PR_POSTN_ID from S_ORG_EXT where ROW_ID=a.ROW_ID)),'')  as PriOwnerDivision,  " +
                    " PR_POSTN_ID as PriOwnerRowId,IsNull((select top 1 f.NAME from S_POSTN f where f.ROW_ID in (select PR_POSTN_ID from S_ORG_EXT where ROW_ID=a.ROW_ID)),'')  as PriOwnerPosition,  " +
                    " cast('' as nvarchar(10)) as LOCATION, cast('' as nvarchar(10)) as ACCOUNT_TEAM,  " +
                    " IsNull(d.ADDR_LINE_2,'') as ADDRESS2, IsNull(b.ATTRIB_36,'') as ACCOUNT_CC_GRADE, IsNull(a.BASE_CURCY_CD,'') as CURRENCY  " +
                    " from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID left join S_PARTY c on a.BU_ID=c.ROW_ID left join S_ADDR_PER d on a.PR_ADDR_ID=d.ROW_ID  " +
                    " where a.ROW_ID in ('" + string.Join("','", arrAccId.ToArray()) + "')";
                    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["CRMDB75"].ConnectionString);
                    conn.Open();
                    System.Data.SqlClient.SqlDataAdapter apt = new System.Data.SqlClient.SqlDataAdapter(sql, conn);
                    apt.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0) { retry += 1; System.Threading.Thread.Sleep(5 * 1000); }
                    else break;
                }

                if (dt.Rows.Count == 0)
                {
                    string smtp = ConfigurationManager.AppSettings["SMTPServer"] != null ? ConfigurationManager.AppSettings["SMTPServer"] : "172.20.0.76";
                    SmtpClient smtpClient1 = new SmtpClient(smtp);
                    MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "rudy.wang@advantech.com.tw");
                    mail.Subject = "Sync Siebel Account Failed in AssociateSiebelSAPAccountContact function";
                    mail.IsBodyHtml = true;
                    mail.Body = "Message: No Account Data<br/>SQL: " + sql;
                    smtpClient1.Send(mail);
                    return false;
                }

                System.Data.SqlClient.SqlConnection connMY = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
                connMY.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("delete from SIEBEL_ACCOUNT where ROW_ID in ('" + string.Join("','", arrAccId.ToArray()) + "')", connMY);
                cmd.ExecuteNonQuery();

                System.Data.SqlClient.SqlBulkCopy bk = new System.Data.SqlClient.SqlBulkCopy(connMY);
                bk.DestinationTableName = "SIEBEL_ACCOUNT";
                bk.WriteToServer(dt);
                connMY.Close();
            }
            catch (Exception ex)
            {
                string smtp = ConfigurationManager.AppSettings["SMTPServer"] != null ? ConfigurationManager.AppSettings["SMTPServer"] : "172.20.0.76";
                SmtpClient smtpClient1 = new SmtpClient(smtp);
                MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "MyAdvantech@advantech.com");
                mail.Subject = "Sync Siebel Account Failed in Adding Account";
                mail.IsBodyHtml = true;
                mail.Body = string.Format("Message: {0}, {1}<br />", ex.Message, ex.ToString());
                smtpClient1.Send(mail);
                return false;
            }
            return true;
        }

        public static bool SyncSiebelContact(List<string> arrConId)
        {
            int retry = 0;
            try
            {
                var dt = new System.Data.DataTable();
                string sql = "";
                while (retry < 6)
                {
                    sql = " SELECT  A.ROW_ID, IsNull(A.FST_NAME, '') AS 'FirstName',IsNull(A.MID_NAME, '') as 'MiddleName', IsNull(A.LAST_NAME, '') AS 'LastName', " +
                       " IsNull(A.WORK_PH_NUM, '') as 'WorkPhone',IsNull(A.CELL_PH_NUM, '') as 'CellPhone',IsNull(A.FAX_PH_NUM, '') as 'FaxNumber',  " +
                       " IsNull(E.ATTRIB_37, '') as 'JOB_FUNCTION', IsNull(A.PAR_ROW_ID, '') as PAR_ROW_ID,IsNull(D.ATTRIB_05, '') AS 'ERPID',  " +
                       " IsNull(A.BU_ID, '') as 'PriOrgId',(select top 1 z.NAME from S_PARTY z where z.ROW_ID=A.BU_ID) as 'OrgID',  " +
                       " IsNull(A.PR_POSTN_ID, '') as 'OwnerId',IsNull(E.ATTRIB_09, 'N') AS 'CanSeeOrder',IsNull(A.X_CONTACT_LOGIN_PASSWORD, '') AS Password, " +
                       " '' as 'Sales_Rep',IsNull(A.SUPPRESS_EMAIL_FLG, '') as NeverEmail,IsNull(A.SUPPRESS_CALL_FLG,'') as NeverCall, " +
                       " IsNull(A.SUPPRESS_FAX_FLG, '') as NeverFax,IsNull(A.SUPPRESS_MAIL_FLG, '') as NeverMail,IsNull(A.JOB_TITLE, '') as JOB_TITLE, " +
                       " IsNull(A.EMAIL_ADDR, '') AS 'EMAIL_ADDRESS',A.COMMENTS,B.ROW_ID as ACCOUNT_ROW_ID,IsNull(B.NAME, '') AS ACCOUNT,IsNull(B.OU_TYPE_CD, '') AS 'ACCOUNT_TYPE',  " +
                       " IsNull(B.CUST_STAT_CD, '') AS 'ACCOUNT_STATUS',IsNull(C.COUNTRY, '') AS COUNTRY,IsNull(A.PER_TITLE, '') as Salutation, " +
                       " A.EMP_FLG as EMPLOYEE_FLAG,IsNull(A.ACTIVE_FLG,'N') as ACTIVE_FLG,IsNull(A.DFLT_ORDER_PROC_CD,'') as User_Type, IsNull(F.APPL_SRC_CD,'') as Reg_Source, " +
                       " A.CREATED, A.LAST_UPD as LAST_UPDATED, A.PR_REP_SYS_FLG as PRIMARY_FLAG    " +
                       " FROM S_CONTACT A LEFT JOIN S_CONTACT_X E ON A.ROW_ID = E.ROW_ID LEFT JOIN S_ORG_EXT B ON A.PR_DEPT_OU_ID = B.PAR_ROW_ID  " +
                       " LEFT JOIN S_ORG_EXT_X D ON B.ROW_ID = D.ROW_ID LEFT JOIN S_ADDR_PER C ON A.PR_OU_ADDR_ID = C.ROW_ID LEFT JOIN S_PER_PRTNRAPPL F ON A.ROW_ID=F.ROW_ID  " +
                       " WHERE A.ROW_ID = A.PAR_ROW_ID and A.ROW_ID in ('" + string.Join("','", arrConId.ToArray()) + "')";
                    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["CRMDB75"].ConnectionString);
                    conn.Open();
                    System.Data.SqlClient.SqlDataAdapter apt = new System.Data.SqlClient.SqlDataAdapter(sql, conn);
                    apt.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0) { retry += 1; System.Threading.Thread.Sleep(5 * 1000); }
                    else break;
                }

                if (dt.Rows.Count == 0)
                {
                    string smtp = ConfigurationManager.AppSettings["SMTPServer"] != null ? ConfigurationManager.AppSettings["SMTPServer"] : "172.20.0.76";
                    SmtpClient smtpClient1 = new SmtpClient(smtp);
                    MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "MyAdvantech@advantech.com");
                    mail.Subject = "Sync Siebel Contact Failed in AssociateSiebelSAPAccountContact function";
                    mail.IsBodyHtml = true;
                    mail.Body = "Message: No Contact Data<br/>SQL: " + sql;
                    smtpClient1.Send(mail);
                    return false;
                }

                System.Data.SqlClient.SqlConnection connMY = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
                connMY.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("delete from SIEBEL_CONTACT where ROW_ID in ('" + string.Join("','", arrConId.ToArray()) + "')", connMY);
                cmd.ExecuteNonQuery();

                System.Data.SqlClient.SqlBulkCopy bk = new System.Data.SqlClient.SqlBulkCopy(connMY);
                bk.DestinationTableName = "SIEBEL_CONTACT";
                bk.WriteToServer(dt);
                connMY.Close();
            }
            catch (Exception ex)
            {
                string smtp = ConfigurationManager.AppSettings["SMTPServer"] != null ? ConfigurationManager.AppSettings["SMTPServer"] : "172.20.0.76";
                SmtpClient smtpClient1 = new SmtpClient(smtp);
                MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "rudy.wang@advantech.com.tw");
                mail.Subject = "Sync Siebel Contact Failed in Adding Account";
                mail.IsBodyHtml = true;
                mail.Body = string.Format("Message: {0}, {1}<br />", ex.Message, ex.ToString());
                smtpClient1.Send(mail);
                return false;
            }
            return true;
        }

        #endregion

        #region Siebel Quote


        //public static string CreateSiebelQuote(QuotationMaster QM, List<QuotationDetail> QD, string optyid, ref string WSParameters, ref string ErrorMSG)
        //{
        //    string WS_ID = ConfigurationManager.AppSettings["CRMHQId"];
        //    string WS_PASSWORD = ConfigurationManager.AppSettings["CRMHQPwd"];
        //    int _QDCount = 0;
        //    if (QD != null && QD.Count > 0) 
        //    {
        //        _QDCount = QD.Count;
        //    }
        //    var EMP = new SiebelWebService.EMPLOYEE() { USER_ID = WS_ID, PASSWORD = WS_PASSWORD };
        //    string[] partno = new string[_QDCount];
        //    string[] qty = new string[_QDCount];
        //    string[] unitprice = new string[_QDCount];
        //    string[] duedate = new string[_QDCount];

        //    int i = 0;
        //    if (_QDCount > 0)
        //    {
        //        foreach (QuotationDetail _QuoteItem in QD)
        //        {
        //            partno[i] = _QuoteItem.partNo;
        //            qty[i] = _QuoteItem.qty.GetValueOrDefault(0).ToString();
        //            if (QM.quoteNo.StartsWith("TWQ"))
        //            {
        //                unitprice[i] = _QuoteItem.DisplayUnitPrice;
        //            }
        //            else
        //            {
        //                unitprice[i] = _QuoteItem.newUnitPrice.ToString();
        //            }
        //            duedate[i] = _QuoteItem.dueDate.GetValueOrDefault(DateTime.Now).ToString("MM/dd/yyyy");
        //            i += 1;
        //        }
        //    }

        //    if (string.IsNullOrEmpty(optyid) || optyid.Equals("NULL", StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        optyid = "";
        //    }

        //    var QUOTE = new SiebelWebService.QUOTE()
        //    {
        //        NAME = QM.quoteNo,
        //        CURRENCY = QM.currency, //"NTD",
        //        DUE_DATE = QM.expiredDate.GetValueOrDefault(DateTime.Now).ToString("MM/dd/yyyy"),// "12/30/2014",
        //        COMMENTS = QM.quoteNote,
        //        ACCOUNT_ID = QM.quoteToRowId,
        //        OPPORTUNITY_ID = optyid,
        //        ORGANIZATION = QM.siebelRBU,
        //        OWNER_LOGIN_NAME = GetSiebelLoginNameByRowid(QM.salesRowId), //"IRENELEE",
        //        OWNER_EMAIL = QM.salesEmail, //"irene.lee@advantech.com.tw",
        //        PRODUCT_NAME = partno,// = new string[]{ " ADAM-0060-A", "ADAM-2017PZ-AE", "ADAM-2017Z-AE", "ADAM-2018Z-AE", "ADAM-2051PZ-AE" },
        //        PRODUCT_QUANTITY = qty,// = new string[]{ "3", "4", "5", "6", "7" },
        //        DISCOUNT_PRICE = unitprice,// = new string[]{ "130", "140", "150", "160", "170" },
        //        DELIVERY_DATE = duedate,// = new string[]{ "12/31/2014", "12/01/2014", "12/02/2014", "12/03/2014", "12/03/2014" },
        //    };

        //    //Frank 20150408
        //    if (QM.quoteNo.StartsWith("TWQ")){
        //        QUOTE.NAME = QM.customId;
        //    }
        //    StringBuilder _wspara = new StringBuilder();
        //    _wspara.Append("NAME=" + QUOTE.NAME);
        //    _wspara.Append(",CURRENCY=" + QUOTE.CURRENCY);
        //    _wspara.Append(",DUE_DATE=" + QUOTE.DUE_DATE);
        //    _wspara.Append(",COMMENTS=" + QUOTE.COMMENTS);
        //    _wspara.Append(",ACCOUNT_ID=" + QUOTE.ACCOUNT_ID);
        //    _wspara.Append(",OPPORTUNITY_ID=" + QUOTE.OPPORTUNITY_ID);
        //    _wspara.Append(",ORGANIZATION=" + QUOTE.ORGANIZATION);
        //    _wspara.Append(",OWNER_LOGIN_NAME=" + QUOTE.OWNER_LOGIN_NAME);
        //    _wspara.Append(",OWNER_EMAIL=" + QUOTE.OWNER_EMAIL);
        //    _wspara.Append(",PRODUCT_NAME=" + String.Join("|", QUOTE.PRODUCT_NAME));
        //    _wspara.Append(",PRODUCT_QUANTITY=" + String.Join("|", QUOTE.PRODUCT_QUANTITY));
        //    _wspara.Append(",DISCOUNT_PRICE=" + String.Join("|", QUOTE.DISCOUNT_PRICE));
        //    _wspara.Append(",DELIVERY_DATE=" + String.Join("|", QUOTE.DELIVERY_DATE));
        //    WSParameters = _wspara.ToString();

        //    var WS = new SiebelWebService.WSSiebel();
        //    WS.Timeout = _SiebelWSTimeOut;

        //    SiebelWebService.RESULT _result = null;
        //    _result = WS.AddQuote(EMP, QUOTE);

        //    if (_result == null){
        //        ErrorMSG="Siebel WS returned RESULT object is null";
        //        return string.Empty;
        //    }
        //    if (_result.ROW_ID.Equals("Null", StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        ErrorMSG = "Siebel WS returned Quote ID is Null";
        //        return string.Empty;
        //    }
        //    if (string.IsNullOrEmpty(_result.ROW_ID))
        //    {
        //        ErrorMSG = "Siebel WS returned Quote ID is string.IsNullorEmpty";
        //        return string.Empty;
        //    }


        //    string QuoteID = QM.quoteId;
        //    //if (_result != null && _result.ROW_ID != "Null" && !string.IsNullOrEmpty(_result.ROW_ID) && !string.IsNullOrEmpty(QuoteID))
        //    if (!string.IsNullOrEmpty(QuoteID))
        //    {
        //        eQuotationDAL.updateQuoteSiebelQuote(QuoteID, _result.ROW_ID);
        //    }
        //    return _result.ROW_ID;
        //}

        /// <summary>
        /// New version of create Siebel quote
        /// </summary>
        /// <param name="QM"></param>
        /// <param name="QD"></param>
        /// <param name="optyid"></param>
        /// <param name="WSParameters"></param>
        /// <param name="ErrorMSG"></param>
        /// <returns></returns>
        public static string CreateSiebelQuoteV2(QuotationMaster QM, List<QuotationDetail> QD, string optyid, ref string WSParameters, ref string ErrorMSG)
        {
            if (string.IsNullOrEmpty(optyid) || optyid.Equals("NULL", StringComparison.InvariantCultureIgnoreCase))
            {
                optyid = string.Empty;
            }

            var strMeth = new WSSiebel_AddQuote.ADVWebServiceAddQuote();
            var strAddQoute = new WSSiebel_AddQuote.QUOTE();

            var strAddQouteInput = new WSSiebel_AddQuote.AddQuote_Input();
            var strAddQouteOutput = new WSSiebel_AddQuote.AddQuote_Output();

            strAddQoute.ACCOUNT_ID = QM.quoteToRowId;
            if (QM.currency == "CNY")
                strAddQoute.CURRENCY_CODE = "RMB";
            else
                strAddQoute.CURRENCY_CODE = QM.currency;

            strAddQoute.COMMENTS = QM.quoteNote;
            strAddQoute.DUE_DATE = QM.expiredDate.GetValueOrDefault(DateTime.Now).ToString("MM/dd/yyyy");
            strAddQoute.NAME = QM.quoteNo.StartsWith("TWQ") ? QM.customId : QM.quoteNo;
            strAddQoute.OPPORTUNITY_ID = optyid;

            //ORG
            strAddQoute.ORGANIZATION = new WSSiebel_AddQuote.ORGANIZATION[1];

            var strAddQuoteOrg = new WSSiebel_AddQuote.ORGANIZATION();
            strAddQuoteOrg.NAME = QM.siebelRBU;
            strAddQuoteOrg.IS_PRIMARY = "Y";
            strAddQoute.ORGANIZATION[0] = strAddQuoteOrg;

            //  POSITION 
            strAddQoute.POSITION = new WSSiebel_AddQuote.POSITION[1];

            var strAddQoutePosition = new WSSiebel_AddQuote.POSITION();
            strAddQoutePosition.NAME = QM.Position;
            strAddQoutePosition.IS_PRIMARY = "Y";
            strAddQoute.POSITION[0] = strAddQoutePosition;

            List<WSSiebel_AddQuote.QUOTE_ITEM> QuoteItems = new List<WSSiebel_AddQuote.QUOTE_ITEM>();
            foreach (var _QuoteItem in QD)
            {
                var strAddQouteQuoteItem = new WSSiebel_AddQuote.QUOTE_ITEM();
                strAddQouteQuoteItem.PRODUCT_NAME = _QuoteItem.partNo;
                strAddQouteQuoteItem.PRODUCT_QUANTITY = _QuoteItem.qty.GetValueOrDefault(0).ToString();
                strAddQouteQuoteItem.DISCOUNT_PRICE = QM.quoteNo.StartsWith("TWQ") ? _QuoteItem.DisplayUnitPrice : _QuoteItem.newUnitPrice.ToString();
                //ICC 2015/11/6 Siebel cannot verify the year 9999, so change to this year + 100 / 12 / 31. Ex. If value is 9999/12/31, and then today is 2015/11/6, delivery day should be 2115/12/31.
                DateTime deliveryDate = _QuoteItem.dueDate.GetValueOrDefault(DateTime.Now);
                strAddQouteQuoteItem.DELIVERY_DATE = deliveryDate.Year == 9999 ? new DateTime(DateTime.Now.AddYears(100).Year, 12, 31).ToString("MM/dd/yyyy") : deliveryDate.ToString("MM/dd/yyyy");
                QuoteItems.Add(strAddQouteQuoteItem);
            }

            strAddQoute.QUOTE_ITEM = QuoteItems.ToArray();

            strAddQouteInput.QUOTE = strAddQoute;

            strAddQouteInput.SOURCE = "eQuotation";

            strAddQouteOutput = strMeth.AddQuote(strAddQouteInput);


            StringBuilder _wspara = new StringBuilder();
            _wspara.AppendLine("Siebel new add quote WS");
            _wspara.AppendLine("QuoteNAME=" + strAddQoute.NAME);
            _wspara.AppendLine(",CURRENCY=" + strAddQoute.CURRENCY_CODE);
            _wspara.AppendLine(",DUE_DATE=" + strAddQoute.DUE_DATE);
            _wspara.AppendLine(",COMMENTS=" + strAddQoute.COMMENTS);
            _wspara.AppendLine(",ACCOUNT_ID=" + strAddQoute.ACCOUNT_ID);
            _wspara.AppendLine(",OPPORTUNITY_ID=" + strAddQoute.OPPORTUNITY_ID);
            _wspara.AppendLine(",ORGANIZATION=" + QM.siebelRBU);
            _wspara.AppendLine(",OWNER_LOGIN_NAME=" + QM.Position);
            //_wspara.Append(",OWNER_EMAIL=" + strAddQoute.OWNER_EMAIL);
            foreach (WSSiebel_AddQuote.QUOTE_ITEM _qitem in QuoteItems)
            {
                _wspara.Append(",PRODUCT_NAME=" + _qitem.PRODUCT_NAME);
                _wspara.Append(",PRODUCT_QUANTITY=" + _qitem.PRODUCT_QUANTITY);
                _wspara.Append(",DISCOUNT_PRICE=" + _qitem.DISCOUNT_PRICE);
                _wspara.AppendLine(",DELIVERY_DATE=" + _qitem.DELIVERY_DATE);
            }
            WSParameters = _wspara.ToString();

            string strStatus = strAddQouteOutput.STATUS;
            if (strStatus == "SUCCESS")
            {
                return strAddQouteOutput.ROW_ID;
            }
            else
            {
                ErrorMSG = string.Format("Error spcCode: {0}, Error spcMessage: {1}", strAddQouteOutput.Error_spcCode, strAddQouteOutput.Error_spcMessage);
                return string.Empty;
            }

        }

        ///// <summary>
        ///// Create Siebel Quote through Siebel web service locate: http://172.20.1.43:8089/WSSiebel.asmx
        ///// </summary>
        ///// <param name="active"></param>
        ///// <returns>Quote ROW_ID</returns>
        //public static string CreateSiebelQuote(SiebelActive active)
        //{
        //    var emp = new SiebelWebService.EMPLOYEE();
        //    emp.USER_ID = ConfigurationManager.AppSettings["CRMHQId"];
        //    emp.PASSWORD = ConfigurationManager.AppSettings["CRMHQPwd"];

        //    List<string> pnList = new List<string>();
        //    List<string> qtyList = new List<string>();
        //    List<string> upriceList = new List<string>();
        //    List<string> dueDateList = new List<string>();

        //    foreach (QuotationDetail detail in active.QuotationDetail)
        //    {
        //        pnList.Add(detail.partNo);
        //        qtyList.Add(detail.qty.GetValueOrDefault(0).ToString());

        //        if (active.QuotationMaster.quoteNo.StartsWith("TWQ"))
        //            upriceList.Add(detail.DisplayQty);
        //        else
        //            upriceList.Add(detail.newUnitPrice.ToString());

        //        dueDateList.Add(detail.dueDate.GetValueOrDefault(DateTime.Now).ToString("MM/dd/yyyy"));
        //    }

        //    var Quote = new SiebelWebService.QUOTE()
        //    {
        //        NAME = "Y" + DateTime.Now.ToString("yyyyMMddhhmmss"),//active.QuotationMaster.quoteNo
        //        CURRENCY = active.QuotationMaster.currency,
        //        DUE_DATE = active.QuotationMaster.expiredDate.GetValueOrDefault(DateTime.Now).ToString("MM/dd/yyyy"),
        //        COMMENTS = active.QuotationMaster.quoteNote,
        //        ACCOUNT_ID = active.QuotationMaster.quoteToRowId,
        //        OPPORTUNITY_ID = "",//active.OptyID,
        //        ORGANIZATION = active.QuotationMaster.siebelRBU,
        //        OWNER_LOGIN_NAME = GetSiebelLoginNameByRowid(active.QuotationMaster.salesRowId),
        //        OWNER_EMAIL = active.QuotationMaster.salesEmail,
        //        PRODUCT_NAME = pnList.ToArray(),
        //        PRODUCT_QUANTITY = qtyList.ToArray(),
        //        DISCOUNT_PRICE = upriceList.ToArray(),
        //        DELIVERY_DATE = dueDateList.ToArray()
        //        //PRODUCT_NAME =  new string[]{ "ADAM-0060-A", "ADAM-2017PZ-AE", "ADAM-2017Z-AE", "ADAM-2018Z-AE", "ADAM-2051PZ-AE" },
        //        //PRODUCT_QUANTITY  = new string[]{ "3", "4", "5", "6", "7" },
        //        //DISCOUNT_PRICE  = new string[]{ "130", "140", "150", "160", "170" },
        //        //DELIVERY_DATE  = new string[]{ "12/31/2014", "12/01/2014", "12/02/2014", "12/03/2014", "12/03/2014" },
        //    };

        //    var WSSiebel = new SiebelWebService.WSSiebel();
        //    WSSiebel.Timeout = 30000;
        //   //WSSiebel.AddQuoteCompleted += new SiebelWebService.AddQuoteCompletedEventHandler(AddQuoteAsyncCompleted);
        //   //WSSiebel.AddQuoteAsync(emp, Quote, active.QuotationMaster.quoteId);

        //    try
        //    {
        //        WSSiebel.AddQuote(emp, Quote);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    if (Quote == null || Quote.ROW_ID == null || string.IsNullOrEmpty(Quote.ROW_ID)) return "";
        //    string QuoteID =  active.QuotationMaster.quoteId;
        //    if (Quote != null &&!string.IsNullOrEmpty(Quote.ROW_ID) && !string.IsNullOrEmpty(QuoteID))
        //    {
        //        eQuotationDAL.updateQuoteSiebelQuote(QuoteID, Quote.ROW_ID);
        //    }
        //   // smtpClient1.Send("myadvantech@advantech.com", "ming.zhao@advantech.com.cn", "create: sieble Quote  succssed", RowID);

        //    //因改成EventHandler做，仍然需要返回Siebel Quote ID作為確認
        //    //if (active.SiebelQuote != null)
        //    //    return active.SiebelQuote.siebelQuoteId;
        //    return Quote.ROW_ID;
        //}

        /// <summary>
        /// CreateSiebleActivity
        /// </summary>
        /// <param name="active">active</param>
        /// <returns></returns>
        public static string CreateSiebleActivity(SiebelActive active, ref string wspara, ref string errmsg)
        {
            var emp = new SiebelWebService.EMPLOYEE();
            emp.USER_ID = ConfigurationManager.AppSettings["CRMHQId"];
            emp.PASSWORD = ConfigurationManager.AppSettings["CRMHQPwd"];
            QuotationMaster QM = active.QuotationMaster;
            var Activity = new SiebelWebService.ACTION
            {
                ACT_TYPE = "Email - Outbound",
                STATUS = "Done",
                DESP = active.Subject,
                ACC_ROW_ID = QM.quoteToRowId,
                CMT = active.Greeting,
                ORG = QM.siebelRBU,
                OWNER_EMAIL = QM.salesEmail,
                CON_ROW_ID = active.ContactRowId
            };

            StringBuilder _wspara = new StringBuilder();
            _wspara.Append("ACT_TYPE=" + Activity.ACT_TYPE);
            _wspara.Append(",STATUS=" + Activity.STATUS);
            _wspara.Append(",DESP=" + Activity.DESP);
            _wspara.Append(",ACC_ROW_ID=" + Activity.ACC_ROW_ID);
            _wspara.Append(",CMT=" + Activity.CMT);
            _wspara.Append(",ORG=" + Activity.ORG);
            _wspara.Append(",OWNER_EMAIL=" + Activity.OWNER_EMAIL);
            _wspara.Append(",CON_ROW_ID=" + Activity.CON_ROW_ID);
            wspara = _wspara.ToString();

            var WSSiebel = new SiebelWebService.WSSiebel();
            WSSiebel.Timeout = _SiebelWSTimeOut;
            SiebelWebService.RESULT _result = WSSiebel.AddAction(emp, Activity);

            if (_result != null)
            {
                string SiebelActivityRowID = _result.ROW_ID;
                if (!string.IsNullOrEmpty(_result.ERR_MSG))
                {
                    errmsg = _result.ERR_MSG;
                    return string.Empty;
                }
                if (string.IsNullOrEmpty(SiebelActivityRowID))
                {
                    errmsg = "Siebel WS returned Activity Row ID is string.IsNullorEmpty";
                    return string.Empty;
                }
                return SiebelActivityRowID;
            }

            return string.Empty;
        }
        public static string CreateSiebleActivityV2(SiebelActive active, ref string wspara, ref string errmsg)
        {

            QuotationMaster QM = active.QuotationMaster;
            var strMeth = new WSSiebel_AddAction.ADVWebServoceAddAction();
            var strAction = new WSSiebel_AddAction.ACT();
            var ACTInput = new WSSiebel_AddAction.AddAction_Input();
            var ACTOutput = new WSSiebel_AddAction.AddAction_Output();

            strAction.ACT_TYPE = "Email - Outbound";
            strAction.COMMENT = active.Subject;
            strAction.DESP = active.Subject;
            strAction.STATUS = "Done";
            strAction.CON_ROW_ID = active.ContactRowId;
            //Frank 20161011 This is a bug because I don't understand why to input quote sales owner to contact_email @.@?
            //strAction.CONTACT_EMAIL = QM.salesEmail;
            strAction.ORG = QM.siebelRBU;
            strAction.SALES_LEADS_FLAG = "Y";
            strAction.OWNER_EMAIL = QM.salesEmail;
            strAction.PLANNED_START = DateTime.Now.ToString("MM/dd/yyyy");

            ACTInput.ACT = strAction;
            ACTInput.SOURCE = "TEST";
            ACTOutput = strMeth.AddAction(ACTInput);
            string strStatus = ACTOutput.STATUS;
            string strROW_ID = "";
            if (strStatus == "SUCCESS")
            {
                strROW_ID = ACTOutput.ROW_ID;
                StringBuilder _wspara = new StringBuilder();
                _wspara.Append("ACT_TYPE=" + strAction.ACT_TYPE);
                _wspara.Append(",STATUS=" + ACTOutput.STATUS);
                _wspara.Append(",DESP=" + strAction.DESP);
                _wspara.Append(",ORG=" + strAction.ORG);
                _wspara.Append(",OWNER_EMAIL=" + strAction.OWNER_EMAIL);
                _wspara.Append(",CON_ROW_ID=" + strAction.CON_ROW_ID);
                wspara = _wspara.ToString();
            }
            else
            {

                errmsg = ACTOutput.Error_spcCode + "; " + strStatus + "; " + ACTOutput.Error_spcMessage;
            }


            return strROW_ID;
        }

        #endregion

        #region Siebel Opportunity

        /// <summary>
        /// Create Siebel Opportunity through Siebel web service locate: http://172.20.1.43:8089/WSSiebel.asmx
        /// </summary>
        /// <param name="active"></param>
        /// <returns>Opportunity ROW_ID</returns>
        public static string CreateSiebelOpty(SiebelActive active, ref string wspara, ref string errmsg)
        {
            //System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient("172.20.0.76");
            var emp = new SiebelWebService.EMPLOYEE();
            emp.USER_ID = ConfigurationManager.AppSettings["CRMHQId"];
            emp.PASSWORD = ConfigurationManager.AppSettings["CRMHQPwd"];

            active.QuotationMaster.siebelRBU = active.QuotationMaster.siebelRBU.Replace("AMX", "ANADMF");
            StringBuilder _wspara = new StringBuilder();

            var Opty = new SiebelWebService.OPPTY()
            {
                ACC_ROW_ID = active.QuotationMaster.quoteToRowId,
                CLOSE_DATE = DateTime.Now.AddMonths(1),
                CURRENCY_CODE = active.QuotationMaster.currency,
                DESP = string.Empty,
                ORG = active.QuotationMaster.siebelRBU,
                OWNER_EMAIL = active.OptyOwnerEmail,
                PROJ_NAME = active.OptyName,
                SALES_METHOD = "Funnel Sales Methodology",
                SALES_STAGE = active.OptyStage,
                REVENUE = active.QuotationMaster.Revenue  //Revenue暫時沒有區別ATW
            };

            if (!string.IsNullOrEmpty(active.QuotationMaster.attentionRowId))
                Opty.CON_ROW_ID = active.QuotationMaster.attentionRowId;

            var WSSiebel = new SiebelWebService.WSSiebel();
            var Result = new SiebelWebService.RESULT();
            StringBuilder MailSubject = new StringBuilder("Creating New Opportunity by eQuotation ");
            StringBuilder MessageSB = new StringBuilder();

            _wspara.Append("ACC_ROW_ID=" + Opty.ACC_ROW_ID);
            _wspara.Append(",CLOSE_DATE=" + Opty.CLOSE_DATE);
            _wspara.Append(",CURRENCY_CODE=" + Opty.CURRENCY_CODE);
            _wspara.Append(",DESP=" + Opty.DESP);
            _wspara.Append(",ORG=" + Opty.ORG);
            _wspara.Append(",OWNER_EMAIL=" + Opty.OWNER_EMAIL);
            _wspara.Append(",PROJ_NAME=" + Opty.PROJ_NAME);
            _wspara.Append(",SALES_METHOD=" + Opty.SALES_METHOD);
            _wspara.Append(",SALES_STAGE=" + Opty.SALES_STAGE);
            _wspara.Append(",REVENUE=" + Opty.REVENUE);
            _wspara.Append(",CON_ROW_ID=" + Opty.CON_ROW_ID);
            wspara = _wspara.ToString();
            try
            {
                Result = WSSiebel.AddOppty(emp, Opty);

                if (Result != null)
                {


                    errmsg = Result.ERR_MSG;
                    if (!string.IsNullOrEmpty(errmsg))
                    {
                        return string.Empty;
                    }

                    //MailSubject.Append(string.IsNullOrEmpty(Result.ERR_MSG) ? "(Success)" : "(Failed!)");

                    //MessageSB.Append(string.Format("Return OptyRowID: {0} <br />", Result.ROW_ID));
                    //MessageSB.Append(string.Format("Return Error Message:<font color='red'>{0}</font><br />", Result.ERR_MSG));
                    //MessageSB.Append("================================<br />");
                    //MessageSB.Append("Call Method:eCovWs.AddOppty <br />");
                    //MessageSB.Append(string.Format("OPPTY.PROJ_NAME = {0} <br />", Opty.PROJ_NAME));
                    //MessageSB.Append(string.Format("OPPTY.SALES_STAGE = {0} <br />", Opty.SALES_STAGE));
                    //MessageSB.Append(string.Format("OPPTY.ACC_ROW_ID = {0} <br />", Opty.ACC_ROW_ID));
                    //MessageSB.Append(string.Format("OPPTY.CLOSE_DATE = {0} <br />", Opty.CLOSE_DATE));
                    //MessageSB.Append(string.Format("OPPTY.CURRENCY_CODE = {0} <br />", Opty.CURRENCY_CODE));
                    //MessageSB.Append(string.Format("OPPTY.DESP = {0} <br />", Opty.DESP));
                    //MessageSB.Append(string.Format("OPPTY.ORG = {0 <br />", Opty.ORG));
                    //MessageSB.Append(string.Format("OPPTY.OWNER_EMAIL = {0} <br />", Opty.OWNER_EMAIL));
                    //MessageSB.Append(string.Format("OPPTY.SALES_METHOD = {0} <br />", Opty.SALES_METHOD));
                    //MessageSB.Append(string.Format("OPPTY.CON_ROW_I}D = {0} <br />", Opty.CON_ROW_ID));
                    //MessageSB.Append(string.Format("OPPTY.SRC_ID = {0} <br />", Opty.SRC_ID));

                    //MailMessage newEmail = new MailMessage();
                    //newEmail.From = new MailAddress("myadvantech@advantech.com");
                    //newEmail.To.Add(new MailAddress("myadvantech@advantech.com"));
                    //newEmail.Subject = MailSubject.ToString();
                    //newEmail.Body = MessageSB.ToString();
                    //newEmail.IsBodyHtml = true;                
                    //newEmail.Priority = MailPriority.Normal;
                    //smtpClient1.Send(newEmail);

                    return Result.ROW_ID;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                //StringBuilder pare = new StringBuilder();
                //pare.Append(string.Format("{0}###{1}", active.QuotationMaster.quoteToRowId, DateTime.Now.AddMonths(1)));
                //pare.Append(string.Format("###{0}###{1}###{2}", active.QuotationMaster.currency, active.QuotationMaster.siebelRBU, active.SiebelOpty.Opty_Owner_Email));
                //pare.Append(string.Format("###{0}###Funnel Sales Methodology###{1}###", active.SiebelOpty.optyName, active.SiebelOpty.optyStage));

                //SiebelWSFailedLog log = new SiebelWSFailedLog();
                //log.quoteId = active.QuotationMaster.quoteId;
                //log.FunctionName = "CreateSiebelOpty";
                //log.Parameters = pare.ToString();
                //log.EmailAddress = active.SiebelOpty.Opty_Owner_Email;
                //log.ErrorMessage = ex.Message;
                //log.InsertTime = DateTime.Now;
                //log.LastRecallTime = DateTime.Now;
                //log.IsRecall = false;
                //log.Add();

                //MailSubject.Append("(Error!)");
                //MessageSB.Append("CreateSiebelOpty exception <br />");
                //MessageSB.Append(ex.ToString());
                //MailMessage newEmail = new MailMessage();
                //newEmail.From = new MailAddress("myadvantech@advantech.com");
                //newEmail.To.Add(new MailAddress("myadvantech@advantech.com"));
                //newEmail.Subject = MailSubject.ToString();
                //newEmail.Body = MessageSB.ToString();
                //newEmail.IsBodyHtml = true;
                //newEmail.Priority = MailPriority.Normal;
                //smtpClient1.Send(newEmail);
                errmsg = ex.Message;
                return string.Empty;
            }

        }

        public static string CreateSiebelOptyV2(SiebelActive active, ref string wspara, ref string errmsg)
        {

            active.QuotationMaster.siebelRBU = active.QuotationMaster.siebelRBU.Replace("AMX", "ANADMF");
            StringBuilder _wspara = new StringBuilder();
            var strMeth = new WSSiebel_AddOpty.ADVWebServiceAddOppty();//.ADVWebServiceAddOpptyClient();
            var strAddOpty = new WSSiebel_AddOpty.OPPTY();
            var strOpptyInput = new WSSiebel_AddOpty.AddOppty_Input();
            var strOpptyOutput = new WSSiebel_AddOpty.AddOppty_Output();
            strAddOpty.ACC_ROW_ID = active.QuotationMaster.quoteToRowId;
            strAddOpty.BIZ_GROUP = "Industrial Automation";
            //strAddOpty.BUYCYCLE_STAGE = "3-Cust. commits funding";

            QuotationMasterHelper qmh = new QuotationMasterHelper();
            QuotationMaster qm = qmh.GetQuotationMaster(active.QuoteID);
            string _channel = string.Empty;
            if (qm != null)
            {
                if (qm.quoteNo.StartsWith("AACQ") || qm.quoteNo.StartsWith("GQ"))
                {
                    SIEBEL_ACCOUNT _Account = SiebelDAL.getSiebelAccount(active.QuotationMaster.quoteToRowId);

                    if (_Account != null)
                    {
                        int account_status_id = 0;
                        if (!string.IsNullOrEmpty(_Account.ACCOUNT_STATUS) && _Account.ACCOUNT_STATUS.Length > 2)
                        {
                            int.TryParse(_Account.ACCOUNT_STATUS.Substring(0, 2), out account_status_id);
                        }
                        switch (account_status_id)
                        {
                            case 1:
                            case 2:
                            case 3:
                                _channel = "CSF";
                                break;
                            case 4:
                            case 5:
                            case 6:
                                _channel = "KASF";
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                                if (qm.quoteNo.StartsWith("GQ"))
                                {
                                    _channel = "AOnline";
                                }
                                break;
                        }
                    }
                }
                else if (qm.quoteNo.StartsWith("AUSQ") || qm.quoteNo.StartsWith("TWQ") || qm.quoteNo.StartsWith("ACNQ") || qm.quoteNo.StartsWith("AKRQ"))
                {
                    _channel = "AOnline";
                }
            }


            //strAddOpty.CHANNEL = "KASF";
            strAddOpty.CHANNEL = _channel;
            strAddOpty.CLOSE_DATE = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");// "10/30/2016";
            strAddOpty.CONTRACT_DATE = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");//
            if(active.QuotationMaster.currency == "CNY")
                strAddOpty.CURRENCY_CODE = "RMB";
            else
                strAddOpty.CURRENCY_CODE = active.QuotationMaster.currency;
            strAddOpty.DESP = "";
            strAddOpty.IS_ASSIGN_TO_PARTNER = "Y";
            strAddOpty.LEAD_QUALITY = "Cool";
            strAddOpty.PROJ_NAME = active.OptyName;
            strAddOpty.REASON_WON_LOST = "Other";
            //ICC 20170918 For eQ3 Send amount value to Siebel directly.
            if (active.Amount.HasValue == true && active.Amount.Value > 0)
                strAddOpty.REVENUE = active.Amount.Value.ToString();
            else
                strAddOpty.REVENUE = active.QuotationMaster.Revenue.ToString();
            strAddOpty.SALES_METHOD = "Funnel Sales Methodology";
            strAddOpty.SALES_STAGE = active.OptyStage;

            if (!string.IsNullOrEmpty(qm.attentionRowId))
            {
                var optycontact = new WSSiebel_AddOpty.CONTACT();
                optycontact.CON_ID = qm.attentionRowId;
                optycontact.IS_PRIMARY = "Y";
                strAddOpty.CONTACT = new WSSiebel_AddOpty.CONTACT[] { optycontact };
            }

            //strAddOpty.SUCCESS_FACTOR = "Success Factor";
            //strAddOpty.SUPPORT_REQUEST = "Support Request";

            //ORG
            strAddOpty.ORG = new WSSiebel_AddOpty.ORG[1];

            var strAddOptyOrg = new WSSiebel_AddOpty.ORG();
            strAddOptyOrg.NAME = active.QuotationMaster.siebelRBU;
            strAddOptyOrg.IS_PRIMARY = "Y";
            strAddOpty.ORG[0] = strAddOptyOrg;

            //strAddOptyOrg = new WSSiebel_AddOpty.ORG();
            //strAddOptyOrg.NAME = "AJP";
            //strAddOpty.ORG[1] = strAddOptyOrg;

            //POSITION
            string PrimaryPositionName = string.Empty; string ContactID = string.Empty;
            if (IsSiebelOwner(active.OptyOwnerEmail, ref PrimaryPositionName, ref ContactID))
            {
                strAddOpty.POSITION = new WSSiebel_AddOpty.POSITION[1];

                var strAddOptyPosition = new WSSiebel_AddOpty.POSITION();
                strAddOptyPosition.NAME = PrimaryPositionName;
                strAddOptyPosition.IS_PRIMARY = "Y";
                strAddOpty.POSITION[0] = strAddOptyPosition;
            }
            //strAddOptyPosition = new WSSiebel_AddOpty.POSITION();
            //strAddOptyPosition.NAME = "CRM MEMBER - Candice.Yeh";
            //strAddOpty.POSITION[1] = strAddOptyPosition;

            //SOURCE

            //strAddOpty.SOURCE = new WSSiebel_AddOpty.SOURCE[1];

            //var strAddOptySource = new WSSiebel_AddOpty.SOURCE();
            //strAddOptySource.NAME = "   2014 智能醫院電子報7月號";
            //strAddOptySource.IS_PRIMARY = "Y";
            //strAddOpty.SOURCE[0] = strAddOptySource;

            strOpptyInput.OPPTY = strAddOpty;
            strOpptyInput.SOURCE = "test";
            strOpptyOutput = strMeth.AddOppty(strOpptyInput);
            string strStatus = strOpptyOutput.STATUS;
            string optyid = string.Empty;
            if (strStatus == "SUCCESS")
            {
                optyid = strOpptyOutput.ROW_ID;
                _wspara.Append("ACC_ROW_ID=" + strAddOpty.ACC_ROW_ID);
                _wspara.Append(",CLOSE_DATE=" + strAddOpty.CLOSE_DATE);
                _wspara.Append(",CURRENCY_CODE=" + strAddOpty.CURRENCY_CODE);
                _wspara.Append(",DESP=" + strAddOpty.DESP);
                _wspara.Append(",ORG=" + strAddOpty.ORG);
                _wspara.Append(",OWNER_EMAIL=" + active.OptyOwnerEmail);
                _wspara.Append(",PROJ_NAME=" + strAddOpty.PROJ_NAME);
                _wspara.Append(",SALES_METHOD=" + strAddOpty.SALES_METHOD);
                _wspara.Append(",SALES_STAGE=" + strAddOpty.SALES_STAGE);
                _wspara.Append(",REVENUE=" + strAddOpty.REVENUE);
                _wspara.Append(",CON_ROW_ID=" + ContactID);
                wspara = _wspara.ToString();

            }
            else
            {

                errmsg = strStatus + ";  " + strOpptyOutput.Error_spcCode + "; " + strOpptyOutput.Error_spcMessage;
                //if status is error, 就不會建立reocrd, 請提供error_spcCode and error_spcMessage讓我們查找
            }
            return optyid;
        }

        /// <summary>
        /// ICC 2016/3/8 Add new function for project registration to create Siebel opportunity
        /// </summary>
        /// <param name="pr">ProjectRegistration</param>
        /// <returns>Tuple</returns>
        public static Tuple<bool, string> CreateSiebelOpty4PrjReg(ProjectRegistration pr)
        {
            var strMeth = new WSSiebel_AddOpty.ADVWebServiceAddOppty();
            var strAddOpty = new WSSiebel_AddOpty.OPPTY();
            var strOpptyInput = new WSSiebel_AddOpty.AddOppty_Input();
            var strOpptyOutput = new WSSiebel_AddOpty.AddOppty_Output();

            strAddOpty.BIZ_GROUP = "Industrial Automation";

            //ICC 2016/3/22 Prevent negative revenue
            if (!string.IsNullOrEmpty(pr.Revenue))
            {
                decimal amount = 0;
                decimal.TryParse(pr.Revenue, out amount);
                if (amount > 0) //ICC 2016/10/28 Must more than zero
                    strAddOpty.REVENUE = pr.Revenue;
            }

            strAddOpty.CLOSE_DATE = pr.Close_Date;
            strAddOpty.CONTRACT_DATE = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            strAddOpty.CURRENCY_CODE = pr.Currency;
            strAddOpty.DESP = string.Empty;
            strAddOpty.IS_ASSIGN_TO_PARTNER = "Y";
            strAddOpty.LEAD_QUALITY = "Cool";
            strAddOpty.PROJ_NAME = pr.Project_Name;
            strAddOpty.REASON_WON_LOST = "Other";
            strAddOpty.SALES_METHOD = "Funnel Sales Methodology";
            strAddOpty.SALES_STAGE = "10% Validating";
            strAddOpty.DESP = pr.Description; //Description
            strAddOpty.CHANNEL = "Registered by CP"; //2016/7/11 Add channel for project registration

            //Partner
            //var partner = new WSSiebel_AddOpty.PARTNER();
            //partner.PARTNER_ID = pr.Account_Row_ID;
            //strAddOpty.PARTNER = new WSSiebel_AddOpty.PARTNER[] { partner };
            strAddOpty.PARTNER_ID = pr.Account_Row_ID;

            if (!string.IsNullOrEmpty(pr.Contact_Row_ID))
                strAddOpty.PARTNER_CON_ID = pr.Contact_Row_ID;

            //Contact
            //if (!string.IsNullOrEmpty(contact_row_id))
            //{
            //    var optycontact = new WSSiebel_AddOpty.CONTACT();
            //    optycontact.CON_ID = contact_row_id;
            //    optycontact.IS_PRIMARY = "Y";
            //    strAddOpty.CONTACT = new WSSiebel_AddOpty.CONTACT[] { optycontact };
            //}

            //ORG
            strAddOpty.ORG = new WSSiebel_AddOpty.ORG[1];
            var strAddOptyOrg = new WSSiebel_AddOpty.ORG();
            strAddOptyOrg.NAME = pr.RBU;
            strAddOptyOrg.IS_PRIMARY = "Y";
            strAddOpty.ORG[0] = strAddOptyOrg;

            //Position
            string PrimaryPositionName = string.Empty; string ContactID = string.Empty;
            if (IsSiebelOwner(pr.Owner_Email, ref PrimaryPositionName, ref ContactID))
            {
                strAddOpty.POSITION = new WSSiebel_AddOpty.POSITION[1];
                var strAddOptyPosition = new WSSiebel_AddOpty.POSITION();
                strAddOptyPosition.NAME = PrimaryPositionName;
                strAddOptyPosition.IS_PRIMARY = "Y";
                strAddOpty.POSITION[0] = strAddOptyPosition;
            }

            //Competition
            if (!string.IsNullOrEmpty(pr.Competition))
                strAddOpty.COMPETITION = pr.Competition;

            //Product
            if (pr.Products != null && pr.Products.Count > 0)
            {
                strAddOpty.PRODUCT = new WSSiebel_AddOpty.PRODUCT[1];
                var strAddOptyProduct = new WSSiebel_AddOpty.PRODUCT();
                strAddOptyProduct.NAME = pr.Products[0].Main_Product;
                strAddOptyProduct.QTY = pr.Products[0].Main_Product_Qty;
                strAddOpty.PRODUCT[0] = strAddOptyProduct;
            }

            strOpptyInput.OPPTY = strAddOpty;
            strOpptyInput.SOURCE = "test";

            try
            {
                strOpptyOutput = strMeth.AddOppty(strOpptyInput);
                string strStatus = strOpptyOutput.STATUS;

                if (strStatus.ToUpper() == "SUCCESS")
                    return new Tuple<bool, string>(true, strOpptyOutput.ROW_ID);
                else
                    return new Tuple<bool, string>(false, strStatus + ";  " + strOpptyOutput.Error_spcCode + "; " + strOpptyOutput.Error_spcMessage);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.ToString());
            }

        }

        public static bool IsSiebelOwner(string OwnerEmail, ref string PrimaryPositionName, ref string ContactID, string User = "")
        {
            try
            {
                OwnerEmail = OwnerEmail.Trim();
                WSSiebel_New.WSSiebel ws = new WSSiebel_New.WSSiebel();
                WSSiebel_New.RESULT res = new WSSiebel_New.RESULT();

                int num = 0;

                while ((res.ERR_MSG == null || res.ERR_MSG == "Login Failed") && num < 3)
                {
                    if (num != 0)
                    {
                        System.Threading.Thread.Sleep(5 * 1000);
                    }

                    res = ws.CheckEmployee(ConfigurationManager.AppSettings["CRMHQId"], OwnerEmail);
                    num += 1;
                }

                if (res.IS_COMMITTED)
                {
                    ContactID = res.ROW_ID;
                    PrimaryPositionName = res.RESULT_A;

                }
                return res.IS_COMMITTED;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        ///// <summary>
        ///// Update Siebel Opportunity through Siebel web service locate: http://172.20.1.43:8089/WSSiebel.asmx
        ///// This method only update Stage, Revenue column
        ///// </summary>
        ///// <param name="OptyID"></param>
        ///// <param name="Stage"></param>
        ///// <param name="Revenue"></param>
        ///// <returns>Opportunity ROW_ID</returns>
        //public static string UpdateOptyStage(string OptyID, string Stage, int Revenue)
        //{
        //    System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient("172.20.0.76");
        //    var ws = new SiebelWebService.WSSiebel();
        //    var emp = new SiebelWebService.EMPLOYEE();
        //    emp.USER_ID = ConfigurationManager.AppSettings["CRMHQId"];
        //    emp.PASSWORD = ConfigurationManager.AppSettings["CRMHQPwd"];
        //    var res = new SiebelWebService.RESULT();
        //    try
        //    {
        //        var opty = new SiebelWebService.OPPTY();
        //        opty.ROW_ID = OptyID;
        //        opty.SALES_STAGE = Stage;
        //        opty.REVENUE = Revenue;
        //        res = ws.UpdOppty(emp, opty);
        //        smtpClient1.Send("myadvantech@advantech.com", "ming.zhao@advantech.com.cn", "UPDATE: OptyStage  succssed", res.ROW_ID);
        //        return res.ROW_ID;
        //    }
        //    catch (Exception ex)
        //    {
        //        smtpClient1.Send("myadvantech@advantech.com", "ming.zhao@advantech.com.cn", "UPDATE: OptyStage  Failed", ex.ToString());
        //        //return ex.Message.ToString();
        //    }
        //    return string.Empty;
        //}

        /// <summary>
        /// Ryan Huang 2015/12/04. Update UpdateOptyStageV2 with new Web-Service"UpdOppty".
        /// </summary>
        /// <param name="OptyID"></param>
        /// <param name="Stage"></param>
        /// <param name="Revenue"></param>
        /// <param name="desc"></param>
        /// <param name="closeDate"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool UpdateOptyStageV2(string OptyID, string Stage, int? Revenue, string desc, DateTime? closeDate, ref string msg)
        {
            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient("172.20.0.76");

            //UpdOpty
            var strMeth = new UpdOppty.ADVWebServiceUpdOppty();
            var strOppty = new UpdOppty.OPPTY();
            var OpptyInput = new UpdOppty.UpdOppty_Input();
            var OpptyOut = new UpdOppty.UpdOppty_Output();

            strOppty.ROW_ID = OptyID;
            strOppty.SALES_STAGE = Stage;
            OpptyInput.OPPTY = strOppty;
            OpptyInput.SOURCE = "eQuotation"; //It's Required.     

            if (Revenue.HasValue) strOppty.REVENUE = Revenue.Value.ToString();
            strOppty.DESP = desc;
            if (closeDate.HasValue) strOppty.CLOSE_DATE = closeDate.Value.ToString();
            if (!String.IsNullOrEmpty(desc)) strOppty.DESP = desc;
            OpptyOut = strMeth.UpdOppty(OpptyInput);

            string strStatus = OpptyOut.STATUS;
            if (!String.IsNullOrEmpty(strStatus) && strStatus.Equals("SUCCESS", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                string strErrorMesg = OpptyOut.Error_spcMessage;
                string strErrorCode = OpptyOut.Error_spcCode;
                msg = strErrorMesg + strErrorCode;
                //If STATUS is not SUCCESS, 就不會建立reocrd,請提供Error_spcCode & Error_spcMessage讓我們追蹤錯誤原因
                return false;
            }
        }

        ///// <summary>
        ///// Update Siebel Opportunity through Siebel web service locate: http://172.20.1.43:8089/WSSiebel.asmx
        ///// This method will update total parameters of OPPTY
        ///// </summary>
        ///// <param name="active"></param>
        ///// <returns>Opportunity ROW_ID</returns>
        //public static bool UpdateSiebelOpty(SiebelActive active, ref string wsparas, ref string errmsg)
        //{
        //    var ws = new SiebelWebService.WSSiebel();
        //    var emp = new SiebelWebService.EMPLOYEE();
        //    emp.USER_ID = ConfigurationManager.AppSettings["CRMHQId"];
        //    emp.PASSWORD = ConfigurationManager.AppSettings["CRMHQPwd"];
        //    var Result = new SiebelWebService.RESULT();
        //    StringBuilder _wspars = new StringBuilder();
        //    StringBuilder MailSubject = new StringBuilder("Update Opportunity by eQuotation ");
        //    StringBuilder MessageSB = new StringBuilder();

        //    try
        //    {
        //        var Opty = new SiebelWebService.OPPTY();
        //        Opty.ROW_ID = active.OptyID;

        //        if (!string.IsNullOrEmpty(active.OptyOwnerEmail)) Opty.OWNER_EMAIL = active.OptyOwnerEmail;

        //        if (!string.IsNullOrEmpty(active.OptyName)) Opty.PROJ_NAME = active.OptyName;

        //        if (!string.IsNullOrEmpty(active.OptyStage)) Opty.SALES_STAGE = active.OptyStage;

        //        Opty.REVENUE = Convert.ToInt32(active.Amount.GetValueOrDefault(0)); //Amount有值就帶

        //        _wspars.Append("ROW_ID=" + Opty.ROW_ID);
        //        _wspars.Append(",OWNER_EMAIL=" + Opty.OWNER_EMAIL);
        //        _wspars.Append(",PROJ_NAME=" + Opty.PROJ_NAME);
        //        _wspars.Append(",SALES_STAGE=" + Opty.SALES_STAGE);
        //        _wspars.Append(",REVENUE=" + Opty.REVENUE);
        //        wsparas = _wspars.ToString();

        //        Result = ws.UpdOppty(emp, Opty);

        //        if (Result != null)
        //        {
        //            errmsg = Result.ERR_MSG;
        //            if (!string.IsNullOrEmpty(errmsg))
        //            {
        //                return false;
        //            }

        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        errmsg = ex.Message;
        //        return false;
        //    }

        //}

        /// <summary>
        /// Ryan Huang 2015/12/03. Update new UpdOppty.ws and it's function(UpdateSiebelOptyV2)
        /// </summary>
        /// <param name="active"></param>
        /// <param name="wsparas"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static bool UpdateSiebelOptyV2(SiebelActive active, ref string wsparas, ref string errmsg)
        {

            //UpdOpty
            var strMeth = new UpdOppty.ADVWebServiceUpdOppty();
            var strOppty = new UpdOppty.OPPTY();
            var OpptyInput = new UpdOppty.UpdOppty_Input();
            var OpptyOut = new UpdOppty.UpdOppty_Output();

            StringBuilder _wspars = new StringBuilder();

            //var strOpptyPOS = new UpdOppty.POSITION();
            //var strOpptyORG = new UpdOppty.ORG();
            //var strOpptyPartner = new UpdOppty.PARTNER();
            //var strOpptyContact = new UpdOppty.CONTACT();
            //var strOpptySource = new UpdOppty.SOURCE();

            strOppty.ROW_ID = active.OptyID; //It's Required.
            //strOppty.ACC_ROW_ID = "1-Q2RINF";
            //strOppty.IS_ASSIGN_TO_PARTNER = "Y";
            //strOppty.BIZ_GROUP = "Applied Computing";
            //strOppty.BUYCYCLE_STAGE = "7-Cust signs contract / P.O.";
            //strOppty.CHANNEL = "AOnline";
            //strOppty.CONTRACT_DATE = "12/23/2015";
            //strOppty.SUCCESS_FACTOR = "success factor";
            //strOppty.CURRENCY_CODE = "AUD";
            //strOppty.DESP = "WebService inbound";
            //strOppty.PROJ_NAME = "eQuotation Project 20151123";
            //ICC 2017/3/7 For MyAdvantech order doesn't have QuoteID, we change to use amount as revenue to update Siebel opportunity.
            if (string.IsNullOrEmpty(active.QuoteID))
                strOppty.REVENUE = active.Amount.ToString();
            else
            {
                strOppty.REVENUE = active.QuotationMaster.Revenue.ToString();

                //Ryan 20180316 Send close date to Siebel except AEU/ATW per Patty's request.
                if ((!String.IsNullOrEmpty(active.QuotationMaster.org) && !active.QuotationMaster.org.ToUpper().Equals("EU10"))
                    &&
                    (!String.IsNullOrEmpty(active.QuotationMaster.siebelRBU) && !active.QuotationMaster.siebelRBU.ToUpper().Equals("ATW")))
                {
                    strOppty.CLOSE_DATE = ((DateTime)active.CreatedDate).ToString("MM/dd/yyyy");
                }
            }
            //strOppty.CLOSE_DATE = "02/15/2016";
            //strOppty.PROGRAM = "program23";
            //strOppty.LEAD_QUALITY = "Hot";
            //strOppty.REASON_WON_LOST = "Price";
            //strOppty.SALES_METHOD = "DMS Sales Methodology";
            strOppty.SALES_STAGE = active.OptyStage.ToString();
            //strOppty.STATUS = "Pending";
            //strOppty.SUPPORT_REQUEST = "support request23";

            //Position
            //第一組
            //stroppty.position = new updoppty.position[2];
            //stropptypos.name = "crm member - candice.yeh";
            //stropptypos.is_primary = "y"; //設 y 為 primary owner
            //stroppty.position[0] = stropptypos;
            ////第二組
            //stropptypos = new updoppty.position();
            //stropptypos.name = "09.iag/product ae - john.chuang";
            //stroppty.position[1] = stropptypos;

            ////organization
            ////第一組
            //stroppty.org = new updoppty.org[2];
            //stropptyorg.name = "atw";
            //stroppty.org[0] = stropptyorg;
            ////第二組
            //stropptyorg = new updoppty.org();
            //stropptyorg.name = "abr";
            //stropptyorg.is_primary = "y"; //設 y 為 primary organization
            //stroppty.org[1] = stropptyorg;

            ////partner
            //stroppty.partner = new updoppty.partner[1];
            //stropptypartner.partner_id = "ei-1tp65";
            //stroppty.partner[0] = stropptypartner;

            ////contact
            //stroppty.contact = new updoppty.contact[1];
            //stropptycontact.con_id = "1+aa+1016";
            //stroppty.contact[0] = stropptycontact;

            ////source
            ////第一組
            //stroppty.source = new updoppty.source[2];
            //stropptysource.name = "研华远程i/o模块与plc无“限”畅连--forebn";
            //stroppty.source[0] = stropptysource;

            ////第二組
            //stropptysource = new updoppty.source();
            //stropptysource.name = "研华webaccess+ 物联产业联盟行业应用论坛暨伙伴峰会";
            //stroppty.source[1] = stropptysource;

            OpptyInput.OPPTY = strOppty;
            OpptyInput.SOURCE = "eQuotation"; //It's Required.
            OpptyOut = strMeth.UpdOppty(OpptyInput);

            _wspars.Append("ROW_ID=" + strOppty.ROW_ID);
            _wspars.Append(",PROJ_NAME=" + strOppty.PROJ_NAME);
            _wspars.Append(",SALES_STAGE=" + strOppty.SALES_STAGE);
            _wspars.Append(",REVENUE=" + strOppty.REVENUE);
            wsparas = _wspars.ToString();

            string strStatus = OpptyOut.STATUS;
            if (!String.IsNullOrEmpty(strStatus) && strStatus.Equals("SUCCESS", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                string strErrorMesg = OpptyOut.Error_spcMessage;
                string strErrorCode = OpptyOut.Error_spcCode;
                errmsg = strErrorMesg + strErrorCode;
                //If STATUS is not SUCCESS, 就不會建立reocrd,請提供Error_spcCode & Error_spcMessage讓我們追蹤錯誤原因
                return false;
            }

        }

        /// <summary>
        /// Ryan Huang 2015/12/03. Update new UpdOppty.ws and it's function(UpdateSiebelOptyV2)
        /// </summary>
        /// <param name="QM"></param>
        /// <param name="wsparas"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static bool UpdateForecast(QuotationMaster QM, ref string errmsg)
        {
            //UpdOpty
            var strMeth = new UpdOppty.ADVWebServiceUpdOppty();
            var strOppty = new UpdOppty.OPPTY();
            var OpptyInput = new UpdOppty.UpdOppty_Input();
            var OpptyOut = new UpdOppty.UpdOppty_Output();

            strOppty.ROW_ID = QM.QuotationOpty.optyId;

            var num = QM.QuotationDetail.Count;
            strOppty.PRODUCT = new UpdOppty.PRODUCT[num];
            for (int i = 0; i < num; i++)
            {
                var strOpptyProduct = new UpdOppty.PRODUCT();
                strOpptyProduct.NAME = QM.QuotationDetail[i].partNo;
                strOpptyProduct.QTY = QM.QuotationDetail[i].qty.GetValueOrDefault().ToString();
                //strOpptyProduct.IS_PRIMARY = "Y";
                strOppty.PRODUCT[i] = strOpptyProduct;
            }

            OpptyInput.OPPTY = strOppty;
            OpptyInput.SOURCE = "eQuotation"; //It's Required.
            OpptyOut = strMeth.UpdOppty(OpptyInput);

            string strStatus = OpptyOut.STATUS;
            if (!String.IsNullOrEmpty(strStatus) && strStatus.Equals("SUCCESS", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                string strErrorMesg = OpptyOut.Error_spcMessage;
                string strErrorCode = OpptyOut.Error_spcCode;
                errmsg = strErrorMesg + strErrorCode;
                //If STATUS is not SUCCESS, 就不會建立reocrd,請提供Error_spcCode & Error_spcMessage讓我們追蹤錯誤原因
                return false;
            }

        }

        /// <summary>
        /// ICC 2016/3/8 Add new function for project registration to update Siebel opportunity
        /// </summary>
        /// <param name="optyID"></param>
        /// <param name="revenue"></param>
        /// <param name="stage"></param>
        /// <param name="status"></param>
        /// <param name="closeDate"></param>
        /// <param name="lPartner_ID"></param>
        /// <param name="lPartnerContact_ID"></param>
        /// <returns></returns>
        public static Tuple<bool, string> UpdateSiebelOpty4PrjReg(string optyID, string revenue, string stage, string status, string closeDate, string lPartner_ID = "", string lPartnerContact_ID = "", string desp = "", string Competition = "", List<ProjectRegistrationProduct> products = null)
        {
            var strMeth = new UpdOppty.ADVWebServiceUpdOppty();
            var strOppty = new UpdOppty.OPPTY();
            var OpptyInput = new UpdOppty.UpdOppty_Input();
            var OpptyOut = new UpdOppty.UpdOppty_Output();
            //var partner = new UpdOppty.PARTNER();
            //Update opty do not update product temporarily.

            strOppty.ROW_ID = optyID; //Opportunity is required.

            //ICC 2016/3/22 Prevent negative revenue
            if (!string.IsNullOrEmpty(revenue))
            {
                decimal amount = 0;
                decimal.TryParse(revenue, out amount);
                if (amount > 0)  //ICC 2016/10/28 Must more than zero
                    strOppty.REVENUE = revenue;
            }

            if (!string.IsNullOrEmpty(stage)) //percentage
                strOppty.SALES_STAGE = stage;

            if (!string.IsNullOrEmpty(status)) //Status
                strOppty.STATUS = status;

            if (!string.IsNullOrEmpty(closeDate))
                strOppty.CLOSE_DATE = closeDate;

            if (!string.IsNullOrEmpty(lPartner_ID))
            {
                strOppty.PARTNER_ID = lPartner_ID;
                strOppty.IS_ASSIGN_TO_PARTNER = "Y";
                //partner.PARTNER_ID = lPartner_ID;
                //strOppty.PARTNER = new UpdOppty.PARTNER[] { partner };

                if (!string.IsNullOrEmpty(lPartnerContact_ID))
                    strOppty.PARTNER_CON_ID = lPartnerContact_ID;
            }

            if (!string.IsNullOrEmpty(desp))
                strOppty.DESP = desp.Trim();

            //Competition
            if (!string.IsNullOrEmpty(Competition))
                strOppty.COMPETITION = Competition;

            //Product
            if (products != null && products.Count > 0)
            {
                strOppty.PRODUCT = new UpdOppty.PRODUCT[1];
                var strAddOptyProduct = new UpdOppty.PRODUCT();
                strAddOptyProduct.NAME = products[0].Main_Product;
                strAddOptyProduct.QTY = products[0].Main_Product_Qty;
                strOppty.PRODUCT[0] = strAddOptyProduct;
            }

            OpptyInput.OPPTY = strOppty;
            OpptyInput.SOURCE = "MyAdvantech"; //Source is required

            try
            {
                OpptyOut = strMeth.UpdOppty(OpptyInput);
                string strStatus = OpptyOut.STATUS;

                if (!string.IsNullOrEmpty(strStatus) && strStatus.ToUpper() == "SUCCESS")
                    return new Tuple<bool, string>(true, string.Empty);
                else
                    return new Tuple<bool, string>(false, strStatus + ";" + OpptyOut.Error_spcCode + ";" + OpptyOut.Error_spcMessage);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.ToString());
            }

        }

        public static DataTable GetSiebelOptyList(String _OptyName, String _AccountRowID, String _OwnerID, bool _isACN = false)
        {
            string sql1 = string.Empty;
            string sql2 = string.Empty;
            if (_isACN == true)
            {
                sql1 = " INNER JOIN S_POSTN P  ON A.PR_POSTN_ID = P.ROW_ID INNER JOIN S_CONTACT R ON P.PR_EMP_ID = R.ROW_ID ";
                sql2 = string.Format(" AND LOWER(R.EMAIL_ADDR) = LOWER('{0}') ", _OwnerID);
            }

            String str = String.Format(@"SELECT CASE A.CURR_STG_ID WHEN '1-VXVAID' THEN 'c' WHEN '1-VXVAIE' THEN 'b' ELSE 'a' END AS orderbystr, 
                                A.ROW_ID, A.PR_DEPT_OU_ID AS ACCOUNT_ROW_ID, A.CURR_STG_ID, A.NAME, A.CREATED, B.NAME AS ACCOUNT_NAME, A.CREATED_BY, 
                                ISNULL((SELECT TOP (1) EMAIL_ADDR FROM S_CONTACT AS z WHERE (ROW_ID = A.CAMP_CON_ID)), N'') AS CONTACT, 
                                ISNULL((SELECT TOP (1) NAME FROM S_STG AS z 
                                WHERE (ROW_ID = A.CURR_STG_ID) AND (UPPER(NAME) <> UPPER('0% Lost')) AND (UPPER(NAME) <> UPPER('100% Won-PO Input in SAP'))
                                ), N'') AS STAGE_NAME, 
                                ISNULL((SELECT TOP (1) su.LOGIN FROM S_POSTN AS z LEFT OUTER JOIN S_USER AS su ON z.PR_EMP_ID = su.ROW_ID WHERE (z.ROW_ID = A.PR_POSTN_ID)), N'') AS [PRIMARY], 
                                ISNULL((SELECT TOP (1) NAME FROM S_ORG_EXT AS z WHERE (ROW_ID = A.PR_PRTNR_ID)), N'') AS ChannelPartner, A.CURCY_CD, A.SUM_REVN_AMT, A.PR_DEPT_OU_ID AS ACCOUNT_ROW_ID, A.SUM_WIN_PROB, A.STATUS_CD, 
                                ISNULL((SELECT NAME FROM S_SALES_METHOD AS SM WHERE (ROW_ID = A.SALES_METHOD_ID)), N'') AS SALES_METHOD_NAME, ISNULL(A.CHANNEL_TYPE_CD, N'') AS Channel, A.SUM_EFFECTIVE_DT
                                FROM S_OPTY AS A 
                                INNER JOIN S_ORG_EXT AS B ON A.PR_DEPT_OU_ID = B.ROW_ID OR A.PR_PRTNR_ID = B.ROW_ID 
                                LEFT OUTER JOIN S_OPTY_X AS X ON A.ROW_ID = X.ROW_ID
                                LEFT JOIN S_STG S ON A.CURR_STG_ID = S.ROW_ID 
                                {2} 
                                WHERE (A.PR_DEPT_OU_ID IS NOT NULL) AND (UPPER(A.NAME) LIKE N'%{0}%') AND (B.ROW_ID = '{1}') 
                                and S.NAME in('0% Lost','10% Validating','100% Won-PO Input in SAP','25% Proposing/Quoting','40% Testing','5% New Lead'
                                ,'50% Negotiating','75% Waiting for PO/Approval','90% Expected Flow Business','Rejected by Partner','Rejected by Sales')
                                {3}  
                                ORDER BY   orderbystr, A.CREATED DESC", _OptyName.Replace("'", "''").Replace("*", "%").ToUpper(), _AccountRowID, sql1, sql2);
            DataTable dt = SqlProvider.dbGetDataTable("CRM", str);
            return dt;
        }

        #endregion

        #region Siebel Contact

        /// <summary>
        /// Create Siebel Contact through Siebel web service locate: http://172.20.1.43:8089/WSSiebel.asmx
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Row ID</returns>
        public static string CreateSiebelContactByWS(SIEBEL_CONTACT contact)
        {
            SmtpClient smtpClient1 = new SmtpClient("172.20.0.76");
            MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "MyAdvantech@advantech.com");
            var ws = new WSSiebel_AddContact.ADVWebServiceAddContact();
            ws.Timeout = _SiebelWSTimeOut;
            var WScontact = new WSSiebel_AddContact.CON();

            //var ws = new SiebelWebService.WSSiebel();
            //ws.Timeout = _SiebelWSTimeOut;
            //var emp = new SiebelWebService.EMPLOYEE();
            //var WScontact = new SiebelWebService.CONTACT();
            //emp.USER_ID = ConfigurationManager.AppSettings["CRMHQId"];
            //emp.PASSWORD = ConfigurationManager.AppSettings["CRMHQPwd"];
            //var res = new SiebelWebService.RESULT();

            try
            {
                if (contact.ACCOUNT_ROW_ID != "")
                {
                    var objAcc = new WSSiebel_AddContact.ACC();
                    objAcc.ACC_ROW_ID = contact.ACCOUNT_ROW_ID;
                    WScontact.ACC = new WSSiebel_AddContact.ACC[] { objAcc };
                }

                WScontact.EMAIL = contact.EMAIL_ADDRESS;//*
                WScontact.FST_NAME = contact.FirstName;//*
                WScontact.LST_NAME = contact.LastName;//*
                WScontact.JOB_TITLE = contact.JOB_TITLE;
                WScontact.WORK_PHONE = contact.WorkPhone;//*
                WScontact.FAX_PHONE = contact.FaxNumber;
                WScontact.IS_NEVER_EMAIL = contact.IsNeverEmail == true ? "Y" : "N";//*

                //Position
                var objPos = new WSSiebel_AddContact.POSITION();
                if (!string.IsNullOrEmpty(contact.OwnerId))
                {
                    objPos.NAME = GetSiebelPositionNameByPostnid(contact.OwnerId);
                }
                else
                {
                    //Set Account Owner as Contact Owner
                    if (contact.ACCOUNT_ROW_ID != "")
                    {
                        objPos.NAME = getSiebelAccount(contact.ACCOUNT_ROW_ID).PriOwnerPosition;
                    }
                }
                objPos.NAME = string.IsNullOrEmpty(objPos.NAME) ? "System Use - MyAdvantech" : objPos.NAME;
                objPos.IS_PRIMARY = "Y";
                WScontact.POSITION = new WSSiebel_AddContact.POSITION[] { objPos };

                //Org
                if (!string.IsNullOrEmpty(contact.OrgID))
                {
                    var objOrg = new WSSiebel_AddContact.ORG();
                    objOrg.NAME = contact.OrgID;
                    objOrg.IS_PRIMARY = "Y";
                    WScontact.ORG = new WSSiebel_AddContact.ORG[] { objOrg };
                }

                //Interested Product
                if (contact.InterestedProduct.Length > 0)
                {
                    var ListOfIntProd = new List<WSSiebel_AddContact.LIST_IP>();
                    foreach (string intprod in contact.InterestedProduct)
                    {
                        var objIntProd = new WSSiebel_AddContact.LIST_IP();
                        objIntProd.NAME = intprod;
                        ListOfIntProd.Add(objIntProd);
                    }
                    WScontact.LIST_IP = ListOfIntProd.ToArray();
                }

                //BAA
                if (contact.BAA.Length > 0)
                {
                    var ListOfBAA = new List<WSSiebel_AddContact.BAA>();
                    foreach (string baa in contact.BAA)
                    {
                        var objBAA = new WSSiebel_AddContact.BAA();
                        objBAA.NAME = baa;
                        ListOfBAA.Add(objBAA);
                    }
                    WScontact.BAA = ListOfBAA.ToArray();
                }

                var ConInput = new WSSiebel_AddContact.AddContact_Input();
                ConInput.CON = WScontact;
                ConInput.SOURCE = "MyAdvantech";

                //Currently, privilege data can not be created and updated through Siebel web service
                var res = new WSSiebel_AddContact.AddContact_Output();
                res = ws.AddContact(ConInput);
                //res = ws.AddContact(emp, WScontact);
                //if (!string.IsNullOrEmpty(res.ERR_MSG))
                if (res.STATUS != "SUCCESS")
                {
                    mail.Subject = "Create Siebel Contact  Failed";
                    mail.IsBodyHtml = true;
                    mail.Body = string.Format("Row ID: {0}<br />Status: {1}<br />Error Code: {2}<br />Error Message: {3}", contact.ROW_ID, res.STATUS, res.Error_spcCode, res.Error_spcMessage);
                    smtpClient1.Send(mail);
                    return string.Empty;
                }
                else
                    return res.ROW_ID;
            }
            catch (Exception ex)
            {
                mail.Subject = "Create Siebel Contact  Failed - exception";
                mail.IsBodyHtml = true;
                mail.Body = string.Format("Row ID: {0}<br />Message: {1}<br />Exception: {2}", contact.ROW_ID, ex.Message, ex.ToString());
                smtpClient1.Send(mail);
                return string.Empty;
            }
        }

        /// <summary>
        /// Update Siebel Contact through Siebel web service locate: http://172.20.1.43:8089/WSSiebel.asmx
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>bool</returns>
        public static bool UpdateSiebelContactByWS(SIEBEL_CONTACT contact)
        {
            SmtpClient smtpClient1 = new SmtpClient("172.20.0.76");
            MailMessage mail = new MailMessage("MyAdvantech@advantech.com", "MyAdvantech@advantech.com");
            var ws = new WSSiebel_UpdContact.ADVWebServiceUpdateContact();
            var WScontact = new WSSiebel_UpdContact.CON();

            //var ws = new SiebelWebService.WSSiebel();
            //var emp = new SiebelWebService.EMPLOYEE();
            //var WScontact = new SiebelWebService.CONTACT();
            //emp.USER_ID = ConfigurationManager.AppSettings["CRMUNICAId"];
            //emp.PASSWORD = ConfigurationManager.AppSettings["CRMUNICAPwd"];
            //var res = new SiebelWebService.RESULT();
            //Currently, privilege data can not be created and updated through Siebel web service
            try
            {
                // *表示必填
                if (!string.IsNullOrEmpty(contact.ACCOUNT_ROW_ID))
                {
                    var objAcc = new WSSiebel_UpdContact.ACC();
                    objAcc.ACC_ROW_ID = contact.ACCOUNT_ROW_ID;
                    WScontact.ACC = new WSSiebel_UpdContact.ACC[] { objAcc };
                }

                //WScontact.ACC_ROW_ID = contact.ACCOUNT_ROW_ID;//不確定
                WScontact.ROW_ID = contact.ROW_ID;//*
                //WScontact.EMAIL = contact.EMAIL_ADDRESS;//*
                WScontact.FST_NAME = contact.FirstName;
                WScontact.LST_NAME = contact.LastName;
                WScontact.JOB_FUNCTION = contact.JOB_FUNCTION;//*
                WScontact.JOB_TITLE = contact.JOB_TITLE;
                WScontact.WORK_PHONE = contact.WorkPhone;//*
                WScontact.FAX_PHONE = contact.FaxNumber;
                WScontact.IS_NEVER_EMAIL = contact.IsNeverEmail == true ? "Y" : "N";//*
                WScontact.IS_ACTIVE = contact.ACTIVE_FLAG; // ICC 2015/9/18 Add Active flag
                if (contact.InterestedProduct != null && contact.InterestedProduct.Length > 0)
                {
                    var ListOfIntProd = new List<WSSiebel_UpdContact.LIST_IP>();
                    foreach (string intprod in contact.InterestedProduct)
                    {
                        var objIntProd = new WSSiebel_UpdContact.LIST_IP();
                        objIntProd.NAME = intprod;
                        ListOfIntProd.Add(objIntProd);
                    }
                    WScontact.LIST_IP = ListOfIntProd.ToArray();
                }

                if (contact.BAA != null && contact.BAA.Length > 0)
                {
                    var ListOfBAA = new List<WSSiebel_UpdContact.BAA>();
                    foreach (string baa in contact.BAA)
                    {
                        var objBAA = new WSSiebel_UpdContact.BAA();
                        objBAA.NAME = baa;
                        ListOfBAA.Add(objBAA);
                    }
                    WScontact.BAA = ListOfBAA.ToArray();
                }

                var ConInput = new WSSiebel_UpdContact.UpdContact_Input();
                ConInput.CON = WScontact;
                ConInput.SOURCE = "MyAdvantech";

                var res = ws.UpdContact(ConInput);
                //res = ws.UpdContact(emp, WScontact);
                if (res.STATUS != "SUCCESS")
                {
                    mail.Subject = "Update Siebel Contact  Failed";
                    mail.IsBodyHtml = true;
                    mail.Body = string.Format("Row ID: {0}<br />Status: {1}<br />Error Code: {2}<br />Error Message: {3}", contact.ROW_ID, res.STATUS, res.Error_spcCode, res.Error_spcMessage);
                    smtpClient1.Send(mail);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                mail.Subject = "Update Siebel Contact  Failed - exception";
                mail.IsBodyHtml = true;
                mail.Body = string.Format("Row ID: {0}<br />Message: {1}<br />Exception: {2}", contact.ROW_ID, ex.Message, ex.ToString());
                smtpClient1.Send(mail);
                return false;
            }

        }

        /// <summary>
        /// Get Siebel Contact from MyGlobal by Contact Row ID
        /// </summary>
        /// <param name="RowID"></param>
        /// <returns></returns>
        public static SIEBEL_CONTACT GetSiebelContact(string RowID)
        {
            return MyAdvantechContext.Current.SIEBEL_CONTACT.Find(RowID);
        }

        public static List<SIEBEL_CONTACT> GetSiebelContactByEMAIL(string EMAIL)
        {
            return MyAdvantechContext.Current.SIEBEL_CONTACT.Where(d => d.EMAIL_ADDRESS.Equals(EMAIL, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        #endregion

        #region -- For table [eQuotation].[dbo].[SiebelActive] logic functions --

        public static List<SiebelActive> GetSiebelActiveByQuoteID(string eQuoteID)
        {
            return eQuotationContext.Current.SiebelActive.Where(p => p.QuoteID == eQuoteID && p.ActiveType == "CreateQuote").ToList();
        }

        public static List<SiebelActive> GetSiebelActiveByQuoteID(string eQuoteID, int ID)
        {
            var result = (from active in eQuotationContext.Current.SiebelActive
                          where active.QuoteID == eQuoteID
                          select active).ToList();

            return result;
        }

        public static List<SiebelActive> GetActiveList(SiebelActiveStatus status, SiebelActiveType type, int count = 10)
        {
            try
            {
                var result = (from active in eQuotationContext.Current.SiebelActive
                              where active.Status == status.ToString() && active.ActiveType == type.ToString()
                              orderby active.CreatedDate
                              select active)
                              .Take(count)
                              .ToList();

                return result;
            }
            catch
            {
                return new List<SiebelActive>();
            }

        }

        public static List<QuotationMaster> GetQuotationMasterForProductForecast(DateTime dt)
        {
            try
            {
                // Ryan 20180627 - Do not get ACN quote master for forecast
                var result = (from qm in eQuotationContext.Current.QuotationMaster
                              where qm.DOCSTATUS == 1
                              && qm.LastUpdatedDate.Value.Year == dt.Year
                              && qm.LastUpdatedDate.Value.Month == dt.Month
                              && qm.LastUpdatedDate.Value.Day == dt.Day
                              && !qm.org.StartsWith("CN", StringComparison.OrdinalIgnoreCase)
                              select qm).ToList();

                return result;
            }
            catch
            {
                return new List<QuotationMaster>();
            }
        }
        /// <summary>
        /// 检测是否存在 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<SiebelActive> GetActiveListForCheck(SiebelActiveStatus status, SiebelActiveType type, string QuoteID, string OptyName, int count = 1)
        {
            try
            {
                var result = (from active in eQuotationContext.Current.SiebelActive
                              where active.Status == status.ToString()
                              where active.ActiveType == type.ToString()
                              where active.QuoteID == QuoteID
                              where active.OptyName == OptyName
                              orderby active.CreatedDate
                              select active)
                              .Take(count)
                              .ToList();

                return result;
            }
            catch
            {
                return new List<SiebelActive>();
            }

        }
        public static bool CreateSiebelActive(SiebelActive active)
        {
            try
            {
                active.Add();
                return true;
            }
            catch (Exception ex)
            {
                active.FailedLog = string.Format("Create SiebelActive fail! - Exception: {0}", ex.ToString());
                return false;
            }
        }

        public static void updateOptyID(int ActiveID, string OptyID)
        {
            eQuotationContext.Current.Database.ExecuteSqlCommand(string.Format("update SiebelActive set OptyID ='{0}' where ID={1}", OptyID, ActiveID));
        }
        public static void updateActive(int ActiveID, string Status)
        {
            eQuotationContext.Current.Database.ExecuteSqlCommand(string.Format("update SiebelActive set Status ='{0}' where ID={1}", Status, ActiveID));
        }
        #endregion

        #region -- For table [eQuotation].[dbo].[optyQuote] logic functions --

        public static bool CreateOptyQuote(optyQuote optyQuote)
        {
            try
            {
                optyQuote.Add();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool UpdateOptyQuote(optyQuote optyQuote)
        {
            try
            {
                optyQuote.Update();
                return true;
            }
            catch
            {
                return false;
            }
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

        #endregion

        #region -- For table [eQuotation].[dbo].[quoteSiebelQuote] logic functions --
        #endregion

    }
}
