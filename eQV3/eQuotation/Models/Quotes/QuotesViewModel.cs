using Advantech.Myadvantech.Business;
using Advantech.Myadvantech.DataAccess;
using eQuotation.Entities;
using eQuotation.Utility;
using eQuotation.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkFlowlAPI;

namespace eQuotation.Models.Quotes
{
    public class QuotesViewModel : ViewModelBase<object>
    {
        public string UserEmail { get; set; }
        public string AppRegion { get; set; }
        public string QuoteID { get; set; }
        public string QuoteNo { get; set; }
        public string RevisionNo { get; set; }
        public string Description { get; set; }
        public string AccountRowID { get; set; }
        public string AccountName{ get; set; }
        public string ERPID { get; set; }
        public string PONO { get; set; }
        public string Address { get; set; }
        public string Sales { get; set; }
        public string DateType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Status { get; set; }
        public DateTime QuoteDate { get; set; }

        public DateTime ExpiredDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ACNMaxNewExpiredDate { get; set; }

        public DateTime PrintDate { get; set; }
        public string Tel { get; set; }
        public string Contact { get; set; }
        public string InsideSales { get; set; }
        public decimal tax { get; set; }
        public decimal Freight { get; set; }
        public string ShipTerm { get; set; }
        public string Incoterms { get; set; }
        public string QuoteNote { get; set; }

        public List<ShippingMethod> ShippingMethods { get; set; }
        public string Warranty { get; set; }
        public string exp { get; set; }
        public string CommentExp { get; set; }
        public List<SelectListItem> GeneralRateList { get; set; }
        public List<SelectListItem> TaxRateList { get; set; }
        public string CustomerMailSubject
        {
            get
            {
                switch (this.AppRegion)
                {
                    case "ABB":
                        return " Advantech B+B SmartWorx eQuotation";
                    default:
                        return "Advantech eQuotation";

                }


            }
        }
        public decimal taxPercent
        {
            get
            {
                return this.tax * 100;
            }
        }
        public string PaymentTerm { get; set; }
        public string Currency { get; set; }
        public string CurrencySign { get; set; }
        public string org { get; set; }
        public string Opportunity { get; set; }
        public string OptyStage { get; set; }
        public string Parent { get; set; }

        public List<string> CreateDetailTitle;
        public List<QuotationDetail> QuotationDetails;
        public List<SelectListItem> ORGList;
        public List<SelectListItem> ContactList;
        public List<SelectListItem> InsideSalesList;
        public List<SelectListItem> ParentItem;
        public List<MyQuotationMaster> QM;
        public List<MyQuotationDetail> QD;
        public List<MyQuotationApproval> QA;
        public List<EQPARTNER> QuotationPartner;

        public EQPARTNER SoldToPartner;
        public EQPARTNER ShipToPartner;
        public EQPARTNER BillToPartner;

        public decimal QuoteTotalAmount_WithoutTax { get; set; }
        public decimal QuoteTotalAmount_WithTax { get; set; }
        public decimal QuoteTotalTax { get; set; }
        public decimal QuoteTotalGeneralAmount { get; set; }
        //PDF attributes
        public string AccountContactName { get; set; }
        public string AccountContactTel { get; set; }
        public string AccountContactEmail { get; set; }
        public string SalesName { get; set; }
        public string SalesTel { get; set; }
        public string SalesEmail { get; set; }
        public string SalesComment { get; set; }
        public string pagetype { get; set; }
        public bool ViewGP { get; set; }

        public string QuotationNotes { get; set; }

        public string Remarks { get; set; }

        public string QuotationTitle { get; set; }

        public DisplayItemOptionsForBto DisplayItemType { get; set; }

        //public List<SelectListItem> QuoteTitleOptions { get; set; }

        public string StrTemplate { get; set; }

        public bool GPIsViewable
        {
            get
            {
                if (this.Approval != null && this.Approval.ViewGP == 1)
                    return true;
                return false;
            }
        }

        public double ACNTotalMargin { get; set; }
        public double TotalMargin { get; set; }

        public WorkFlowApproval Approval { get; set; }

        public string ApprovalComment { get; set; }


        public string AdditionalApprover { get; set; }
        public IEnumerable<SelectListItem> AdditionalApproversOption { get; set; }
       
        public QuotationTemplateInformation QuoteTemplateInformation { get; set; }

        public IEnumerable<WorkFlowApproval> QuoteApprovalList { get; set; }


