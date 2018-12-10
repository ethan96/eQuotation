using Advantech.Myadvantech.DataAccess;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using eQuotation.ViewModels;
using eQuotation.Utility;
using System.ComponentModel.DataAnnotations;
using Advantech.Myadvantech.DataAccess.DataCore.eQuotation.Model;
using System.Linq;
using eQuotation.Validators;
using eQuotation.Models.Quotes;

namespace eQuotation.ViewModels.QuoteForm
{
    public class QuoteFormViewModel
    {
        public QuoteFormViewModel()
        {
            this.ContactOptions = new List<SelectListItem>();
            this.OrgOptions = new List<SelectListItem>();
            this.ExtraTermsAndConditionOptions = new List<ExtraTermsAndConditions>();
            this.ParentItemOptions = new List<SelectListItem>() {
                                            new SelectListItem()
                                            {
                                                Text = "Loose items",
                                                Value = ""
                                            }
                                        };
            this.OrgOptions = new List<SelectListItem>();
            this.SalesRepresentatives = new List<SalesRepresentative>();
            this.QuoteItems = new List<QuoteItemViewModel>();
            this.InsideSalesOptions = new List<SelectListItem>();
            this.CurrencyOptions = new List<SelectListItem>();
            this.TaxRateOptions = new List<SelectListItem>();
            this.QuoteApproval = new QuoteApprovalViewModel() { QuoteNo = this.QuoteNo};
            this.PaymentTermOptions = new List<SelectListItem>();
            this.EWPartList = new List<ExtendedWarrantyPartNo_V2>();
        }
        public bool IsReadonly { get; set; }
        public string Region { get; set; }
        public string QuoteId { get; set; }
        public string QuoteNo { get; set; }
        public string Address { get; set; }
        [EnsureMinimumElements(1, ErrorMessage = "At least a sales is required")]
        public List<SalesRepresentative> SalesRepresentatives { get; set; }
        public string InSideSales { get; set; }
        public string Description { get; set; }
        public string QuoteToRowId { get; set; }
        public string QuoteToErpId { get; set; }
        public string QuoteToName { get; set; }
        public string Currency { get; set; }
        public string CurrencySign {
            get
            {
                return Advantech.Myadvantech.DataAccess.MyExtension.GetCurrencySignByCurrency(this.Currency);
            }
        }
        public string Plant { get; set; }
        public string SalesEmail { get; set; }
        public string SalesRowId { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime PrintDate { get; set; }
        public DateTime ACNMaxNewExpiredDate { get; set; }
        public string RelatedInfo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IsRepeatedOrder { get; set; }
        public string Org { get; set; }
        public string SiebelRBU { get; set; }
        public string DistChan { get; set; }
        public string Division { get; set; }
        public string SalesGroup { get; set; }
        public string SalesOffice { get; set; }
        public string PoNo { get; set; }
        public string OriginalQuoteID { get; set; }
        public DateTime ReqDate { get; set; }
        public int DOCSTATUS { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime PoDate { get; set; }
        public short RevisionNumber { get; set; }
        public bool Active { get; set; }
        public string OptyId { get; set; }
        [Required]
        public string OptyName { get; set; }
        public string OptyStage { get; set; }
        //public string ApprovalReason { get; set; }
        public string Warranty { get; set; }
        public string CommentExp { get; set; }
        public string Inco1 { get; set; }
        public string Inco2 { get; set; }


        public List<SelectListItem> PaymentTermOptions { get; set; }
        public List<SelectListItem> ContactOptions { get; set; }
        public List<SelectListItem> OrgOptions { get; set; }
        public List<SelectListItem> InsideSalesOptions { get; set; }
        public List<SelectListItem> CurrencyOptions { get; set; }

        public List<SelectListItem> TaxRateOptions { get; set; }

        public List<ExtendedWarrantyPartNo_V2> EWPartList { get; set; }

        public EQPARTNER SoldToPartner { get; set; }
        public EQPARTNER ShipToPartner { get; set; }
        public EQPARTNER BillToPartner { get; set; }
        public EQPARTNER EndCustomer { get; set; }

        public string ShipTerm { get; set; }
        public string PaymentTerm { get; set; }
        public decimal Freight { get; set; }
        public decimal Tax { get; set; }
        public string QuoteNote { get; set; }
        public string Remark { get; set; }
        public string QuoteTitle { get; set; }
        public List<ExtraTermsAndConditions> ExtraTermsAndConditionOptions { get; set; }
        public List<SelectListItem> ParentItemOptions { get; set; }

        public string currentPartentOption { get; set; }

        public List<QuoteItemViewModel> QuoteItems { get; set; }

        public string ApprovalReason { get; set; }

        public WorkFlowlAPI.FindApproverResult FindApproverResult { get; set; }
        public QuoteApprovalViewModel QuoteApproval{ get; set; }


        public QuotesViewModel quote { get; set; }

        public string ErrorMessage { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal PreTaxTotalAmount
        {
            get
            {
                return QuoteItems.Sum(d => d.PreTaxSubTotal);
            }
        }
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal TotalTaxAmount
        {
            get
            {
                return this.PostTaxTotalAmount - this.PreTaxTotalAmount;
            }
        }
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal PostTaxTotalAmount
        {
            get
            {
                return this.QuoteItems.Sum(d => d.PostTaxSubTotal);
            }
        }

        public QuotationMaster ConverToQuote()
        {
            QuotationMaster quote = new QuotationMaster();

            quote.quoteNo = this.QuoteNo;
            quote.quoteId = this.QuoteId;
            quote.quoteToErpId = this.QuoteToErpId != null? this.QuoteToErpId: "";
            quote.quoteToRowId = this.QuoteToRowId;
            quote.siebelRBU = this.SiebelRBU;
            quote.quoteDate = this.QuoteDate;
            quote.expiredDate = TimeHelper.ConvertToSystemTime(this.ExpiredDate, this.Region);
            quote.createdBy = this.CreatedBy;
            quote.createdDate = this.CreatedDate;
            quote.LastUpdatedBy = this.LastUpdatedBy;
            quote.LastUpdatedDate = this.LastUpdatedDate;
            quote.deliveryDate = this.DeliveryDate;
            quote.reqDate = this.ReqDate;
            quote.DOCSTATUS = this.DOCSTATUS;
            quote.PODate = this.PoDate;
            quote.Revision_Number = this.RevisionNumber;
            quote.Active = this.Active = true;
            quote.tax = this.Tax;
            quote.isRepeatedOrder = this.IsRepeatedOrder;
            quote.PO_NO = string.IsNullOrEmpty(this.PoNo) ? "" : this.PoNo.Trim().Replace("'", "''");
            quote.DIST_CHAN = this.DistChan;
            quote.DIVISION = this.Division;
            quote.currency = this.Currency;
            quote.org = this.Org;
            quote.SALESOFFICE = this.SalesOffice;
            quote.SALESGROUP = this.SalesGroup;
            quote.customId = string.IsNullOrEmpty(this.Description) ? "" : this.Description.Trim().Replace("'", "''"); 
            quote.salesRowId = string.IsNullOrEmpty(this.SalesRowId) ? "" : this.SalesRowId.Trim().Replace("'", "''");
            quote.salesEmail = this.InSideSales;
            quote.quoteToName = string.IsNullOrEmpty(this.QuoteToName) ? "" : this.QuoteToName.Trim().Replace("'", "''"); 
            quote.relatedInfo = string.IsNullOrEmpty(this.ApprovalReason) ? "" : this.ApprovalReason.Trim().Replace("'", "''");
            quote.quoteNote = string.IsNullOrEmpty(this.QuoteNote) ? "" : this.QuoteNote.Trim().Replace("'", "''");
            //quote.QuotationExtensionNew.Warranty = string.IsNullOrEmpty(this.Warranty) ? "" : this.Warranty.Trim().Replace("'", "''");
            quote.freight = this.Freight;
            quote.INCO1 = this.Inco1;
            quote.INCO2 = this.Inco2;
            quote.OriginalQuoteID = this.OriginalQuoteID;
            quote.paymentTerm = this.PaymentTerm;
            //quote.INCO1 = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyByID(this.QuoteToErpId).FirstOrDefault();
            quote.Remark =  string.IsNullOrEmpty(this.Remark) ? "" : this.Remark.Trim().Replace("'", "''");

            var quoteExten = new QuotationExtensionNew();
            quoteExten.QuoteID = this.QuoteId;
            quoteExten.GeneralRate = 0.24m;
            quoteExten.Warranty = this.Warranty;
            quoteExten.BelowGP = false;
            quote.QuotationExtensionNew = quoteExten;

            var quotePartners = new List<EQPARTNER>();
            if (this.SoldToPartner != null)
                quotePartners.Add(this.SoldToPartner);
            if (this.ShipToPartner != null)
                quotePartners.Add(this.ShipToPartner);
            if (this.BillToPartner != null)
                quotePartners.Add(this.BillToPartner);
            if (this.EndCustomer != null && !string.IsNullOrEmpty(this.EndCustomer.ERPID) ) //Alex 20180712: For ACN, only endcutomer with epr id will be added to eqparters. 
                quotePartners.Add(this.EndCustomer);

            foreach (var item in this.ExtraTermsAndConditionOptions)
            {
                EQPARTNER e = new EQPARTNER();
                if (item.Selected == true)
                {
                    e.QUOTEID = this.QuoteId;
                    e.NAME = item.Name;
                    e.TYPE = item.Type;
                    quotePartners.Add(e);
                }
            }

            if (this.SalesRepresentatives != null)
            {
                int count = 1;
                foreach (var sales in this.SalesRepresentatives)
                {

                    EQPARTNER partner_sales = new EQPARTNER();
                    partner_sales.QUOTEID = this.QuoteId;
                    partner_sales.ERPID = sales.SalesCode;
                    partner_sales.NAME = sales.Email;
                    if (count == 1)
                        partner_sales.TYPE = "E";
                    if (count == 2)
                        partner_sales.TYPE = "E2";
                    if (count == 3)
                        partner_sales.TYPE = "E3";

                    quotePartners.Add(partner_sales);
                    count += 1;
                }
            }
            quote.QuotationPartner = quotePartners;
            quote.QuotationDetail = this.QuoteItems
                                  .Select(x => new QuotationDetail()
                                  {
                                      quoteId = x.QuoteId,
                                      line_No = x.LineNo,
                                      partNo = x.PartNo,
                                      description = x.Description,
                                      listPrice = x.ListPrice,
                                      unitPrice = x.UnitPrice,
                                      newUnitPrice = x.QuotingPrice,
                                      itp = x.Itp,
                                      newItp = x.NewItp,
                                      qty = x.Qty,
                                      sprNo = x.SprNo,
                                      HigherLevel = x.HigherLevel,
                                      DMF_Flag = x.DMF_Flag,
                                      ItemType = x.ItemType,
                                      deliveryPlant = x.DeliveryPlant,
                                      NCNR = Convert.ToInt32(x.NCNR),
                                      RecyclingFee = x.RecyclingFee,
                                      category =  string.IsNullOrEmpty(x.Category) ? "" : x.Category.Trim().Replace("'", "''"),
                                      classABC =  string.IsNullOrEmpty(x.ClassABC) ? "" : x.ClassABC.Trim().Replace("'", "''"),
                                      rohs = x.Rohs,
                                      ewFlag = x.EWFlag,
                                      reqDate = x.RequiredDate,
                                      dueDate = x.DueDate,
                                      satisfyFlag = x.SatisfyFlag,
                                      canBeConfirmed = x.CanBeConfirmed,
                                      custMaterial = x.CusMaterial==null?"": x.CusMaterial,
                                      inventory = x.Inventory,
                                      modelNo = string.IsNullOrEmpty(x.ModelNo) ? "" : x.ModelNo.Trim().Replace("'", "''"),
                                      VirtualPartNo = x.VirtualPartNo,
                                      RECFIGID = string.IsNullOrEmpty(x.Recfigid) ? "" : x.Recfigid.Trim().Replace("'", "''"),
                                      SequenceNo = x.SequenceNo,
                                      OriginalSalesDiscountRate = x.SalesDiscountRate,                                     
                                  })
                                  .ToList();

            if (!string.IsNullOrEmpty(this.OptyId))
            {
                optyQuote oppty = new optyQuote();
                oppty.optyId = this.OptyId;
                oppty.quoteId = this.QuoteId;
                oppty.optyName = this.OptyName ?? "";
                oppty.optyStage = this.OptyStage ?? "";
                oppty.Opty_Owner_Email = AppContext.UserEmail ?? "";
                quote.QuotationOpty = new optyQuote();
                quote.QuotationOpty = oppty;
            }

            if (this.QuoteApproval.QuoteApprovalList!=null)
                quote.WaitingApprovals = this.QuoteApproval.QuoteApprovalList.ToList();
            return quote;
        }

        public void SetQuoteItems(List<QuotationDetail> quoteDetails)
        {
            this.QuoteItems.Clear();

            this.QuoteItems = quoteDetails
                                  .Select(x => new QuoteItemViewModel()
                                  {
                                      QuoteId = x.quoteId,
                                      LineNo = x.line_No.Value,
                                      PartNo = x.partNo,
                                      Description = x.description,
                                      ListPrice = x.listPrice.Value,
                                      UnitPrice = x.unitPrice.Value,
                                      QuotingPrice = x.newUnitPrice.Value,
                                      Itp = x.itp.Value,
                                      NewItp = x.newItp.Value,
                                      Qty = x.qty.Value,
                                      CurrentQty = x.qty.Value,
                                      SprNo = x.sprNo,
                                      HigherLevel = x.HigherLevel.Value,
                                      DMF_Flag = x.DMF_Flag,
                                      ItemType = x.ItemType.Value,
                                      DeliveryPlant = x.deliveryPlant,
                                      NCNR = Convert.ToBoolean(x.NCNR.GetValueOrDefault(0)),
                                      RecyclingFee = x.RecyclingFee.GetValueOrDefault(0),
                                      Category = x.category,
                                      ClassABC = x.classABC,
                                      Rohs = x.rohs.GetValueOrDefault(1),
                                      EWFlag = x.ewFlag.GetValueOrDefault(0),
                                      RequiredDate = x.reqDate.GetValueOrDefault(DateTime.Now.AddDays(5)),
                                      DueDate = x.dueDate.GetValueOrDefault(DateTime.Now.AddDays(5)),
                                      SatisfyFlag = x.satisfyFlag.GetValueOrDefault(0),
                                      CanBeConfirmed = x.canBeConfirmed.GetValueOrDefault(1),
                                      CusMaterial = x.custMaterial,
                                      Inventory = x.inventory.GetValueOrDefault(0),
                                      ModelNo = x.modelNo,
                                      VirtualPartNo = x.VirtualPartNo,
                                      Recfigid = x.RECFIGID,
                                      SequenceNo = x.SequenceNo.GetValueOrDefault(0),
                                      IsEWPart = x.IsEWpartnoX,
                                      EWDrpIsEditable = (!x.IsEWpartnoX && x.ItemType != (int)LineItemType.BTOSChild && x.IsServicePartX == false && x.IsSoftwarePart == false),
                                      PostTaxListPrice = x.PostTaxListPrice,
                                      PostTaxUnitPrice = x.PostTaxUnitPrice,
                                      PostTaxQuotingPrice = x.PostTaxNewUnitPrice,
                                      PostTaxBtoParentListPrice = quoteDetails.Where(d => d.quoteId.Equals(x.quoteId) && d.HigherLevel == x.line_No).Sum(d => d.PostTaxListPrice),
                                      PostTaxBtoParentUnitPrice = quoteDetails.Where(d => d.quoteId.Equals(x.quoteId) && d.HigherLevel == x.line_No).Sum(d => d.PostTaxUnitPrice),
                                      PostTaxBtoParentQuotingPrice = quoteDetails.Where(d => d.quoteId.Equals(x.quoteId) && d.HigherLevel == x.line_No).Sum(d => d.PostTaxNewUnitPrice),
                                      PostTaxBtoParentSubTotal = quoteDetails.Where(d => d.quoteId.Equals(x.quoteId) && d.HigherLevel == x.line_No).Sum(d => d.PostTaxSubTotal)
                                  })
                                  .ToList();

            //Extend warranty options    
            var ExWarrantyList = MyAdvantechDAL.GetExtendedWarrantyByOrg(this.Org);
            foreach (var ewItem in ExWarrantyList)
            {
                foreach (var quoteItem in this.QuoteItems)
                {
                    quoteItem.EWPartOptions.Add(new SelectListItem()
                    {
                        Text = ewItem.EW_PartNO,
                        Value = ewItem.ID.ToString(),
                        Selected = ewItem.ID == quoteItem.EWFlag
                    });
                }
            }

            //Prepare parent items
            foreach (var item in this.QuoteItems)
            {
                if (item.PartNo.IndexOf("-BTO", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    this.ParentItemOptions.Add(new SelectListItem()
                    {
                        Text = item.PartNo,
                        Value = item.LineNo.ToString(),
                        Selected = item.PartNo == this.currentPartentOption
                    });
                }
            }

        }
    }

}