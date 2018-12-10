using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Advantech.Myadvantech.DataAccess;
using System.Text.RegularExpressions;

namespace Advantech.Myadvantech.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRoleBusinessLogic
    {
        public static Boolean IsInMailGroup(String _GroupName, String _UserMail)
        {
            if (String.IsNullOrEmpty(_GroupName.Trim()) || String.IsNullOrEmpty(_UserMail.Trim()))
                return false;

            System.Text.StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format(" select COUNT(a.PrimarySmtpAddress) as c "));
            sb.AppendLine(String.Format(" from AD_MEMBER a left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL "));
            sb.AppendLine(String.Format(" where c.GROUP_NAME=N'{0}' and (a.PrimarySmtpAddress=N'{1}' or b.ALIAS_EMAIL=N'{1}') ", _GroupName.Replace("'", "''"), _UserMail));
            var c = SqlProvider.dbExecuteScalar("MY", sb.ToString());

            if (c != null && int.Parse(c.ToString()) > 0)
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// Identify if user belongs to ANA IAG CP KA Sales team SALES.IAG.USA
        /// </summary>
        /// <param name="GA"></param>
        /// <returns></returns>
        public static bool IsInGroupSalesIagUsa(ArrayList GA)
        {
            return GA.Contains("SALES.IAG.USA");
        }


        /// <summary>
        /// Identify if user belongs to ANA AOnline teams: AOnline EC, AOnline IAG, AOnline iSystem
        /// </summary>
        /// <param name="GA"></param>
        /// <returns></returns>
        public static bool IsAUSAOlineSales(ArrayList GA)
        {
            return (IsInGroupAonlineUsa(GA) || IsInGroupAonlineUsaIag(GA) || IsInGroupAonlineUsaISystem(GA));
        }

        /// <summary>
        /// Identify if user belongs to ANA AOnline Sales team Aonline.USA.iSystem (iSystem)
        /// </summary>
        /// <param name="GA"></param>
        /// <returns></returns>
        public static bool IsInGroupAonlineUsaISystem(ArrayList GA)
        {
            return GA.Contains(("Aonline.USA.iSystem").ToUpper());
        }

        /// <summary>
        /// Identify if user belongs to ANA AOnline Sales team Aonline.USA.IAG (IAG)
        /// </summary>
        /// <param name="GA"></param>
        /// <returns></returns>
        public static bool IsInGroupAonlineUsaIag(ArrayList GA)
        {
            return GA.Contains(("Aonline.USA.IAG").ToUpper());
        }


        /// <summary>
        /// Identify if user belongs to ANA AOnline Sales team Aonline.USA (EC)
        /// </summary>
        /// <param name="GA"></param>
        /// <returns></returns>
        public static bool IsInGroupAonlineUsa(ArrayList GA)
        {
            return (GA.Contains(("Aonline.USA").ToUpper()) || GA.Contains("SALES.AISA.USA"));
        }

        /// <summary>
        /// Identify if user belongs to ANA AOnline Sales team Aonline.USA (EC)
        /// </summary>
        /// <param name="GA"></param>
        /// <returns></returns>
        public static bool IsInGroupUsaAASCSCMOnly(ArrayList GA)
        {
            if (IsInGroupAonlineUsa(GA) || IsInGroupAonlineUsa(GA) || IsInGroupAonlineUsaISystem(GA))
            {
                return false;
            }

            return (GA.Contains(("AASC.SCM").ToUpper()));
        }



        public static AOnlineRegion GetTeamOfAonlineSalesBySalesCode(string sales_code)
        {
            DataTable dt = null;

            dt = Advantech.Myadvantech.DataAccess.MyAdvantechDAL.GetSaleseManager(sales_code);
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["id_rbu"] != DBNull.Value)
            {
                if (dt.Rows[0]["id_rbu"].ToString() == "IA-AOnline")
                {
                    return AOnlineRegion.AUS_AOnline_IAG;
                }
                if (dt.Rows[0]["id_rbu"].ToString() == "AOnline")
                {
                    return AOnlineRegion.AUS_AOnline;
                }
            }
            return AOnlineRegion.NA;
        }


        public static bool IsGroupPAPSeStore(ArrayList GA)
        {
            return GA.Contains(("PAPS.eStore").ToUpper());
        }

        public static bool IsGroupUsaAenc(ArrayList GA)
        {
            if (IsAUSAOlineSales(GA))
            {
                return false;
            }
            return (GA.Contains(("EMPLOYEE.AENC.USA").ToUpper()) || GA.Contains(("EMPLOYEES.Irvine").ToUpper()) || GA.Contains(("EMPLOYEE.AENC.NONIRVINE").ToUpper()));
            //return GA.Contains(("EMPLOYEE.AENC.USA").ToUpper());
        }

        /// <summary>
        /// If sales belongs to BB Ireland, then return ture
        /// </summary>
        /// <param name="SalesID">Sales ID in SAP</param>
        /// <returns>True means sales belongs to BB Ireland, otherwise return false</returns>
        public static Boolean IsBBIrelandBySalesID(String SalesID)
        {

            //String _salesoffice = Advantech.Myadvantech.DataAccess.SAPDAL.GetOfficeCodeBySalesID(SalesID);
            String _salesoffice = String.Empty;
            String _salesgroup = String.Empty;
            GetSalesOfficeAndGroupCodeBySalesCode(SalesID, out _salesoffice, out _salesgroup);

            if (_salesoffice.Equals("3410"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static void GetSalesOfficeAndGroupCodeBySalesCode(String SalesCode, out String SalesOffice, out String SalesGroup)
        {
            List<SAP_EMPLOYEE> se = new MyAdvantechDAL().GetSAPEmployeeBySalesCode(SalesCode);
            if (se != null && se.Count > 0)
            {
                SalesOffice = (from d in se select (String.IsNullOrEmpty(d.SALESOFFICE) ? "" : d.SALESOFFICE)).FirstOrDefault();
                SalesGroup = (from d in se select (String.IsNullOrEmpty(d.SALESGROUP) ? "" : d.SALESGROUP)).FirstOrDefault();
            }
            else
            {
                SalesOffice = "";
                SalesGroup = "";
            }
        }

        public static DataTable GetUSAonlineSalesEmployee(AOnlineRegion region, string SoldToERPID)
        {
            return Advantech.Myadvantech.DataAccess.SAPDAL.GetSalesEmployee(region, SoldToERPID);
        }

        public static DataTable GetTWAonlineSalesEmployee(string SoldToERPID)
        {
            return Advantech.Myadvantech.DataAccess.SAPDAL.GetSalesEmployee(AOnlineRegion.ATW_AOnline, SoldToERPID);
        }
        public static DataTable GetTWSalesEmployee(string SoldToERPID)
        {
            return Advantech.Myadvantech.DataAccess.SAPDAL.GetSalesEmployee(AOnlineRegion.ATW, SoldToERPID);
        }

        public static DataTable GetACNSalesEmployee(String _ORGID)
        {
            String str = String.Empty;
            if (_ORGID.ToUpper().Equals("CN10"))
            {
                str = @"select SALES_CODE, FULL_NAME from SAP_EMPLOYEE a inner join EZ_EMPLOYEE b on a.EMAIL=b.EMAIL_ADDR 
                            where SALES_CODE >= '41010000' and SALES_CODE <= '41399999' and SALESOFFICE >= '6100' and SALESOFFICE <= '6430' 
                            order by a.SALES_CODE";
            }
            else
            {
                str = @"select SALES_CODE, FULL_NAME from SAP_EMPLOYEE a inner join EZ_EMPLOYEE b on a.EMAIL=b.EMAIL_ADDR 
                            where SALES_CODE >= '41010000' and SALES_CODE <= '41399999' and SALESOFFICE >= '6100' and SALESOFFICE <= '6430' 
                            order by a.SALES_CODE";
            }

            DataTable dt = SqlProvider.dbGetDataTable("MY", str);
            return dt;
        }

        public static DataTable GetBBUSSalesEmployee()
        {
            String str = @"select SALES_CODE, FULL_NAME from SAP_EMPLOYEE where PERS_AREA = 'US10' order by SALES_CODE";

            DataTable dt = SqlProvider.dbGetDataTable("MY", str);
            return dt;
        }

        public static DataTable GetADLOGSalesEmployee()
        {
            String str = @"select SALES_CODE, FULL_NAME from SAP_EMPLOYEE where PERS_AREA = 'EU80' order by SALES_CODE";

            DataTable dt = SqlProvider.dbGetDataTable("MY", str);
            return dt;
        }

        public static bool IsValidEmailFormat(string email)
        {
            string reg = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            RegexOptions options = RegexOptions.Singleline;
            return Regex.Match(email, reg, options).Length == 0 ? false : true;
        }

        public static string GetCountryTelephoneFormat(string number, AOnlineRegion region)
        {
            //ICC 2016/1/26 Update telephone format rule.
            if (!string.IsNullOrEmpty(number))
            {
                string phone = number;
                string format = string.Empty;
                try
                {
                    string[] numbers = number.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);//Replace newline character
                    phone = numbers[0];
                    long no = 0;
                    switch (region)
                    {
                        case AOnlineRegion.AKR:
                            phone = phone.Replace("+82", "");

                            if (numbers.Length == 1) //Without format
                                return phone;

                            format = numbers[1];
                            if (numbers[1].IndexOf("-") == 0) //Remove first dash. ex -00-000-0000 to 00-000-0000
                                format = format.Remove(0, 1);

                            int pound = phone.IndexOf("#"); //Remove extension temporary.
                            if (pound > -1)
                                phone = phone.Remove(pound);

                            no = Convert.ToInt64(phone);
                            //return no.ToString(format);
                            String _returnnum = no.ToString(format);
                            if (!_returnnum.StartsWith("0"))
                            {
                                _returnnum = "0" + _returnnum;
                            }
                            return _returnnum;
                            break;

                        case AOnlineRegion.ACN:
                            phone = phone.Replace("+86", "");
                            if (numbers.Length == 1) //Without format
                                return phone;

                            format = numbers[1];
                            if (numbers[1].IndexOf("-") == 0) //Remove first dash. ex -00-000-0000 to 00-000-0000
                                format = format.Remove(0, 1);

                            //int pound = phone.IndexOf("#"); //Remove extension temporary.
                            //if (pound > -1)
                            //    phone = phone.Remove(pound);

                            no = Convert.ToInt64(phone);
                            return no.ToString(format);

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //Save error message.
                    return phone;
                }
            }
            return number;
        }

        public static Boolean CanSeePipelineReport(String user_id)
        {
            string strPipeLineSales = "select distinct sales_email from [eQuotation].[dbo].[SALES_ROLE_MAPPING] where isActive=1";

            DataTable dt = DataAccess.SqlProvider.dbGetDataTable("MY", strPipeLineSales);
            if (dt != null & dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    string strSales = r.ItemArray[0].ToString();
                    if (user_id.Equals(strSales, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;


            //if (user_id.Equals("Fei.Khong@advantech.com", StringComparison.OrdinalIgnoreCase) ||
            //    user_id.Equals("Cathee.cao@advantech.com", StringComparison.OrdinalIgnoreCase) ||
            //    user_id.Equals("Mike.arcure@advantech.com", StringComparison.OrdinalIgnoreCase) ||
            //    user_id.Equals("Kurt.kleinschmidt@advantech.com", StringComparison.OrdinalIgnoreCase) ||
            //    user_id.Equals("Gabriela.meza@advantech.com", StringComparison.OrdinalIgnoreCase) ||
            //    user_id.Equals("Noel.Nguyen@advantech.com", StringComparison.OrdinalIgnoreCase) ||
            //    user_id.Equals("Denise.kwong@advantech.com", StringComparison.OrdinalIgnoreCase))
            //    return true;
            //else
            //    return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static Boolean CanSee968TParts(String CompanyID)
        {

            //T-Code	    Maintained	  968Q*	                    968T* 
            //ZTSD_106A	    Yes	          Only as Assembly order	Assembly or Single item
            //ZTSD_106A	    No 	          Only as Assembly order	Not allowed 
            //ZTSD_106C	    Yes	          Only as Assembly order	Not allowed + no price info
            //ZTSD_106C	    No 	          Only as Assembly order	Assembly or Single item
            //----------------------------------------------------------------------------------
            //              ZTSD_106C     ZTSD_106A       Result
            //Maintained    Yes           No              Not allowed
            //Maintained    Yes           Yes             Not allowed
            //Maintained    No            Yes             Allowed
            //Maintained    No            No              Not allowed


            //Frank : get the maintained result from ZTSD_106A, and put it to Boolean Result_106A
            String str = "select VKORG,KUNNR from saprdp.ZTSD_106A where KUNNR = '" + CompanyID + "' and BDATE < '" + DateTime.Now.ToString("yyyyMMdd") + "' and EDATE > '" + DateTime.Now.ToString("yyyyMMdd") + "'";
            DataTable dt = DataAccess.OracleProvider.GetDataTable("SAP_PRD", str);
            Boolean Result_106A = false;
            if (dt.Rows.Count > 0)
            {
                Result_106A = true;
            }

            //Frank : get the maintained result from ZTSD_106C, and put it to Boolean Result_106C
            str = "select VKORG,KUNNR from saprdp.ZTSD_106C where KUNNR = '" + CompanyID + "'";
            dt = DataAccess.OracleProvider.GetDataTable("SAP_PRD", str);
            Boolean Result_106C = false;
            if (dt.Rows.Count > 0)
            {
                Result_106C = true;
            }

            //Frank: if account is maintained in both the 106A and 106C, return cannot see as default
            //Of course this rule can be adjusted if user feel bad.....
            if (Result_106A == true && Result_106C == true)
            {
                return false;
            }

            //Based on account is maintained in A or C to return the result
            if (Result_106A == true && Result_106C == false)
            {
                return true;
            }

            //Based on account is maintained in A or C to return the result
            if (Result_106A == false && Result_106C == true)
            {
                return false;
            }

            //If account is not maintained in both A and C, users can't access 968T Parts
            //if (Result_106A == false && Result_106C == false)
            return false;

        }

        /// <summary>
        /// Sync table from SAP to get 968T company
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns>CLAcompany</returns>
        public static CLAcompany Get968TCompany(string datetime = "")
        {
            if (string.IsNullOrEmpty(datetime))
                datetime = DateTime.Now.ToString("yyyyMMdd");

            List<SAPCompany> ls106A = new List<SAPCompany>();
            DataTable dt106A = DataAccess.OracleProvider.GetDataTable("SAP_PRD", string.Format(" select VKORG,KUNNR from saprdp.ZTSD_106A where  BDATE < '{0}' and EDATE > '{0}' ", datetime));
            if (dt106A != null && dt106A.Rows.Count > 0)
                foreach (DataRow dr in dt106A.Rows)
                    ls106A.Add(new SAPCompany(dr));

            List<SAPCompany> ls106C = new List<SAPCompany>();
            DataTable dt106C = DataAccess.OracleProvider.GetDataTable("SAP_PRD", string.Format(" select VKORG,KUNNR from saprdp.ZTSD_106C "));
            if (dt106C != null && dt106C.Rows.Count > 0)
                foreach (DataRow dr in dt106C.Rows)
                    ls106C.Add(new SAPCompany(dr));

            return new CLAcompany(ls106A, ls106C);
        }

        public static String getPlantByOrg(String _orgid)
        {
            switch (_orgid.Substring(0, 2))
            {
                case "TW":
                    return "TWH1";
                case "US":
                    if (_orgid.ToString().ToUpper().Equals("US10"))
                        return "UBH1";
                    else
                        return "USH1";
                case "EU":
                    if (_orgid.ToString().ToUpper().Equals("EU80"))
                        return "DLM1";
                    else
                        return "EUH1";
                case "SG":
                    return "SGH1";
                case "MX":
                    return "TWH1";
                case "CN":
                    if (_orgid.ToString().ToUpper().Equals("CN30"))
                        return "CNH3";
                    else
                        return "CNH1";
                default:
                    return "TWH1";
            }
        }

        public static String MYAgetShiptoIDBySoldtoID(String _SoldtoID, String _CartID)
        {
            String result = String.Empty;

            result = (String)DataAccess.SqlProvider.dbExecuteScalar("MY", "SELECT top 1 ERPID FROM ORDER_PARTNERS WHERE ORDER_ID = '" + _CartID + "' AND TYPE = 'S'");
            if (String.IsNullOrEmpty(result))
            {
                result = (String)DataAccess.SqlProvider.dbExecuteScalar("MY", "SELECT top 1 PARENT_COMPANY_ID FROM SAP_COMPANY_PARTNERS WHERE COMPANY_ID = '" + _SoldtoID + "' and PARTNER_FUNCTION = 'WE'");
                if (String.IsNullOrEmpty(result))
                    return _SoldtoID;
                else
                    return result;
            }
            else
                return result;
        }

        public static String EQgetShiptoIDBySoldtoID(String _SoldtoID, String _QuoteID)
        {
            String result = String.Empty;
            result = (String)DataAccess.SqlProvider.dbExecuteScalar("EQ", "SELECT TOP 1 ERPID FROM eQuotationStaging.dbo.EQPARTNER where QUOTEID = '" + _QuoteID + "' and TYPE = 'S'");
            if (String.IsNullOrEmpty(result))
                return _SoldtoID;
            else
                return result;
        }

        public static String getCountryCodeByERPID(String _erpid)
        {
            var obj = DataAccess.MyAdvantechDAL.GetSAPDIMCompanyByERPID(_erpid).FirstOrDefault();
            if (obj == null)
            {
                var obj_sap = OracleProvider.ExecuteScalar("SAP_PRD", "select LAND1 from saprdp.kna1 where KUNNR = '" + _erpid + "' AND rownum=1");
                if (obj_sap == null)
                    return "";
                else if (String.IsNullOrEmpty(obj_sap.ToString()))
                    return "";
                else
                    return obj_sap.ToString();
            }
            else if (String.IsNullOrEmpty(obj.COUNTRY))
                return "";
            else
                return obj.COUNTRY;
        }

        public static String getSectorBySalesEmail(string salesEmail)
        {
            var sales = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesEmail == salesEmail).FirstOrDefault();
            if (sales != null && sales.Sector != null)
                return sales.Sector;
            return "";
        }


        public static String getABRSectorBySalesCode(string salesCode)
        {

            DataTable _dt = SqlProvider.dbGetDataTable("MY", string.Format(" Select id_sap,id_email,id_sbu,id_sector,id_by_country,id_rbu FROM MyAdvantechGlobal.DBO.EAI_IDMAP WHERE id_rbu='Brazil' and id_by_country<>'X' and id_sap = '{0}'", salesCode.Trim()));
            if (_dt != null && _dt.Rows.Count > 0)
            {
                string _id_sector = _dt.Rows[0]["id_sector"].ToString();
                if (!string.IsNullOrEmpty(_id_sector))
                {
                    //Frank 20180611
                    //EC - AOnline
                    //EC - CSF
                    //EC - KA
                    //General IIoT
                    //iHealthcare
                    //IIoT - AOnline
                    //IIoT - CSF
                    //iLogistics
                    //iRetail
                    //Others(RMA)
                    switch (_id_sector)
                    {
                        case "iHealthcare":
                            return "SIOT";
                        case "iLogistics":
                            return "SIOT";
                        case "EC-AOnline":
                            return "EIOT";
                        case "EC-CSF":
                            return "EIOT";
                        case "EC-KA":
                            return "EIOT";
                        default:
                            return "IIOT";
                    }
                }
            }
            return "";
        }

        public static String getSectorBySalesEmailSalesCode(string salesEmail, string salesCode)
        {

            DataTable _dt = SqlProvider.dbGetDataTable("MY", string.Format(" Select id_sap,id_sbu,id_rbu,id_sector FROM MyAdvantechGlobal.DBO.EAI_IDMAP WHERE id_sap = '{0}'", salesCode.Trim()));
            if(_dt != null && _dt.Rows.Count > 0)
            {
                string _id_sub = _dt.Rows[0]["id_sbu"].ToString();
                string _id_rbu = _dt.Rows[0]["id_sector"].ToString();
                if(!string.IsNullOrEmpty(_id_sub) && _id_sub.Equals("AISC", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(_id_rbu))
                    {
                        switch (_id_rbu)
                        {
                            case "iRetail":
                                return "AISC-iRetail";
                            case "iLogistics":
                                return "AISC-iLogistics";
                            case "iHealthcare":
                                return "AISC-iHealthcare";
                            default:
                                return "AISC-iRetail";
                        }
                    }
                }
                else if ((!string.IsNullOrEmpty(_id_sub) && _id_sub.Contains("ASG")))
                {
                    if (!string.IsNullOrEmpty(_id_rbu))
                    {
                        if (_id_rbu == "iConnectivity CSF")
                            return "ASG-iConnectivityCSF";
                        return "ASG-" + _id_rbu;
                    }
                }
            }

            // Ryan 20171213 Sales Code is also needed due to some sales email may cross different sectors.
            var sales = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesEmail == salesEmail && s.SalesCode == salesCode).FirstOrDefault();
            if (sales != null && sales.Sector != null)
                return sales.Sector;
            return "";
        }

        public static String getSectorBySalesEmailSalesCode(string salesEmail, string salesCode, string region)
        {
            if (salesEmail != null && salesCode != null)
            {
                DataTable _dt = SqlProvider.dbGetDataTable("MY", string.Format(" Select id_sap,id_sbu,id_rbu,id_sector FROM MyAdvantechGlobal.DBO.EAI_IDMAP WHERE id_sap = '{0}'", salesCode.Trim()));
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    string _id_sub = _dt.Rows[0]["id_sbu"].ToString();
                    string _id_rbu = _dt.Rows[0]["id_sector"].ToString();
                    if (!string.IsNullOrEmpty(_id_sub) && _id_sub.Equals("AISC", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(_id_rbu))
                        {
                            switch (_id_rbu)
                            {
                                case "iRetail":
                                    return "AISC-iRetail";
                                case "iLogistics":
                                    return "AISC-iLogistics";
                                case "iHealthcare":
                                    return "AISC-iHealthcare";
                                default:
                                    return "AISC-iRetail";
                            }
                        }
                    }
                    else if (region == "ASG")
                    {
                        if (!string.IsNullOrEmpty(_id_rbu))
                        {
                            if (_id_rbu == "iConnectivity CSF")
                                return "ASG-iConnectivityCSF";
                            return "ASG-" + _id_rbu;
                        }
                    }
                }

                // Ryan 20171213 Sales Code is also needed due to some sales email may cross different sectors.
                var sales = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesEmail == salesEmail && s.SalesCode == salesCode).FirstOrDefault();
                if (sales != null && sales.Sector != null)
                    return sales.Sector;
            }
            return "";
        }

        public static BBCustomer getBBcustomerByUserID(string userID)
        {
            object ERPID = SqlProvider.dbExecuteScalar("MY", string.Format("SELECT TOP 1 sd.COMPANY_ID FROM SIEBEL_CONTACT sc INNER JOIN SAP_DIMCOMPANY sd ON sc.ERPID = sd.COMPANY_ID WHERE sc.EMAIL_ADDRESS = '{0}' AND sc.OrgID = 'ABB' AND sd.ORG_ID = 'US10' AND sd.COMPANY_TYPE ='Z001' AND sc.ACCOUNT_ROW_ID<>'1-1CK7KL5' ", userID.Trim()));
            if (ERPID != null && !string.IsNullOrEmpty(ERPID.ToString()))
                return new BBCustomer(ERPID.ToString(), "US10", userID);
            return null;
        }

        public static bool AssociateSiebelSAPAccountContact(bool isTesting, string orderNo, string ERPID)
        {
            Advantech.Myadvantech.DataAccess.Entities.Order order = BBeStoreDAL.GetBBeStoreOrderByOrderNo(orderNo);
            if (order != null && order.Cart != null && order.Cart.SoldToContact != null)
            {
                DataAccess.SAPDAL.CreateSAPContact(isTesting, ERPID, order.Cart.SoldToContact.FirstName, order.Cart.SoldToContact.LastName, order.UserID, string.Empty, string.Empty, "0005", "12");
                return SiebelDAL.AssociateSiebelSAPAccountContact(order.UserID, order.Cart.SoldToContact.FirstName, order.Cart.SoldToContact.LastName, ERPID.ToUpper(), string.Empty, false, true);
            }
            return false;
        }

        public static bool IsEmployee(string email)
        {
            Object o = SqlProvider.dbExecuteScalar("EZ", String.Format("SELECT count(email_addr) FROM [Employee_New].[dbo].[EZ_PROFILE] where email_addr='{0}'", email));

            if (o != null)
            {
                double number;
                bool isNumeric = Double.TryParse(o.ToString(), out number);
                

                if (isNumeric && Convert.ToInt32(o) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static DataTable GetEmployeeProfile(string email)
        {
            var dt = SqlProvider.dbGetDataTable("EZ", String.Format("SELECT top 1  FST_NAME, LST_NAME FROM [Employee_New].[dbo].[EZ_PROFILE] where email_addr='{0}'", email));
            return dt;
        }
    }
}
