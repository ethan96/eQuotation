using Advantech.Myadvantech.DataAccess;
using eQuotation.Models.Quotes;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Advantech.Myadvantech.Business;
using eQuotation.Models.Shared;
using eQuotation.Business;
using System.IO;
using System.Web.Configuration;
using System.Text;
using SAPDAL;
using eQuotation.Models.Enum;
using eQuotation.ViewModels;
using System.Configuration;
//using Advantech.Myadvantech.DataAccess;

namespace eQuotation.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class OperationController : AppControllerBase
    {
        private DataAccess.AppDbContext dbContext = new DataAccess.AppDbContext();

        // GET: Operation
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult SearchAccount()
        {
            //var model = this.UnitOfWork.Customer.Get();
            //return PartialView("_customers", model.ToList());

            List<SelectListItem> items1 = new List<SelectListItem>();
            items1.Add(new SelectListItem() { Text = "select..", Value = "" });
            items1.Add(new SelectListItem() { Text = "01- Channel Partner", Value = "01- Channel Partner" });
            //items1.Add(new SelectListItem() { Text = "01-Premier Channel Partner", Value = "01-Premier Channel Partner" });
            //items1.Add(new SelectListItem() { Text = "02-Gold Channel Partner", Value = "02-Gold Channel Partner" });
            //items1.Add(new SelectListItem() { Text = "03-Certified Channel Partner", Value = "03-Certified Channel Partner" });
            items1.Add(new SelectListItem() { Text = "04-DMS Premier Key Account", Value = "04-DMS Premier Key Account" });
            items1.Add(new SelectListItem() { Text = "06-Key Account", Value = "06-Key Account" });
            items1.Add(new SelectListItem() { Text = "06P-Potential Key Account", Value = "06P-Potential Key Account" });
            items1.Add(new SelectListItem() { Text = "07-General Account", Value = "07-General Account" });
            items1.Add(new SelectListItem() { Text = "08-Partner's Existing Customer", Value = "08-Partner's Existing Customer" });
            items1.Add(new SelectListItem() { Text = "09-Assigned to Partner", Value = "09-Assigned to Partner" });
            items1.Add(new SelectListItem() { Text = "10-Sales Contact", Value = "10-Sales Contact" });
            items1.Add(new SelectListItem() { Text = "11-Prospect", Value = "11-Prospect" });
            items1.Add(new SelectListItem() { Text = "12-Leads", Value = "12-Leads" });
            ViewBag.Status = items1;

            return PartialView("_accounts");
        }

        [Authorize]
        public ActionResult SearchAccountV2()
        {
            List<SelectListItem> items1 = new List<SelectListItem>();
            items1.Add(new SelectListItem() { Text = "select..", Value = "" });
            items1.Add(new SelectListItem() { Text = "01- Channel Partner", Value = "01- Channel Partner" });
            items1.Add(new SelectListItem() { Text = "04-DMS Premier Key Account", Value = "04-DMS Premier Key Account" });
            items1.Add(new SelectListItem() { Text = "06-Key Account", Value = "06-Key Account" });
            items1.Add(new SelectListItem() { Text = "06P-Potential Key Account", Value = "06P-Potential Key Account" });
            items1.Add(new SelectListItem() { Text = "07-General Account", Value = "07-General Account" });
            items1.Add(new SelectListItem() { Text = "08-Partner's Existing Customer", Value = "08-Partner's Existing Customer" });
            items1.Add(new SelectListItem() { Text = "09-Assigned to Partner", Value = "09-Assigned to Partner" });
            items1.Add(new SelectListItem() { Text = "10-Sales Contact", Value = "10-Sales Contact" });
            items1.Add(new SelectListItem() { Text = "11-Prospect", Value = "11-Prospect" });
            items1.Add(new SelectListItem() { Text = "12-Leads", Value = "12-Leads" });
            ViewBag.Status = items1;

            return PartialView("_accountsV2");
        }

        [Authorize]
        public ActionResult QuotationFormat()
        {
            List<SelectListItem> items1 = new List<SelectListItem>();
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            if (Quote.QuotationExtensionNew != null && Quote.QuotationExtensionNew.CompanyTitle != null)
            {

                if (Quote.QuotationExtensionNew.CompanyTitle == "CN10")
                {
                    items1.Add(new SelectListItem() { Text = "北京", Value = "CN10", Selected = true });
                    items1.Add(new SelectListItem() { Text = "上海(CN30)", Value = "CN30" });
                    items1.Add(new SelectListItem() { Text = "上海(CN70)", Value = "CN70" });
                }
                else if (Quote.QuotationExtensionNew.CompanyTitle == "CN30")
                {
                    items1.Add(new SelectListItem() { Text = "北京", Value = "CN10" });
                    items1.Add(new SelectListItem() { Text = "上海(CN30)", Value = "CN30", Selected = true });
                    items1.Add(new SelectListItem() { Text = "上海(CN70)", Value = "CN70" });
                }
                else if (Quote.QuotationExtensionNew.CompanyTitle == "CN70")
                {
                    items1.Add(new SelectListItem() { Text = "北京", Value = "CN10" });
                    items1.Add(new SelectListItem() { Text = "上海(CN30)", Value = "CN30" });
                    items1.Add(new SelectListItem() { Text = "上海(CN70)", Value = "CN70", Selected = true });
                }
                else
                {
                    items1.Add(new SelectListItem() { Text = "北京", Value = "CN10" });
                    items1.Add(new SelectListItem() { Text = "上海(CN30)", Value = "CN30" });
                    items1.Add(new SelectListItem() { Text = "上海(CN70)", Value = "CN70" }); //Wait open for CN70
                }
            }
            else
            {
                items1.Add(new SelectListItem() { Text = "北京", Value = "CN10" });
                items1.Add(new SelectListItem() { Text = "上海(CN30)", Value = "CN30" });
                items1.Add(new SelectListItem() { Text = "上海(CN70)", Value = "CN70" }); //Wait open for CN70
            }

            ViewBag.Company = items1;
            return PartialView("_change_company_title");
        }

        [HttpPost]
        [Authorize]
        public ActionResult getAccountList(string SearchAccount)
        {
            var result = true;
            string Err = "";
            var items = new List<List<string>>();
            String RBU = AppContext.AppRegion;

            try
            {
                //var account = string.IsNullOrEmpty(AccountName) ? "" : AccountName;
                //var erpid = string.IsNullOrEmpty(ERPID) ? "" : ERPID;
                //var sales = string.IsNullOrEmpty(Primary_Sales) ? "" : Primary_Sales;
                //var status = Status;
                List<String> SiebelRBU = eQuotation.DataAccess.SiebelDAL.GetRBUList(RBU);
                //ICC 2018/2/9 Add Country, city, zip code, state and region columns for searching
                SiebelAccountSearch search = Newtonsoft.Json.JsonConvert.DeserializeObject<SiebelAccountSearch>(SearchAccount);
                if (search == null)
                    search = new SiebelAccountSearch();

                var AccountList = eQuotation.DataAccess.SiebelDAL.GetSiebelAccountList(search, SiebelRBU);

                foreach (DataRow item in AccountList.Rows)
                {
                    var ListItem = new List<string>();
                    ListItem.Add(item["COMPANYNAME"].ToString());
                    ListItem.Add(item["ERPID"].ToString());

                    // Ryan 20171114 Comment below out due to org and currency will no longer show on table.
                    //if (!string.IsNullOrEmpty(item["ERPID"].ToString()))
                    //{
                    //    //Frank這應該會有效能問題，將來要想辦法改善
                    //    DataTable SAP = SqlProvider.dbGetDataTable("MY", string.Format(" select top 1 isnull(ORG_ID,'') as OrgID,isnull(CURRENCY,'') as CURRENCY  from  SAP_DIMCOMPANY  where COMPANY_ID='{0}'", item["ERPID"].ToString()));
                    //    if (SAP.Rows.Count > 0)
                    //    {
                    //        ListItem.Add(SAP.Rows[0][0].ToString());
                    //        ListItem.Add(SAP.Rows[0][1].ToString());
                    //    }
                    //    else
                    //    {
                    //        //Frank：要以登入者所屬的RBU來決定預設值ORG ID與幣別，等權限控管進行時要回頭調這一段
                    //        ListItem.Add("CN10");
                    //        ListItem.Add("CNY");
                    //    }
                    //}
                    //else
                    //{
                    //    //Frank：要以登入者所屬的RBU來決定預設值ORG ID與幣別，等權限控管進行時要回頭調這一段
                    //    ListItem.Add("CN10");
                    //    ListItem.Add("CNY");
                    //}

                    // Ryan 20171114 Org and currency will no longer show on table, just insert some values.
                    ListItem.Add("ORG");
                    if(RBU == "ASG")
                    {
                        if(item["COUNTRY"].ToString().Equals("Singapore", StringComparison.OrdinalIgnoreCase))
                            ListItem.Add("SGD");
                        else
                            ListItem.Add("USD");
                    }
                    else
                        ListItem.Add(DataAccess.SiebelDAL.GetCorrectCurrency(item["Currency"].ToString()));
                    ListItem.Add(item["STATUS"].ToString());
                    ListItem.Add(item["priSales"].ToString());
                    ListItem.Add(item["ROW_ID"].ToString());

                    //ICC 20170202 Add Country, city, zip code, state and district column in search account function
                    ListItem.Add(item["COUNTRY"].ToString());
                    ListItem.Add(item["CITY"].ToString());
                    ListItem.Add(item["ZIPCODE"].ToString());
                    ListItem.Add(item["STATE"].ToString());
                    string district = string.Empty;
                    if (!string.IsNullOrEmpty(item["ERPID"].ToString()))
                    {
                        DataTable dt = Advantech.Myadvantech.DataAccess.SAPDAL.GetSalesGroupOfficeDivisionDistrictByERPID(item["ERPID"].ToString());
                        if (dt != null && dt.Rows.Count > 0)
                            district = dt.Rows[0]["District"].ToString();
                    }

                    ListItem.Add(district);
                    //ICC 2018/2/9 Check region value
                    if (!string.IsNullOrEmpty(search.Region) && !string.Equals(search.Region, district, StringComparison.OrdinalIgnoreCase))
                        continue;

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

        [Authorize]
        public ActionResult SearchOppty()
        {
            List<SelectListItem> items1 = new List<SelectListItem>();
            //items1.Add(new SelectListItem() { Text = "select..", Value = "" });
            //items1.Add(new SelectListItem() { Text = "0% Lost", Value = "0% Lost" });
            items1.Add(new SelectListItem() { Text = "25% Proposing/Quoting", Value = "25% Proposing/Quoting", Selected = true });
            items1.Add(new SelectListItem() { Text = "50% Negotiating", Value = "50% Negotiating" });
            items1.Add(new SelectListItem() { Text = "75% Waiting for PO/Approval", Value = "75% Waiting for PO/Approval" });
            //items1.Add(new SelectListItem() { Text = "100% Won-PO Input in SAP", Value = "100% Won-PO Input in SAP" });
            ViewBag.Stage = items1;

            return PartialView("_oppty");
        }

        [Authorize]
        public ActionResult Configuration_List(string ORG)
        {
            ConfiguratorViewModel VM = new ConfiguratorViewModel();
            if (!string.IsNullOrWhiteSpace(ORG))
            {
                ORG = Advantech.Myadvantech.DataAccess.DataCore.CBOMV2_ConfiguratorDAL.GetCBOMORG(ORG);
                String str = "DECLARE @Child hierarchyid " +
                       " SELECT @Child = HIE_ID FROM CBOM_CATALOG_V2 " +
                       " WHERE ID = '" + ORG.Substring(0, 2) + "_Root' " +
                       " SELECT ID,CATALOG_NAME FROM CBOM_CATALOG_V2 " +
                       " WHERE HIE_ID.GetAncestor(1) = @Child " +
                       " ORDER BY SEQ_NO";

                DataTable _Catalog = SqlProvider.dbGetDataTable("CBOMV2", str);
                var _CatalogList = new List<SelectListItem>();
                _CatalogList.Add(new SelectListItem() { Text = "All", Value = "" });
                for (int i = 0; i <= _Catalog.Rows.Count - 1; i++)
                {
                    _CatalogList.Add(new SelectListItem()
                    {
                        Text = _Catalog.Rows[i][1].ToString(),
                        Value = _Catalog.Rows[i][0].ToString()
                    });
                }

                VM.CatalogList = new List<SelectListItem>();
                VM.CatalogList = _CatalogList;
            }

            VM.ORG = ORG;

            return PartialView("_Configurator_List", VM);
        }

        [Authorize]
        public ActionResult GetConfigurationResult(string quoteId, DateTime originalTime, int btoLineNo)
        {
            string errMsg = "";
            var result = true;
            string data = "";

            try
            {
                String str = string.Format("  SELECT TOP 1 Result, SelectedItems, CreatedTime FROM [HubConfiguredResult] where source = 'eQuotation3' and ID = '{0}' and ParentLineNo = '{1}'", quoteId, btoLineNo);
                DataTable dtConfigurationResult = SqlProvider.dbGetDataTable("MY", str);

                if (dtConfigurationResult != null && dtConfigurationResult.Rows.Count > 0)
                {
                    if (dtConfigurationResult.Rows[0][0].ToString() == "1")
                    {
                        var BtoUpdateTime = Convert.ToDateTime(dtConfigurationResult.Rows[0][2]);
                        if (BtoUpdateTime > originalTime)
                        {
                            data = dtConfigurationResult.Rows[0][1].ToString();

                        }
                    }
                    else
                    {
                        errMsg = "Fail to add configurator items";
                        result = false;
                    }
                }
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
            }
            return Json(new { succeed = result, data = data, err = errMsg }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetCurrentServerTime()
        {
  
            return Json(new { currentServerTime = DateTime.Now.ToString() }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetConfiguratorUrl(string quoteId, string org, string companyId,string currency, bool isReconfig, int btoLineNo)
        {
            //string strMyAdvantechUrl = "http://aclecampaign2:7600";
            string strMyAdvantechUrl = "http://myacore.advantech.com/";
            var configuratorUrl = string.Format(strMyAdvantechUrl +
                "/Configurator/ParametersCheck?sourceId={0}&sourceLineNo={1}&sourceSite=eQuotation3&salesOrg={2}&companyId={3}&currency={4}",
                quoteId, btoLineNo, org, companyId, currency);
            if (isReconfig)
            {
                configuratorUrl += "&reconfigId=" + quoteId;
                //configuratorUrl = string.Format(strMyAdvantechUrl +
                //    "/Configurator/ParametersCheck?sourceId={0}&sourceSite=eQuotation3&salesOrg={1}&companyId={2}&currency={3}&reconfigId={4}",
                //    quoteId, org, companyId, currency, quoteId);
            }
            return Content(configuratorUrl, "text/html");
        }


        [Authorize]
        public ActionResult PartInfo(string PartNo, string ORG, string Currency)
        {
            ViewBag.PartNo = PartNo;
            ViewBag.ORG = ORG;
            ViewBag.Currency = Currency;
            return PartialView("_ProductInfo");
        }

        [Authorize]
        public ActionResult SPRNO(int LineNo)
        {
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            var QD = Quote.QuotationDetail.Where(p => p.line_No == LineNo).FirstOrDefault();
            if (QD != null)
            {
                ViewBag.LineNo = LineNo;
                ViewBag.sprNo = QD.sprNo;
            }
            return PartialView("_SPRNO");
        }

        [Authorize]
        public ActionResult SPRNoV2(int lineNo, string  sprNo, string erpId, string partNo, DateTime expiredDate)
        {
            ViewBag.LineNo = lineNo;
            ViewBag.sprNo = sprNo;
            ViewBag.erpId = erpId;
            ViewBag.partNo = partNo;
            ViewBag.expiredDate = Convert.ToString(expiredDate);
            return PartialView("_SPRNoV2");
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MO01001", "It allows user to update SPR NO.", "Module Quote", "MO00000")]
        public ActionResult updateSPRNO(string SPRNO, int LineNo)
        {
            var result = true;
            string Err = "";
            var myQD = new List<MyQuotationDetail>();
            var QuotesControl = new QuotesController();
            try
            {
                QuotationMaster Quote = new QuotationMaster();
                Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
                var item = Quote.QuotationDetail.Where(p => p.line_No == LineNo).FirstOrDefault();
                if (item != null)
                {
                    if (string.IsNullOrEmpty(SPRNO))
                    {
                        Quote.QuotationDetail.Where(d => d.line_No == LineNo)
                                                .ToList()
                                                .ForEach(d =>
                                                {
                                                    item.newItp = item.itp;
                                                    item.sprNo = "";
                                                    //item.newUnitPrice = item.unitPrice;
                                                });
                        Quote.InitializeQuotationDetail();
                        //Quote.SaveAllChanges(); Alex20180331 輸入SPR 時還不用SAVE TO Table
                        myQD = QuotesControl.getMyQuotationDetail(Quote);
                        return Json(new { succeed = result, data = myQD, err = Err }, JsonRequestBehavior.AllowGet);
                    }

                    var _sql = new StringBuilder();
                    _sql.AppendLine(" select A.Request_No,A.RBU_NAME,A.CUS_ID,B.SpecialOffered ,B.Model,A.P_From, A.P_End,isnull(B.Request_SP,0) as Request_SP,A.STATUS, isnull(A.CUS_CURRENCY,'') AS CUS_CURRENCY ");
                    _sql.AppendLine(" from  [SPR_record] A right join  SPR_Model B on  A.RECORD_ID =B.Record_ID ");
                    _sql.AppendLine(string.Format(" where A.Request_No='{0}'", SPRNO));

                    DataTable DT = SqlProvider.dbGetDataTable("EZ", _sql.ToString());
                    if (DT != null && DT.Rows.Count > 0)
                    {
                        DataRow[] dr = DT.Select("CUS_ID='" + Quote.quoteToErpId + "'");
                        if (dr.Length == 0)
                        {
                            return Json(new { succeed = false, data = "", err = "The SPR is not applied for the quote-to account : " + Quote.quoteToErpId }, JsonRequestBehavior.AllowGet);
                        }
                        dr = DT.Select("Model='" + item.partNo + "'");
                        if (dr.Length == 0)
                        {
                            return Json(new { succeed = false, data = "", err = "The part no. of this SPR is not the same as the one of the current quote line." }, JsonRequestBehavior.AllowGet);
                        }
                        DataRow dritem = dr[0];
                        if (Convert.IsDBNull(dritem["SpecialOffered"]))
                        {
                            return Json(new { succeed = false, data = "", err = "Please input the SPR No. which is applied for the special ITP." }, JsonRequestBehavior.AllowGet);
                        }
                        if (Convert.IsDBNull(dritem["STATUS"]))
                        {
                            if (!string.Equals("APPROVED", dritem["STATUS"]))
                            {
                                return Json(new { succeed = false, data = "", err = "The SPR has not yet been approved." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        if (!Convert.IsDBNull(dritem["P_From"]) && !Convert.IsDBNull(dritem["P_End"]))
                        {
                            DateTime P_From = DateTime.Now;
                            DateTime P_End = DateTime.Now;
                            if (DateTime.TryParse(dritem["P_From"].ToString(), out P_From) && DateTime.TryParse(dritem["P_End"].ToString(), out P_End))
                            {
                                if (P_From.Date <= Quote.expiredDate.Value && Quote.expiredDate.Value <= P_End.Date)
                                {
                                    //Means SPR expired date and Quotation expired date is valid
                                }
                                else
                                {
                                    if (DateTime.Now <= P_End.Date && P_From.Date <= Quote.expiredDate.Value && Quote.expiredDate.Value > P_End.Date)
                                    {
                                        Quote.expiredDate = P_End.Date;
                                    }
                                    else
                                    {
                                        return Json(new { succeed = false, data = "", err = "The SPR is expired." }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                        decimal SITP = 0m; decimal Request_SP = 0m;
                        if (decimal.TryParse(dritem["SpecialOffered"].ToString(), out SITP) && decimal.TryParse(dritem["Request_SP"].ToString(), out Request_SP))
                        {
                            if (SITP > 0 && Request_SP > 0)
                            {
                                //_sql.Clear();
                                //_sql.AppendLine(string.Format(" update QuotationDetail set newItp='{0}',sprNo='{1}',newUnitPrice='{2}' ",SITP,SPRNO,Request_SP));
                                //_sql.AppendLine(string.Format(" where quoteId='{0}' and line_No={1} ",Quote.quoteId, LineNo));
                                //SqlProvider.dbExecuteNoQuery("EQ", _sql.ToString());

                                Quote.QuotationDetail.Where(d => d.line_No == LineNo)
                                                 .ToList()
                                                 .ForEach(d =>
                                                 {
                                                     item.newItp = SITP;
                                                     item.sprNo = SPRNO;
                                                     //item.newUnitPrice = Request_SP;
                                                 });
                                Quote.InitializeQuotationDetail();
                                //Quote.SaveAllChanges(); Alex20180331 輸入SPR 時還不用SAVE TO Table
                                myQD = QuotesControl.getMyQuotationDetail(Quote);
                            }
                        }
                    }
                }
                else
                {
                    return Json(new { succeed = false, data = "", err = "It's not find quotation detail." }, JsonRequestBehavior.AllowGet);
                }

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
        public ActionResult UpdateSPRNoV2(string sprNo, int lineNo, string erpId, string partNo, string expiredDateString)
        {
            var expiredDate = Convert.ToDateTime(expiredDateString);
            var result = true;
            string Err = "";
            var newItp = 0m;
            var QuotesControl = new QuotesController();
            try
            {
                if (!string.IsNullOrEmpty(sprNo))
                {
                    newItp = QuoteBusinessLogic.GetNewItpBySPR(sprNo, lineNo, erpId, partNo, expiredDateString, ref Err);
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }
            if (!string.IsNullOrEmpty(Err))
            {
                sprNo = "";
                result = false;
            }


            return Json(new { succeed = result, data = sprNo, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetProductATP(string strPartNo, string strORG, string strCurrency)
        {
            if (string.IsNullOrEmpty(strORG)) strORG = "";
            var result = true;
            string Err = ""; string plant1 = string.Empty; string plant2 = string.Empty; string plant3 = string.Empty;
            string plant4 = string.Empty; string plant5 = string.Empty;
            var RetStr = new StringBuilder();
            PartBusinessLogic.ShippingVia _ShipVia = PartBusinessLogic.ShippingVia.NA;
            try
            {
                switch (strORG)
                {
                    case "US01":
                        plant1 = "USH1";
                        //Jay 20150720:Determine ACL inventory's plant by MRP2-Special Procurement
                        plant2 = PartBusinessLogic.GetReferencePlantBySpecialProcurement(strPartNo, plant1);
                        //Ivy Yes, please always use USH1 material master for the default ship mode.  The TWH1 or CKH2 ship mode only determines how parts are being ship to TW & CN
                        _ShipVia = PartBusinessLogic.GetShippingVia(strPartNo, plant1);
                        break;
                    case "US10":
                        plant1 = "UBH1";
                        plant2 = "USH1";
                        break;
                    case "EU10":
                        plant1 = "EUH1";
                        plant2 = "TWH1";
                        break;
                    case "TW01":
                        plant1 = "TWH1";
                        plant2 = "CKH2";
                        break;
                    case "KR01":
                        plant1 = "KRH1";
                        plant2 = "TWH1";
                        break;
                    case "BR01":
                        plant1 = "BRH1";
                        plant2 = "TWH1";
                        break;
                    case "JP01":
                        plant1 = "TWH1";
                        plant2 = "JPH1";
                        plant3 = "TWM3";
                        plant4 = "TWM4";
                        plant5 = "CKH1";
                        break;
                    case "CN10":
                        plant1 = "CNH1";
                        plant2 = "CNH3";
                        plant3 = "TWH1";
                        plant4 = "CKH2";
                        break;
                    case "CN30":
                        plant1 = "CNH1";
                        plant2 = "CNH3";
                        plant3 = "TWH1";
                        plant4 = "CKH2";
                        break;
                    case "CN70":
                        plant1 = "CNH1";
                        plant2 = "CNH7";
                        plant3 = "TWH1";
                        plant4 = "CKH2";
                        break;
                    default:
                        plant1 = "TWH1";
                        plant2 = "CKH2";
                        break;
                }

                string strStatusCode = ""; string strStatusDesc = ""; decimal decATP = 0;

                var _sql = new StringBuilder();
                _sql.AppendLine(" Select top 1 a.PART_NO,a.PRODUCT_STATUS,isnull(a.MIN_ORDER_QTY,0) as  MIN_ORDER_QTY,isnull(a.DLV_PLANT,'') as DLV_PLANT,isnull(b.PLANT,'') as PLANT,isnull(b.ABC_INDICATOR,'') as  ABC_INDICATOR,isnull(c.txt,'') as PLMNotice ");
                _sql.AppendLine(" From SAP_PRODUCT_STATUS a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT ");
                _sql.AppendLine(" left join SAP_PRODUCT_ORDERNOTE c on a.PART_NO=c.PART_NO and a.SALES_ORG=c.ORG ");
                _sql.AppendLine(string.Format(" Where a.SALES_ORG='{1}' and a.PART_NO='{0}' ", strPartNo, strORG));

                DataTable DT = SqlProvider.dbGetDataTable("b2b", _sql.ToString());

                if (DT.Rows.Count > 0)
                {
                    string strMinPrice = "";
                    if (strORG.Substring(0, 2).ToUpper() == "TW")
                    {
                        string tmpCurrency = ""; string tmpErrMsg = "";
                        var _MinimumPrice_SalesTeam = new SAPDAL.SAPDAL.MinimumPrice_SalesTeam();
                        double tmpMinPrice = SAPDAL.SAPDAL.GetMinPrice("TW01", strPartNo, strCurrency, _MinimumPrice_SalesTeam, ref tmpErrMsg, ref tmpCurrency);
                        if (string.IsNullOrEmpty(tmpErrMsg) && tmpMinPrice >= 0)
                        {
                            strMinPrice = Advantech.Myadvantech.DataAccess.MyExtension.GetCurrencySignByCurrency(tmpCurrency) + tmpMinPrice;
                        }
                    }

                    RetStr.Append(" <table id='divProduct' style='width: 250px;float: left;' valign='top'>");
                    RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' width='100px'> Part No : </td>");
                    RetStr.Append(" <td  align='left' nowrap='nowrap' width='150px'> " + DT.Rows[0]["PART_NO"].ToString() + "</td>");
                    RetStr.Append("</tr>");
                    RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' > Product Status :</td>");
                    RetStr.Append(" <td align='left'> " + DT.Rows[0]["PRODUCT_STATUS"].ToString() + " </td> </tr>");
                    RetStr.Append("<tr> <td align='right' class='black'  nowrap='nowrap' > ABCD Indicator :</td>");
                    RetStr.Append(" <td align='left'> " + DT.Rows[0]["ABC_INDICATOR"].ToString() + " </td> </tr>");
                    RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' > MOQ :</td>");
                    RetStr.Append(" <td align='left'>" + Convert.ToDecimal(DT.Rows[0]["MIN_ORDER_QTY"]).ToString("f0") + " </td> </tr>");
                    if (_ShipVia != Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.NA)
                    {
                        string _PostPone = string.Empty;
                        switch (_ShipVia)
                        {
                            case Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.Air:
                                _PostPone = "(plus 2 weeks)";
                                break;
                            case Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.Sea:
                                _PostPone = "(plus 6 weeks)";
                                break;
                        }
                        RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' > Loading Group :</td>");
                        RetStr.Append(" <td align='left'>" + _ShipVia.ToString() + _PostPone + " </td> </tr>");
                    }
                    RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' > PLM Notice :</td>");
                    RetStr.Append(" <td align='left'> " + DT.Rows[0]["PLMNotice"].ToString());
                    RetStr.Append(" </td> </tr>");

                    if (!string.IsNullOrEmpty(strMinPrice))
                    {
                        RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' > Min. Price :</td>");
                        RetStr.Append(" <td align='left'> " + strMinPrice + " </td> </tr>");
                    }
                    RetStr.Append(" </table>");
                }
                RetStr.Append(GetACLATP(strPartNo, plant1, Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.NA));
                RetStr.Append(GetACLATP(strPartNo, plant2, _ShipVia));

                if (strORG.Equals("JP01", StringComparison.InvariantCultureIgnoreCase))
                {
                    RetStr.Append(GetACLATP(strPartNo, plant3, _ShipVia));
                    RetStr.Append(GetACLATP(strPartNo, plant4, _ShipVia));
                    RetStr.Append(GetACLATP(strPartNo, plant5, _ShipVia));
                }
                if (strORG.Equals("CN10", StringComparison.InvariantCultureIgnoreCase))
                {
                    RetStr.Append(GetACLATP(strPartNo, plant3, _ShipVia));
                    RetStr.Append(GetACLATP(strPartNo, plant4, _ShipVia));
                }
                if (strORG.Equals("CN30", StringComparison.InvariantCultureIgnoreCase))
                {
                    RetStr.Append(GetACLATP(strPartNo, plant3, _ShipVia));
                    RetStr.Append(GetACLATP(strPartNo, plant4, _ShipVia));
                }
                if (strORG.Equals("CN70", StringComparison.InvariantCultureIgnoreCase))
                {
                    RetStr.Append(GetACLATP(strPartNo, plant3, _ShipVia));
                    RetStr.Append(GetACLATP(strPartNo, plant4, _ShipVia));
                }
                RetStr.Append("<div class='clear'></div><style>.clear{ clear:both;}</style>");

            }
            catch (Exception ex)
            {
                Err = ex.ToString();
                result = false;
            }

            return Json(new { succeed = result, data = RetStr.ToString(), err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetUSVolumeDiscountInfo(string strPartNo)
        {
            var result = true;
            string RetStr = "", Err = "";

            try
            {
                DataTable dtHeader = new DataTable();
                DataTable dtResult = new DataTable();
            }
            catch (Exception ex)
            {
                Err = ex.ToString();
                result = false;
            }

            return Json(new { succeed = result, data = RetStr.ToString(), err = Err }, JsonRequestBehavior.AllowGet);
        }

        public string GetACLATP(string strPartNo, string strPlant, Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia shipvia)
        {
            var ATP = new StringBuilder();
            var prod_input = new SAPDAL.SAPDALDS.ProductInDataTable();
            var _sapdal = new SAPDAL.SAPDAL();
            string MainDeliveryPlant = "TWH1"; string _errormsg = string.Empty;
            if (!string.IsNullOrEmpty(strPlant)) MainDeliveryPlant = strPlant;
            var inventory_out = new SAPDAL.SAPDALDS.QueryInventory_OutputDataTable();
            prod_input.AddProductInRow(strPartNo, 0, MainDeliveryPlant);
            if (_sapdal.QueryInventory_V2(prod_input, MainDeliveryPlant, DateTime.Now, ref inventory_out, ref _errormsg))
            {
                ATP.Append("<table valign='top' style='width: 120px;float: left;margin-left:2px'><tr ><th colspan='2' style='color:Black'>Plant: " + strPlant + "</th></tr>");
                ATP.Append("<tr class='HeaderStyle'><th>Available Date</th><th>Qty</th></tr>");
                foreach (SAPDAL.SAPDALDS.QueryInventory_OutputRow invRow in inventory_out)
                {
                    ATP.Append("<tr><td align='center'>" + invRow.STOCK_DATE.ToString("yyyy/MM/dd") + "</td><td align='right'>" + invRow.STOCK.ToString() + "</td></tr>");
                }
                if (inventory_out.Rows.Count == 0)
                {
                    ATP.Append("<tr><td colspan='2'>No inventory</td></tr>");
                }
                ATP.Append(" </table>");
            }

            return ATP.ToString();
        }


        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [Authorize]
        public ActionResult checkInfo(string QuoteID)
        {
            if (AppContext.IsNewVersion)
                return CheckApprovalStatus(QuoteID);
            QuotesViewModel QVModel = new QuotesViewModel();
            QuotationMaster Quote = new QuotationMaster();

            if (!string.IsNullOrWhiteSpace(QuoteID))
            {
                Quote = QuoteBusinessLogic.GetQuotationMaster(QuoteID);
                if (Quote != null)
                {
                    IEnumerable<WorkFlowApproval> qaList = eQuotationContext.Current.WorkFlowApproval.Where(a => a.TypeID == QuoteID).ToList();
                    if (qaList != null && qaList.Count() > 0)
                        QVModel.QuoteApprovalList = qaList;
                    QVModel.QuoteNo = Quote.quoteNo;
                    QVModel.SalesComment = Quote.relatedInfo;
                }
                else
                    QVModel.QuoteNo = "";
            }
            else
                QVModel.QuoteNo = "";

            //========Region Variables Settings========            
            String ViewName = String.Empty;

            if (AppContext.AppRegion == "ACN")
            {
                ViewName = "_QuoteApproverList";
            }
            else if (AppContext.AppRegion == "ABB")
            {
                ViewName = "ABB_QuoteApproverList";
            }
            //========End Region Variables Settings========
            return PartialView(ViewName, QVModel);
        }

        [Authorize]
        public ActionResult CheckApprovalStatus(string quoteId)
        {
            var  model = new QuoteApprovalViewModel();

            if (!string.IsNullOrWhiteSpace(quoteId))
            {
                var quote = QuoteBusinessLogic.GetQuotationMaster(quoteId);
                if (quote != null)
                {
                    List<WorkFlowApproval> qaList = eQuotationContext.Current.WorkFlowApproval.OrderBy(q=>q.LevelNum).Where(a => a.TypeID == quoteId).ToList();
                    if (qaList != null && qaList.Count() > 0)
                        model.QuoteApprovalList = qaList;
                    model.QuoteNo = quote.quoteNo;
                    model.ApprovalReason = quote.relatedInfo;
                    model.ApprovedCommentIsViewable = false;

                    //check user if contain ViewTopExecutives action, if not, hide top executive approver
                    var mngr = new IdentityManager();
                    if(AppContext.AppRegion == "ACN" && !mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewTopExecutives"))
                            model.HiddenApprovers = eQuotationContext.Current.ACN_Executive.Select(e => e.Email.ToLower()).ToList();
                    else
                        model.HiddenApprovers = new List<string>();
                    if (mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewApprovedQuoteComment"))
                        model.ApprovedCommentIsViewable = true;

                }
                else
                    model.QuoteNo = "";
            }
            else
                model.QuoteNo = "";

            var viewName =  eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "QuoteApproverList_ViewName", "");

            //if (AppContext.AppRegion == "ASG")
            //{
            //    viewName = "_ASG_QuoteApproverListV2";
            //}
            return PartialView(viewName, model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult getConfiguration_List(string CatalogID, string ORG)
        {
            var result = true;
            string Err = "";
            var items = new List<List<string>>();
            DataTable _PartNoList = new DataTable();
            try
            {
                var org = string.IsNullOrEmpty(ORG) ? "" : ORG;

                if (!string.IsNullOrWhiteSpace(CatalogID))
                {
                    String str = "DECLARE @Child hierarchyid " +
                        " SELECT @Child = HIE_ID FROM CBOM_CATALOG_V2 " +
                        " WHERE ID = '" + CatalogID + "' " +
                        " SELECT a.ID, a.CATALOG_NAME, a.CATALOG_DESC, a.CATEGORY_GUID, b.CATEGORY_ID as CATEGORY_NAME " +
                        " FROM CBOM_CATALOG_V2 a left join CBOM_CATALOG_CATEGORY_V2 b on a.CATEGORY_GUID = b.ID " +
                        " WHERE a.HIE_ID.GetAncestor(1) = @Child " +
                        " ORDER BY a.SEQ_NO";
                    _PartNoList = SqlProvider.dbGetDataTable("CBOMV2", str);
                }
                else
                {
                    String str2 = "DECLARE @Child hierarchyid " +
                       " SELECT @Child = HIE_ID FROM CBOM_CATALOG_V2 " +
                       " WHERE ID = '" + ORG.Substring(0, 2) + "_Root' " +
                       " SELECT a.ID, a.CATALOG_NAME, a.CATALOG_DESC, a.CATEGORY_GUID, b.CATEGORY_ID as CATEGORY_NAME " +
                       " FROM CBOM_CATALOG_V2 a left join CBOM_CATALOG_CATEGORY_V2 b on a.CATEGORY_GUID = b.ID " +
                       " WHERE a.HIE_ID.IsDescendantOf(@Child) = 1 AND CATALOG_TYPE = '2' " +
                       " ORDER BY a.SEQ_NO";
                    _PartNoList = SqlProvider.dbGetDataTable("CBOMV2", str2);
                }

                for (int i = 0; i <= _PartNoList.Rows.Count - 1; i++)
                {
                    var ListItem = new List<string>();
                    ListItem.Add(i.ToString());
                    ListItem.Add(_PartNoList.Rows[i]["CATALOG_NAME"].ToString());
                    ListItem.Add(_PartNoList.Rows[i]["CATALOG_DESC"].ToString());
                    ListItem.Add(_PartNoList.Rows[i]["CATEGORY_GUID"].ToString());
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
        public ActionResult Configurator(string NAME, string ID, string QTY, string Currency, string CurrencySign, string ORG)
        {
            ConfiguratorViewModel VM = new ConfiguratorViewModel();

            VM.BTOId = ID;
            VM.BTOName = NAME;
            VM.QTY = QTY;
            VM.Currency = Currency;
            VM.CurrencySign = CurrencySign;
            VM.ORG = ORG;

            if (!string.IsNullOrEmpty(ORG))
            {
                VM.Language = ORG.ToUpper().Substring(0, 2);
            }
            else
            {
                VM.Language = "";
            }

            return PartialView("_Configurator", VM);
        }

        [HttpPost]
        [Authorize]
        public ActionResult getOpptyList(string OptyName, string AccountRowID, string EndCustomerERPID)
        {
            var result = true;
            string Err = "";
            var items = new List<List<string>>();

            try
            {
                //ICC 2018/2/2 如果是ACN的報價單，同時也不是eQ.HelpDesk成員，在Pick opportunity的時候要限制opportunity owner 必須是自己
                //非ACN區域則不受此限制
                var _isACN = !(AppContext.AppRegion != "ACN" || AppContext.UserBelongRoles.Where(u => u.Contains("eQ.HelpDesk")).Any());
                var _OptyName = string.IsNullOrEmpty(OptyName) ? "" : OptyName;
                //DataAccess.SiebelDAL.GetSiebelOpportunityList(ProjectName,"");
                DataTable OpptyList = SiebelDAL.GetSiebelOptyList(_OptyName, AccountRowID, AppContext.UserEmail, _isACN);
                foreach (DataRow item in OpptyList.Rows)
                {
                    //ICC 2018/1/29 Seperate 100% & 0% opportunities. These opportunities won't be selected for sales.
                    if (item["orderbystr"].ToString() != "a")
                        continue;
                    var ListItem = new List<string>();
                    ListItem.Add(item["ROW_ID"].ToString());
                    ListItem.Add(item["NAME"].ToString());
                    ListItem.Add(item["ACCOUNT_NAME"].ToString());
                    ListItem.Add(Convert.ToDateTime(item["CREATED"]).ToShortDateString());
                    ListItem.Add(item["PRIMARY"].ToString());
                    ListItem.Add(item["STAGE_NAME"].ToString());
                    ListItem.Add(item["CONTACT"].ToString());
                    items.Add(ListItem);
                }

                //JJ 2017/10/3：pick opportunity時加入撈出經銷商終端客戶opty(只適用中國區)
                //JJ 2017/11/9：因無法串接新客戶的Opty,新客戶沒有ERP ID,所以改為串接siebel的parent account就好，不加end customer的ERP ID
                //JJ 2017/11/22：Oppty只顯示Login業務名下的 

                //Alex 20180712: New design for eQuotation, don't use session
                //QuotationMaster Quote = new QuotationMaster();
                //Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
                if (AppContext.AppRegion == "ACN" && !string.IsNullOrEmpty(AccountRowID))
                {
                    //DataTable EndCustomerOppty = eQuotation.DataAccess.SiebelDAL.GetEndCustomerOpty(Quote.quoteToRowId, EndCustomerERPID);
                    DataTable EndCustomerOppty = eQuotation.DataAccess.SiebelDAL.GetEndCustomerOpty(AccountRowID, _OptyName, AppContext.UserEmail);
                    foreach (DataRow item in EndCustomerOppty.Rows)
                    {
                        //ICC 2018/1/29 Seperate 100% & 0% opportunities. These opportunities won't be selected for sales.
                        if (item["orderbystr"].ToString() != "a")
                            continue;
                        var ListItem = new List<string>();
                        ListItem.Add(item["ROW_ID"].ToString());
                        ListItem.Add(item["NAME"].ToString());
                        ListItem.Add(item["ACCOUNT_NAME"].ToString());
                        ListItem.Add(Convert.ToDateTime(item["CREATED"]).ToShortDateString());
                        ListItem.Add(item["PRIMARY"].ToString());
                        ListItem.Add(item["STAGE_NAME"].ToString());
                        ListItem.Add(item["CONTACT"].ToString());
                        items.Add(ListItem);
                    }
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = items, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult SearchShipBill(string type, string SoldtoID, string org)
        {
            ViewBag.type = type;
            ViewBag.SoldtoID = SoldtoID;
            ViewBag.Org = org;
            if (AppContext.IsNewVersion)
                return PartialView("_QuotePartnerList");       
            return PartialView("_ShipBill");
        }

        [HttpPost]
        [Authorize]
        public ActionResult getShipBillList(string SoldtoID, string AccountName, string ERPID, string Type)
        {
            var result = true;
            string Err = "";
            var items = new List<List<string>>();

            if (string.IsNullOrEmpty(Type))
            {
                result = false;
                Err = "System Err: The type is empty!";
            }

            try
            {
                var accountname = string.IsNullOrEmpty(AccountName) ? "" : AccountName;
                var erpid = string.IsNullOrEmpty(ERPID) ? "" : ERPID;
                var type = string.IsNullOrEmpty(Type) ? "" : Type;
                Advantech.Myadvantech.DataAccess.QuotationMaster Quote = new Advantech.Myadvantech.DataAccess.QuotationMaster();
                Quote = (Advantech.Myadvantech.DataAccess.QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

                //var AccountList = eQuotation.DataAccess.SiebelDAL.GetSiebelAccountList(account, erpid);
                var AccountList = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();
                if (type == "S")
                {
                    AccountList = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyShiptoByID(SoldtoID, Quote.org, erpid, accountname);
                }
                else if (type == "B")
                {
                    AccountList = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyBilltoByID(SoldtoID, Quote.org, erpid, accountname);
                }
                else if (type == "EM")
                {
                    AccountList = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyEndCustomerByID(SoldtoID, Quote.org, erpid, accountname);
                }

                if (AccountList.Count > 0)
                {
                    foreach (Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY item in AccountList)
                    {
                        var ListItem = new List<string>();
                        ListItem.Add(item.COMPANY_ID);
                        ListItem.Add(item.COMPANY_NAME);
                        ListItem.Add(item.ADDRESS);
                        ListItem.Add(item.ATTENTION);
                        ListItem.Add(SoldtoID);
                        items.Add(ListItem);
                    }
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
        public ActionResult getQuotePartnerList(string soldtoId, string accountName, string erpId, string type, string org)
        {
            var result = true;
            string Err = "";
            var items = new List<List<string>>();

            if (string.IsNullOrEmpty(type))
            {
                result = false;
                Err = "System Err: The type is empty!";
            }

            try
            {

                //var AccountList = eQuotation.DataAccess.SiebelDAL.GetSiebelAccountList(account, erpid);
                var AccountList = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();
                if (type == "S")
                {
                    AccountList = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyShiptoByID(soldtoId, org, erpId, accountName);
                }
                else if (type == "B")
                {
                    AccountList = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyBilltoByID(soldtoId, org, erpId, accountName);
                }
                else if (type == "EM")
                {
                    AccountList = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyEndCustomerByID(soldtoId, org, erpId, accountName);
                }

                if (AccountList.Count > 0)
                {
                    foreach (Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY item in AccountList)
                    {
                        var ListItem = new List<string>();
                        ListItem.Add(item.COMPANY_ID);
                        ListItem.Add(item.COMPANY_NAME);
                        ListItem.Add(item.ADDRESS);
                        ListItem.Add(item.ATTENTION);
                        ListItem.Add(soldtoId);
                        items.Add(ListItem);
                    }
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
        public ActionResult RemovePartner(string Type)
        {
            var result = true;
            string Err = "";
            Advantech.Myadvantech.DataAccess.QuotationMaster Quote = new Advantech.Myadvantech.DataAccess.QuotationMaster();
            Quote = (Advantech.Myadvantech.DataAccess.QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            try
            {
                Quote.QuotationPartner.RemoveAll(x => x.TYPE == Type);           
            }
            catch (Exception e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, err = Err }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Authorize]
        public ActionResult getShipBill(string SoldtoID, string AccountRowID, string AccountName, string ORG, string ERPID, string Type, string CURRENCY)
        {
            var result = true;
            string Err = "";
            var items = new Dictionary<string, Ship_Bill>();
            Advantech.Myadvantech.DataAccess.QuotationMaster Quote = new Advantech.Myadvantech.DataAccess.QuotationMaster();
            Quote = (Advantech.Myadvantech.DataAccess.QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            try
            {
                var soldtoId = string.IsNullOrEmpty(SoldtoID) ? "" : SoldtoID.Trim();
                var accountname = string.IsNullOrEmpty(AccountName) ? "" : AccountName.Trim();
                var erpid = string.IsNullOrEmpty(ERPID) ? "" : ERPID.Trim();
                var type = string.IsNullOrEmpty(Type) ? "" : Type.Trim();

                var AccountList_Siebel = new List<SIEBEL_ACCOUNT>();
                var AccountList_sold = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();
                var AccountList_ship = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();
                var AccountList_bill = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();
                var AccountList_customer = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();

                if (type == "S")
                {
                    AccountList_ship = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyShiptoByID(soldtoId, Quote.org, erpid, accountname);
                    Quote.QuotationPartner.RemoveAll(x => x.TYPE == "S");
                }
                else if (type == "B")
                {
                    AccountList_bill = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyBilltoByID(soldtoId, Quote.org, erpid, accountname);
                    Quote.QuotationPartner.RemoveAll(x => x.TYPE == "B");
                }
                else if (type == "EM")
                {
                    AccountList_customer = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyEndCustomerByID(soldtoId, Quote.org, erpid, accountname);
                    Quote.QuotationPartner.RemoveAll(x => x.TYPE == "EM");
                }
                else if (type == "")
                {
                    AccountList_Siebel = MyAdvantechDAL.GetSiebelAccountByRowID(AccountRowID);
                    AccountList_sold = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyByID(erpid);
                    AccountList_ship = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyShiptoByID(soldtoId, ORG, "", "");
                    AccountList_bill = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyBilltoByID(soldtoId, ORG, "", "");
                    
                    //20180411 Alex: Do not pick default endcunstomer
                    //AccountList_customer = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyEndCustomerByID(soldtoId, ORG, "", "");

                    Quote.QuotationPartner.RemoveAll(x => x.TYPE == "S");
                    Quote.QuotationPartner.RemoveAll(x => x.TYPE == "B");
                    Quote.QuotationPartner.RemoveAll(x => x.TYPE == "E");
                    Quote.QuotationPartner.RemoveAll(x => x.TYPE == "SOLDTO");
                    Quote.QuotationPartner.RemoveAll(x => x.TYPE == "EM");

                    Quote.org = ORG;
                    Quote.quoteToErpId = erpid;
                    Quote.quoteToName = accountname;
                    Quote.DIST_CHAN = "10";
                    Quote.DIVISION = "00";
                    

                    //Alex 20180510 get siebel Rbu from siebel directly if it's not found in sibel account table
                    if (AccountList_Siebel.Count > 0)
                    {
                        Quote.quoteToRowId = AccountList_Siebel.FirstOrDefault().ROW_ID;
                        Quote.siebelRBU = AccountList_Siebel.FirstOrDefault().RBU;
                    }
                    else
                    {
                        SiebelAccountSearch search = new SiebelAccountSearch();
                        search.AccountRowId = AccountRowID;
                        var AccountList = eQuotation.DataAccess.SiebelDAL.GetSiebelAccountList(search, null);
                        if (AccountList != null && AccountList.Rows.Count > 0 && AccountList.Rows[0]["RBU"] != null)
                            Quote.siebelRBU = AccountList.Rows[0]["RBU"].ToString();
                        else
                            Quote.siebelRBU = "";

                        Quote.quoteToRowId = AccountRowID;
                        
                    }

                    //Alex20180510 不使用siebel 帶進來的currency，改抓sap撈出的account currency, 若無ERPID 則用default erpid的幣別
                    if (AccountList_sold.Count > 0)
                    {
                        Quote.SALESGROUP = AccountList_sold.FirstOrDefault().SALESGROUP;
                        Quote.SALESOFFICE = AccountList_sold.FirstOrDefault().SALESOFFICE;
                        Quote.currency = AccountList_sold.FirstOrDefault().CURRENCY;
                    }
                    else
                        Quote.currency = Quote.DefaultCurrency;
                }

                if (AccountList_sold.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_sold[0].COMPANY_ID.Trim();
                    ListItem.AccountID = (AccountList_Siebel.Count > 0) ? AccountList_Siebel.FirstOrDefault().ROW_ID : AccountRowID;
                    ListItem.Name = AccountList_sold[0].COMPANY_NAME;
                    ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_sold[0].ADDRESS);
                    ListItem.City = AccountList_sold[0].CITY;
                    ListItem.State = AccountList_sold[0].REGION_CODE;
                    ListItem.Zipcode = AccountList_sold[0].ZIP_CODE;
                    ListItem.Country = AccountList_sold[0].COUNTRY;
                    ListItem.Attention = AccountList_sold[0].ATTENTION;
                    ListItem.Tel = AccountList_sold[0].TEL_NO;
                    ListItem.Type = "SOLDTO";
                    ListItem.Currency = CURRENCY;
                    items.Add("SOLDTO", ListItem);

                    EQPARTNER partner_sold = new EQPARTNER();
                    partner_sold.QUOTEID = Quote.quoteId.Trim();
                    partner_sold.TYPE = "SOLDTO";
                    partner_sold.ERPID = AccountList_sold[0].COMPANY_ID.Trim();
                    partner_sold.NAME = AccountList_sold[0].COMPANY_NAME;
                    partner_sold.ADDRESS = FormatHelper.CorrectPartnerAddressFormat(AccountList_sold[0].ADDRESS);
                    partner_sold.CITY = AccountList_sold[0].CITY;
                    partner_sold.STATE = AccountList_sold[0].REGION_CODE;
                    partner_sold.ZIPCODE = AccountList_sold[0].ZIP_CODE;
                    partner_sold.COUNTRY = AccountList_sold[0].COUNTRY;
                    partner_sold.ATTENTION = AccountList_sold[0].ATTENTION;
                    partner_sold.TEL = AccountList_sold[0].TEL_NO;

                    Quote.QuotationPartner.Add(partner_sold);
                }
                else if (AccountList_Siebel.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    ListItem.AccountID = AccountList_Siebel[0].ROW_ID;
                    ListItem.Name = AccountList_Siebel[0].ACCOUNT_NAME;
                    ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    ListItem.City = AccountList_Siebel[0].CITY;
                    ListItem.State = AccountList_Siebel[0].STATE;
                    ListItem.Zipcode = AccountList_Siebel[0].ZIPCODE;
                    ListItem.Country = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry(AccountList_Siebel[0].COUNTRY);
                    ListItem.Attention = "";
                    ListItem.Tel = AccountList_Siebel[0].PHONE_NUM;
                    ListItem.Type = "SOLDTO";
                    ListItem.Currency = CURRENCY;
                    items.Add("SOLDTO", ListItem);

                    EQPARTNER partner_sold = new EQPARTNER();
                    partner_sold.QUOTEID = Quote.quoteId.Trim();
                    partner_sold.TYPE = "SOLDTO";
                    partner_sold.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    partner_sold.NAME = AccountList_Siebel[0].ACCOUNT_NAME;
                    partner_sold.ADDRESS = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    partner_sold.CITY = AccountList_Siebel[0].CITY;
                    partner_sold.STATE = AccountList_Siebel[0].STATE;
                    partner_sold.ZIPCODE = AccountList_Siebel[0].ZIPCODE;
                    partner_sold.COUNTRY = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry(AccountList_Siebel[0].COUNTRY);
                    partner_sold.ATTENTION = "";
                    partner_sold.TEL = AccountList_Siebel[0].PHONE_NUM;

                    Quote.QuotationPartner.Add(partner_sold);

                }

                if (AccountList_ship.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_ship[0].COMPANY_ID.Trim();
                    ListItem.Name = AccountList_ship[0].COMPANY_NAME;
                    ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_ship[0].ADDRESS);
                    ListItem.City = AccountList_ship[0].CITY;
                    ListItem.State = AccountList_ship[0].REGION_CODE;
                    ListItem.Zipcode = AccountList_ship[0].ZIP_CODE;
                    ListItem.Country = AccountList_ship[0].COUNTRY;
                    ListItem.Attention = AccountList_ship[0].ATTENTION;
                    ListItem.Tel = AccountList_ship[0].TEL_NO;
                    ListItem.Type = "S";
                    items.Add("S", ListItem);

                    EQPARTNER partner_ship = new EQPARTNER();
                    partner_ship.QUOTEID = Quote.quoteId;
                    partner_ship.TYPE = "S";
                    partner_ship.ERPID = AccountList_ship[0].COMPANY_ID.Trim();
                    partner_ship.NAME = AccountList_ship[0].COMPANY_NAME;
                    partner_ship.ADDRESS = FormatHelper.CorrectPartnerAddressFormat(AccountList_ship[0].ADDRESS);
                    partner_ship.CITY = AccountList_ship[0].CITY;
                    partner_ship.STATE = AccountList_ship[0].REGION_CODE;
                    partner_ship.ZIPCODE = AccountList_ship[0].ZIP_CODE;
                    partner_ship.COUNTRY = AccountList_ship[0].COUNTRY;
                    partner_ship.ATTENTION = AccountList_ship[0].ATTENTION;
                    partner_ship.TEL = AccountList_ship[0].TEL_NO;

                    Quote.QuotationPartner.Add(partner_ship);
                }
                else if (AppContext.AppRegion == "ABB" && AccountList_Siebel.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    ListItem.AccountID = AccountList_Siebel[0].ROW_ID;
                    ListItem.Name = AccountList_Siebel[0].ACCOUNT_NAME;
                    ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    ListItem.City = AccountList_Siebel[0].CITY;
                    ListItem.State = AccountList_Siebel[0].STATE;
                    ListItem.Zipcode = AccountList_Siebel[0].ZIPCODE;
                    ListItem.Country = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry(AccountList_Siebel[0].COUNTRY);
                    ListItem.Attention = "";
                    ListItem.Tel = AccountList_Siebel[0].PHONE_NUM;
                    ListItem.Type = "S";
                    ListItem.Currency = CURRENCY;
                    items.Add("S", ListItem);

                    EQPARTNER partner_ship = new EQPARTNER();
                    partner_ship.QUOTEID = Quote.quoteId.Trim();
                    partner_ship.TYPE = "S";
                    partner_ship.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    partner_ship.NAME = AccountList_Siebel[0].ACCOUNT_NAME;
                    partner_ship.ADDRESS = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    partner_ship.CITY = AccountList_Siebel[0].CITY;
                    partner_ship.STATE = AccountList_Siebel[0].STATE;
                    partner_ship.ZIPCODE = AccountList_Siebel[0].ZIPCODE;
                    partner_ship.COUNTRY = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry(AccountList_Siebel[0].COUNTRY);
                    partner_ship.ATTENTION = "";
                    partner_ship.TEL = AccountList_Siebel[0].PHONE_NUM;

                    Quote.QuotationPartner.Add(partner_ship);

                }

                if (AccountList_bill.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_bill[0].COMPANY_ID.Trim();
                    ListItem.Name = AccountList_bill[0].COMPANY_NAME;
                    ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_bill[0].ADDRESS);
                    ListItem.City = AccountList_bill[0].CITY;
                    ListItem.State = AccountList_bill[0].REGION_CODE;
                    ListItem.Zipcode = AccountList_bill[0].ZIP_CODE;
                    ListItem.Country = AccountList_bill[0].COUNTRY;
                    ListItem.Attention = AccountList_bill[0].ATTENTION;
                    ListItem.Tel = AccountList_bill[0].TEL_NO;
                    ListItem.Type = "B";
                    items.Add("B", ListItem);

                    EQPARTNER partner_Bill = new EQPARTNER();
                    partner_Bill.QUOTEID = Quote.quoteId;
                    partner_Bill.TYPE = "B";
                    partner_Bill.ERPID = AccountList_bill[0].COMPANY_ID.Trim();
                    partner_Bill.NAME = AccountList_bill[0].COMPANY_NAME;
                    partner_Bill.ADDRESS = FormatHelper.CorrectPartnerAddressFormat(AccountList_bill[0].ADDRESS);
                    partner_Bill.CITY = AccountList_bill[0].CITY;
                    partner_Bill.STATE = AccountList_bill[0].REGION_CODE;
                    partner_Bill.ZIPCODE = AccountList_bill[0].ZIP_CODE;
                    partner_Bill.COUNTRY = AccountList_bill[0].COUNTRY;
                    partner_Bill.ATTENTION = AccountList_bill[0].ATTENTION;
                    partner_Bill.TEL = AccountList_bill[0].TEL_NO;

                    Quote.QuotationPartner.Add(partner_Bill);
                }
                else if (AppContext.AppRegion == "ABB" && AccountList_Siebel.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    ListItem.AccountID = AccountList_Siebel[0].ROW_ID;
                    ListItem.Name = AccountList_Siebel[0].ACCOUNT_NAME;
                    ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    ListItem.City = AccountList_Siebel[0].CITY;
                    ListItem.State = AccountList_Siebel[0].STATE;
                    ListItem.Zipcode = AccountList_Siebel[0].ZIPCODE;
                    ListItem.Country = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry((AccountList_Siebel[0].COUNTRY));
                    ListItem.Attention = "";
                    ListItem.Tel = AccountList_Siebel[0].PHONE_NUM;
                    ListItem.Type = "B";
                    ListItem.Currency = CURRENCY;
                    items.Add("B", ListItem);

                    EQPARTNER partner_Bill = new EQPARTNER();
                    partner_Bill.QUOTEID = Quote.quoteId.Trim();
                    partner_Bill.TYPE = "B";
                    partner_Bill.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    partner_Bill.NAME = AccountList_Siebel[0].ACCOUNT_NAME;
                    partner_Bill.ADDRESS = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    partner_Bill.CITY = AccountList_Siebel[0].CITY;
                    partner_Bill.STATE = AccountList_Siebel[0].STATE;
                    partner_Bill.ZIPCODE = AccountList_Siebel[0].ZIPCODE;
                    partner_Bill.COUNTRY = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry((AccountList_Siebel[0].COUNTRY));
                    partner_Bill.ATTENTION = "";
                    partner_Bill.TEL = AccountList_Siebel[0].PHONE_NUM;

                    Quote.QuotationPartner.Add(partner_Bill);

                }

                if (AccountList_customer.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_customer[0].COMPANY_ID.Trim();
                    ListItem.Name = AccountList_customer[0].COMPANY_NAME;
                    ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_customer[0].ADDRESS);
                    ListItem.City = AccountList_customer[0].CITY;
                    ListItem.State = AccountList_customer[0].REGION_CODE;
                    ListItem.Zipcode = AccountList_customer[0].ZIP_CODE;
                    ListItem.Country = AccountList_customer[0].COUNTRY;
                    ListItem.Attention = AccountList_customer[0].ATTENTION;
                    ListItem.Tel = AccountList_customer[0].TEL_NO;
                    ListItem.Type = "EM";
                    items.Add("EM", ListItem);

                    EQPARTNER partner_Customer = new EQPARTNER();
                    partner_Customer.QUOTEID = Quote.quoteId;
                    partner_Customer.TYPE = "EM";
                    partner_Customer.ERPID = AccountList_customer[0].COMPANY_ID.Trim();
                    partner_Customer.NAME = AccountList_customer[0].COMPANY_NAME;
                    partner_Customer.ADDRESS = FormatHelper.CorrectPartnerAddressFormat(AccountList_customer[0].ADDRESS);
                    partner_Customer.CITY = AccountList_customer[0].CITY;
                    partner_Customer.STATE = AccountList_customer[0].REGION_CODE;
                    partner_Customer.ZIPCODE = AccountList_customer[0].ZIP_CODE;
                    partner_Customer.COUNTRY = AccountList_customer[0].COUNTRY;
                    partner_Customer.ATTENTION = AccountList_customer[0].ATTENTION;
                    partner_Customer.TEL = AccountList_customer[0].TEL_NO;

                    Quote.QuotationPartner.Add(partner_Customer);
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
        public ActionResult GetQuotePartners(string soldtoId, string accountRowID, string accountName, string org, string erpId, string type, string currency, string country)
        {
            var result = true;
            string Err = "";
            var items = new Dictionary<string, Ship_Bill>();
            try
            {
                soldtoId = string.IsNullOrEmpty(soldtoId) ? "" : soldtoId.Trim();
                accountName = string.IsNullOrEmpty(accountName) ? "" : accountName.Trim();
                erpId = string.IsNullOrEmpty(erpId) ? "" : erpId.Trim();
                var siebelRBU = "";
                decimal taxRate = 0;


                var AccountList_Siebel = new List<SIEBEL_ACCOUNT>();
                var AccountList_sold = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();
                var AccountList_ship = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();
                var AccountList_bill = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();
                var AccountList_customer = new List<Advantech.Myadvantech.DataAccess.SAP_DIMCOMPANY>();

                if (type == "S")
                    AccountList_ship = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyShiptoByID(soldtoId, org, erpId, accountName);
                else if (type == "B")
                    AccountList_bill = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyBilltoByID(soldtoId, org, erpId, accountName);
                else if (type == "EM")
                    AccountList_customer = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyEndCustomerByID(soldtoId, org, erpId, accountName);
                else if (type == "")
                {
                    AccountList_Siebel = MyAdvantechDAL.GetSiebelAccountByRowID(accountRowID);
                    AccountList_sold = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyByID(erpId);
                    AccountList_ship = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyShiptoByID(soldtoId, org, "", "");
                    AccountList_bill = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyBilltoByID(soldtoId, org, "", "");


                    //Quote.DIST_CHAN = "10";
                    //Quote.DIVISION = "00";


                    if (AccountList_Siebel.Count > 0)
                        siebelRBU = AccountList_Siebel.FirstOrDefault().RBU;
                    else
                    {
                        SiebelAccountSearch search = new SiebelAccountSearch();
                        search.AccountRowId = accountRowID;
                        var AccountList = eQuotation.DataAccess.SiebelDAL.GetSiebelAccountList(search, null);
                        if (AccountList != null && AccountList.Rows.Count > 0 && AccountList.Rows[0]["RBU"] != null)
                            siebelRBU = AccountList.Rows[0]["RBU"].ToString();
                    }
                    //if(AppContext.AppRegion == "ASG" && !string.IsNullOrEmpty(country))
                    taxRate = QuoteBusinessLogic.GetTaxRateByAPPRegionAndCountry(AppContext.AppRegion, country);




                }

                if (AccountList_sold.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_sold[0].COMPANY_ID.Trim();
                    ListItem.AccountID = accountRowID;
                    ListItem.Name = AccountList_sold[0].COMPANY_NAME;
                    //ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_sold[0].ADDRESS);
                    ListItem.Address = AccountList_sold[0].ADDRESS;
                    ListItem.City = AccountList_sold[0].CITY;
                    ListItem.State = AccountList_sold[0].REGION_CODE;
                    ListItem.Zipcode = AccountList_sold[0].ZIP_CODE;
                    ListItem.Country = AccountList_sold[0].COUNTRY;
                    ListItem.Attention = AccountList_sold[0].ATTENTION;
                    ListItem.Tel = AccountList_sold[0].TEL_NO;
                    ListItem.Type = "SOLDTO";
                    ListItem.Currency = AccountList_sold.FirstOrDefault().CURRENCY;
                    ListItem.SiebelRBU = siebelRBU;
                    ListItem.SalesOffice = AccountList_sold.FirstOrDefault().SALESOFFICE; 
                    ListItem.SalesGroup = AccountList_sold.FirstOrDefault().SALESGROUP;
                    ListItem.Inco1 = AccountList_sold.FirstOrDefault().INCO1;
                    ListItem.Inco2 = AccountList_sold.FirstOrDefault().INCO2;
                    ListItem.TaxRate = taxRate;
                    items.Add("SOLDTO", ListItem);


                }
                else if (AccountList_Siebel.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    ListItem.AccountID = AccountList_Siebel[0].ROW_ID;
                    ListItem.Name = AccountList_Siebel[0].ACCOUNT_NAME;
                    //ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    ListItem.Address = AccountList_Siebel[0].ADDRESS;
                    ListItem.City = AccountList_Siebel[0].CITY;
                    ListItem.State = AccountList_Siebel[0].STATE;
                    ListItem.Zipcode = AccountList_Siebel[0].ZIPCODE;
                    ListItem.Country = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry(AccountList_Siebel[0].COUNTRY);
                    ListItem.Attention = "";
                    ListItem.Tel = AccountList_Siebel[0].PHONE_NUM;
                    ListItem.Type = "SOLDTO";
                    ListItem.Currency = currency;
                    ListItem.SiebelRBU = siebelRBU;
                    ListItem.SalesOffice = "";
                    ListItem.SalesGroup = "";
                    ListItem.Inco1 = null;
                    ListItem.Inco2 = null;
                    ListItem.TaxRate = taxRate;
                    items.Add("SOLDTO", ListItem);

                }

                if (AccountList_ship.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_ship[0].COMPANY_ID.Trim();
                    ListItem.Name = AccountList_ship[0].COMPANY_NAME;
                    //ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_ship[0].ADDRESS);
                    ListItem.Address = AccountList_ship[0].ADDRESS;
                    ListItem.City = AccountList_ship[0].CITY;
                    ListItem.State = AccountList_ship[0].REGION_CODE;
                    ListItem.Zipcode = AccountList_ship[0].ZIP_CODE;
                    ListItem.Country = AccountList_ship[0].COUNTRY;
                    ListItem.Attention = AccountList_ship[0].ATTENTION;
                    ListItem.Tel = AccountList_ship[0].TEL_NO;
                    ListItem.Type = "S";
                    items.Add("S", ListItem);
                }
                else if (AppContext.AppRegion != "ACN" && AccountList_Siebel.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    ListItem.AccountID = AccountList_Siebel[0].ROW_ID;
                    ListItem.Name = AccountList_Siebel[0].ACCOUNT_NAME;
                    //ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    ListItem.Address = AccountList_Siebel[0].ADDRESS;
                    ListItem.City = AccountList_Siebel[0].CITY;
                    ListItem.State = AccountList_Siebel[0].STATE;
                    ListItem.Zipcode = AccountList_Siebel[0].ZIPCODE;
                    ListItem.Country = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry(AccountList_Siebel[0].COUNTRY);
                    ListItem.Attention = "";
                    ListItem.Tel = AccountList_Siebel[0].PHONE_NUM;
                    ListItem.Type = "S";
                    ListItem.Currency = currency;
                    items.Add("S", ListItem);

                }

                if (AccountList_bill.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_bill[0].COMPANY_ID.Trim();
                    ListItem.Name = AccountList_bill[0].COMPANY_NAME;
                    //ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_bill[0].ADDRESS);
                    ListItem.Address = AccountList_bill[0].ADDRESS;
                    ListItem.City = AccountList_bill[0].CITY;
                    ListItem.State = AccountList_bill[0].REGION_CODE;
                    ListItem.Zipcode = AccountList_bill[0].ZIP_CODE;
                    ListItem.Country = AccountList_bill[0].COUNTRY;
                    ListItem.Attention = AccountList_bill[0].ATTENTION;
                    ListItem.Tel = AccountList_bill[0].TEL_NO;
                    ListItem.Type = "B";
                    items.Add("B", ListItem);
                }
                else if (AppContext.AppRegion != "ACN" && AccountList_Siebel.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_Siebel[0].ERP_ID.Trim();
                    ListItem.AccountID = AccountList_Siebel[0].ROW_ID;
                    ListItem.Name = AccountList_Siebel[0].ACCOUNT_NAME;
                    //ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_Siebel[0].ADDRESS);
                    ListItem.Address = AccountList_Siebel[0].ADDRESS;
                    ListItem.City = AccountList_Siebel[0].CITY;
                    ListItem.State = AccountList_Siebel[0].STATE;
                    ListItem.Zipcode = AccountList_Siebel[0].ZIPCODE;
                    ListItem.Country = SiebelBusinessLogic.ConvertSiebelCountrToSAPCountry(AccountList_Siebel[0].COUNTRY);
                    ListItem.Attention = "";
                    ListItem.Tel = AccountList_Siebel[0].PHONE_NUM;
                    ListItem.Type = "B";
                    ListItem.Currency = currency;
                    items.Add("B", ListItem);

                }

                if (AccountList_customer.Count > 0)
                {
                    var ListItem = new Ship_Bill();
                    ListItem.ERPID = AccountList_customer[0].COMPANY_ID.Trim();
                    ListItem.Name = AccountList_customer[0].COMPANY_NAME;
                    //ListItem.Address = FormatHelper.CorrectPartnerAddressFormat(AccountList_customer[0].ADDRESS);
                    ListItem.Address = AccountList_customer[0].ADDRESS;
                    ListItem.City = AccountList_customer[0].CITY;
                    ListItem.State = AccountList_customer[0].REGION_CODE;
                    ListItem.Zipcode = AccountList_customer[0].ZIP_CODE;
                    ListItem.Country = AccountList_customer[0].COUNTRY;
                    ListItem.Attention = AccountList_customer[0].ATTENTION;
                    ListItem.Tel = AccountList_customer[0].TEL_NO;
                    ListItem.Type = "EM";
                    items.Add("EM", ListItem);
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
        public ActionResult GetORGList(string ERPID)
        {
            var result = true;
            string Err = "";
            var ORGList = new List<SelectListItem>();
            String RBU = AppContext.AppRegion;

            try
            {
                var erpid = string.IsNullOrEmpty(ERPID) ? "" : ERPID.Trim();
                ORGList = eQuotation.DataAccess.SAPDAL.GetORGList(erpid, RBU);
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = ORGList, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetContactList(string AccountRowId)
        {
            var result = true;
            string Err = "";
            var ContactList = new List<SelectListItem>();
            var Contact = new List<SIEBEL_CONTACT>();
            try
            {
                Contact = MyAdvantechDAL.GetSiebelContactByAccountRowId(AccountRowId);
                ContactList.Add(new SelectListItem()
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
                        fullname = Contact[i].LastName + " " + Contact[i].FirstName;
                    ContactList.Add(new SelectListItem()
                    {
                        Text = fullname,
                        Value = Contact[i].ROW_ID
                    });
                }



            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = ContactList, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetDefaultSales(string AccountRowID, string ERPID)
        {
            var result = true;
            string Err = "";

            try
            {
                var salesList = QuoteBusinessLogic.GetSalesRepresentatives("", AppContext.AppRegion);

                if (AppContext.AppRegion == "ACN")// Alex 20180808: For ACN, using login person as default
                {
                    foreach(var sales in salesList)
                    {
                        if(sales.Email.IndexOf(AppContext.UserEmail, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            result = true;
                            return Json(new { succeed = result, sales = sales.SalesCode, salesemail = sales.Email, err = Err }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            result = false;
                        }

                    }
                        
                }
                else
                {
                    DataTable dtSiebel = new DataTable();
                    dtSiebel = DataAccess.SAPDAL.GetDefaultSalesFromSiebel(AccountRowID);
                    if (dtSiebel != null && dtSiebel.Rows.Count > 0 && salesList.Select(s => s.SalesCode).Contains(dtSiebel.Rows[0]["SALES_CODE"].ToString()))
                    {
                        result = true;
                        return Json(new { succeed = result, sales = dtSiebel.Rows[0]["SALES_CODE"].ToString(), salesemail = dtSiebel.Rows[0]["EMAIL"].ToString(), err = Err }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result = false;
                    }

                    DataTable dtSAP = new DataTable();
                    dtSAP = DataAccess.SAPDAL.GetDefaultSalesFromSAP(ERPID);
                    if (dtSAP != null && dtSAP.Rows.Count > 0 && salesList.Select(s => s.SalesCode).Contains(dtSAP.Rows[0]["SALES_CODE"].ToString()))
                    {
                        result = true;
                        return Json(new { succeed = result, sales = dtSAP.Rows[0]["SALES_CODE"].ToString(), salesemail = dtSAP.Rows[0]["EMAIL"].ToString(), err = Err }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, sales = "", salesemail = "", err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult setOpportunity(string optyId, string optyName, string quoteId, string optyStage)
        {
            if (AppContext.IsNewVersion)
                return SetOpportunityV2(optyId, optyName, quoteId, optyStage);
            var result = true;
            string Err = "";
            var items = new List<string>();
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            try
            {
                optyQuote oppty = new optyQuote();
                oppty.optyId = optyId ?? "";
                oppty.optyName = optyName ?? "";
                oppty.quoteId = quoteId ?? "";
                oppty.optyStage = optyStage ?? "";
                oppty.Opty_Owner_Email = AppContext.UserEmail ?? "";
                Quote.QuotationOpty = new optyQuote();
                Quote.QuotationOpty = oppty;
            }
            catch (IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = optyName, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SetOpportunityV2(string optyId, string optyName, string quoteId, string optyStage)
        {
            var result = true;
            string Err = "";
            var items = new List<string>();

            return Json(new { succeed = result, data = optyName, err = Err }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        public ActionResult updateQuotationFormat(string Company)
        {
            var result = true;
            string Err = "";
            var items = new List<string>();
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            try
            {
                Quote.QuotationExtensionNew.CompanyTitle = Company;
            }
            catch (IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = "", err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult addOpportunity(string optyName, string quoteId, string optyStage)
        {
            if (AppContext.IsNewVersion)
                return AddOpportunityV2(optyName, quoteId, optyStage); 
            var result = true;
            string Err = "";
            var items = new List<string>();
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];

            try
            {
                optyQuote oppty = new optyQuote();
                oppty.optyId = "new ID";
                oppty.optyName = optyName ?? "";
                oppty.quoteId = quoteId ?? "";
                oppty.optyStage = optyStage ?? "";
                oppty.Opty_Owner_Email = AppContext.UserEmail ?? "";
                Quote.QuotationOpty = new optyQuote();
                Quote.QuotationOpty = oppty;
            }
            catch (IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = optyName, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddOpportunityV2(string optyName, string quoteId, string optyStage)
        {
            return Json(new { succeed = true, data = optyName, err = "" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetConfigRecord(string RootID, string OrgID, int Type)
        {
            var result = true;
            string Err = "";
            var List = new List<EasyUITreeNode>();
            if (!string.IsNullOrEmpty(RootID) && !string.IsNullOrEmpty(OrgID))
            {
                //Ryan 20180328 below codes for temporary used only, will need to be adjusted
                String SalesOrg = OrgID;
                if (OrgID.ToUpper().Equals("DL"))
                    SalesOrg = "EU80";

                List = Advantech.Myadvantech.DataAccess.DataCore.CBOMV2_ConfiguratorDAL.GetConfigRecord(RootID, SalesOrg, OrgID, Type);
            }

            return Json(new { succeed = result, data = List, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CheckCompatibility(string PartNo, string SelectedItem)
        {
            Tuple<bool, string> result = null;
            string Err = "";
            List<string> list = new List<string>();
            try
            {
                if (!string.IsNullOrWhiteSpace(PartNo) && !string.IsNullOrWhiteSpace(SelectedItem))
                {
                    //Prepare selected list
                    foreach (string item in SelectedItem.Split(','))
                    {
                        list.Add(item.Trim());
                    }
                }
                //Get/Set Cache
                List<PRODUCT_COMPATIBILITY> pcList = HttpContext.Cache["PRODUCT_COMPATIBILITY"] as List<PRODUCT_COMPATIBILITY>;
                if (pcList == null)
                {
                    pcList = Advantech.Myadvantech.DataAccess.MyAdvantechDAL.GetProductCompatibility(Compatibility.Incompatible);
                    HttpContext.Cache.Add("PRODUCT_COMPATIBILITY", pcList, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                }

                //Check compatibility
                result = Advantech.Myadvantech.DataAccess.DataCore.CBOMV2_ConfiguratorDAL.CheckCompatibility(PartNo.Trim(), list, pcList);
                return Json(new { Result = result.Item1, Message = (result.Item1 ? string.Format("This part - {0} is {1} with {2}.", PartNo, Compatibility.Incompatible.ToString().ToLower(), result.Item2) : string.Empty) }, JsonRequestBehavior.AllowGet);
            }
            catch (IndexOutOfRangeException e)
            {
                return Json(new { Result = false, Message = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetPriceATP(string ComponentName, string CompanyID, string ORG, string Currency, string ConfigQty)
        {
            var result = true;
            string Err = "";
            Advantech.Myadvantech.DataAccess.PriceATP objPriceATP = new Advantech.Myadvantech.DataAccess.PriceATP();
            try
            {
                if (!string.IsNullOrEmpty(ComponentName) && !string.IsNullOrEmpty(ConfigQty))
                {
                    //Get/Set Cache
                    string _ORG = HttpContext.Cache["ORG"] as string;
                    string _Plant = HttpContext.Cache["Plant"] as string;
                    if (_ORG != null)
                    {
                        if (_ORG != ORG)
                        {
                            _Plant = Advantech.Myadvantech.DataAccess.SAPDAL.GetPlantByOrg(ORG);
                            HttpContext.Cache.Add("Plant", _Plant, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                        }
                    }
                    else
                    {
                        HttpContext.Cache.Add("ORG", ORG, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                    }

                    if (string.IsNullOrWhiteSpace(_Plant))
                    {
                        _Plant = Advantech.Myadvantech.DataAccess.SAPDAL.GetPlantByOrg(ORG);
                        HttpContext.Cache.Add("Plant", _Plant, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                    }

                    objPriceATP = Advantech.Myadvantech.DataAccess.SAPDAL.GetPriceATP(ComponentName, CompanyID, ORG, _Plant, Currency, Convert.ToInt32(ConfigQty));
                }
            }
            catch (IndexOutOfRangeException e)
            {
                result = false;
                Err = e.ToString();
            }

            return Json(new { succeed = result, data = objPriceATP, err = Err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Send(string QuoteId, string QuoteNo, string Type, string R_Email, string Subject, string mailbody)
        {
            if(AppContext.IsNewVersion)
                return RedirectToAction("SendV2", "Quotes", new { quoteId = QuoteId, quoteNo = QuoteNo, type = Type, reciverEmail = R_Email, subject =  Subject, mailbody =  mailbody });
            var result = true;
            string Err = "";

            try
            {
                var oMail = new System.Net.Mail.MailMessage();

                QuotationMaster QM = QuoteBusinessLogic.GetQuotationMaster(QuoteId);
                var salesRepresentative = QM.QuotationPartner.Where(s => s.TYPE == "E").FirstOrDefault();
                if (salesRepresentative != null && !string.IsNullOrEmpty(salesRepresentative.NAME))
                {
                    var salesEmail = salesRepresentative.NAME;
                    oMail.From = new System.Net.Mail.MailAddress(salesEmail);
                }
                else
                {
                    oMail.From = new System.Net.Mail.MailAddress("myadvantech@advantech.com");
                }
                
                var ToEmail = R_Email.Split(';');
                foreach (var email in ToEmail)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        oMail.To.Add(email);
                    }
                }
                oMail.Bcc.Add("myadvantech@advantech.com");

                oMail.Subject = Subject;

                
                QuotesViewModel QVModel = new QuotesViewModel();
                var QC = new QuotesController();
                QVModel = QC.setQuoteViewModel(QM);
                QVModel.pagetype = "SendMailToCustomer";
                // pass model to view and get html
                string template = "ACN_QuotationTemplate";
                if (!string.IsNullOrEmpty(AppContext.AppRegion))
                {
                    switch (AppContext.AppRegion)
                    {
                        case "ACN":
                            template = "ACN_QuotationTemplate";
                            break;
                        case "ABB":
                            template = "ABB_QuotationTemplate";
                            break;
                        case "ICG":
                            template = "ICG_QuotationTemplate";
                            break;
                        case "ADLOG":
                            template = "ADLOG_QuotationTemplate";
                            break;
                        default:
                            break;
                    }
                }
                var strHTML = RazorViewToString.RenderRazorViewToString(this, template, QVModel);
                var myContentByteAs = QuoteBusinessLogic.DownloadQuotePDFByHtmlString(strHTML);
                var MS = new MemoryStream(myContentByteAs);

                if (Type == "html")//html
                {
                    oMail.IsBodyHtml = true;
                    oMail.Body = "<div>" + mailbody + "</div><br />" + strHTML;
                }
                else //pdf
                {
                    oMail.IsBodyHtml = true;
                    oMail.Body = "<div>" + mailbody + "</div>";
                    oMail.Attachments.Add(new System.Net.Mail.Attachment(MS, string.Format("{0}.pdf", QuoteNo)));
                }

                //string smtp = WebConfigurationManager.AppSettings["SMTPServer"];
                string smtp = QuoteBusinessLogic.GetSMTPServerByRegion(AppContext.AppRegion); // alex 20180710 change get smtp by region
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

        [HttpPost]
        [Authorize]
        public ActionResult ChangeCurrentRegion(string region)
        {
            var result = true;
            string err = "";
            try
            {
                AppContext.AppRegion = region;
            }
            catch (Exception ex)
            {
                result = false;
                err = ex.Message;
            }
            return Json(new { succeed = result, data = "", err = err }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangeCurrentSector(string sector)
        {
            var result = true;
            string err = "";
            try
            {
                AppContext.AppSector = sector;
            }
            catch (Exception ex)
            {
                result = false;
                err = ex.Message;
            }
            return Json(new { succeed = result, data = "", err = err }, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult GetFreight(string shipToCountry, string shipToZipCode, string shipToState)
        {
            QuotationMaster Quote = new QuotationMaster();
            Quote = (QuotationMaster)System.Web.HttpContext.Current.Session["QuotationMaster"];
            if (AppContext.AppRegion == "ABB")
            {
                ShippingResult shippingResult = FreightCalculateBusinessLogic.CalculateBBFreight(shipToCountry, shipToZipCode, shipToState, Quote.quoteId, WebSource.eQuotation);
                return PartialView("_FreightOption", shippingResult);
            }
            else
                return HttpNotFound();


        }

    }

    public class PriceATP
    {
        public decimal Price { get; set; }
        public string ATPDate { get; set; }
        public int ATPQty { get; set; }
        public string CurrencySign { get; set; }
        public bool IsEw { get; set; }
    }

}