        public IEnumerable<string> HiddenApprovers
        {
            get
            {
                if(this.AppRegion == "ACN" && !TopExecutiveIsViewable)
                    return eQuotationContext.Current.ACN_Executive.Select(e=>e.Email.ToLower()).ToList();
                return new List<string>();
                
            }
        }

        public bool TopExecutiveIsViewable
        {
            get
            {             
                //get role actions for current user, and check if contain ViewApprovedQuoteComment action
                var mngr = new IdentityManager();
                return mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewTopExecutives");

            }
        }

        public List<string> DobulePSMList
        {
            get
            {
                var finalDoublePSMList = new List<string>();
                //Group by productDiv
                var PSMGroup = eQuotationContext.Current.ACN_EQ_PSM.Where(s => s.Level == 1).GroupBy(s => s.ProductDivision).Select(group =>
                         new
                         {
                             ProductDivision = group.Key,
                             PSMList = group.Select(a => a.PSM).ToList(),
                             Count = group.Count()
                         });

                //Loop group when group count >1 (duplicated default PSM)
                foreach (var g in PSMGroup.Where(g => g.Count > 1))
                {
                    var count = 0;
                    foreach (var approver in this.QuoteApprovalList.Where(a=>a.ApproverType == ApproverType.PSM.ToString()).Select(a => a.Approver))
                    {
                        if (g.PSMList.Contains(approver))
                            count++;
                    }
                    if (count > 1)//if count >1, it means approver list exist duplicated psm with same product division, add this duplicated psm to finalDoublePSMList
                        finalDoublePSMList.AddRange(g.PSMList);
                }
                return finalDoublePSMList;
            }
        }


        public WorkFlowlAPI.FindApproverResult FindApproverResult { get; set; }

        public ShippingResult ShippingResult { get; set; }

        public bool ApprovedCommentIsViewable
        {
            get
            {
                //get role actions for current user, and check if contain ViewApprovedQuoteComment action
                var mngr = new IdentityManager();
                return mngr.UserIsInAction(mngr.CurrentUser.Id, "ViewApprovedQuoteComment");

            }
        }

        public string SimulatePriceErrorMessage { get; set; }

        public WorkFlowlAPI.ApprovalResultMessage ApprovalResultMessage { get; set; }

        public bool GeneralRateIsAdjustable
        {
            get
            {
                //get role actions for current user, and check if contain updateGenralRate action
                var mngr = new IdentityManager();
                return mngr.UserIsInAction(mngr.CurrentUser.Id,"updateGeneralRate");

            }
        }

        public bool ExpiredDateIsAdjustable
        {
            get
            {
                //get role actions for current user, and check if contain updateExpiredDate action
                var mngr = new IdentityManager();
                return mngr.UserIsInAction(mngr.CurrentUser.Id, "updateExpiredDate");

            }
        }

        public bool SPRIsAdjustable
        {
            get
            {
                //get role actions for current user, and check if contain updateSPRNO action
                var mngr = new IdentityManager();
                return mngr.UserIsInAction(mngr.CurrentUser.Id, "updateSPRNO");

            }
        }

        public bool End2EndGPIsViewable
        {
            get
            {
                //get role actions for approver, and check if contain ViewEnd2EndGP action
                if (this.Approval != null && this.Approval.ViewGP == 1)
                {
                    var mngr = new IdentityManager();
                    var approver = mngr.GetUserByEmail(this.Approval.Approver);
                    if(approver!=null)
                        return mngr.UserIsInAction(approver.Id, "ViewEnd2EndGP");
                }
                return false;

            }
        }

        public bool ViewQuoteIsEnabled
        {
            get
            {
                if (AppContext.AppRegion == "ASG" || AppContext.AppRegion == "ACN")
                    return true;
                return false;
            }
        } 

        public QuotesViewModel()
        {
            this.UserEmail = AppContext.UserEmail;
            this.AppRegion = AppContext.AppRegion;
            this.DisplayItemType = DisplayItemOptionsForBto.SubItemWithPrice;
        }
        public override void Init()
        {
                var QuotationMaster = new QuotationMaster();
                HttpContext.Current.Session["QuotationMaster"] = QuotationMaster;
           
        }

