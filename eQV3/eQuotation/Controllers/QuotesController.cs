using eQuotation.Utility;
using eQuotation.Business;
using eQuotation.DataAccess;
using eQuotation.Models.Quotes;
using Advantech.Myadvantech.Business;
using Advantech.Myadvantech.DataAccess;
using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Mvc;
using System.Data.Entity;
using System.Configuration;
using System.Collections.Generic;
using WorkFlowlAPI;
using eQuotation.Models.Enum;
using System.Data.SqlClient;
using WorkFlowlAPI.SimulatePrice;
using eQuotation.ViewModels.QuoteForm;
using Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model;
using System.IO;
using System.Web.Configuration;
using eQuotation.ViewModels;

namespace eQuotation.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class QuotesController : AppControllerBase
    {
        private AppDbContext dbContext = new AppDbContext();

        // GET: Quotes
        public ActionResult Index()
        {
            return PartialView();
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [Authorize]
        [AuthorizeInfo("MQ03002", "It allows user to view my team's quotations.", "Module Quote", "MQ00000")]
        public ActionResult MyTeamQuotation()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Created Date", Value = "Created_Date" });
            items.Add(new SelectListItem() { Text = "Last Updated Date", Value = "Last_Update_Date" });
            ViewBag.DateType = items;

            List<SelectListItem> items1 = new List<SelectListItem>();
            items1.Add(new SelectListItem() { Text = "All", Value = "-1" });
            items1.Add(new SelectListItem() { Text = "Draft", Value = "0" });
            items1.Add(new SelectListItem() { Text = "Finish", Value = "1" });
            ViewBag.Status = items1;
            var model = new QuotesViewModel();
            var myQM = new List<MyQuotationMaster>();


            myQM = getQuotation("MyTeam");

            model.QM = myQM;
            return PartialView(model);
        }

        //[OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [OutputCache(Duration = 1, VaryByParam = "*")]
        [Authorize]
        [AuthorizeInfo("MQ03001", "It allows user to view my quotations.", "Module Quote", "MQ00000")]
        public ActionResult MyQuotation()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Created Date", Value = "Created_Date" });
            items.Add(new SelectListItem() { Text = "Last Updated Date", Value = "Last_Update_Date" });
            ViewBag.DateType = items;

            List<SelectListItem> items1 = new List<SelectListItem>();
            items1.Add(new SelectListItem() { Text = "All", Value = "-1" });
            items1.Add(new SelectListItem() { Text = "Draft", Value = "0" });
            items1.Add(new SelectListItem() { Text = "Finish", Value = "1" });
            ViewBag.Status = items1;
            var model = new QuotesViewModel();
            var myQM = new List<MyQuotationMaster>();

            myQM = getQuotation("My");

            model.QM = myQM;
            return PartialView(model);
        }

        public ActionResult ResendCurrentApprovalEmail(string quoteID)
        {
            //更新waiting approvals 的mail body為最新內容
            var quotationMaster = QuoteBusinessLogic.GetQuotationMaster(quoteID);
            var waitReviewList = GPControlBusinessLogic.GetQuoteWaitApprovalsByQuoteId(quotationMaster.quoteId);
            QuotesViewModel QVModel = new QuotesViewModel();
            QVModel = setQuoteViewModel(quotationMaster);
            string currentRegion = eQuotation.DataAccess.SiebelDAL.GetMasterRBU(quotationMaster.siebelRBU);
            updateFinalMailBodyForApprovers(currentRegion, waitReviewList, QVModel);
            updateMailBodyForApprovers(currentRegion, waitReviewList, QVModel);

            var result = true;
            string Err = "";
            try
            {
                if (QuoteBusinessLogic.IsApprovedByAllApprovers(quoteID) || QuoteBusinessLogic.IsRejectedByApprover(quoteID))
                {
                    result = false;
                    Err = "Quote has been approved by all approvers or rejected by approver.";
                }
                else
                    QuoteBusinessLogic.SendApproveEmail(quoteID, AppContext.AppRegion);

            }
            catch (Exception ex)
            {
                result = false;
                Err = ex.Message.ToString();
            }
            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ResendFinalApprovalEmail(string quoteID)
        {
            var result = true;
            string Err = "";
            try
            {
                if (QuoteBusinessLogic.IsApprovedByAllApprovers(quoteID) || QuoteBusinessLogic.IsRejectedByApprover(quoteID))
                    QuoteBusinessLogic.SendFinalApprovalEmail(quoteID, AppContext.AppRegion);
                else
                {
                    result = false;
                    Err = "Quote is not approved by all approvers or not rejected by any approver."; 
                }
                    

            }
            catch (Exception ex)
            {
                result = false;
                Err = ex.Message.ToString();
            }
            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);

        }



        public ActionResult ResendCurrentApprovalEmailAndRecreateWorkflow(string quoteId)
        {
            var result = true;
            string Err = "";

            var waitingApprovals = GPControlBusinessLogic.GetQuoteWaitApprovalsByQuoteId(quoteId);

            if (waitingApprovals.Any())
            {
                var url = Request.Url.Scheme + "://" + Request.Url.Authority;
                QuoteApprovalDog.StartFlow(quoteId, url, waitingApprovals, AppContext.AppRegion);// Start new approval flow again for existed approvals
            }
            else
            {
                Err = "No waiting approver for this quote.";
            }
            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);

        }

        public List<MyQuotationMaster> getQuotation(string type)
        {
            var model = new QuotesViewModel();
            var qItem = new List<QuotationMaster>();
            var myQM = new List<MyQuotationMaster>();

            // Ryan 20171213 Use siebel rbu list to get quotation records instead of ORG.
            List<String> RegionalSiebelRBUList = eQuotation.DataAccess.SiebelDAL.GetRBUList(AppContext.AppRegion);

            string[] myTeamSalesCode = null;
            bool isMyTeam = false;
            if (type != "My")
            {
                isMyTeam = true;
                myTeamSalesCode = getMyTeamSalesCode();
            }

            qItem = QuoteBusinessLogic.GetQuotationMastersBySearchTerms(RegionalSiebelRBUList, "", ""
            , "", "", model.UserEmail, "", "", "", "", QuoteDocStatus.All, isMyTeam, myTeamSalesCode);

            //if (type == "My")
            //{
            //    qItem = QuotationMasterHelper.GetQuotationMasterBySearchTerms(RegionalSiebelRBUList, "", ""
            //    , "", "", model.UserEmail, "", "", "", "", QuoteDocStatus.All);
            //}
            //else
            //{
            //    //MyTeamQuotation Default 會顯示將登入者指派為IS 的報價單，但By Region會再多增加下列不同情境的報價單          
            //    qItem = QuotationMasterHelper.GetTeamQuotationMasterBySearchTerms(RegionalSiebelRBUList, "", ""
            //            , "", "", model.UserEmail, "", "", "", "", QuoteDocStatus.All, getMyTeamSalesCode());
            //}

            string region = AppContext.AppRegion;
            foreach (var item in qItem)
            {
                var ListItem = new MyQuotationMaster();
                ListItem.quoteId = item.quoteId;
                ListItem.quoteNo = item.quoteNo;
                ListItem.customId = item.customId;
                ListItem.quoteToName = item.quoteToName;

                var opty = eQuotationContext.Current.optyQuote.Where(d => d.quoteId == item.quoteId).FirstOrDefault();
                if (opty != null)
                {
                    ListItem.optyName = opty.optyName;
                }
                else
                {
                    ListItem.optyName = "";
                }

                var Sales1 = item.QuotationPartner.Where(s => s.TYPE == "E").FirstOrDefault();
                if (Sales1 != null)
                {
                    ListItem.salesEmail = Sales1.NAME;
                }
                else
                {
                    ListItem.salesEmail = "";
                }

                if (item.quoteDate != null)
                {
                    ListItem.quoteDate = TimeHelper.ConvertToLocalTime(item.quoteDate.Value, region).ToShortDateString();
                }
                else
                {
                    ListItem.quoteDate = "";
                }


                //ListItem.DOCSTATUS = QuoteBusinessLogic.getQuoteStatus(item.quoteId);
                ListItem.DOCSTATUS = item.ApprovalStatus;

                if (item.quoteToErpId != null)
                {
                    ListItem.quoteToErpId = item.quoteToErpId;
                }
                else
                {
                    ListItem.quoteToErpId = "";
                }

                var finishdate = eQuotationContext.Current.QuotationExtensionNew.Where(d => d.QuoteID == item.quoteId).FirstOrDefault();
                if (finishdate != null)
                {
                    if (finishdate.FinishDate != null)
                    {
                        ListItem.FinishDate = finishdate.FinishDate.Value.ToShortDateString();
                    }
                    else
                    {
                        ListItem.FinishDate = "";
                    }
                }
                else
                {
                    ListItem.FinishDate = "";
                }


                ListItem.Editable = IsQuoteEditable(ListItem.DOCSTATUS, type, region);
                ListItem.RevisionIsEnabled = (region == "ABB" || (region == "ACN" && (ListItem.DOCSTATUS == "Finish" || ListItem.DOCSTATUS == "Waiting for approval"))) ? true : false;

                ListItem.RevisionNumber = (int)item.Revision_Number;

                if (item.expiredDate != null)
                {
                    ListItem.ExpiredDate = item.expiredDate.Value.ToShortDateString();
                }
                else
                {
                    ListItem.ExpiredDate = "";
                }

                var dtQuote2OrderLog = SqlProvider.dbGetDataTable("EQ", String.Format("select * from QUOTE_TO_ORDER_LOG where QUOTEID = '{0}'", item.quoteId));
                if (dtQuote2OrderLog != null && dtQuote2OrderLog.Rows.Count > 0)
                {
                    if(dtQuote2OrderLog.Rows[0]["SO_NO"] !=null && !String.IsNullOrEmpty(dtQuote2OrderLog.Rows[0]["SO_NO"].ToString()))
                        ListItem.Quote2OrderNo = (dtQuote2OrderLog.Rows[0]["SO_NO"].ToString());

                    if (dtQuote2OrderLog.Rows[0]["ORDER_DATE"] != null && !String.IsNullOrEmpty(dtQuote2OrderLog.Rows[0]["ORDER_DATE"].ToString()))
                            ListItem.Quote2OrderDate = (dtQuote2OrderLog.Rows[0]["ORDER_DATE"].ToString());
                }

                myQM.Add(ListItem);
            }

            return myQM;
        }

        private string[] getMyTeamSalesCode()
        {
            string[] myTeamSalesCodes = new string[] { };
            if (AppContext.AppRegion == "ACN")
            {
                // if power user, get user Sector by ACN_EQ_Sales table(但AISC暫時不適用), then get all sales which belong to this sector
                foreach (var rolename in AppContext.UserBelongRoles)
                {
                    if (rolename.Contains("Power User"))
                    {
                        var sector = UserRoleBusinessLogic.getSectorBySalesEmail(AppContext.UserEmail);
                        List<ACN_EQ_Sales> sectorSales = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.Sector == sector).GroupBy(x => new { x.SalesCode, x.SalesEmail }).Select(s => s.FirstOrDefault()).ToList();
                        myTeamSalesCodes = sectorSales.Select(s => s.SalesCode).ToArray();
                    }
                }
            }
            else if (AppContext.AppRegion == "ABB") // get my team all sales code by emanager
            {
                //Supervisor can view all quotation, sales can view quotations within same team
                List<string> myTeamSalesCodesList = new List<string>();
                var roles = AppContext.UserBelongRoles;
                if (roles.Contains("ABB Sales Supervisor") || roles.Contains("ABB Customer Service"))
                {
                    //20180409 TC: Since not every eQ user has sales id on eManager, use SAP employee instead
                    DataTable dtsalesCode = DBUtil.dbGetDataTable("MY", string.Format(" Select SALES_CODE FROM SAP_EMPLOYEE (nolock) where PERS_AREA = 'US10' "));
                    for (int i = 0; i <= dtsalesCode.Rows.Count - 1; i++)
                        myTeamSalesCodesList.Add(dtsalesCode.Rows[i][0].ToString());
                    myTeamSalesCodes = myTeamSalesCodesList.ToArray();
                }
                else
                {
                    //DataTable dtSector = DBUtil.dbGetDataTable("MY", string.Format("Select id_sbu FROM EAI_IDMAP where id_email = '{0}' ", AppContext.UserEmail));

                    //for (int i = 0; i <= dtSector.Rows.Count - 1; i++)
                    //{
                    //    DataTable dtsalesCode = DBUtil.dbGetDataTable("MY", string.Format("  Select id_sap FROM EAI_IDMAP where id_sbu = '{0}' ", dtSector.Rows[i][0].ToString()));
                    //    for (int j = 0; j <= dtsalesCode.Rows.Count - 1; j++)
                    //        myTeamSalesCodesList.Add(dtsalesCode.Rows[j][0].ToString());
                    //}
                    //20180409 TC: Change to be based on SAP employee's sales office to control My Team's Quotation's visibility, 
                    //that Sales in same sales group can see each other's quote.
                    DataTable dtsalesCode = DBUtil.dbGetDataTable("MY", 
                        string.Format(" select a.SALES_CODE from SAP_EMPLOYEE a (nolock) " + 
                                      " where a.PERS_AREA='US10' and a.SALESGROUP in " + 
                                      " (select z.SALESGROUP from SAP_EMPLOYEE z (nolock) where z.EMAIL='{0}')", AppContext.UserEmail));
                    for (int j = 0; j <= dtsalesCode.Rows.Count - 1; j++)
                        myTeamSalesCodesList.Add(dtsalesCode.Rows[j][0].ToString());
                    //20180410 TC: Per Tracy, Juanita (21100032) services Fanny, so Fanny should be able to see all quotes made by Juanita
                    if (AppContext.UserEmail.Equals("FSCARGLE@ADVANTECH-BB.COM", StringComparison.CurrentCultureIgnoreCase) 
                        && !myTeamSalesCodesList.Contains("21100032"))
                        myTeamSalesCodesList.Add("21100032");

                    myTeamSalesCodes = myTeamSalesCodesList.ToArray();
                }
                
            }
            else if(AppContext.AppRegion == "ASG")
            {
                List<string> myTeamSalesCodesList = new List<string>();
                var rolenames = AppContext.UserBelongRoles;
                if (rolenames.Contains("ASG Power User") )
                {
                    //20180409 TC: Since not every eQ user has sales id on eManager, use SAP employee instead
                    DataTable dtsalesCode = DBUtil.dbGetDataTable("MY", string.Format(" Select SALES_CODE FROM SAP_EMPLOYEE (nolock) where PERS_AREA = 'SG01' "));
                    for (int i = 0; i <= dtsalesCode.Rows.Count - 1; i++)
                        myTeamSalesCodesList.Add(dtsalesCode.Rows[i][0].ToString());
                    myTeamSalesCodes = myTeamSalesCodesList.ToArray();
                }
                else
                {

                    StringBuilder sql = new StringBuilder();
                    sql.AppendFormat(" select distinct s.SALES_CODE from AD_MEMBER_GROUP ad "+
                                     " inner join  SAP_EMPLOYEE(nolock) s on ad.EMAIL = s.EMAIL where s.PERS_AREA = 'SG01' and ad.GROUP_NAME in ("+
                                     " select ad.GROUP_NAME from AD_MEMBER_GROUP ad (nolock) where ad.EMAIL = '{0}' and ad.GROUP_NAME in ('ASG.IIoT', 'ASG.AOnline', 'ASG. EIoT', 'ASG.SIoT '))", AppContext.UserEmail);
                    DataTable dtsalesCode = DBUtil.dbGetDataTable("MY", sql.ToString());
                    for (int i = 0; i <= dtsalesCode.Rows.Count - 1; i++)
                        myTeamSalesCodesList.Add(dtsalesCode.Rows[i][0].ToString());
                    myTeamSalesCodes = myTeamSalesCodesList.ToArray();
                    
                }
            }

            return myTeamSalesCodes;
        }

        private bool IsQuoteEditable(string quoteStatus, string quoteListPageType, string region)
        {
            if (quoteStatus == "Draft")
            {
                if (quoteListPageType == "My")
                    return true;
                else
                    if (region == "ABB")
                        return true;
            }
            return false;
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MQ03004", "It allows user to delete quotations.", "Module Quote", "MQ00000")]
        public ActionResult DeleteQuotation(string QuoteID)
        {
            var result = true;
            string Err = "";
            try
            {
                QuotationMasterHelper.UpdateQuotationStatus(QuoteID, OrderStatus.Deleted);
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, ID = QuoteID, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult MyQuotation(QuotesViewModel Quotation)
        {
            return Json(Quotation);
        }

        [HttpPost]
        [Authorize]
        public ActionResult getQuoteList(string QuoteNo, string Description, string AccountName, string ERPID
            , string DateType, string From, string To, int ddlStatus)
        {
            var result = true;
            string Err = "";
            var qItem = new List<QuotationMaster>();
            var items = new List<List<string>>();
            var model = new QuotesViewModel();
            try
            {
                var UserEmail = model.UserEmail;
                var quoteno = string.IsNullOrEmpty(QuoteNo) ? "" : QuoteNo;
                var description = string.IsNullOrEmpty(Description) ? "" : Description;
                var account = string.IsNullOrEmpty(AccountName) ? "" : AccountName;
                var erpid = string.IsNullOrEmpty(ERPID) ? "" : ERPID;
                var datetype = string.IsNullOrEmpty(DateType) ? "" : DateType;
                var from = string.IsNullOrEmpty(From) ? "" : From;
                var to = string.IsNullOrEmpty(To) ? "" : To;

                var Status = QuoteDocStatus.All;
                if (ddlStatus == -1)
                {
                    Status = QuoteDocStatus.All;
                }
                else if (ddlStatus == 0)
                {
                    Status = QuoteDocStatus.Active;
                }
                else if (ddlStatus == 1)
                {
                    Status = QuoteDocStatus.Finish;
                }

                // Ryan 20171213 Use siebel rbu list to get quotation records instead of ORG.
                List<String> RegionalSiebelRBUList = eQuotation.DataAccess.SiebelDAL.GetRBUList(AppContext.AppRegion);

                if (datetype == "Created_Date")
                {
                    qItem = QuoteBusinessLogic.GetQuotationMastersBySearchTerms(RegionalSiebelRBUList, quoteno, description
                   , account, erpid, UserEmail, from, to, "", "", Status,false,null);
                }
                else if (datetype == "Last_Update_Date")
                {
                    qItem = QuoteBusinessLogic.GetQuotationMastersBySearchTerms(RegionalSiebelRBUList, quoteno, description
                   , account, erpid, UserEmail, "", "", from, to, Status, false, null);
                }

                foreach (var item in qItem)
                {
                    var ListItem = new List<string>();

                    ListItem.Add(item.quoteId);
                    ListItem.Add(item.quoteNo);
                    ListItem.Add(item.customId);
                    ListItem.Add(item.quoteToErpId);
                    ListItem.Add(item.quoteToName);

                    var opty = eQuotationContext.Current.optyQuote.Where(d => d.quoteId == item.quoteId).FirstOrDefault();
                    if (opty != null)
                    {
                        ListItem.Add(opty.optyId);
                    }
                    else
                    {
                        ListItem.Add("");
                    }

                    var Sales2 = item.QuotationPartner.Where(s => s.TYPE == "E").FirstOrDefault();
                    if (Sales2 != null)
                    {
                        ListItem.Add(Sales2.NAME);
                    }
                    else
                    {
                        ListItem.Add("");
                    }

                    if (item.quoteDate != null)
                    {
                        ListItem.Add(item.quoteDate.Value.ToShortDateString());
                    }
                    else
                    {
                        ListItem.Add("");
                    }

                    //var quoteStatus = QuoteBusinessLogic.getQuoteStatus(item.quoteId);
                    var quoteStatus = item.ApprovalStatus;
                    ListItem.Add(quoteStatus);

                    var finishdate = eQuotationContext.Current.QuotationExtensionNew.Where(d => d.QuoteID == item.quoteId).FirstOrDefault();
                    if (finishdate != null)
                    {
                        if (finishdate.FinishDate != null)
                        {
                            ListItem.Add(finishdate.FinishDate.Value.ToShortDateString());
                        }
                        else
                        {
                            ListItem.Add("");
                        }
                    }
                    else
                    {
                        ListItem.Add("");
                    }


                    if (IsQuoteEditable(quoteStatus, "My", AppContext.AppRegion))
                        ListItem.Add("Editable");
                    else
                        ListItem.Add("NonEditable");

                    if (item.expiredDate != null)
                    {
                        ListItem.Add(item.expiredDate.Value.ToShortDateString());
                    }
                    else
                    {
                        ListItem.Add("");
                    }


                    items.Add(ListItem);
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = items, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult getTeamQuoteList(string QuoteNo, string Description, string AccountName, string ERPID
            , string DateType, string From, string To, int ddlStatus)
        {
            var result = true;
            string Err = "";
            var qItem = new List<QuotationMaster>();
            var items = new List<List<string>>();
            var model = new QuotesViewModel();
            try
            {
                var UserEmail = model.UserEmail;
                var quoteno = string.IsNullOrEmpty(QuoteNo) ? "" : QuoteNo;
                var description = string.IsNullOrEmpty(Description) ? "" : Description;
                var account = string.IsNullOrEmpty(AccountName) ? "" : AccountName;
                var erpid = string.IsNullOrEmpty(ERPID) ? "" : ERPID;
                var datetype = string.IsNullOrEmpty(DateType) ? "" : DateType;
                var from = string.IsNullOrEmpty(From) ? "" : From;
                var to = string.IsNullOrEmpty(To) ? "" : To;

                // Ryan 20171213 Use siebel rbu list to get quotation records instead of ORG.
                List<String> RegionalSiebelRBUList = eQuotation.DataAccess.SiebelDAL.GetRBUList(AppContext.AppRegion);
                //暫時先限制只能抓到ACN的報價單
                //string ORG = "CN10";
                //string ORG = "CN";
                //現在是給ACN用，所以必須是ACN
                //if (quoteno.Length > 2)
                //{
                //	if (quoteno.Substring(0, 3) != "ACN")
                //	{
                //		return Json(new { succeed = result, data = items, err = Err }, JsonRequestBehavior.AllowGet);
                //	}
                //}

                var Status = QuoteDocStatus.All;
                if (ddlStatus == -1)
                {
                    Status = QuoteDocStatus.All;
                }
                else if (ddlStatus == 0)
                {
                    Status = QuoteDocStatus.Active;
                }
                else if (ddlStatus == 1)
                {
                    Status = QuoteDocStatus.Finish;
                }

                var sector = "";
                // if power user, get user Sector
                foreach (var rolename in AppContext.UserBelongRoles)
                {
                    if (rolename.Contains("Power User"))
                        sector = UserRoleBusinessLogic.getSectorBySalesEmail(AppContext.UserEmail);
                }
                if (datetype == "Created_Date")
                {
                    qItem = QuoteBusinessLogic.GetQuotationMastersBySearchTerms(RegionalSiebelRBUList, quoteno, description
                   , account, erpid, UserEmail, from, to, "", "", Status, true, getMyTeamSalesCode());
                }
                else if (datetype == "Last_Update_Date")
                {
                    qItem = QuoteBusinessLogic.GetQuotationMastersBySearchTerms(RegionalSiebelRBUList, quoteno, description
                   , account, erpid, UserEmail, "", "", from, to, Status, true, getMyTeamSalesCode());
                }

                foreach (var item in qItem)
                {
                    var ListItem = new List<string>();

                    ListItem.Add(item.quoteId);
                    ListItem.Add(item.quoteNo);
                    ListItem.Add(item.customId);
                    ListItem.Add(item.quoteToErpId);
                    ListItem.Add(item.quoteToName);

                    var opty = eQuotationContext.Current.optyQuote.Where(d => d.quoteId == item.quoteId).FirstOrDefault();
                    if (opty != null)
                    {
                        ListItem.Add(opty.optyId);
                    }
                    else
                    {
                        ListItem.Add("");
                    }

                    var Sales2 = item.QuotationPartner.Where(s => s.TYPE == "E").FirstOrDefault();
                    if (Sales2 != null)
                    {
                        ListItem.Add(Sales2.NAME);
                    }
                    else
                    {
                        ListItem.Add("");
                    }
                    //ListItem.Add(item.salesEmail);

                    if (item.quoteDate != null)
                    {
                        ListItem.Add(item.quoteDate.Value.ToShortDateString());
                    }
                    else
                    {
                        ListItem.Add("");
                    }


                    //DateTime? quotedate = item.quoteDate;
                    //if (quotedate != null)
                    //{
                    //    DateTime newQuotedate = quotedate.Value;
                    //    ListItem.Add(item.quoteDate.Value.ToShortDateString());
                    //}
                    //else
                    //{
                    //    ListItem.Add("");
                    //}

                    //if (item.DOCSTATUS == 0)
                    //{
                    //    var sStatus = "Draft";
                    //    var quoteApproval = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == item.quoteId && p.Status == (int)QuoteApprovalStatus.Wait_for_review).OrderBy(a => a.LevelNum).FirstOrDefault();
                    //    if (quoteApproval != null && quoteApproval.Approver != null)
                    //        sStatus += "(Waiting for " + quoteApproval.Approver.Split('@')[0] + "'s approval)";

                    //    ListItem.Add(sStatus);
                    //}
                    //else
                    //{
                    //    ListItem.Add("Finish");
                    //}

                    //var quoteStatus = QuoteBusinessLogic.getQuoteStatus(item.quoteId);
                    var quoteStatus = item.ApprovalStatus;
                    ListItem.Add(quoteStatus);


                    var finishdate = eQuotationContext.Current.QuotationExtensionNew.Where(d => d.QuoteID == item.quoteId).FirstOrDefault();
                    if (finishdate != null && finishdate.FinishDate != null)
                    {
                        ListItem.Add(finishdate.FinishDate.Value.ToShortDateString());
                    }
                    else
                    {
                        ListItem.Add("");
                    }

                    if (IsQuoteEditable(quoteStatus, "MyTeam", AppContext.AppRegion))
                        ListItem.Add("Editable");
                    else
                        ListItem.Add("NonEditable");

                    if (item.expiredDate != null)
                    {
                        ListItem.Add(item.expiredDate.Value.ToShortDateString());
                    }
                    else
                    {
                        ListItem.Add("");
                    }

                    items.Add(ListItem);
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = items, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [Authorize]
        public ActionResult Preview(string quoteID, string[] SalesCode, string[] SalesName, string InsideSales, string Description, string ContactRowID, string AccountName, decimal Freight, string Warranty, string Comment, string QuotationNote, string ExpiredDate, string PONO)
        {
            QuotationMaster Quote = new QuotationMaster();
            List<QuotationDetail> QD = new List<QuotationDetail>();
            if (string.IsNullOrEmpty(quoteID))
            {
                setValue(SalesCode, SalesName, InsideSales, Description, ContactRowID, AccountName, Freight, Warranty, Comment, QuotationNote, ExpiredDate, PONO);
                Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            }
            else
            {
                Quote = QuoteBusinessLogic.GetQuotationMaster(quoteID);
            }

            QuotesViewModel QVModel = new QuotesViewModel();

            QVModel = setQuoteViewModel(Quote);
            QVModel.pagetype = "Template";
            return PartialView(TemplateView(AppContext.AppRegion), QVModel);
        }

        public string TemplateView(string region)
        {
            string view = "ACN_QuotationTemplate";
            if (!string.IsNullOrEmpty(region))
            {
                switch (region)
                {
                    case "ACN":
                        view = "ACN_QuotationTemplate";
                        break;
                    case "ABB":
                        view = "ABB_QuotationTemplate";
                        break;
                    case "ADLOG":
                        view = "ADLOG_QuotationTemplate";
                        break;
                    case "ICG":
                        view = "ICG_QuotationTemplate";
                        break;
                    case "ASG":
                        view = eQuotationDAL.GetRegionParameterValue(region, "", "QuotationTemplate_ViewName", ""); ;
                        break;
                    default:
                        break;
                }
            }

            return view;
        }

        public string ReviewQuoteApprovalView(string region)
        {
            string view = "ACN_QuotationTemplate";
            if (!string.IsNullOrEmpty(region))
            {
                switch (region)
                {
                    //case "ACN":
                    //    view = "ReviewQuoteApproval";
                    //    break;
                    case "ABB":
                        view = "ABB_ReviewQuoteApproval";
                        break;
                    case "ACN":
                    case "ASG":
                        view = eQuotationDAL.GetRegionParameterValue(region, "", "ReviewQuoteApproval_ViewName", "");
                        //_region = "ASG_ReviewQuoteApproval";
                        break;
                    default:
                        break;
                }
            }

            return view;
        }


        [Authorize]
        [AuthorizeInfo("MQ03007", "It allows user to convert quote to order.", "Module Quote", "MQ00000")]
        public ActionResult Quote2Cart(string QuoteID)
        {
            string _quoteid = QuoteID;
            bool ChangeQtyIsAllowed = false;
            IEnumerable<WorkFlowApproval> qaList = eQuotationContext.Current.WorkFlowApproval.Where(a => a.TypeID == _quoteid).ToList();
            if (qaList != null && qaList.Count() == 0)
                ChangeQtyIsAllowed = true;
            else
            {
                // if power user, get user Sector
                foreach (var rolename in AppContext.UserBelongRoles)
                {
                    if (rolename.Contains("Power User"))
                        ChangeQtyIsAllowed = true;
                }
            }

            QuotationMaster QM = QuoteBusinessLogic.GetQuotationMaster(_quoteid);
            string strMyAdvantechUrl = "http://my.advantech.com";
            if (Util.isTesting())
                strMyAdvantechUrl = "http://172.20.1.30:4002";


            return Redirect(string.Format(strMyAdvantechUrl + "/ORDER/Quote2CartEQ3.ASPX?UID={0}&USER={1}&COMPANY={2}&ORG={3}&ChangeQtyIsAllowed={4}"
               , QuoteID, AppContext.UserEmail, HttpUtility.UrlEncode(QM.quoteToErpId), QM.org, ChangeQtyIsAllowed.ToString()));
        }

        [Authorize]
        public ActionResult UpdateErpId(string QuoteID)
        {
            // Get ERP ID
            QuotationMaster QM = QuoteBusinessLogic.GetQuotationMaster(QuoteID);
            var quoteToErpId = QuoteBusinessLogic.GetErpIdFromSiebelByRowId(QuoteID, QM.quoteToRowId);

            // Update all the related data
            if (!string.IsNullOrEmpty(quoteToErpId))
            {
                QuoteBusinessLogic.UpdateErpIdAllRelated(quoteToErpId, QM.quoteId, QM.quoteToRowId);
            }
            else
            {
                quoteToErpId = "";
            }

            return Json(new { erpid = QM.quoteToErpId }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AuthorizeInfo("MQ03005", "It allows user to download pdf quotation.", "Module Quote", "MQ00000")]
        public ActionResult Download_pdf(string QuoteID)
        {
            string _quoteid = QuoteID;

            QuotationMaster QM = QuoteBusinessLogic.GetQuotationMaster(_quoteid);


            QuotesViewModel QVModel = new QuotesViewModel();

            QVModel = setQuoteViewModel(QM);

            // pass model to view and get html
            var strHTML = RazorViewToString.RenderRazorViewToString(this, TemplateView(AppContext.AppRegion), QVModel);
            var myContentByteAs = QuoteBusinessLogic.DownloadQuotePDFByHtmlString(strHTML);

            string fname = QM.quoteNo;
            //Response.Clear();
            //Response.AddHeader("Content-Type", "binary/octet-stream");
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fname) + ".pdf;size = " + myContentByteAs.Length.ToString());
            //Response.Flush();
            //Response.BinaryWrite(myContentByteAs);
            //Response.Flush(); Response.End();

            return File(myContentByteAs, "application/pdf", HttpUtility.UrlEncode(fname) + ".pdf");
        }

        [Authorize]
        public ActionResult DownloadPDFV2(string quoteId, string quoteTitle, DisplayItemOptionsForBto displayItemType, string advantechCompanyTitleOrg)
        {
            string _quoteid = quoteId;

            QuotationMaster qm = QuoteBusinessLogic.GetQuotationMaster(_quoteid);
            //qm.Remark = QuoteBusinessLogic.GetQuotationRemark(qm.quoteId);

            QuotesViewModel model = new QuotesViewModel();

            if (string.IsNullOrEmpty(advantechCompanyTitleOrg))
                qm.QuotationExtensionNew.CompanyTitle = qm.org;
            else
                qm.QuotationExtensionNew.CompanyTitle = advantechCompanyTitleOrg;

            model = setQuoteViewModel(qm);
            model.QuotationTitle = quoteTitle;
            model.DisplayItemType = displayItemType;
            // pass model to view and get html
            var strHTML = RazorViewToString.RenderRazorViewToString(this, TemplateView(AppContext.AppRegion), model);
            var myContentByteAs = QuoteBusinessLogic.DownloadQuotePDFByHtmlStringV2(strHTML, AppContext.AppRegion, true);

            string fname = qm.quoteNo;
            //Response.Clear();
            //Response.AddHeader("Content-Type", "binary/octet-stream");
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fname) + ".pdf;size = " + myContentByteAs.Length.ToString());
            //Response.Flush();
            //Response.BinaryWrite(myContentByteAs);
            //Response.Flush(); Response.End();

            return File(myContentByteAs, "application/pdf", HttpUtility.UrlEncode(fname) + ".pdf");
        }

        public ActionResult ACN_QuotationTemplate()
        {
            QuotationMaster QM = new QuotationMaster();
            QM = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            //QM.quoteToRowId

            QuotesViewModel QVModel = new QuotesViewModel();

            QVModel = setQuoteViewModel(QM);

            return PartialView(QVModel);
        }

        public ActionResult ABBQuotationTemplate()
        {
            QuotationMaster QM = new QuotationMaster();
            QM = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            //QM.quoteToRowId

            QuotesViewModel QVModel = new QuotesViewModel();

            QVModel = setQuoteViewModel(QM);

            return PartialView(QVModel);
        }

        public ActionResult PreviewQuotationTemplate(QuoteFormViewModel quoteFormModel)
        {
            var quote = quoteFormModel.ConverToQuote();
            QuotesViewModel quoteViewModel = new QuotesViewModel();

            quoteViewModel = setQuoteViewModel(quote);
            quoteViewModel.QuotationTitle = quoteFormModel.QuoteTitle;
            var viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuotationTemplate_ViewName", "QuotationTemplate");
            return PartialView(viewName, quoteViewModel);
        }

        public ActionResult QuotationTemplate(string quoteId, string quoteTitle,DisplayItemOptionsForBto displayItemType, string advantechCompanyTitleOrg)
        {
            var err = "";
            var result = true;
            var retStr = "";

            try
            {
                QuotationMaster qm = QuoteBusinessLogic.GetQuotationMaster(quoteId);
                //qm.Remark = QuoteBusinessLogic.GetQuotationRemark(quoteId); // alex: strange to put function here, wait modify.
                if(string.IsNullOrEmpty(advantechCompanyTitleOrg))
                    qm.QuotationExtensionNew.CompanyTitle = qm.org;
                else
                    qm.QuotationExtensionNew.CompanyTitle = advantechCompanyTitleOrg;


                QuotesViewModel quoteViewModel = new QuotesViewModel();
                quoteViewModel = setQuoteViewModel(qm);
                quoteViewModel.QuotationTitle = quoteTitle;
                quoteViewModel.DisplayItemType = displayItemType;
                var viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuotationTemplate_ViewName", "QuotationTemplate");
                retStr = RazorViewToString.RenderRazorViewToString(this, viewName, quoteViewModel);
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                result = false;
            }

            return Json(new { succeed = result, data = retStr, err = err }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AuthorizeInfo("MQ03008", "It allows user to view, download and forward quotation.", "Module Quote", "MQ00000")]
        public ActionResult ViewDownloadForwardQuotation(string quoteId)
        {
            QuotationMaster qm = QuoteBusinessLogic.GetQuotationMaster(quoteId);
            ViewDownloadForwardQuoteViewModel model = new ViewDownloadForwardQuoteViewModel(quoteId, qm.quoteNo, qm.org);
             
            if(AppContext.AppRegion == "ASG" &&  qm.DOCSTATUS != 1 && qm.QuotationExtensionNew.BelowGP == true)//Alex: For ASG, if quotation status is not finished and below GP, not allowing user to issue PDF or email.
                model.CanClickPdfAndEmail = false;

            model.DisplayItemOptionsForBtoIsViewable = QuoteBusinessLogic.IsContainBto(qm.QuotationDetail) && AppContext.AppRegion == "ASG";
            return PartialView(model);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public QuotesViewModel setQuoteViewModel(QuotationMaster Quote)
        {
            QuotesViewModel QVModel = new QuotesViewModel();

            QVModel.QuoteID = Quote.quoteId;
            QVModel.QuoteNo = Quote.quoteNo;
            QVModel.ERPID = Quote.quoteToErpId;
            QVModel.RevisionNo = Quote.Revision_Number.GetValueOrDefault(1).ToString();
            QVModel.AccountName = Quote.quoteToName;
            QVModel.QuoteDate = TimeHelper.ConvertToLocalTime(Quote.quoteDate.GetValueOrDefault(DateTime.Now), AppContext.AppRegion);
            QVModel.ExpiredDate = TimeHelper.ConvertToLocalTime(Quote.expiredDate.GetValueOrDefault(DateTime.Now.AddMonths(1)), AppContext.AppRegion);
            QVModel.PrintDate = TimeHelper.ConvertToLocalTime(DateTime.Now, AppContext.AppRegion);
            QVModel.tax = Quote.tax.GetValueOrDefault(0);
            QVModel.Freight = Quote.freight.GetValueOrDefault(0);
            QVModel.SimulatePriceErrorMessage = Quote.SimulatePriceErrorMessage;
            //QVModel.GeneralRateList = Quote.ACN_EQ_QuotationEx.GeneralRate.GetValueOrDefault(0);


            //20180302 取消讓user修改0.27-->0.24, default即為0.24
            //decimal[] gerneralRateList = { 0.27m, 0.24m };

            if (Quote.QuotationExtensionNew != null)
            {
                //QVModel.GeneralRateList = gerneralRateList.ToList().Select(c => new SelectListItem
                //{
                //    Text = (c * 100).ToString(),
                //    Value = c.ToString(),
                //    Selected = (c == Quote.QuotationExtensionNew.GeneralRate.GetValueOrDefault(0))
                //}).ToList();

                //New expired date is for ACN PSM adjust expired date function
                if (Quote.QuotationExtensionNew.NewExpiredDate != null)
                    QVModel.ACNMaxNewExpiredDate = TimeHelper.ConvertToLocalTime(Quote.QuotationExtensionNew.NewExpiredDate.Value, AppContext.AppRegion);
                else
                    QVModel.ACNMaxNewExpiredDate = Quote.quoteDate.GetValueOrDefault(DateTime.Now).AddDays(180);


                if (!string.IsNullOrEmpty(Quote.QuotationExtensionNew.Warranty))
                {
                    QVModel.Warranty = Quote.QuotationExtensionNew.Warranty;
                }
                else
                {
                    QVModel.Warranty = "2年";
                }
            }
            //else
            //{
            //    QVModel.GeneralRateList = gerneralRateList.ToList().Select(c => new SelectListItem
            //    {
            //        Text = (c * 100).ToString(),
            //        Value = c.ToString()
            //    }).ToList();
            //}

            QVModel.Currency = Quote.currency;
            QVModel.CurrencySign = Advantech.Myadvantech.DataAccess.MyExtension.GetCurrencySignByCurrency(Quote.currency);
            QVModel.Description = Quote.customId;
            QVModel.Sales = Quote.salesEmail;
            QVModel.SalesComment = Quote.relatedInfo;
            QVModel.Opportunity = Quote.QuotationOpty == null ? "" : Quote.QuotationOpty.optyName;
            QVModel.OptyStage = Quote.QuotationOpty == null ? "" : Quote.QuotationOpty.optyStage;
            QVModel.TotalMargin = (double)QuoteBusinessLogic.GetTotalMarginByQuotationDetails(Quote.QuotationDetail);
            if (QuoteBusinessLogic.IsACNLoosItemQuote(Quote.QuotationDetail))
                QVModel.ACNTotalMargin = (double)QuoteBusinessLogic.GetACNTotalMarginForQuote(Quote.QuotationDetail);
            else
                QVModel.ACNTotalMargin = (double)QuoteBusinessLogic.GetMinACNMarginForConfigurationList(Quote.QuotationDetail);
            QVModel.QuoteApprovalList = Quote.WaitingApprovals;

            SIEBEL_ACCOUNT _SiebelAccount = Advantech.Myadvantech.DataAccess.SiebelDAL.getSiebelAccount(Quote.quoteToRowId);
            if (_SiebelAccount != null)
            {
                QVModel.Address = _SiebelAccount.ADDRESS;
            }

            SIEBEL_CONTACT _SiebelContact = Advantech.Myadvantech.DataAccess.SiebelDAL.GetSiebelContact(Quote.salesRowId);
            if (_SiebelContact != null)
            {
                //中國、台灣的姓名排列不同
                if (_SiebelContact.OrgID != null && (_SiebelContact.OrgID == "ACN" || _SiebelContact.OrgID == "ATW"))
                {
                    QVModel.AccountContactName = _SiebelContact.LastName + _SiebelContact.FirstName;
                }
                else
                {
                    QVModel.AccountContactName = _SiebelContact.FirstName + " " + _SiebelContact.LastName;
                }

                QVModel.AccountContactTel = UserRoleBusinessLogic.GetCountryTelephoneFormat(_SiebelContact.WorkPhone, AOnlineRegion.ACN);
                //QVModel.AccountContactFax = UserRoleBusinessLogic.GetCountryTelephoneFormat(_SiebelContact.FaxNumber, AOnlineRegion.ACN);
                QVModel.AccountContactEmail = _SiebelContact.EMAIL_ADDRESS;
            }

            //抓出負責業務相關資料

            List<EQPARTNER> EP = new List<EQPARTNER>();
            EP = Quote.QuotationPartner.Where(e => e.TYPE == "E").ToList();
            if (EP.Count > 0)
            {
                DataTable DT = new DataTable();
                var sql = new StringBuilder();
                sql.AppendLine("  select top 1 id_chi, id_email from EAI_IDMAP  ");
                sql.AppendFormat(" where id_sap = '{0}' and id_email = '{1}' ", EP[0].ERPID, EP[0].NAME);
                DT = DBUtil.dbGetDataTable("MY", sql.ToString());
                for (var i = 0; i < DT.Rows.Count; i++)
                {
                    QVModel.SalesName = DT.Rows[i][0].ToString();
                    QVModel.SalesEmail = DT.Rows[i][1].ToString().ToLower().Trim();
                }
                if (AppContext.AppRegion == "ACN" || AppContext.AppRegion == "ASG")
                {
                    DataTable DT2 = new DataTable();
                    DT2 = eQuotation.DataAccess.SiebelDAL.GetSalesInfo(EP[0].NAME);
                    for (var i = 0; i < DT2.Rows.Count; i++)
                    {
                        if(AppContext.AppRegion == "ACN")
                            QVModel.SalesTel = UserRoleBusinessLogic.GetCountryTelephoneFormat(DT2.Rows[i][1].ToString(), AOnlineRegion.ACN);
                        else
                            QVModel.SalesTel = UserRoleBusinessLogic.GetCountryTelephoneFormat(DT2.Rows[i][1].ToString(), AOnlineRegion.NA);
                    }
                }

                //if (AppContext.AppRegion == "ABB")
                //{
                //    var sql = new StringBuilder();
                //    sql.AppendLine("  select top 1 id_chi, id_email from EAI_IDMAP  ");
                //    sql.AppendFormat(" where id_sap = '{0}' and id_email = '{1}' ", EP[0].ERPID, EP[0].NAME);
                //    DT = DBUtil.dbGetDataTable("MY", sql.ToString());
                //    for (var i = 0; i < DT.Rows.Count; i++)
                //    {
                //        QVModel.SalesName = DT.Rows[i][0].ToString();
                //        QVModel.SalesEmail = DT.Rows[i][1].ToString().ToLower().Trim();
                //    }
                //}
                //else 
                //{
                //    //DT = eQuotation.DataAccess.SAPDAL.GetSalesInfo(EP[0].NAME);
                //    DT = eQuotation.DataAccess.SAPDAL.GetSalesInfoBySalesCode(EP[0].ERPID);
                //    for (var i = 0; i < DT.Rows.Count; i++)
                //    {
                //        QVModel.SalesName = DT.Rows[i][0].ToString();
                //        QVModel.SalesEmail = DT.Rows[i][2].ToString().ToLower().Trim();
                //    }
                //    DataTable DT2 = new DataTable();
                //    DT2 = eQuotation.DataAccess.SiebelDAL.GetSalesInfo(EP[0].NAME);
                //    for (var i = 0; i < DT2.Rows.Count; i++)
                //    {
                //        QVModel.SalesTel = UserRoleBusinessLogic.GetCountryTelephoneFormat(DT2.Rows[i][1].ToString(), AOnlineRegion.ACN);
                //    }
                //}
             
            }

            //QuoteTitle 下拉選單設定
            //var defaultQuoteTitleOptions = QuoteBusinessLogic.GetQuoteTitleOptions(AppContext.AppRegion);
            //if (defaultQuoteTitleOptions.Any())
            //{
            //    QVModel.QuoteTitleOptions = defaultQuoteTitleOptions
            //                  .Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == Quote.QuotationExtensionNew.QuoteTitle })
            //                  .ToList();
            //}
            //QVModel.QuotationTitle = Quote.QuotationExtensionNew.QuoteTitle;

            QVModel.QuoteTotalAmount_WithoutTax = Quote.PreTaxTotalAmount;
            QVModel.QuoteTotalAmount_WithTax = Quote.PostTaxTotalAmount;
            QVModel.QuoteTotalTax = Quote.TotalTaxAmount;
            QVModel.QuoteTotalGeneralAmount = Quote.TotalGeneralAmount;
            QVModel.QuotationNotes = Quote.quoteNote;
            QVModel.Remarks = Quote.Remark;

            //QVModel.QuotationDetails = Quote.QuotationDetail;
            QVModel.QD = getMyQuotationDetail(Quote); // for 畫面呈現的QD
            QuotationTemplateInformation qti = null;

            if (AppContext.AppRegion == "ACN")
            {
                qti = new QuotationTemplateInformation();
                qti.RBUCompanyName = "北京研华兴业电子科技有限公司";
                qti.RBUCompanyAddress = "北京市海淀区上地信息产业基地六街七号";
                qti.RBUCompanyTel = "86-10-6298-4346";
                qti.RBUCompanyZipCode = "100085";
                qti.RBUBankAccountName = "花旗银行北京分行";
                qti.RBUBankAccountNumber = "1733158219";

                if (Quote.QuotationExtensionNew != null && Quote.QuotationExtensionNew.CompanyTitle != null)
                {
                    if(Quote.QuotationExtensionNew.CompanyTitle == "CN30")
                    {
                        qti.RBUCompanyName = "上海研华慧胜智能科技有限公司";
                        qti.RBUCompanyAddress = "上海市静安区江场三路136号";
                        qti.RBUCompanyTel = "86-21-3632-1616";
                        qti.RBUCompanyZipCode = "200436";
                        qti.RBUBankAccountName = "花旗银行 (中国) 有限公司上海分行";
                        qti.RBUBankAccountNumber = "1747561202";
                    }
                    else if (Quote.QuotationExtensionNew.CompanyTitle == "CN70")
                    {
                        qti.RBUCompanyName = "研华服创（上海）智能科技有限公司";
                        qti.RBUCompanyAddress = "上海市静安区江场三路56、58号901室";
                        qti.RBUCompanyTel = "86-21-3632-1616";
                        qti.RBUCompanyZipCode = "200436";
                        qti.RBUBankAccountName = "花旗银行 (中国) 有限公司上海分行";
                        qti.RBUBankAccountNumber = "1778345206";
                    }
                }   
            }
            else if (AppContext.AppRegion == "ICG")
            {
                qti = new QuotationTemplateInformation();
                qti.RBUCompanyName = "Advantech Quotation";
                qti.RBUCompanyAddress = "No. 1, Alley 20, Lane 26, Rueiguang Road, Neihu District, Taipei 11491, Taiwan, R.O.C.";
                qti.RBUCompanyTel = "886-2-2792-7818";
                qti.RBUCompanyZipCode = "11491";
                qti.RBUBankAccountName = "";
                qti.RBUBankAccountNumber = "";
            }
            else if (AppContext.AppRegion == "ABB")
            {
                qti = new QuotationTemplateInformation();
                var tc = DBUtil.dbExecuteScalar("MY", "SELECT TOP 1 [File_Data] FROM Terms_Conditions WHERE ORG = 'US10'");
                if (tc != null && !string.IsNullOrEmpty(tc.ToString()))
                    qti.RBUTermsAndCondition = tc.ToString();
            }
            else if (AppContext.AppRegion == "ASG")
            {
                qti = new QuotationTemplateInformation();
                var tc = DBUtil.dbExecuteScalar("MY", "SELECT TOP 1 [File_Data] FROM Terms_Conditions WHERE ORG = 'US10'");
                if (tc != null && !string.IsNullOrEmpty(tc.ToString()))
                    qti.RBUTermsAndCondition = tc.ToString();
            }

            QVModel.QuoteTemplateInformation = qti;
            //需簽核理由註解用，只適用ACN
            QVModel.CommentExp = AppContext.AppRegion == "ACN" ? "*销售必填项：最终客户名称，最终客户价格，MOQ，项目名称，项目预计总数量，项目持续时间，今年预计出货数量，DI任务号" : "";

            QVModel.QuotationPartner = Quote.QuotationPartner;

            ///Alex 20180604 add:
            QVModel.SoldToPartner = Quote.QuotationPartner.Where(s => s.TYPE == "SOLDTO").FirstOrDefault();
            QVModel.ShipToPartner = Quote.QuotationPartner.Where(s => s.TYPE == "S").FirstOrDefault();
            QVModel.BillToPartner = Quote.QuotationPartner.Where(s => s.TYPE == "B").FirstOrDefault();

            QVModel.PaymentTerm = Quote.paymentTerm;
            QVModel.ShipTerm = Quote.shipTerm;
            if(Quote.INCO1!=null && Quote.INCO2!=null)
                QVModel.Incoterms = Quote.INCO1 + ", " + Quote.INCO2;
            QVModel.DeliveryDate = TimeHelper.ConvertToLocalTime(Quote.deliveryDate.GetValueOrDefault(DateTime.Now), AppContext.AppRegion);

            return QVModel;
        }

        public ActionResult UpdateTermAndCondition(string org, string tc)
        {
            var result = true;
            string Err = "";

            var oldTc = DBUtil.dbExecuteScalar("MY", string.Format("SELECT TOP 1 [File_Data] FROM Terms_Conditions WHERE ORG = '{0}'", org));

            try
            {
                if (oldTc != null)
                {
                    var sqlParameter = new List<SqlParameter>();
                    SqlParameter p1 = new SqlParameter("@pn1", SqlDbType.NText);
                    p1.Value = tc;
                    sqlParameter.Add(p1);

                    SqlParameter p2 = new SqlParameter("@pn2", SqlDbType.NVarChar, 20);
                    p2.Value = org;
                    sqlParameter.Add(p2);


                    Advantech.Myadvantech.DataAccess.SqlProvider.dbExecuteNoQuery2("MY", "UPDATE Terms_Conditions SET [File_Data] = @pn1 WHERE ORG = @pn2 ", sqlParameter.ToArray());

                }
                else
                {
                    result = false;
                    Err = "No Term/Condition for " + org + ".";
                }
            }
            catch(Exception ex)
            {
                result = false;
                Err = ex.Message;
            }

            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UpdateFinalMailBody(string quoteId)
        {
            var result = true;
            string Err = "";

            try
            {

                    var quotationMaster = QuoteBusinessLogic.GetQuotationMaster(quoteId);
                    QuotesViewModel quoteVModel = setQuoteViewModel(quotationMaster);
                    var url = Request.Url.Scheme + "://" + Request.Url.Authority;

                    quoteVModel.pagetype = "QuoteApprovalMail";

                    var finalMailBody = RenderRazorViewToString(ReviewQuoteApprovalView(AppContext.AppRegion), quoteVModel);
                    var QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId).OrderBy(p => p.LevelNum).ToList();

                    foreach (var qa in QAlist)
                    {

                        qa.FinalMailBody = finalMailBody;
                        qa.Update();
                    }

            }
            catch (Exception ex)
            {
                result = false;
                Err = ex.Message;
            }

            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        [AuthorizeInfo("MQ03006", "It allows user to send email with quotations.", "Module Quote", "MQ00000")]
        public ActionResult SendEmail(string QuoteID, string QuoteNo)
        {
            var model = new QuotesViewModel();
            model.QuoteID = QuoteID;
            model.QuoteNo = QuoteNo;
            return PartialView("_SendEmail", model);
        }

        [Authorize]
        public ActionResult SendEmailV2(string quoteId, string quoteNo, string quoteTitle, DisplayItemOptionsForBto displayItemType, string advantechCompanyTitleOrg)
        {
            var err = "";
            var result = true;
            var retStr = "";

            try
            {
                var model = new SendEmailViewModel();
                model.QuoteId = quoteId;
                model.QuoteNo = quoteNo;
                model.AdvantechCompanyTitleOrg = advantechCompanyTitleOrg;
                model.QuoteTitle = quoteTitle;
                model.DisplayItemType = displayItemType;
                model.CustomerMailSubject = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "CustomerMailSubject", "Advantech eQuotation") + " " + quoteNo;
                retStr = RazorViewToString.RenderRazorViewToString(this, "_SendEmailV2", model);
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                result = false;
            }

            return Json(new { succeed = result, data = retStr, err = err }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult SendV2(SendEmailViewModel model)
        {
            var result = true;
            string Err = "";

            try
            {
                var oMail = new System.Net.Mail.MailMessage();

                QuotationMaster qm = QuoteBusinessLogic.GetQuotationMaster(model.QuoteId);
                //qm.Remark = QuoteBusinessLogic.GetQuotationRemark(model.QuoteId);

                var salesRepresentative = qm.QuotationPartner.Where(s => s.TYPE == "E").FirstOrDefault();
                if (salesRepresentative != null && !string.IsNullOrEmpty(salesRepresentative.NAME))
                {
                    var salesEmail = salesRepresentative.NAME;
                    oMail.From = new System.Net.Mail.MailAddress(salesEmail);
                }
                else
                {
                    oMail.From = new System.Net.Mail.MailAddress("myadvantech@advantech.com");
                }

                var toEmail = model.ReciverEmail.Split(';');
                foreach (var email in toEmail)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        oMail.To.Add(email);
                    }
                }
                oMail.Bcc.Add("myadvantech@advantech.com");

                oMail.Subject = model.MailSubject;


                QuotesViewModel quoteVModel = new QuotesViewModel();
                var QC = new QuotesController();
                qm.QuotationExtensionNew.CompanyTitle = model.AdvantechCompanyTitleOrg;
                quoteVModel = QC.setQuoteViewModel(qm);
                quoteVModel.QuotationTitle = model.QuoteTitle;
                quoteVModel.DisplayItemType = model.DisplayItemType;
                quoteVModel.pagetype = "SendMailToCustomer";

                // pass model to view and get html
                var viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuotationTemplate_ViewName", "QuotationTemplate");

                var strHTML = RazorViewToString.RenderRazorViewToString(this, viewName, quoteVModel);
                var myContentByteAs = QuoteBusinessLogic.DownloadQuotePDFByHtmlStringV2(strHTML, AppContext.AppRegion, true);
                //var myContentByteAs = QuoteBusinessLogic.DownloadQuotePDFByHtmlString(strHTML);
                var MS = new MemoryStream(myContentByteAs);

                if (model.FileType == FileType.HTML)//html
                {
                    oMail.IsBodyHtml = true;
                    oMail.Body = "<div>" + model.MailBody + "</div><br />" + strHTML;
                }
                else //pdf
                {
                    oMail.IsBodyHtml = true;
                    oMail.Body = "<div>" + model.MailBody + "</div>";
                    oMail.Attachments.Add(new System.Net.Mail.Attachment(MS, string.Format("{0}.pdf", qm.quoteNo)));
                }

                //string smtp = WebConfigurationManager.AppSettings["SMTPServer"];
                string smtp = QuoteBusinessLogic.GetSMTPServerByRegion(AppContext.AppRegion);
                var oSmtp = new System.Net.Mail.SmtpClient(smtp);
                try // alex 20180710 add try catch for ACN Mail server
                {
                    oSmtp.Send(oMail);
                }
                catch (Exception ex)
                {
                    //throw;
                    oSmtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                    try
                    {
                        oSmtp.Send(oMail);
                    }
                    catch
                    {
                        try
                        {
                            oSmtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServerBAK"]);
                            oSmtp.Send(oMail);
                        }
                        catch { throw; }
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = "", err = Err }, JsonRequestBehavior.AllowGet);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [Authorize]
        [AuthorizeInfo("MQ03003", "It allows user to create/edit/copy quotations.", "Module Quote", "MQ00000")]
        public ActionResult CreateNewQuotation(string ID, string type)
        {
            //// For new version region, redirect to new action///
            if (AppContext.IsNewVersion)
            {
                if(string.IsNullOrEmpty(ID))
                    return RedirectToAction("CreateNewQuotationV2", "Quotes");
                else
                    return RedirectToAction("EditQuotationV2", "Quotes", new { quoteId = ID, copyType = type });
            }
            //// For new version region, redirect to new action///

            var model = new QuotesViewModel();
            model.Init();//建立Session

            //model.SetValue();

            var QuotationMaster = new QuotationMaster();
            QuotationMaster = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            //========Region Variables Settings========            
            String ViewName = String.Empty;
            String QuotePrefix = String.Empty;
            String DescriptionFormat = String.Empty;
            String WarrantyDescription = String.Empty;
            String IS_Group = String.Empty;
            Decimal TaxRate = 0;
            int ExpiredDays = 0;

            if (AppContext.AppRegion == "ACN")
            {
                ViewName = "CreateNewQuotation";
                TaxRate = 0.16m;
                QuotePrefix = "ACNQ";
                DescriptionFormat = "*格式：最终用户+主要料号+其他";
                WarrantyDescription = "2年";
                IS_Group = "IS.ACN";
                ExpiredDays = 30;
            }
            else if (AppContext.AppRegion == "ABB")
            {
                ViewName = "ABB_CreateNewQuotation";
                TaxRate = 0;
                QuotePrefix = "BBEQ";
                WarrantyDescription = "2 years";
                IS_Group = "";
                ExpiredDays = 90;
            }
            else if (AppContext.AppRegion == "ADLOG")
            {
                ViewName = "ADLOG_CreateNewQuotation";
                TaxRate = 0;
                QuotePrefix = "DLGQ";
                WarrantyDescription = "2 years";
                IS_Group = "";
                ExpiredDays = 30;
            }
            else if (AppContext.AppRegion == "ICG")
            {
                ViewName = "ICG_CreateNewQuotation";
                TaxRate = 0;
                QuotePrefix = "ICGQ";
                WarrantyDescription = "2 years";
                IS_Group = "ICG.sales";
                ExpiredDays = 30;
            }
            //========End Region Variables Settings========


            if (string.IsNullOrEmpty(ID))
            {
                QuotationMaster.quoteNo = QuoteBusinessLogic.GetEQOrderNumber(QuotePrefix, Util.isTesting());
                QuotationMaster.quoteId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                QuotationMaster.quoteDate = DateTime.Now;
                QuotationMaster.expiredDate = DateTime.Now.AddDays(ExpiredDays);
                QuotationMaster.createdBy = AppContext.UserEmail;
                QuotationMaster.createdDate = DateTime.Now;
                QuotationMaster.LastUpdatedBy = AppContext.UserEmail;
                QuotationMaster.LastUpdatedDate = DateTime.Now;
                QuotationMaster.deliveryDate = DateTime.Now.AddDays(3);
                QuotationMaster.reqDate = DateTime.Now;
                QuotationMaster.DOCSTATUS = 0;
                QuotationMaster.PODate = DateTime.Now;
                QuotationMaster.Revision_Number = 1;
                QuotationMaster.Active = true;
                QuotationMaster.tax = TaxRate;
                QuotationMaster.isRepeatedOrder = 0;
                QuotationMaster.PO_NO = String.Empty;

                QuotationExtensionNew QuotationExtensionNew = new QuotationExtensionNew();
                QuotationExtensionNew.QuoteID = QuotationMaster.quoteId;
                QuotationExtensionNew.GeneralRate = 0.24m;
                QuotationExtensionNew.Warranty = WarrantyDescription;
                QuotationMaster.QuotationExtensionNew = QuotationExtensionNew;

                model.QuoteNo = QuotationMaster.quoteNo;
                model.QuoteID = QuotationMaster.quoteId;
                model.QuoteDate = (DateTime)QuotationMaster.quoteDate;
                model.ExpiredDate = (DateTime)QuotationMaster.expiredDate;
                model.tax = QuotationMaster.tax.GetValueOrDefault();
                model.pagetype = "Create";
                model.ORGList = new List<SelectListItem>();
                model.ContactList = new List<SelectListItem>();
                model.Warranty = QuotationMaster.QuotationExtensionNew.Warranty;
                model.RevisionNo = QuotationMaster.Revision_Number.ToString();
                model.PONO =  QuotationMaster.PO_NO;

                // Add siebel active create quote record
                SiebelActive SA = new SiebelActive();
                SA.ActiveSource = "eQuotation";
                SA.ActiveType = "CreateQuote";
                SA.Status = "UnProcess";
                SA.QuoteID = QuotationMaster.quoteId;
                SA.CreateBy = AppContext.UserEmail;
                SA.CreatedDate = DateTime.Now;
                SA.LastUpdatedBy = AppContext.UserEmail;
                SA.LastUpdatedDate = DateTime.Now;
                QuotationMaster.SiebelActive.Add(SA);
            }
            else
            {
                if (!string.IsNullOrEmpty(type))
                {
                    if (type.Equals("Revise", StringComparison.OrdinalIgnoreCase))
                    {
                        String OriginalQuoteNo = QuoteBusinessLogic.GetQuotationMaster(ID).quoteNo;
                        int RevisionNO = (QuotationMasterHelper.GetQuotationMastersByQuoteNo(OriginalQuoteNo).FirstOrDefault() == null) ? 1 : (int)QuotationMasterHelper.GetQuotationMastersByQuoteNo(OriginalQuoteNo).OrderByDescending(d => d.Revision_Number).FirstOrDefault().Revision_Number + 1;

                        QuotationMaster = QuoteBusinessLogic.CopyQuotation(ID, Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15), OriginalQuoteNo, AppContext.UserEmail, TaxRate, RevisionNO);
                        model.pagetype = "Revise";
                    }
                    else
                    {
                        QuotationMaster = QuoteBusinessLogic.CopyQuotation(ID, Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15), QuoteBusinessLogic.GetEQOrderNumber(QuotePrefix, Util.isTesting()), AppContext.UserEmail, TaxRate);
                        model.pagetype = "Copy";
                    }
                }
                else
                {
                    QuotationMaster = QuoteBusinessLogic.GetQuotationMaster(ID);
                    model.pagetype = "Edit";
                }

                //20180410 Get price again(itp, pcp price, bom cost)            
                QuotationMaster.RemoveUnorderableItems();
                string errMsg = "";
                
                if (model.pagetype != "Edit" && QuotationMaster.QuotationDetail != null && QuotationMaster.QuotationDetail.Count > 0)
                {
                    //QuotationMaster.SimulatePrice(ref errMsg);
                    QuoteSimulatePrice.StartFlow(QuotationMaster, ref errMsg);// 20180423 Move simulate price and ITP to workflow
                    if (!string.IsNullOrEmpty(errMsg)) // if error, clear all items, and show message to user 
                    {
                        QuotationMaster.QuotationDetail.Clear();
                        QuotationMaster.SimulatePriceErrorMessage = "Clear all items because: " + errMsg;
                    }
                }
                else
                    QuotationMaster.GetProductBOMCost("CNY", ref errMsg);

                System.Web.HttpContext.Current.Session["QuotationMaster"] = QuotationMaster;
                model = setQuoteViewModel(QuotationMaster);
                model.AccountRowID = QuotationMaster.quoteToRowId;
                model.org = QuotationMaster.org;
                model.QuotationNotes = QuotationMaster.quoteNote;
                model.PONO = String.IsNullOrEmpty(QuotationMaster.PO_NO) ? string.Empty : QuotationMaster.PO_NO;

                //ORG下拉選單設定
                model.ORGList = new List<SelectListItem>();
                var SAPList = new List<SAP_DIMCOMPANY>();
                SAPList = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyByID(QuotationMaster.quoteToErpId);
                var invalidOrg = WebConfigurationManager.AppSettings["InvalidOrg"].Replace("'", "").Replace("(", "").Replace(")", "").Split(',').ToList();
                if (SAPList != null && SAPList.Count > 0)
                {
                    for (int i = 0; i <= SAPList.Count - 1; i++)
                    {
                        if (!invalidOrg.Contains(SAPList[i].ORG_ID))
                        {
                            model.ORGList.Add(new SelectListItem()
                            {
                                Text = SAPList[i].ORG_ID,
                                Value = SAPList[i].ORG_ID,
                                Selected = (SAPList[i].ORG_ID == QuotationMaster.org ? true : false)
                            });
                        }
                    }
                }
                else
                {
                    // Add default org for no ERPID case.
                    model.ORGList.Add(new SelectListItem()
                    {
                        Text = DataAccess.SAPDAL.GetDefaultOrgByRBU(AppContext.AppRegion),
                        Value = DataAccess.SAPDAL.GetDefaultOrgByRBU(AppContext.AppRegion),
                        Selected = true
                    });
                }


                //Customer Contact
                model.ContactList = new List<SelectListItem>();
                var Contact = new List<SIEBEL_CONTACT>();
                Contact = MyAdvantechDAL.GetSiebelContactByERPID(QuotationMaster.quoteToErpId);
                model.ContactList.Add(new SelectListItem()
                {
                    Text = "Select...",
                    Value = ""
                });
                for (int i = 0; i <= Contact.Count - 1; i++)
                {
                    var fullname = "";
                    if (AppContext.AppRegion == Region.ACN.ToString())
                        fullname = Contact[i].FirstName + Contact[i].LastName;
                    else
                        fullname = Contact[i].LastName + Contact[i].FirstName;
                    model.ContactList.Add(new SelectListItem()
                    {
                        Text = fullname,
                        Value = Contact[i].ROW_ID,
                        Selected = (Contact[i].ROW_ID == QuotationMaster.salesRowId ? true : false)
                    });
                }
            }


            decimal[] taxRateList = { 0.17m, 0.16m };
            model.TaxRateList = taxRateList.ToList().Select(c => new SelectListItem
            {
                Text = (c * 100).ToString(),
                Value = c.ToString(),
                Selected = (c == QuotationMaster.tax.GetValueOrDefault(0))
            }).ToList();

            //Inside Sales
            model.InsideSalesList = new List<SelectListItem>();
            model.InsideSalesList.Add(new SelectListItem()
            {
                Text = "Select...",
                Value = ""
            });
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select EMAIL from AD_MEMBER_GROUP where GROUP_NAME = '{0}' order by EMAIL ", IS_Group);
            DataTable dt = DBUtil.dbGetDataTable("MY", sql.ToString());

            for (int j = 0; j <= dt.Rows.Count - 1; j++)
            {
                model.InsideSalesList.Add(new SelectListItem()
                {
                    Text = dt.Rows[j][0].ToString(),
                    Value = dt.Rows[j][0].ToString(),
                    Selected = (dt.Rows[j][0].ToString() == QuotationMaster.salesEmail ? true : false)
                });
            }

            //Choose Parent Item
            model.ParentItem = new List<SelectListItem>();
            model.ParentItem.Add(new SelectListItem()
            {
                Text = "Loose items",
                Value = ""
            });
            for (int i = 0; i <= QuotationMaster.QuotationDetail.Count - 1; i++)
            {
                if (QuotationMaster.QuotationDetail[i].partNo.IndexOf("-BTO", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    model.ParentItem.Add(new SelectListItem()
                    {
                        Text = QuotationMaster.QuotationDetail[i].partNo,
                        Value = QuotationMaster.QuotationDetail[i].line_No.ToString()
                    });
                }
            }

            model.CreateDetailTitle = new List<string>();
            model.CreateDetailTitle.Add("No.");
            model.CreateDetailTitle.Add("Part No");
            model.CreateDetailTitle.Add("Description");
            model.CreateDetailTitle.Add("List Price");
            model.CreateDetailTitle.Add("Unit Price");
            model.CreateDetailTitle.Add("Quoting Price");
            model.CreateDetailTitle.Add("Quantity");
            model.CreateDetailTitle.Add("SPR No");
            model.CreateDetailTitle.Add("Sub total");

            model.exp = DescriptionFormat;
            model.QD = getMyQuotationDetail(QuotationMaster);
            model.QuotationPartner = QuotationMaster.QuotationPartner;

            return PartialView(ViewName, model);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [Authorize]
        public ActionResult CreateNewQuotationV2()
        {
            var currentRegion = AppContext.AppRegion;            
            
            var model = new QuoteFormViewModel();
            model.Region = currentRegion;
            var quotePrefix = eQuotationDAL.GetRegionParameterValue(currentRegion, "", "QuotePrefix", "");
            model.QuoteNo = QuoteBusinessLogic.GetEQOrderNumber(quotePrefix, Util.isTesting());
            model.QuoteId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            model.QuoteToRowId = "";
            model.QuoteDate = DateTime.Now;
            int expiredDays = Convert.ToInt32(eQuotationDAL.GetRegionParameterValue(currentRegion, "", "DefaultExpiredDate", "0"));
            model.ExpiredDate = DateTime.Now.AddDays(expiredDays);
            model.CreatedBy = AppContext.UserEmail;
            model.CreatedDate = DateTime.Now;
            model.LastUpdatedBy = AppContext.UserEmail;
            model.LastUpdatedDate = DateTime.Now;
            model.DeliveryDate = DateTime.Now.AddDays(3);
            model.ReqDate = DateTime.Now;
            model.DOCSTATUS = 0;
            model.PoDate = DateTime.Now;
            model.RevisionNumber = 1;
            model.Active = true;
            decimal taxRate = Convert.ToDecimal(eQuotationDAL.GetRegionParameterValue(currentRegion, "", "TaxRate", "0"));
            model.Tax = taxRate;
            model.IsRepeatedOrder = 0;
            model.PoNo = String.Empty;
            model.DistChan = "10";
            model.Division = "00";
            //model.Currency = "USD"; //temp, should be removed later
            //model.Plant = "UBH1"; //temp, should be removed later
            model.Org = DataAccess.SAPDAL.GetDefaultOrgByRBU(currentRegion);

            model.SoldToPartner = new EQPARTNER() { QUOTEID = model.QuoteId, TYPE = "SOLDTO" };
            model.ShipToPartner = new EQPARTNER() { QUOTEID = model.QuoteId, TYPE = "S" }; ;
            model.BillToPartner = new EQPARTNER() { QUOTEID = model.QuoteId, TYPE = "B" }; ;
            model.EndCustomer = new EQPARTNER() { QUOTEID = model.QuoteId, TYPE = "EM" }; ;
            model.QuoteTitle = QuoteBusinessLogic.GetQuoteTitleOptions(AppContext.AppRegion).FirstOrDefault();

            //Payment Term Option
            model.PaymentTermOptions.Add(new SelectListItem()
            {
                Text = "Select...",
                Value = ""
            });
            var paymentTerms = MyAdvantechDAL.GetPaymentTermByOrg(model.Org);
            if (paymentTerms.Any())
            {
                model.PaymentTermOptions = paymentTerms
                              .Select(x => new SelectListItem() { Text = x.Key, Value = x.Value })
                              .ToList();
            }


            //Currency下拉選單設定
            var defaultCurrencyOptions = QuoteBusinessLogic.GetCurrencyOptions(currentRegion);
            if (defaultCurrencyOptions.Any())
            {
                model.CurrencyOptions = defaultCurrencyOptions
                              .Select(x => new SelectListItem() { Text = x, Value = x })
                              .ToList();
            }

            var extraTermsAndConditionList = QuoteBusinessLogic.GetDefaultTermsAndConditions(currentRegion);
            foreach (var item in extraTermsAndConditionList)
            {
                model.ExtraTermsAndConditionOptions.Add(new ExtraTermsAndConditions() { Name = item, Type = "Extra", Selected = false });
            }


            //Inside Sales options
            model.InsideSalesOptions.Add(new SelectListItem()
            {
                Text = "Select...",
                Value = ""
            });
            var IS_Group = eQuotationDAL.GetRegionParameterValue(currentRegion, "", "ISGroup", "");
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select EMAIL from AD_MEMBER_GROUP where GROUP_NAME = '{0}' order by EMAIL ", IS_Group);
            DataTable dtIS = DBUtil.dbGetDataTable("MY", sql.ToString());
            for (int j = 0; j <= dtIS.Rows.Count - 1; j++)
            {
                model.InsideSalesOptions.Add(new SelectListItem()
                {
                    Text = dtIS.Rows[j][0].ToString(),
                    Value = dtIS.Rows[j][0].ToString()
                });
            }

            // Tax Rate options
            var defaultTaxRateOptions = QuoteBusinessLogic.GetTaxRateOptions(currentRegion);
            model.TaxRateOptions = defaultTaxRateOptions.ToList().Select(c => new SelectListItem
            {
                Text = (c * 100).ToString(),
                Value = c.ToString(),
                Selected = (c == model.Tax)
            }).ToList();

            //Extend warranty options    
            var ExWarrantyList = MyAdvantechDAL.GetExtendedWarrantyByOrg(model.Org); 
            foreach (var ewItem in ExWarrantyList)
            {
                foreach (var quoteItem in model.QuoteItems)
                {
                    quoteItem.EWPartOptions.Add(new SelectListItem()
                    {
                        Text = ewItem.EW_PartNO,
                        Value = ewItem.ID.ToString()
                    });
                }
            }


            //GET default warranty
            model.Warranty = QuoteBusinessLogic.GetDefaultWarranty(currentRegion);

            ///暫時把View寫入RegionParameter, 需要再研究///
            var viewName = eQuotationDAL.GetRegionParameterValue(currentRegion, "", "QuotationForm_ViewName", "QuoteForm");

            return PartialView(viewName, model);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [Authorize]
        public ActionResult EditQuotationV2(string quoteId, string copyType)
        {
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first

            var currentRegion = AppContext.AppRegion; 
            var viewName = eQuotationDAL.GetRegionParameterValue(currentRegion, "", "QuotationForm_ViewName", "QuoteForm");

            var model = new QuoteFormViewModel();
            var qm = QuoteBusinessLogic.GetQuotationMaster(quoteId);
            if (copyType != null)
            {
                if (copyType.Equals("Revise", StringComparison.OrdinalIgnoreCase))
                {
                    String originalQuoteNo = qm.quoteNo;
                    int RevisionNO = (QuotationMasterHelper.GetQuotationMastersByQuoteNo(originalQuoteNo).FirstOrDefault() == null) ? 1 : (int)QuotationMasterHelper.GetQuotationMastersByQuoteNo(originalQuoteNo).OrderByDescending(d => d.Revision_Number).FirstOrDefault().Revision_Number + 1;

                    //qm = QuoteBusinessLogic.CopyQuotation(quoteId, Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15), originalQuoteNo, AppContext.UserEmail, qm.tax.GetValueOrDefault(0), RevisionNO);
                    qm = QuoteBusinessLogic.ReviseQuotation(quoteId, Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15), AppContext.UserEmail, RevisionNO);
                }
                else
                {
                    var quotePrefix = eQuotationDAL.GetRegionParameterValue(currentRegion, "", "QuotePrefix", "");
                    qm = QuoteBusinessLogic.CopyQuotation(quoteId, Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15), QuoteBusinessLogic.GetEQOrderNumber(quotePrefix, Util.isTesting()), AppContext.UserEmail, qm.tax.GetValueOrDefault(0));

                    //simulate price again //Alex : only sumulate price again when copying quotation.
                    string errMsg = "";
                    qm.SimulateListUnitPriceVPRS(ref errMsg);
                    if (!string.IsNullOrEmpty(errMsg)) // if error, clear all items, and show message to user 
                    {
                        qm.QuotationDetail.Clear();
                        model.ErrorMessage = "Clear all items because: " + errMsg;
                    }
                }
            }

            //Remove unorderable items          
            qm.RemoveUnorderableItems();

            model.Region = currentRegion;           
            model.QuoteNo = qm.quoteNo;
            model.QuoteId = qm.quoteId;
            model.QuoteToErpId = qm.quoteToErpId;
            model.QuoteDate = TimeHelper.ConvertToLocalTime(qm.quoteDate.GetValueOrDefault(DateTime.Now), currentRegion);
            model.ExpiredDate = TimeHelper.ConvertToLocalTime(qm.expiredDate.GetValueOrDefault(DateTime.Now.AddMonths(1)), AppContext.AppRegion);
            model.PrintDate = TimeHelper.ConvertToLocalTime(DateTime.Now, currentRegion);
            model.RevisionNumber = qm.Revision_Number.GetValueOrDefault(1);
            model.QuoteToName = qm.quoteToName;
            model.Tax = qm.tax.GetValueOrDefault(0);
            model.Freight = qm.freight.GetValueOrDefault(0);
            model.CreatedBy = qm.createdBy;
            model.CreatedDate = qm.createdDate.GetValueOrDefault(DateTime.Now);
            model.LastUpdatedBy = qm.LastUpdatedBy;
            model.LastUpdatedDate = qm.LastUpdatedDate.GetValueOrDefault(DateTime.Now);
            model.DeliveryDate = qm.deliveryDate.GetValueOrDefault(DateTime.Now); 
            model.ReqDate = qm.reqDate.GetValueOrDefault(DateTime.Now.AddDays(3));
            model.DOCSTATUS = qm.DOCSTATUS.GetValueOrDefault(0);
            model.PoDate = qm.PODate.GetValueOrDefault(DateTime.Now);
            model.Active = qm.Active.Value;
            model.IsRepeatedOrder = qm.isRepeatedOrder.Value;
            model.PoNo = String.IsNullOrEmpty(qm.PO_NO) ? string.Empty : qm.PO_NO;
            model.DistChan = qm.DIST_CHAN;
            model.Division = qm.DIVISION;
            model.SalesGroup = qm.SALESGROUP;
            model.SalesOffice = qm.SALESOFFICE;
            model.Currency = qm.currency; //temp, should be removed later
            //model.CurrencySign = Advantech.Myadvantech.DataAccess.MyExtension.GetCurrencySignByCurrency(qm.currency);
            model.Plant = qm.Plant; //temp, should be removed later
            model.Description = qm.customId;
            model.InSideSales = qm.salesEmail;
            model.ApprovalReason = qm.relatedInfo;
            model.OptyName = qm.QuotationOpty == null ? "" : qm.QuotationOpty.optyName;
            model.OptyStage = qm.QuotationOpty == null ? "" : qm.QuotationOpty.optyStage;
            //model.QuoteApprovals.QuoteApprovalList = qm.WaitingApprovals; //edit 時還不會有approval
            model.QuoteToRowId = qm.quoteToRowId;
            model.SiebelRBU = qm.siebelRBU;
            model.Org = qm.org;
            model.QuoteNote = qm.quoteNote;
            //需簽核理由註解用，只適用ACN
            model.CommentExp = model.Region == "ACN" ? "*销售必填项：最终客户名称，最终客户价格，MOQ，项目名称，项目预计总数量，项目持续时间，今年预计出货数量，DI任务号" : "";
            model.OptyName = qm.QuotationOpty == null ? "" : qm.QuotationOpty.optyName;
            model.OptyStage = qm.QuotationOpty == null ? "" : qm.QuotationOpty.optyStage;
            model.OptyId = qm.QuotationOpty == null ? "" : qm.QuotationOpty.optyId;
            model.QuoteTitle = QuoteBusinessLogic.GetQuoteTitleOptions(AppContext.AppRegion).FirstOrDefault();
            model.Inco1 = qm.INCO1;
            model.Inco2 = qm.INCO2;
            model.SalesRowId = qm.salesRowId;
            model.OriginalQuoteID = qm.OriginalQuoteID;
            model.Remark = qm.Remark;

            if (qm.QuotationExtensionNew != null)
            {
                //New expired date is for ACN PSM adjust expired date function
                if (qm.QuotationExtensionNew.NewExpiredDate != null)
                    model.ACNMaxNewExpiredDate = TimeHelper.ConvertToLocalTime(qm.QuotationExtensionNew.NewExpiredDate.Value, AppContext.AppRegion);
                else
                    model.ACNMaxNewExpiredDate = qm.quoteDate.GetValueOrDefault(DateTime.Now).AddDays(180);


                if (!string.IsNullOrEmpty(qm.QuotationExtensionNew.Warranty))
                {
                    model.Warranty = qm.QuotationExtensionNew.Warranty;
                }
                else
                {
                    model.Warranty = QuoteBusinessLogic.GetDefaultWarranty(AppContext.AppRegion);
                }
            }




            //ORG下拉選單設定
            var sapCompanys = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyByID(qm.quoteToErpId);

            if (sapCompanys != null && sapCompanys.Count > 0)
            {
                for (int i = 0; i <= sapCompanys.Count - 1; i++)
                {
                    model.OrgOptions.Add(new SelectListItem()
                    {
                        Text = sapCompanys[i].ORG_ID,
                        Value = sapCompanys[i].ORG_ID,
                        Selected = (sapCompanys[i].ORG_ID == qm.org ? true : false)
                    });
                }
            }
            else
            {
                // Add default org for no ERPID case.
                model.OrgOptions.Add(new SelectListItem()
                {
                    Text = DataAccess.SAPDAL.GetDefaultOrgByRBU(AppContext.AppRegion),
                    Value = DataAccess.SAPDAL.GetDefaultOrgByRBU(AppContext.AppRegion),
                    Selected = true
                });
            }


            //Currency下拉選單設定
            var defaultCurrencyOptions = QuoteBusinessLogic.GetCurrencyOptions(currentRegion);
            if (defaultCurrencyOptions.Any())
            {
                model.CurrencyOptions = defaultCurrencyOptions
                              .Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == qm.currency })
                              .ToList();
            }
            else
                model.CurrencyOptions.Add(new SelectListItem() { Text = qm.currency, Value = qm.currency, Selected = true });


            //Payment Term Option
            model.PaymentTermOptions.Add(new SelectListItem()
            {
                Text = "Select...",
                Value = ""
            });
            var paymentTerms = MyAdvantechDAL.GetPaymentTermByOrg(model.Org);
            if (paymentTerms.Any())
            {
                model.PaymentTermOptions = paymentTerms
                              .Select(x => new SelectListItem() { Text = x.Key, Value = x.Value, Selected = x.Value == qm.paymentTerm })
                              .ToList();
            }


            // Tax Rate options
            var defaultTaxRateOptions = QuoteBusinessLogic.GetTaxRateOptions(currentRegion);
            model.TaxRateOptions = defaultTaxRateOptions.ToList().Select(c => new SelectListItem
            {
                Text = (c * 100).ToString(),
                Value = c.ToString(),
                Selected = (c == model.Tax)
            }).ToList();


            //Customer Contact
            var Contact = MyAdvantechDAL.GetSiebelContactByAccountRowId(qm.quoteToRowId); 
            model.ContactOptions.Add(new SelectListItem()
            {
                Text = "Select...",
                Value = ""
            });
            for (int i = 0; i <= Contact.Count - 1; i++)
            {
                var fullname = "";
                if (currentRegion == Region.ACN.ToString())
                    fullname = Contact[i].FirstName + Contact[i].LastName;
                else
                    fullname = Contact[i].LastName + Contact[i].FirstName;
                model.ContactOptions.Add(new SelectListItem()
                {
                    Text = fullname,
                    Value = Contact[i].ROW_ID
                    //Value = Contact[i].ROW_ID,
                    //Selected = (Contact[i].ROW_ID == qm.salesRowId ? true : false)
                });
            }

            //抓出負責業務相關資料
            var EPs = qm.QuotationPartner.Where(e => e.TYPE == "E").ToList();
            if (EPs.Count > 0)
            {
                foreach (var ep in EPs)
                {
                    var sales = new SalesRepresentative();
                    sales.SalesCode = ep.ERPID;
                    sales.Email = ep.NAME;
                    model.SalesRepresentatives.Add(sales);
                }
            }
            var soldToPartner = qm.QuotationPartner.Where(e => e.TYPE == "SOLDTO").FirstOrDefault();
            var shipToPartner = qm.QuotationPartner.Where(e => e.TYPE == "S").FirstOrDefault();
            var billToPartner = qm.QuotationPartner.Where(e => e.TYPE == "B").FirstOrDefault();
            var endCustomer = qm.QuotationPartner.Where(e => e.TYPE == "EM").FirstOrDefault();

            model.SoldToPartner = soldToPartner !=null? soldToPartner: new EQPARTNER() { QUOTEID = model.QuoteId, TYPE = "SOLDTO" };
            model.ShipToPartner = shipToPartner != null ? shipToPartner : new EQPARTNER() { QUOTEID = model.QuoteId, TYPE = "S" }; 
            model.BillToPartner = billToPartner != null ? billToPartner : new EQPARTNER() { QUOTEID = model.QuoteId, TYPE = "B" }; 
            model.EndCustomer = endCustomer != null ? endCustomer : new EQPARTNER() { QUOTEID = model.QuoteId, TYPE = "EM" };


            if (soldToPartner != null)
            {
                model.Address = soldToPartner.ADDRESS;
            }
            else { 
                SIEBEL_ACCOUNT _SiebelAccount = Advantech.Myadvantech.DataAccess.SiebelDAL.getSiebelAccount(qm.quoteToRowId);
                if (_SiebelAccount != null)
                {
                    model.Address = _SiebelAccount.ADDRESS;
                }
            }


            //Extra Terms And Conditions options// need move to regionparameter
            var termsTypes = qm.QuotationPartner.Where(e => e.TYPE.Contains("Extra")).Select(r=>r.TYPE).ToList();
            var extraTermsAndConditionList = QuoteBusinessLogic.GetDefaultTermsAndConditions(currentRegion);
            foreach (var item in extraTermsAndConditionList)
            {
                if (termsTypes.Contains(item))
                    model.ExtraTermsAndConditionOptions.Add(new ExtraTermsAndConditions() { Name = item, Type = "Extra", Selected = true });
                else
                    model.ExtraTermsAndConditionOptions.Add(new ExtraTermsAndConditions() { Name = item, Type = "Extra", Selected = false });

            }


            //Inside Sales options
            model.InsideSalesOptions.Add(new SelectListItem()
            {
                Text = "Select...",
                Value = ""
            });
            var IS_Group = eQuotationDAL.GetRegionParameterValue(currentRegion, "", "ISGroup", "");
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select EMAIL from AD_MEMBER_GROUP where GROUP_NAME = '{0}' order by EMAIL ", IS_Group);
            DataTable dtIS = DBUtil.dbGetDataTable("MY", sql.ToString());
            for (int j = 0; j <= dtIS.Rows.Count - 1; j++)
            {
                model.InsideSalesOptions.Add(new SelectListItem()
                {
                    Text = dtIS.Rows[j][0].ToString(),
                    Value = dtIS.Rows[j][0].ToString(),
                    Selected = (dtIS.Rows[j][0].ToString() == qm.salesEmail ? true : false)
                });
            }

            // Alex20180806 put set quote items to last line.
            model.SetQuoteItems(qm.QuotationDetail);

            return PartialView(viewName, model);
        }

        [HttpPost]
        public ActionResult AddItemToQuote(QuoteFormViewModel quoteFormViewModel, string addPartsJson, string parentJson, bool isReconfig)
        {
            string errMsg = "";
            var result = true;
            string data = "";

            var currentQuote = quoteFormViewModel.ConverToQuote();
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
            List<QuotationDetail> originalQuotCartItems = currentQuote.QuotationDetail.ToList();
            try
            {
                if (addPartsJson != null)
                {

                    List<Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model.JsonPart> parts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model.JsonPart>>(addPartsJson);

                    
                    foreach (var partNo in parts.Select(p => p.Partno))
                    {
                        if(QuoteBusinessLogic.IsBtoPart(partNo, currentQuote.org))
                        { 
   
                            if(AppContext.AppRegion == "ACN")  //ACN,如加入BTO，要先清除QuotationDetail, 以防組裝品與單品混搭的狀況
                                currentQuote.QuotationDetail.Clear();
                            break;
                        }
                    }


                    currentQuote.AddQuotationDetail(parts, parentJson, isReconfig);
                   
                    currentQuote.SimulateListUnitPriceVPRS(ref errMsg);

                    //20180806 Alex: Calculate extend earranty price
                    //currentQuote.CalculateExtendWarrantyPrice();

                    if (!string.IsNullOrEmpty(errMsg))// if error occur, recover to original quoteDetails
                    {
                        currentQuote.QuotationDetail = originalQuotCartItems;
                        result = false;
                    }
                    else
                    {
                        quoteFormViewModel.SetQuoteItems(currentQuote.QuotationDetail);// convert quote details to quoteitems
                        string viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteCartPartial_ViewName", "");
                        data = this.RenderRazorViewToString(viewName, quoteFormViewModel);
                    }
                }

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                result = false;
            }



            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateExtendWarrantyItem(QuoteFormViewModel quoteFormViewModel, string EWPartsJson, int lineNoWithEx)
        {
            string errMsg = "";
            var result = true;
            string data = "";

            var currentQuote = quoteFormViewModel.ConverToQuote();
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
            List<QuotationDetail> originalQuotCartItems = currentQuote.QuotationDetail.ToList();
            try
            {

                if (EWPartsJson != null)
                {

                    Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model.JsonEWPart EWparts = Newtonsoft.Json.JsonConvert.DeserializeObject<Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model.JsonEWPart>(EWPartsJson);

                    currentQuote.UpdateExtendWarranty(EWparts, lineNoWithEx, ref errMsg);

                    currentQuote.CalculateExtendWarrantyPriceByLineNo(lineNoWithEx);


                    if (!string.IsNullOrEmpty(errMsg))// if error occur, recover to original quoteDetails
                    {
                        currentQuote.QuotationDetail = originalQuotCartItems;
                        result = false;
                    }
                    else
                    {
                        quoteFormViewModel.SetQuoteItems(currentQuote.QuotationDetail);// convert quote details to quoteitems
                        string viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteCartPartial_ViewName", "");
                        data = this.RenderRazorViewToString(viewName, quoteFormViewModel);
                    }
                }
                

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                result = false;
            }



            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Save(string[] SalesCode, string[] SalesName, string InsideSales, string Description, string ContactRowID, string AccountName, decimal Freight, string Warranty, string Comment, string QuotationNote, string ExpiredDate, string PONO)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            try
            {

                if (Quote != null)
                {
                    if (string.IsNullOrEmpty(Quote.org))
                    {
                        if (AppContext.AppRegion == "ABB")
                            Quote.org = "US10";
                        else if (AppContext.AppRegion == "ACN")
                            Quote.org = "CN10";
                        else
                            Quote.org = "";
                    }
                    Quote.DOCSTATUS = 0;
                    setValue(SalesCode, SalesName, InsideSales, Description, ContactRowID, AccountName, Freight, Warranty, Comment, QuotationNote, ExpiredDate, PONO);
                    Quote.SaveAllChanges();
                }
                else
                {
                    result = false;
                    Err = "The Quote is empty!";
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, ID = "", err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Confirm(string[] SalesCode, string[] SalesName, string InsideSales, string Description, string ContactRowID, string AccountName, decimal Freight, string Warranty, string Comment, string QuotationNote, string ExpiredDate, string PONO)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            try
            {
                if (Quote != null)
                {

                    //若需簽核 啟動簽核程序 根據簽核者發送mail
                    if (Quote.WaitingApprovals.Any())
                    {

                        //if ACN ISG, at least select one PSM
                        if (AppContext.AppRegion == "ACN")
                        {
                            var ACNSector = UserRoleBusinessLogic.getSectorBySalesEmailSalesCode(SalesName[0], SalesCode[0], AppContext.AppRegion);
                            if (ACNSector == "ISG" || ACNSector == "IAG" ||(ACNSector == "EIOT" && !QuoteBusinessLogic.IsACNLoosItemQuote(Quote.QuotationDetail)))
                            {
                                if (Quote.WaitingApprovals.Where(a => a.ApproverType == ApproverType.PSM.ToString() || a.ApproverType == ApproverType.OptionalPSM.ToString()).ToList().Count == 0)
                                    return Json(new { succeed = false, ID = "", err = "Please select at least one PSM!" }, JsonRequestBehavior.AllowGet);
                            }
                            
                        }
                            
                        Quote.DOCSTATUS = 0;
                    }                        
                    else
                    {
                        Quote.DOCSTATUS = 1;
                        Quote.QuotationExtensionNew.FinishDate = DateTime.Now;
                    }
                    setValue(SalesCode, SalesName, InsideSales, Description, ContactRowID, AccountName, Freight, Warranty, Comment, QuotationNote, ExpiredDate, PONO);

                    if (Quote.QuotationOpty != null)
                    {
                        //新增Oppty的執行寫在這裡
                        if (Quote.QuotationOpty.optyId.Equals("new ID"))
                        {
                            // Add siebel active create opportunity record
                            SiebelActive SA = new SiebelActive();
                            SA.ActiveSource = "eQuotation";
                            SA.ActiveType = "CreateOpportunity";
                            SA.Status = "UnProcess";
                            SA.QuoteID = Quote.quoteId ?? "";
                            SA.OptyName = Quote.QuotationOpty.optyName ?? "";
                            SA.OptyStage = Quote.QuotationOpty.optyStage ?? "";
                            SA.Amount = Quote.PostTaxTotalAmount;
                            if (!string.IsNullOrEmpty(SalesName[0]))
                            {
                                SA.OptyOwnerEmail = SalesName[0];
                            }
                            else if (!string.IsNullOrEmpty(Quote.salesEmail))
                            {
                                SA.OptyOwnerEmail = Quote.salesEmail;
                            }
                            else
                            {
                                SA.OptyOwnerEmail = AppContext.UserEmail ?? "";
                            }
                            SA.CreateBy = AppContext.UserEmail ?? "";
                            SA.CreatedDate = DateTime.Now;
                            SA.LastUpdatedBy = AppContext.UserEmail ?? "";
                            SA.LastUpdatedDate = DateTime.Now;
                            Quote.SiebelActive.Add(SA);
                        }
                    }
                    Quote.SaveAllChanges();

                    if (Quote.DOCSTATUS == 0)
                    {
                        //更新 approvals 物件的 mail body(因需要ReviewQuoteApproval生成郵件內容)
                        QuotesViewModel quoteVModel = setQuoteViewModel(Quote);
                        quoteVModel.pagetype = "QuoteApprovalMail";

                        //updateMailBodyForApprovers(Quote.WaitingApprovals, quoteVModel);  //會已context問題
                        //updateFinalMailBodyForApprovers(Quote.WaitingApprovals, quoteVModel); //會已context問題

                        var finalMailBody = RenderRazorViewToString(ReviewQuoteApprovalView(AppContext.AppRegion), quoteVModel);// 此final mail body不會有GP
                        foreach (var qa in Quote.WaitingApprovals)
                        {
                            quoteVModel.Approval = qa;

                            ViewBag.approval = qa;

                            qa.Mailbody = RenderRazorViewToString(ReviewQuoteApprovalView(AppContext.AppRegion), quoteVModel);
                            qa.FinalMailBody = finalMailBody;
                        }


                        //建立approvals 並啟動發送簽核信件的流程
                        var url = Request.Url.Scheme + "://" + Request.Url.Authority;
                        QuoteApprovalDog.StartFlow(Quote.quoteId, url, Quote.WaitingApprovals, AppContext.AppRegion);

                        if (QuoteApprovalDog.ApprovalResult != WorkFlowlAPI.ApprovalResult.WaitingForApproval && QuoteApprovalDog.ApprovalResult != WorkFlowlAPI.ApprovalResult.Finish)
                        {
                            result = false;
                            Err = QuoteApprovalDog.ApprovalResult.ToString();
                        }
                    }
                }
                else
                {
                    result = false;
                    Err = "The Quote is empty!";
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, ID = "", err = Err }, JsonRequestBehavior.AllowGet);
        }




        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CheckBeforeQuoteApproval(string quoteID, string[] SalesCode, string[] SalesName, string InsideSales, string Description, string ContactRowID, string AccountName, decimal Freight, string Warranty, string Comment, string QuotationNote, string ExpiredDate, string PONO)
        {
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            setValue(SalesCode, SalesName, InsideSales, Description, ContactRowID, AccountName, Freight, Warranty, Comment, QuotationNote, ExpiredDate, PONO);


            //傳入QutoeMaster啟動找尋approver的流程 並返回帶有紀錄GP的QuoteMaster
            var url = Request.Url.Scheme + "://" + Request.Url.Authority;

            QuoteApproverFinder.StartFlow(Quote, url, AppContext.AppRegion);
            Quote.WaitingApprovals = QuoteApproverFinder.WaitingApprovals;

            if (QuoteApproverFinder.FindApproverResult == WorkFlowlAPI.FindApproverResult.NoNeed)
                return Json(new { succeed = true, ID = "", err = "" }, JsonRequestBehavior.AllowGet);

            //========Region Variables Settings========            
            String ViewName = String.Empty;

            if (AppContext.AppRegion == "ACN")
            {
                ViewName = "CheckBeforeQuoteApproval";
            }
            else if (AppContext.AppRegion == "ABB")
            {
                ViewName = "ABB_CheckBeforeQuoteApproval";
            }
            //========End Region Variables Settings========

            QuotesViewModel quoteVModel = setQuoteViewModel(Quote);
            quoteVModel.FindApproverResult = QuoteApproverFinder.FindApproverResult;
            quoteVModel.pagetype = "CheckBeforeApproval";
            return PartialView(ViewName, quoteVModel);
        }

        void updateMailBodyForApprovers(string appRegion,IEnumerable<WorkFlowApproval> QAlist, QuotesViewModel QVModel)
        {
            QVModel.pagetype = "QuoteApprovalMail";
            foreach (var qa in QAlist)
            {
                QVModel.Approval = qa;              
                ViewBag.approval = qa;

                qa.Mailbody = RenderRazorViewToString(ReviewQuoteApprovalView(appRegion), QVModel);// 此 mail body根據view GP欄位來決定是否顯示GP
                qa.Update();
            }
        }

        void updateFinalMailBodyForApprovers(string appRegion, IEnumerable<WorkFlowApproval> QAlist, QuotesViewModel QVModel)
        {
            QVModel.pagetype = "QuoteApprovalMail";
            var finalMailBody = RenderRazorViewToString(ReviewQuoteApprovalView(appRegion), QVModel);// 此final mail body不會有GP
            foreach (var qa in QAlist)
            {
                qa.FinalMailBody = finalMailBody;
                qa.Update();
            }
        }


        public void setValue(string[] SalesID, string[] SalesName, string InsideSales, string Description, string ContactRowID, string AccountName, decimal Freight, string Warranty, string Comment, string QuotationNote, string ExpiredDate, string PONO)
        {
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            if (Quote != null)
            {
                if (SalesID != null)
                {
                    for (var i = 0; i < SalesID.Length; i++)
                    {
                        if (i == 0)
                        {
                            Quote.QuotationPartner.RemoveAll(x => x.TYPE == "E");
                            EQPARTNER partner_sales = new EQPARTNER();
                            partner_sales.QUOTEID = Quote.quoteId;
                            partner_sales.ERPID = SalesID[i];
                            partner_sales.NAME = SalesName[i];
                            partner_sales.TYPE = "E";
                            Quote.QuotationPartner.Add(partner_sales);
                        }
                        else if (i == 1)
                        {
                            Quote.QuotationPartner.RemoveAll(x => x.TYPE == "E2");
                            EQPARTNER partner_sales = new EQPARTNER();
                            partner_sales.QUOTEID = Quote.quoteId;
                            partner_sales.ERPID = SalesID[i];
                            partner_sales.NAME = SalesName[i];
                            partner_sales.TYPE = "E2";
                            Quote.QuotationPartner.Add(partner_sales);
                        }
                        else if (i == 2)
                        {
                            Quote.QuotationPartner.RemoveAll(x => x.TYPE == "E3");
                            EQPARTNER partner_sales = new EQPARTNER();
                            partner_sales.QUOTEID = Quote.quoteId;
                            partner_sales.ERPID = SalesID[i];
                            partner_sales.NAME = SalesName[i];
                            partner_sales.TYPE = "E3";
                            Quote.QuotationPartner.Add(partner_sales);
                        }
                    }
                }
            }

            if (InsideSales != null)
            {
                if (!string.IsNullOrEmpty(InsideSales))
                {
                    Quote.salesEmail = InsideSales.Trim().Replace("'", "''");
                }
                else
                {
                    Quote.salesEmail = "";
                }
            }


            if (!string.IsNullOrEmpty(Description))
            {
                Quote.customId = Description.Trim().Replace("'", "''");
            }
            else
            {
                Quote.customId = "";
            }

            if (!string.IsNullOrEmpty(ContactRowID))
            {
                Quote.salesRowId = ContactRowID.Trim().Replace("'", "''");
            }
            else
            {
                Quote.salesRowId = "";
            }

            if (!string.IsNullOrWhiteSpace(AccountName))
            {
                Quote.quoteToName = AccountName.Trim().Replace("'", "''");
            }
            else
            {
                Quote.quoteToName = "";
            }

            if (!string.IsNullOrWhiteSpace(Comment))
            {
                Quote.relatedInfo = Comment.Trim().Replace("'", "''");
            }
            else
            {
                Quote.relatedInfo = "";
            }

            if (!string.IsNullOrWhiteSpace(QuotationNote))
            {
                Quote.quoteNote = QuotationNote.Trim().Replace("'", "''");
            }
            else
            {
                Quote.quoteNote = "";
            }

            Quote.freight = Freight;

            if (!string.IsNullOrWhiteSpace(Warranty))
            {
                Quote.QuotationExtensionNew.Warranty = Warranty.Trim().Replace("'", "''");
            }
            else
            {
                Quote.QuotationExtensionNew.Warranty = "";
            }

            DateTime expDate = new DateTime();
            if (!string.IsNullOrEmpty(ExpiredDate) && DateTime.TryParse(ExpiredDate, out expDate))
            {
                Quote.expiredDate = TimeHelper.ConvertToSystemTime(expDate, AppContext.AppRegion);
            }

            if (!string.IsNullOrWhiteSpace(PONO))
            {
                Quote.PO_NO = PONO.Trim().Replace("'", "''");
            }
            else
            {
                Quote.PO_NO = "";
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult AddProductItem(string _parts, string _parent)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            var myQD = new List<MyQuotationDetail>();

            List<Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model.JsonPart> Parts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model.JsonPart>>(_parts);

            try
            {
                if (Parts != null && Parts.Count > 0)
                {
                    List<QuotationDetail> originalQuotDetailList = Quote.QuotationDetail.ToList();

                    //ACN 如加入BTO，要先清除QuotationDetail, 以防組裝品與單品混搭的狀況
                    if (AppContext.AppRegion == "ACN" )
                    {
                        foreach (var partNo in Parts.Select(p => p.Partno))
                        {
                            //SAP_PRODUCT p = MyAdvantechDAL.GetSAP_ProductByOrg(partNo, Quote.org);
                            //if (p!=null && (partNo.EndsWith("-BTO", StringComparison.OrdinalIgnoreCase) || p.MATERIAL_GROUP.Equals("BTOS", StringComparison.InvariantCultureIgnoreCase)))
                            if (QuoteBusinessLogic.IsBtoPart(partNo, Quote.org))
                            {
                                Quote.QuotationDetail.Clear();
                                break;
                            }
                                
                        }
                    }
                    Quote.AddQuotationDetail(Parts, _parent, false);

                    
                    //Quote.SimulatePrice(ref Err);
                    QuoteSimulatePrice.StartFlow(Quote, ref Err);// 20180423 Move simulate price and ITP to workflow
                    if (!string.IsNullOrEmpty(Err))// if error occur, recover to original quoteDetails
                    {
                        Quote.QuotationDetail = originalQuotDetailList.ToList();
                        result = false;
                    }
                }

                myQD = getMyQuotationDetail(Quote);
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = myQD, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteProductItem(int ID)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            var myQD = new List<MyQuotationDetail>();
            try
            {
                if (ID % 100 == 0) //沒有餘數為BTO
                {
                    Quote.QuotationDetail.RemoveAll(o => o.HigherLevel == ID);//刪除子項
                    Quote.QuotationDetail.RemoveAll(y => y.line_No == ID);//刪除BTO母階
                }
                else
                {
                    Quote.QuotationDetail.RemoveAll(x => x.line_No == ID);
                }

                //Quote.UpdateBTOSParentPriceProperties();
                Quote.ReorderQuotationDetail(); // 刪除item後,重新定義lineNo，讓lineNo連續
                myQD = getMyQuotationDetail(Quote);
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = myQD, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteQuoteItemV2(QuoteFormViewModel quoteFormViewModel, int lineNo)
        {
            var result = true;
            string errMsg = "";
            string data = "";
            var currentQuote = quoteFormViewModel.ConverToQuote();
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
            try
            {
                var quoteItem = currentQuote.QuotationDetail.FirstOrDefault(x => x.line_No == lineNo);

                if (quoteItem != null)
                {
                    var itemType = quoteItem.ItemType;
                    if (quoteItem.line_No % 100 == 0) //沒有餘數為BTO
                    {
                        currentQuote.QuotationDetail.RemoveAll(o => o.HigherLevel == lineNo);//刪除子項
                        currentQuote.QuotationDetail.RemoveAll(y => y.line_No == lineNo);//刪除BTO母階
                    }
                    else
                    {
                        if (quoteItem != null && quoteItem.ewFlag != null && quoteItem.ewFlag != 0)
                            currentQuote.RemoveExtendWarranty(quoteItem);


                        currentQuote.QuotationDetail.RemoveAll(x => x.line_No == lineNo);
                    }
                    currentQuote.ReorderQuotationDetail(); // 刪除item後,重新定義lineNo，讓lineNo連續


                    if (itemType == (int)LineItemType.BTOSChild)
                        currentQuote.CalculateExtendWarrantyPriceByLineNo(lineNo); // re calculate EW Part again
                }

                quoteFormViewModel.SetQuoteItems(currentQuote.QuotationDetail);// convert quote details to quoteitems
                string viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteCartPartial_ViewName", "");

                data = this.RenderRazorViewToString(viewName, quoteFormViewModel);
               
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                result = false;
            }

            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult updateProductItem(int ID, string Description, decimal Quoting_Price, int Quantity, string isNCNR)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            var myQD = new List<MyQuotationDetail>();
            try
            {
                Quote.UpdateQuotationDetail(ID, Description, Quoting_Price, Quantity, ref Err, Convert.ToBoolean(isNCNR));
                if (!string.IsNullOrEmpty(Err))
                    result = false;
                myQD = getMyQuotationDetail(Quote);
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = myQD, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateQuoteItemV2(QuoteFormViewModel quoteFormViewModel, int lineNo)
        {
            //,  decimal quotingPrice, int qty, string isNCNR, string description

            var result = true;
            string errMsg = "";
            string data = "";
            var currentQuote = quoteFormViewModel.ConverToQuote();
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
            var item = quoteFormViewModel.QuoteItems.Where(q => q.LineNo == lineNo).FirstOrDefault();
            try
            {
                currentQuote.UpdateQuotationDetailV2(lineNo, item.Description, item.QuotingPrice, item.CurrentQty, item.Qty, ref errMsg, item.NCNR);

                if(item.EWFlag !=0)// update extended warranty parts too for qty change
                {
                    Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model.JsonEWPart  ewPart = new Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model.JsonEWPart();
                    ewPart.Partno = MyAdvantechDAL.GetExtendedWarrantyById(item.EWFlag).EW_PartNO;
                    ewPart.Qty = item.Qty;
                    ewPart.EWId = item.EWFlag;
                    currentQuote.UpdateExtendWarranty(ewPart, lineNo, ref errMsg);
                }

                if(!item.IsEWPart)
                    currentQuote.CalculateExtendWarrantyPriceByLineNo(item.LineNo);


                if (!string.IsNullOrEmpty(errMsg))
                    result = false;

                quoteFormViewModel.SetQuoteItems(currentQuote.QuotationDetail);// convert quote details to quoteitems
                string viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteCartPartial_ViewName", "");

                data = this.RenderRazorViewToString(viewName, quoteFormViewModel);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                result = false;
            }

            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MQ02001", "It allows user to update General Rate for ITP.", "Module Quote", "MQ00000")]

        public ActionResult updateGeneralRate(decimal newRate, string approveID)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            Quote.QuotationExtensionNew.GeneralRate = newRate;
            //var myQD = new List<MyQuotationDetail>();
            try
            {
                Quote.InitializeQuotationDetail();
                //myQD = getMyQuotationDetail(Quote);
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            //Prepare new quotaionDetail after tax adjustment
            WorkFlowApproval approval = eQuotationContext.Current.WorkFlowApproval.Where(a => a.UID == approveID).FirstOrDefault();
            QuotesViewModel QVModel = new QuotesViewModel();
            QVModel = setQuoteViewModel(Quote);
            QVModel.pagetype = "ReviewQuoteApproval";
            QVModel.Approval = approval;

            return PartialView("_QuoteDetails", QVModel);
            //return Json(new { succeed = result, data = myQD, err = Err }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateTax(decimal newRate)
        {
            var result = true;
            string Err = "";
            List<MyQuotationDetail> myQD = new List<MyQuotationDetail>();
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            Quote.tax = newRate;
            try
            {
                Quote.InitializeQuotationDetail();
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();                               
            }
            myQD = getMyQuotationDetail(Quote);
            return Json(new { succeed = result, data = myQD, err = Err }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateTaxRateV2(QuoteFormViewModel quoteFormViewModel)
        {
            string errMsg = "";
            var result = true;
            string data = "";

            var currentQuote = quoteFormViewModel.ConverToQuote();
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
            try
            {
                currentQuote.InitializeQuotationDetail();
                quoteFormViewModel.SetQuoteItems(currentQuote.QuotationDetail);// convert quote details to quoteitems
                string viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteCartPartial_ViewName", "");

                data = this.RenderRazorViewToString(viewName, quoteFormViewModel);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                result = false;
            }

            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuoteDetailView(string approveID)
        {

            try
            {
                QuotationMaster Quote = new QuotationMaster();
                Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];


                WorkFlowApproval approval = eQuotationContext.Current.WorkFlowApproval.Where(a => a.UID == approveID).FirstOrDefault();
                QuotesViewModel QVModel = new QuotesViewModel();
                QVModel = setQuoteViewModel(Quote);
                QVModel.pagetype = "ReviewQuoteApproval";
                QVModel.Approval = approval;
                string view = string.Empty;
                switch (AppContext.AppRegion)
                {
                    case "ACN":
                        view = "_QuoteDetails";
                        break;
                    case "ABB":
                        view = "ABB_QuoteDetails";
                        break;
                    default:
                        break;
                }

                return PartialView(view, QVModel);
            }
            catch(Exception)
            {

            }

            return new EmptyResult();


        }

        [Authorize]
        [AuthorizeInfo("MQ02006", "It allows user to update expired date in approval process.", "Module Quote", "MQ00000")]

        public ActionResult updateExpiredDate(DateTime newExpiredDate, DateTime currentExpiredDate)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            
            try
            {
                Quote.expiredDate = newExpiredDate;
                Quote.QuotationExtensionNew.NewExpiredDate = newExpiredDate;
                //if (Quote.QuotationExtensionNew.NewExpiredDate == null)
                //{
                //    Quote.expiredDate = newExpiredDate;
                //    Quote.QuotationExtensionNew.NewExpiredDate = newExpiredDate;
                //}
                //else
                //{
                //    if (newExpiredDate <= currentExpiredDate)
                //    {
                //        Quote.expiredDate = newExpiredDate;
                //        Quote.QuotationExtensionNew.NewExpiredDate = newExpiredDate;
                //    }                       
                //    else
                //    {
                //        result = false;
                //        Err = "Another PSM updates expired date to " + Quote.QuotationExtensionNew.NewExpiredDate.Value.ToString("yyyy-MM-dd");

                //    }
                //}
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult changeORG(string ORG)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            var myQD = new List<MyQuotationDetail>();
            try
            {
                Quote.org = ORG;
                Quote.RemoveUnorderableItems();//刪除不在新org下的QuotationDetails
                if (Quote.QuotationDetail != null && Quote.QuotationDetail.Count > 0)
                {
                    //Quote.SimulatePrice(ref Err);
                    QuoteSimulatePrice.StartFlow(Quote, ref Err);
                    if (!string.IsNullOrEmpty(Err))// if error, clear all items, and show message to user 
                    {
                        Quote.QuotationDetail.Clear();
                        Err = "Clear all items because: " + Err;
                    }
                }

                myQD = getMyQuotationDetail(Quote);                  
                
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = myQD, err = Err }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        public ActionResult ChangeORGV2(QuoteFormViewModel quoteFormViewModel)
        {
            string errMsg = "";
            var result = true;
            string data = "";

            var currentQuote = quoteFormViewModel.ConverToQuote();
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
            try
            {
                currentQuote.RemoveUnorderableItems();//刪除不在新org下的QuotationDetails
                if (currentQuote.QuotationDetail != null && currentQuote.QuotationDetail.Count() > 0)
                {
                    currentQuote.SimulateListUnitPriceVPRS(ref errMsg);

                    //20180806 Alex: Calculate extend earranty price
                    currentQuote.CalculateAllExtendWarrantyPrice();
                }
                if (!string.IsNullOrEmpty(errMsg))// if error occur, recover to original quoteDetails
                {
                    currentQuote.QuotationDetail.Clear();
                    errMsg = "Clear all items because: " + errMsg;
                }

                quoteFormViewModel.SetQuoteItems(currentQuote.QuotationDetail);// convert quote details to quoteitems
                string viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteCartPartial_ViewName", "");

                data = this.RenderRazorViewToString(viewName, quoteFormViewModel);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                result = false;
            }

            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReloadPriceForAccountInfoChange(QuoteFormViewModel quoteFormViewModel)
        {
            string errMsg = "";
            var result = true;
            string data = "";

            var currentQuote = quoteFormViewModel.ConverToQuote();
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
            List<QuotationDetail> originalQuotCartItems = currentQuote.QuotationDetail.ToList();
            try
            {
                if (currentQuote.QuotationDetail != null && currentQuote.QuotationDetail.Count() > 0)
                {
                    if(quoteFormViewModel.Region == "ASG") // ASG may have different taxrate
                        currentQuote.SimulateListUnitPriceWithSameSalesDiscountRate(ref errMsg);
                    else
                        currentQuote.SimulateListUnitPriceVPRS(ref errMsg);

                    currentQuote.CalculateAllExtendWarrantyPrice(); ;
                }
                if (!string.IsNullOrEmpty(errMsg))// if error occur, recover to original quoteDetails
                {
                    currentQuote.QuotationDetail = originalQuotCartItems;
                    result = false;
                }
                else
                {
                    quoteFormViewModel.SetQuoteItems(currentQuote.QuotationDetail);// convert quote details to quoteitems
                    string viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteCartPartial_ViewName", "");

                    data = this.RenderRazorViewToString(viewName, quoteFormViewModel);
                }

                


            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                result = false;
            }



            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AuthorizeInfo("MQ02007", "It allows user to View End2EndGP in approval process.", "Module Quote", "MQ00000")]
        public ActionResult ViewEnd2EndGP()
        {
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize]
        public ActionResult LogOrderPartnerAddress(string QuotationPartner)
        {         
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            
            dynamic JSONQP = System.Web.Helpers.Json.Decode(QuotationPartner);
            try
            {
                EQPARTNER ExistedPartner = Quote.QuotationPartner.FirstOrDefault(r => r.TYPE == JSONQP.TYPE);

                if (ExistedPartner != null)
                {
                    EQPARTNER NewPartner = new EQPARTNER();
                    NewPartner.QUOTEID = String.IsNullOrEmpty(JSONQP.QUOTEID) ? ExistedPartner.QUOTEID : JSONQP.QUOTEID;
                    NewPartner.ERPID = String.IsNullOrEmpty(JSONQP.ERPID) ? ExistedPartner.ERPID : JSONQP.ERPID;
                    NewPartner.NAME = String.IsNullOrEmpty(JSONQP.NAME) ? ExistedPartner.NAME : JSONQP.NAME;
                    NewPartner.ADDRESS = String.IsNullOrEmpty(JSONQP.ADDRESS) ? ExistedPartner.ADDRESS : JSONQP.ADDRESS;
                    NewPartner.TYPE = String.IsNullOrEmpty(JSONQP.TYPE) ? ExistedPartner.TYPE : JSONQP.TYPE;
                    NewPartner.ATTENTION = String.IsNullOrEmpty(JSONQP.ATTENTION) ? ExistedPartner.ATTENTION : JSONQP.ATTENTION;
                    NewPartner.TEL = String.IsNullOrEmpty(JSONQP.TEL) ? ExistedPartner.TEL : JSONQP.TEL;
                    NewPartner.ZIPCODE = String.IsNullOrEmpty(JSONQP.ZIPCODE) ? ExistedPartner.ZIPCODE : JSONQP.ZIPCODE;
                    NewPartner.COUNTRY = String.IsNullOrEmpty(JSONQP.COUNTRY) ? ExistedPartner.COUNTRY : JSONQP.COUNTRY;
                    NewPartner.CITY = String.IsNullOrEmpty(JSONQP.CITY) ? ExistedPartner.CITY : JSONQP.CITY;
                    NewPartner.STATE = String.IsNullOrEmpty(JSONQP.STATE) ? ExistedPartner.STATE : JSONQP.STATE;
                    NewPartner.STREET = String.IsNullOrEmpty(JSONQP.STREET) ? ExistedPartner.STREET : JSONQP.STREET;
                    Quote.QuotationPartner.RemoveAll(d => d.TYPE == JSONQP.TYPE);
                    Quote.QuotationPartner.Add(NewPartner);
                }                
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult LogTermsConditions(string IsShippingHandlingIncluded, string IsTaxesIncluded, string IsFreightOnlyEstimeated)
        {
            var result = true;
            string Err = "";
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            //var itemsToRemove = Quote.QuotationPartner.Where(r => r.TYPE.Contains("BB_Extra")).ToList();
            if (Quote.QuotationPartner != null && Quote.QuotationPartner.Count > 0)
            {
                var item = Quote.QuotationPartner.FirstOrDefault(r => r.TYPE == "BB_Extra1");
                if (item != null)
                    Quote.QuotationPartner.Remove(item);
                item = Quote.QuotationPartner.FirstOrDefault(r => r.TYPE == "BB_Extra2");
                if (item != null)
                    Quote.QuotationPartner.Remove(item);
                item = Quote.QuotationPartner.FirstOrDefault(r => r.TYPE == "BB_Extra3");
                if (item != null)
                    Quote.QuotationPartner.Remove(item);
            }

            try
            {
                if (!String.IsNullOrEmpty(IsShippingHandlingIncluded) && IsShippingHandlingIncluded.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    EQPARTNER e1 = new EQPARTNER();
                    e1.QUOTEID = Quote.quoteId;
                    e1.TYPE = "BB_Extra1";
                    e1.NAME = "Shipping and Handling not included.";
                    Quote.QuotationPartner.RemoveAll(d => d.TYPE == "BB_Extra1");
                    Quote.QuotationPartner.Add(e1);
                }
                if (!String.IsNullOrEmpty(IsTaxesIncluded) && IsTaxesIncluded.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    EQPARTNER e2 = new EQPARTNER();
                    e2.QUOTEID = Quote.quoteId;
                    e2.TYPE = "BB_Extra2";
                    e2.NAME = "Taxes not included. (applicable in CA, FL, GA, IL, MN, OH, PA)";
                    Quote.QuotationPartner.RemoveAll(d => d.TYPE == "BB_Extra2");
                    Quote.QuotationPartner.Add(e2);
                }
                if (!String.IsNullOrEmpty(IsFreightOnlyEstimeated) && IsFreightOnlyEstimeated.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    EQPARTNER e3 = new EQPARTNER();
                    e3.QUOTEID = Quote.quoteId;
                    e3.TYPE = "BB_Extra3";
                    e3.NAME = "Shipping/Freight is only an estimate and subject to revision in the actual order.";
                    Quote.QuotationPartner.RemoveAll(d => d.TYPE == "BB_Extra3");
                    Quote.QuotationPartner.Add(e3);
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);
        }

        //由QuotationMaster內的QuotationDetail中取出頁面上要用的部份QuotationDetail
        public List<MyQuotationDetail> getMyQuotationDetail(QuotationMaster Quote)
        {
            var myQD = new List<MyQuotationDetail>();

            foreach (var item in Quote.QuotationDetail)
            {
                var qditem = new MyQuotationDetail();

                qditem.quoteId = item.quoteId;
                qditem.line_No = item.line_No.GetValueOrDefault();
                qditem.partNo = item.partNo;
                qditem.description = item.description;
                qditem.qty = item.qty.GetValueOrDefault();
                qditem.ITP = item.itp.GetValueOrDefault();
                qditem.SPRNO = item.sprNo ?? "";


                if (item.ItemType == (int)LineItemType.BTOSParent)
                {
                    if (AppContext.AppRegion == Region.ASG.ToString() || Quote.quoteNo.StartsWith("ASG"))
                    {
                        qditem.listPrice = Quote.SumBTOChildPreTaxListPrice(item.line_No.Value);
                        qditem.unitPrice = Quote.SumBTOChildPreTaxUnitPrice(item.line_No.Value);
                        qditem.newUnitPrice = Quote.SumBTOChildPreTaxNewUnitPrice(item.line_No.Value);
                        qditem.SubTotal = Quote.SumBTOChildPreTaxSubTotal(item.line_No.Value);
                    }
                    else
                    {
                        qditem.listPrice = Quote.SumBTOChildPostTaxListPrice(item.line_No.Value);
                        qditem.unitPrice = Quote.SumBTOChildPostTaxUnitPrice(item.line_No.Value);
                        qditem.newUnitPrice = Quote.SumBTOChildPostTaxNewUnitPrice(item.line_No.Value);
                        qditem.SubTotal = Quote.SumBTOChildPostTaxSubTotal(item.line_No.Value);

                    }
                    qditem.DefaultDiscountRate = (qditem.listPrice == 0) ? 0 : Decimal.Round((qditem.listPrice - qditem.unitPrice) / qditem.listPrice, 4);
                    qditem.SalesDiscountRate = (qditem.unitPrice == 0) ? 0 : Decimal.Round((qditem.unitPrice - qditem.newUnitPrice) / qditem.unitPrice, 4);

                }
                else
                {
                    if (AppContext.AppRegion == Region.ASG.ToString() || Quote.quoteNo.StartsWith("ASG"))
                    {
                        qditem.listPrice = item.listPrice.Value;
                        qditem.unitPrice = item.unitPrice.Value;
                        qditem.newUnitPrice = item.newUnitPrice.Value;
                        qditem.SubTotal = item.SubTotal;
                    }
                    else
                    {
                        qditem.listPrice = item.PostTaxListPrice;
                        qditem.unitPrice = item.PostTaxUnitPrice;
                        qditem.newUnitPrice = item.PostTaxNewUnitPrice;
                        qditem.SubTotal = item.PostTaxSubTotal;
                    }
                    qditem.DefaultDiscountRate = (qditem.listPrice == 0) ? 0 : Decimal.Round((qditem.listPrice - qditem.unitPrice) / qditem.listPrice, 4);
                    qditem.SalesDiscountRate = (qditem.unitPrice == 0) ? 0 : Decimal.Round((qditem.unitPrice - qditem.newUnitPrice) / qditem.unitPrice, 4);

                }
                
                qditem.BelowSAPPriceRate = item.BelowSAPPriceRate;

                //Alex 20180120 Don not show Service Part Margin
                if (PartBusinessLogic.IsServicePart2(qditem.partNo))
                {
                    qditem.Margin = 9999;
                    qditem.End2EndMargin = 9999;
                }
                else
                {
                    if (AppContext.AppRegion == Region.ACN.ToString() || Quote.quoteNo.StartsWith("ACN"))
                    {
                        qditem.Margin = item.ACNMargin;
                        qditem.End2EndMargin = item.End2EndGP;
                    }
                    else
                        qditem.Margin = item.Margin;
                }


                qditem.GPStatus = item.GPStatus;

                qditem.currency = Quote.currency;
                qditem.currencysign = Advantech.Myadvantech.DataAccess.MyExtension.GetCurrencySignByCurrency(Quote.currency);
                qditem.PreTaxTotalAmount = Quote.PreTaxTotalAmount.ToString("N");
                qditem.TotalTaxAmount = Quote.TotalTaxAmount.ToString("N");
                qditem.PostTaxTotalAmount = Quote.PostTaxTotalAmount.ToString("N");
                qditem.TaxRate = (Quote.tax * 100).ToString();
                qditem.itemtype = (int)item.ItemType;
                qditem.NCNR = (item.NCNR != null && item.NCNR == 1) ? true : false;
                qditem.CustomerPartNo = PartBusinessLogic.GetCustomerPartNoFromSAP(item.partNo, Quote.org, Quote.quoteToErpId);
                qditem.dlvplant = item.deliveryPlant;
                if(AppContext.AppRegion == Region.ASG.ToString())
                    qditem.ABCIndicator = PartBusinessLogic.GetABCIndicator(item.partNo, item.deliveryPlant);
                myQD.Add(qditem);
            }
            return myQD;
        }


        //[Authorize]
        [AllowAnonymous]
        public ActionResult QuoteApprove(string approveID, bool IsMobileApproved, QuotesViewModel model)
        {
            var approval = GPControlBusinessLogic.GetQuoteApprovalByApproveId(approveID);
            if (approval != null)
            {
                var currentQuoteApprovals = QuoteBusinessLogic.FindLatestWaitingApprovals(approval.TypeID);
                if (currentQuoteApprovals == null || !currentQuoteApprovals.Select(a => a.UID).Contains(approval.UID))
                    return RedirectToAction("QuoteApprovalResult", new { approveID = approveID, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.NotCurrentApprovalLevel });

                try
                {

                    //update approval status to 'approved'
                    if (!String.IsNullOrEmpty(AppContext.UserEmail))
                        approval.ApprovedBy = AppContext.UserEmail;
                    else
                        approval.ApprovedBy = approval.Approver;

                    if (IsMobileApproved)
                        approval.ApprovedReason = model == null ? "MailApprove:Approved": "MailApprove:Approved," + model.ApprovalComment;
                    else
                        approval.ApprovedReason = model == null ? "" : model.ApprovalComment;

                    approval.ApprovedDate = DateTime.Now;
                    approval.Status = (int)QuoteApprovalStatus.Approved;
                    approval.Update();

                    var quote = QuoteBusinessLogic.GetQuotationMaster(approval.TypeID);
                    ////確認是否最高簽核層級已經approve過
                    if (QuoteBusinessLogic.IsApprovedForMaxApprover(approval.TypeID))
                    {
                        
                        QuotationMasterHelper.UpdateQuotationStatus(quote.quoteId, OrderStatus.Finish);
                        QuotationMasterHelper.UpdateQuotationFinishDate(quote.quoteId, DateTime.Now);

                        //20180309 alex 改為由此驅動寄給業務的final mail
                        //20180323 alex 取消此作法 因為若沒登入 會無法取得appregion 而使用到錯誤的template 或無appregion而報錯
                        //SendFinalApprovalEmail(approval.TypeID);
                    }



                    // continue workflow to send email to  next approver
                    var url = Request.Url.Scheme + "://" + Request.Url.Authority;
                    // if addition Approvals existed, add new approval after current approval level num
                    var additionalApprovals = new List<WorkFlowApproval>();
                    if (model != null && model.AdditionalApprover != null)
                    {
                        var additionalApproval = new WorkFlowApproval
                        {
                            Approver = model.AdditionalApprover,
                            ApproverType = ApproverType.Other.ToString(),
                            LevelNum = GPControlBusinessLogic.GenerateNewSecondaryApproverLevel(approval.LevelNum),
                            ViewGP = approval.ViewGP
                        };
                        additionalApprovals.Add(additionalApproval);
                    }
                    QuoteApprovalDog.ResumeFlow(approval.TypeID, additionalApprovals);




                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return RedirectToAction("QuoteApprovalResult", new { approveID = approveID, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.Approved });
            }

            return RedirectToAction("MyQuotation");
        }


        //[Authorize]
        [AllowAnonymous]
        public ActionResult QuoteReject(string approveID, bool IsMobileApproved, QuotesViewModel model)
        {
            var approval = GPControlBusinessLogic.GetQuoteApprovalByApproveId(approveID);
            if (approval != null)
            {
                var currentQuoteApprovals = QuoteBusinessLogic.FindLatestWaitingApprovals(approval.TypeID);
                if (currentQuoteApprovals == null || !currentQuoteApprovals.Select(a => a.UID).Contains(approval.UID))
                    return RedirectToAction("QuoteApprovalResult", new { approveID = approveID, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.NotCurrentApprovalLevel });

                try
                {
                    if (!String.IsNullOrEmpty(AppContext.UserEmail))
                        approval.RejectedBy = AppContext.UserEmail;
                    else
                        approval.RejectedBy = approval.Approver;

                    if (IsMobileApproved)
                        approval.RejectReason = "MailApprove:Rejected";
                    else
                        approval.RejectReason = model == null ? "" : model.ApprovalComment;

                    approval.RejectedDate = DateTime.Now;
                    approval.Status = (int)QuoteApprovalStatus.Rejected;
                    approval.Update();

                    // Continue workflow to send email
                    var url = Request.Url.Scheme + "://" + Request.Url.Authority;
                    QuoteApprovalDog.ResumeFlow(approval.TypeID, null);


                    //20180309 alex 改為由此驅動寄給業務的final mail
                    //20180323 alex 取消此作法 因為若沒登入 會無法取得appregion 而使用到錯誤的template 或無appregion而報錯
                    //SendFinalApprovalEmail(approval.TypeID);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return RedirectToAction("QuoteApprovalResult", new { approveID = approveID, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.Rejected });
            }


            return RedirectToAction("MyQuotation");
        }


        [Authorize]
        //[AuthorizeInfo("MQ02002", "It allows user to update PSM approvers.", "Module Quote", "MQ00000")]
        public ActionResult GetACNPSMApproversList(int psmlevelNum)
        {
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            //List<PSMApproverViewModel> model = new List<PSMApproverViewModel>();

            var list = eQuotationContext.Current.ACN_EQ_PSM.OrderBy(p => p.Sector).ThenBy(p => p.PG).ToList();

            var model = new PSMApproverViewModel();

            foreach (var productDivGroup in list.GroupBy(l=>l.ProductDivision))
            {
                var productDivGroupModel = new ProductDivisionGroupModel();
                productDivGroupModel.Name = productDivGroup.Key;
                productDivGroupModel.PSMs = productDivGroup.ToList();
                model.ProductDivisionGroups.Add(productDivGroupModel);
            }

            //Alex 20180509 不需要幫user判斷是不是已經存在於approval list, 因為就算選到也不會重複加入相同的PSM
            //foreach (var psm in list)
            //{


            //    //int primaryLevel = GPControlBusinessLogic.GetPSMApproverLevelPriority(Region.ACN.ToString(), "");
            //    //int secondaryLevel = psm.Level.Value;
            //    //decimal levelNum = GPControlBusinessLogic.GenerateNewApproverLevel(primaryLevel, secondaryLevel);
            //    //if (Quote.WaitingApprovals.SingleOrDefault(p => p.Approver == psm.PSM &&
            //    //                            p.LevelNum == levelNum) == null)
            //    //{
            //    //    //var approvalViewmodel = new PSMApproverViewModel();
            //    //    //approvalViewmodel.Id = psm.Id;
            //    //    //approvalViewmodel.Name= psm.PSM;
            //    //    //approvalViewmodel.Sector = psm.Sector;
            //    //    //approvalViewmodel.ProductGroup = psm.PG;
            //    //    //approvalViewmodel.ProductDiv = psm.ProductDivision;
            //    //    //model.Add(approvalViewmodel);
            //    //}
            //}

            return PartialView("_PSMApproversList", model);

        }

        [Authorize]
        public ActionResult GetACNPSMApproversListV2(QuoteFormViewModel formModel)
        {


            var list = eQuotationContext.Current.ACN_EQ_PSM.OrderBy(p => p.Sector).ThenBy(p => p.PG).ToList();

            var model = new PSMApproverViewModel();
            model.CurrentApprovals = formModel.QuoteApproval.QuoteApprovalList;
            foreach (var productDivGroup in list.GroupBy(l => l.ProductDivision))
            {
                var productDivGroupModel = new ProductDivisionGroupModel();
                productDivGroupModel.Name = productDivGroup.Key;
                productDivGroupModel.PSMs = productDivGroup.ToList();
                model.ProductDivisionGroups.Add(productDivGroupModel);
                var sales = formModel.SalesRepresentatives.FirstOrDefault();
                if(sales != null)
                {
                    model.SalesCode = sales.SalesCode;
                    model.SalesEmail = sales.Email;
                }
                
            }

            return PartialView("_PSMApproversListV2", model);

        }

        [HttpPost]
        [Authorize]
        //[AuthorizeInfo("MQ02003", "It allows user to update PSM approvers.", "Module Quote", "MQ00000")]
        public ActionResult AddPSMApprovers(PSMApproverViewModel model)
        {
            if (AppContext.IsNewVersion)
            {
                //// For new version region, redirect to new action///
                if (AppContext.IsNewVersion)
                {
                    return RedirectToAction("AddPSMApproversV2", model);
                }
            }
            var result = true;
            string Err = "";
            if (ModelState.IsValid)
            {
                QuotationMaster Quote = new QuotationMaster();
                Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
                try
                {
                    //foreach (var item in model)
                    //{
                    //if (item.Selected)
                    if (model.ProductDivisionGroups != null)
                    {
                        var selectedProductDivs = model.ProductDivisionGroups.Where(g => g.Selected == true);
                        foreach (var selectedProductDiv in selectedProductDivs)
                        {

                            //var selectedPSMRule = GPControlBusinessLogic.FindACNPSMRuleById(item.Id);
                            var relatedPSMRules = GPControlBusinessLogic.FindACNPSMRulesByProductDiv(selectedProductDiv.Name);
                            if (relatedPSMRules != null)
                            {
                                //var relatedPSMRules = GPControlBusinessLogic.FindACNPSMRulesByProductDiv(selectedPSMRule.ProductDivision);
                                foreach (var PSMRule in relatedPSMRules)
                                {
                                    int primaryLevel = GPControlBusinessLogic.GetPSMApproverLevelPriority(AppContext.AppRegion, AppContext.AppSector);
                                    int secondaryLevel = PSMRule.Level.Value;
                                    decimal levelNum = GPControlBusinessLogic.GenerateNewApproverLevel(primaryLevel, secondaryLevel);

                                    var existApproval = Quote.WaitingApprovals
                                                .SingleOrDefault(p => p.TypeID == Quote.quoteId && p.Approver == PSMRule.PSM &&
                                                p.LevelNum == levelNum);
                                    if (existApproval == null)
                                    {
                                        WorkFlowApproval approval = new WorkFlowApproval();
                                        approval.UID = System.Guid.NewGuid().ToString();
                                        approval.LevelNum = levelNum;
                                        approval.ViewGP = 1;
                                        approval.TypeID = Quote.quoteId;
                                        approval.Url = Request.Url.Scheme + "://" + Request.Url.Authority;
                                        approval.Approver = PSMRule.PSM;
                                        approval.ApproverType = ApproverType.OptionalPSM.ToString();
                                        Quote.WaitingApprovals.Add(approval);


                                    }
                                }
                            }
                        }

                    }

                    //}
                }
                catch (Exception ex)
                {
                    result = false;
                    Err = ex.ToString();
                }

                QuotesViewModel QVModel = new QuotesViewModel();
                QVModel = setQuoteViewModel(Quote);
                QVModel.pagetype = "CheckBeforeApproval";
                return PartialView("_QuoteApproverList", QVModel);
            }


            return Json(new { succeed = result, err = "Model state invalid" }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [Authorize]
        public ActionResult AddPSMApproversV2(PSMApproverViewModel model)
        {
            var result = true;
            string Err = "";
            string data = "";
            if (ModelState.IsValid)
            {
                ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
                try
                {
                    if (model.ProductDivisionGroups != null)
                    {
                        var selectedProductDivs = model.ProductDivisionGroups.Where(g => g.Selected == true);
                        foreach (var selectedProductDiv in selectedProductDivs)
                        {

                            //var selectedPSMRule = GPControlBusinessLogic.FindACNPSMRuleById(item.Id);
                            var relatedPSMRules = GPControlBusinessLogic.FindACNPSMRulesByProductDiv(selectedProductDiv.Name);
                            if (relatedPSMRules != null)
                            {
                                //var relatedPSMRules = GPControlBusinessLogic.FindACNPSMRulesByProductDiv(selectedPSMRule.ProductDivision);
                                foreach (var PSMRule in relatedPSMRules)
                                {
                                    string quoteId = model.CurrentApprovals.FirstOrDefault().TypeID;
                                    string sector  = UserRoleBusinessLogic.getSectorBySalesEmailSalesCode(model.SalesEmail, model.SalesCode, AppContext.AppRegion);

                                    int primaryLevel = GPControlBusinessLogic.GetPSMApproverLevelPriority(AppContext.AppRegion, sector);
                                    int secondaryLevel = PSMRule.Level.Value;
                                    decimal levelNum = GPControlBusinessLogic.GenerateNewApproverLevel(primaryLevel, secondaryLevel);
                                    
                                    var existApproval = model.CurrentApprovals
                                                .SingleOrDefault(p => p.Approver == PSMRule.PSM &&
                                                p.LevelNum == levelNum);
                                    if (existApproval == null)
                                    {
                                        WorkFlowApproval approval = new WorkFlowApproval();
                                        approval.UID = System.Guid.NewGuid().ToString();
                                        approval.LevelNum = levelNum;
                                        approval.ViewGP = 1;
                                        approval.TypeID = quoteId;
                                        approval.Url = Request.Url.Scheme + "://" + Request.Url.Authority;
                                        approval.Approver = PSMRule.PSM;
                                        approval.ApproverType = ApproverType.OptionalPSM.ToString();
                                        model.CurrentApprovals.Add(approval);


                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    Err = ex.ToString();
                }

                String viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteApproverList_ViewName", "");
                var approvalViewModel = new QuoteApprovalViewModel();
                approvalViewModel.QuoteApprovalList = model.CurrentApprovals.OrderBy(a => a.LevelNum).ToList();
                approvalViewModel.IsCheckBeforeApproval = true;
                approvalViewModel.ApprovedCommentIsViewable = false;

                //check user if contain ViewTopExecutives action, if not, hide top executive approver
                var mngr = new IdentityManager();
                if (AppContext.AppRegion == "ACN" && !mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewTopExecutives"))
                    approvalViewModel.HiddenApprovers = eQuotationContext.Current.ACN_Executive.Select(e => e.Email.ToLower()).ToList();
                else
                    approvalViewModel.HiddenApprovers = new List<string>();
                if (mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewApprovedQuoteComment"))
                    approvalViewModel.ApprovedCommentIsViewable = true;

                ViewData.TemplateInfo.HtmlFieldPrefix = "QuoteApproval";


                data = this.RenderRazorViewToString(viewName, approvalViewModel);


                
            }


            return Json(new { succeed = result, data = data, err = Err }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Authorize]
        public ActionResult RemovePSMApproversV2(QuoteFormViewModel model, string approverId)
        {
            var result = true;
            string Err = "";
            string data = "";
            if (ModelState.IsValid)
            {
                ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
                try
                {
                    var approval = model.QuoteApproval.QuoteApprovalList.Where(a => a.UID == approverId).FirstOrDefault();
                    if(approval !=null)
                        model.QuoteApproval.QuoteApprovalList.Remove(approval);
                }
                catch (Exception ex)
                {
                    result = false;
                    Err = ex.ToString();
                }

                String viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteApproverList_ViewName", "");
                var approvalViewModel = new QuoteApprovalViewModel();
                approvalViewModel.QuoteApprovalList = model.QuoteApproval.QuoteApprovalList.OrderBy(a => a.LevelNum).ToList();
                approvalViewModel.IsCheckBeforeApproval = true;
                approvalViewModel.ApprovedCommentIsViewable = false;

                //check user if contain ViewTopExecutives action, if not, hide top executive approver
                var mngr = new IdentityManager();
                if (AppContext.AppRegion == "ACN" && !mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewTopExecutives"))
                    approvalViewModel.HiddenApprovers = eQuotationContext.Current.ACN_Executive.Select(e => e.Email.ToLower()).ToList();
                else
                    approvalViewModel.HiddenApprovers = new List<string>();
                if (mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewApprovedQuoteComment"))
                    approvalViewModel.ApprovedCommentIsViewable = true;

                ViewData.TemplateInfo.HtmlFieldPrefix = "QuoteApproval";


                data = this.RenderRazorViewToString(viewName, approvalViewModel);



            }





            return Json(new { succeed = result, data = data, err = Err }, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        [Authorize]
        //[AuthorizeInfo("MQ02003", "It allows user to update PSM approvers.", "Module Quote", "MQ00000")]
        public ActionResult RemovePSMApprovers(string approvalId)
        {
            var result = true;
            string Err = "";

            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            try
            {
                var existApproval = Quote.WaitingApprovals
                            .SingleOrDefault(p => p.UID == approvalId);
                if (existApproval != null)
                {
                    Quote.WaitingApprovals.Remove(existApproval);
                }
            }


            catch (Exception ex)
            {
                result = false;
                Err = ex.ToString();
            }

            QuotesViewModel QVModel = new QuotesViewModel();
            QVModel = setQuoteViewModel(Quote);
            QVModel.pagetype = "CheckBeforeApproval";
            return PartialView("_QuoteApproverList", QVModel);
        }

        [Authorize]
        [AuthorizeInfo("MQ02003", "It allows user to get CFO approvers.", "Module Quote", "MQ00000")]
        public List<Entities.AppUser> GetCFOApprovers()
        {
            var mngr = new IdentityManager();
            var roleRegionCFO = mngr.GetRole("CFO", AppContext.AppRegion);
            if (roleRegionCFO != null)
                return mngr.GetUsersByRole(roleRegionCFO.Id);
            return null;

        }


        [Authorize]
        [AuthorizeInfo("MQ02002", "It allows user to review quote approval.", "Module Quote", "MQ00000")]
        public ActionResult ReviewQuoteApproval(string approveId)
        {
            var approval = GPControlBusinessLogic.GetQuoteApprovalByApproveId(approveId);

            if (approval != null && approval.Status == (int)QuoteApprovalStatus.Wait_for_review)
            {
                if (QuoteBusinessLogic.IsRejectedByApprover(approval.TypeID))
                    return RedirectToAction("QuoteApprovalResult", new { approveID = approveId, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.HasBeenRejected });

                else
                {
                    QuotationMaster Quote = new QuotationMaster();
                    Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
                    Quote = QuoteBusinessLogic.GetQuotationMaster(approval.TypeID);
                    System.Web.HttpContext.Current.Session["QuotationMaster"] = Quote;

                    string errorMsg = "";
                    Quote.GetProductBOMCost("CNY", ref errorMsg); // get prodcut bom cost for ACN end2end GP

                    QuotesViewModel QVModel = new QuotesViewModel();
                    QVModel = setQuoteViewModel(Quote);
                    QVModel.Approval = approval;


                    //還沒輪到簽核者簽的話，只能先preview，不能按下簽核
                    var latestWaitingApprovals = QuoteBusinessLogic.FindLatestWaitingApprovals(Quote.quoteId);
                    if (latestWaitingApprovals != null && latestWaitingApprovals.Select(a => a.UID).Contains(approval.UID))
                        QVModel.pagetype = "ReviewQuoteApproval";
                    else
                        QVModel.pagetype = "PreviewQuoteApproval";


                    var mngr = new IdentityManager();

                    //========Region Variables Settings========            
                    //String ViewName = String.Empty;
                    //if (AppContext.AppRegion == "ACN")
                    //{
                    //    if (mngr.UserIsInAction(mngr.CurrentUser.Id, "GetCFOApprovers") && approval.ApproverType == ApproverType.PSM.ToString())
                    //    {
                    //        List<SelectListItem> items = new List<SelectListItem>();
                    //        var CFOApprovers = GetCFOApprovers();
                    //        if (CFOApprovers != null)
                    //        {
                    //            foreach (var user in CFOApprovers)
                    //            {
                    //                var item = new SelectListItem() { Value = user.Email, Text = user.Email };
                    //                items.Add(item);
                    //            }
                    //            QVModel.AdditionalApproversOption = items;
                    //        }
                    //    };
                    //    ViewName = "ReviewQuoteApproval";
                    //}
                    //else if (AppContext.AppRegion == "ABB")
                    //{
                    //    ViewName = "ABB_ReviewQuoteApproval";
                    //}
                    //========End Region Variables Settings========


                    return PartialView(ReviewQuoteApprovalView(AppContext.AppRegion), QVModel);
                }
            }
            else if (approval != null && approval.Status == (int)QuoteApprovalStatus.Approved)
            {
                return RedirectToAction("QuoteApprovalResult", new { approveID = approveId, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.HasBeenApproved });
            }
            else if (approval != null && QuoteBusinessLogic.IsRejectedByApprover(approval.TypeID))
            {
                return RedirectToAction("QuoteApprovalResult", new { approveID = approveId, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.HasBeenRejected });
            }

            return RedirectToAction("MyQuotation");
        }

        [HttpPost]
        [Authorize]
        public ActionResult ReviewQuoteApproval(string approveId, string submitButton, QuotesViewModel model)
        {
            var approval = GPControlBusinessLogic.GetQuoteApprovalByApproveId(approveId);
            if (approval != null && approval.Status == (int)QuoteApprovalStatus.Wait_for_review)
            {
                //update quote for tax chnage
                QuotationMaster newQuote = new QuotationMaster();
                newQuote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
                if (newQuote != null)// 從Reivewquoteapproval 頁面送出更新時
                {
                    newQuote.SaveAllChanges();

                    //For ACN, 當PSM全部簽核完 因可能apply SPR 所以再一次找尋approvers 根據結果把多的Approver刪除 
                    if (AppContext.AppRegion == "ACN" && (approval.ApproverType == ApproverType.PSM.ToString() || approval.ApproverType == ApproverType.OptionalPSM.ToString()))
                    {
                        var url = Request.Url.Scheme + "://" + Request.Url.Authority;
                        QuoteApproverFinder.StartFlow(newQuote, url, AppContext.AppRegion);

                        decimal newMaxLevel = 100;
                        if (QuoteApproverFinder.FindApproverResult == WorkFlowlAPI.FindApproverResult.NeedApprovalForGP)
                        {
                            newMaxLevel = QuoteApproverFinder.WaitingApprovals.Max(w => w.LevelNum);
                            if (newMaxLevel < approval.LevelNum)
                                newMaxLevel = approval.LevelNum;
                            GPControlBusinessLogic.RemoveQuoteWaitApprovalsByLevel(newQuote.quoteId, newMaxLevel);
                        }

                    }

                    // change mail content of following approvers
                    var waitReviewList = GPControlBusinessLogic.GetQuoteWaitApprovalsByQuoteId(newQuote.quoteId);
                    var allApprovalList = GPControlBusinessLogic.GetQuoteAllApprovalsByQuoteId(newQuote.quoteId);
                    QuotesViewModel QVModel = new QuotesViewModel();
                    QVModel = setQuoteViewModel(newQuote);
                    updateFinalMailBodyForApprovers(AppContext.AppRegion, allApprovalList, QVModel); // update all approver's final mail body(no GP)
                    updateMailBodyForApprovers(AppContext.AppRegion, waitReviewList, QVModel);// update all waiting approver's mail body


                }


                switch (submitButton)
                {
                    case "Approve":
                        return QuoteApprove(approveId, false, model);
                    case "Reject":
                        return QuoteReject(approveId, false, model);
                    default:
                        return RedirectToAction("MyQuotation");
                }
            }
            else
                return RedirectToAction("MyQuotation");
        }


        [AllowAnonymous]
        public ActionResult QuoteApprovalResult(string approveId, WorkFlowlAPI.ApprovalResultMessage approvalResultMessage)
        {
            var approval = GPControlBusinessLogic.GetQuoteApprovalByApproveId(approveId);

            if (approval != null)
            {
                QuotationMaster Quote = new QuotationMaster();
                Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
                Quote = QuoteBusinessLogic.GetQuotationMaster(approval.TypeID);
                System.Web.HttpContext.Current.Session["QuotationMaster"] = Quote;

                string errorMsg = "";
                Quote.GetProductBOMCost("CNY",ref errorMsg); // get prodcut bom cost for ACN end2end GP

                QuotesViewModel QVModel = new QuotesViewModel();
                QVModel = setQuoteViewModel(Quote);
                QVModel.Approval = approval;
                QVModel.ApprovalResultMessage = approvalResultMessage;
                QVModel.pagetype = "QuoteApprovalResult";
                //========Region Variables Settings========            
                String ViewName = String.Empty;
                string currentRegion = "";
                if (User.Identity.IsAuthenticated)
                    currentRegion = AppContext.AppRegion;
                else
                    currentRegion = eQuotation.DataAccess.SiebelDAL.GetMasterRBU(Quote.siebelRBU);

                if (currentRegion == "ACN" || currentRegion == "ASG")
                    ViewName = eQuotationDAL.GetRegionParameterValue(currentRegion, "", "QuoteApprovalResult_ViewName", "");
                else if (currentRegion == "ABB")
                    ViewName = "ABB_QuoteApprovalResult";
                //========End Region Variables Settings========
                return PartialView(ViewName, QVModel);
            }

            return RedirectToAction("MyQuotation");
        }


        [AllowAnonymous]
        public ActionResult QuoteMobileApprove(string mobileId, int expiredDays = 0)
        {
            var approval = GPControlBusinessLogic.GetQuoteApprovalByMobileId(mobileId);

            //20180517Alex 因為此action不需要登入  所以需透過Siebel Rbu 來對應正確的Region
            var quote = QuoteBusinessLogic.GetQuotationMaster(approval.TypeID);
            if (quote!= null && quote.siebelRBU != null)
            {
                string currentRegion = eQuotation.DataAccess.SiebelDAL.GetMasterRBU(quote.siebelRBU);
                AppContext.AppRegion = currentRegion;
            }



            if (approval != null && approval.Status == (int)QuoteApprovalStatus.Wait_for_review)
            {
                if (QuoteBusinessLogic.IsRejectedByApprover(approval.TypeID))
                    return RedirectToAction("QuoteApprovalResult", new { approveID = approval.UID, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.HasBeenRejected });
                else
                {
                    if (mobileId.StartsWith("MY"))
                    {
                        if (expiredDays != 0)
                            return QuoteMobileApproveAndUpdateExpiredDate(approval.UID, approval.TypeID, expiredDays);
                        return QuoteApprove(approval.UID, true, null);
                    }
                    if (mobileId.StartsWith("MN"))
                        return QuoteReject(approval.UID, true, null);
                }
            }
            else if (approval != null && approval.Status == (int)QuoteApprovalStatus.Approved)
                return RedirectToAction("QuoteApprovalResult", new { approveID = approval.UID, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.HasBeenApproved });
            else if (approval != null && QuoteBusinessLogic.IsRejectedByApprover(approval.TypeID))
                return RedirectToAction("QuoteApprovalResult", new { approveID = approval.UID, approvalResultMessage = WorkFlowlAPI.ApprovalResultMessage.HasBeenRejected });

            return RedirectToAction("MyQuotation");
        }

        [AllowAnonymous]
        public ActionResult QuoteMobileApproveWithExpiredDate90(string mobileId)
        {
            if (mobileId.StartsWith("MY"))
                return QuoteMobileApprove(mobileId, 90);
            return RedirectToAction("MyQuotation");
        }

        [AllowAnonymous]
        public ActionResult QuoteMobileApproveWithExpiredDate180(string mobileId)
        {
            if (mobileId.StartsWith("MY"))
                return QuoteMobileApprove(mobileId, 180);
            return RedirectToAction("MyQuotation");
        }

        private string GetAppRegionByQuoteOrgId(string orgId)
        {
            switch (orgId.ToUpper())
            {
                case "CN10":
                case "CN30":
                case "CN70":
                    return Region.ACN.ToString();
                case "US10":
                    return Region.ABB.ToString();
                default:
                    return Region.None.ToString(); ;
            }
        }

        public ActionResult QuoteMobileApproveAndUpdateExpiredDate(string approveId, string quoteId, int expiredDays)
        {
            var quote = QuoteBusinessLogic.GetQuotationMaster(quoteId);
            var newExpiredDate = quote.quoteDate.GetValueOrDefault(DateTime.Now).AddDays(expiredDays);
            QuoteBusinessLogic.updateQuoteWithNewExpiredDate(quoteId, newExpiredDate);
            QuotesViewModel model = new QuotesViewModel();

            // change mail content of following approvers
            var waitReviewList = GPControlBusinessLogic.GetQuoteWaitApprovalsByQuoteId(quoteId);
            var allApprovalList = GPControlBusinessLogic.GetQuoteAllApprovalsByQuoteId(quoteId);
            model = setQuoteViewModel(quote);

            //string currentRegion = eQuotation.DataAccess.SiebelDAL.GetMasterRBU(quote.siebelRBU);

            updateFinalMailBodyForApprovers(AppContext.AppRegion, allApprovalList, model); // update all approver's final mail body(no GP)
            updateMailBodyForApprovers(AppContext.AppRegion, waitReviewList, model);// update all waiting approver's mail body

            model.ApprovalComment = "(Change expired date to " + newExpiredDate.ToString("yyyy-MM-dd") + ")";
            return QuoteApprove(approveId, true, model);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [Authorize]
        [AuthorizeInfo("MQ02004", "It allows user to review waiting approval quote list.", "Module Quote", "MQ00000")]
        public ActionResult MyQuoteApproval()
        {
            var model = new QuotesViewModel();
            var myQA = new List<MyQuotationApproval>();
            myQA = getApproval();

            model.QA = myQA;
            return PartialView(model);
        }

        public List<MyQuotationApproval> getApproval()
        {
            var model = new QuotesViewModel();
            var qItem = new List<QuotationMaster>();
            var myQA = new List<MyQuotationApproval>();


            //Alex 20180319: fix 
            var approvalQuoteIdWaitingForUser = QuoteBusinessLogic.FindApprovalsWaitingForUser(model.UserEmail).Select(w => w.TypeID).ToList();
            qItem = (from QM in eQuotationContext.Current.QuotationMaster
                     where approvalQuoteIdWaitingForUser.Contains(QM.quoteId) && QM.DOCSTATUS != 2
                     select QM
                     ).OrderByDescending(a => a.quoteDate).ToList();

            foreach (var item in qItem)
            {
                var ListItem = new MyQuotationApproval();
                ListItem.quoteNo = item.quoteNo;
                ListItem.customId = item.customId;
                ListItem.quoteToErpId = item.quoteToErpId ?? "";
                ListItem.quoteToName = item.quoteToName;

                // Alex 如果quote的簽核流程還沒走到current user，則myApproval選level最接近的
                var myApproval = QuoteBusinessLogic.FindMyLatestWaitingApproval(item.quoteId, model.UserEmail);
                if (myApproval != null)
                {
                    ListItem.UID = myApproval.UID ?? "";
                }
                else
                {
                    //Alex 如果current user都已經完成該quote的所有簽核流程，則myApproval選level最大的
                    myApproval = QuoteBusinessLogic.FindMyMaxLevelApproval(item.quoteId, model.UserEmail);
                    ListItem.UID = myApproval.UID ?? "";
                }

                var Sales1 = item.QuotationPartner.Where(s => s.TYPE == "E").FirstOrDefault();
                ListItem.salesEmail = Sales1.NAME ?? "";
                ListItem.quoteDate = item.quoteDate != null ? item.quoteDate.Value.ToShortDateString() : "";

                ListItem.DOCSTATUS = QuoteBusinessLogic.getQuoteStatus(item.quoteId);

                ListItem.quoteId = item.quoteId;
                myQA.Add(ListItem);
            }

            return myQA;
        }

        [HttpPost]
        [Authorize]
        public ActionResult getApprovalList(string QuoteNo, string Description, string AccountName, string ERPID
           , string WaitforMe)
        {
            var result = true;
            string Err = "";
            var qItem = new List<QuotationMaster>();
            var items = new List<List<string>>();
            var model = new QuotesViewModel();

            try
            {
                //JJ：撈出Quotataion裡面有需要簽核的且簽核者裡面有登入者的
                qItem = (from QM in eQuotationContext.Current.QuotationMaster
                         where
((from WA in eQuotationContext.Current.WorkFlowApproval
  where WA.TypeID == QM.quoteId
  && model.UserEmail.ToLower().IndexOf(WA.Approver.ToLower()) > -1
  select WA.UID).Count() > 0) && QM.DOCSTATUS != 2
&& (string.IsNullOrEmpty(QuoteNo) ? true : QM.quoteNo.Equals(QuoteNo, StringComparison.OrdinalIgnoreCase))
&& (string.IsNullOrEmpty(Description) ? true : QM.customId.Equals(Description, StringComparison.OrdinalIgnoreCase))
&& (string.IsNullOrEmpty(AccountName) ? true : QM.quoteToName.Equals(AccountName, StringComparison.OrdinalIgnoreCase))
&& (string.IsNullOrEmpty(ERPID) ? true : QM.quoteToErpId.Equals(ERPID, StringComparison.OrdinalIgnoreCase))
                         select QM).OrderByDescending(a => a.quoteDate).Take(200).ToList();


                foreach (var item in qItem)
                {
                    //var waitingApproval = QuoteBusinessLogic.FindLatestWaitingApproval(item.quoteId);
                    //JJ：WaitforMe == true :撈現在簽核到該登入者的報價單、false：撈該登入者有需要簽核的報價單

                    //Alex 因有可能同層多人簽核，所以修正邏輯，並move邏輯至QuoteBusinessLogic
                    //var waitingApprovals = QuoteBusinessLogic.FindLatestWaitingApprovals(item.quoteId);
                    if (string.Compare(WaitforMe, "true", true) == 0)
                    {
                        if (!QuoteBusinessLogic.IsBelongCurrentApprovers(item.quoteId, model.UserEmail))
                            continue;
                    }


                    var ListItem = new List<string>();
                    ListItem.Add(item.quoteNo);//0
                    ListItem.Add(item.customId);//1
                    var quoteToErpId = item.quoteToErpId != null ? item.quoteToErpId : "";
                    ListItem.Add(quoteToErpId);//2
                    ListItem.Add(item.quoteToName);//3

                    var Sales1 = item.QuotationPartner.Where(s => s.TYPE == "E").FirstOrDefault();
                    var salesEmail = Sales1.NAME ?? "";
                    ListItem.Add(salesEmail);//4
                    var quoteDate = item.quoteDate != null ? item.quoteDate.Value.ToShortDateString() : "";
                    ListItem.Add(quoteDate);//5

                    //Alex把取得簽核狀態的邏輯寫到businesslogic
                    ListItem.Add(QuoteBusinessLogic.getQuoteStatus(item.quoteId));

                    // Alex 如果quote的簽核流程還沒走到current user，則myApproval選level最接近的
                    var myApproval = QuoteBusinessLogic.FindMyLatestWaitingApproval(item.quoteId, model.UserEmail);
                    if (myApproval != null)
                    {
                        var UID = myApproval.UID ?? "";
                        ListItem.Add(UID);//7
                    }
                    else
                    {
                        //Alex 如果current user都已經完成該quote的所有簽核流程，則myApproval選level最大的
                        myApproval = QuoteBusinessLogic.FindMyMaxLevelApproval(item.quoteId, model.UserEmail);
                        var UID = myApproval.UID ?? "";
                        ListItem.Add(UID);//7
                    }

                    ListItem.Add(item.quoteId);//8
                    items.Add(ListItem);
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = items, err = Err }, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //public ActionResult ViewQuotation(string quoteID)
        //{

        //    QuotationMaster Quote = new QuotationMaster();

        //    Quote = QuoteBusinessLogic.GetQuotationMaster(quoteID);

        //    QuotesViewModel QVModel = new QuotesViewModel();

        //    QVModel = setQuoteViewModel(Quote);

        //    return PartialView("ACN_QuotationTemplate", QVModel);
        //}

        [Authorize]
        [AuthorizeInfo("MQ02005", "It allows user to view approved quote comment.", "Module Quote", "MQ00000")]
        public ActionResult ViewApprovedQuoteComment(string quoteID)
        {
            QuotationMaster Quote = new QuotationMaster();
            Quote = QuoteBusinessLogic.GetQuotationMaster(quoteID);

            QuotesViewModel QVModel = new QuotesViewModel();
            Quote.WaitingApprovals = GPControlBusinessLogic.GetQuoteApprovedApprovalByQuoteId(quoteID);
            QVModel = setQuoteViewModel(Quote);

            return PartialView("_QuoteApproverList", QVModel);
        }

        [Authorize]
        [AuthorizeInfo("MQ02008", "It allows user to view top executives approvers in approval process.", "Module Quote", "MQ00000")]
        public ActionResult ViewTopExecutives()
        {
            return HttpNotFound();

        }

        [HttpPost]
        [Authorize]
        public ActionResult RecalBtoPrice(int parentLineNo, decimal totalAmount)
        {
            var result = true;
            string err = "";
            QuotationMaster quote = new QuotationMaster();
            quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            var myQD = new List<MyQuotationDetail>();
            try
            {
                if (quote != null)
                {
                    var recalResult = OrderBusinessLogic.SplitPricefromQuote(quote.QuotationDetail, parentLineNo, totalAmount, quote.DefaultERPID, quote.org, quote.currency, quote.tax.GetValueOrDefault(), ref err);
                    if (recalResult.Item1 == true)
                    {
                        quote.QuotationDetail = recalResult.Item2;
                        quote.InitializeQuotationDetail();
                        myQD = getMyQuotationDetail(quote);
                    }
                    else
                        result = false;

                }
                else
                {
                    result = false;
                    err = "Quote is null";
                }
            }
            catch (IndexOutOfRangeException e)
            {
                result = false;
                err = e.ToString();
            }

            return Json(new { succeed = result, data = myQD, err = err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult RecalBtoPriceV2(QuoteFormViewModel quoteFormViewModel , int parentLineNo, decimal totalAmount)
        {
            string errMsg = "";
            var result = true;
            string data = "";

            var currentQuote = quoteFormViewModel.ConverToQuote();
            ModelState.Clear();// for model will bind to partial view again, clear all value in modelstate first
            List<QuotationDetail> originalQuotCartItems = currentQuote.QuotationDetail.ToList();
            try
            {

                var recalResult = OrderBusinessLogic.SplitPricefromQuote(currentQuote.QuotationDetail, parentLineNo, totalAmount, currentQuote.DefaultERPID, currentQuote.org, currentQuote.currency, currentQuote.tax.GetValueOrDefault(), ref errMsg);
                if (!string.IsNullOrEmpty(errMsg))// if error occur, recover to original quoteDetails
                {
                    currentQuote.QuotationDetail = originalQuotCartItems;
                    result = false;
                }
                else
                {
                    if (recalResult.Item1 == true)
                    {
                        currentQuote.QuotationDetail = recalResult.Item2;
                        currentQuote.CalculateExtendWarrantyPriceByLineNo(parentLineNo);//Alex 20180810 Recalculate EW Part price 
                        currentQuote.InitializeQuotationDetail();

                    }
                    else
                        result = false;
                }

                quoteFormViewModel.SetQuoteItems(currentQuote.QuotationDetail);// convert quote details to quoteitems
                string viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteCartPartial_ViewName", "");

                data = this.RenderRazorViewToString(viewName, quoteFormViewModel);


            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                result = false;
            }



            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitQuote(QuoteFormViewModel model, string submitButton)
        {
            var result = true;
            string checkApprovalData = "";
            string errMsg = "";
            try
            {
                QuotationMaster quote = model.ConverToQuote();

                //Get itp and update Itp by SPR
                if (quote.QuotationDetail.Count() > 0)
                {
                    QuoteSimulatePriceV2.StartFlow(ref quote, ref errMsg);

                    QuoteBusinessLogic.UpdateItpBySprNo(ref quote);
                }
                else
                    errMsg = "No item in this quote!";

                if (submitButton == "CheckBeforeConfirm" && ModelState.IsValid)
                {


                    if (string.IsNullOrEmpty(errMsg))
                    {
                        //傳入QutoeMaster啟動找尋approver的流程 並返回帶有紀錄GP的QuoteMaster
                        QuoteApproverFinder.StartFlow(quote, Util.CurrentUrl(), AppContext.AppRegion);

                        if (QuoteApproverFinder.FindApproverResult == WorkFlowlAPI.FindApproverResult.NoNeed)
                        {

                            quote.DOCSTATUS = 1;
                            quote.QuotationExtensionNew.FinishDate = DateTime.Now;
                            return ConfirmV2(quote);
                        }
                        else
                        {
                            model.FindApproverResult = QuoteApproverFinder.FindApproverResult;
                            model.QuoteApproval.QuoteApprovalList = QuoteApproverFinder.WaitingApprovals.OrderBy(a=>a.LevelNum).ToList();
                            model.QuoteApproval.IsCheckBeforeApproval = true;
                            model.QuoteApproval.ApprovedCommentIsViewable = false;

                            //check user if contain ViewTopExecutives action, if not, hide top executive approver
                            var mngr = new IdentityManager();
                            if (AppContext.AppRegion == "ACN" && !mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewTopExecutives"))
                                model.QuoteApproval.HiddenApprovers = eQuotationContext.Current.ACN_Executive.Select(e => e.Email.ToLower()).ToList();
                            else
                                model.QuoteApproval.HiddenApprovers = new List<string>();
                            if (mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewApprovedQuoteComment"))
                                model.QuoteApproval.ApprovedCommentIsViewable = true;

                            QuotesViewModel qvModel = new QuotesViewModel();
                            qvModel = setQuoteViewModel(QuoteApproverFinder.QuoteMasterWithGPStatus);// Alex 20180724: Using QuoteMasterWithGPStatus to show item's GP Status in approval preview
                            qvModel.pagetype = "GPCheck";
                            model.quote = qvModel;
                            

                            String viewName = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "CheckBeforeApproval_ViewName", "");

                            checkApprovalData = this.RenderRazorViewToString(viewName, model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, errMsg);
                        result = false;
                    }

                }
                else if (submitButton == "ConfirmAfterCheck" && quote.WaitingApprovals.Any())
                {
                    //if ACN ISG, at least select one PSM
                    if (AppContext.AppRegion == "ACN")
                    {
                        var sales = quote.QuotationPartner.Where(q => q.TYPE == "E").FirstOrDefault();
                        var ACNSector = UserRoleBusinessLogic.getSectorBySalesEmailSalesCode(sales.NAME, sales.ERPID, AppContext.AppRegion);
                        if (ACNSector == "ISG" || ACNSector == "IAG" || (ACNSector == "EIOT" && !QuoteBusinessLogic.IsACNLoosItemQuote(quote.QuotationDetail)))
                        {
                            if (quote.WaitingApprovals.Where(a => a.ApproverType == ApproverType.PSM.ToString() || a.ApproverType == ApproverType.OptionalPSM.ToString()).ToList().Count == 0)
                                return Json(new { succeed = false, checkApprovalData = "", msg = "Please select at least one PSM!" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    //Get itp again(because itp lost after checking approvals)  and update Itp by SPR
                    QuoteSimulatePriceV2.StartFlow(ref quote, ref errMsg);
                    QuoteBusinessLogic.UpdateItpBySprNo(ref quote);

                    quote.DOCSTATUS = 0;
                    quote.QuotationExtensionNew.BelowGP = true;
                    return ConfirmV2(quote);

                }
                else if (submitButton == "Save")
                {
                    QuoteApproverFinder.StartFlow(quote, Util.CurrentUrl(), AppContext.AppRegion);

                    if (QuoteApproverFinder.FindApproverResult != WorkFlowlAPI.FindApproverResult.NoNeed)
                    {
                        quote.QuotationExtensionNew.BelowGP = true;
                        ModelState.AddModelError("GPStatus", "Below GP");
                    }

                    quote.SaveAllChanges();
                }
                else if (!ModelState.IsValid)
                    result = false;

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                result = false;
            }

            errMsg = GetErrorsFromModelState(); // get model state error and normal error


            return Json(new { succeed = result, checkApprovalData = checkApprovalData, msg = errMsg }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmV2(QuotationMaster quote)
        {
            var result = true;
            string errMsg = "";
            try
            {
                if (!ModelState.IsValid)
                    result = false;
                else
                {

                    if (quote.QuotationOpty != null)
                    {
                        //新增Oppty的執行寫在這裡
                        if (quote.QuotationOpty.optyId.Equals("new ID"))
                        {
                            // Add siebel active create opportunity record
                            SiebelActive SA = new SiebelActive();
                            SA.ActiveSource = "eQuotation";
                            SA.ActiveType = "CreateOpportunity";
                            SA.Status = "UnProcess";
                            SA.QuoteID = quote.quoteId ?? "";
                            SA.OptyName = quote.QuotationOpty.optyName ?? "";
                            SA.OptyStage = quote.QuotationOpty.optyStage ?? "";
                            SA.Amount = quote.PostTaxTotalAmount;
                            var sales = quote.QuotationPartner.Where(q => q.TYPE == "E").FirstOrDefault();
                            if (sales != null && !string.IsNullOrEmpty(sales.NAME))
                            {
                                SA.OptyOwnerEmail = sales.NAME;
                            }
                            else if (!string.IsNullOrEmpty(quote.salesEmail))
                            {
                                SA.OptyOwnerEmail = quote.salesEmail;
                            }
                            else
                            {
                                SA.OptyOwnerEmail = AppContext.UserEmail ?? "";
                            }
                            SA.CreateBy = AppContext.UserEmail ?? "";
                            SA.CreatedDate = DateTime.Now;
                            SA.LastUpdatedBy = AppContext.UserEmail ?? "";
                            SA.LastUpdatedDate = DateTime.Now;
                            quote.SiebelActive.Add(SA);
                        }
                    }

                    // 20180717 Alex: Move Crate Quote siebel active to confirm action
                    SiebelActive SAQuote = new SiebelActive();
                    SAQuote.ActiveSource = "eQuotation";
                    SAQuote.ActiveType = "CreateQuote";
                    SAQuote.Status = "UnProcess";
                    SAQuote.QuoteID = quote.quoteId;
                    SAQuote.CreateBy = AppContext.UserEmail;
                    SAQuote.CreatedDate = DateTime.Now;
                    SAQuote.LastUpdatedBy = AppContext.UserEmail;
                    SAQuote.LastUpdatedDate = DateTime.Now;
                    quote.SiebelActive.Add(SAQuote);

                    //Save to DB
                    quote.SaveAllChanges();

                    //Approval start
                    if (quote.DOCSTATUS == 0 && quote.WaitingApprovals.Any())
                    {
                        //啟動簽核程序 根據簽核者發送mail更新 approvals 物件的 mail body(因需要ReviewQuoteApproval生成郵件內容)
                        QuotesViewModel quoteVModel = setQuoteViewModel(quote);
                        quoteVModel.pagetype = "QuoteApprovalMail";
                        var finalMailBody = RenderRazorViewToString(ReviewQuoteApprovalView(AppContext.AppRegion), quoteVModel);// 此final mail body不會有GP
                        foreach (var qa in quote.WaitingApprovals)
                        {
                            quoteVModel.Approval = qa;
                            ViewBag.approval = qa;
                            qa.Mailbody = RenderRazorViewToString(ReviewQuoteApprovalView(AppContext.AppRegion), quoteVModel);
                            qa.FinalMailBody = finalMailBody;
                        }


                        //建立approvals 並啟動發送簽核信件的流程
                        QuoteApprovalDog.StartFlow(quote.quoteId, Util.CurrentUrl(), quote.WaitingApprovals, AppContext.AppRegion);

                        if (QuoteApprovalDog.ApprovalResult != WorkFlowlAPI.ApprovalResult.WaitingForApproval && QuoteApprovalDog.ApprovalResult != WorkFlowlAPI.ApprovalResult.Finish)
                        {
                            result = false;
                            ModelState.AddModelError(string.Empty, QuoteApprovalDog.ApprovalResult.ToString());
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                result = false;
            }


            errMsg = GetErrorsFromModelState(); // for ajax.beginform


            return Json(new { succeed = result, checkApprovalData = "", msg = errMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTokenInputPartNo(string q)
        {
            string keyword = q;
            keyword = keyword.Trim().Replace("'", "''").Replace("*", "%");
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" select distinct top 10 A.PART_NO, A.PRODUCT_DESC from  dbo.SAP_PRODUCT A INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO  ");
            sql.AppendFormat(" where A.PART_NO like '{0}%' ", keyword);
            sql.AppendFormat(" and  B.PRODUCT_STATUS in {0}", "('A','N','H','O','M1','C','P','S2','S5','T','V')");
            sql.AppendFormat(" AND B.SALES_ORG ='{0}' ", ((QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"]).org);
            //sql.AppendLine(" AND B.SALES_ORG ='CN10' ");
            sql.AppendLine(" order by part_no ");
            DataTable dt = DBUtil.dbGetDataTable("B2B", sql.ToString());
            List<TokeInputPartNo> list = new List<TokeInputPartNo>();
            if ((dt != null) && dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    list.Add(new TokeInputPartNo(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString(), ""));
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTokenInputPartNoBB(string q)
        {
            string keyword = q;
            keyword = keyword.Trim().Replace("'", "''").Replace("*", "%");
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" select * from ( select distinct top 4 A.PART_NO, A.PRODUCT_DESC, ISNULL(C.MATNR_P,'') as LegacyPN ");
            sql.AppendLine(" from dbo.SAP_PRODUCT A INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO  ");
            sql.AppendLine(" left join SAP_PRODUCT_AFFILIATE_MAPPING c on a.PART_NO = c.MATNR ");
            sql.AppendFormat(" where A.PART_NO like '%{0}%' ", keyword);
            sql.AppendFormat(" and  B.PRODUCT_STATUS in {0}", "('A','N','H','O','M1','C','P','S2','S5','T','V')");
            sql.AppendFormat(" AND B.SALES_ORG ='{0}' ", ((QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"]).org);            
            sql.AppendLine(" union ");
            sql.AppendLine(" select distinct top 2 A.PART_NO, A.PRODUCT_DESC, ISNULL(C.MATNR_P,'') as LegacyPN ");
            sql.AppendLine(" from dbo.SAP_PRODUCT A INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO  ");
            sql.AppendLine(" left join SAP_PRODUCT_AFFILIATE_MAPPING c on a.PART_NO = c.MATNR ");
            sql.AppendFormat(" where c.MATNR_P Like '%{0}%' ", keyword);
            sql.AppendFormat(" and  B.PRODUCT_STATUS in {0}", "('A','N','H','O','M1','C','P','S2','S5','T','V')");
            sql.AppendFormat(" AND B.SALES_ORG ='{0}' ", ((QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"]).org);
            sql.AppendLine("  ) t order by len(t.PART_NO) asc ");
            DataTable dt = DBUtil.dbGetDataTable("B2B", sql.ToString());
            List<TokeInputPartNo> list = new List<TokeInputPartNo>();
            if ((dt != null) && dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    list.Add(new TokeInputPartNo(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString(), dt.Rows[i][2].ToString()));
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTokenInputPartNoV2(string q, string org)
        {
            string keyword = q;

            List<TokeInputPartNo> list = new List<TokeInputPartNo>();

            var dt = QuoteBusinessLogic.GetPartNoByKeyWordAndOrg(keyword, org);
            if ((dt != null) && dt.Rows.Count > 0)
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i][0].ToString().StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase) == false)// 20180806 Alex, if ewPart, remove it in searching
                        list.Add(new TokeInputPartNo(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString(), dt.Rows[i][2].ToString()));
                }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetTokenInputSales(string q)
        {
            string keyword = q;
            keyword = keyword.Trim().Replace("'", "''").Replace("*", "%");
            StringBuilder sql = new StringBuilder();
            if (AppContext.AppRegion == "ACN")
            {
                sql.AppendLine(" select b.id_sap, c.EMAIL_ADDR ");
                sql.AppendLine(" from EAI_IDMAP b inner join EZ_EMPLOYEE c on b.id_email=c.EMAIL_ADDR  ");
                sql.AppendLine(" where b.id_fact_zone = 'China' and b.id_by_country <> 'X' ");
                sql.AppendFormat(" and (b.id_sap like '{0}%' or b.id_email like N'%{1}%') ", keyword, keyword);
                sql.AppendLine("  order by  b.id_sap ");
                //sql.AppendLine(" select SALES_CODE, EMAIL_ADDR from SAP_EMPLOYEE a inner join EZ_EMPLOYEE b on a.EMAIL=b.EMAIL_ADDR ");
                //sql.AppendLine(" inner join EAI_IDMAP c on a.SALES_CODE = c.id_sap ");
                //sql.AppendLine(" where SALES_CODE >= '40000001' and SALES_CODE <= '41340016' and SALESOFFICE >= '6100' and SALESOFFICE <= '6400' ");
                //sql.AppendLine(" and c.id_by_country <> 'X' ");
                //sql.AppendFormat(" and ( a.SALES_CODE like '{0}%' or a.FULL_NAME like N'%{1}%' ) ", keyword, keyword);
                //sql.AppendLine(" order by a.SALES_CODE ");


            }
            else if (AppContext.AppRegion == "ABB")
            {
                sql.AppendLine(" select distinct b.id_sap, c.EMAIL_ADDR  ");
                sql.AppendLine(" from  EAI_IDMAP b inner join EZ_EMPLOYEE c on b.id_email = c.EMAIL_ADDR ");
                sql.AppendLine(" where b.id_fact_zone = 'B+B US' and b.id_by_country <> 'X' ");
                sql.AppendFormat(" and (b.id_sap like '{0}%' or b.id_email like N'%{1}%') ", keyword, keyword);
                sql.AppendLine("  order by  b.id_sap ");
                //sql.AppendLine(" select distinct a.SALES_CODE, a.EMAIL ");
                //sql.AppendLine(" from SAP_EMPLOYEE a inner join EAI_IDMAP b on a.SALES_CODE = b.id_sap ");
                //sql.AppendLine(" inner join EZ_EMPLOYEE c on a.EMAIL = c.EMAIL_ADDR ");
                //sql.AppendLine(" where b.id_fact_zone = 'B+B US' and b.id_by_country <> 'X' ");
                //sql.AppendFormat(" and ( a.SALES_CODE like '{0}%' or a.EMAIL like N'%{1}%' ) ", keyword, keyword);
            }
            else if (AppContext.AppRegion == "ICG")
            {
                sql.AppendLine(" select distinct a.SALES_CODE, a.EMAIL ");
                sql.AppendLine(" from SAP_EMPLOYEE a inner join EAI_IDMAP b on a.SALES_CODE = b.id_sap ");
                sql.AppendLine(" inner join EZ_EMPLOYEE c on a.EMAIL = c.EMAIL_ADDR ");
                sql.AppendLine(" where b.id_rbu like '%ICG%' and b.id_by_country <> 'X' ");
            }


            DataTable dt = DBUtil.dbGetDataTable("MY", sql.ToString());
            List<postObject> list = new List<postObject>();
            if ((dt != null) && dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    list.Add(new postObject(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString()));
                }
            }

            // IT人員也可被加入Sales representative for testing purpose
            string[] ITMemberEmails = ConfigurationManager.AppSettings["EQV3_ITMember"].Split(';');
            if (ITMemberEmails.Contains(keyword))
            {
                list.Add(new postObject("49999999", keyword));
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTokenInputSalesV2(string q, string accountRowId)
        {
            var salesList = QuoteBusinessLogic.GetSalesRepresentatives(q, AppContext.AppRegion);

            if (AppContext.AppRegion == "ASG" && accountRowId != null)//  如果accountRowId有值,則檢查sales是否存在於account owners,沒有就移除
            {
                var accountOwners = eQuotation.DataAccess.SiebelDAL.GetAccountOwners(accountRowId);
                salesList.RemoveAll(item => !accountOwners.Contains(item.Email, StringComparer.CurrentCultureIgnoreCase));
            }

            List<postObject> result = salesList
                                      .Select(x => new postObject(x.SalesCode, x.Email))
                                      .ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }

    public class postObject
    {
        public string id;
        public string name;
        private string v1;
        private string v2;

        public postObject(string v1, string v2)
        {
            this.id = v1;
            this.name = v2;
        }
    }




    public class TokeInputPartNo
    {
        private string myid;
        public string id
        {
            get { return myid; }
            set { myid = value; }
        }

        private string myname;
        public string name
        {
            get { return myname; }
            set { myname = value; }
        }

        private string mycpn;
        public string cpn
        {
            get { return mycpn; }
            set { mycpn = value; }
        }


        public TokeInputPartNo()
        {
        }

        public TokeInputPartNo(string partno, string desc, string cpn)
        {
            this.id = partno;
            this.name = desc;
            this.cpn = cpn;
        }
    }

    public class TokeInput
    {
        private string myid;
        public string id
        {
            get { return myid; }
            set { myid = value; }
        }

        private string myname;
        public string name
        {
            get { return myname; }
            set { myname = value; }
        }

        public TokeInput()
        {
        }

        public TokeInput(string myid, string myname)
        {
            this.id = myid;
            this.name = myname;
        }
    }


}