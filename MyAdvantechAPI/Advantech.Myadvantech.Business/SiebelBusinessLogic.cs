using Advantech.Myadvantech.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.Business
{
    /// <summary>
    /// Processing SiebelActive business logic
    /// </summary>
    public class SiebelBusinessLogic
    {
        public static string CreateSiebelQuoteWithOpty(string eQuoteID, string optyid, ref string WSParameters, ref string ErrorMSG)
        {
            if (string.IsNullOrEmpty(eQuoteID)) {
                ErrorMSG = "eQuotation Quote ID is string.IsNullOrEmpty";
                return string.Empty; 
            }
            try
            {
                QuotationMaster QM = QuoteBusinessLogic.GetQuotationMaster(eQuoteID);
                if (QM == null) {
                    ErrorMSG = "Quote master cannot be found in eQuotation";
                    return string.Empty; 
                }
                List<QuotationDetail> QD = QuoteBusinessLogic.GetQuotationDetail(eQuoteID);

                //ICC 2015/11/3 Use new Siebel web service to add quote
                return SiebelDAL.CreateSiebelQuoteV2(QM, QD, optyid, ref WSParameters, ref ErrorMSG);
                //return SiebelDAL.CreateSiebelQuote(QM, QD, optyid, ref WSParameters, ref ErrorMSG);
                //return SiebelDAL.CreateSiebelQuote(QM, QD, optyid, WS_ID, WS_PASSWORD);
            }
            catch(Exception ex)
            {
                ErrorMSG = ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// Create Opty的指令檔，quoteID必填，同時必須有對應的QuotationMaster資料
        /// </summary>
        /// <param name="active"></param>
        /// <returns>true or false</returns>
        public static bool CreateOpportunityCommand(SiebelActive active)
        {
            if (string.IsNullOrEmpty(active.QuoteID))
            {
                active.FailedLog = "eQuote ID is null";
                return false;
            }
            if (SiebelDAL.GetActiveListForCheck(SiebelActiveStatus.UnProcess, SiebelActiveType.CreateOpportunity, active.QuoteID, active.OptyName, 1).Count() > 0)
            {
                active.FailedLog = "Opportunity is already exists";
                return false;
            }
            if (active.QuotationMaster == null)
            {
                active.FailedLog = "No QuotationMaster data!";
                return false;
            }
            //2015/6/9 Add new rule to check opty owner email
            if (string.IsNullOrEmpty(active.OptyOwnerEmail))
            {
                active.FailedLog = "No Opty owner email";
                return false;
            }
            //檢查 optyName (原本Business.vb的邏輯)
            if (string.IsNullOrEmpty(active.OptyName))
            {
                if (active.QuotationMaster.Revision_Number != null && active.QuotationMaster.Revision_Number > 0)
                {
                    active.OptyName = string.Format("{0}V{1}", active.SiebelOpty.optyName, active.QuotationMaster.Revision_Number);
                }
            }

            //檢查 optySate (原本Business.vs的邏輯)
            if (string.IsNullOrEmpty(active.OptyStage))
            {
                active.OptyStage = "25% Proposing/Quoting";
            }

            //檢查Owner email (原本SiebelTools的邏輯) 2015/6/9 This rule is still in Business.vb and it will prevent someone's eQutation ID is not equal to SIEBEL ID.
            //if (string.IsNullOrEmpty(active.OptyOwnerEmail))
            //{
            //    if (!string.IsNullOrEmpty(active.QuotationMaster.salesEmail))
            //        active.OptyOwnerEmail = active.QuotationMaster.salesEmail;
            //    else if (!string.IsNullOrEmpty(active.QuotationMaster.createdBy))
            //        active.OptyOwnerEmail = active.QuotationMaster.createdBy;
            //    else if (string.IsNullOrEmpty(active.QuotationMaster.attentionRowId))
            //        active.OptyOwnerEmail = active.QuotationMaster.attentionRowId;
            //}

            active.ActiveSource = SiebelActiveSource.eQuotation.ToString();
            active.ActiveType = SiebelActiveType.CreateOpportunity.ToString();
            active.Status = SiebelActiveStatus.UnProcess.ToString();
            active.CreatedDate = DateTime.Now;
            active.LastUpdatedDate = DateTime.Now;

            if (!SiebelDAL.CreateSiebelActive(active))
            {
                return false; //創建SiebelActive失敗，錯誤訊息紀錄在active.FailedLog內
            }
            return true;
        }

        /// <summary>
        /// Create Opty的指令檔，quoteID必填，同時必須有對應的QuotationMaster資料
        /// </summary>
        /// <param name="active"></param>
        /// <returns>true or false</returns>
        public static bool CreateQuoteCommand(SiebelActive active)
        {
            if (string.IsNullOrEmpty(active.QuoteID))
            {
                active.FailedLog = "eQuote ID is null";
                return false;
            }

            if (active.QuotationMaster == null)
            {
                active.FailedLog = "No QuotationMaster data!";
                return false;
            }
            if (SiebelDAL.GetSiebelActiveByQuoteID(active.QuoteID).Count > 0)
            {
                active.FailedLog = "QuotationID already exists!";
                return false;
            }
            active.ActiveSource = SiebelActiveSource.eQuotation.ToString();
            active.ActiveType = SiebelActiveType.CreateQuote.ToString();
            active.Status = SiebelActiveStatus.UnProcess.ToString();
            active.CreatedDate = DateTime.Now;
            active.LastUpdatedDate = DateTime.Now;

            if (!SiebelDAL.CreateSiebelActive(active))
            {
                return false; //創建SiebelActive失敗，錯誤訊息紀錄在active.FailedLog內
            }
            return true;
        }
        
        /// <summary>
        /// Create QuoteActive的指令檔，quoteID必填，同時必須有對應的QuotationMaster資料
        /// </summary>
        /// <param name="active"></param>
        /// <returns>true or false</returns>
        public static bool CreateQuoteActive(SiebelActive active)
        {
            if (string.IsNullOrEmpty(active.QuoteID))
            {
                active.FailedLog = "eQuote ID is null";
                return false;
            }

            if (active.QuotationMaster == null)
            {
                active.FailedLog = "No QuotationMaster data!";
                return false;
            }

            active.ActiveSource = SiebelActiveSource.eQuotation.ToString();
            active.ActiveType = SiebelActiveType.CreateActivity.ToString();
            active.Status = SiebelActiveStatus.UnProcess.ToString();
            active.CreatedDate = DateTime.Now;
            active.LastUpdatedDate = DateTime.Now;

            if (!SiebelDAL.CreateSiebelActive(active))
            {
                return false; //創建SiebelActive失敗，錯誤訊息紀錄在active.FailedLog內
            }
            return true;
        }
        
        /// <summary>
        /// Update Opty的指令檔，quoteID必填，同時必須有對應的QuotationMaster資料
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public static bool UpdateOpportunityCommand(SiebelActive active)
        {
            if (string.IsNullOrEmpty(active.OptyID) && string.IsNullOrEmpty(active.QuoteID))
            {
                //eQuoteID有可能有null，因為有可能從MyAdvantech過來，所以改為QuoteID & OptyID 同時都是空時才報錯
                active.FailedLog = "eQuote ID and Opportunity ID are all null";
                return false;
            }

            if (string.IsNullOrEmpty(active.OptyID) && active.QuotationMaster == null)
            {
                active.FailedLog = "No QuotationMaster data!";
                return false;
            }

            ////檢查 optyName (原本Business.vb的邏輯)
            //if (string.IsNullOrEmpty(active.OptyName))
            //{
            //    if (active.QuotationMaster.Revision_Number != null && active.QuotationMaster.Revision_Number > 0)
            //    {
            //        active.OptyName = string.Format("{0}V{1}", active.SiebelOpty.optyName, active.QuotationMaster.Revision_Number);
            //    }
            //}

            ////檢查Owner email (原本SiebelTools的邏輯)
            //if (string.IsNullOrEmpty(active.OptyOwnerEmail) && active.QuotationMaster != null)
            //{
            //    if (!string.IsNullOrEmpty(active.QuotationMaster.salesEmail))
            //        active.OptyOwnerEmail = active.QuotationMaster.salesEmail;
            //    else if (!string.IsNullOrEmpty(active.QuotationMaster.createdBy))
            //        active.OptyOwnerEmail = active.QuotationMaster.createdBy;
            //    else if (string.IsNullOrEmpty(active.QuotationMaster.attentionRowId))
            //        active.OptyOwnerEmail = active.QuotationMaster.attentionRowId;
            //}

            if (string.IsNullOrEmpty(active.ActiveSource)) active.ActiveSource = SiebelActiveSource.eQuotation.ToString();
            if (string.IsNullOrEmpty(active.ActiveType)) active.ActiveType = SiebelActiveType.UpdateOpportunity.ToString();
            if (string.IsNullOrEmpty(active.Status)) active.Status = SiebelActiveStatus.UnProcess.ToString();
            active.CreatedDate = DateTime.Now;
            active.LastUpdatedDate = DateTime.Now;

            if (!SiebelDAL.CreateSiebelActive(active))
            {
                return false; //創建SiebelActive失敗，錯誤訊息紀錄在active.FailedLog內
            }
            return true;
        }

        #region 以下四個方法是給eQuotation web job執行時使用的

        /// <summary>
        /// Web background job - Create Siebel Opportunity
        /// </summary>
        /// <param name="active"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool BatchCreateSiebelOpportunity(SiebelActive active)
        {
            string row_id = string.Empty;
            string wsparas = string.Empty;
            string wserrmsg = string.Empty;
            try {

                //先把狀態更新成Processing以免任何失敗造成job一直執行這筆指令
                active.UpdateActiveProcessing(SiebelActiveUpdatedUser.System);

                row_id = SiebelDAL.CreateSiebelOptyV2(active, ref wsparas, ref wserrmsg);

                if (!string.IsNullOrEmpty(wserrmsg))
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Opportunity through Siebel web service failed: " + wserrmsg, wsparas);
                    return false;
                }


                if (string.IsNullOrEmpty(row_id))
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Opportunity Row id Siebel WS returned is string.isNullOrEmpty", wsparas);
                    return false;
                }

                if (row_id.Equals("NULL", StringComparison.InvariantCultureIgnoreCase))
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Opportunity Row id Siebel WS returned is Null string!", wsparas);
                    return false;
                }
            
            }
            catch(Exception e) {
                active.UpdateActiveFailed(SiebelActiveUpdatedUser.System,"Create Siebel Opportunity failed: " + e.Message, wsparas);
                return false;
            }

            try
            {
                /*
                 * 檢查SiebelActive裡面，是否有別的數據(同quoteID)，
                 * 預防同一批次作業中，還要再Create Quote或Update Opty的情況
                 * Case 1: Create Opty後 還要Create Quote
                 * Case 2: Create Opty後 還要Update Opty
                 * Case 3: Create Opty後 先Update Opty 再Create Quote
                */
                List<SiebelActive> list = SiebelDAL.GetSiebelActiveByQuoteID(active.QuoteID, active.ID); //必須排除自己這筆紀錄

                if (list != null)
                {
                    foreach (SiebelActive sa in list)
                    {
                        //sa.OptyID = row_id;
                        //sa.Update();
                        SiebelDAL.updateOptyID(sa.ID, row_id);
                    }
                }

                //2015/4/27 Also have to check and update [MyAdvantechGlobal].[CARTMASTERV2].[optyID] after creating Opportunity successfully
                MyAdvantechDAL.UpdateOptyID(active.QuoteID, row_id);
                /*
                 * end
                 */

                //建立Siebel opty成功
                active.UpdateActiveSuccess(SiebelActiveUpdatedUser.System, wsparas);

                active.UpdateOptyIDtoOptyQuote(active.QuoteID, row_id);

                ////檢查optyQuote table 把optyId跟相關的數據一併更新過去
                //if (active.SiebelOpty == null)
                //{
                //    optyQuote opty = new optyQuote();
                //    opty.quoteId = active.QuoteID;
                //    opty.optyId = active.OptyID;
                //    opty.optyStage = active.OptyStage;
                //    opty.Opty_Owner_Email = active.OptyOwnerEmail;
                //    opty.Add();
                //}
                //else
                //{
                //    active.SiebelOpty.optyId = active.OptyID;
                //    active.SiebelOpty.optyName = active.OptyName;
                //    active.SiebelOpty.optyStage = active.OptyStage;
                //    active.SiebelOpty.Opty_Owner_Email = active.OptyOwnerEmail;
                //    active.SiebelOpty.Update();
                //}
                return true;
            }
            catch (Exception ex)
            {
                active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Opportunity failed: " + ex.ToString(), wsparas);
                return false;
            }
        }

        /// <summary>
        /// Web background job - Update Siebel Opportunity
        /// </summary>
        /// <param name="active"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool BatchUpdateSiebelOpportunity(SiebelActive active)
        {
            string wsparas = string.Empty;
            string wserrmsg = string.Empty;
            try
            {

                //先把狀態更新成Processing以免任何失敗造成job一直執行這筆指令
                active.UpdateActiveProcessing(SiebelActiveUpdatedUser.System);

                if (string.IsNullOrEmpty(active.OptyID))
                {
                    if (active.SiebelOpty == null)
                    {
                        //Opportunity ID 是空 而且optyQuote的optyID也找不到
                        active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Update Siebel Opportunity failed: Object SiebelOpty is null", wsparas);
                        return false;
                    }

                    if (string.IsNullOrEmpty(active.SiebelOpty.optyId))
                    {
                        //Opportunity ID 是空 而且optyQuote的optyID也找不到
                        active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Update Siebel Opportunity failed: OptyID of Object SiebelOpty is null", wsparas);
                        return false;
                    }

                    active.OptyID = active.SiebelOpty.optyId;
                }

                if (!SiebelDAL.UpdateSiebelOptyV2(active, ref wsparas, ref wserrmsg))
                {
                    //Update Siebel Opportunity fail
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Update Siebel Opportunity through Siebel WS failed: " + wserrmsg, wsparas);
                    return false;
                }

                //Frank 20150411先暫時不用執行這段
                ////走到這表示update成功，最後將SiebelActive的數據更新到optyQuote
                //if (active.SiebelOpty != null)
                //{
                //    if (!string.IsNullOrEmpty(active.OptyName))
                //        active.SiebelOpty.optyName = active.OptyName;

                //    if (!string.IsNullOrEmpty(active.OptyStage))
                //        active.SiebelOpty.optyStage = active.OptyStage;

                //    if (!string.IsNullOrEmpty(active.OptyOwnerEmail))
                //        active.SiebelOpty.Opty_Owner_Email = active.OptyOwnerEmail;

                //    active.SiebelOpty.Update();
                //}

                //Update Siebel Opportunity success
                active.UpdateActiveSuccess(SiebelActiveUpdatedUser.System, wsparas);
                return true;
            }
            catch (Exception ex)
            {

                active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Update Siebel Opportunity failed: " + ex.ToString(), wsparas);
                return false;
            }
          
        }

        /// <summary>
        /// Web bacjground job - Create Siebel Quote
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public static bool BatchCreateSiebelQuote(SiebelActive active)
        {
            string _wspara = string.Empty;
            string _errmsg = string.Empty;
            try
            {

                //先把狀態更新成Processing以免任何失敗造成job一直執行這筆指令
                active.UpdateActiveProcessing(SiebelActiveUpdatedUser.System);

                //先檢查QuotationMaster & QuotationDetail
                if (active.QuotationMaster == null)
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Quote failed: No quote master data in eQuotation", _wspara);
                    return false;
                }
                if (active.QuotationDetail == null)
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Quote failed: No quote detail data in eQuotation", _wspara);
                    return false;
                }

                //無論有無optyId都去呼叫Siebel web service
                string SiebelQuoteRowID = SiebelBusinessLogic.CreateSiebelQuoteWithOpty(active.QuoteID, active.OptyID, ref _wspara, ref _errmsg);

                if (!string.IsNullOrEmpty(_errmsg))
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Quote through Siebel WS failed: " + _errmsg, _wspara);
                    return false;
                }
                if (string.IsNullOrEmpty(SiebelQuoteRowID))
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Quote through Siebel WS failed: Quote row id WS returned is String.IsNullOrEmpty", _wspara);
                    return false;
                }

                //quoteID有值才代表成功
                active.UpdateActiveSuccess(SiebelActiveUpdatedUser.System, _wspara);
            
                //quoteSiebelQuote table不需要再insert，已經在SiebelDa做了
                return true;
            }
            catch (Exception ex)
            {
                active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Quote failed: " + ex.ToString(), _wspara);
                return false;
            }
           
        }

        /// <summary>
        /// Web bacjground job - Create Siebel Activity
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public static bool BatchCreateSiebelActivity(SiebelActive active)
        {
            string wsparas = string.Empty;
            string wserrmsg = string.Empty;
            try
            {

                //先把狀態更新成Processing以免任何失敗造成job一直執行這筆指令
                active.UpdateActiveProcessing(SiebelActiveUpdatedUser.System);

                if (string.IsNullOrEmpty(active.QuoteID) || active.QuotationMaster == null)
                {
                    //active.FailedLog = "Create Siebel Activity failed! No master data";
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Activity failed: No Quotation master data in eQuotation", "");
                    return false;
                }

                string SiebelActivityRowID = SiebelDAL.CreateSiebleActivityV2(active, ref wsparas, ref wserrmsg);

                if (!string.IsNullOrEmpty(wserrmsg))
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Activity failed: " + wserrmsg, wsparas);
                    return false;
                }

                if (string.IsNullOrEmpty(SiebelActivityRowID))
                {
                    active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Acvivity Row ID Siebel WS returned is string.IsNullOrEmpty", wsparas);
                    return false;
                }
                
                active.UpdateActiveSuccess(SiebelActiveUpdatedUser.System, wsparas);
                return true;
            }
            catch (Exception ex)
            {
                active.UpdateActiveFailed(SiebelActiveUpdatedUser.System, "Create Siebel Activity failed: " + ex.ToString(), wsparas);
                return false;
            }
        
        }

        /// <summary>
        /// Convert Sibel Country to SAP Country
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="plant"></param>
        /// <returns></returns>
        public static string ConvertSiebelCountrToSAPCountry(string siebelCountry)
        {
            String result = String.Empty;
            StringBuilder sb = new StringBuilder();
            string sapCountry = "";

            switch (siebelCountry.Trim())
            {
                case "United States":
                    sapCountry = "USA";
                    break;

                case "Korea":
                    sapCountry = "South Korea";
                    break;

                case "Russia":
                    sapCountry = "Russian Fed.";
                    break;

                case "Bosnia and Herzegovina":
                    sapCountry = "Bosnia-Herz.";
                    break;

                default:
                    sapCountry = siebelCountry;
                    break;
            }
            //var dt = Advantech.Myadvantech.DataAccess.SAPDAL.GetSAPCountryList();
            sb.AppendFormat(" select land1, landx from saprdp.t005t where mandt = '168' and spras = 'E' and LANDX = '{0}' order by landx ", sapCountry.Replace("'", "''"));
            var dt = OracleProvider.GetDataTable("SAP_PRD", sb.ToString());
            if (dt.Rows.Count > 0)
            {
                DataRow _row = dt.Rows[0];
                result = _row["land1"].ToString();
            }
            else
            {
                result = siebelCountry;
            }



            return result;
        }

        #endregion

        public static SIEBEL_CONTACT GetSiebelContact(string RowID)
        {
            return SiebelDAL.GetSiebelContact(RowID);
        }

        public static string CreateSiebelContactByWS(SIEBEL_CONTACT contact)
        {
            return SiebelDAL.CreateSiebelContactByWS(contact);
        }
        public static bool UpdateSiebelContactByWS(SIEBEL_CONTACT contact)
        {
            return SiebelDAL.UpdateSiebelContactByWS(contact);
        }

        public static List<SiebelActive> GetActiveList(SiebelActiveStatus status, SiebelActiveType type, int count = 10)
        {
            return SiebelDAL.GetActiveList(status, type, count);
        }

        public static List<SiebelActive> GetQuotationMasterForProductForecast(DateTime dt)
        {
            var qm = SiebelDAL.GetQuotationMasterForProductForecast(dt);

            List<SiebelActive> result = new List<SiebelActive>();
            foreach (var q in qm)
            {
                if (q.QuotationOpty != null && !string.IsNullOrEmpty(q.QuotationOpty.optyId) && q.QuotationDetail.Count > 0)
                {
                    string errMsg = string.Empty;
                    SiebelActive sa = new SiebelActive();
                    sa.OptyID = q.QuotationOpty.optyId;
                    sa.QuoteID = q.quoteId;
                    if (SiebelDAL.UpdateForecast(q, ref errMsg) == false)
                        sa.FailedLog = errMsg;
                    else
                        sa.FailedLog = "Success";
                    result.Add(sa);
                }
            }
            return result;
        }

        /// <summary>
        /// For AENC user to pick a SIEBEL account
        /// </summary>
        /// <param name="name"></param>
        /// <param name="RBU"></param>
        /// <param name="erpID"></param>
        /// <param name="country"></param>
        /// <param name="location"></param>
        /// <param name="state"></param>
        /// <param name="province"></param>
        /// <param name="status"></param>
        /// <param name="address1"></param>
        /// <param name="ZipCode"></param>
        /// <param name="City"></param>
        /// <param name="priSales"></param>
        /// <returns>DataTable dt</returns>
        public static DataTable GetAencSiebelAccountList(string name, string RBU, string erpID, string country, string location, string state, string province, string status, string address1, string ZipCode, string City, string priSales, System.Collections.ArrayList defaultRBU)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select TOP 100 a.ROW_ID AS ROW_ID, a.NAME as COMPANYNAME, IsNull(b.ATTRIB_05,'') as ERPID, ");
            sql.Append(" IsNull(d.COUNTRY,'') as COUNTRY, IsNull(d.CITY,'') as CITY, Isnull(a.LOC,'') as LOCATION, ");
            sql.Append(" IsNull(c.NAME, '') as RBU, IsNull(d.STATE,'') as STATE,IsNull(d.PROVINCE,'') as PROVINCE, ");
            sql.Append(" IsNull(a.CUST_STAT_CD,'') as STATUS ,IsNull(d.ADDR,'') as ADDRESS, IsNull(d.ZIPCODE,'') as ZIPCODE, IsNull(d.ADDR_LINE_2,'') as ADDRESS2, ");
            sql.Append(" (SELECT TOP 1 isnull(EMAIL_ADDR,'') FROM S_CONTACT WHERE ROW_ID=(SELECT TOP 1 PR_EMP_ID from S_POSTN where ROW_ID = (SELECT TOP 1 PR_POSTN_ID FROM S_ORG_EXT WHERE ROW_ID=a.ROW_ID)) and EMAIL_ADDR is not null) AS priSales ");
            sql.Append(" from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID ");
            sql.Append(" left join S_PARTY c on a.BU_ID=c.ROW_ID ");
            sql.Append(" left join S_ADDR_PER d on a.PR_ADDR_ID=d.ROW_ID where 1=1 ");

            if (!string.IsNullOrEmpty(name))
                sql.Append(string.Format(" and Upper(ISNULL(a.NAME,'')) like Upper(N'%{0}%') ", name.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));

            if (!string.IsNullOrEmpty(erpID))
                sql.Append(string.Format(" and Upper(ISNULL(b.ATTRIB_05,'')) like Upper(N'%{0}%') ", erpID.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));

            if (!string.IsNullOrEmpty(RBU))
            {
                string[] RBUs = RBU.Split(',');
                sql.Append(string.Format(" and Upper(c.NAME) in ({0}) ", string.Join(",", RBUs.Select(p => string.Format("'{0}'", p.ToUpper())))));
            }
            else
            {
                if (defaultRBU.Count > 0)
                {
                    sql.Append(string.Format(" and Upper(c.NAME) in ({0}) ", string.Join(",", defaultRBU.ToArray().Select(p => string.Format("'{0}'", p.ToString().ToUpper())))));
                }
            }

            if (!string.IsNullOrEmpty(country))
                sql.Append(string.Format(" and Upper(d.COUNTRY) like Upper(N'%{0}%') ", country.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));

            if (!string.IsNullOrEmpty(location))
                sql.Append(string.Format(" and Upper(a.LOC) like Upper(N'%{0}%') ", location.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));

            if (!string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(province))
            {
                sql.Append(string.Format(" and (Upper(d.STATE) like Upper(N'%{0}%') or Upper(d.PROVINCE) like Upper(N'%{1}%')) ", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"), province.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));
            }
            else if (!string.IsNullOrEmpty(state))
            {
                sql.Append(string.Format(" and Upper(d.STATE) like Upper(N'%{0}%') ", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));
            }
            else if (!string.IsNullOrEmpty(province))
            {
                sql.Append(string.Format(" and Upper(d.PROVINCE) like Upper(N'%{0}%') ", province.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));
            }

            if (!string.IsNullOrEmpty(status))
                sql.Append(string.Format(" and Upper(a.CUST_STAT_CD) = Upper(N'{0}') ", status.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));
            else
                sql.Append(string.Format(" and a.CUST_STAT_CD in ('01-Premier Channel Partner','02-Gold Channel Partner','03-Certified Channel Partner','04-DMS Premier Key','Account','06-Key Account','06P-Potential Key Account') "));//

            if (!string.IsNullOrEmpty(address1))
                sql.Append(string.Format(" and Upper(d.ADDR) like Upper(N'%{0}%') ", address1.Trim().ToUpper().Replace("'", "''").Replace("*", "%")));

            if (!string.IsNullOrEmpty(ZipCode))
                sql.Append(string.Format(" and Upper(d.ZIPCODE) like Upper(N'%{0}%') ", ZipCode.ToUpper()));

            if (!string.IsNullOrEmpty(City))
                sql.Append(string.Format(" and Upper(d.CITY) like Upper(N'%{0}%') ", City.ToUpper()));

            /*Check admin*/

            if (!string.IsNullOrEmpty(priSales))
                sql.Append(string.Format(" AND a.PR_POSTN_ID IN (SELECT ROW_ID from S_POSTN WHERE S_POSTN.PR_EMP_ID IN(SELECT ROW_ID FROM S_CONTACT WHERE Upper(S_CONTACT.EMAIL_ADDR) like '%{0}%')) ", priSales.ToUpper()));

            sql.Append(" order by a.ROW_ID ");

            return SqlProvider.dbGetDataTable("CRM", sql.ToString());
        }
    }
}