        public override void SetValue()
        {
            //var QuotationMaster = new QuotationMaster();
            //QuotationMaster =(QuotationMaster)HttpContext.Current.Session["QuotationMaster"];
            //QuotationMaster.quoteNo = QuoteBusinessLogic.GetEQOrderNumber("ACNQ",true);
            //QuotationMaster.quoteId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            //QuotationMaster.quoteDate = DateTime.Now;
            //QuotationMaster.expiredDate = DateTime.Now;
            //QuotationMaster.createdBy = this.UserEmail;
            //QuotationMaster.createdDate = DateTime.Now;
            //QuotationMaster.LastUpdatedBy = this.UserEmail;
            //QuotationMaster.LastUpdatedDate = DateTime.Now;
            //QuotationMaster.deliveryDate = DateTime.Now.AddDays(3);
            //QuotationMaster.reqDate = DateTime.Now;
            //QuotationMaster.DOCSTATUS = 0;
            //QuotationMaster.PODate = DateTime.Now;
            //QuotationMaster.Revision_Number = 1;
            //QuotationMaster.Active = true;
            //QuotationMaster.tax = 0.17m;
            //this.QuoteNo = QuotationMaster.quoteNo;
            //this.QuoteID = QuotationMaster.quoteId;
            //this.ExpiredDate = DateTime.Now;
            //this.tax = QuotationMaster.tax.GetValueOrDefault();

            //DataTable dt = UserRoleBusinessLogic.GetACNSalesEmployee();
            //List<SelectListItem> list = new List<SelectListItem>();
            //if ((dt != null) && dt.Rows.Count > 0)
            //{
            //    list.Add(new SelectListItem() { Text = "", Value = "" });
            //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
            //    {
            //        list.Add(new SelectListItem() { Text = dt.Rows[i][1].ToString(), Value = dt.Rows[i][0].ToString() });
            //    }
            //}
            //this.ComboboxItem = list;
        }

        public override void GetValue(object data)
        {
            throw new NotImplementedException();
        }

  
    }

    public class Ship_Bill
    {
        public string AccountID;
        public string Name;
        public string ERPID;
        public string Address;
        public string City;
        public string State;
        public string Zipcode;
        public string Country;
        public string Attention;
        public string Tel;
        public string Type;
        public string Currency;
        public string SiebelRBU;
        public string SalesGroup;
        public string SalesOffice;
        public string Inco1;
        public string Inco2;
        public decimal TaxRate;
    }

    public class MyQuotationApproval
    {
        public string UID;
        public string quoteId;
        public string quoteNo;
        public string customId;
        public string quoteToName;
        public string salesEmail;
        public string quoteDate;
        public string DOCSTATUS;
        public string quoteToErpId;
        public bool Editable;
    }
    public class MyQuotationMaster
    {
        public string quoteId;
        public string quoteNo;
        public string customId;
        public string quoteToName;
        public string optyName;
        public string salesEmail;
        public string quoteDate;
        public string DOCSTATUS;
        public string quoteToErpId;
        public string FinishDate;
        public string ExpiredDate;
        public int RevisionNumber;
        public bool Editable;
        public bool RevisionIsEnabled;
        public bool ViewQuoteIsEnabled;
        public string Quote2OrderNo;
        public string Quote2OrderDate;
    }

    public class MyQuotationDetail
    {
        public string quoteId;
        public int line_No;
        public string partNo;
        public string description;
        public decimal listPrice;
        public decimal unitPrice;
        public decimal newUnitPrice;
        public int qty;
        public decimal SubTotal;
        public decimal ITP;
        public string SPRNO;
        public string currency;
        public string currencysign;
        public string PreTaxTotalAmount;
        public string TotalTaxAmount;
        public string PostTaxTotalAmount;
        public string TaxRate;
        public string GPStatus;
        public string CustomerPartNo;
        public decimal BelowSAPPriceRate;
        public decimal Margin;
        public decimal End2EndMargin;
        public decimal DefaultDiscountRate;
        public decimal SalesDiscountRate;
        public int itemtype;
        public Boolean NCNR;
        public string dlvplant;
        public string ABCIndicator;
    }

    public class QuotationTemplateInformation
    {
        public string RBUCompanyName;
        public string RBUCompanyTel;
        public string RBUCompanyFax;
        public string RBUCompanyAddress;
        public string RBUCompanyZipCode;
        public string RBUBankAccountNumber;
        public string RBUBankAccountName;
        public string RBUTermsAndCondition;
    }

    public class ShippingMethod
    {
        public string MethodName { get; set; }
        public string MethodValue { get; set; }
        public float ShippingCost { get; set; }
        public string DisplayShippingCost { get; set; }
        public string EstoreServiceName { get; set; }
        public string ErrorMessage { get; set; }

        public decimal TotalWeight { get; set; }

    }
}