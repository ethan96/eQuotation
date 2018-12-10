//extern alias SAPDALVB;
using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Collections;
using System.Web.Caching;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Winnovative;
using Advantech.Myadvantech.DataAccess;
using Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model;

namespace Advantech.Myadvantech.Business
{

    /// <summary>
    /// Quote business layer
    /// </summary>
    public class QuoteBusinessLogic
    {
        /// <summary>
        /// 插入Quote转Order记录
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="QuoteID">QuoteID</param>
        /// <param name="Msg">返回信息</param>
        /// <returns></returns>
        public static bool LogQuote2Order(string OrderID, string QuoteID, ref string Msg)
        {
            return eQuotationDAL.LogQuote2Order(OrderID, QuoteID, ref Msg);
        }
        /// <summary>
        /// 判断arrary中的Part是否有特殊指定产品
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public static bool IsPartIn30ValidDaysLimitation(string[] products)
        {
            if (products.Length > 0)
            {
                foreach (string part in products)
                {
                    if (IsPTradePart(part)) return true;
                    if (part.StartsWith("TPC", StringComparison.InvariantCultureIgnoreCase)
                        || part.StartsWith("WOP", StringComparison.InvariantCultureIgnoreCase)
                        || part.StartsWith("SPC", StringComparison.InvariantCultureIgnoreCase)
                        || part.StartsWith("IPPC", StringComparison.InvariantCultureIgnoreCase)
                        || part.StartsWith("FPM", StringComparison.InvariantCultureIgnoreCase)
                        || part.StartsWith("IDS", StringComparison.InvariantCultureIgnoreCase)
                        || part.StartsWith("IDK", StringComparison.InvariantCultureIgnoreCase)
                        )
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断时候是Ptrade Part
        /// </summary>
        /// <param name="PartNo"></param>
        /// <returns></returns>
        public static bool IsPTradePart(string PartNo)
        {
            // Part _part = PartBusinessLogic.GetPartWithBasicModelInformation(PartNo);
            //目前没有看到属性PRODUCT_TYPE 去SAPProduct取资料
            //   string ProdType = _part.SAPDescription;
            eQuotationDAL _eQuotationDAL = new eQuotationDAL();
            string ProdType = _eQuotationDAL.getProductType(PartNo);
            if (ProdType == "ZPER" || ((ProdType == "ZFIN" | ProdType == "ZOEM") && (PartNo.StartsWith("BT") | PartNo.StartsWith("DSD") | PartNo.StartsWith("ES") | PartNo.StartsWith("EWM") | PartNo.StartsWith("GPS") | PartNo.StartsWith("SQF") | PartNo.StartsWith("WIFI") | PartNo.StartsWith("PMM") | PartNo.StartsWith("Y"))) || (ProdType == "ZRAW" & PartNo.StartsWith("206Q")) || (ProdType == "ZSEM" && PartNo.StartsWith("968Q")))
            {
                return true;
            }
            return false;
        }
        public static bool InitQuotationApprovalExpirationFlow(string QuoteID, ref string msg)
        {
            eQuotationDAL _eQuotationDAL = new eQuotationDAL();
            QuotationMaster _QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            if (_QuotationMaster.QuoteExtensionX.ApprovalFlowTypeX == eQApprovalFlowType.ThirtyDaysExpiration)
            {

                String officecode = String.Empty;
                String groupcode = String.Empty;

                //_eQuotationDAL.getOfficeGroupByEripIdOrg(_QuotationMaster.quoteToRowId, _QuotationMaster.quoteToErpId, _QuotationMaster.org, out officecode, out groupcode, ref msg);

                String _SalesCode = GetSalesCodeByQuoteID(QuoteID);
                if (UserRoleBusinessLogic.IsBBIrelandBySalesID(_SalesCode))
                {
                    UserRoleBusinessLogic.GetSalesOfficeAndGroupCodeBySalesCode(_SalesCode, out officecode, out groupcode);
                }
                else
                {
                    _eQuotationDAL.getOfficeGroupByEripIdOrg(_QuotationMaster.quoteToRowId, _QuotationMaster.quoteToErpId, _QuotationMaster.org, out officecode, out groupcode, ref msg);
                }

                IEnumerable<GPBLOCK_LOGIC> _ApproverList = _eQuotationDAL.getApproverList(officecode, groupcode, ref msg);
                if (_ApproverList != null)
                {
                    _QuotationMaster.QuoteExtensionX.delete(QuoteID);
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
                        eQuotationContext.Current.Entry(qae).State = System.Data.Entity.EntityState.Added;
                        if (i == 1) break;
                        i++;
                    }
                    eQuotationContext.Current.SaveChanges();
                }


            }
            return true;
        }

        public static List<QuotationMaster> GetMyQuotes(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                return (new QuotationMasterHelper()).GetMyQuotes(UserID);
            }
            return null;
        }

        public static String GetEQOrderNumber(String _Prefix, Boolean _isTesting)
        {
            String tablename = _isTesting == true ? "OrderNoStaging" : "OrderNo";
            String str = string.Format("begin tran SELECT num+1 as num FROM " + tablename + " with (tablockx) where prefix='{0}' update " + tablename + " set num=num+1 where prefix='{0}' commit tran", _Prefix);
            var NUM = SqlProvider.dbExecuteScalar("MY", str);

            int forparse = 0;
            if (NUM != null && Int32.TryParse(NUM.ToString(), out forparse))
            {
                return _Prefix + Convert.ToDouble(NUM).ToString("000000");
            }
            else
                return "";
        }


        /// <summary>
        /// Get quotation master information
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        public static QuotationMaster GetQuotationMaster(string quoteID)
        {
            if (!string.IsNullOrEmpty(quoteID))
            {
                return (new QuotationMasterHelper()).GetQuotationMaster(quoteID);
            }
            return null;
        }

        /// <summary>
        /// Get QuotationMaster data for eStore web service
        /// </summary>
        /// <param name="quoteID"></param>
        /// <param name="status"></param>
        /// <returns>QuotationMaster</returns>
        public static QuotationMaster GetQuotationMasterForeStore(string quoteID, QuoteDocStatus status = QuoteDocStatus.All)
        {
            if (!string.IsNullOrEmpty(quoteID))
            {
                return (new QuotationMasterHelper()).GetQuotationMasterForeStore(quoteID, status);
            }
            return null;
        }

        /// <summary>
        /// Get quotation detail(product list) information
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        public static List<QuotationDetail> GetQuotationDetail(string quoteID)
        {
            QuotationDetailHelper _QuotationDetailHelper = new QuotationDetailHelper();
            return _QuotationDetailHelper.GetQuotationDetail(quoteID);
        }

        public static List<PRODUCT_DEPENDENCY> getProductDependencyByPartNo(string PartNo)
        {
            MyAdvantechDAL _My = new MyAdvantechDAL();
            List<PRODUCT_DEPENDENCY> pd = _My.getProductDependencyByPartNo(PartNo);
            return pd;
        }

        public static void RefreshPartInventoryOfUSAOnline(String quoteID, int LineNo)
        {

            String _Plant = "USH1";
            String _RefPlant = "TWH1";
            Part _part = null;

            QuotationDetailHelper _QuotationDetailHelper = new QuotationDetailHelper();

            PartBusinessLogic.ShippingVia _ShipVia = PartBusinessLogic.ShippingVia.NA;
            QuotationMaster QM = GetQuotationMaster(quoteID);
            List<QuotationDetail> QDList = null;
            int PostPoneDays = 0;

            //Get all quote items or one quote item
            if (LineNo < 0)
            {
                QDList = GetQuotationDetail(quoteID);
                if (QDList == null || QDList.Count == 0) { return; }
            }
            else
            {
                QuotationDetail qd = _QuotationDetailHelper.GetQuotationDetailItem(quoteID, LineNo);
                if (qd == null) { return; }
                QDList = new List<QuotationDetail>();
                QDList.Add(qd);
            }



            List<Part> _QueryPartInventoryList = new List<Part>();
            List<string> _addedpartlist = new List<string>();
            foreach (QuotationDetail QDItem in QDList)
            {
                if (_addedpartlist.Contains(QDItem.partNo)) { continue; }
                if (QDItem.ItemTypeX == QuoteItemType.BtosParent) { continue; }
                if (QDItem.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase)) { continue; }

                _RefPlant = PartBusinessLogic.GetReferencePlantBySpecialProcurement(QDItem.partNo, _Plant);
                //20150729 Jay said that do not need to add postpone days to available date
                _ShipVia = PartBusinessLogic.GetShippingVia(QDItem.partNo, _Plant);
                switch (_ShipVia)
                {
                    case PartBusinessLogic.ShippingVia.Air:
                        PostPoneDays = 14;
                        break;
                    case PartBusinessLogic.ShippingVia.Sea:
                        PostPoneDays = 42;
                        break;
                    case PartBusinessLogic.ShippingVia.NA:
                        PostPoneDays = 0;
                        break;
                }
                _part = new Part();
                _part.PartNumber = QDItem.partNo;
                _part.PlantID = _Plant;
                _QueryPartInventoryList.Add(_part);

                _part = new Part();
                _part.PartNumber = QDItem.partNo;
                _part.PlantID = _RefPlant;
                _QueryPartInventoryList.Add(_part);

                _addedpartlist.Add(QDItem.partNo);

            }



            //Get inventory information of all parts in quote
            string _errmsg = string.Empty;
            List<Inventory> PartsInventoryList = Advantech.Myadvantech.DataAccess.SAPDAL.GetInventory(_QueryPartInventoryList, PostPoneDays, ref _errmsg);

            string _codebyorgid = "TW";
            if (!string.IsNullOrEmpty(QM.org) && QM.org.Length > 2)
            {
                _codebyorgid = Advantech.Myadvantech.DataAccess.SAPDAL.GetCalendarIDbyOrg(QM.org.Substring(0, 2));
            }

            //Group part by partno for getting part's total request qty
            var _GroupPartList = from p in QDList
                                 group p by p.partNo into g
                                 select new
                                 {
                                     PartNumber = g.Key,
                                     PartTotalQty = g.Sum(x => x.qty)
                                 };


            // Frank 2012/09/18
            // 1.component
            // 1.1 if available qty can not be found，then available date need to be changed to 12/31/9999；set available qty=0
            // 1.2 if available qty is not enough for request qty, then available date need to be changed to 12/31/9999；and set available qty as real stock
            // 2.BTOS 100 line：No need to query available qty，but available date need to be updated by lastest available date(due date) of sub components；and set available qty as request qty
            // 3.If part's pref string is AGS-：No need to query available qty and set available date as today, set available qty as request qty


            foreach (var _GroupedPart in _GroupPartList)
            {

                var _PartInventorys = from p in PartsInventoryList
                                      where p.PartNumber == _GroupedPart.PartNumber
                                      orderby p.AvailableDate
                                      select p;

                //rule 1.1
                int PartRealInventory = 0;
                DateTime PartRealAvailableDate = DateTime.Parse("9999/12/31");
                int _CurrentyTotalCount = 0;
                bool _IsHaveStockAndEnough = false;
                if (_PartInventorys.Count() > 0)
                {
                    foreach (Inventory _inventoryobj in _PartInventorys)
                    {
                        //identify which available date can fulfil the request qty
                        _CurrentyTotalCount += _inventoryobj.AvailableQty;
                        if (_CurrentyTotalCount >= _GroupedPart.PartTotalQty)
                        {
                            _IsHaveStockAndEnough = true;
                            //PartRealInventory = _CurrentyTotalCount;
                            PartRealAvailableDate = _inventoryobj.AvailableDate;

                            Advantech.Myadvantech.DataAccess.SAPDAL.Get_Next_WorkingDate_ByCode(ref PartRealAvailableDate, 0, _codebyorgid);

                            break;
                        }
                    }
                }

                var _UpdateQuoteItems = from c in QDList
                                        where c.partNo == _GroupedPart.PartNumber
                                        select c;

                foreach (QuotationDetail _UpdatedQuoteItem in _UpdateQuoteItems)
                {
                    //by default part's inventory equals request qty
                    PartRealInventory = _UpdatedQuoteItem.qty.GetValueOrDefault();

                    //rule 1.2
                    if (_PartInventorys.Count() > 0 && !_IsHaveStockAndEnough)
                    {
                        PartRealAvailableDate = DateTime.Parse("9999/12/31");
                        PartRealInventory = _CurrentyTotalCount;
                    }
                    //rule 2
                    if (_UpdatedQuoteItem.ItemTypeX == QuoteItemType.BtosParent)
                    {
                        PartRealInventory = _UpdatedQuoteItem.qty.GetValueOrDefault();
                    }
                    //rule 3
                    if (_UpdatedQuoteItem.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase))
                    {
                        PartRealAvailableDate = DateTime.Now;
                        PartRealInventory = _UpdatedQuoteItem.qty.GetValueOrDefault();
                    }

                    _UpdatedQuoteItem.inventory = PartRealInventory;
                    _UpdatedQuoteItem.dueDate = PartRealAvailableDate;
                    _QuotationDetailHelper.UpdateQuotationDetail(_UpdatedQuoteItem);
                }

            }

            // update btos parent item
            foreach (QuotationDetail QDItem in QDList)
            {
                if (QDItem.ItemTypeX == QuoteItemType.BtosParent)
                {
                    var dd = (from qdl in QDList where qdl.HigherLevel == QDItem.line_No select qdl).Max(x => x.dueDate);
                    if (dd != null)
                    {
                        QDItem.dueDate = dd;
                        _QuotationDetailHelper.UpdateQuotationDetail(QDItem);
                    }
                }
            }
        }

        /// <summary>
        /// Check US AOnline sales district
        /// </summary>
        /// <param name="SalesID"></param>
        /// <param name="District"></param>
        /// <returns></returns>
        public static bool CheckANASalesDistrict(string SalesID, string District)
        {
            bool result = false;
            try
            {
                StringBuilder sb = new StringBuilder();
                //ICC 2015/10/28 Change table to AUS_AOnLine_SALES_DISTRICT
                //sb.AppendFormat(" SELECT COUNT(*) FROM AUS_AOnLine_SALES_DISTRICT WHERE YYYYMM = '{0}' ", DateTime.Now.ToString("yyyyMM"));
                //Frank 2015/10/29 Check all history settings
                sb.AppendFormat(" SELECT COUNT(*) FROM AUS_AOnLine_SALES_DISTRICT ");
                sb.AppendFormat(" WHERE SalesId = '{0}' AND District = '{1}' ", SalesID, District);
                object count = SqlProvider.dbExecuteScalar("EQ", sb.ToString());

                if (count != null && Convert.ToInt32(count) > 0)
                    result = true;
            }
            catch
            {

            }
            return result;
        }

        /// <summary>
        /// Check US AOnline IAG sales district
        /// </summary>
        /// <param name="SalesID"></param>
        /// <param name="District"></param>
        /// <returns></returns>
        public static bool CheckUSAOnlineIagDistrict(string SalesID, string District)
        {
            bool result = false;
            try
            {
                object count = SqlProvider.dbExecuteScalar("EQ", string.Format(" select count(*) from AUS_AOnLine_IAG_SALES_DISTRICT where sales_id = '{0}' and district = '{1}' and GETDATE() between eff_from and eff_to ", SalesID, District));
                if (count != null && Convert.ToInt32(count) > 0)
                    result = true;
            }
            catch
            {

            }
            return result;
        }


        public static bool CheckSalesDistrict(string SalesID, string District)
        {
            bool result = false;
            try
            {
                object count = SqlProvider.dbExecuteScalar("CP", string.Format(" select count(*) from US_COMM_SALES_DISTRICT where sales_id = '{0}' and district = '{1}' and GETDATE() between eff_from and eff_to ", SalesID, District));
                if (count != null && Convert.ToInt32(count) > 0)
                    result = true;
            }
            catch
            {

            }
            return result;
        }

        public static string GetQuotationRemark(string quoteId)
        {
            string remarkResult = "";
            try
            {
                object remark = SqlProvider.dbExecuteScalar("EQ", string.Format(" select top 1 notetext from QuotationNote where quoteid = '{0}' and notetype = 'Remark' ", quoteId));
                if (remark != null)
                    remarkResult =  remark.ToString();
            }
            catch { }
            return remarkResult;
        }


        public static QuotationMaster CopyQuotation(String _QuoteID, String _NewQuoteID, String _NewQuoteNo, String _SalesEmail, decimal taxRate, int _RevisionNumber = 1)
        {
            QuotationMaster QM = (new QuotationMasterHelper()).GetQuotationMaster(_QuoteID);
            eQuotationContext.Current.Entry(QM).State = System.Data.Entity.EntityState.Detached;
            QM.quoteId = _NewQuoteID;
            QM.quoteNo = _NewQuoteNo;
            QM.quoteDate = DateTime.Now;
            if (QM.org == "US10")
                QM.expiredDate = DateTime.Now.AddDays(90);
            else
                QM.expiredDate = DateTime.Now.AddMonths(1);
            QM.createdBy = _SalesEmail;
            QM.createdDate = DateTime.Now;
            QM.LastUpdatedBy = _SalesEmail;
            QM.LastUpdatedDate = DateTime.Now;
            QM.deliveryDate = DateTime.Now.AddDays(3);
            QM.reqDate = DateTime.Now;
            QM.DOCSTATUS = 0;
            QM.Revision_Number = (short)_RevisionNumber;
            QM.PODate = DateTime.Now;
            QM.OriginalQuoteID = _QuoteID;
            QM.tax = taxRate;
            QM.relatedInfo = "";
            QM.Remark = GetQuotationRemark(_QuoteID);
            eQuotationContext.Current.QuotationMaster.Add(QM);


            QuotationExtensionNew QuotationExtensionNew = eQuotationContext.Current.QuotationExtensionNew.Where(x => x.QuoteID == _QuoteID).FirstOrDefault();
            QuotationExtensionNew.GeneralRate = 0.24m;
            QuotationExtensionNew.QuoteID = _NewQuoteID;
            QuotationExtensionNew.FinishDate = null;
            QuotationExtensionNew.NewExpiredDate = null;
            QuotationExtensionNew.BelowGP = false;
            eQuotationContext.Current.QuotationExtensionNew.Add(QuotationExtensionNew);


            List<QuotationDetail> QDs = (new QuotationDetailHelper()).GetQuotationDetail(_QuoteID);
            foreach (QuotationDetail qd in QDs)
            {
                eQuotationContext.Current.Entry(qd).State = System.Data.Entity.EntityState.Detached;
                qd.quoteId = _NewQuoteID;
                qd.sprNo = null;
            }



            eQuotationContext.Current.QuotationDetail.AddRange(QDs);

            List<EQPARTNER> QPs = (new QuotationPartnerHelper()).GetQuotationPartner(_QuoteID);
            foreach (EQPARTNER e in QPs)
            {
                eQuotationContext.Current.Entry(e).State = System.Data.Entity.EntityState.Detached;
                e.QUOTEID = _NewQuoteID;
            }
            eQuotationContext.Current.EQPARTNER.AddRange(QPs);

            // Quotation Opty
            optyQuote opty = (new OptyQuoteHelper()).GetOptyQuote(_QuoteID);
            if (opty != null)
            {
                eQuotationContext.Current.Entry(opty).State = System.Data.Entity.EntityState.Detached;
                opty.quoteId = _NewQuoteID;
                eQuotationContext.Current.optyQuote.Add(opty);
            }



            eQuotationContext.Current.SaveChanges();

            //Copy ConfigurationRecords for reconfig purpose.
            var newConfiguredRecords = MyAdvantechDAL.GetHubConfiguredResults(_QuoteID);
            if (newConfiguredRecords != null && newConfiguredRecords.Count() > 0)
            {
                foreach (var record in newConfiguredRecords)
                {
                    HubConfiguredResult newRecord = new HubConfiguredResult();
                    newRecord.ID = QM.quoteId;
                    newRecord.CompanyID = record.CompanyID;
                    newRecord.Currency = record.Currency;
                    newRecord.ParentLineNo = record.ParentLineNo;
                    newRecord.ReConfigData = record.ReConfigData;
                    newRecord.Result = record.Result;
                    newRecord.RootID = record.RootID;
                    newRecord.SelectedItems = record.SelectedItems;
                    newRecord.Source = record.Source;
                    newRecord.CreatedTime = DateTime.Now;
                    MyAdvantechDAL.AddConfigurationHubConfiguredRecords(newRecord);
                }
            }

            return QM;
        }

        public static QuotationMaster ReviseQuotation(String _QuoteID, String _NewQuoteID, String _CurrentUserMail, int _RevisionNumber = 1)
        {
            QuotationMaster QM = (new QuotationMasterHelper()).GetQuotationMaster(_QuoteID);
            eQuotationContext.Current.Entry(QM).State = System.Data.Entity.EntityState.Detached;
            QM.quoteId = _NewQuoteID;
            QM.quoteNo = QM.quoteNo;

            QM.createdBy = _CurrentUserMail;
            QM.createdDate = DateTime.Now;
            QM.LastUpdatedBy = _CurrentUserMail;
            QM.LastUpdatedDate = DateTime.Now;

            QM.DOCSTATUS = 0;
            QM.Revision_Number = (short)_RevisionNumber;
            QM.OriginalQuoteID = _QuoteID;
            QM.Remark = GetQuotationRemark(_QuoteID);
            eQuotationContext.Current.QuotationMaster.Add(QM);


            QuotationExtensionNew QuotationExtensionNew = eQuotationContext.Current.QuotationExtensionNew.Where(x => x.QuoteID == _QuoteID).FirstOrDefault();
            QuotationExtensionNew.QuoteID = _NewQuoteID;
            QuotationExtensionNew.FinishDate = null;
            QuotationExtensionNew.BelowGP = false;
            eQuotationContext.Current.QuotationExtensionNew.Add(QuotationExtensionNew);


            List<QuotationDetail> QDs = (new QuotationDetailHelper()).GetQuotationDetail(_QuoteID);
            foreach (QuotationDetail qd in QDs)
            {
                eQuotationContext.Current.Entry(qd).State = System.Data.Entity.EntityState.Detached;
                qd.quoteId = _NewQuoteID;
            }



            eQuotationContext.Current.QuotationDetail.AddRange(QDs);

            List<EQPARTNER> QPs = (new QuotationPartnerHelper()).GetQuotationPartner(_QuoteID);
            foreach (EQPARTNER e in QPs)
            {
                eQuotationContext.Current.Entry(e).State = System.Data.Entity.EntityState.Detached;
                e.QUOTEID = _NewQuoteID;
            }
            eQuotationContext.Current.EQPARTNER.AddRange(QPs);

            // Quotation Opty
            optyQuote opty = (new OptyQuoteHelper()).GetOptyQuote(_QuoteID);
            if (opty != null)
            {
                eQuotationContext.Current.Entry(opty).State = System.Data.Entity.EntityState.Detached;
                opty.quoteId = _NewQuoteID;
                eQuotationContext.Current.optyQuote.Add(opty);
            }



            eQuotationContext.Current.SaveChanges();

            //Copy ConfigurationRecords for reconfig purpose.
            var newConfiguredRecords = MyAdvantechDAL.GetHubConfiguredResults(_QuoteID);
            if (newConfiguredRecords != null && newConfiguredRecords.Count() > 0)
            {
                foreach (var record in newConfiguredRecords)
                {
                    HubConfiguredResult newRecord = new HubConfiguredResult();
                    newRecord.ID = QM.quoteId;
                    newRecord.CompanyID = record.CompanyID;
                    newRecord.Currency = record.Currency;
                    newRecord.ParentLineNo = record.ParentLineNo;
                    newRecord.ReConfigData = record.ReConfigData;
                    newRecord.Result = record.Result;
                    newRecord.RootID = record.RootID;
                    newRecord.SelectedItems = record.SelectedItems;
                    newRecord.Source = record.Source;
                    newRecord.CreatedTime = DateTime.Now;
                    MyAdvantechDAL.AddConfigurationHubConfiguredRecords(newRecord);
                }
            }

            return QM;
        }
        #region Email

        public static string GetDefaultSignature(string UserID)
        {

            EmailGreeting _eg = eQuotationContext.Current.EmailGreeting.Find(UserID);
            if (_eg != null)
            {
                return _eg.Greeting;
            }
            return string.Empty;
        }

        public static void SaveDefaultSignature(string UserID, string Greeting)
        {

            //EmailGreeting _eg = eQuotationContext.Current.EmailGreeting.Find(UserID);

            EmailGreeting _eg = eQuotationContext.Current.EmailGreeting.Where(x => x.UserID == UserID).FirstOrDefault();


            if (_eg == null)
            {
                _eg = new EmailGreeting();
                _eg.UserID = UserID;
                _eg.Greeting = Greeting;
                //eQuotationContext.Current.EmailGreeting.Add(_eg);
                eQuotationContext.Current.Entry(_eg).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                EmailGreeting _updateEG = new EmailGreeting();
                _updateEG.UserID = UserID;
                _updateEG.Greeting = Greeting;
                //_eg.Greeting = Greeting;
                //eQuotationContext.Current.EmailGreeting.Attach(_updateEG);
                //eQuotationContext.Current.Entry(_updateEG).State = System.Data.Entity.EntityState.Modified;

                eQuotationContext.Current.Entry(_eg).CurrentValues.SetValues(_updateEG);
                //db.SaveChanges();


            }
            eQuotationContext.Current.SaveChanges();
        }

        #endregion
        #region GP Approve

        public static bool IsNeedGPforANAAOnline(string QuoteID)
        {
            List<QuotationDetail> items = GetQuotationDetail(QuoteID);

            foreach (QuotationDetail _item in items)
            {
                if (_item.newUnitPrice < _item.newItp)
                {
                    return true;
                }
            }
            return false;
        }


        public static decimal GetANAPartGPBySAPGPBlockRFC(string QuoteID, string PartNo, string DeliveryPlant)
        {
            QuotationMaster _QMaster = GetQuotationMaster(QuoteID);

            List<QuotationDetail> _QDetail = GetQuotationDetail(QuoteID);

            Order _order = new Order();

            _order.Currency = _QMaster.currency;
            _order.DistChannel = _QMaster.DIST_CHAN;
            _order.Division = _QMaster.DIVISION;
            _order.OrgID = _QMaster.org;

            _order.AddLooseItem(PartNo, DeliveryPlant, 1, 1);
            _order.LineItems[0].PlantID = DeliveryPlant;

            List<EQPARTNER> EQPList = eQuotationDAL.GetQuotePartnerByQuoteID(QuoteID);

            EQPARTNER sold_to_party = EQPList.Where(t => t.TYPE == "S").FirstOrDefault();

            if (EQPList != null && EQPList.Count > 0 && !String.IsNullOrEmpty(sold_to_party.ERPID))
            {
                foreach (EQPARTNER PartnerRow in EQPList)
                {
                    switch (PartnerRow.TYPE)
                    {
                        case "S":
                            _order.SetOrderPartnet(new OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.ShipTo));
                            break;
                        case "B":
                            _order.SetOrderPartnet(new OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.BillTo));
                            break;
                        case "SOLDTO":
                            _order.SetOrderPartnet(new OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.SoldTo));
                            break;
                    }
                }
            }
            else
            {
                //If quote-to-erpid is empty or cannot be found, then use eStore price inquiry erpid instead.
                //UEPP5001
                _order.SetOrderPartnet(new OrderPartner("UEPP5001", "US01", OrderPartnerType.ShipTo));
                _order.SetOrderPartnet(new OrderPartner("UEPP5001", "US01", OrderPartnerType.BillTo));
                _order.SetOrderPartnet(new OrderPartner("UEPP5001", "US01", OrderPartnerType.SoldTo));

            }


            string _errmsg = String.Empty;

            Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrderGPBlock(ref _order, ref _errmsg);

            if (_order.LineItems != null && _order.LineItems.Count > 0)
            {
                return _order.LineItems[0].ITP;
            }
            else
            {
                return 0;
            }


        }



        //public static void WriteACNQuoteLinesITP(string QuoteID, ref string msg)
        //{
        //    var items = GetQuotationDetail(QuoteID);
        //    SAPDAL.SAPDAL.GPInfo GPInfoAENC = SAPDAL.SAPDAL.CalcACNQuoteGP(QuoteID);
        //    ArrayList array = new ArrayList();
        //    foreach (var _quoteitem in items)
        //    {
        //        var _cond = GPInfoAENC.LineItems.FirstOrDefault(p => p.PartNo == _quoteitem.partNo);
        //        if (_cond != null)
        //        {
        //            _quoteitem.itp = _cond.Cost;
        //            _quoteitem.newItp = _cond.Cost;
        //        }
        //    }
        //    eQuotationContext.Current.SaveChanges();
        //}


        public static bool IsNeedGPforAENC(string QuoteID)
        {
            //string msg = string.Empty;
            //List<QuotationDetail> items = GetQuotationDetail(QuoteID);
            //decimal TotalAmount = 0;
            //decimal TotalCost = 0;

            SAPDAL.SAPDAL.GPInfo GPInfoAENC = SAPDAL.SAPDAL.CalcAENCQuoteGP(QuoteID);

            if (GPInfoAENC != null)
            {
                foreach (SAPDAL.SAPDAL.DocumentLine _item in GPInfoAENC.LineItems)
                {
                    if (_item.GPBlock)
                    {
                        return true;
                    }
                }
            }
            return false;

            //foreach (QuotationDetail item in items)
            //{
            //    //Exclude service parts and parent items
            //    if (item.IsServicePartX) { continue; }
            //    if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { continue; }

            //    TotalAmount += item.newUnitPrice.Value * item.qty.Value;

            //    SAPDAL.SAPDAL.PN_COND PNC = Conditions.FirstOrDefault(P => P.PART_NO == item.partNo);
            //    if (PNC != null)
            //    {
            //        TotalCost += PNC.COND_VAL * item.qty.Value;
            //    }
            //    else
            //    {
            //        TotalCost += item.newUnitPrice.Value * item.qty.Value;
            //    }
            //}

            //DataTable dt = eQuotationDAL.getGP_Parameter("AKR");
            //double _GPpercent = 0.30;
            //if (dt.Rows.Count == 1 && !string.IsNullOrEmpty(dt.Rows[0]["GP_Percent"].ToString()))
            //{
            //    double.TryParse(dt.Rows[0]["GP_Percent"].ToString().Trim(), out _GPpercent);
            //}

            //if (TotalCost > 0 && (TotalAmount - TotalCost) / TotalCost <= (decimal)_GPpercent)
            //{
            //    return true;
            //}
            //return false;
        }



        //public static bool IsNeedGPforARK(string QuoteID)
        //{
        //    string msg = string.Empty;
        //    List<QuotationDetail> items = GetQuotationDetail(QuoteID);
        //    //decimal TotalAmount = items.Count > 0 ? items.Sum(p => p.newUnitPrice * p.qty).Value : 0;
        //    decimal TotalAmount = 0;



        //    decimal TotalCost = 0;
        //    List<SAPDAL.SAPDAL.PN_COND> Conditions = SAPDAL.SAPDAL.GetAKRPN_Cost_GPPercent(QuoteID, ref msg);
        //    foreach (QuotationDetail item in items)
        //    {
        //        //Exclude service parts and parent items
        //        if (item.IsServicePartX) { continue; }
        //        if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { continue; }

        //        TotalAmount += item.newUnitPrice.Value * item.qty.Value;

        //        SAPDAL.SAPDAL.PN_COND PNC = Conditions.FirstOrDefault(P => P.PART_NO == item.partNo);
        //        if (PNC != null)
        //        {
        //            TotalCost += PNC.COND_VAL * item.qty.Value;
        //        }
        //        else
        //        {
        //            TotalCost += item.newUnitPrice.Value * item.qty.Value;
        //        }
        //    }

        //    DataTable dt = eQuotationDAL.getGP_Parameter("AKR");
        //    double _GPpercent = 0.30;
        //    if (dt.Rows.Count == 1 && !string.IsNullOrEmpty(dt.Rows[0]["GP_Percent"].ToString()))
        //    {
        //        double.TryParse(dt.Rows[0]["GP_Percent"].ToString().Trim(), out _GPpercent);
        //    }

        //    if (TotalCost > 0 && (TotalAmount - TotalCost) / TotalCost <= (decimal)_GPpercent)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public static List<QuotationDetail> RemoveServicePartInQuotationDetails(List<QuotationDetail> quotationDetails)
        {
            return quotationDetails.Where(qd => PartBusinessLogic.IsServicePart2(qd.partNo) == false).ToList();

        }


        public static decimal GetTotalMarginByQuotationDetails(String QuoteID)
        {
            List<QuotationDetail> items = GetQuotationDetail(QuoteID);
            decimal _TotalMargin = 0;
            return GetTotalMarginByQuotationDetails(items);
        }

        public static decimal GetTotalMarginByQuotationDetails(List<QuotationDetail> _QuotationDetails)
        {
            decimal r = -99999;
            decimal sumAmt = 0;
            decimal sumITP = 0;

            //Alex 20180119, remove service part in total margin calculation 
            var quotationDetailsWithoutServicePart = RemoveServicePartInQuotationDetails(_QuotationDetails);


            foreach (QuotationDetail qd in quotationDetailsWithoutServicePart)
            {
                sumAmt += (Decimal)(qd.newUnitPrice * qd.qty);
                sumITP += (Decimal)(qd.newItp * qd.qty);
            }
            if (sumAmt != 0)
            {
                r = (sumAmt - sumITP) / sumAmt;
            }
            return r;
        }




        public static bool IsNeedGPforARK(string QuoteID)
        {
            //string msg = string.Empty;
            List<QuotationDetail> items = GetQuotationDetail(QuoteID);
            decimal _TotalMargin = 0;
            _TotalMargin = GetTotalMarginByQuotationDetails(items);
            DataTable dt = eQuotationDAL.getGP_Parameter("AKR");
            double _GPpercent = 0.30;
            if (dt.Rows.Count == 1 && !string.IsNullOrEmpty(dt.Rows[0]["GP_Percent"].ToString()))
            {
                double.TryParse(dt.Rows[0]["GP_Percent"].ToString().Trim(), out _GPpercent);
            }

            // Ryan 20171102 New AKR GP percent logic
            // Team_Name        GP%
            // EIoT             25%
            // EIoT Aonline     30%
            // IIoT AOnline     30%
            // IIoT CSF         25%
            // IIoT KA          25%
            // SIoT             25%
            String _sales = QuoteBusinessLogic.GetSalesEmailByQuoteID(QuoteID);
            var objTeamName = SqlProvider.dbExecuteScalar("EQ", String.Format("SELECT TOP 1 Team_Name as TeamName FROM AKR_Sales_List WHERE [Sales_email]= '{0}'", _sales));
            if (objTeamName != null && !String.IsNullOrEmpty(objTeamName.ToString()))
            {
                if (objTeamName.ToString().Equals("EIoT", StringComparison.OrdinalIgnoreCase) ||
                    objTeamName.ToString().Equals("IIoT CSF", StringComparison.OrdinalIgnoreCase) ||
                    objTeamName.ToString().Equals("IIoT KA", StringComparison.OrdinalIgnoreCase) ||
                    objTeamName.ToString().Equals("SIoT", StringComparison.OrdinalIgnoreCase))
                {
                    _GPpercent = 0.25;
                }
            }

            if (_TotalMargin <= (decimal)(_GPpercent))
            {
                return true;
            }
            return false;
        }

        //public static bool IsQuoteItemBelowMinimumPrice(string QuoteID)

        public static Order SimulateQuotePrice(string QuoteID, Boolean IsReturnSystemFormat)
        {

            //Get Quotation Master
            QuotationMaster Master = GetQuotationMaster(QuoteID);
            //Get Quotation Item list
            List<QuotationDetail> items = GetQuotationDetail(QuoteID);

            // Ryan 20160901 Order simulation process
            Boolean isBelowMinPrice = false;
            Decimal MaxGPPercent = 0;
            Order _order = new Order();
            _order.Currency = Master.currency;
            _order.OrgID = Master.org;
            _order.DistChannel = Master.DIST_CHAN;
            _order.Division = Master.DIVISION;

            //Frank 20170703:Sabrina suggests that please use quote simulation to get ZMIP
            _order.OrderType = SAPOrderType.ZOR;

            int i = 0;
            foreach (QuotationDetail _Quotelineitem in items)
            {


                if (IsReturnSystemFormat)
                {
                    switch (_Quotelineitem.ItemType)
                    {
                        case 0: //Parent item
                            _order.AddBTOSParentItem(Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(_Quotelineitem.partNo), _Quotelineitem.deliveryPlant, _Quotelineitem.line_No.GetValueOrDefault(100), _Quotelineitem.qty.GetValueOrDefault(1));
                            break;

                        case 1: //child item
                            //loose item
                            if (_Quotelineitem.HigherLevel == 0)
                            {
                                _order.AddLooseItem(Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(_Quotelineitem.partNo), _Quotelineitem.deliveryPlant, _Quotelineitem.line_No.GetValueOrDefault(100), _Quotelineitem.qty.GetValueOrDefault(1));
                            }
                            else // system child items
                            {
                                _order.AddBTOSChildItem(Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(_Quotelineitem.partNo), _Quotelineitem.HigherLevel.GetValueOrDefault(100), _Quotelineitem.deliveryPlant, _Quotelineitem.line_No.GetValueOrDefault(100), _Quotelineitem.qty.GetValueOrDefault(1));
                            }
                            break;
                        default:
                            _order.AddLooseItem(Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(_Quotelineitem.partNo), _Quotelineitem.deliveryPlant, _Quotelineitem.line_No.GetValueOrDefault(100), _Quotelineitem.qty.GetValueOrDefault(1));
                            break;
                    }

                }
                else
                {
                    _order.AddLooseItem(Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(_Quotelineitem.partNo), 1);
                    _order.LineItems[i].PlantID = _Quotelineitem.deliveryPlant;
                    i++;

                }


                //_order.AddLooseItem(Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(_Quotelineitem.partNo), 1);
                //_order.LineItems[i].PlantID = _Quotelineitem.deliveryPlant;
                //i++;
            }
            _order.SetOrderPartnet(new OrderPartner(Master.quoteToErpId, Master.org, OrderPartnerType.SoldTo));
            _order.SetOrderPartnet(new OrderPartner(Master.quoteToErpId, Master.org, OrderPartnerType.ShipTo));
            _order.SetOrderPartnet(new OrderPartner(Master.quoteToErpId, Master.org, OrderPartnerType.BillTo));
            String _errMsg = String.Empty;
            Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref _order, ref _errMsg);

            return _order;
        }

        public static bool IsNeedGPforANAIIoTAOnline(string QuoteID, ref decimal Rate, ref List<QuotationDetail> mitems)
        {

            string msg = string.Empty; bool isNeedGP = false; decimal minRate = 1;
            QuotationMaster Master = GetQuotationMaster(QuoteID);
            List<QuotationDetail> items = GetQuotationDetail(QuoteID);
            decimal _TotalMargin = 0;
            _TotalMargin = GetTotalMarginByQuotationDetails(items);
            DataTable _dtGPParas = eQuotationDAL.getGP_Parameter("ANA-IIoT-IAG-AOnline");
            double _GPpercent = 0.20;
            mitems = new List<QuotationDetail>();
            Decimal MaxGPPercent = 0;
            Boolean isBelowMinPrice = false; //here the minmun price is L2 price for ANA AOnline sales team
            Decimal L2Price = 0;
            SAPDAL.SAPDAL _OldSAPDAL = new SAPDAL.SAPDAL();

            foreach (QuotationDetail item in items)
            {
                //if (QDItem.ItemTypeX == QuoteItemType.BtosParent) { continue; }
                //if (item.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                if (item.IsServicePartX) { continue; }
                if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { continue; }
                if (!string.IsNullOrEmpty(item.sprNo)) { continue; }
                if (item.partNo.Equals("Freight_Fee", StringComparison.OrdinalIgnoreCase)) { continue; }
                //if(Advantech.Myadvantech.Business.PartBusinessLogic.IsServicePart(item.partNo,Master.org)) { continue; }

                // Ryan 20171025 Add for service part check through SQL select due to unknown context error in PartBusinessLogic.IsServicePart
                var objGENITEMCATGRP = SqlProvider.dbExecuteScalar("MY", String.Format("SELECT top 1 a.GENITEMCATGRP FROM SAP_PRODUCT a inner join SAP_PRODUCT_ORG b on a.PART_NO = b.PART_NO WHERE a.PART_NO = '{0}' and b.ORG_ID = '{1}'", item.partNo, Master.org));
                if (objGENITEMCATGRP != null && !String.IsNullOrEmpty(objGENITEMCATGRP.ToString()))
                {
                    if (objGENITEMCATGRP.ToString().ToUpper().Equals("DIEN") || objGENITEMCATGRP.ToString().ToUpper().Equals("LEIS"))
                        continue;
                }

                String errmsg = "";
                L2Price = _OldSAPDAL.getPriceByProdLine(item.partNo, _OldSAPDAL.getProdPricingGrp(item.partNo), ref errmsg);

                //Check if new unit price is lower than L2 price
                //If so then the quote needs to be approved by first line manager Matt.Dentino(IAG) / Daniel.Tom(ISG)
                if (item.newUnitPrice < L2Price)
                {
                    isBelowMinPrice = true;
                    isNeedGP = true;

                    Rate = (decimal)0.30;
                }
                else
                {
                    continue;
                }

                double _GPValueFromGPParameter = 0; decimal _QuoteItemGP = 0;
                if (item.newUnitPrice.Value > 0)
                {
                    _QuoteItemGP = (item.newUnitPrice.Value - item.newItp.Value) / item.newUnitPrice.Value;
                }
                else
                {
                    _QuoteItemGP = 0;
                }

                //Identify which part's GP is the lowest GP value and the GP approval level
                foreach (DataRow _GPParameterItemRow in _dtGPParas.Rows)
                {
                    double.TryParse(_GPParameterItemRow["GP_Percent"].ToString().Trim(), out _GPValueFromGPParameter);

                    if ((decimal)_GPValueFromGPParameter > MaxGPPercent)
                        MaxGPPercent = (decimal)_GPValueFromGPParameter;

                    if (_QuoteItemGP <= (decimal)_GPValueFromGPParameter)
                    {
                        var mitem = mitems.FirstOrDefault(p => p.partNo == item.partNo && p.line_No == item.line_No);
                        if (mitem == null)
                        {
                            mitems.Add(item);
                        }
                        if (_QuoteItemGP != 0 && minRate > _QuoteItemGP) minRate = _QuoteItemGP;
                        isNeedGP = true;
                    }
                    else
                    {
                        // although mRate is larger than mGPpercent, but is below min price, need to add in gp items list.
                        var mitem = mitems.FirstOrDefault(p => p.partNo == item.partNo && p.line_No == item.line_No);
                        if (mitem == null)
                        {
                            mitems.Add(item);
                        }
                    }
                }
            }

            Rate = minRate;
            if (isBelowMinPrice)
            {
                if (Rate > MaxGPPercent)
                    Rate = MaxGPPercent;
            }

            return isNeedGP;

        }

        private static String ConvertABRSectorToGPFlowParameter(String _sector)
        {
            String _GPParameter = ""; //"ABR-IIoT";
            if (_sector.Equals("EIOT", StringComparison.InvariantCultureIgnoreCase))
            {
                _GPParameter = "ABR-EIoT";
            }
            else if (_sector.Equals("SIOT", StringComparison.InvariantCultureIgnoreCase))
            {
                _GPParameter = "ABR-SIoT";
            }
            else
            {
                _GPParameter = "ABR-IIoT";
            }
            return _GPParameter;
        }

        public static bool IsNeedGPforABR(string QuoteID, ref decimal Rate, ref List<QuotationDetail> mitems)
        {
            string msg = string.Empty; bool isNeedGP = false; decimal minRate = 1;
            List<QuotationDetail> items = GetQuotationDetail(QuoteID);

            mitems = new List<QuotationDetail>();
            decimal TotalAmount = items.Count > 0 ? items.Sum(p => p.newUnitPrice * p.qty).Value : 0;
            decimal TotalCost = 0;

            //DataTable _dtGPParas = eQuotationDAL.getGP_Parameter("ABR-IIoT");
            String _salescode = GetSalesCodeByQuoteID(QuoteID);
            String _sector = UserRoleBusinessLogic.getABRSectorBySalesCode(_salescode);
            String _GPParameter = ConvertABRSectorToGPFlowParameter(_sector); //"ABR-IIoT";
            DataTable _dtGPParas = eQuotationDAL.getGP_Parameter(_GPParameter);

            QuotationMaster Master = GetQuotationMaster(QuoteID);
            Decimal MaxGPPercent = 0;

            foreach (QuotationDetail item in items)
            {
                if (item.IsServicePartX) { continue; }
                if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { continue; }
                if (!string.IsNullOrEmpty(item.sprNo)) { continue; }
                if (item.partNo.Equals("Freight_Fee", StringComparison.OrdinalIgnoreCase)) { continue; }

                // Ryan 20171025 Add for service part check through SQL select due to unknown context error in PartBusinessLogic.IsServicePart
                var objGENITEMCATGRP = SqlProvider.dbExecuteScalar("MY", String.Format("SELECT top 1 a.GENITEMCATGRP FROM SAP_PRODUCT a inner join SAP_PRODUCT_ORG b on a.PART_NO = b.PART_NO WHERE a.PART_NO = '{0}' and b.ORG_ID = '{1}'", item.partNo, Master.org));
                if (objGENITEMCATGRP != null && !String.IsNullOrEmpty(objGENITEMCATGRP.ToString()))
                {
                    if (objGENITEMCATGRP.ToString().ToUpper().Equals("DIEN") || objGENITEMCATGRP.ToString().ToUpper().Equals("LEIS"))
                        continue;
                }


                //TotalCost += item.newItp.Value * item.qty.Value;
                double mGPpercent = 0; decimal mRate = 0;
                if (item.newUnitPrice.Value > 0)
                {
                    mRate = (item.newUnitPrice.Value - item.newItp.Value) / item.newUnitPrice.Value;
                }
                else
                {
                    mRate = 0;
                }

                //Identify which part's GP is the lowest GP value and the GP approval level
                foreach (DataRow itemGP in _dtGPParas.Rows)
                {
                    double.TryParse(itemGP["GP_Percent"].ToString().Trim(), out mGPpercent);
                    if ((decimal)mGPpercent > MaxGPPercent)
                        MaxGPPercent = (decimal)mGPpercent;

                    if (mRate <= (decimal)mGPpercent)
                    {
                        var mitem = mitems.FirstOrDefault(p => p.partNo == item.partNo && p.line_No == item.line_No);
                        if (mitem == null)
                        {
                            mitems.Add(item);
                        }
                        if (mRate != 0 && minRate > mRate) minRate = mRate;
                        isNeedGP = true;
                    }
                    //else
                    //{
                    //    // although mRate is larger than mGPpercent, but is below min price, need to add in gp items list.
                    //    var mitem = mitems.FirstOrDefault(p => p.partNo == item.partNo && p.line_No == item.line_No);
                    //    if (mitem == null)
                    //    {
                    //        mitems.Add(item);
                    //    }
                    //}
                }
            }

            Rate = minRate;

            if (Rate > MaxGPPercent) Rate = MaxGPPercent;

            return isNeedGP;
        }



        public static bool IsNeedGPforIntercon(string QuoteID, ref decimal Rate, ref List<QuotationDetail> mitems)
        {
            string msg = string.Empty; bool isNeedGP = false; decimal minRate = 1;
            List<QuotationDetail> items = GetQuotationDetail(QuoteID);

            mitems = new List<QuotationDetail>();
            decimal TotalAmount = items.Count > 0 ? items.Sum(p => p.newUnitPrice * p.qty).Value : 0;
            decimal TotalCost = 0;
            DataTable _dtGPParas = eQuotationDAL.getGP_Parameter("Intercon");

            QuotationMaster Master = GetQuotationMaster(QuoteID);

            // Ryan 20160909 Add extra validation for no ERPID case, set its ERPID to default value.
            if (String.IsNullOrEmpty(Master.quoteToErpId))
            {
                if (Master.quoteNo.StartsWith("AIAQ"))
                    Master.quoteToErpId = "AADE007";
                else
                    Master.quoteToErpId = "AILR001";
            }


            // Ryan 20160901 Order simulation process
            Boolean isBelowMinPrice = false;
            Decimal MaxGPPercent = 0;
            Order _order = new Order();
            _order.Currency = Master.currency;
            _order.OrgID = Master.org;
            _order.DistChannel = Master.DIST_CHAN;
            _order.Division = Master.DIVISION;

            //Frank 20170703:Sabrina suggests that please use quote simulation to get ZMIP
            _order.OrderType = SAPOrderType.AG;

            int i = 0;
            foreach (QuotationDetail qd in items)
            {
                _order.AddLooseItem(Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(qd.partNo), 1);
                _order.LineItems[i].PlantID = qd.deliveryPlant;
                i++;
            }
            _order.SetOrderPartnet(new OrderPartner(Master.quoteToErpId, Master.org, OrderPartnerType.SoldTo));
            _order.SetOrderPartnet(new OrderPartner(Master.quoteToErpId, Master.org, OrderPartnerType.ShipTo));
            _order.SetOrderPartnet(new OrderPartner(Master.quoteToErpId, Master.org, OrderPartnerType.BillTo));
            String _errMsg = String.Empty;
            Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref _order, ref _errMsg);
            List<Product> pr = new List<Product>();
            if (_order != null)
            {
                pr = _order.LineItems;
            }

            foreach (QuotationDetail item in items)
            {
                //if (QDItem.ItemTypeX == QuoteItemType.BtosParent) { continue; }
                //if (item.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                if (item.IsServicePartX) { continue; }
                if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { continue; }
                if (!string.IsNullOrEmpty(item.sprNo)) { continue; }
                if (item.partNo.Equals("Freight_Fee", StringComparison.OrdinalIgnoreCase)) { continue; }
                //if(Advantech.Myadvantech.Business.PartBusinessLogic.IsServicePart(item.partNo,Master.org)) { continue; }

                // Ryan 20171025 Add for service part check through SQL select due to unknown context error in PartBusinessLogic.IsServicePart
                var objGENITEMCATGRP = SqlProvider.dbExecuteScalar("MY", String.Format("SELECT top 1 a.GENITEMCATGRP FROM SAP_PRODUCT a inner join SAP_PRODUCT_ORG b on a.PART_NO = b.PART_NO WHERE a.PART_NO = '{0}' and b.ORG_ID = '{1}'", item.partNo, Master.org));
                if (objGENITEMCATGRP != null && !String.IsNullOrEmpty(objGENITEMCATGRP.ToString()))
                {
                    if (objGENITEMCATGRP.ToString().ToUpper().Equals("DIEN") || objGENITEMCATGRP.ToString().ToUpper().Equals("LEIS"))
                        continue;
                }

                // Ryan 20160902 New logic for intercon, if higher than min price, than won't need GP process anyway.
                Decimal _zmip = pr.Where(d => d.PartNumber == item.partNo).Select(d => d.MinimumPrice).FirstOrDefault();

                // Ryan 20170920 if min price is lower than ITP, then use ITP to check GP instead
                if (_zmip < item.newItp)
                    _zmip = (Decimal)item.newItp;

                if (item.newUnitPrice < _zmip || _zmip == 0)
                {
                    isNeedGP = true;
                    isBelowMinPrice = true;
                    //item.MinimumPrice = Math.Round(pr.Where(d => d.PartNumber == item.partNo).Select(d => d.MinimumPrice).FirstOrDefault(), 2, MidpointRounding.AwayFromZero);
                    item.MinimumPrice = Math.Round(_zmip, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    continue;
                }

                //ICC 2016/2/15 When new unit price >= unit price then bypass this item.
                //if (item.newUnitPrice.Value >= item.unitPrice.Value) { continue; }

                TotalCost += item.newItp.Value * item.qty.Value;
                double mGPpercent = 0; decimal mRate = 0;
                if (item.newUnitPrice.Value > 0)
                {
                    mRate = (item.newUnitPrice.Value - item.newItp.Value) / item.newUnitPrice.Value;
                }
                else
                {
                    mRate = 0;
                }

                //Identify which part's GP is the lowest GP value and the GP approval level
                foreach (DataRow itemGP in _dtGPParas.Rows)
                {
                    double.TryParse(itemGP["GP_Percent"].ToString().Trim(), out mGPpercent);
                    if ((decimal)mGPpercent > MaxGPPercent)
                        MaxGPPercent = (decimal)mGPpercent;

                    if (mRate <= (decimal)mGPpercent)
                    {
                        var mitem = mitems.FirstOrDefault(p => p.partNo == item.partNo && p.line_No == item.line_No);
                        if (mitem == null)
                        {
                            mitems.Add(item);
                        }
                        if (mRate != 0 && minRate > mRate) minRate = mRate;
                        //isNeedGP = true;
                    }
                    else
                    {
                        // although mRate is larger than mGPpercent, but is below min price, need to add in gp items list.
                        var mitem = mitems.FirstOrDefault(p => p.partNo == item.partNo && p.line_No == item.line_No);
                        if (mitem == null)
                        {
                            mitems.Add(item);
                        }
                    }
                }
            }
            //eQuotationContext.Current.SaveChanges();
            //Frank 20151223 Do not need to check whole quote's margin. If all lines are higher than margin checking threadhold
            //, it is no reason that total margin amount is lower than threadhold.
            //Identify if the quote need to be approved by tatal amount and total qty
            //if (!isNeedGP)
            //{
            //    Rate = 0;

            //    if (TotalAmount > 0)
            //    {
            //        Rate = (TotalAmount - TotalCost) / TotalAmount;
            //    }
            //    else
            //    {
            //        Rate = 0;
            //    }

            //    if (Rate == 0) return false;
            //    double _GPpercent = 0;
            //    foreach (DataRow item in _dtGPParas.Rows)
            //    {
            //        double.TryParse(item["GP_Percent"].ToString().Trim(), out _GPpercent);

            //        if (Rate <= (decimal)_GPpercent)
            //        {
            //            isNeedGP = true;
            //        }
            //    }
            //    if (Rate > minRate) Rate = minRate;

            //}
            //else
            //{
            //    Rate = minRate;
            //}
            Rate = minRate;
            if (isBelowMinPrice)
            {
                if (Rate > MaxGPPercent)
                    Rate = MaxGPPercent;
            }


            return isNeedGP;
        }
        public static void WriteInterconQuoteLinesITP(string QuoteID, ref string msg)
        {
            var items = GetQuotationDetail(QuoteID);
            var Conditions = SAPDAL.SAPDAL.GetInterconPN_Cost_GPPercent(QuoteID, ref msg);
            ArrayList array = new ArrayList();

            foreach (var _quoteitem in items)
            {
                if (PartBusinessLogic.IsODMTPart(_quoteitem.partNo))
                {
                    _quoteitem.itp = _quoteitem.unitPrice;
                    _quoteitem.newItp = _quoteitem.unitPrice;
                }
                else
                {
                    var _cond = Conditions.FirstOrDefault(p => p.PART_NO == _quoteitem.partNo);
                    if (_cond != null)
                    {
                        _quoteitem.itp = _cond.COND_VAL;
                        if (string.IsNullOrEmpty(_quoteitem.sprNo))
                        {
                            _quoteitem.newItp = _cond.COND_VAL;
                        }
                    }
                }
            }
            eQuotationContext.Current.SaveChanges();
        }




        public static string getGPmailBodyForAUS(string QuoteID)
        {
            QuotationMaster _QMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            List<QuotationDetail> items = _QMaster.QuotationDetail;
            string str = "";

            str += "<table style='width:100%;'><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Name</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Id</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Total</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Create By</nobr></td></TR>";
            if ((_QMaster != null))
            {
                str += "<tr>";
                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToName + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToErpId + "</nobr></td>";

                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.currency + items.Sum(p => p.qty * p.newUnitPrice).ToString() + "</nobr></td>";
                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.createdBy + "</nobr></td>";
                str += "</TR>";

                StringBuilder SB = new StringBuilder();
                SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>");
                SB.AppendFormat(" <ul>");
                SB.AppendFormat("<li>Below GP</li>");
                SB.AppendFormat("</ul>");
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;\">" + SB.ToString() + "</td></tr>";
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;\"><b>Reason: </b>" + _QMaster.relatedInfo + "</td></tr>";
            }
            str += "</table>";
            // str += "<HR/>";

            //str += "<table><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Line No</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Part No</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Unit Price</nobr></td>" + "<TD style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>QTY</nobr></TD>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr></nobr></td></TR>";
            //foreach (var item in items)
            //{
            //    str += "<tr>";
            //    str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + item.line_No + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + item.partNo + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + item.newUnitPrice.ToString() + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + item.qty.ToString() + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + "" + "</nobr></td>";
            //    str += "</TR>";
            //}

            //str += "</TABLE>";
            //  str += "<HR/>";
            StringBuilder sb = new StringBuilder();
            //SAPDAL.SAPDAL.GPInfo GPInfo1 = SAPDAL.SAPDAL.CalcANAQuoteGP(QuoteID);
            SAPDAL.SAPDAL.GPInfo GPInfo1 = null;

            if (_QMaster.quoteNo.StartsWith("AENQ"))
            {
                //AENC KA
                GPInfo1 = SAPDAL.SAPDAL.CalcAENCQuoteGP(QuoteID);
            }
            else if (_QMaster.quoteNo.StartsWith("AUSQ"))
            {
                //ANAAOline
                GPInfo1 = SAPDAL.SAPDAL.CalcANAAOlineQuoteGP(QuoteID);
            }
            else
            {
                //AAC KA CP
                GPInfo1 = SAPDAL.SAPDAL.CalcANAQuoteGP(QuoteID);
            }

            if (GPInfo1.LineItems != null)
            {
                sb.AppendFormat(" <table cellspacing='0' style='width:100%;border-collapse:collapse;' rules='all' border='1'> <tr style='color:Black;background-color:Gainsboro;'><th scope='col'>Item</th><th scope='col'>Material</th><th scope='col'>Qty</th><th scope='col'>List Price</th><th scope='col'>Quoted Unit Price</th><th scope='col'>Quoted Discount</th><th scope='col'>Cost (ITP)</th><th scope='col'>GP</th><th scope='col'>Min GP Threshold</th><th scope='col'>GP Block</th></tr>");
                string cssstr = "";
                foreach (var i in GPInfo1.LineItems)
                {
                    if (i.GPBlock)
                    { cssstr = "style='color:red;white-space:nowrap;'"; }
                    else
                    { cssstr = ""; }
                    sb.AppendFormat("<tr {10}><td align='center'>{0}</td><td align=left> {1}</td><td align='center'>{2}</td><td align=right>{3}</td><td align=right>{4}</td><td align='right'>{5}</td><td align='right'>{6}</td><td align='right'>{7}</td><td align='right'>{8}</td><td align='center'>{9}</td>"
                    + "</tr> ", i.LineNo, i.PartNo, i.Qty, i.ListPrice, i.UnitPrice, i.Discount.ToString("P"), i.Cost, i.GPPercent.ToString("P"), i.ThresholdPercent.ToString("P"), i.GPBlock ? "Yes" : "No", cssstr);

                }
                sb.AppendFormat("</table>");


                sb.AppendFormat("<br/><table cellspacing='0' style='width:100%;border-collapse:collapse;' rules='all' border='1'><tr style='color:Black;background-color:Gainsboro;'><th scope='col'>Total List Price</th><th scope='col'>Total Unit Price</th><th scope='col'>Total Discount</th><th scope='col'>Total Cost</th><th scope='col'>Total GP</th><th scope='col'>TotalMinPrice</th><th scope='col'>Total Min. GP Threshold</th><th scope='col'>GP Block</th></tr>");


                sb.AppendFormat("  <tr><td align='right'>{0}</td><td align='right'>{1}</td><td align='right'>{2}</td><td align='right'>{3}</td><td align='right'>{4}</td><td align='right'>{5}</td><td align='right'>{6}</td><td align='center'>{7}</td></tr> ",
                  GPInfo1.TotalUnitPrice, GPInfo1.TotalListPrice, GPInfo1.TotalDiscount.ToString("P"), GPInfo1.TotalCost,
                  GPInfo1.TotalGPPercent.ToString("P"), GPInfo1.TotalMinPrice, GPInfo1.TotalThresholdPercent.ToString("P"), GPInfo1.IsTotalGPBlock ? "Yes" : "Yes");


                sb.AppendFormat("</table>");
            }

            return str + sb.ToString();

        }

        public static string getGPmailBodyForAENCWithoutGPITP(string QuoteID)
        {
            QuotationMaster _QMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            List<QuotationDetail> items = _QMaster.QuotationDetail;
            string str = "";

            str += "<table style='width:100%;'><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Name</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Id</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Total</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Create By</nobr></td></TR>";
            if ((_QMaster != null))
            {
                str += "<tr>";
                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToName + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToErpId + "</nobr></td>";

                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.currency + items.Sum(p => p.qty * p.newUnitPrice).ToString() + "</nobr></td>";
                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.createdBy + "</nobr></td>";
                str += "</TR>";

                StringBuilder SB = new StringBuilder();
                SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>");
                SB.AppendFormat(" <ul>");
                SB.AppendFormat("<li>Below GP</li>");
                SB.AppendFormat("</ul>");
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;\">" + SB.ToString() + "</td></tr>";
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;\"><b>Reason: </b>" + _QMaster.relatedInfo + "</td></tr>";
            }
            str += "</table>";

            StringBuilder sb = new StringBuilder();
            SAPDAL.SAPDAL.GPInfo GPInfo1 = null;

            if (_QMaster.quoteNo.StartsWith("AENQ"))
            {
                //AENC KA
                GPInfo1 = SAPDAL.SAPDAL.CalcAENCQuoteGP(QuoteID);
            }
            else if (_QMaster.quoteNo.StartsWith("AUSQ"))
            {
                //ANAAOline
                //ddd
                GPInfo1 = SAPDAL.SAPDAL.CalcANAAOlineQuoteGP(QuoteID);
            }
            else
            {
                //AAC KA CP
                GPInfo1 = SAPDAL.SAPDAL.CalcANAQuoteGP(QuoteID);
            }

            if (GPInfo1.LineItems != null)
            {
                //sb.AppendFormat(" <table cellspacing='0' style='width:100%;border-collapse:collapse;' rules='all' border='1'> <tr style='color:Black;background-color:Gainsboro;'><th scope='col'>Item</th><th scope='col'>Material</th><th scope='col'>Qty</th><th scope='col'>List Price</th><th scope='col'>Quoted Unit Price</th><th scope='col'>Quoted Discount</th><th scope='col'>Min GP Threshold</th><th scope='col'>GP Block</th></tr>");
                sb.AppendFormat(" <table cellspacing='0' style='width:100%;border-collapse:collapse;' rules='all' border='1'> <tr style='color:Black;background-color:Gainsboro;'><th scope='col'>Item</th><th scope='col'>Material</th><th scope='col'>Qty</th><th scope='col'>List Price</th><th scope='col'>Quoted Unit Price</th><th scope='col'>Quoted Discount</th></tr>");
                string cssstr = "";
                foreach (var i in GPInfo1.LineItems)
                {
                    if (i.GPBlock)
                    { cssstr = "style='color:red;white-space:nowrap;'"; }
                    else
                    { cssstr = ""; }
                    //sb.AppendFormat("<tr {10}><td align='center'>{0}</td><td align=left> {1}</td><td align='center'>{2}</td><td align=right>{3}</td><td align=right>{4}</td><td align='right'>{5}</td><td align='right'>{8}</td><td align='center'>{9}</td>"
                    //+ "</tr> ", i.LineNo, i.PartNo, i.Qty, i.ListPrice, i.UnitPrice, i.Discount.ToString("P"), i.Cost, i.GPPercent.ToString("P"), i.ThresholdPercent.ToString("P"), i.GPBlock ? "Yes" : "No", cssstr);

                    sb.AppendFormat("<tr {6}><td align='center'>{0}</td><td align=left> {1}</td><td align='center'>{2}</td><td align=right>{3}</td><td align=right>{4}</td><td align='right'>{5}</td>"
                    + "</tr> ", i.LineNo, i.PartNo, i.Qty, i.ListPrice, i.UnitPrice, i.Discount.ToString("P"), cssstr);


                }
                sb.AppendFormat("</table>");

                //sb.AppendFormat("<br/><table cellspacing='0' style='width:100%;border-collapse:collapse;' rules='all' border='1'><tr style='color:Black;background-color:Gainsboro;'><th scope='col'>Total List Price</th><th scope='col'>Total Unit Price</th><th scope='col'>Total Discount</th><th scope='col'>Total Cost</th><th scope='col'>TotalMinPrice</th><th scope='col'>GP Block</th></tr>");
                sb.AppendFormat("<br/><table cellspacing='0' style='width:100%;border-collapse:collapse;' rules='all' border='1'><tr style='color:Black;background-color:Gainsboro;'><th scope='col'>Total List Price</th><th scope='col'>Total Unit Price</th><th scope='col'>Total Discount</th></tr>");

                //sb.AppendFormat("  <tr><td align='right'>{0}</td><td align='right'>{1}</td><td align='right'>{2}</td><td align='right'>{3}</td><td align='right'>{5}</td><td align='center'>{7}</td></tr> ",
                //  GPInfo1.TotalListPrice, GPInfo1.TotalUnitPrice,  GPInfo1.TotalDiscount.ToString("P"), GPInfo1.TotalCost,
                //  GPInfo1.TotalGPPercent.ToString("P"), GPInfo1.TotalMinPrice, GPInfo1.TotalThresholdPercent.ToString("P"), GPInfo1.IsTotalGPBlock ? "Yes" : "Yes");

                sb.AppendFormat("  <tr><td align='right'>{0}</td><td align='right'>{1}</td><td align='right'>{2}</td></tr> ",
                  GPInfo1.TotalListPrice, GPInfo1.TotalUnitPrice, GPInfo1.TotalDiscount.ToString("P"));


                sb.AppendFormat("</table>");
            }

            return str + sb.ToString();

        }


        public static string getGPmailBodyForAKR(string QuoteID)
        {
            QuotationMaster _QMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            List<QuotationDetail> items = _QMaster.QuotationDetail;
            decimal TotalAmount = items.Count > 0 ? items.Sum(p => p.newUnitPrice * p.qty).Value : 0;
            decimal TotalCost = 0; string msg = string.Empty;
            //List<SAPDAL.SAPDAL.PN_COND> Conditions = SAPDAL.SAPDAL.GetAKRPN_Cost_GPPercent(QuoteID, ref msg);
            //foreach (QuotationDetail item in items)
            //{
            //    SAPDAL.SAPDAL.PN_COND PNC = Conditions.FirstOrDefault(P => P.PART_NO == item.partNo);
            //    if (PNC != null)
            //    {
            //        TotalCost += PNC.COND_VAL * item.qty.Value;
            //    }
            //    else
            //    {
            //        TotalCost += item.newUnitPrice.Value * item.qty.Value;
            //    }
            //}

            //Total Margin
            decimal _TotalMargin = GetTotalMarginByQuotationDetails(items);

            string str = "";
            //ICC 2016/4/14 Add Sales email in GP block email & page.
            str += "<table style='width:100%;'><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Name</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Id</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Total</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Margin</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Sales email</nobr></td></TR>";
            if ((_QMaster != null))
            {
                str += "<tr>";
                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToName + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToErpId + "</nobr></td>";

                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.currency + " " + items.Sum(p => p.qty * p.newUnitPrice).Value.ToString("N0") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + (_TotalMargin).ToString("P") + "</nobr></td>";
                //ICC 2016/4/14 Add Sales email in GP block email & page.
                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.createdBy + "</nobr></td>";
                str += "</TR>";

                StringBuilder SB = new StringBuilder();
                SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>");
                SB.AppendFormat(" <ul>");
                SB.AppendFormat("<li>Below GP</li>");
                SB.AppendFormat("</ul>");
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;text-align:left;\">" + SB.ToString() + "</td></tr>";
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;text-align:left;\"><b>Reason: </b>" + _QMaster.relatedInfo + "</td></tr>";
            }
            str += "</table>";
            str += "<HR/>";

            str += "<table border=1 cellpadding=10 cellspacing=0 width=800px  ><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Line No</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Part No</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Unit Price</nobr></td>" + "<TD style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>QTY</nobr></TD>" + "</TR>";
            decimal cost = 0;
            foreach (var item in items)
            {

                //str += "<tr>";
                //str += "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.line_No + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + item.partNo + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + "  " + item.newUnitPrice.Value.ToString("N0") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.qty.ToString() + "</nobr></td>" + "";//Math.Round(cost,2).ToString()
                //str += "</TR>";

                decimal itp = 0;
                decimal ListSubTotal = 0;
                decimal UnitSubTotal = 0;

                if (item.line_No >= 100 && item.line_No % 100 == 0)
                {

                    var subItems = items.Where(p => p.HigherLevel == item.line_No && !p.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase));
                    int _ParentItemQty = item.qty.Value;
                    if (_ParentItemQty < 1) { _ParentItemQty = 1; }
                    //itp = subItems.Sum(p => p.newItp * p.qty.Value).Value;
                    itp = subItems.Sum(p => p.newItp * p.qty.Value).Value / _ParentItemQty;
                    ListSubTotal = subItems.Sum(p => p.listPrice * p.qty.Value).Value;
                    UnitSubTotal = subItems.Sum(p => p.newUnitPrice * p.qty.Value).Value;
                }
                else
                {

                    itp = item.newItp.GetValueOrDefault(0);
                    ListSubTotal = item.listPrice.GetValueOrDefault(0);
                    UnitSubTotal = item.newUnitPrice.GetValueOrDefault(0);


                }
                str += "<tr>";
                str += "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.line_No + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + item.partNo + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + "  " + UnitSubTotal.ToString("N0") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.qty.ToString() + "</nobr></td>" + "";//Math.Round(cost,2).ToString()
                str += "</TR>";


            }

            str += "</TABLE>";

            // StringBuilder sb = new StringBuilder();



            return str;

        }
        public static string getGPmailBodyForAKRApprover(string QuoteID)
        {
            QuotationMaster _QMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            List<QuotationDetail> items = _QMaster.QuotationDetail;
            decimal TotalAmount = items.Count > 0 ? items.Sum(p => p.newUnitPrice * p.qty).Value : 0;
            //decimal TotalCost = 0; string msg = string.Empty;
            //List<SAPDAL.SAPDAL.PN_COND> Conditions = SAPDAL.SAPDAL.GetAKRPN_Cost_GPPercent(QuoteID, ref msg);
            //foreach (QuotationDetail item in items)
            //{
            //    SAPDAL.SAPDAL.PN_COND PNC = Conditions.FirstOrDefault(P => P.PART_NO == item.partNo);
            //    if (PNC != null)
            //    {
            //        TotalCost += PNC.COND_VAL * item.qty.Value;
            //    }
            //}

            //Total Margin
            decimal _TotalMargin = GetTotalMarginByQuotationDetails(items);

            string str = "";
            //ICC 2016/4/14 Add Sales email in GP block email & page.
            str += "<table style='width:100%;'  class='table table-bordered'><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Name</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Id</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Total</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Margin</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Sales emai</nobr></td></TR>";
            if ((_QMaster != null))
            {
                str += "<tr>";
                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToName + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToErpId + "</nobr></td>";

                str += "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + ": " + (items.Sum(p => p.qty * p.newUnitPrice)).Value.ToString("N0") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + (_TotalMargin).ToString("P") + "</nobr></td>";
                //ICC 2016/4/14 Add Sales email in GP block email & page.
                str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.createdBy + "</nobr></td>";
                str += "</TR>";

                StringBuilder SB = new StringBuilder();
                SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>");
                SB.AppendFormat(" <ul>");
                SB.AppendFormat("<li>Below GP</li>");
                SB.AppendFormat("</ul>");
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;text-align:left;\">" + SB.ToString() + "</td></tr>";
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;text-align:left;\"><b>Reason: </b>" + _QMaster.relatedInfo + "</td></tr>";
            }
            str += "</table>";
            str += "<HR/>";

            str += "<table boder=1 width=99% class='table table-bordered'><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Line No</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Part No</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>List Price</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Unit Price</nobr></td>" + "<TD style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>QTY</nobr></TD>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Cost</nobr></td></TR>";
            decimal cost = 0;
            foreach (var item in items)
            {
                //SAPDAL.SAPDAL.PN_COND PNC = Conditions.FirstOrDefault(P => P.PART_NO == item.partNo);
                //if (PNC != null)
                //{
                //    cost = PNC.COND_VAL;
                //}

                decimal itp = 0;
                decimal ListSubTotal = 0;
                decimal UnitSubTotal = 0;

                if (item.line_No >= 100 && item.line_No % 100 == 0)
                {

                    var subItems = items.Where(p => p.HigherLevel == item.line_No && !p.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase));
                    int _ParentItemQty = item.qty.Value;
                    if (_ParentItemQty < 1) { _ParentItemQty = 1; }
                    //itp = subItems.Sum(p => p.newItp * p.qty.Value).Value;
                    itp = subItems.Sum(p => p.newItp * p.qty.Value).Value / _ParentItemQty;
                    ListSubTotal = subItems.Sum(p => p.listPrice * p.qty.Value).Value;
                    UnitSubTotal = subItems.Sum(p => p.newUnitPrice * p.qty.Value).Value;
                }
                else
                {

                    itp = item.newItp.GetValueOrDefault(0);
                    ListSubTotal = item.listPrice.GetValueOrDefault(0);
                    UnitSubTotal = item.newUnitPrice.GetValueOrDefault(0);

                    //str += "<tr>";
                    //str += "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.line_No + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:left;\"><nobr>" + item.partNo + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + " " + item.listPrice.Value.ToString("N0") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + " " + item.newUnitPrice.Value.ToString("N0") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.qty.ToString() + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + " " + Math.Round(item.newItp.GetValueOrDefault(0), 2).ToString("N0") + "</nobr></td>";
                    //str += "</TR>";

                }
                str += "<tr>";
                str += "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.line_No + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:left;\"><nobr>" + item.partNo + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + " " + ListSubTotal.ToString("N0") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + " " + UnitSubTotal.ToString("N0") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.qty.ToString() + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + " " + Math.Round(itp, 2).ToString("N0") + "</nobr></td>";
                str += "</TR>";



            }

            str += "</TABLE>";

            // StringBuilder sb = new StringBuilder();



            return str;

        }
        //public static string getGPmailBodyForIntercon(string QuoteID)
        //{
        //    QuotationMaster _QMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
        //    List<QuotationDetail> items = _QMaster.quoteDetailList;
        //    //decimal TotalAmount = items.Count > 0 ? items.Sum(p => p.newUnitPrice * p.qty).Value : 0;
        //    decimal TotalAmount = 0;
        //    decimal TotalCost = 0; string msg = string.Empty;
        //    //Frank 20151222 exculde quote items which has spr no.
        //    foreach (QuotationDetail item in items)
        //    {
        //        if (string.IsNullOrEmpty(item.sprNo))
        //        {
        //            TotalAmount += item.newUnitPrice.Value * item.qty.Value;
        //            TotalCost += item.newItp.Value * item.qty.Value;
        //        }
        //    }
        //    string str = "";
        //    str += "<table style='width:100%;'><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Name</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Acount Id</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Total</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Margin</nobr></td></TR>";
        //    if ((_QMaster != null))
        //    {
        //        str += "<tr>";
        //        str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToName + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.quoteToErpId + "</nobr></td>";

        //        str += "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + _QMaster.currency + " " + items.Sum(p => p.qty * p.newUnitPrice).Value.ToString("N2") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + ((TotalAmount - TotalCost) / TotalAmount).ToString("P") + "</nobr></td>";
        //        str += "</TR>";

        //        StringBuilder SB = new StringBuilder();
        //        SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>");
        //        SB.AppendFormat(" <ul>");
        //        SB.AppendFormat("<li>Below GP</li>");
        //        SB.AppendFormat("</ul>");
        //        str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;text-align:left;\">" + SB.ToString() + "</td></tr>";
        //        str += "<tr><td colspan=\"4\" style=\"font-family:Arial;color:#1965b1;text-align:left;\"><b>Reason: </b>" + _QMaster.relatedInfo + "</td></tr>";
        //    }
        //    str += "</table>";
        //    str += "<HR/>";

        //    str += "<table border=1 cellpadding=10 cellspacing=0 width=800px  ><tr><td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Line No</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Part No</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Unit Price</nobr></td>" + "<TD style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>QTY</nobr></TD>" + "<TD style=\"font-family:Arial;color:#1965b1;font-weight:bold;\"><nobr>Sub Total</nobr></TD>" + "</TR>";
        //    decimal newUnitPrice = 0, SubTotal = 0;
        //    foreach (var item in items)
        //    {
        //        //SAPDAL.SAPDAL.PN_COND PNC = Conditions.FirstOrDefault(P => P.PART_NO == item.partNo);
        //        //if (PNC != null)
        //        //{
        //        //    cost = PNC.COND_VAL;
        //        //}
        //        newUnitPrice = item.newUnitPrice.Value;
        //        SubTotal = item.newUnitPrice.Value * item.qty.Value;
        //        if (item.line_No >= 100 && item.line_No % 100 == 0)
        //        {
        //            var subItems = items.Where(p => p.HigherLevel == item.line_No);                  
        //            SubTotal = subItems.Sum(p => p.newUnitPrice*p.qty).Value;
        //            newUnitPrice = SubTotal/item.qty.Value;
        //        }
        //        str += "<tr>";
        //        str += "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.line_No + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;\"><nobr>" + item.partNo + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:right;\"><nobr>" + _QMaster.currency + "  " + newUnitPrice.ToString("N2") + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + item.qty.ToString() + "</nobr></td>" + "<td style=\"font-family:Arial;color:#1965b1;text-align:center;\"><nobr>" + SubTotal.ToString("N2") + "</nobr></td>";//Math.Round(cost,2).ToString()
        //        str += "</TR>";
        //    }

        //    str += "</TABLE>";

        //    // StringBuilder sb = new StringBuilder();



        //    return str;

        //}
        public static string getGPmailBodyForInterconApprover(string QuoteID)
        {
            QuotationMaster _QMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            List<QuotationDetail> items = _QMaster.QuotationDetail;
            decimal TotalAmount = 0; decimal TotalITP = 0;
            decimal TotalMargin = 0; string msg = string.Empty;
            var GPitems = new List<QuotationDetail>(); decimal Rate = 0;
            IsNeedGPforIntercon(QuoteID, ref Rate, ref GPitems);
            //Frank 20151222 exculde quote items which has spr no.
            foreach (QuotationDetail item in items)
            {
                //if (item.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                if (item.IsServicePartX) { continue; }
                if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { continue; }
                if (item.partNo.Equals("Freight_Fee", StringComparison.OrdinalIgnoreCase)) { continue; }
                //if (Advantech.Myadvantech.Business.PartBusinessLogic.IsServicePart(item.partNo, _QMaster.org)) { continue; }

                // Ryan 20171025 Add for service part check through SQL select due to unknown context error in PartBusinessLogic.IsServicePart
                var objGENITEMCATGRP = SqlProvider.dbExecuteScalar("MY", String.Format("SELECT top 1 a.GENITEMCATGRP FROM SAP_PRODUCT a inner join SAP_PRODUCT_ORG b on a.PART_NO = b.PART_NO WHERE a.PART_NO = '{0}' and b.ORG_ID = '{1}'", item.partNo, _QMaster.org));
                if (objGENITEMCATGRP != null && !String.IsNullOrEmpty(objGENITEMCATGRP.ToString()))
                {
                    if (objGENITEMCATGRP.ToString().ToUpper().Equals("DIEN") || objGENITEMCATGRP.ToString().ToUpper().Equals("LEIS"))
                        continue;
                }

                TotalAmount += item.newUnitPrice.Value * item.qty.Value;
                TotalITP += item.newItp.Value * item.qty.Value;
            }
            if (TotalAmount > 0)
            {
                TotalMargin = (TotalAmount - TotalITP) / TotalAmount;
            }
            else
            {
                TotalMargin = 0;
            }

            // Calculate max gp percent rate
            DataTable _dtGPParas = eQuotationDAL.getGP_Parameter("Intercon");
            Decimal MaxGPPercent = 0;
            foreach (DataRow itemGP in _dtGPParas.Rows)
            {
                if (Convert.ToDecimal(itemGP["GP_Percent"].ToString().Trim()) > MaxGPPercent)
                    MaxGPPercent = Convert.ToDecimal(itemGP["GP_Percent"].ToString().Trim());
            }
            MaxGPPercent = MaxGPPercent * 100;

            string str = "";
            str += "<table style='width:80%;'  class='table table-bordered'><tr><td style=\"font-family:Arial;font-weight:bold;\"><nobr>Acount Name</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Acount Id</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Total</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Margin</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Create By</nobr></td></TR>";
            if ((_QMaster != null))
            {
                str += "<tr>";
                str += "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.quoteToName + "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.quoteToErpId + "</nobr></td>";

                str += "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.currency + ": " + (items.Sum(p => p.qty * p.newUnitPrice)).Value.ToString("N2") + "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + (TotalMargin).ToString("P") + "</nobr></td>";
                str += "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.createdBy + "</nobr></td>";
                str += "</TR>";

                StringBuilder SB = new StringBuilder();
                SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>");
                SB.AppendFormat(" <ul>");
                SB.AppendFormat("<li>Below GP</li>");
                SB.AppendFormat("</ul>");
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;text-align:left;\">" + SB.ToString() + "</td></tr>";
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;text-align:left;\"><b>Reason: </b>" + _QMaster.relatedInfo + "</td></tr>";
            }
            str += "</table>";
            str += "<HR/>";

            string _tdHeader = "<td style='border:1px solid darkgray;border-collapse:collapse;font-family:Arial;font-weight:bold;text-align:center;'>";
            string _td = "<td style='border:1px solid darkgray;border-collapse:collapse;font-family:Arial;text-align:center;'>";

            str += "<table style='border:1px black solid;' rules='all' cellpadding='1'; width='99%' ><tr>" + _tdHeader + "Line No</td>" + _tdHeader + "Part No</td>" + _tdHeader + "List Price</td>" + _tdHeader + "Unit Price</td>" + _tdHeader + "QTY</TD>" + _tdHeader + "Sub Total</td>" + _tdHeader + "Margin</td>" + _tdHeader + "SPR</td>" + _tdHeader + "Note</td></TR>";
            decimal itp = 0, newUnitPrice = 0, listPrice = 0, SubTotal = 0, ItemMargin = 0;
            string ItemMarginStr = string.Empty;
            foreach (var item in items)
            {
                itp = item.newItp.Value;
                listPrice = item.listPrice.Value;
                newUnitPrice = item.newUnitPrice.Value;
                SubTotal = item.newUnitPrice.Value * item.qty.Value;

                if (item.line_No >= 100 && item.line_No % 100 == 0)
                {
                    //Frank 20160129: Here we only culcalate one system's price, so the itp, unit price and list price need to be divided by system total qty
                    var subItems = items.Where(p => p.HigherLevel == item.line_No && !p.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase));
                    int _ParentItemQty = item.qty.Value;
                    if (_ParentItemQty < 1) { _ParentItemQty = 1; }
                    //itp = subItems.Sum(p => p.newItp * p.qty.Value).Value;
                    itp = subItems.Sum(p => p.newItp * p.qty.Value).Value / _ParentItemQty;
                    SubTotal = subItems.Sum(p => p.newUnitPrice * p.qty.Value).Value;
                    newUnitPrice = SubTotal / _ParentItemQty;
                    listPrice = (subItems.Sum(p => p.listPrice * p.qty.Value).Value) / _ParentItemQty;
                }

                if (newUnitPrice > 0)
                {
                    ItemMargin = (newUnitPrice - itp) / newUnitPrice * 100;
                }
                else
                {
                    ItemMargin = 0;
                }

                str += "<tr>";
                ItemMarginStr = Math.Round(ItemMargin, 2).ToString("N2") + "%";

                //if (item.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase)) { ItemMarginStr = ""; }
                //if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { ItemMarginStr = ""; }
                if (item.IsServicePartX) { ItemMarginStr = ""; }

                str += _td + item.line_No + "</td>" + _td + item.partNo + "</td>" + _td + _QMaster.currency + " " + listPrice.ToString("N2") + "</td>" + _td + _QMaster.currency + " " + newUnitPrice.ToString("N2") + "</td>" + _td + item.qty.ToString() + "</td>" + _td + _QMaster.currency + " " + SubTotal.ToString("N2") + "</td>" + _td + ItemMarginStr + "</td>" + _td + item.sprNo + "</td>";
                var ckstr = "";
                if (null != GPitems.FirstOrDefault(p => p.line_No == item.line_No && p.partNo == item.partNo))
                {
                    if (ItemMargin > MaxGPPercent)
                        ckstr = "<Font color='red'>Below min price.</font>";
                    else
                        ckstr = "<Font color='red'>Below GP.</font>";
                }
                str += _td + ckstr + "</td>";
                str += "</tr>";
            }

            str += "</TABLE>";

            return str;

        }

        public static string getGPmailBodyForANAAonlineIIoTApprover(string QuoteID)
        {
            QuotationMaster _QMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            List<QuotationDetail> items = _QMaster.QuotationDetail;
            decimal TotalAmount = 0; decimal TotalITP = 0;
            decimal TotalMargin = 0; string msg = string.Empty;
            var GPitems = new List<QuotationDetail>(); decimal Rate = 0;
            IsNeedGPforANAIIoTAOnline(QuoteID, ref Rate, ref GPitems);
            //Frank 20151222 exculde quote items which has spr no.
            SAPDAL.SAPDAL _OldSAPDAL = new SAPDAL.SAPDAL();

            foreach (QuotationDetail item in items)
            {
                //if (item.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                if (item.IsServicePartX) { continue; }
                if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { continue; }
                if (item.partNo.Equals("Freight_Fee", StringComparison.OrdinalIgnoreCase)) { continue; }
                //if (Advantech.Myadvantech.Business.PartBusinessLogic.IsServicePart(item.partNo, _QMaster.org)) { continue; }

                // Ryan 20171025 Add for service part check through SQL select due to unknown context error in PartBusinessLogic.IsServicePart
                var objGENITEMCATGRP = SqlProvider.dbExecuteScalar("MY", String.Format("SELECT top 1 a.GENITEMCATGRP FROM SAP_PRODUCT a inner join SAP_PRODUCT_ORG b on a.PART_NO = b.PART_NO WHERE a.PART_NO = '{0}' and b.ORG_ID = '{1}'", item.partNo, _QMaster.org));
                if (objGENITEMCATGRP != null && !String.IsNullOrEmpty(objGENITEMCATGRP.ToString()))
                {
                    if (objGENITEMCATGRP.ToString().ToUpper().Equals("DIEN") || objGENITEMCATGRP.ToString().ToUpper().Equals("LEIS"))
                        continue;
                }

                TotalAmount += item.newUnitPrice.Value * item.qty.Value;
                TotalITP += item.newItp.Value * item.qty.Value;
            }
            if (TotalAmount > 0)
            {
                TotalMargin = (TotalAmount - TotalITP) / TotalAmount;
            }
            else
            {
                TotalMargin = 0;
            }

            // Calculate max gp percent rate
            //DataTable _dtGPParas = eQuotationDAL.getGP_Parameter("Intercon");




            Decimal MaxGPPercent = (Decimal)0.20;
            //foreach (DataRow itemGP in _dtGPParas.Rows)
            //{
            //    if (Convert.ToDecimal(itemGP["GP_Percent"].ToString().Trim()) > MaxGPPercent)
            //        MaxGPPercent = Convert.ToDecimal(itemGP["GP_Percent"].ToString().Trim());
            //}
            MaxGPPercent = MaxGPPercent * 100;

            string str = "";
            str += "<table style='width:80%;'  class='table table-bordered'><tr><td style=\"font-family:Arial;font-weight:bold;\"><nobr>Acount Name</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Acount Id</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Total</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Margin</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Create By</nobr></td></TR>";
            if ((_QMaster != null))
            {
                str += "<tr>";
                str += "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.quoteToName + "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.quoteToErpId + "</nobr></td>";

                str += "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.currency + ": " + (items.Sum(p => p.qty * p.newUnitPrice)).Value.ToString("N2") + "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + (TotalMargin).ToString("P") + "</nobr></td>";
                str += "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.createdBy + "</nobr></td>";
                str += "</TR>";

                StringBuilder SB = new StringBuilder();
                SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>");
                SB.AppendFormat(" <ul>");
                SB.AppendFormat("<li>Below GP</li>");
                SB.AppendFormat("</ul>");
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;text-align:left;\">" + SB.ToString() + "</td></tr>";
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;text-align:left;\"><b>Reason: </b>" + _QMaster.relatedInfo + "</td></tr>";
            }
            str += "</table>";
            str += "<HR/>";

            string _tdHeader = "<td style='border:1px solid darkgray;border-collapse:collapse;font-family:Arial;font-weight:bold;text-align:center;'>";
            string _td = "<td style='border:1px solid darkgray;border-collapse:collapse;font-family:Arial;text-align:center;'>";

            str += "<table style='border:1px black solid;' rules='all' cellpadding='1'; width='99%' ><tr>" + _tdHeader + "Line No</td>" + _tdHeader + "Part No</td>" + _tdHeader + "List Price</td>" + _tdHeader + "Unit Price</td>" + _tdHeader + "QTY</TD>" + _tdHeader + "Sub Total</td>" + _tdHeader + "Margin</td>" + _tdHeader + "SPR</td>" + _tdHeader + "Note</td></TR>";
            decimal itp = 0, newUnitPrice = 0, listPrice = 0, SubTotal = 0, ItemMargin = 0;
            string ItemMarginStr = string.Empty;
            foreach (var item in items)
            {
                itp = item.newItp.Value;
                listPrice = item.listPrice.Value;
                newUnitPrice = item.newUnitPrice.Value;
                SubTotal = item.newUnitPrice.Value * item.qty.Value;
                Boolean isBelowL2Price = false;
                String errmsg = "";
                Decimal L2Price = _OldSAPDAL.getPriceByProdLine(item.partNo, _OldSAPDAL.getProdPricingGrp(item.partNo), ref errmsg);

                //Check if new unit price is lower than L2 price
                //If so then the quote needs to be approved by first line manager Matt.Dentino(IAG) / Daniel.Tom(ISG)
                if (item.newUnitPrice < L2Price)
                {
                    isBelowL2Price = true;
                }

                if (item.line_No >= 100 && item.line_No % 100 == 0)
                {
                    //Frank 20160129: Here we only culcalate one system's price, so the itp, unit price and list price need to be divided by system total qty
                    var subItems = items.Where(p => p.HigherLevel == item.line_No && !p.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase));
                    int _ParentItemQty = item.qty.Value;
                    if (_ParentItemQty < 1) { _ParentItemQty = 1; }
                    //itp = subItems.Sum(p => p.newItp * p.qty.Value).Value;
                    itp = subItems.Sum(p => p.newItp * p.qty.Value).Value / _ParentItemQty;
                    SubTotal = subItems.Sum(p => p.newUnitPrice * p.qty.Value).Value;
                    newUnitPrice = SubTotal / _ParentItemQty;
                    listPrice = (subItems.Sum(p => p.listPrice * p.qty.Value).Value) / _ParentItemQty;
                }

                if (newUnitPrice > 0)
                {
                    ItemMargin = (newUnitPrice - itp) / newUnitPrice * 100;
                }
                else
                {
                    ItemMargin = 0;
                }

                str += "<tr>";
                ItemMarginStr = Math.Round(ItemMargin, 2).ToString("N2") + "%";

                if (item.IsServicePartX) { ItemMarginStr = ""; }

                str += _td + item.line_No + "</td>" + _td + item.partNo + "</td>" + _td + _QMaster.currency + " " + listPrice.ToString("N2") + "</td>" + _td + _QMaster.currency + " " + newUnitPrice.ToString("N2") + "</td>" + _td + item.qty.ToString() + "</td>" + _td + _QMaster.currency + " " + SubTotal.ToString("N2") + "</td>" + _td + ItemMarginStr + "</td>" + _td + item.sprNo + "</td>";
                var ckstr = "";
                if (null != GPitems.FirstOrDefault(p => p.line_No == item.line_No && p.partNo == item.partNo))
                {
                    if (isBelowL2Price && ItemMargin > MaxGPPercent)
                    {
                        ckstr = "<Font color='red'>Below L2 Price Grade.</font>";
                    }
                    else if (ItemMargin <= MaxGPPercent)
                    {
                        ckstr = "<Font color='red'>Below GP.</font>";
                    }
                }
                str += _td + ckstr + "</td>";
                str += "</tr>";
            }

            str += "</TABLE>";

            return str;

        }


        public static void InitQuotApprove(string QuoteID, string WorkFlowID, string url)
        {
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            if (QM == null) return;

            //if (string.Equals(QM.siebelRBU, "HQDC"))
            if (QM.quoteNo.StartsWith("AIAQ") || QM.quoteNo.StartsWith("AIEQ") || QM.quoteNo.StartsWith("AISQ"))
            {

                //DataTable dt = eQuotationDAL.getGP_Parameter("INTERCON");
                DataTable dt = null;

                if (QM.quoteNo.StartsWith("AIAQ"))
                {
                    //dt = eQuotationDAL.getGP_Parameter("INTERCON");
                    string _GPRBU = "INTERCON-IA";
                    //Frank: implement Julia's new rules here to identify what the GP approval flow is
                    if (QM.siebelRBU.Equals("ARU", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _GPRBU = "INTERCON-IA-ARU";
                    }
                    else if (QM.siebelRBU.Equals("HQDC", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Get account's country from Siebel
                        DataTable _dt = new DataTable();
                        SiebelDAL _siebeldal = new SiebelDAL();
                        SIEBEL_ACCOUNT _account = SiebelDAL.getSiebelAccount(QM.quoteToRowId);
                        if (_account != null && _account.COUNTRY.Equals("New Zealand", StringComparison.InvariantCultureIgnoreCase) == false)
                        {
                            _GPRBU = "INTERCON-IA-MEA";
                        }
                    }
                    else if (QM.siebelRBU.Equals("ATR", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _GPRBU = "INTERCON-IA-MEA";
                    }
                    else if (QM.siebelRBU.Equals("LATAM", StringComparison.InvariantCultureIgnoreCase)
                        || QM.siebelRBU.Equals("AMX", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _GPRBU = "INTERCON-IA-LATAM";
                    }
                    dt = eQuotationDAL.getGP_Parameter(_GPRBU);
                }
                else if (QM.quoteNo.StartsWith("AIEQ"))
                {
                    dt = eQuotationDAL.getGP_Parameter("INTERCON-EC");
                }
                else if (QM.quoteNo.StartsWith("AISQ"))
                {
                    //Ryan 20171102 Intercon IS new logic here, check its own mail group and go approval
                    if (UserRoleBusinessLogic.IsInMailGroup("InterCon.iService", QM.createdBy))
                        dt = eQuotationDAL.getGP_Parameter("InterCon.iService");
                    else if (UserRoleBusinessLogic.IsInMailGroup("InterCon.iLogistic", QM.createdBy))
                        dt = eQuotationDAL.getGP_Parameter("InterCon.iLogistic");
                    else
                        dt = eQuotationDAL.getGP_Parameter("INTERCON-IS");
                }

                IEnumerable<DataRow> drs = dt.AsEnumerable().OrderByDescending(p => p["GP_Percent"]);
                Int32 i = 1;
                decimal Rate = 0;
                var GPitems = new List<QuotationDetail>();
                IsNeedGPforIntercon(QuoteID, ref Rate, ref GPitems);
                foreach (var item in drs)
                {
                    decimal _GPpercent = 0;
                    decimal.TryParse(item["GP_Percent"].ToString().Trim(), out _GPpercent);
                    if (_GPpercent != 0 && Rate != 0 && Rate <= _GPpercent)
                    {
                        QuoteApproval QA = new QuoteApproval();
                        QA.UID = System.Guid.NewGuid().ToString();
                        QA.QuoteID = QuoteID;
                        QA.GPBrief = "INTERCON GP";
                        QA.LevelNum = i;
                        QA.Approver = "myadvantech@advantech.com";
                        if (!string.IsNullOrEmpty(item["Approver"].ToString()))
                        {
                            QA.Approver = item["Approver"].ToString().Trim();
                        }
                        QA.Status = (int)QuoteApprovalStatus.Wait_for_review;
                        QA.WorkFlowID = WorkFlowID;
                        QA.IsSendSuccessfully = 0;
                        QA.MobileYes = "MY" + System.Guid.NewGuid().ToString();
                        QA.MobileNo = "MN" + System.Guid.NewGuid().ToString();
                        QA.CreatedDate = DateTime.Now;
                        QA.LastUpdatedDate = DateTime.Now;
                        //QA.Mailbody = getGPmailBodyForIntercon(QuoteID); 
                        QA.Mailbody = getGPmailBodyForInterconApprover(QuoteID);
                        QA.Url = url;
                        QA.Add();
                        i++;
                    }

                }
            }
            else if (QM.quoteNo.StartsWith("ABRQ"))
            {

                DataTable dt = null;

                String _salescode = GetSalesCodeByQuoteID(QuoteID);

                //dt = eQuotationDAL.getGP_Parameter("ABR-IIoT");
                String _sector = UserRoleBusinessLogic.getABRSectorBySalesCode(_salescode);
                String _GPParameter = ConvertABRSectorToGPFlowParameter(_sector); //"ABR-IIoT";
                dt = eQuotationDAL.getGP_Parameter(_GPParameter);

                IEnumerable<DataRow> drs = dt.AsEnumerable().OrderByDescending(p => p["GP_Percent"]);
                Int32 i = 1;
                decimal Rate = 0;
                var GPitems = new List<QuotationDetail>();

                IsNeedGPforABR(QuoteID, ref Rate, ref GPitems);
                foreach (var item in drs)
                {
                    decimal _GPpercent = 0;
                    decimal.TryParse(item["GP_Percent"].ToString().Trim(), out _GPpercent);
                    if (_GPpercent != 0 && Rate != 0 && Rate <= _GPpercent)
                    {
                        QuoteApproval QA = new QuoteApproval();
                        QA.UID = System.Guid.NewGuid().ToString();
                        QA.QuoteID = QuoteID;
                        QA.GPBrief = "ABR GP";
                        QA.LevelNum = i;
                        QA.Approver = "myadvantech@advantech.com";
                        if (!string.IsNullOrEmpty(item["Approver"].ToString()))
                        {
                            QA.Approver = item["Approver"].ToString().Trim();
                        }
                        QA.Status = (int)QuoteApprovalStatus.Wait_for_review;
                        QA.WorkFlowID = WorkFlowID;
                        QA.IsSendSuccessfully = 0;
                        QA.MobileYes = "MY" + System.Guid.NewGuid().ToString();
                        QA.MobileNo = "MN" + System.Guid.NewGuid().ToString();
                        QA.CreatedDate = DateTime.Now;
                        QA.LastUpdatedDate = DateTime.Now;
                        //QA.Mailbody = getGPmailBodyForIntercon(QuoteID); 
                        QA.Mailbody = getGPmailBodyForInterconApprover(QuoteID);
                        QA.Url = url;
                        QA.Add();
                        i++;
                    }

                }

            }
            else if (QM.quoteNo.StartsWith("AENQ"))
            {
                DataTable dt = eQuotationDAL.getGP_Parameter("AENC");
                QuoteApproval QA = new QuoteApproval();
                QA.UID = System.Guid.NewGuid().ToString();
                QA.QuoteID = QuoteID;
                QA.GPBrief = "AENC GP 1.1";
                QA.LevelNum = 1;
                QA.Approver = "myadvantech@advantech.com";
                if (dt.Rows.Count == 1 && !string.IsNullOrEmpty(dt.Rows[0]["Approver"].ToString()))
                {
                    QA.Approver = dt.Rows[0]["Approver"].ToString().Trim();
                }

                QA.Status = (int)QuoteApprovalStatus.Wait_for_review;
                QA.WorkFlowID = WorkFlowID;
                QA.IsSendSuccessfully = 0;
                QA.MobileYes = "MY" + System.Guid.NewGuid().ToString();
                QA.MobileNo = "MN" + System.Guid.NewGuid().ToString();
                QA.CreatedDate = DateTime.Now;
                QA.LastUpdatedDate = DateTime.Now;
                QA.Mailbody = getGPmailBodyForAUS(QuoteID);
                QA.Url = url;
                QA.Add();
            }
            else if (QM.quoteNo.StartsWith("AUSQ")) // ANA AOnline
            {
                String _Team = String.Empty;

                String _SalesCode = GetSalesCodeByQuoteID(QuoteID);
                String _OfficeCode = String.Empty;
                String _GroupCode = String.Empty;
                UserRoleBusinessLogic.GetSalesOfficeAndGroupCodeBySalesCode(_SalesCode, out _OfficeCode, out _GroupCode);

                switch (_OfficeCode)
                {
                    case "2700":
                        _Team = "ANAAONLINE";
                        break;

                    case "2110":
                        _Team = "ANA-IIoT-IAG-AOnline";
                        switch (_GroupCode)
                        {
                            case "219":
                                _Team = "ANA-IIoT-IAG-AOnline";
                                break;
                            case "21A":
                                _Team = "ANA-IIoT-ISG-AOnline";
                                break;
                            default:
                                _Team = String.Empty;
                                break;
                        }
                        break;
                    default:
                        _Team = String.Empty;
                        break;
                }

                if (String.IsNullOrEmpty(_Team))
                {
                    if (UserRoleBusinessLogic.IsInMailGroup("Aonline.USA", QM.createdBy))
                    {
                        _Team = "ANAAONLINE";
                    }
                    else if (UserRoleBusinessLogic.IsInMailGroup("Aonline.USA.IAG", QM.createdBy))
                    {
                        _Team = "ANA-IIoT-IAG-AOnline";
                    }
                    else if (UserRoleBusinessLogic.IsInMailGroup("Aonline.USA.iSystem", QM.createdBy))
                    {
                        _Team = "ANA-IIoT-ISG-AOnline";
                    }
                }


                DataTable dt = eQuotationDAL.getGP_Parameter(_Team);

                IEnumerable<DataRow> drs = dt.AsEnumerable().OrderByDescending(p => p["GP_Percent"]);
                Int32 i = 1;
                decimal Rate = 0;
                var GPitems = new List<QuotationDetail>();

                IsNeedGPforANAIIoTAOnline(QuoteID, ref Rate, ref GPitems);

                foreach (var item in drs)
                {
                    decimal _GPpercent = 0;
                    decimal.TryParse(item["GP_Percent"].ToString().Trim(), out _GPpercent);
                    if (_GPpercent != 0 && Rate != 0 && Rate <= _GPpercent)
                    {
                        QuoteApproval QA = new QuoteApproval();
                        QA.UID = System.Guid.NewGuid().ToString();
                        QA.QuoteID = QuoteID;
                        QA.GPBrief = "ANAAonline GP 2.0";
                        QA.LevelNum = i;
                        QA.Approver = "myadvantech@advantech.com";
                        if (!string.IsNullOrEmpty(item["Approver"].ToString()))
                        {
                            QA.Approver = item["Approver"].ToString().Trim();
                        }
                        QA.Status = (int)QuoteApprovalStatus.Wait_for_review;
                        QA.WorkFlowID = WorkFlowID;
                        QA.IsSendSuccessfully = 0;
                        QA.MobileYes = "MY" + System.Guid.NewGuid().ToString();
                        QA.MobileNo = "MN" + System.Guid.NewGuid().ToString();
                        QA.CreatedDate = DateTime.Now;
                        QA.LastUpdatedDate = DateTime.Now;
                        //QA.Mailbody = getGPmailBodyForIntercon(QuoteID); 
                        if (_Team.Equals("ANAAONLINE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            QA.Mailbody = getGPmailBodyForAUS(QuoteID);
                        }
                        else
                        {
                            QA.Mailbody = getGPmailBodyForANAAonlineIIoTApprover(QuoteID);
                        }
                        QA.Url = url;
                        QA.Add();
                        i++;
                    }

                }

                //QuoteApproval QA = new QuoteApproval();
                //QA.UID = System.Guid.NewGuid().ToString();
                //QA.QuoteID = QuoteID;
                //QA.GPBrief = "ANAAonline GP 2.0";
                //QA.LevelNum = 1;
                //QA.Approver = "myadvantech@advantech.com";
                //if (dt.Rows.Count == 1 && !string.IsNullOrEmpty(dt.Rows[0]["Approver"].ToString()))
                //{
                //    QA.Approver = dt.Rows[0]["Approver"].ToString().Trim();
                //}

                //QA.Status = (int)QuoteApprovalStatus.Wait_for_review;
                //QA.WorkFlowID = WorkFlowID;
                //QA.IsSendSuccessfully = 0;
                //QA.MobileYes = "MY" + System.Guid.NewGuid().ToString();
                //QA.MobileNo = "MN" + System.Guid.NewGuid().ToString();
                //QA.CreatedDate = DateTime.Now;
                //QA.LastUpdatedDate = DateTime.Now;
                //QA.Mailbody = getGPmailBodyForAUS(QuoteID);
                //QA.Url = url;
                //QA.Add();
            }
            else
            {

                QuoteApproval QA = new QuoteApproval();
                DataTable dt = null;
                switch (QM.org.ToUpper())
                {
                    case "KR01":
                        dt = eQuotationDAL.getGP_Parameter("AKR");
                        QA.UID = System.Guid.NewGuid().ToString();
                        QA.QuoteID = QuoteID;
                        QA.GPBrief = "AKR GP 1.1";
                        QA.LevelNum = 1;
                        QA.Approver = "myadvantech@advantech.com";
                        if (dt.Rows.Count == 1 && !string.IsNullOrEmpty(dt.Rows[0]["Approver"].ToString()))
                        {
                            QA.Approver = dt.Rows[0]["Approver"].ToString().Trim();
                        }

                        //Frank 20170717 based on report line to assign the quote's Approver

                        //String _reapapprover = eQuotationDAL.getAKRGP_Approver(QM.salesEmail);
                        String _sales = QuoteBusinessLogic.GetSalesEmailByQuoteID(QM.quoteId);
                        String _reapapprover = eQuotationDAL.getAKRGP_Approver(_sales);

                        if (!string.IsNullOrEmpty(_reapapprover))
                        {
                            QA.Approver = _reapapprover;
                        }

                        QA.Status = (int)QuoteApprovalStatus.Wait_for_review;
                        QA.WorkFlowID = WorkFlowID;
                        QA.IsSendSuccessfully = 0;
                        QA.MobileYes = "MY" + System.Guid.NewGuid().ToString();
                        QA.MobileNo = "MN" + System.Guid.NewGuid().ToString();
                        QA.CreatedDate = DateTime.Now;
                        QA.LastUpdatedDate = DateTime.Now;
                        QA.Mailbody = getGPmailBodyForAKR(QuoteID);
                        QA.Url = url;
                        QA.Add();
                        break;
                    default://AAC
                        QA.UID = System.Guid.NewGuid().ToString();
                        QA.QuoteID = QuoteID;
                        QA.GPBrief = "AAC GP 1.1";
                        QA.LevelNum = 1;
                        QA.Approver = "gpapproval.aac@advantech.com";
                        QA.Status = (int)QuoteApprovalStatus.Wait_for_review;
                        QA.WorkFlowID = WorkFlowID;
                        QA.IsSendSuccessfully = 0;
                        QA.MobileYes = "MY" + System.Guid.NewGuid().ToString();
                        QA.MobileNo = "MN" + System.Guid.NewGuid().ToString();
                        QA.CreatedDate = DateTime.Now;
                        QA.LastUpdatedDate = DateTime.Now;
                        QA.Mailbody = getGPmailBodyForAUS(QuoteID);
                        QA.Url = url;
                        QA.Add();
                        break;
                }
            }


        }
        public static string GetNameVonEmail(string email)
        {
            if (email == null) return string.Empty;
            if (email.Contains("@"))
            {
                string strNamePart = email.Split(new char[] { '@' })[0];
                //字首改大寫，其他改小寫 ex:tc.chen ->Tc.Chen
                string[] strNameArry = strNamePart.Split(new char[] { '.' });
                for (int i = 0; i <= strNameArry.Length - 1; i++)
                {
                    if (strNameArry[i].Length > 1)
                    {
                        strNameArry[i] = strNameArry[i].Substring(0, 1).ToUpper() + strNameArry[i].Substring(1).ToLower();
                    }
                    else
                    {
                        strNameArry[i] = strNameArry[i].ToUpper();
                    }
                }
                strNamePart = string.Join(".", strNameArry);
                return strNamePart;
            }
            else
            {
                return email;
            }
        }
        public static DateTime GetLocalTime(string org, DateTime ServerTime)
        {
            org = org.ToUpper().Trim();
            DateTime utcTime = DateTime.Now.ToUniversalTime();
            Dictionary<string, string> Org2Timezone = new Dictionary<string, string>();
            if (HttpContext.Current != null)
                Org2Timezone = (Dictionary<string, string>)HttpContext.Current.Cache["Org2Timezone"];
            if (Org2Timezone == null && HttpContext.Current != null)
            {
                Org2Timezone = new Dictionary<string, string>();
                HttpContext.Current.Cache.Add("Org2Timezone", Org2Timezone, null, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            }
            if (!Org2Timezone.ContainsKey(org))
            {
                string timezone = eQuotationDAL.getTimeZoneName(org);
                if (timezone != null && !string.IsNullOrEmpty(timezone))
                {
                    Org2Timezone.Add(org, timezone.ToString());
                }
            }
            if (!string.IsNullOrEmpty(Org2Timezone[org]))
            {
                TimeZoneInfo TZ_Local = TimeZoneInfo.FindSystemTimeZoneById(Org2Timezone[org]);
                TimeZoneInfo TZI_Tw = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                TimeSpan TimeDifference = TZ_Local.GetUtcOffset(utcTime) - TZI_Tw.GetUtcOffset(utcTime);
                return ServerTime.Add(TimeDifference);
            }
            return DateTime.Now;
        }
        public static bool checkWFData(string QuoteID)
        {
            IEnumerable<QuoteApproval> QAlist = eQuotationContext.Current.QuoteApproval.Where(p => p.QuoteID == QuoteID);
            if (QAlist.Count() > 0) return true;
            return false;
        }
        public static bool getApproverStatusforAAC(string QuoteID, ref string statustxt, ref bool isGP)
        {
            int Approved = (int)QuoteApprovalStatus.Approved;
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            IEnumerable<QuoteApproval> QAlist = eQuotationContext.Current.QuoteApproval.Where(p => p.QuoteID == QuoteID).OrderBy(p => p.LevelNum).ToList();
            if (QAlist.Count() == 0) return false;
            if (QAlist.Count() > 0) isGP = true;

            IEnumerable<QuoteApproval> QAWlist = QAlist.Where(p => p.Status == Wait).OrderBy(p => p.LevelNum).ToList();
            IEnumerable<QuoteApproval> QARlist = QAlist.Where(p => p.Status == Rejected).OrderBy(p => p.LevelNum).ToList();
            StringBuilder sb = new StringBuilder();
            string Currentstatus = string.Empty;
            sb.AppendFormat("<table style='display:none;' id='{4}' boder=1><tr><th width=77>{0}</th><th width=180>{1}</th><th width=80>{2}</th><th width=70>{3}</th></tr>", "Stage", "By", "DateTime", "Remarks", QuoteID);
            if (QAlist.Count() > 0)
            {
                Boolean isPending = false, isRejected = false;// string rejectTempStr = string.Empty;
                foreach (QuoteApproval x in QAlist)
                {

                    string img = string.Empty; string date = string.Empty; string reson = string.Empty; string leader = string.Empty;
                    switch (x.Status)
                    {
                        case -1:
                            img = "Rejected.png";
                            Currentstatus = string.Format("Stage：Rejected by {0}<span> <img src='../images/CheckList.png'/></span>", GetNameVonEmail(x.RejectedBy));
                            if (x.RejectedDate.HasValue) date = GetLocalTime("A" + QM.org.Substring(0, 2), x.RejectedDate.Value).ToString("MM/dd/yyyy HH:mm");
                            reson = x.RejectReason;
                            leader = x.RejectedBy;
                            //if (string.IsNullOrEmpty(rejectTempStr)) rejectTempStr = Currentstatus;
                            isRejected = true;
                            break;
                        case 0:
                            img = "Current.png";
                            Currentstatus = string.Format("Stage：Waiting for {0}<span> <img src='../images/CheckList.png' /> </span>", GetNameVonEmail(x.Approver));
                            if (isPending) img = "Pending.png";
                            isPending = true;
                            leader = x.Approver;
                            break;
                        case 1:
                            img = "Approved.png";
                            Currentstatus = string.Format("Stage：Approved by {0}<span> <img src='../images/CheckList.png' />  </span>", GetNameVonEmail(x.ApprovedBy));
                            if (x.ApprovedDate.HasValue) date = GetLocalTime("A" + QM.org.Substring(0, 2), x.ApprovedDate.Value).ToString("MM/dd/yyyy HH:mm");
                            reson = x.ApprovedReason;
                            leader = x.ApprovedBy;
                            break;
                        default:
                            break;
                    }
                    if (string.Equals(img, "Pending.png"))
                    {
                        sb.AppendFormat("<tr><td style='text-align:center;'></td><td>{0}</td><td width=110px>{1}</td><td>{2}</td></tr>", leader, date, HttpContext.Current.Server.HtmlEncode(reson));
                    }
                    else
                    {
                        sb.AppendFormat("<tr><td style='text-align:center;'><img src='../images/{0}'></td><td>{1}</td><td width=110px>{2}</td><td>{3}</td></tr>", img, leader, date, HttpContext.Current.Server.HtmlEncode(reson));
                    }

                    // if (isPending || isRejected) break;
                    if (isRejected) break;
                }
                sb.Append("</table>");
                statustxt = "<div class='divgp'>" + Currentstatus + sb.ToString() + "</div>";
                if (QAWlist.Count() > 0)
                {
                    var waititem = QAWlist.FirstOrDefault();
                    if (waititem != null)
                    {
                        Currentstatus = string.Format("Stage：Waiting for {0}<span> <img src='../images/CheckList.png' /> </span>", GetNameVonEmail(waititem.Approver));
                        statustxt = "<div class='divgp'>" + Currentstatus + sb.ToString() + "</div>";
                    }

                }
                if (QARlist.Count() > 0)
                {
                    var Ritem = QARlist.FirstOrDefault();
                    if (Ritem != null)
                    {
                        Currentstatus = string.Format("Stage：Rejected by {0}<span> <img src='../images/CheckList.png' /> </span>", GetNameVonEmail(Ritem.Approver));
                        statustxt = "<div class='divgp'>" + Currentstatus + sb.ToString() + "</div>";
                    }

                }

            }
            if (QAWlist.Count() > 0 || QARlist.Count() > 0) return true;
            return false;
        }
        public static void SendGPmail(string QuoteID)
        {
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            //  if (QM == null) return;
            int Approved = (int)QuoteApprovalStatus.Approved;
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;
            string cc = "myadvantech@advantech.com", subject = "";
            StringBuilder sbComment = new StringBuilder();
            bool IsTesting = true;
            IEnumerable<QuoteApproval> QAlist = eQuotationContext.Current.QuoteApproval.Where(p => p.QuoteID == QuoteID).OrderBy(p => p.LevelNum).ToList();
            string Mailbody = QAlist.FirstOrDefault().Mailbody + "<style>  th { background-color:#C60; color:#FFF;}</style>";
            string url = QAlist.FirstOrDefault().Url.ToString().ToUpper();

            //if (url.Contains("eq.advantech.com/".ToUpper())) IsTesting = false;
            if (url.Equals("http://eq.advantech.com", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;
            if (url.Equals("https://eq.advantech.com", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;

            int MaxLevelNum = QAlist.Max(p => p.LevelNum);
            QuoteApproval approvedMax = QAlist.Where(p => (p.Status == Approved || p.Status == Rejected) && p.LevelNum == MaxLevelNum).FirstOrDefault();
            if (approvedMax != null)
            {
                string txt = "approved";
                IEnumerable<QuoteApproval> approvedlist = QAlist.Where(p => p.Status == Approved).OrderBy(p => p.LevelNum);
                foreach (var item in approvedlist)
                {
                    sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0}</b> </td><td style=\"padding:1px 15px;\">by {1} on {2}</td></tr>", "Approved", item.ApprovedBy, GetLocalTime("A" + QM.org.Substring(0, 2), item.ApprovedDate.Value).ToString("MM/dd/yyyy HH:mm"));
                    sbComment.AppendFormat("</table>");
                }
                IEnumerable<QuoteApproval> approvedlist2 = QAlist.Where(p => p.Status == Rejected).OrderBy(p => p.LevelNum);
                foreach (var item in approvedlist2)
                {
                    txt = "rejected";
                    sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0}</b> </td><td style=\"padding:1px 15px;\">by {1} on {2}. {3}</td></tr>", "Rejected", item.RejectedBy, GetLocalTime("A" + QM.org.Substring(0, 2), item.RejectedDate.Value).ToString("MM/dd/yyyy HH:mm"), string.IsNullOrEmpty(item.RejectReason) || (item.RejectReason != null && item.RejectReason.Contains("MobileApprove")) ? "" : "Reason: " + item.RejectReason);
                    sbComment.AppendFormat("</table>");
                }
                subject = string.Format("Quote ({0}) has been {1}", QM.quoteNo, txt);

                //If is Intercon's quote, needs to append OP to CC list here
                if (QM.quoteNo.StartsWith("AIAQ") || QM.quoteNo.StartsWith("AIEQ") || QM.quoteNo.StartsWith("AISQ"))
                {
                    QuotationExtension QEXT = eQuotationContext.Current.QuotationExtension.Find(QuoteID);
                    if (QEXT != null && !string.IsNullOrEmpty(QEXT.Engineer))
                    {
                        cc += "," + QEXT.Engineer;
                    }
                }

                //Ryan 20170810 If is US AENC quote, reset mailbody content due to ITP/GP info should not be sent to sales per Esther's request.
                Mailbody = getGPmailBodyForAENCWithoutGPITP(QuoteID);

                COMM.Util.SendEmailV3("myadvantech@advantech.com", QM.createdBy, cc, "", subject, "", Mailbody + sbComment.ToString(), "", IsTesting);
                return;
            }
            for (int i = 1; i <= MaxLevelNum; i++)
            {
                IEnumerable<QuoteApproval> currlist = from a in QAlist where a.LevelNum == i select a;

                QuoteApproval approved = currlist.Where(p => p.Status == Approved).FirstOrDefault();
                if (approved != null)
                {
                    sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0}</b></td><td style=\"padding:1px 15px;\">by {1} on {2}</td></tr>", "Approved", approved.ApprovedBy, GetLocalTime("A" + QM.org.Substring(0, 2), approved.ApprovedDate.Value).ToString("MM/dd/yyyy HH:mm"));
                    sbComment.AppendFormat("</table>");
                    continue;

                }
                QuoteApproval rejected = currlist.Where(p => p.Status == Rejected).FirstOrDefault();
                if (rejected != null)
                {
                    sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0}</b> Reason:{3}</td><td style=\"padding:1px 15px;\">by {1} on {2}</td></tr>", "Rejected", rejected.RejectedBy, GetLocalTime("A" + QM.org.Substring(0, 2), rejected.RejectedDate.Value).ToString("MM/dd/yyyy HH:mm"), rejected.RejectReason);
                    sbComment.AppendFormat("</table>");
                    subject = string.Format("Quote ({0}) has been rejected", QM.quoteNo);

                    COMM.Util.SendEmailV3("myadvantech@advantech.com", QM.createdBy, cc, "", subject, "", Mailbody + sbComment.ToString(), "", IsTesting);//拒绝信
                    return;
                }
                IEnumerable<QuoteApproval> Waitlist = currlist.Where(p => p.Status == Wait);
                if (Waitlist.Any())
                {
                    foreach (QuoteApproval item in Waitlist)
                    {
                        subject = string.Format("eQuotation Approval request for:({0} V{1})", QM.quoteNo, QM.Revision_Number);
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("<BR/><table>");
                        //sb.AppendFormat("<tr><td width=100 align='center'><a  href='{0}/GPPad/MobileApprove.aspx?MID={1}'>Approve</a></td><td> | </td><td width=100 align='center'><a  href='{0}/GPPad/MobileApprove.aspx?MID={2}'>Reject</a></td></tr>", url, item.MobileYes, item.MobileNo);
                        sb.AppendFormat("<tr><td width=100 align='center'><a  href='{0}/GPPad/MobileApprove.aspx?MID={1}'>Approve</a></td><td> | </td><td width=100 align='center'><a  href='{0}/GPPad/MobileApprove.aspx?MID={3}'>Reject</a></td>", url, item.MobileYes, item.MobileNo, item.MobileNo);
                        sb.AppendFormat("<td> | </td><td width=230 align='center' ><a  href='{0}/GPPad/Approval.aspx?UID={1}'> check quote detail</a></td></tr>", url, item.UID);

                        sb.AppendFormat("</table>");

                        // Ryan 20171030 If is Intercon quote, check agent existed or not and put it into cc list.
                        var agentemail = SqlProvider.dbExecuteScalar("EQ", String.Format("select top 1 aemail from Agent where email = '{0}' and  GETDATE() > fDate and GETDATE() < tDate ", item.Approver));
                        if (agentemail != null && !string.IsNullOrEmpty(agentemail.ToString()))
                        {
                            cc += "," + agentemail.ToString();
                        }

                        COMM.Util.SendEmailV3("myadvantech@advantech.com", item.Approver, cc, "", subject, "", Mailbody + sbComment.ToString() + sb.ToString(), "", IsTesting);

                    }
                    break;
                }

            }
        }
        public static bool IsApprovedByAll(string QuoteID)
        {

            int Approved = (int)QuoteApprovalStatus.Approved;
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;
            IEnumerable<QuoteApproval> QAlist = eQuotationContext.Current.QuoteApproval.Where(p => p.QuoteID == QuoteID).OrderBy(p => p.LevelNum).ToList();
            int MaxLevelNum = QAlist.Max(p => p.LevelNum);
            QuoteApproval approvedMax = QAlist.Where(p => (p.Status == Approved || p.Status == Rejected) && p.LevelNum == MaxLevelNum).FirstOrDefault();
            if (approvedMax != null)
            {
                return true;
            }
            return false;

        }
        public static bool IsRejected(string QuoteID)
        {

            int Approved = (int)QuoteApprovalStatus.Approved;
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;
            IEnumerable<QuoteApproval> QAlist = eQuotationContext.Current.QuoteApproval.Where(p => p.QuoteID == QuoteID).OrderBy(p => p.LevelNum).ToList();
            foreach (QuoteApproval qa in QAlist)
            {
                if (qa.Status == Rejected) return true;
            }
            return false;
        }
        public static string getApprovalTxt(string QuoteID)
        {
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            //  if (QM == null) return;
            int Approved = (int)QuoteApprovalStatus.Approved;
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;
            StringBuilder SB = new StringBuilder();
            IEnumerable<QuoteApproval> QAlist = eQuotationContext.Current.QuoteApproval.Where(p => p.QuoteID == QuoteID).OrderBy(p => p.LevelNum).ToList();

            int MaxLevelNum = QAlist.Max(p => p.LevelNum);


            for (int i = 1; i <= MaxLevelNum; i++)
            {
                IEnumerable<QuoteApproval> currlist = from a in QAlist where a.LevelNum == i select a;

                QuoteApproval approved = currlist.Where(p => p.Status == Approved).FirstOrDefault();
                if (approved != null)
                {
                    SB.AppendFormat("<div class='alert alert-success' role='alert' style='text-align:left;'>");
                    SB.AppendFormat("  <strong>Approved</strong> by {0} on {1}", approved.ApprovedBy, GetLocalTime("A" + QM.org.Substring(0, 2), approved.ApprovedDate.Value).ToString("MM/dd/yyyy HH:mm"));
                    SB.AppendFormat(" </div> ");
                    continue;
                }
                QuoteApproval rejected = currlist.Where(p => p.Status == Rejected).FirstOrDefault();
                if (rejected != null)
                {
                    SB.AppendFormat("<div class='alert alert-danger' role='alert' style='text-align:left;'>");
                    SB.AppendFormat("  <strong>Rejected</strong> by {0} on {1}", rejected.RejectedBy, GetLocalTime("A" + QM.org.Substring(0, 2), rejected.RejectedDate.Value).ToString("MM/dd/yyyy HH:mm"));
                    SB.AppendFormat(" </div> ");
                    return SB.ToString();
                }
                IEnumerable<QuoteApproval> Waitlist = currlist.Where(p => p.Status == Wait);
                if (Waitlist.Any())
                {
                    string[] Approvers = (from a in Waitlist select a.Approver).ToList().ToArray();

                    SB.AppendFormat("<div class='alert alert-warning' role='alert' style='text-align:left;'>");
                    SB.AppendFormat(" Waiting for <strong>{0}</strong>", string.Join(";", Approvers));
                    SB.AppendFormat(" </div> ");
                    break;
                }

            }
            return SB.ToString();
        }
        #endregion

        public static Decimal GetABRQuoteTotalWithTax(String Quoteid, int LineNo)
        {
            Decimal _ABRQuoteTotalAmount = 0;
            Decimal _ABRQuoteItemAmount = 0;

            QuotationDetailHelper qdh = new QuotationDetailHelper();

            List<QuotationDetail> QD = qdh.GetQuotationDetail(Quoteid);

            QuotationDetail_Extension_ABR qdextabritem = qdh.GetQuotationDetail_Extension_ABRItem(Quoteid, LineNo);

            foreach (QuotationDetail qditem in QD)
            {
                _ABRQuoteItemAmount += qditem.listPrice.GetValueOrDefault(0);
                _ABRQuoteItemAmount += qdextabritem.BX13.GetValueOrDefault(0);
                _ABRQuoteItemAmount += qdextabritem.BX23.GetValueOrDefault(0);
                _ABRQuoteItemAmount += qdextabritem.BX41.GetValueOrDefault(0);
                _ABRQuoteItemAmount += qdextabritem.BX72.GetValueOrDefault(0);
                _ABRQuoteItemAmount += qdextabritem.BX82.GetValueOrDefault(0);

                _ABRQuoteTotalAmount += _ABRQuoteItemAmount * qditem.qty.GetValueOrDefault(0);

            }

            return decimal.Round(_ABRQuoteTotalAmount, 2);
        }


        public static Decimal GetABRQuoteItemSubTotalWithTax(String Quoteid, int LineNo)
        {
            Decimal _SubTotalWithTax = 0;

            QuotationDetailHelper qdh = new QuotationDetailHelper();

            QuotationDetail qditem = qdh.GetQuotationDetailItem(Quoteid, LineNo);

            QuotationDetail_Extension_ABR qdextabritem = qdh.GetQuotationDetail_Extension_ABRItem(Quoteid, LineNo);
            if (qdextabritem == null)
            {
                return 0;
            }
            _SubTotalWithTax += qditem.listPrice.GetValueOrDefault(0) * qditem.qty.GetValueOrDefault(0);
            _SubTotalWithTax += qdextabritem.BX13.GetValueOrDefault(0);
            _SubTotalWithTax += qdextabritem.BX23.GetValueOrDefault(0);
            _SubTotalWithTax += qdextabritem.BX41.GetValueOrDefault(0);
            _SubTotalWithTax += qdextabritem.BX72.GetValueOrDefault(0);
            _SubTotalWithTax += qdextabritem.BX82.GetValueOrDefault(0);
            _SubTotalWithTax += qdextabritem.BX94.GetValueOrDefault(0);
            _SubTotalWithTax += qdextabritem.BX95.GetValueOrDefault(0);
            _SubTotalWithTax += qdextabritem.BX96.GetValueOrDefault(0);

            //return decimal.Round(_SubTotalWithTax * qditem.qty.GetValueOrDefault(0), 2);
            return _SubTotalWithTax;
        }


        public static String GetSalesCodeByQuoteID(String quote_id)
        {
            String SalesCode = String.Empty, SalesEmail = String.Empty;

            List<EQPARTNER> eqp = new eQuotationDAL().GetEQPartnerByQuoteID(quote_id);
            SalesCode = (from d in eqp where d.QUOTEID == quote_id && d.TYPE == "E" select d.ERPID).FirstOrDefault();
            if (String.IsNullOrEmpty(SalesCode))
            {
                List<QuotationMaster> qm = new eQuotationDAL().GetQuoteMasterByQuoteID(quote_id);
                SalesEmail = (from d in qm where d.quoteId == quote_id select d.salesEmail).FirstOrDefault();
                if (String.IsNullOrEmpty(SalesEmail))
                {
                    SalesEmail = (from d in qm where d.quoteId == quote_id select d.createdBy).FirstOrDefault();
                    if (String.IsNullOrEmpty(SalesEmail))
                        return "";
                }

                List<SAP_EMPLOYEE> se = new MyAdvantechDAL().GetSAPEmployeeBySalesEmail(SalesEmail);
                SalesCode = (from d in se where d.EMAIL.Equals(SalesEmail, StringComparison.OrdinalIgnoreCase) select d.SALES_CODE).FirstOrDefault();
                return !String.IsNullOrEmpty(SalesCode) ? SalesCode : "";

            }
            else
                return SalesCode;
        }

        public static String GetSalesEmailByQuoteID(String quote_id)
        {
            String SalesCode = String.Empty, SalesEmail = String.Empty;

            List<EQPARTNER> eqp = new eQuotationDAL().GetEQPartnerByQuoteID(quote_id);
            SalesCode = (from d in eqp where d.QUOTEID == quote_id && d.TYPE == "E" select d.ERPID).FirstOrDefault();
            if (String.IsNullOrEmpty(SalesCode))
            {
                List<QuotationMaster> qm = new eQuotationDAL().GetQuoteMasterByQuoteID(quote_id);
                SalesEmail = (from d in qm where d.quoteId == quote_id select d.salesEmail).FirstOrDefault();
                if (String.IsNullOrEmpty(SalesEmail))
                {
                    SalesEmail = (from d in qm where d.quoteId == quote_id select d.createdBy).FirstOrDefault();
                    if (String.IsNullOrEmpty(SalesEmail))
                        return "";
                }
                return !String.IsNullOrEmpty(SalesEmail) ? SalesEmail : "";
            }
            else
            {
                List<SAP_EMPLOYEE> se = new MyAdvantechDAL().GetSAPEmployeeBySalesCode(SalesCode);
                SalesEmail = (from d in se select d.EMAIL).FirstOrDefault();
                return SalesEmail;
            }
        }

        public static String GetSalesOfficeCodeBySalesCode(String SalesCode)
        {
            List<SAP_EMPLOYEE> se = new MyAdvantechDAL().GetSAPEmployeeBySalesCode(SalesCode);
            String result = (from d in se select d.SALESOFFICE).FirstOrDefault();
            return !String.IsNullOrEmpty(result) ? result : "";
        }

        public static String GetQuoteIDByCartID(String cart_id)
        {
            CARTMASTERV2 cm = new MyAdvantechDAL().getCartMasterV2ByCartID(cart_id);
            if (cm != null)
                return cm.QuoteID;
            else
                return "";
        }

        public static Boolean IsFeiOffice(String quote_id)
        {
            String result = GetSalesCodeByQuoteID(quote_id);
            result = GetSalesOfficeCodeBySalesCode(result);
            if (!String.IsNullOrEmpty(result))
                return result.Equals("2700") ? true : false;
            else
                return false;
        }

        public static String GetBankAccountByCurrency(String orgId, String currency)
        {
            String BankAccount = String.Empty;

            if (orgId.ToString().ToUpper().Equals("TW20"))
            {
                switch (currency)
                {
                    case "TWD":
                        BankAccount = "A/C No. 5831680018";
                        break;
                    case "EUR":
                        BankAccount = "A/C No. 5831680239";
                        break;
                    case "USD":
                        BankAccount = "A/C No. 5831680204";
                        break;
                    case "CNY":
                        BankAccount = "A/C No. 5831680212";
                        break;
                    case "JPY":
                        BankAccount = "A/C No. 5831680247";
                        break;
                    default:
                        BankAccount = "A/C No. 5831680204";
                        break;
                }
            }
            else
            {
                switch (currency)
                {
                    case "TWD":
                        BankAccount = "A/C No. 5028523019";
                        break;
                    case "EUR":
                        BankAccount = "A/C No. 5028523221";
                        break;
                    case "USD":
                        BankAccount = "A/C No. 5028523205";
                        break;
                    default:
                        BankAccount = "A/C No. 5028523205";
                        break;
                }
            }
            return BankAccount;
        }

        public static bool UpdateQuotationDescription(string ID, string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                description = "";
            }
            description = description.Replace("'", "''");
            try
            {
                //ICC 2016/2/26 For ATW to update description.
                return SqlProvider.dbExecuteNoQuery("EQ", string.Format("Update [QuotationDetail] set [description] = N'{0}' where id = {1} ", description, ID)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static CreditLimitData GetCustomerCreditExposure(String ERPID, String ORGID)
        {
            CreditControlAreaOptions ccao = CreditControlAreaOptions.EU01;
            String _OrgPref = ORGID.ToUpper().Substring(0, 2);
            switch (_OrgPref)
            {
                case "EU":
                case "AU":
                case "JP":
                case "MY":
                case "BR":
                case "SG":
                case "TL":
                case "TW":
                    Enum.TryParse<CreditControlAreaOptions>(_OrgPref + "01", out ccao);
                    break;
                case "CN":
                    ccao = CreditControlAreaOptions.CNC1;
                    break;
                case "US":
                    if (ORGID.Equals("USAENC", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ccao = CreditControlAreaOptions.USC2;
                    }
                    else
                    {
                        ccao = CreditControlAreaOptions.USC1;
                    }
                    break;
                case "HK":
                    ccao = CreditControlAreaOptions.HK05;
                    break;
            }

            return Advantech.Myadvantech.DataAccess.SAPDAL.GetCustomerCreditExposure(ERPID, ccao);
        }

        public static Order UpdateSystemPriceForABRQuote(string QuoteID, decimal Discount)
        {
            if (string.IsNullOrEmpty(QuoteID))
            {
                return null;
            }

            //Get Quotation Master
            QuotationMasterHelper _QuotationMasterHelper = new QuotationMasterHelper();
            QuotationMaster _QMaster = _QuotationMasterHelper.GetQuotationMaster(QuoteID);

            //Get Quotation Master Extendsion
            QuotationExtension _QME = eQuotationDAL.GetQuoteMasterExtendionByQuoteID(QuoteID);

            //Get Quotation Detail
            QuotationDetailHelper _QuotationDetailHelper = new QuotationDetailHelper();
            List<QuotationDetail> QDlist = _QuotationDetailHelper.GetQuotationDetail(QuoteID);

            List<QuotationDetail_Extension_ABR> QDExtABRlist = _QuotationDetailHelper.GetQuotationDetail_Extension_ABR(QuoteID);
            eQuotationContext.Current.QuotationDetail_Extension_ABR.RemoveRange(QDExtABRlist);
            eQuotationContext.Current.SaveChanges();

            if (QDlist.Count == 0)
            {
                return null;
            }


            Order _order = new Order();
            _order.Currency = _QMaster.currency;
            _order.DistChannel = _QMaster.DIST_CHAN;
            _order.Division = _QMaster.DIVISION;
            _order.OrgID = _QMaster.org;
            _order.Discount = Discount;

            if (_QME != null && !string.IsNullOrEmpty(_QME.ABRQuoteType))
            {
                SAPOrderType _ordertype = SAPOrderType.ZOR;
                if (Enum.TryParse<SAPOrderType>(_QME.ABRQuoteType, out _ordertype))
                {
                    _order.OrderType = _ordertype;
                }
            }

            foreach (QuotationDetail pn in QDlist)
            {
                if (pn.line_No < 100)
                {
                    _order.AddLooseItem(pn.partNo, pn.qty.GetValueOrDefault(1));
                }
                else
                {
                    if (pn.line_No % 100 == 0)
                    {
                        _order.AddBTOSParentItem(pn.partNo, pn.deliveryPlant, pn.line_No.GetValueOrDefault(100), pn.qty.GetValueOrDefault(1));
                    }
                    else
                    {
                        _order.AddBTOSChildItem(pn.partNo, pn.HigherLevel.GetValueOrDefault(0), pn.deliveryPlant, pn.line_No.GetValueOrDefault(100), pn.qty.GetValueOrDefault(1));
                    }
                }
            }

            List<EQPARTNER> EQPList = eQuotationDAL.GetQuotePartnerByQuoteID(QuoteID);
            if (EQPList != null && EQPList.Count > 0)
            {
                foreach (EQPARTNER PartnerRow in EQPList)
                {
                    switch (PartnerRow.TYPE)
                    {
                        case "S":
                            _order.SetOrderPartnet(new OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.ShipTo));
                            break;
                        case "B":
                            _order.SetOrderPartnet(new OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.BillTo));
                            break;
                        case "SOLDTO":
                            _order.SetOrderPartnet(new OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.SoldTo));
                            break;
                    }
                }
            }
            else
            {
                _order.SetOrderPartnet(new OrderPartner(_QMaster.quoteToErpId, _QMaster.org, OrderPartnerType.ShipTo));
                _order.SetOrderPartnet(new OrderPartner(_QMaster.quoteToErpId, _QMaster.org, OrderPartnerType.BillTo));
                _order.SetOrderPartnet(new OrderPartner(_QMaster.quoteToErpId, _QMaster.org, OrderPartnerType.SoldTo));
            }

            string _errmsg = string.Empty;
            Myadvantech.DataAccess.SAPDAL.SimulateABROrder(ref _order, ref _errmsg);


            //Frank update price to quotation
            List<Product> pr = _order.LineItems;
            QuotationDetail_Extension_ABR _newQuoteDetail_Ext_ABR = null;


            foreach (Product p in pr)
            {
                QuotationDetail qd = QDlist.Where(q => q.partNo == p.PartNumber && q.line_No == p.LineNumber).FirstOrDefault();

                qd.listPrice = p.ListPrice;
                qd.unitPrice = p.UnitPrice;
                qd.newUnitPrice = p.UnitPrice;

                _newQuoteDetail_Ext_ABR = new QuotationDetail_Extension_ABR();
                _newQuoteDetail_Ext_ABR.quoteid = qd.quoteId;
                _newQuoteDetail_Ext_ABR.line_No = qd.line_No.GetValueOrDefault(0);
                _newQuoteDetail_Ext_ABR.PartNo = qd.partNo;
                _newQuoteDetail_Ext_ABR.BX10 = p.BX10;
                _newQuoteDetail_Ext_ABR.BX13 = p.BX13;
                _newQuoteDetail_Ext_ABR.BX23 = p.BX23;
                _newQuoteDetail_Ext_ABR.BX40 = p.BX40;
                _newQuoteDetail_Ext_ABR.BX41 = p.BX41;
                _newQuoteDetail_Ext_ABR.BX72 = p.BX72;
                _newQuoteDetail_Ext_ABR.BX82 = p.BX82;
                _newQuoteDetail_Ext_ABR.BX94 = p.BX94;
                _newQuoteDetail_Ext_ABR.BX95 = p.BX95;
                _newQuoteDetail_Ext_ABR.BX96 = p.BX96;
                _newQuoteDetail_Ext_ABR.FK00 = p.FK00;

                _newQuoteDetail_Ext_ABR.ICVA = p.ICVA;
                _newQuoteDetail_Ext_ABR.IPVA = p.IPVA;
                _newQuoteDetail_Ext_ABR.ISIC = p.ISIC;
                _newQuoteDetail_Ext_ABR.ISTS = p.ISTS;
                _newQuoteDetail_Ext_ABR.BCO1 = p.BCO1;
                _newQuoteDetail_Ext_ABR.BPI1 = p.BPI1;
                _newQuoteDetail_Ext_ABR.NCM = p.NCM;

                eQuotationContext.Current.QuotationDetail_Extension_ABR.Add(_newQuoteDetail_Ext_ABR);
                //QuotationDetail_Extension_ABR qd_ext_abr = QDExtABRlist.Where(q => q.line_No == p.LineNumber).FirstOrDefault();
                //qd_ext_abr.BX13 = p.BX13;
                //qd_ext_abr.BX23 = p.BX23;
                //qd_ext_abr.BX41 = p.BX41;
                //qd_ext_abr.BX72 = p.BX72;
                //qd_ext_abr.BX82 = p.BX82;


                eQuotationContext.Current.SaveChanges();
            }

            return _order;
        }

        public static String GetQuotationEndCustomer(String _quoteid)
        {
            List<EQPARTNER> list = eQuotationDAL.GetEQPartner(_quoteid);

            if (list.Where(d => d.TYPE.Equals("EM", StringComparison.OrdinalIgnoreCase)).Any())
                return list.Where(d => d.TYPE.Equals("EM", StringComparison.OrdinalIgnoreCase)).FirstOrDefault().ERPID;
            else
                return "";
        }

        public static byte[] DownloadQuotePDFByHtmlString(string HtmlString)
        {
            string pdfConverterLicenseKey = "fvDg8eDx4+jx6P/h8eLg/+Dj/+jo6Og=";

            byte[] pdfBytes = null;

            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.LicenseKey = pdfConverterLicenseKey;
            pdfConverter.PdfDocumentOptions.EmbedFonts = false;

            pdfBytes = pdfConverter.GetPdfBytesFromHtmlString(HtmlString);

            return pdfBytes;

            // !!!!!!Frank Please do not remove below sample codes during eQ3.0 implementation period, thanks. !!!!!!!!

            //        Shared Function DownloadQuotePDFByHtmlString(ByVal HtmlString As String, ByVal ShowPageNumber As Boolean) As Byte()
            //    Dim selectablePDF As Boolean = True
            //    Dim pdfConverter As New PdfConverter()
            //    'pdfConverter.LicenseKey = "BC81JDUkNTYyJDAqNCQ3NSo1Nio9PT09"
            //    pdfConverter.LicenseKey = pdfConverterLicenseKey
            //    pdfConverter.PdfDocumentOptions.EmbedFonts = False

            //    If ShowPageNumber Then
            //        pdfConverter.PdfDocumentOptions.ShowFooter = True
            //        'pdfConverter.PdfFooterOptions.FooterText = ""
            //        'pdfConverter.PdfFooterOptions.ShowPageNumber = True
            //    End If

            //    Dim footBlock As String = "", headerBlock As String = ""
            //    Dim doc As New HtmlAgilityPack.HtmlDocument
            //    Dim HtmlReader As TextReader = New StringReader(HtmlString.Replace("&nbsp;", " "))
            //    doc.Load(HtmlReader)

            //    If Role.IsTWAonlineSales() OrElse Role.IsCNAonlineSales() Then
            //        pdfConverter.PdfDocumentOptions.EmbedFonts = True

            //        'Header control
            //        pdfConverter.PdfDocumentOptions.ShowHeader = True
            //        pdfConverter.PdfHeaderOptions.HeaderHeight = 130
            //        headerBlock = doc.GetElementbyId("divHeader").InnerHtml
            //        Dim headerHtml As HtmlToPdfElement = New HtmlToPdfElement(0, 0, 600, headerBlock, "")
            //        headerHtml.FitWidth = True
            //        headerHtml.EmbedFonts = True
            //        pdfConverter.PdfHeaderOptions.AddElement(headerHtml)

            //        'footer control
            //        pdfConverter.PdfDocumentOptions.ShowFooter = True
            //        pdfConverter.PdfFooterOptions.FooterHeight = 100
            //        footBlock = doc.GetElementbyId("divFooter").InnerHtml
            //        Dim footerHtml As HtmlToPdfElement = New HtmlToPdfElement(0, 0, 600, footBlock, "")
            //        footerHtml.FitWidth = True
            //        footerHtml.EmbedFonts = True
            //        pdfConverter.PdfFooterOptions.AddElement(footerHtml)


            //        'pdfConverter.PdfFooterOptions.PageNumberingStartIndex = 1


            //        HtmlString = HtmlString.Replace("id=""divHeader""", "id=""divHeader"" style=""display:none;""")
            //        HtmlString = HtmlString.Replace("id=""divFooter""", "id=""divFooter"" style=""display:none;""")
            //    End If

            //    If Role.IsJPAonlineSales() Then
            //        pdfConverter.PdfDocumentOptions.EmbedFonts = True
            //        'pdfConverter.SvgFontsEnabled = True


            //        headerBlock = doc.GetElementbyId("divHeader").InnerHtml
            //        footBlock = doc.GetElementbyId("divFooter").InnerHtml
            //        'Dim MyDOC As New System.Xml.XmlDocument
            //        'MyDOC.LoadXml(HtmlString.Replace("&nbsp;", " "))
            //        'Util.getXmlBlockByID("div", "divHeader", MyDOC, headerBlock)
            //        'Util.getXmlBlockByID("div", "divFooter", MyDOC, footBlock)
            //        pdfConverter.PdfDocumentOptions.ShowHeader = True
            //        pdfConverter.PdfHeaderOptions.HeaderHeight = 110
            //        'pdfConverter.PdfHeaderOptions.HtmlToPdfArea = New Winnovative.WnvHtmlConvert.HtmlToPdfArea(headerBlock, Nothing)

            //        Dim headerHtml As HtmlToPdfElement = New HtmlToPdfElement(headerBlock, Nothing)
            //        headerHtml.FitHeight = True : headerHtml.EmbedFonts = True
            //        pdfConverter.PdfHeaderOptions.AddElement(headerHtml)



            //        pdfConverter.PdfDocumentOptions.ShowFooter = True
            //        pdfConverter.PdfFooterOptions.FooterHeight = 200
            //        Dim Footer As HtmlToPdfElement = New HtmlToPdfElement(footBlock, Nothing)
            //        Footer.EmbedFonts = True
            //        pdfConverter.PdfFooterOptions.AddElement(Footer)
            //        ''
            //        Dim footerTextElement As TextElement = New TextElement(445, 170, "Page &p; / &P;  ", New System.Drawing.Font(New System.Drawing.FontFamily("Arial"), 14, System.Drawing.FontStyle.Bold))
            //        footerTextElement.TextAlign = HorizontalTextAlign.Right
            //        pdfConverter.PdfFooterOptions.AddElement(footerTextElement)
            //        ''
            //        'pdfConverter.PdfFooterOptions.ShowPageNumber = True
            //        HtmlString = HtmlString.Replace("id=""divHeader""", "id=""divHeader"" style=""display:none;""")
            //        HtmlString = HtmlString.Replace("id=""divFooter""", "id=""divFooter"" style=""display:none;""")
            //    End If
            //    Dim pdfBytes As Byte() = pdfConverter.GetPdfBytesFromHtmlString(HtmlString)
            //    Return pdfBytes

            //End Function


        }

        public static byte[] DownloadQuotePDFByHtmlStringV2(string HtmlString, string region, bool showPageNum)
        {
            string pdfConverterLicenseKey = "fvDg8eDx4+jx6P/h8eLg/+Dj/+jo6Og=";
            byte[] pdfBytes = null;
            if (region == "ASG")
            {
                pdfBytes = Advantech.Myadvantech.DataAccess.Common.PDFUtil.GeneratePDFWithHeaderFooter(HtmlString, "pdfHeader", "pdfFooter");
            }
            else
            {
                PdfConverter pdfConverter = new PdfConverter();
                pdfConverter.LicenseKey = pdfConverterLicenseKey;
                pdfConverter.PdfDocumentOptions.EmbedFonts = false;

                pdfBytes = pdfConverter.GetPdfBytesFromHtmlString(HtmlString);
            }



            return pdfBytes;

        }

        /// <summary>
        /// New logic for eQ3.0 approval
        /// </summary>
        /// <param name="quotationDetails"></param>
        /// <returns></returns>
        //public static decimal GetACNTotalMarginForQuote(string quoteId)
        //{
        //    List<QuotationDetail> quoteDetails = GetQuotationDetail(quoteId);
        //    decimal marginTotal = GetACNTotalAmountForQuote(quoteDetails);

        //    return marginTotal;
        //}

        /// <summary>
        /// New logic for eQ3.0 approval
        /// </summary>
        /// <param name="quotationDetails"></param>
        /// <returns></returns>
        public static decimal GetACNTotalMarginForQuote(List<QuotationDetail> quotationDetails)
        {
            decimal marginTotal = -9999;

            decimal sumAmt = 0;
            decimal sumITP = 0;

            //Alex 20180119, remove service part in total margin calculation 
            var quotationDetailsWithoutServicePart = RemoveServicePartInQuotationDetails(quotationDetails); ;

            foreach (var qd in quotationDetailsWithoutServicePart)
            {
                sumAmt += qd.PostTaxNewUnitPrice * qd.qty.Value;
                sumITP += qd.ITPWithGeneralAmount * qd.qty.Value;
            }
            if (sumAmt != 0)
                marginTotal = Math.Round((sumAmt - sumITP) / sumAmt, 4, MidpointRounding.AwayFromZero); ;


            return marginTotal;
        }

        /// <summary>
        /// New logic for eQ3.0 approval
        /// </summary>
        /// <param name="quotationDetails"></param>
        /// <returns></returns>
        public static decimal GetACNTotalAmountForQuote(List<QuotationDetail> quotationDetails)
        {
            decimal sumAmt = 0;

            foreach (var qd in quotationDetails)
            {
                sumAmt += qd.PostTaxNewUnitPrice * qd.qty.Value;
            }


            return sumAmt;
        }

        /// <summary>
        /// New ACN Min Margin logic for eQ3.0 approval 
        /// </summary>
        /// <param name="quotationDetails"></param>
        /// <returns></returns>
        public static decimal GetACNMinMarginForQuote(List<QuotationDetail> quotationDetails)
        {
            decimal minMargin = 1;

            foreach (var qd in quotationDetails)
            {
                if (qd.ACNMargin < minMargin)
                    minMargin = qd.ACNMargin;
            }

            return minMargin;
        }

        /// <summary>
        /// Min Margin for eQ3.0 approval
        /// </summary>
        /// <param name="quotationDetails"></param>
        /// <returns></returns>
        public static decimal GetMinMarginForQuote(List<QuotationDetail> quotationDetails)
        {
            decimal minMargin = 1;

            foreach (var qd in quotationDetails)
            {
                var correctMargin = qd.Margin / 100;
                if (correctMargin < minMargin)
                    minMargin = correctMargin;
            }

            return minMargin;
        }

        /// <summary>
        /// Min Margin for eQ3.0 approval (combined loose item and bto)
        /// </summary>
        /// <param name="quotationDetails"></param>
        /// <returns></returns>
        public static decimal GetMinMarginForQuoteWithLoositemAndBTO(List<QuotationDetail> quotationDetails)
        {
            decimal minLooseItemMargin = 1;
            decimal margin = 0;
            decimal minBTOMargin = 99999;

            var BTOChildList = quotationDetails.Where(d => d.ItemType == (int)LineItemType.BTOSChild).GroupBy(d => d.HigherLevel.Value).Select(grp => grp.ToList());

            if (BTOChildList != null)
            {
                foreach (var c in BTOChildList)
                {

                    margin = GetMinMarginForQuote(c);

                    if (margin < minBTOMargin)
                        minBTOMargin = margin;
                }
            }

            var looseItemList = quotationDetails.Where(d => d.ItemType == (int)LineItemType.LooseItem).ToList();
            minLooseItemMargin = GetMinMarginForQuote(looseItemList);


            if (minBTOMargin < minLooseItemMargin)
                return minBTOMargin;

            return minLooseItemMargin;
        }

        public static decimal GetMaxBelowSAPPriceRateForConfigurationList(List<QuotationDetail> quotationDetails)
        {
            decimal belowSAPPriceRate = 0;
            decimal maxBelowSAPPriceRate = 0;



            var configurationList = quotationDetails.Where(d => d.HigherLevel != 0).GroupBy(d => d.HigherLevel.Value).Select(grp => grp.ToList());

            if (configurationList != null)
            {
                foreach (var c in configurationList)
                {
                    belowSAPPriceRate = BelowSAPPriceRateForQuote(c);


                    if (belowSAPPriceRate > maxBelowSAPPriceRate)
                        maxBelowSAPPriceRate = belowSAPPriceRate;
                }
            }
            return maxBelowSAPPriceRate;
        }

        public static decimal BelowSAPPriceRateForQuote(List<QuotationDetail> quotationDetails)
        {
            decimal belowSAPPriceRate = 0;
            decimal maxBelowSAPPriceRate = 99999;

            decimal sumUnitPrice = 0;
            decimal sumNewUnitPrice = 0;

            foreach (var q in quotationDetails)
            {


                if (q.unitPrice.Value > 0 && q.newUnitPrice.Value > 0)
                {
                    sumUnitPrice += q.unitPrice.Value;
                    sumNewUnitPrice += q.newUnitPrice.Value;

                }


            }
            return Math.Round((sumUnitPrice - sumNewUnitPrice) / sumNewUnitPrice, 4, MidpointRounding.AwayFromZero);
        }


        //public static decimal GetMinMarginForConfigurationList(List<List<QuotationDetail>> configurationList)
        //{
        //    decimal margin = 0;
        //    decimal minMargin = 99999;

        //    foreach (var c in configurationList)
        //    {
        //        margin = GetACNTotalMarginForQuote(c);

        //        if (margin < minMargin)
        //            minMargin = margin;
        //    }
        //    return minMargin;
        //}

        public static decimal GetMinACNMarginForConfigurationList(List<QuotationDetail> quotationDetails)
        {
            decimal margin = 0;
            decimal minMargin = 99999;

            var configurationList = quotationDetails.Where(d => d.HigherLevel != 0).GroupBy(d => d.HigherLevel.Value).Select(grp => grp.ToList());

            if (configurationList != null)
            {
                foreach (var c in configurationList)
                {
                    margin = GetACNTotalMarginForQuote(c);

                    if (margin < minMargin)
                        minMargin = margin;
                }
            }
            return minMargin;
        }

        public static bool IsACNLoosItemQuote(List<QuotationDetail> quotationDetails)
        {
            if (quotationDetails != null)
            {
                if (quotationDetails.Where(d => d.line_No <= 99).FirstOrDefault() != null)
                    return true;
            }
            return false;
        }

        public static bool IsContainBto(List<QuotationDetail> quotationDetails)
        {
            if (quotationDetails != null)
            {
                if (quotationDetails.Where(d => d.ItemType == -1).FirstOrDefault() != null)
                    return true;
            }
            return false;
        }

        public static bool IsBtoPart(string partNo, string org)
        {
            SAP_PRODUCT p = MyAdvantechDAL.GetSAP_ProductByOrg(partNo, org);
            if (p != null && (partNo.EndsWith("-BTO", StringComparison.OrdinalIgnoreCase) || p.MATERIAL_GROUP.Equals("BTOS", StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }


        // For eQ 3.0
        public static bool IsRejectedByApprover(string QuoteID)
        {
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            //IEnumerable<WorkFlowApproval> QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == QuoteID).OrderBy(p => p.LevelNum).ToList();
            //foreach (WorkFlowApproval qa in QAlist)
            //{
            //    if (qa.Status == Rejected) return true;
            //}
            IEnumerable<WorkFlowApproval> rejectedQAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == QuoteID && p.Status == Rejected).ToList();
            if (rejectedQAlist.Any()) return true;

            return false;
        }

        // For eQ 3.0
        public static bool IsApprovedByAllApprovers(string QuoteID)
        {
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;
            IEnumerable<WorkFlowApproval> QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == QuoteID).OrderBy(p => p.LevelNum).ToList();
            decimal MaxLevelNum = QAlist.Max(p => p.LevelNum);
            WorkFlowApproval waitingMax = QAlist.Where(p => (p.Status == Wait) && p.LevelNum == MaxLevelNum).FirstOrDefault();
            if (waitingMax != null)
            {
                return false;
            }
            return true;
        }

        // For eQ 3.0
        public static bool IsApprovedForMaxApprover(string quoteId)
        {
            //確認是否最高簽核層級已經approve過
            int Approved = (int)QuoteApprovalStatus.Approved;
            IEnumerable<WorkFlowApproval> QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId).OrderBy(p => p.LevelNum).ToList();
            decimal MaxLevelNum = QAlist.Max(p => p.LevelNum);
            List<WorkFlowApproval> maxApprovalList = QAlist.Where(p => p.LevelNum == MaxLevelNum).ToList();
            foreach (var approval in maxApprovalList)
                if (approval.Status != Approved)
                    return false;
            return true;
        }


        public static WorkFlowApproval FindLatestWaitingApproval(string quoteID)
        {
            var waitQA = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteID && p.Status == (int)QuoteApprovalStatus.Wait_for_review).OrderBy(a => a.LevelNum).FirstOrDefault();
            if (waitQA != null)
                return waitQA;
            return null;
        }

        public static List<WorkFlowApproval> FindLatestWaitingApprovals(string quoteID)
        {
            var waitingApprovals = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteID && p.Status == (int)QuoteApprovalStatus.Wait_for_review).ToList();
            if (waitingApprovals.Any())
            {
                decimal latestWaitingLevelNum = waitingApprovals.Min(p => p.LevelNum);
                var latestWaitingApprovals = waitingApprovals.Where(a => a.LevelNum == latestWaitingLevelNum).ToList();
                if (latestWaitingApprovals != null)
                    return latestWaitingApprovals;
            }
            return null;
        }

        public static List<WorkFlowApproval> FindApprovalsWaitingForUser(string userEmail)
        {
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;
            var approvalsWaitingForUser = new List<WorkFlowApproval>();
            //先撈出所有等待簽核中且過程未被reject的 approvals
            //20180412 Alex 加入approvals需包含current user email 的條件來提升效能
            var waitingApprovalsWithoutRjectedStatus = (from WA1 in eQuotationContext.Current.WorkFlowApproval
                                                        where
                                                        WA1.Status == Wait &&
                                                           ((from WA2 in eQuotationContext.Current.WorkFlowApproval
                                                             where WA1.TypeID == WA2.TypeID && WA2.Status == Rejected
                                                             select WA2).Any() == false) &&
                                                            ((from WA3 in eQuotationContext.Current.WorkFlowApproval
                                                              where WA1.TypeID == WA3.TypeID && WA3.Approver.ToLower() == userEmail.ToLower()
                                                              select WA3).Any() == true)
                                                        select WA1).ToList();

            if (waitingApprovalsWithoutRjectedStatus.Any())
            {
                var groups = waitingApprovalsWithoutRjectedStatus.GroupBy(w => w.TypeID);
                foreach (var g in groups)
                {
                    decimal latestWaitingLevelNum = g.Min(p => p.LevelNum);
                    //確認是否最新的waiting approvals 中的approver有包含 currentUser
                    var latestWaitingApproval = g.Where(a => a.LevelNum == latestWaitingLevelNum && a.Approver.ToLower() == userEmail.ToLower()).FirstOrDefault();
                    if (latestWaitingApproval != null)
                        approvalsWaitingForUser.Add(latestWaitingApproval);
                }
            }

            return approvalsWaitingForUser;

        }

        public static WorkFlowApproval FindMyLatestWaitingApproval(string quoteID, string email)
        {
            var waitQA = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteID
            && p.Status == (int)QuoteApprovalStatus.Wait_for_review && email.ToLower().IndexOf(p.Approver.ToLower()) > -1
            ).OrderBy(a => a.LevelNum).FirstOrDefault();
            if (waitQA != null)
                return waitQA;
            return null;
        }

        public static WorkFlowApproval FindMyMaxLevelApproval(string quoteID, string email)
        {
            var waitQA = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteID
            && email.ToLower().IndexOf(p.Approver.ToLower()) > -1
            ).OrderByDescending(a => a.LevelNum).FirstOrDefault();
            if (waitQA != null)
                return waitQA;
            return null;
        }

        public static bool IsBelongCurrentApprovers(string quoteId, string currentUser)
        {
            var waitingApprovals = QuoteBusinessLogic.FindLatestWaitingApprovals(quoteId);

            // Rejected的報價單不需要再有簽核者了
            if (QuoteBusinessLogic.IsRejectedByApprover(quoteId) == true)
                return false;

            if (waitingApprovals != null)
            {
                foreach (var approval in waitingApprovals)
                {
                    if (currentUser.ToLower().IndexOf(approval.Approver.ToLower()) > -1)
                        return true;
                }
            }

            return false;

        }

        public static string getQuoteStatus(string quoteId)
        {
            string status = "";
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Find(quoteId);
            if (QM != null)
            {
                if (QM.DOCSTATUS == 0)
                {
                    status = "Draft";

                    if (QuoteBusinessLogic.IsRejectedByApprover(quoteId) == true)
                        status = "Rejected";
                    else
                    {
                        //Alex 20180430  remove approver name in waiting approval status temporarly
                        //List<string> approvers = new List<string>();
                        //foreach (var approval in waitingApprovals)
                        //{
                        //    if (!string.IsNullOrEmpty(approval.Approver))
                        //        approvers.Add(approval.Approver.Split('@')[0]);
                        //}
                        //status += " (Waiting for " + String.Join(",", approvers) + "'s approval)";
                        //var latestWaitingApprovals = QuoteBusinessLogic.FindLatestWaitingApprovals(quoteId);
                        //if(latestWaitingApprovals!=null)
                        var waitingApprovals = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId && p.Status == (int)QuoteApprovalStatus.Wait_for_review).ToList();
                        if (waitingApprovals.Any())
                            status += " (Waiting for approval)";
                    }
                }
                else if (QM.DOCSTATUS == 1)
                    status = "Finish";
                else if (QM.DOCSTATUS == 2)
                    status = "Delete";
            }
            return status;
        }


        public static List<QuotationMaster> GetQuotationMastersBySearchTerms(List<String> _RBUList, String _QuoteNo, String _Description, String _AccountName, String _AccountERPID, String _CurrentMail, String _CreatedFrom, String _CreatedTo, String _LastUpdatedFrom, String _LastUpdatedTo, QuoteDocStatus _Status, bool isMyteam, string[] myTeamSalesCodes)
        {
            List<QuotationMaster> finalQM = new List<QuotationMaster>();
            try
            {
                // Parse Date value
                Boolean hasCreatedDate = false, hasUpdatedDate = false;
                DateTime CreatedFrom = new DateTime(), CreatedTo = new DateTime(), LastUpdatedFrom = new DateTime(), LastUpdatedTo = new DateTime();
                string CreatedBy = isMyteam ? "" : _CurrentMail;


                DateTime forParse = new DateTime();
                if ((!String.IsNullOrEmpty(_CreatedFrom) && DateTime.TryParse(_CreatedFrom, out forParse))
                    || (!String.IsNullOrEmpty(_CreatedTo) && DateTime.TryParse(_CreatedTo, out forParse)))
                {
                    hasCreatedDate = true;
                    CreatedFrom = string.IsNullOrEmpty(_CreatedFrom) ? DateTime.Parse("1980-01-01") : DateTime.Parse(_CreatedFrom);
                    CreatedTo = string.IsNullOrEmpty(_CreatedTo) ? DateTime.Parse("2050-12-31") : DateTime.Parse(_CreatedTo);
                }
                if ((!String.IsNullOrEmpty(_LastUpdatedFrom) && DateTime.TryParse(_LastUpdatedFrom, out forParse))
                   || (!String.IsNullOrEmpty(_LastUpdatedTo) && DateTime.TryParse(_LastUpdatedTo, out forParse)))
                {
                    hasUpdatedDate = true;
                    LastUpdatedFrom = string.IsNullOrEmpty(_LastUpdatedFrom) ? DateTime.Parse("1980-01-01") : DateTime.Parse(_LastUpdatedFrom);
                    LastUpdatedTo = string.IsNullOrEmpty(_LastUpdatedTo) ? DateTime.Parse("2050-12-31") : DateTime.Parse(_LastUpdatedTo);
                }


                // Use conditions in where clause.
                var result = (from m in eQuotationContext.Current.QuotationMaster
                              join d in eQuotationContext.Current.QuotationDetail on m.quoteId equals d.quoteId into md
                              from x in md.DefaultIfEmpty()
                              join p in eQuotationContext.Current.EQPARTNER on m.quoteId equals p.QUOTEID
                              join w in eQuotationContext.Current.WorkFlowApproval on m.quoteId equals w.TypeID into mw
                              where
                                          (isMyteam ? true : m.createdBy.Equals(CreatedBy, StringComparison.OrdinalIgnoreCase)) &&
                                          (_RBUList.Contains(m.siebelRBU)) &&
                                          (String.IsNullOrEmpty(_QuoteNo) ? true : m.quoteNo.Equals(_QuoteNo, StringComparison.OrdinalIgnoreCase)) &&
                                          ((String.IsNullOrEmpty(_Description) ? true : m.customId.Contains(_Description)) || (String.IsNullOrEmpty(_Description) ? true : x.description.Contains(_Description))) &&
                                          (String.IsNullOrEmpty(_AccountName) ? true : m.quoteToName.Equals(_AccountName, StringComparison.OrdinalIgnoreCase)) &&
                                          (String.IsNullOrEmpty(_AccountERPID) ? true : m.quoteToErpId.Equals(_AccountERPID, StringComparison.OrdinalIgnoreCase)) &&
                                          (hasCreatedDate == true ? (CreatedFrom <= m.createdDate && m.createdDate <= CreatedTo) : true) &&
                                          (hasUpdatedDate == true ? (LastUpdatedFrom <= m.LastUpdatedDate && m.LastUpdatedDate <= LastUpdatedTo) : true) &&
                                          (_Status == QuoteDocStatus.All ? true : (_Status == QuoteDocStatus.Finish ? m.DOCSTATUS == 1 : m.DOCSTATUS == 0)) &&
                                          m.DOCSTATUS != 2 && p.TYPE == "E"
                              select new
                              {
                                  M = m,
                                  P = p,
                                  ApprovalStatus = m.DOCSTATUS == 1 ? "Finish" : m.DOCSTATUS == 0 ? mw.Where(w => w.Status == (int)QuoteApprovalStatus.Rejected).FirstOrDefault() != null ? "Rejected" : mw.Where(w => w.Status == (int)QuoteApprovalStatus.Wait_for_review).FirstOrDefault() != null ? "Waiting for approval" : "Draft" : "Delete"
                              })
                              .Distinct().ToList();

                //20180517 Alex 填入quote's approval status for performance issue
                foreach (var item in result)
                {
                    item.M.ApprovalStatus = item.ApprovalStatus;

                }

                if (isMyteam)
                {
                    foreach (var salesCode in myTeamSalesCodes)
                    {
                        finalQM.AddRange(result.Where(r => r.P.ERPID == salesCode).Select(r => r.M).ToList());
                    }

                    //Get belong insidesales QM
                    var insideSalesQM = result.Where(q => String.Equals(q.M.salesEmail, _CurrentMail, StringComparison.CurrentCultureIgnoreCase)).Select(r => r.M).ToList();
                    if (insideSalesQM != null)
                        finalQM.AddRange(insideSalesQM);
                }
                else
                {
                    foreach (var item in result)
                    {
                        finalQM.Add(item.M);
                    }
                }



            }
            catch (Exception e)
            {
                return new List<QuotationMaster>();
            }
            return finalQM.Distinct().OrderByDescending(x => x.quoteNo).ThenByDescending(x => x.Revision_Number).Take(300).ToList();
        }

        public static void SendFinalApprovalEmail(string quoteId, string region)
        {
            int Approved = (int)QuoteApprovalStatus.Approved;
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;
            IEnumerable<WorkFlowApproval> QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId).OrderBy(p => p.LevelNum).ToList();


            var finalMailBody = QAlist.LastOrDefault().FinalMailBody; // final mailbody不會有GP

            if (finalMailBody == null)// 為了現存的沒有final mailbody的approval暫寫,直接取第一個mailbody當final
                finalMailBody = QAlist.FirstOrDefault().Mailbody;

            string url = QAlist.FirstOrDefault().Url.ToString().ToUpper();

            QuotationMaster quotationMaster = eQuotationContext.Current.QuotationMaster.Find(quoteId);

            string bcc = "", subject = "";
            StringBuilder sbComment = new StringBuilder();

            string txt = "approved";

            IEnumerable<string> HiddenApprovers = eQuotationContext.Current.ACN_Executive.Select(e => e.Email.ToLower()).ToList();

            IEnumerable<WorkFlowApproval> approvedlist = QAlist.Where(p => p.Status == Approved).OrderBy(p => p.LevelNum);
            foreach (var item in approvedlist)
            {
                //ACN approved 的comment不要秀出來
                if (region == "ACN")
                {
                    //Alex20180430 ACN Top Executive 歷程不要秀出來
                    if (HiddenApprovers.Contains(item.Approver.ToLower()))
                        continue;
                    sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0} </b> </td><td style=\"padding:1px 15px;\">by {1} on {2}. </td></tr>", "Approved", item.ApprovedBy, GetLocalTime("A" + quotationMaster.org.Substring(0, 2), item.ApprovedDate.Value).ToString("MM/dd/yyyy HH:mm"));
                }
                else
                    sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0} </b> </td><td style=\"padding:1px 15px;\">by {1} on {2}. {3}</td></tr>", "Approved", item.ApprovedBy, GetLocalTime("A" + quotationMaster.org.Substring(0, 2), item.ApprovedDate.Value).ToString("MM/dd/yyyy HH:mm"), string.IsNullOrEmpty(item.ApprovedReason) /*|| (item.ApprovedReason != null && item.ApprovedReason.Contains("MobileApprove"))*/ ? "" : "Comments: " + item.ApprovedReason);
                sbComment.AppendFormat("</table>");

            }
            IEnumerable<WorkFlowApproval> approvedlist2 = QAlist.Where(p => p.Status == Rejected).OrderBy(p => p.LevelNum);
            foreach (var item in approvedlist2)
            {

                txt = "rejected";
                //Alex20180430 ACN Top Executive rejected的話不要秀出是誰
                if (region == "ACN" && HiddenApprovers.Contains(item.Approver.ToLower()))
                {
                    sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0} </b> </td><td style=\"padding:1px 15px;\"> on {1}. {2}</td></tr>", "Rejected", GetLocalTime("A" + quotationMaster.org.Substring(0, 2), item.RejectedDate.Value).ToString("MM/dd/yyyy HH:mm"), string.IsNullOrEmpty(item.RejectReason) /*|| (item.RejectReason != null && item.RejectReason.Contains("MobileApprove"))*/ ? "" : "Comments: " + item.RejectReason);
                    sbComment.AppendFormat("</table>");
                }
                else
                {
                    sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0} </b> </td><td style=\"padding:1px 15px;\">by {1} on {2}. {3}</td></tr>", "Rejected", item.RejectedBy, GetLocalTime("A" + quotationMaster.org.Substring(0, 2), item.RejectedDate.Value).ToString("MM/dd/yyyy HH:mm"), string.IsNullOrEmpty(item.RejectReason) /*|| (item.RejectReason != null && item.RejectReason.Contains("MobileApprove"))*/ ? "" : "Comments: " + item.RejectReason);
                    sbComment.AppendFormat("</table>");
                }
            }
            subject = string.Format("Quote ({0}) has been {1}", quotationMaster.quoteNo, txt);




            bcc = ConfigurationManager.AppSettings["EQV3_BCCMember"];

            string final_mailTo = quotationMaster.createdBy;
            string salesEmail = QuoteBusinessLogic.GetSalesEmailByQuoteID(quotationMaster.quoteId);
            if (salesEmail != null && salesEmail != final_mailTo)
            {
                final_mailTo += ";" + salesEmail;
            }
            if (!string.IsNullOrEmpty(quotationMaster.salesEmail) && quotationMaster.salesEmail != salesEmail)
            {
                final_mailTo += ";" + quotationMaster.salesEmail;
            }

            string[] testingRecivers = GetTestingReceivers(url, region, salesEmail);

            QuoteBusinessLogic.SendEQV3EmailGeneral("myadvantech@advantech.com", final_mailTo, "", bcc, subject, "", finalMailBody + sbComment.ToString(), "", url, region, testingRecivers, IsTestingForEQ3(url));

        }

        public static string[] GetTestingReceivers(string url, string region, string salesEmail)
        {
            string[] testingRecivers = null;
            if (!url.ToLower().Contains("localhost"))
            {
                /// For ACN 才去找sector, 因各sector測試信件收件人不同
                if (region == "ACN" && salesEmail != null)
                {
                    string sector = UserRoleBusinessLogic.getSectorBySalesEmail(salesEmail);// 未來可能要改掉,因為只有ACN業務才會在這張表裡面,改用emanager 為來源判斷sector?
                    testingRecivers = GetTestingMailReceiver(region, sector).Split(';'); // 由RegionSectorParameter 表來控管
                }
                else
                    testingRecivers = GetTestingMailReceiver(region, "").Split(';');

            }
            return testingRecivers;
        }

        public static bool IsTestingForEQ3(string url)
        {
            bool IsTesting = true;
            if (url.Equals("http://equotation.advantech.com", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;
            if (url.Equals("https://equotation.advantech.com", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;
            if (url.Equals("http://172.20.1.30:8600", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;
            if (url.Equals("https://172.20.1.30:8600", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;

            return IsTesting;
        }

        public static void SendApproveEmail(string QuoteID, string region)
        {
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Find(QuoteID);

            int Approved = (int)QuoteApprovalStatus.Approved;
            int Rejected = (int)QuoteApprovalStatus.Rejected;
            int Wait = (int)QuoteApprovalStatus.Wait_for_review;

            string bcc = "", subject = "";
            bcc = ConfigurationManager.AppSettings["EQV3_BCCMember"];

            string salesEmail = QuoteBusinessLogic.GetSalesEmailByQuoteID(QM.quoteId);

            StringBuilder sbComment = new StringBuilder();

            IEnumerable<WorkFlowApproval> QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == QuoteID).OrderBy(p => p.LevelNum).ToList();
            string Mailbody = QAlist.FirstOrDefault().Mailbody + "<style>  th { background-color:#C60; color:#FFF;}</style>";
            string url = QAlist.FirstOrDefault().Url.ToString().ToUpper();
            string[] testingRecivers = GetTestingReceivers(url, region, salesEmail);
            bool IsTesting = IsTestingForEQ3(url);

            //decimal MaxLevelNum = QAlist.Max(p => p.LevelNum);
            //int loopMaxNubmer = (int)(MaxLevelNum * 10);

            foreach (var levelNum in QAlist.Select(a => a.LevelNum).Distinct())
            {
                IEnumerable<WorkFlowApproval> currlist = from a in QAlist where a.LevelNum == levelNum select a;

                List<WorkFlowApproval> approvedList = currlist.Where(p => p.Status == Approved).ToList();
                if (approvedList != null && approvedList.Count > 0)
                {
                    if (approvedList.Count != currlist.ToList().Count)  // 如果同一level的簽核者沒全部approve 則不進行發送信件給下一層簽核者
                        return;
                    foreach (var approved in approvedList)
                    {
                        var comment = string.IsNullOrEmpty(approved.ApprovedReason) /*|| (approved.ApprovedReason != null && approved.ApprovedReason.Contains("MobileApprove"))*/ ? "" : "Comments: " + approved.ApprovedReason;
                        sbComment.AppendFormat("<table><tr><td style=\"font-family:Arial;color:#1965b1;\"><b>{0} </b></td><td style=\"padding:1px 15px;\">by {1} on {2}. {3}</td></tr>", "Approved", approved.ApprovedBy, GetLocalTime("A" + QM.org.Substring(0, 2), approved.ApprovedDate.Value).ToString("MM/dd/yyyy HH:mm"), comment);
                        sbComment.AppendFormat("</table>");
                    }
                    continue;

                }

                IEnumerable<WorkFlowApproval> Waitlist = currlist.Where(p => p.Status == Wait);
                if (Waitlist.Any())
                {
                    foreach (WorkFlowApproval item in Waitlist)
                    {
                        subject = string.Format("eQuotation Approval request for:({0} V{1})", QM.quoteNo, QM.Revision_Number);
                        StringBuilder sb = new StringBuilder();

                        sb.AppendFormat("<BR/><table align='center'>");
                        sb.AppendFormat("<tr><td><table cellpadding='0' cellmargin='0' border='0' height='44' width='120' style='border-collapse: collapse; border:5px solid #89D085' align='center'><tr><td bgcolor='#89D085' valign='middle' align='center' width='118'><div style='font-size: 18px; color: #ffffff; line-height: 1; margin: 0; padding: 0; mso-table-lspace:0; mso-table-rspace:0;'>");
                        sb.AppendFormat("<a href='{0}?ReturnUrl=/Quotes/QuoteMobileApprove?mobileId={1}' style='text-decoration: none; color: #ffffff; border: 0; font-family: Arial, arial, sans-serif; mso-table-lspace:0; mso-table-rspace:0;' border='0'>Approve</a></div></td></tr></table></td>", url, item.MobileYes);

                        if (region == "ACN")//Alex20180508: ACN PSM收到的信需要多2個Approve button with expired date
                        {
                            List<EQPARTNER> eqp = new eQuotationDAL().GetEQPartnerByQuoteID(QuoteID);
                            var salesCode = (from d in eqp where d.QUOTEID == QuoteID && d.TYPE == "E" select d.ERPID).FirstOrDefault();
                            string sector = UserRoleBusinessLogic.getSectorBySalesEmailSalesCode(salesEmail, salesCode, region);
                            int PSMLevel = GPControlBusinessLogic.GetPSMApproverLevelPriority("ACN", sector);
                            int currentIntLevel = Convert.ToInt16(Math.Floor(item.LevelNum));
                            if (item.LevelNum > PSMLevel && currentIntLevel == PSMLevel)
                            {
                                sb.AppendFormat("<td><table cellpadding='0' cellmargin='0' border='0' height='44' width='300' style='border-collapse: collapse; border:5px solid #89D085' align='center'><tr><td bgcolor='#89D085' valign='middle' align='center' width='298'><div style='font-size: 18px; color: #ffffff; line-height: 1; margin: 0; padding: 0; mso-table-lspace:0; mso-table-rspace:0;'>");
                                sb.AppendFormat("<a href='{0}?ReturnUrl=/Quotes/QuoteMobileApproveWithExpiredDate90?mobileId={1}' style='text-decoration: none; color: #ffffff; border: 0; font-family: Arial, arial, sans-serif; mso-table-lspace:0; mso-table-rspace:0;' border='0'>Approve and valid for 90 days</a></div></td></tr></table></td>", url, item.MobileYes);

                                sb.AppendFormat("<td><table cellpadding='0' cellmargin='0' border='0' height='44' width='300' style='border-collapse: collapse; border:5px solid #89D085' align='center'><tr><td bgcolor='#89D085' valign='middle' align='center' width='298'><div style='font-size: 18px; color: #ffffff; line-height: 1; margin: 0; padding: 0; mso-table-lspace:0; mso-table-rspace:0;'>");
                                sb.AppendFormat("<a href='{0}?ReturnUrl=/Quotes/QuoteMobileApproveWithExpiredDate180?mobileId={1}' style='text-decoration: none; color: #ffffff; border: 0; font-family: Arial, arial, sans-serif; mso-table-lspace:0; mso-table-rspace:0;' border='0'>Approve and valid for 180 days</a></div></td></tr></table></td>", url, item.MobileYes);
                            }
                        }

                        sb.AppendFormat("<td><table cellpadding='0' cellmargin='0' border='0' height='44' width='120' style='border-collapse: collapse; border:5px solid #EA5B3A' align='center'><tr><td bgcolor='#EA5B3A' valign='middle' align='center' width='118'><div style='font-size: 18px; color: #ffffff; line-height: 1; margin: 0; padding: 0; mso-table-lspace:0; mso-table-rspace:0;'>");
                        sb.AppendFormat("<a href='{0}?ReturnUrl=/Quotes/QuoteMobileApprove?mobileId={1}' style='text-decoration: none; color: #ffffff; border: 0; font-family: Arial, arial, sans-serif; mso-table-lspace:0; mso-table-rspace:0;' border='0'>Reject</a></div></td></tr></table></td>", url, item.MobileNo);


                        sb.AppendFormat("<td><table cellpadding='0' cellmargin='0' border='0' height='44' width='300' style='border-collapse: collapse; border:5px solid #2BAADF' align='center'><tr><td bgcolor='#2BAADF' valign='middle' align='center' width='298'><div style='font-size: 18px; color: #ffffff; line-height: 1; margin: 0; padding: 0; mso-table-lspace:0; mso-table-rspace:0;'>");
                        sb.AppendFormat("<a href='{0}?ReturnUrl=/Quotes/ReviewQuoteApproval?approveID={1}' style='text-decoration: none; color: #ffffff; border: 0; font-family: Arial, arial, sans-serif; mso-table-lspace:0; mso-table-rspace:0;' border='0'>Approve/Reject with Comment</a></div></td></tr></table></td>", url, item.UID);
                        sb.AppendFormat("</tr></table>");
                        try
                        {
                            //Check if any agent for current reciver
                            var cc = "";
                            var agentemail = SqlProvider.dbExecuteScalar("EQ", String.Format("select top 1 aemail from Agent where email = '{0}' and  GETDATE() > fDate and GETDATE() < tDate ", item.Approver));
                            if (agentemail != null && !string.IsNullOrEmpty(agentemail.ToString()))
                            {
                                cc = agentemail.ToString();
                            }
                            SendEQV3EmailGeneral("myadvantech@advantech.com", item.Approver, cc, bcc, subject, "", item.Mailbody + sbComment.ToString() + sb.ToString(), "", url, region, testingRecivers, IsTesting);
                            item.IsSendSuccessfully = 1;
                            item.Update();
                        }
                        catch { throw; }
                    }
                    break;
                }

            }

        }

        private static void SendEQV3EmailGeneral(string FROM_Email, string TO_Email, string CC_Email, string BCC_Email, string Subject_Email, string AttachFile, string MailBody, string str_type, string url, string region, string[] testingRecivers, bool IsTesting = true)
        {
            try
            {
                System.Net.Mail.MailMessage oMail = new System.Net.Mail.MailMessage();

                if (!string.IsNullOrEmpty(FROM_Email))
                {
                    oMail.From = new System.Net.Mail.MailAddress(FROM_Email);
                }
                else
                {
                    FROM_Email = "myadvantech@advantech.com";
                    oMail.From = new System.Net.Mail.MailAddress(FROM_Email);
                }


                if (IsTesting)//Testing就只寄給myadvantech 與 Region相關測試人員
                {
                    oMail.To.Add("eQ.Helpdesk@advantech.com");

                    if (testingRecivers != null)
                    {
                        foreach (var email in testingRecivers)
                            if (!string.IsNullOrEmpty(email))
                                oMail.To.Add(email);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(TO_Email))
                    {
                        TO_Email = TO_Email.Replace(',', ';');
                        foreach (string item in TO_Email.Split(';'))
                        {
                            oMail.To.Add(item);
                        }
                    }

                }



                if (!IsTesting && !string.IsNullOrEmpty(CC_Email))
                {
                    CC_Email = CC_Email.Replace(',', ';');
                    foreach (string item in CC_Email.Split(';'))
                    {
                        oMail.CC.Add(item);
                    }
                }


                if (!IsTesting && !string.IsNullOrEmpty(BCC_Email))
                {
                    BCC_Email = BCC_Email.Replace(',', ';');
                    foreach (string item in BCC_Email.Split(';'))
                    {
                        oMail.Bcc.Add(item);
                    }
                }


                oMail.IsBodyHtml = true;
                oMail.Body = RemoveHTMLTag(MailBody);
                oMail.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                oMail.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                if (IsTesting)
                {
                    oMail.Subject = "[eQuotation Testing] " + Subject_Email + "         TO:" + TO_Email + "         CC:" + CC_Email + "         BCC:" + BCC_Email;
                }
                else
                {
                    oMail.Subject = Subject_Email;
                }


                if (!string.IsNullOrWhiteSpace(AttachFile))
                {
                    foreach (string item in AttachFile.Split(';'))
                    {
                        oMail.Attachments.Add(new System.Net.Mail.Attachment(item));
                    }
                }
                //因應各區會使用不同mail server(ex. ACN), maintain mail server address in parameter table
                var oSmp = new System.Net.Mail.SmtpClient(GetSMTPServerByRegion(region));

                try
                {
                    oSmp.Send(oMail);
                }
                catch (Exception ex)
                {
                    //throw;
                    oSmp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                    try
                    {
                        oSmp.Send(oMail);
                    }
                    catch
                    {
                        try
                        {
                            oSmp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServerBAK"]);
                            oSmp.Send(oMail);
                        }
                        catch { throw; }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static void SendEmailforACN(string FROM_Email, string TO_Email, string CC_Email, string BCC_Email, string Subject_Email, string AttachFile, string MailBody, string str_type, bool IsTesting = true)
        {
            System.Net.Mail.MailMessage oMail = new System.Net.Mail.MailMessage();

            if (!string.IsNullOrEmpty(FROM_Email))
            {
                oMail.From = new System.Net.Mail.MailAddress(FROM_Email);
            }
            else
            {
                FROM_Email = "myadvantech@advantech.com";
                oMail.From = new System.Net.Mail.MailAddress(FROM_Email);
            }


            if (IsTesting)//Testing就只寄給myadvantech 與 ACN 測試人員
            {
                oMail.To.Add("myadvantech@advantech.com");

                string[] emails = ConfigurationManager.AppSettings["ACNTestingMember"].Split(';');
                //string[] emails = GetTestingMailReceiver()
                foreach (var email in emails)
                    oMail.To.Add(email);

            }
            else
            {
                if (!string.IsNullOrEmpty(TO_Email))
                {
                    TO_Email = TO_Email.Replace(',', ';');
                    foreach (string item in TO_Email.Split(';'))
                    {
                        oMail.To.Add(item);
                    }
                }

            }

            if (!IsTesting && !string.IsNullOrEmpty(CC_Email))
            {
                CC_Email = CC_Email.Replace(',', ';');
                foreach (string item in CC_Email.Split(';'))
                {
                    oMail.CC.Add(item);
                }
            }


            if (!IsTesting && !string.IsNullOrEmpty(BCC_Email))
            {
                BCC_Email = BCC_Email.Replace(',', ';');
                foreach (string item in BCC_Email.Split(';'))
                {
                    oMail.Bcc.Add(item);
                }
            }


            oMail.IsBodyHtml = true;
            oMail.Body = RemoveHTMLTag(MailBody);
            oMail.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            oMail.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            if (IsTesting)
            {
                oMail.Subject = "[eQuotation Testing] " + Subject_Email + "         TO:" + TO_Email + "         CC:" + CC_Email + "         BCC:" + BCC_Email;
            }
            else
            {
                oMail.Subject = Subject_Email;
            }


            if (!string.IsNullOrWhiteSpace(AttachFile))
            {
                foreach (string item in AttachFile.Split(';'))
                {
                    oMail.Attachments.Add(new System.Net.Mail.Attachment(item));
                }
            }

            var oSmp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["ACNSMTPServer"]);

            try
            {
                oSmp.Send(oMail);
            }
            catch (Exception ex)
            {
                //throw;
                oSmp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                try
                {
                    oSmp.Send(oMail);
                }
                catch
                {
                    try
                    {
                        oSmp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServerBAK"]);
                        oSmp.Send(oMail);
                    }
                    catch { throw; }
                }
            }
        }

        /// <summary>
        /// 移除html tag
        /// </summary>
        /// <param name="htmlSource"></param>
        /// <returns></returns>
        public static string RemoveHTMLTag(string htmlSource)
        {
            //移除  javascript code.
            htmlSource = Regex.Replace(htmlSource, @"<script[\d\D]*?>[\d\D]*?</script>", String.Empty);

            //移除html tag.
            //htmlSource = Regex.Replace(htmlSource, @"<[^>]*>", String.Empty);
            return htmlSource;
        }

        public static string getGPmailBodyForACNApprover(string QuoteID)
        {
            QuotationMaster _QMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID);
            List<QuotationDetail> items = _QMaster.QuotationDetail;
            decimal TotalAmount = 0; decimal TotalITP = 0;
            decimal TotalMargin = 0; string msg = string.Empty;
            var GPitems = new List<QuotationDetail>(); decimal Rate = 0;
            IsNeedGPforIntercon(QuoteID, ref Rate, ref GPitems);
            //Frank 20151222 exculde quote items which has spr no.
            foreach (QuotationDetail item in items)
            {
                //if (item.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                if (item.IsServicePartX) { continue; }
                if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { continue; }

                TotalAmount += item.newUnitPrice.Value * item.qty.Value;
                TotalITP += item.newItp.Value * item.qty.Value;
            }
            if (TotalAmount > 0)
            {
                TotalMargin = (TotalAmount - TotalITP) / TotalAmount;
            }
            else
            {
                TotalMargin = 0;
            }

            // Calculate max gp percent rate
            DataTable _dtGPParas = eQuotationDAL.getGP_Parameter("Intercon");
            Decimal MaxGPPercent = 0;
            foreach (DataRow itemGP in _dtGPParas.Rows)
            {
                if (Convert.ToDecimal(itemGP["GP_Percent"].ToString().Trim()) > MaxGPPercent)
                    MaxGPPercent = Convert.ToDecimal(itemGP["GP_Percent"].ToString().Trim());
            }
            MaxGPPercent = MaxGPPercent * 100;

            string str = "";
            str += "<table style='width:80%;'  class='table table-bordered'><tr><td style=\"font-family:Arial;font-weight:bold;\"><nobr>Acount Name</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Acount Id</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Total</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Margin</nobr></td>" + "<td style=\"font-family:Arial;font-weight:bold;\"><nobr>Create By</nobr></td></TR>";
            if ((_QMaster != null))
            {
                str += "<tr>";
                str += "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.quoteToName + "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.quoteToErpId + "</nobr></td>";

                str += "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.currency + ": " + (items.Sum(p => p.qty * p.newUnitPrice)).Value.ToString("N2") + "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + (TotalMargin).ToString("P") + "</nobr></td>";
                str += "</nobr></td>" + "<td style=\"font-family:Arial;\"><nobr>" + _QMaster.createdBy + "</nobr></td>";
                str += "</TR>";

                StringBuilder SB = new StringBuilder();
                SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>");
                SB.AppendFormat(" <ul>");
                SB.AppendFormat("<li>Below GP</li>");
                SB.AppendFormat("</ul>");
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;text-align:left;\">" + SB.ToString() + "</td></tr>";
                str += "<tr><td colspan=\"4\" style=\"font-family:Arial;text-align:left;\"><b>Reason: </b>" + _QMaster.relatedInfo + "</td></tr>";
            }
            str += "</table>";
            str += "<HR/>";

            string _tdHeader = "<td style='border:1px solid darkgray;border-collapse:collapse;font-family:Arial;font-weight:bold;text-align:center;'>";
            string _td = "<td style='border:1px solid darkgray;border-collapse:collapse;font-family:Arial;text-align:center;'>";

            str += "<table style='border:1px black solid;' rules='all' cellpadding='1'; width='99%' ><tr>" + _tdHeader + "Line No</td>" + _tdHeader + "Part No</td>" + _tdHeader + "List Price</td>" + _tdHeader + "Unit Price</td>" + _tdHeader + "QTY</TD>" + _tdHeader + "Sub Total</td>" + _tdHeader + "Margin</td>" + _tdHeader + "SPR</td>" + _tdHeader + "Note</td></TR>";
            decimal itp = 0, newUnitPrice = 0, listPrice = 0, SubTotal = 0, ItemMargin = 0;
            string ItemMarginStr = string.Empty;
            foreach (var item in items)
            {
                itp = item.newItp.Value;
                listPrice = item.listPrice.Value;
                newUnitPrice = item.newUnitPrice.Value;
                SubTotal = item.newUnitPrice.Value * item.qty.Value;

                if (item.line_No >= 100 && item.line_No % 100 == 0)
                {
                    //Frank 20160129: Here we only culcalate one system's price, so the itp, unit price and list price need to be divided by system total qty
                    var subItems = items.Where(p => p.HigherLevel == item.line_No && !p.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase));
                    int _ParentItemQty = item.qty.Value;
                    if (_ParentItemQty < 1) { _ParentItemQty = 1; }
                    //itp = subItems.Sum(p => p.newItp * p.qty.Value).Value;
                    itp = subItems.Sum(p => p.newItp * p.qty.Value).Value / _ParentItemQty;
                    SubTotal = subItems.Sum(p => p.newUnitPrice * p.qty.Value).Value;
                    newUnitPrice = SubTotal / _ParentItemQty;
                    listPrice = (subItems.Sum(p => p.listPrice * p.qty.Value).Value) / _ParentItemQty;
                }

                if (newUnitPrice > 0)
                {
                    ItemMargin = (newUnitPrice - itp) / newUnitPrice * 100;
                }
                else
                {
                    ItemMargin = 0;
                }

                str += "<tr>";
                ItemMarginStr = Math.Round(ItemMargin, 2).ToString("N2") + "%";

                //if (item.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase)) { ItemMarginStr = ""; }
                //if (item.ItemTypeX == Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent) { ItemMarginStr = ""; }
                if (item.IsServicePartX) { ItemMarginStr = ""; }

                str += _td + item.line_No + "</td>" + _td + item.partNo + "</td>" + _td + _QMaster.currency + " " + listPrice.ToString("N2") + "</td>" + _td + _QMaster.currency + " " + newUnitPrice.ToString("N2") + "</td>" + _td + item.qty.ToString() + "</td>" + _td + _QMaster.currency + " " + SubTotal.ToString("N2") + "</td>" + _td + ItemMarginStr + "</td>" + _td + item.sprNo + "</td>";
                var ckstr = "";
                if (null != GPitems.FirstOrDefault(p => p.line_No == item.line_No && p.partNo == item.partNo))
                {
                    if (ItemMargin > MaxGPPercent)
                        ckstr = "<Font color='red'>Below min price.</font>";
                    else
                        ckstr = "<Font color='red'>Below GP.</font>";
                }
                str += _td + ckstr + "</td>";
                str += "</tr>";
            }

            str += "</TABLE>";

            return str;
        }

        public static void UpdateAllQuotesERPIDByRowID(String _ERPID, String _AccountRowID)
        {
            List<QuotationMaster> Quotes = QuotationMasterHelper.GetQuotationMastersByAccountRowID(_AccountRowID);
            if (Quotes != null && Quotes.Count > 0)
            {
                foreach (QuotationMaster q in Quotes)
                {
                    UpdateErpIdAllRelated(_ERPID, q.quoteId, _AccountRowID);
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        public static void UpdateErpIdAllRelated(string quoteToErpId, string quoteId, string quoteToRowId)
        {
            // Update ERP ID for all quotes with same quoteToRowId
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Where(x => x.quoteId == quoteId).First();
            if (QM != null)
            {
                QM.quoteToErpId = quoteToErpId;
                eQuotationContext.Current.Entry(QM).State = EntityState.Modified;
            }

            // Frank 2012/07/27
            // 還要將ERPID塞入到EQPARTNER裡面, 包含Sold / Bill / Ship - To三條數據, 每一條都用同樣的ADDRESS / ATTENTION / TEL
            // 請使用SAPTools.SearchAllSAPCompanySoldBillShipTo來抓出address這些數據, call一次即可
            // D:\workspace\MyAdvantech\eDoc\App_Code\Business.vb > updateQuoteToErpId
            DataTable _dt = SearchAllSAPCompanySoldBillShipTo(quoteToErpId, "", "", "", "", "", "", "", ""); // 有點慢
            if (_dt.Rows.Count > 0)
            {
                DbSet<EQPARTNER> eqp = eQuotationContext.Current.EQPARTNER;
                DataRow _row = _dt.Rows[0];
                List<EQPARTNER> SoldToTable = eqp.Where(x => x.QUOTEID == quoteId && x.TYPE == "SOLDTO").ToList();
                List<EQPARTNER> ShipToTable = eqp.Where(x => x.QUOTEID == quoteId && x.TYPE == "S").ToList();
                List<EQPARTNER> BillToTable = eqp.Where(x => x.QUOTEID == quoteId && x.TYPE == "B").ToList();

                // insert sold to
                if (SoldToTable.Count > 0)
                {
                    foreach (EQPARTNER entity in SoldToTable)
                    { eqp.Remove(entity); }
                }
                EQPARTNER newEntity_SoldTo = new EQPARTNER
                {
                    QUOTEID = quoteId,
                    ROWID = quoteToRowId,
                    ERPID = quoteToErpId,
                    NAME = _row["COMPANY_NAME"].ToString(),
                    ADDRESS = _row["Address"].ToString(),
                    TYPE = "SOLDTO",
                    TEL = _row["TEL_NO"].ToString(),
                    MOBILE = _row["TEL_NO"].ToString()
                };
                eqp.Add(newEntity_SoldTo);

                // insert ship to
                if (ShipToTable.Count == 0)
                {
                    EQPARTNER newEntity_ShipTo = new EQPARTNER
                    {
                        QUOTEID = quoteId,
                        ROWID = quoteToRowId,
                        ERPID = quoteToErpId,
                        NAME = _row["COMPANY_NAME"].ToString(),
                        ADDRESS = _row["Address"].ToString(),
                        TYPE = "S",
                        TEL = _row["TEL_NO"].ToString(),
                        MOBILE = _row["TEL_NO"].ToString()
                    };
                    eqp.Add(newEntity_ShipTo);
                }
                else//除了所以只更新ERPID外，其他資訊都要能修改,所以只更新ERPID
                {
                    foreach (var item in ShipToTable)
                        item.ERPID = quoteToErpId; // wait to check
                }



                // insert bill to
                if (BillToTable.Count == 0)
                {
                    EQPARTNER newEntity_BillTo = new EQPARTNER
                    {
                        QUOTEID = quoteId,
                        ROWID = quoteToRowId,
                        ERPID = quoteToErpId,
                        NAME = _row["COMPANY_NAME"].ToString(),
                        ADDRESS = _row["Address"].ToString(),
                        TYPE = "B",
                        TEL = _row["TEL_NO"].ToString(),
                        MOBILE = _row["TEL_NO"].ToString()
                    };
                    eqp.Add(newEntity_BillTo);
                }
                else//除了所以只更新ERPID外，其他資訊都要能修改,所以只更新ERPID
                {
                    foreach (var item in BillToTable)
                        item.ERPID = quoteToErpId; // wait to check
                }
            }

            eQuotationContext.Current.SaveChanges();
        }

        // D:\workspace\MyAdvantech\eDoc\App_Code\SAPTools.vb > SearchAllSAPCompanySoldBillShipTo
        private static DataTable SearchAllSAPCompanySoldBillShipTo(string ERPID, string Org_id, string CompanyName, string Address, string State,
        string Division, string SalesGroup, string SalesOffice, string ComType, string isAll = "")
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            if (ComType.Equals("EM", StringComparison.OrdinalIgnoreCase))
            {
                // For AJP end customer searching, needs to select all JP01 customer ID, else only allowed to select EMs which are under Sold-to
                if (Org_id.Equals("JP01", StringComparison.OrdinalIgnoreCase))
                { sb.Append(Advantech.Myadvantech.Business.OrderBusinessLogic.GetAJPAddressString(ERPID, CompanyName)); }
                else
                { sb.Append(Advantech.Myadvantech.Business.OrderBusinessLogic.GetSAPPartnerAddressString("", ERPID, CompanyName, "EM")); }
            }
            else
            {
                sb.AppendLine(" SELECT distinct A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME, " + " D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1) AS Address, "); // B.STRAS AS ADDRESS,
                sb.AppendLine(" B.Land1 AS  COUNTRY,B.Ort01 AS CITY,");
                sb.AppendLine(" B.PSTLZ AS ZIP_CODE, D.region AS STATE, B.TELF1 AS TEL_NO,B.TELFX AS FAX_NO, ");
                sb.AppendLine(" case A.PARVW when 'WE' then 'Ship-To' when 'AG' then 'Sold-To' when 'RE' then 'Bill-To' end as PARTNER_FUNCTION, ");
                sb.AppendLine(" E.VKBUR as SalesOffice, E.VKGRP as SalesGroup, E.SPART as division, E.BZIRK as District,A.PARVW   ");
                sb.AppendLine(" FROM saprdp.knvp A  ");
                sb.AppendLine(" INNER JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR ");
                sb.AppendLine(" inner join saprdp.adrc D on  D.country=B.land1 and D.addrnumber=B.adrnr inner join saprdp.knvv E on B.KUNNR=E.KUNNR  ");
                sb.AppendLine(" where rownum<=300 ");
                if (!String.IsNullOrEmpty(State)) { sb.AppendFormat(" and Upper(D.region) LIKE '%{0}%' ", State.Replace("'", "''").Trim().ToUpper()); }
                if (!String.IsNullOrEmpty(Address)) { sb.AppendFormat(" and B.STRAS ||' '|| B.ORT01 ||' '|| B.REGIO ||' '|| B.PSTLZ ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1) LIKE '%{0}%' ", Address.Replace("'", "''").Trim()); }
                if (!String.IsNullOrEmpty(CompanyName)) { sb.AppendFormat(" and (Upper(B.NAME1) LIKE '%{0}%' or B.NAME2 like '%{0}%') ", CompanyName.Replace("'", "''").Trim().ToUpper()); }
                if (!String.IsNullOrEmpty(ERPID))
                {
                    // .AppendFormat(" and (A.Kunnr LIKE ' %{ 0}% ' or A.KUNN2 like ' %{ 0}% ') ", UCase(ERPID.Replace("'", "''").Trim))
                    if (isAll == "Y") { sb.AppendFormat(" and (A.Kunnr LIKE '%{0}%' ) ", ERPID.Replace("'", "''").Trim().ToUpper()); }
                    else { sb.AppendFormat(" and (A.Kunnr = '{0}' ) ", ERPID.Replace("'", "''").Trim().ToUpper()); }
                }
                if (!String.IsNullOrEmpty(Org_id))
                { sb.AppendFormat(" and A.VKORG = '{0}' ", Org_id.Replace("'", "''").Trim().ToUpper()); }
                if (!String.IsNullOrEmpty(Division)) { sb.AppendFormat(" and E.SPART = '{0}' ", Division.Replace("'", "''").Trim().ToUpper()); }
                if (!String.IsNullOrEmpty(SalesGroup)) { sb.AppendFormat(" and E.VKGRP = '{0}' ", SalesGroup.Replace("'", "''").Trim().ToUpper()); }
                if (!String.IsNullOrEmpty(SalesOffice)) { sb.AppendFormat(" and E.VKBUR = '{0}' ", SalesOffice.Replace("'", "''").Trim().ToUpper()); }
                // if( ! String.IsNullOrEmpty(ComType) ){ .AppendLine(" and (B.KTOKD='Z001' or B.KTOKD='" + ComType + "') ")
                if (!String.IsNullOrEmpty(ComType))
                {
                    switch (ComType)
                    {
                        case "SHIP":
                            sb.Append(" AND (A.PARVW = 'WE' OR A.PARVW = 'AG')");
                            sb.AppendFormat(" AND B.ktokd in ({0})", "'Z001','Z002'");
                            break;
                        case "BILL":
                            sb.Append(" AND (A.PARVW ='RE' OR A.PARVW = 'AG')");
                            sb.AppendFormat(" AND B.ktokd in ({0})", "'Z001','Z003'");
                            break;
                    }

                }
                sb.AppendFormat(" AND A.PARVW in ('WE','AG','RE') ORDER BY A.PARVW asc, A.KUNN2 desc", Org_id);
            }
            dt = OracleProvider.GetDataTable("SAP_PRD", sb.ToString());
            dt.TableName = "SAPPF";
            return dt;
        }

        public static string GetErpIdFromSiebelByRowId(string quoteID, string quoteToRowId)
        {
            string ErpId;
            string STR = String.Format("SELECT TOP 1 ISNULL(ATTRIB_05,'') FROM S_ORG_EXT_X WHERE ROW_ID='{0}'", quoteToRowId);
            DataTable dt = SqlProvider.dbGetDataTable("CRM", STR);
            if (dt.Rows.Count > 0)
            {
                ErpId = dt.Rows[0][0].ToString();
                if (Advantech.Myadvantech.Business.QuoteBusinessLogic.Is_Valid_Company_Id(quoteID, ErpId))
                { return ErpId; }
                else
                { return ""; }
            }
            else
            { return ""; }
        }

        public static bool Is_Valid_Company_Id(string quoteID, string company_id)
        {
            if (company_id.Trim() == "")
            {
                return false;
            }
            Dictionary<string, bool> Valid_Company = (Dictionary<string, bool>)HttpContext.Current.Cache["Valid_Company"];

            if (Valid_Company == null)
            {
                Valid_Company = new Dictionary<string, bool>();
                HttpContext.Current.Cache["Valid_Company"] = Valid_Company;
                HttpContext.Current.Cache.Add("Valid_Company", Valid_Company, null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }

            if ((Valid_Company.ContainsKey(company_id.ToUpper())) == false)
            {
                string str = String.Format("select top 1 COMPANY_ID from SAP_DIMCOMPANY where COMPANY_ID='{0}' and COMPANY_Type='Z001'", company_id);
                DataTable dt = SqlProvider.dbGetDataTable("B2B", str);
                if (dt.Rows.Count == 0)
                {
                    // Dim ws As New MAMigrationWS.MAMigration ----- Marked Nada 
                    // Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Pick Account Sync ERP ID Start : " + company_id, "", Now.ToString, "")
                    // if( SyncCompanyIdFromSAP(company_id) AndAlso tbOPBase.dbGetDataTable("B2B", str).Rows.Count > 0 ){ Return True
                    // Ming add 20140404 Cache只记录验证为true的Erpid, 同步customer调用最新的同步方法
                    string errMsg = "";
                    ArrayList Erpids = new ArrayList();
                    Erpids.Add(company_id);
                    SAPDAL.DimCompanySet ds = SAPDAL.syncSingleCompany.syncSingleSAPCustomer(Erpids, false, ref errMsg);
                    if (!string.IsNullOrEmpty(errMsg.Trim()))
                    {
                        bool IsTesting = true;
                        IEnumerable<WorkFlowApproval> QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteID).OrderBy(p => p.LevelNum).ToList();
                        string Mailbody = QAlist.FirstOrDefault().Mailbody + "<style>  th { background-color:#C60; color:#FFF;}</style>";
                        string url = QAlist.FirstOrDefault().Url.ToString().ToUpper();

                        //if (url.Contains("eq.advantech.com/".ToUpper())) IsTesting = false;
                        if (url.Equals("http://equotation.advantech.com", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;
                        if (url.Equals("https://equotation.advantech.com", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;
                        if (url.Equals("http://172.20.1.30:8600", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;
                        if (url.Equals("https://172.20.1.30:8600", StringComparison.InvariantCultureIgnoreCase)) IsTesting = false;
                        //SendEmailGeneral("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Pick Account Sync ERP ID Error : " + company_id, "", DateTime.Now.ToString() + "\r\n" + errMsg, "", url, IsTesting);
                        SendEQV3EmailGeneral("myadvantech@advantech.com", "eQ.Helpdesk@advantech.com", "", "", "eQuotation Pick Account Sync ERP ID Error : " + company_id, "", DateTime.Now.ToString() + "\r\n" + errMsg, "", url, null, null, IsTesting);
                        return false;
                    }
                    dt = SqlProvider.dbGetDataTable("B2B", str);
                }
                else
                {
                    Valid_Company.Add(company_id.ToUpper(), true);
                }
            }
            if (Valid_Company.ContainsKey(company_id.ToUpper())) ;
            {
                return Valid_Company[company_id.ToUpper()];
            }
            return false;
        }

        public static string GetTestingMailReceiver(string region, string sector)
        {
            return eQuotationDAL.GetRegionParameterValue(region, sector, "TestingMailReceiver", "");

        }

        public static string GetSMTPServerByRegion(string region)
        {
            return eQuotationDAL.GetRegionParameterValue(region, "", "SMTPServer", "172.20.0.76");//找不到就使用台北的172.20.0.76
        }

        public static List<string> GetDefaultTermsAndConditions(string region)
        {
            var extraTermsAndConditionStrings = eQuotationDAL.GetRegionParameterValue(region, "", "ExtraTermsAndConditions", "");
            string[] extraTermsAndConditionArray = extraTermsAndConditionStrings.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return extraTermsAndConditionArray.ToList();
        }

        public static string GetDefaultWarranty(string region)
        {
            return eQuotationDAL.GetRegionParameterValue(region, "", "WarrantyDescription", "");
            //string[] extraTermsAndConditionArray = extraTermsAndConditionStrings.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //return extraTermsAndConditionArray.ToList();
        }

        public static List<string> GetCurrencyOptions(string region)
        {
            var currencyStrings = eQuotationDAL.GetRegionParameterValue(region, "", "CurrencyOption", "");
            string[] currencyArray = currencyStrings.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return currencyArray.ToList();
        }

        public static List<decimal> GetTaxRateOptions(string region)
        {
            var taxRateStrings = eQuotationDAL.GetRegionParameterValue(region, "", "TaxRateOption", "");
            string[] taxRateArray = taxRateStrings.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return taxRateArray.Select(t=>Convert.ToDecimal(t)).ToList();
        }

        public static List<string> GetQuoteTitleOptions(string region)
        {
            var currencyStrings = eQuotationDAL.GetRegionParameterValue(region, "", "QuoteTitleOption", "");
            string[] currencyArray = currencyStrings.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return currencyArray.ToList();
        }

        public static bool IsOnlyContain968MSSW(List<QuotationDetail> quoteDetails)
        {
            if (quoteDetails != null && quoteDetails.Count() > 0)
            {
                foreach (var item in quoteDetails)
                {
                    SAP_PRODUCT sp = MyAdvantechDAL.GetSAPProduct(item.partNo);
                    if (sp.MATERIAL_GROUP != "968MS/SW")
                        return false;
                }
                return true;
            }
            return false;
        }

        public static List<QuotationDetail> GetACNOSPartITP(List<QuotationDetail> quoteDetails, string ERPId)
        {
            if (quoteDetails != null)
            {
                foreach (var item in quoteDetails)
                {
                    if (item.isACNOSParts)
                    {
                        var newItp = Advantech.Myadvantech.DataAccess.SAPDAL.GetACNOSPartITP(item.partNo, ERPId);
                        if (newItp > 0)
                        {
                            item.itp = newItp;
                            item.newItp = newItp;
                        }
                    }
                }
            }
            return quoteDetails;
        }

        public static List<QuotationDetail> GetACNLocalNumberPartITP(List<QuotationDetail> quoteDetails, string ERPId)
        {
            if (quoteDetails != null)
            {
                foreach (var item in quoteDetails)
                {
                    if (item.isACNLocalNumberParts)
                    {
                        var STOPlant = Advantech.Myadvantech.DataAccess.SAPDAL.GetSTOPlant(item.partNo, item.deliveryPlant);
                        if (!string.IsNullOrEmpty(STOPlant))
                        {
                            var newItp = Advantech.Myadvantech.DataAccess.SAPDAL.GetPeriodicCost(item.partNo, STOPlant, "CNY") * 1.05m;
                            if (newItp > 0)
                            {
                                item.itp = newItp;
                                item.newItp = newItp;
                            }
                        }

                    }
                }
            }
            return quoteDetails;
        }

        public static void updateQuoteWithNewExpiredDate(string quoteId, DateTime newExpiredDate)
        {
            var quote = QuoteBusinessLogic.GetQuotationMaster(quoteId);
            if (quote != null)
            {
                try
                {
                    quote.expiredDate = newExpiredDate;
                    quote.QuotationExtensionNew.NewExpiredDate = newExpiredDate;
                    eQuotationContext.Current.SaveChanges();
                }
                catch (Exception ex)
                {

                }

            }
        }

        public static decimal GetTaxRateByAPPRegionAndCountry(string region, string country)
        {
            decimal taxRate = Convert.ToDecimal(eQuotationDAL.GetRegionParameterValue(region, "", "TaxRate", "0"));
            if (region == "ASG")
            {
                if (country.Equals("singapore", StringComparison.InvariantCultureIgnoreCase))
                {
                    taxRate = 0.07m;
                }
            }
            return taxRate;
        }

        public static List<SalesRepresentative> GetSalesRepresentatives(string keyword, string region)
        {
            var salesList = new List<SalesRepresentative>();
            keyword = keyword.Trim().Replace("'", "''").Replace("*", "%");
            StringBuilder sql = new StringBuilder();
            if (region == "ACN")
            {
                sql.AppendLine(" select b.id_sap, c.EMAIL_ADDR ");
                sql.AppendLine(" from EAI_IDMAP b inner join EZ_EMPLOYEE c on b.id_email=c.EMAIL_ADDR  ");
                sql.AppendLine(" where b.id_fact_zone = 'China' and b.id_by_country <> 'X' ");
                sql.AppendFormat(" and (b.id_sap like '{0}%' or b.id_email like N'%{1}%') ", keyword, keyword);
                sql.AppendLine("  order by  b.id_sap ");
            }
            else if (region == "ABB")
            {
                sql.AppendLine(" select distinct b.id_sap, c.EMAIL_ADDR  ");
                sql.AppendLine(" from  EAI_IDMAP b inner join EZ_EMPLOYEE c on b.id_email = c.EMAIL_ADDR ");
                sql.AppendLine(" where b.id_fact_zone = 'B+B US' and b.id_by_country <> 'X' ");
                sql.AppendFormat(" and (b.id_sap like '{0}%' or b.id_email like N'%{1}%') ", keyword, keyword);
                sql.AppendLine("  order by  b.id_sap ");
            }
            else if (region == "ICG")
            {
                sql.AppendLine(" select distinct a.SALES_CODE, a.EMAIL ");
                sql.AppendLine(" from SAP_EMPLOYEE a inner join EAI_IDMAP b on a.SALES_CODE = b.id_sap ");
                sql.AppendLine(" inner join EZ_EMPLOYEE c on a.EMAIL = c.EMAIL_ADDR ");
                sql.AppendLine(" where b.id_rbu like '%ICG%' and b.id_by_country <> 'X' ");
            }
            else if (region == "ASG")
            {
                sql.AppendLine(" select distinct a.SALES_CODE, a.EMAIL    ");
                sql.AppendLine(" from SAP_EMPLOYEE a inner join EZ_EMPLOYEE b on a.EMAIL = b.EMAIL_ADDR ");
                sql.AppendLine(" where  a.SALESOFFICE = '8000' ");
                sql.AppendFormat(" and (a.SALES_CODE like '{0}%' or a.EMAIL like N'%{1}%') ", keyword, keyword);
                sql.AppendLine("  order by a.SALES_CODE ");
            }
            else if (region == "AAU")
            {
                sql.AppendLine(" select distinct b.id_sap, c.EMAIL_ADDR  ");
                sql.AppendLine(" from  EAI_IDMAP b inner join EZ_EMPLOYEE c on b.id_email = c.EMAIL_ADDR ");
                sql.AppendLine(" where b.id_fact_zone = 'AAU/NZ' and b.id_by_country <> 'X' ");
                sql.AppendFormat(" and (b.id_sap like '{0}%' or b.id_email like N'%{1}%') ", keyword, keyword);
                sql.AppendLine("  order by  b.id_sap ");
            }



            DataTable dt = SqlProvider.dbGetDataTable("MY", sql.ToString());

            if ((dt != null) && dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    var sales = new SalesRepresentative();
                    sales.SalesCode = dt.Rows[i][0].ToString();
                    sales.Email = dt.Rows[i][1].ToString();
                    salesList.Add(sales);
                }
            }

            // IT人員也可被加入Sales representative for testing purpose
            string[] ITMemberEmails = ConfigurationManager.AppSettings["EQV3_ITMember"].Split(';');
            if (ITMemberEmails.Contains(keyword))
            {
                var sales = new SalesRepresentative();
                sales.SalesCode = "49999999";
                sales.Email = keyword;
                salesList.Add(sales);
            }
            return salesList;
        }

        public static void UpdateItpBySprNo(ref QuotationMaster quote)
        {


            foreach (var item in quote.QuotationDetail)
            {
                if (!string.IsNullOrEmpty(item.sprNo))
                {
                    try
                    {
                        string err = "";
                        decimal newItp = 0m;
                        newItp = GetNewItpBySPR(item.sprNo, item.line_No.Value, quote.quoteToErpId, item.partNo, Convert.ToString(quote.expiredDate), ref err);
                        if (string.IsNullOrEmpty(err) && newItp > 0)
                            item.newItp = newItp;
                    }
                    catch { }
                }
            }
            quote.InitializeQuotationDetail();
        }

        public static decimal GetNewItpBySPR(string sprNo, int lineNo, string erpId, string partNo, string expiredDateString, ref string msg)
        {
            DateTime expiredDate = Convert.ToDateTime(expiredDateString);
            decimal newItp = 0m;
            var _sql = new StringBuilder();
            _sql.AppendLine(" select A.Request_No,A.RBU_NAME,A.CUS_ID,B.SpecialOffered ,B.Model,A.P_From, A.P_End,isnull(B.Request_SP,0) as Request_SP,A.STATUS, isnull(A.CUS_CURRENCY,'') AS CUS_CURRENCY ");
            _sql.AppendLine(" from  [SPR_record] A right join  SPR_Model B on  A.RECORD_ID =B.Record_ID ");
            _sql.AppendLine(string.Format(" where A.Request_No='{0}'", sprNo));

            DataTable DT = SqlProvider.dbGetDataTable("EZ", _sql.ToString());
            if (DT != null && DT.Rows.Count > 0)
            {
                DataRow[] dr = DT.Select("CUS_ID='" + erpId + "'");
                if (dr.Length == 0)
                {
                    msg = "The SPR is not applied for the quote-to account : " + erpId;
                    return newItp;
                }
                dr = DT.Select("Model='" + partNo + "'");
                if (dr.Length == 0)
                {
                    msg = "The part no. of this SPR is not the same as the one of the current quote line.";
                    return newItp;
                }
                DataRow dritem = dr[0];
                if (Convert.IsDBNull(dritem["SpecialOffered"]))
                {
                    msg = "Please input the SPR No. which is applied for the special ITP.";
                    return newItp;
                }
                if (Convert.IsDBNull(dritem["STATUS"]))
                {
                    if (!string.Equals("APPROVED", dritem["STATUS"]))
                    {
                        msg = "The SPR has not yet been approved.";
                        return newItp;
                    }
                }
                if (!Convert.IsDBNull(dritem["P_From"]) && !Convert.IsDBNull(dritem["P_End"]))
                {
                    DateTime P_From = DateTime.Now;
                    DateTime P_End = DateTime.Now;
                    if (DateTime.TryParse(dritem["P_From"].ToString(), out P_From) && DateTime.TryParse(dritem["P_End"].ToString(), out P_End))
                    {
                        if (P_From.Date <= expiredDate && expiredDate <= P_End.Date)
                        {
                            //Means SPR expired date and Quotation expired date is valid
                        }
                        else
                        {
                            if (DateTime.Now <= P_End.Date && P_From.Date <= expiredDate && expiredDate > P_End.Date)
                            {
                                //Quote.expiredDate = P_End.Date;
                            }
                            else
                            {
                                msg = "The SPR is expired.";
                                return newItp;
                            }
                        }
                    }
                }
                decimal SITP = 0m; decimal Request_SP = 0m;
                if (decimal.TryParse(dritem["SpecialOffered"].ToString(), out SITP) && decimal.TryParse(dritem["Request_SP"].ToString(), out Request_SP))
                {
                    if (SITP > 0 && Request_SP > 0)
                    {
                        newItp = SITP;
                        return newItp;
                    }
                }
            }
            else
            {
                msg = "The SPR No " + sprNo + " is invalid.";
            }

            return newItp;
        }

        public static DataTable GetPartNoByKeyWordAndOrg(string keyword, string org)
        {
            keyword = keyword.Trim().Replace("'", "''").Replace("*", "%");
            StringBuilder sql = new StringBuilder();


            if (org.StartsWith("CN"))
            {
                sql.AppendLine(" select distinct top 10 A.PART_NO, A.PRODUCT_DESC, '' as LegacyPN  from  dbo.SAP_PRODUCT A INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO  ");
                sql.AppendFormat(" where A.PART_NO like '{0}%' ", keyword);
                sql.AppendFormat(" and  B.PRODUCT_STATUS in {0}", "('A','N','H','O','M1','C','P','S2','S5','T','V')");
                sql.AppendFormat(" AND B.SALES_ORG ='{0}' ", org);
                sql.AppendLine(" order by part_no ");
            }
            else
            {
                sql.AppendLine(" select * from ( select distinct top 4 A.PART_NO, A.PRODUCT_DESC, ISNULL(C.MATNR_P,'') as LegacyPN ");
                sql.AppendLine(" from dbo.SAP_PRODUCT A INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO  ");
                sql.AppendLine(" left join SAP_PRODUCT_AFFILIATE_MAPPING c on a.PART_NO = c.MATNR ");
                sql.AppendFormat(" where A.PART_NO like '%{0}%' ", keyword);
                sql.AppendFormat(" and  B.PRODUCT_STATUS in {0}", "('A','N','H','O','M1','C','P','S2','S5','T','V')");
                sql.AppendFormat(" AND B.SALES_ORG ='{0}' ", org);
                sql.AppendLine(" union ");
                sql.AppendLine(" select distinct top 2 A.PART_NO, A.PRODUCT_DESC, ISNULL(C.MATNR_P,'') as LegacyPN ");
                sql.AppendLine(" from dbo.SAP_PRODUCT A INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO  ");
                sql.AppendLine(" left join SAP_PRODUCT_AFFILIATE_MAPPING c on a.PART_NO = c.MATNR ");
                sql.AppendFormat(" where c.MATNR_P Like '%{0}%' ", keyword);
                sql.AppendFormat(" and  B.PRODUCT_STATUS in {0}", "('A','N','H','O','M1','C','P','S2','S5','T','V')");
                sql.AppendFormat(" AND B.SALES_ORG ='{0}' ", org);
                sql.AppendLine("  ) t order by len(t.PART_NO) asc ");
            }
            DataTable dt = SqlProvider.dbGetDataTable("B2B", sql.ToString());
            return dt;
        }
    }
}
