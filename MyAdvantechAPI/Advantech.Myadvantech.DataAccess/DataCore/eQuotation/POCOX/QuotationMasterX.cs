using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class QuotationMaster
    {

        public QuotationExtension QuoteExtensionX
        {
            get
            {
                QuotationExtension _QuotationExtension = eQuotationContext.Current.QuotationExtension.FirstOrDefault(p => p.QuoteID == this.quoteId);
                if (_QuotationExtension != null) return _QuotationExtension;
                return null;
            }
        }

        private IEnumerable<Quotation_Approval_Expiration> _QuotationApprovalExpiration;
        public IEnumerable<Quotation_Approval_Expiration> QuotationApprovalExpiration
        {
            get
            {
                if (_QuotationApprovalExpiration != null)
                { return _QuotationApprovalExpiration; }
                else
                {
                    eQuotationDAL _eQuotationDAL = new eQuotationDAL();
                    IEnumerable<Quotation_Approval_Expiration> _QAEList = _eQuotationDAL.getQuotationApprovalExpiration(this.quoteId);
                    if (_QAEList != null)
                    {
                        _QuotationApprovalExpiration = _QAEList;
                        return _QAEList;
                    }
                    return null;
                }

            }
            set { _QuotationApprovalExpiration = value; }
        }

        private List<QuotationDetail> _QuotationDetail;
        public List<QuotationDetail> QuotationDetail
        {
            get
            {
                if (_QuotationDetail == null)
                {
                    this._QuotationDetail = (new QuotationDetailHelper()).GetQuotationDetail(this.quoteId);
                    InitializeQuotationDetail();
                }
                return this._QuotationDetail;
            }
            set
            {
                this._QuotationDetail = value;
                InitializeQuotationDetail();
            }
        }

        private List<QuotationDetail_Extension_ABR> _QuotationDetail_Extension_ABR;
        public List<QuotationDetail_Extension_ABR> QuotationDetail_Extension_ABR
        {
            get
            {
                if (_QuotationDetail_Extension_ABR == null)
                {
                    this._QuotationDetail_Extension_ABR = (new QuotationDetailHelper()).GetQuotationDetail_Extension_ABR(this.quoteId);
                    InitializeQuotationDetail();
                }
                return this._QuotationDetail_Extension_ABR;
            }
            set
            {
                this._QuotationDetail_Extension_ABR = value;
                InitializeQuotationDetail();
            }
        }



        private QuotationExtensionNew _QuotationExtensionNew;
        public QuotationExtensionNew QuotationExtensionNew
        {
            get
            {
                if (_QuotationExtensionNew == null)
                {
                    this._QuotationExtensionNew = eQuotationContext.Current.QuotationExtensionNew.Where(x => x.QuoteID == this.quoteId).FirstOrDefault();
                    if (_QuotationExtensionNew == null)
                    {
                        QuotationExtensionNew EQQuote = new QuotationExtensionNew();
                        EQQuote.QuoteID = this.quoteId;
                        EQQuote.GeneralRate = 0.24m;
                        this._QuotationExtensionNew = EQQuote;
                    }
                }
                return this._QuotationExtensionNew;
            }
            set
            {
                this._QuotationExtensionNew = value;
            }
        }

        private optyQuote _QuotationOpty;
        public optyQuote QuotationOpty
        {
            get
            {
                if (_QuotationOpty == null)
                {
                    this._QuotationOpty = (new OptyQuoteHelper()).GetOptyQuote(this.quoteId);
                }
                return this._QuotationOpty;
            }
            set
            {
                this._QuotationOpty = value;
                InitializeQuotationDetail();
            }
        }

        private List<SiebelActive> _SiebelActive;
        public List<SiebelActive> SiebelActive
        {
            get
            {
                if (_SiebelActive == null)
                {
                    _SiebelActive = new List<DataAccess.SiebelActive>();
                }
                return this._SiebelActive;
            }
            set
            {
                this._SiebelActive = value;
            }
        }

        public int Revenue
        {
            get
            {
                if (QuotationDetail != null && QuotationDetail.Any())
                {
                    return Convert.ToInt32(QuotationDetail.Sum(p => p.newUnitPrice * p.qty));
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Return if the quote has configuration system
        /// </summary>
        public Boolean IsConfigSystemQuote
        {
            get
            {
                if (QuotationDetail != null && QuotationDetail.Any())
                {
                    if (QuotationDetail.Where(p => p.line_No >= 100).ToList().Count() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Decimal GetABRAllTaxTotalAmount
        {
            get
            {
                Decimal _AllTaxTotalAmount = 0;
                _AllTaxTotalAmount += GetABRTaxTotalAmount(ABRTaxType.BX13);
                _AllTaxTotalAmount += GetABRTaxTotalAmount(ABRTaxType.BX23);
                _AllTaxTotalAmount += GetABRTaxTotalAmount(ABRTaxType.BX41);
                _AllTaxTotalAmount += GetABRTaxTotalAmount(ABRTaxType.BX72);
                _AllTaxTotalAmount += GetABRTaxTotalAmount(ABRTaxType.BX82);
                _AllTaxTotalAmount += GetABRTaxTotalAmount(ABRTaxType.BX94);
                _AllTaxTotalAmount += GetABRTaxTotalAmount(ABRTaxType.BX95);
                _AllTaxTotalAmount += GetABRTaxTotalAmount(ABRTaxType.BX96);
                return _AllTaxTotalAmount;
            }
        }

        private decimal GetABRTaxTotalAmount(ABRTaxType taxtype)
        {
            Decimal _totalTaxAmount = 0;
            if (QuotationDetail != null && QuotationDetail.Any())
            {
                Decimal _itemTaxAmount = 0;

                foreach (QuotationDetail _QuoteDetailItem in QuotationDetail)
                {
                    _itemTaxAmount = 0;
                    QuotationDetail_Extension_ABR _QuoteDetail_ext_abr_item = QuotationDetail_Extension_ABR.Where(x => x.quoteid == _QuoteDetailItem.quoteId && x.line_No == _QuoteDetailItem.line_No).FirstOrDefault();

                    if (_QuoteDetail_ext_abr_item != null)
                    {
                        switch (taxtype)
                        {
                            case ABRTaxType.BX10:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX13.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX10.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX13:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX13.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX13.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX23:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX23.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX23.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX40:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX41.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX40.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX41:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX41.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX41.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX72:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX72.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX72.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX82:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX82.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX82.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX94:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX82.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX94.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX95:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX82.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX95.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.BX96:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX82.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.BX96.GetValueOrDefault(0);
                                break;
                            case ABRTaxType.FK00:
                                //_itemTaxAmount = _QuoteDetailItem.qty.GetValueOrDefault(0) * _QuoteDetail_ext_abr_item.BX82.GetValueOrDefault(0);
                                _itemTaxAmount = _QuoteDetail_ext_abr_item.FK00.GetValueOrDefault(0);
                                break;
                            default:
                                _itemTaxAmount = 0;
                                break;
                        }
                    }
                    _totalTaxAmount += _itemTaxAmount;
                }

            }
            return _totalTaxAmount;
        }

        public Decimal GetABRTotalBX13
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX13);
            }
        }
        public Decimal GetABRTotalBX94
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX94);
            }
        }
        public Decimal GetABRTotalBX95
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX95);
            }
        }
        public Decimal GetABRTotalBX96
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX96);
            }
        }


        public Decimal GetABRTotalBX23
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX23);
            }
        }
        public Decimal GetABRTotalBX41
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX41);
            }
        }

        public Decimal GetABRTotalBX10
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX10);
            }
        }
        public Decimal GetABRTotalBX40
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX40);
            }
        }
        public Decimal GetABRTotalFK00
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.FK00);
            }
        }



        public Decimal GetABRTotalBX72
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX72);
            }
        }
        public Decimal GetABRTotalBX82
        {
            get
            {
                return GetABRTaxTotalAmount(ABRTaxType.BX82);
            }
        }


        /// <summary>
        /// Get all parent line items of quote
        /// </summary>
        public List<QuotationDetail> GetParentQuoteItems
        {
            get
            {
                if (QuotationDetail != null && QuotationDetail.Any())
                {
                    var aaa = (from p in QuotationDetail
                               select p.HigherLevel).Distinct();

                    var parentitems = from p in QuotationDetail
                                      where aaa.Contains(p.line_No)
                                      select p;

                    if (parentitems.Count<QuotationDetail>() > 0)
                    {
                        return (List<QuotationDetail>)parentitems;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Get Quotation Detail into Quotation Partner
        /// </summary>
        private List<EQPARTNER> _QuotationPartner;
        public List<EQPARTNER> QuotationPartner
        {
            get
            {
                if (_QuotationPartner == null)
                    this._QuotationPartner = (new QuotationPartnerHelper()).GetQuotationPartner(this.quoteId);
                return this._QuotationPartner;
            }
            set
            {
                _QuotationPartner = value;
            }
        }

        /// <summary>
        /// Get attention user's first name from Siebel
        /// </summary>
        public string AttentionFirstName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.attentionRowId))
                {
                    object firstName = SqlProvider.dbExecuteScalar("CRM", string.Format(" select top 1 IsNull(FST_NAME, '') from S_CONTACT where ROW_ID = '{0}' ", this.attentionRowId));
                    if (firstName != null)
                        return firstName.ToString();
                }
                return string.Empty;
            }
        }

        //Get attention user's last name from Siebel
        public string AttentionLasteName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.attentionRowId))
                {
                    object lastName = SqlProvider.dbExecuteScalar("CRM", string.Format(" select top 1 IsNull(LAST_NAME, '') from S_CONTACT where ROW_ID = '{0}' ", this.attentionRowId));
                    if (lastName != null)
                        return lastName.ToString();
                }
                return string.Empty;
            }
        }



        //Quotation Remark
        private string _Remark;
        public string Remark {
            get
            {
                if (_Remark == null)
                {
                    object remark = SqlProvider.dbExecuteScalar("EQ", string.Format(" select top 1 notetext from QuotationNote where quoteid = '{0}' and notetype = 'Remark' ", this.quoteId));
                    if (remark != null)
                        _Remark = remark.ToString();
                }
                return _Remark;
            }
            set {
                _Remark = value;
            }
        }


        /// <summary>
        /// Get sales's position. By sales email in SIEBEL_SALES_HIERARCHY.
        /// </summary>
        public string Position
        {
            get
            {
                //ICC 2015/11/12 Use createdBy as emaill address directly.
                string _email = this.createdBy;
                //string _email = this.salesEmail;
                //if (string.IsNullOrEmpty(_email))
                //{
                //    _email = this.createdBy;
                //}

                object position = MyAdvantechDAL.GetSiebelSalesPositionByEmail(_email);
                return position != null ? position.ToString() : string.Empty;
            }
        }

        public String Plant
        {
            get
            {
                return SAPDAL.GetPlantByOrg(this.org); ;
            }
        }

        private String _DefaultERPID;
        public String DefaultERPID
        {
            get
            {
                if (String.IsNullOrEmpty(_DefaultERPID))
                {
                    if (String.IsNullOrEmpty(this.quoteToErpId))
                        _DefaultERPID = SAPDAL.GetDefaultERPIDforSimulation(this.org, this.currency);
                    else
                        _DefaultERPID = this.quoteToErpId;
                }
                return _DefaultERPID;
            }
        }

        private String _DefaultCurrency;
        public String DefaultCurrency
        {
            get
            {
                if (String.IsNullOrEmpty(this.currency))
                    _DefaultCurrency = MyAdvantechDAL.GetSAPDIMCompanyByERPID(this.DefaultERPID).FirstOrDefault().CURRENCY;
                else
                    _DefaultCurrency = this.currency;

                return _DefaultCurrency;
            }
        }

        public Decimal PreTaxTotalAmount
        {
            get
            {
                Decimal result = 0;
                this.QuotationDetail.ForEach(d => result += d.SubTotal);
                return result;
            }
        }

        public Decimal TotalTaxAmount
        {
            get
            {
                return this.PostTaxTotalAmount - this.PreTaxTotalAmount;
            }
        }

        public Decimal PostTaxTotalAmount
        {
            get
            {
                //return (Decimal)(this.PreTaxTotalAmount * (1 + this.tax));

                Decimal result = 0;
                this.QuotationDetail.ForEach(d => result += d.PostTaxSubTotal);
                return result;
            }
        }

        public Decimal ITPTotalAmount
        {
            get
            {
                Decimal result = 0;
                this.QuotationDetail.ForEach(d => result += d.ITPSubTotal);
                return result;
            }
        }

        public Decimal TotalGeneralAmount
        {
            get
            {
                return this.ITPTotalAmountWithGeneralAmount - this.ITPTotalAmount;
            }
        }

        public Decimal ITPTotalAmountWithGeneralAmount
        {
            get
            {
                return (Decimal)(this.ITPTotalAmount * (1 + this.QuotationExtensionNew.GeneralRate));
            }
        }

        private Decimal _CostTax = 0;
        public Decimal CostTax
        {
            get { return _CostTax; }
            set { _CostTax = value; }
        }

        public string CurrencySign
        {
            get
            {
                return Advantech.Myadvantech.DataAccess.MyExtension.GetCurrencySignByCurrency(this.currency);
            }
            
        }
        public int GetNextLooseItemLineNo()
        {
            if (this.QuotationDetail == null || this.QuotationDetail.Count == 0 || !this.QuotationDetail.Where(d => d.line_No < 100).Any())
                return 1;
            else
                return (int)this.QuotationDetail.Where(d => d.line_No < 100).OrderByDescending(d => d.line_No).FirstOrDefault().line_No + 1;
        }

        public int GetNextBTOSParentLineNo()
        {
            if (this.QuotationDetail == null || this.QuotationDetail.Count == 0 || !IsConfigSystemQuote)
                return 100;
            else
                return (int)this.QuotationDetail.Where(d => d.line_No % 100 == 0).OrderByDescending(d => d.line_No).FirstOrDefault().line_No + 100;
        }

        public int GetNextBTOSItemLineNo(int _ParentLineNo)
        {
            if (!IsConfigSystemQuote)
                return 0;
            else
            {
                if (this.QuotationDetail.Where(d => d.HigherLevel == _ParentLineNo).Any())
                    return (int)this.QuotationDetail.Where(d => d.HigherLevel == _ParentLineNo).OrderByDescending(d => d.line_No).FirstOrDefault().line_No + 1;
                else
                    return _ParentLineNo + 1;
            }
        }

        public Order ConvertQuoteToOrder(string simulateOrg = "", string simulateERPId = "")
        {
            Order order = new Order();
            String errMsg = String.Empty;

            // Set Main Data            
            order.Currency = this.DefaultCurrency;
            order.DistChannel = this.DIST_CHAN;
            order.Division = this.DIVISION;
            order.OrderType = SAPOrderType.ZOR;

            // Set Parts
            if (this.QuotationDetail != null && this.QuotationDetail.Count > 0)
            {
                foreach (QuotationDetail q in this.QuotationDetail)
                {
                    switch (q.ItemType)
                    {
                        case -1:
                            order.AddBTOSParentItem(q.partNo, q.deliveryPlant, (int)q.line_No, (int)q.qty);
                            break;
                        case 0:
                            order.AddLooseItem(q.partNo, q.deliveryPlant, (int)q.line_No, (int)q.qty);
                            break;
                        case 1:
                            order.AddBTOSChildItem(q.partNo, (int)q.HigherLevel, q.deliveryPlant, (int)q.line_No, (int)q.qty);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (this.QuotationDetail == null || this.QuotationDetail.Count == 0)
                return order;

            // Set Partners
            if (!string.IsNullOrEmpty(simulateERPId) && !string.IsNullOrEmpty(simulateOrg))
            {
                order.OrgID = simulateOrg;
                order.SetOrderPartnet(new OrderPartner(simulateERPId, simulateOrg, OrderPartnerType.SoldTo));
                order.SetOrderPartnet(new OrderPartner(simulateERPId, simulateOrg, OrderPartnerType.ShipTo));
                order.SetOrderPartnet(new OrderPartner(simulateERPId, simulateOrg, OrderPartnerType.BillTo));
            }
            else
            {
                order.OrgID = this.org;
                if (this.QuotationPartner != null && this.QuotationPartner.Count > 0
                    && this.QuotationPartner.Where(d => d.TYPE.Equals("SOLDTO") && !String.IsNullOrEmpty(d.ERPID)).Any())
                {
                    foreach (EQPARTNER e in this.QuotationPartner)
                    {
                        switch (e.TYPE)
                        {
                            case "SOLDTO":
                                order.SetOrderPartnet(new OrderPartner(e.ERPID, this.org, OrderPartnerType.SoldTo));
                                break;
                            case "S":
                                order.SetOrderPartnet(new OrderPartner(e.ERPID, this.org, OrderPartnerType.ShipTo));
                                break;
                            case "B":
                                order.SetOrderPartnet(new OrderPartner(e.ERPID, this.org, OrderPartnerType.BillTo));
                                break;
                        }
                    }
                }
                else
                {
                    order.SetOrderPartnet(new OrderPartner(this.DefaultERPID, this.org, OrderPartnerType.SoldTo));
                    order.SetOrderPartnet(new OrderPartner(this.DefaultERPID, this.org, OrderPartnerType.ShipTo));
                    order.SetOrderPartnet(new OrderPartner(this.DefaultERPID, this.org, OrderPartnerType.BillTo));
                }
            }
            return order;
        }

        public void SimulateITPByOrgAndERPId(string simulateOrg, string simulateERPId, ref string errMsg)
        {
            if (this.QuotationDetail != null && this.QuotationDetail.Count > 0)
            {
                Order order = new Order();
                try
                {
                    order = this.ConvertQuoteToOrder(simulateOrg, simulateERPId);

                    Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }


                if (String.IsNullOrEmpty(errMsg))
                {
                    foreach (Product p in order.LineItems)
                    {
                        if (p.UnitPrice > 0)
                        {
                            this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                                            d.line_No == p.LineNumber)
                                                .ToList()
                                                .ForEach(c =>
                                                {
                                                    c.itp = p.UnitPrice;
                                                    c.newItp = p.UnitPrice;
                                                });
                        }
                    }
                    //this.InitializeQuotationDetail();
                }
            }
        }

        public void SimulateITPBySAPGPBlockRFC(ref string errMsg)
        {

            if (this.QuotationDetail != null && this.QuotationDetail.Count > 0)
            {
                Order order = new Order();
                try
                {
                    order = this.ConvertQuoteToOrder();
                    //order.OrderType = SAPOrderType.ZOR; 
                    Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrderGPBlock(ref order, ref errMsg);
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }


                if (String.IsNullOrEmpty(errMsg))
                {
                    foreach (Product p in order.LineItems)
                    {
                        this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                                        d.line_No == p.LineNumber)
                                            .ToList()
                                            .ForEach(c =>
                                            {
                                                c.itp = p.ITP ;
                                                c.newItp = p.ITP;
                                            });
                        
                    }
                }
            }

        }

        /// <summary>
        /// Simulate PCP Price by specific Org and ERPID 
        /// </summary>
        /// <param name="simulateOrg"></param>
        /// <param name="simulateERPId"></param>.
        /// <param name="errMsg"></param>
        public void SimulatePCPPrice(string simulateOrg, string simulateERPId, ref string errMsg)
        {
            Order order = this.ConvertQuoteToOrder(simulateOrg, simulateERPId);

            Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
            if (String.IsNullOrEmpty(errMsg))
            {
                foreach (Product p in order.LineItems)
                {
                    // Write price back to quotationdetail's ACN PCPPrice
                    this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                                    d.line_No == p.LineNumber)
                                        .ToList()
                                        .ForEach(c =>
                                        {
                                            c.ACN_PCPPrice = p.UnitPrice;
                                        });
                }
            }
        }

        public void SimulatePrice(ref string finalErrMsg)
        {
            Order order = new Order();
            String errMsg = String.Empty;

            // Set Main Data            
            order.Currency = this.DefaultCurrency;
            order.DistChannel = this.DIST_CHAN;
            order.Division = this.DIVISION;
            order.OrgID = this.org;
            order.OrderType = SAPOrderType.ZOR;

            // Set Parts
            if (this.QuotationDetail != null && this.QuotationDetail.Count > 0)
            {
                foreach (QuotationDetail q in this.QuotationDetail)
                {
                    switch (q.ItemType)
                    {
                        case -1:
                            order.AddBTOSParentItem(q.partNo, q.deliveryPlant, (int)q.line_No, (int)q.qty);
                            break;
                        case 0:
                            order.AddLooseItem(q.partNo, q.deliveryPlant, (int)q.line_No, (int)q.qty);
                            break;
                        case 1:
                            order.AddBTOSChildItem(q.partNo, (int)q.HigherLevel, q.deliveryPlant, (int)q.line_No, (int)q.qty);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (this.QuotationDetail == null || this.QuotationDetail.Count == 0)
                return;

            // Set Partners
            if (this.QuotationPartner != null && this.QuotationPartner.Count > 0
                && this.QuotationPartner.Where(d => d.TYPE.Equals("SOLDTO") && !String.IsNullOrEmpty(d.ERPID)).Any())
            {
                foreach (EQPARTNER e in this.QuotationPartner)
                {
                    switch (e.TYPE)
                    {
                        case "SOLDTO":
                            order.SetOrderPartnet(new OrderPartner(e.ERPID, this.org, OrderPartnerType.SoldTo));
                            break;
                        case "S":
                            order.SetOrderPartnet(new OrderPartner(e.ERPID, this.org, OrderPartnerType.ShipTo));
                            break;
                        case "B":
                            order.SetOrderPartnet(new OrderPartner(e.ERPID, this.org, OrderPartnerType.BillTo));
                            break;
                    }
                }
            }
            else
            {
                order.SetOrderPartnet(new OrderPartner(this.DefaultERPID, this.org, OrderPartnerType.SoldTo));
                order.SetOrderPartnet(new OrderPartner(this.DefaultERPID, this.org, OrderPartnerType.ShipTo));
                order.SetOrderPartnet(new OrderPartner(this.DefaultERPID, this.org, OrderPartnerType.BillTo));
            }

            // Do Simulation
            Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);

            // For ACN, using default ERP ID if customer is overall block.
            if (this.org.ToUpper().StartsWith("CN") && !String.IsNullOrEmpty(errMsg) && errMsg.Contains("Overall block"))
            {

                var defaultERPId = SAPDAL.GetDefaultERPIDforSimulation(this.org, this.currency);
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.SoldTo));
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.ShipTo));
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.BillTo));
                errMsg = "";
                Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
            }


            // Process Simulate Result
            if (String.IsNullOrEmpty(errMsg))
            {
                foreach (Product p in order.LineItems)
                {
                    // Write price back to quotationdetail's unitprice and ITP
                    this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                                    d.unitPrice == d.newUnitPrice &&
                                                    d.line_No == p.LineNumber)
                                        .ToList()
                                        .ForEach(c =>
                                        {
                                            c.listPrice = p.ListPrice;
                                            c.unitPrice = p.UnitPrice;
                                            c.newUnitPrice = p.UnitPrice;
                                            c.itp = p.Conditions.VPRS;
                                            c.newItp = p.Conditions.VPRS;
                                        });

                    this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                d.unitPrice != d.newUnitPrice &&
                                d.line_No == p.LineNumber)
                    .ToList()
                    .ForEach(c =>
                    {
                        c.listPrice = p.ListPrice;
                        c.unitPrice = p.UnitPrice;
                        c.itp = p.Conditions.VPRS;
                        c.newItp = p.Conditions.VPRS;
                    });
                }
                InitializeQuotationDetail();
            }
            else
            {
                // Simulate with SAP failed
                finalErrMsg = errMsg;


            }

            // Extra Process For Each Org
            switch (this.org.ToUpper())
            {
                case "CN10":
                case "CN30":

                    // 20170905 Alex add simulate PCP (C200611) Price for CN quote
                    order.SetOrderPartnet(new OrderPartner("C200611", this.org, OrderPartnerType.SoldTo));
                    order.SetOrderPartnet(new OrderPartner("C200611", this.org, OrderPartnerType.ShipTo));
                    order.SetOrderPartnet(new OrderPartner("C200611", this.org, OrderPartnerType.BillTo));
                    Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
                    if (String.IsNullOrEmpty(errMsg))
                    {
                        foreach (Product p in order.LineItems)
                        {
                            // Write price back to quotationdetail's ACN PCPPrice
                            this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                                            //d.unitPrice == d.newUnitPrice &&
                                                            d.line_No == p.LineNumber)
                                                .ToList()
                                                .ForEach(c =>
                                                {
                                                    c.ACN_PCPPrice = p.UnitPrice;
                                                });
                        }
                    }
                    else
                    {
                        // Simulate with SAP failed
                    }

                    // Get ACN ITP with ADVACN/TW01
                    order.OrgID = "TW01";
                    order.SetOrderPartnet(new OrderPartner("ADVACN", "TW01", OrderPartnerType.SoldTo));
                    order.SetOrderPartnet(new OrderPartner("ADVACN", "TW01", OrderPartnerType.ShipTo));
                    order.SetOrderPartnet(new OrderPartner("ADVACN", "TW01", OrderPartnerType.BillTo));
                    Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
                    if (String.IsNullOrEmpty(errMsg))
                    {
                        foreach (Product p in order.LineItems)
                        {
                            if (p.UnitPrice > 0)
                            {
                                this.QuotationDetail.Where(d => !d.IsPTD &&
                                                                d.partNo.Equals(p.PartNumber) &&
                                                                //d.unitPrice == d.newUnitPrice &&
                                                                d.line_No == p.LineNumber)
                                                    .ToList()
                                                    .ForEach(c =>
                                                    {
                                                        c.itp = p.UnitPrice;
                                                        c.newItp = p.UnitPrice;
                                                    });
                            }
                        }
                        InitializeQuotationDetail();
                    }
                    else
                    {
                        // Simulate with SAP failed
                    }

                    //get product bom cost for end2end gp
                    GetProductBOMCost("CNY", ref errMsg);
                    break;




                case "US10":

                    // Get BBUS ITP with ADVANA/US10
                    order.OrgID = "US10";
                    order.SetOrderPartnet(new OrderPartner("ADVANA", "US10", OrderPartnerType.SoldTo));
                    order.SetOrderPartnet(new OrderPartner("ADVANA", "US10", OrderPartnerType.ShipTo));
                    order.SetOrderPartnet(new OrderPartner("ADVANA", "US10", OrderPartnerType.BillTo));
                    Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
                    if (String.IsNullOrEmpty(errMsg))
                    {
                        foreach (Product p in order.LineItems)
                        {
                            if (p.UnitPrice > 0)
                            {
                                this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                                                //d.unitPrice == d.newUnitPrice &&
                                                                d.line_No == p.LineNumber)
                                                    .ToList()
                                                    .ForEach(c =>
                                                    {
                                                        c.itp = p.UnitPrice;
                                                        c.newItp = p.UnitPrice;
                                                    });
                            }
                        }
                        InitializeQuotationDetail();
                    }
                    else
                    {
                        // Simulate with SAP failed
                    }

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Simulate BOM Cost
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="errMsg"></param>
        public void GetProductBOMCost(string currency, ref string errMsg)
        {

            if (this.QuotationDetail != null && this.QuotationDetail.Count > 0)
            {

                //20180402 Simulate end2end GP for ACN // 寫法很怪,看看是否有更的方式
                var simulateQuoteDetail = this.QuotationDetail;
                Advantech.Myadvantech.DataAccess.SAPDAL.GetProductBOMCost(ref simulateQuoteDetail, currency, ref errMsg);
            }
        }

        /// <summary>
        /// Simulate List,Unit Price and VPRS
        /// </summary>
        /// <param name="errMsg"></param>
        public void SimulateListUnitPriceVPRS(ref string errMsg)
        {
            Order order = new Order();
            try
            {
                order = this.ConvertQuoteToOrder();

                Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
            }

            // For ACN, using default ERP ID if customer is overall block.
            if (this.org.ToUpper().StartsWith("CN") && !String.IsNullOrEmpty(errMsg) && errMsg.Contains("Overall block"))
            {

                var defaultERPId = SAPDAL.GetDefaultERPIDforSimulation(this.org, this.currency);
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.SoldTo));
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.ShipTo));
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.BillTo));
                errMsg = "";
                Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
            }

            // Process Simulate Result
            if (String.IsNullOrEmpty(errMsg))
            {
                foreach (Product p in order.LineItems)
                {
                    // Write price back to quotationdetail's unitprice and ITP
                    this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                                    d.unitPrice == d.newUnitPrice &&
                                                    d.line_No == p.LineNumber && d.IsEWpartnoX == false)//Ales 20180807 exclued EW Part
                                        .ToList()
                                        .ForEach(c =>
                                        {
                                            c.listPrice = p.ListPrice;
                                            c.unitPrice = p.UnitPrice;
                                            c.newUnitPrice = p.UnitPrice;
                                            c.itp = p.Conditions.VPRS;
                                            c.newItp = p.Conditions.VPRS;
                                        });

                    this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                d.unitPrice != d.newUnitPrice &&
                                d.line_No == p.LineNumber && d.IsEWpartnoX == false)//Alex 20180807 exclued EW Part
                    .ToList()
                    .ForEach(c =>
                    {
                        c.listPrice = p.ListPrice;
                        c.unitPrice = p.UnitPrice;
                        c.itp = p.Conditions.VPRS;
                        c.newItp = p.Conditions.VPRS;
                    });
                }

                //Alex 20180809 handle EW Parts:
                foreach(var ewItem in this.QuotationDetail.Where(d => d.IsEWpartnoX == true && d.unitPrice == d.newUnitPrice ))
                {
                    var quoteItemWithEX = this.QuotationDetail.Where(q => q.line_No == ewItem.HigherLevel && q.ewFlag!=null && q.ewFlag != 0).FirstOrDefault();
                    if(quoteItemWithEX == null)
                        quoteItemWithEX = this.QuotationDetail.Where(q => q.line_No == ewItem.line_No - 1 && q.ewFlag != null && q.ewFlag != 0).FirstOrDefault();
                    if(quoteItemWithEX !=null)
                        CalculateExtendWarrantyPriceByLineNo(quoteItemWithEX.line_No.Value);
                }

                this.InitializeQuotationDetail();
            }
        }



        /// <summary>
        /// Simulate List,Unit Price and VPRS
        /// </summary>
        /// <param name="errMsg"></param>
        public void SimulateListUnitPriceWithSameSalesDiscountRate(ref string errMsg)
        {
            Order order = new Order();
            try
            {
                order = this.ConvertQuoteToOrder();

                Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            // For ACN, using default ERP ID if customer is overall block.
            if (this.org.ToUpper().StartsWith("CN") && !String.IsNullOrEmpty(errMsg) && errMsg.Contains("Overall block"))
            {

                var defaultERPId = SAPDAL.GetDefaultERPIDforSimulation(this.org, this.currency);
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.SoldTo));
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.ShipTo));
                order.SetOrderPartnet(new OrderPartner(defaultERPId, this.org, OrderPartnerType.BillTo));
                errMsg = "";
                Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
            }

            // Process Simulate Result
            if (String.IsNullOrEmpty(errMsg))
            {
                foreach (Product p in order.LineItems)
                {
                    // Write price back to quotationdetail's unitprice and ITP
                    this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                                                    d.line_No == p.LineNumber)
                                        .ToList()
                                        .ForEach(c =>
                                        {
                                            c.listPrice = p.ListPrice;
                                            c.unitPrice = p.UnitPrice;
                                            c.newUnitPrice = p.UnitPrice - p.UnitPrice * c.OriginalSalesDiscountRate;
                                        });

                    //this.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
                    //            d.unitPrice != d.newUnitPrice &&
                    //            d.line_No == p.LineNumber)
                    //.ToList()
                    //.ForEach(c =>
                    //{
                    //    c.listPrice = p.ListPrice;
                    //    c.unitPrice = p.UnitPrice;
                    //});
                }
                this.InitializeQuotationDetail();
            }
        }

        /// <summary>
        /// Comment out by Ryan at 20180212 due to new function AddQuotationDetail is applied.
        /// Qty is set in class property instead of all items share a qty parameter.
        /// </summary>
        /// <param name="_items"></param>
        /// <param name="_Parent"></param>
        /// <param name="simulateErrorMsg"></param>
        //public void AddQuotationDetail_Old(List<String> _items, string _Parent, int _Qty, ref string simulateErrorMsg)
        //{
        //    if (_items.Count == 0)
        //        return;

        //    int BTOSLineNo = 0;
        //    List<QuotationDetail> tempQuotDetailList = this.QuotationDetail.ToList();



        //    foreach (String item in _items)
        //    {
        //        var _item = item;
        //        // For ACN, need to split BOTS root name with "/", ex: IPC-510-BTO/P4
        //        if (_item.Contains("-BTO/"))
        //        {
        //            _item = _item.Split(new string[] { "/" }, StringSplitOptions.None)[0];
        //        }
        //        SAP_PRODUCT p = MyAdvantechDAL.GetSAP_ProductByOrg(_item, this.org);
        //        QuotationDetail q = new QuotationDetail();
        //        q.quoteId = this.quoteId;
        //        if (_item.EndsWith("-BTO"))
        //        {
        //            BTOSLineNo = GetNextBTOSParentLineNo();
        //            q.line_No = BTOSLineNo;
        //            q.HigherLevel = 0;
        //            q.ItemType = (int)LineItemType.BTOSParent;
        //        }
        //        else
        //        {
        //            if (BTOSLineNo == 0)
        //            {
        //                if (string.IsNullOrEmpty(_Parent))
        //                {
        //                    q.line_No = GetNextLooseItemLineNo();
        //                    q.ItemType = (int)LineItemType.LooseItem;
        //                    q.HigherLevel = BTOSLineNo;
        //                }
        //                else
        //                {
        //                    q.line_No = GetNextBTOSItemLineNo(Convert.ToInt32(_Parent));
        //                    q.ItemType = (int)LineItemType.BTOSChild;
        //                    q.HigherLevel = Convert.ToInt32(_Parent);
        //                }
        //            }
        //            else
        //            {
        //                q.line_No = GetNextBTOSItemLineNo(BTOSLineNo);
        //                q.ItemType = (int)LineItemType.BTOSChild;
        //                q.HigherLevel = BTOSLineNo;
        //            }
        //        }
        //        q.partNo = _item;
        //        q.description = p.PRODUCT_DESC;
        //        q.qty = _Qty;
        //        q.listPrice = 0;
        //        q.unitPrice = 0;
        //        q.newUnitPrice = 0;
        //        q.itp = 0;
        //        q.newItp = 0;
        //        q.RecyclingFee = 0;
        //        q.deliveryPlant = this.Plant;
        //        q.category = "";
        //        q.classABC = "";
        //        q.rohs = 1;
        //        q.ewFlag = 0;
        //        q.reqDate = DateTime.Now.AddDays(5);
        //        q.dueDate = DateTime.Now.AddDays(5);
        //        q.VirtualPartNo = p.PART_NO;
        //        q.satisfyFlag = 0;
        //        q.canBeConfirmed = 1;
        //        q.custMaterial = "";
        //        q.inventory = 0;
        //        //q.oType = "";
        //        q.modelNo = "";
        //        q.RECFIGID = "";
        //        q.SequenceNo = 0;
        //        q.NCNR = Convert.ToInt32(SAPDAL.isNCNRPart(q.partNo,q.deliveryPlant));

        //        this.QuotationDetail.Add(q);
        //    }
        //    this.QuotationDetail = this.QuotationDetail.OrderBy(d => d.line_No).ToList();
        //    this.SimulatePrice(ref simulateErrorMsg);

        //    if (!string.IsNullOrEmpty(simulateErrorMsg))// if error occur, recover to original orderDetails
        //    {
        //        this.QuotationDetail = tempQuotDetailList.ToList();
        //    }
        //}

        public void AddQuotationDetail(List<DataCore.eQuotation.Model.JsonPart> _items, string _Parent, bool isReconfig)
        {
            if (_items.Count == 0)
                return;


            int BTOSLineNo = 0;

            // item validation
            List<DataCore.eQuotation.Model.JsonPart> toAddItems = new List<DataCore.eQuotation.Model.JsonPart>();            
            foreach (DataCore.eQuotation.Model.JsonPart p in _items)
            {
                if (p.Partno.Contains("|"))
                {                   
                    foreach (var i in p.Partno.Split('|').ToList())
                    {
                        DataCore.eQuotation.Model.JsonPart newitem = new DataCore.eQuotation.Model.JsonPart();
                        newitem.Partno = i;                        
                        newitem.Qty = p.Qty;
                        toAddItems.Add(newitem);
                    }
                }
                else if (p.Partno.Contains("-BTO/"))
                {
                    DataCore.eQuotation.Model.JsonPart newitem = new DataCore.eQuotation.Model.JsonPart();
                    newitem.Partno = p.Partno.Split(new string[] { "/" }, StringSplitOptions.None)[0];
                    newitem.Qty = p.Qty;
                    toAddItems.Add(newitem);
                }
                else
                    toAddItems.Add(p);
            }

            if (isReconfig)
            {
                this.QuotationDetail.RemoveAll(o => o.HigherLevel == Convert.ToInt32(_Parent));//刪除子項
                this.QuotationDetail.RemoveAll(y => y.line_No == Convert.ToInt32(_Parent));//刪除BTO母階
            }

            foreach (DataCore.eQuotation.Model.JsonPart item in toAddItems)
            {
                SAP_PRODUCT p = MyAdvantechDAL.GetSAP_ProductByOrg(item.Partno, this.org);
                QuotationDetail q = new QuotationDetail();
                q.quoteId = this.quoteId;
                if (p != null)
                {

                    if (p.MATERIAL_GROUP.Equals("BTOS", StringComparison.InvariantCultureIgnoreCase) || item.Partno.EndsWith("-BTO", StringComparison.OrdinalIgnoreCase))
                    {
                        if (isReconfig)
                            BTOSLineNo = Convert.ToInt32(_Parent);
                        else
                            BTOSLineNo = GetNextBTOSParentLineNo();
                        q.line_No = BTOSLineNo;
                        q.HigherLevel = 0;
                        q.ItemType = (int)LineItemType.BTOSParent;
                    }
                    else
                    {
                        if (BTOSLineNo == 0)
                        {
                            if (string.IsNullOrEmpty(_Parent))
                            {
                                q.line_No = GetNextLooseItemLineNo();
                                q.ItemType = (int)LineItemType.LooseItem;
                                q.HigherLevel = BTOSLineNo;
                            }
                            else
                            {
                                q.line_No = GetNextBTOSItemLineNo(Convert.ToInt32(_Parent));
                                q.ItemType = (int)LineItemType.BTOSChild;
                                q.HigherLevel = Convert.ToInt32(_Parent);
                            }
                        }
                        else
                        {
                            q.line_No = GetNextBTOSItemLineNo(BTOSLineNo);
                            q.ItemType = (int)LineItemType.BTOSChild;
                            q.HigherLevel = BTOSLineNo;
                        }
                    }
                    q.partNo = item.Partno;

                    int parentLineNo = 0;
                    if (q.IsEWpartnoX)//Alex 20180807 Add EW part logic
                    {
                        try
                        {
                            var lineNoWithEx = BTOSLineNo;
                            var ewList = MyAdvantechDAL.GetExtendedWarrantyByPlant(this.Plant);
                            var exItem = ewList.FirstOrDefault(e => e.EW_PartNO == q.partNo);
                            if (exItem != null)
                            {
                                if (BTOSLineNo == 0 && !string.IsNullOrEmpty(_Parent) && Int32.TryParse(_Parent, out parentLineNo))// loose item
                                    lineNoWithEx = parentLineNo;
                                QuotationDetail quoteItemWithEx = this.QuotationDetail.Where(d => d.line_No == lineNoWithEx).FirstOrDefault();
                                if (quoteItemWithEx == null || (quoteItemWithEx != null && quoteItemWithEx.ItemType != (int)LineItemType.BTOSChild))
                                {
                                    quoteItemWithEx.ewFlag = exItem.ID;
                                    var result = AddExtendWarranty(quoteItemWithEx);
                                    if (result)
                                        continue;// jump to next item because ExWarranty item is add by above function
                                }

                            }
                        }
                        catch { }
                    }


                    q.description = String.IsNullOrEmpty(p.PRODUCT_DESC) ? "" : p.PRODUCT_DESC;
                    q.qty = item.Qty;
                    q.listPrice = 0;
                    q.unitPrice = 0;
                    q.newUnitPrice = 0;
                    q.itp = 0;
                    q.newItp = 0;
                    q.RecyclingFee = 0;
                    q.deliveryPlant = this.Plant;
                    q.category = "";
                    q.classABC = "";
                    q.rohs = 1;
                    q.ewFlag = 0;
                    q.reqDate = DateTime.Now.AddDays(5);
                    q.dueDate = DateTime.Now.AddDays(5);
                    q.VirtualPartNo = p.PART_NO;
                    q.satisfyFlag = 0;
                    q.canBeConfirmed = 1;
                    q.custMaterial = "";
                    q.inventory = 0;
                    //q.oType = "";
                    q.modelNo = "";
                    q.RECFIGID = "";
                    q.SequenceNo = 0;
                    q.NCNR = Convert.ToInt32(SAPDAL.isNCNRPart(q.partNo, q.deliveryPlant));

                    this.QuotationDetail.Add(q);
                }

                
            }

            this.QuotationDetail = this.QuotationDetail.OrderBy(d => d.line_No).ToList();
        }

        public void UpdateExtendWarranty(DataCore.eQuotation.Model.JsonEWPart ewItem, int lineNoWithEx, ref string errMsg)
        {
            try
            {
                if (ewItem != null)
                {
                    if (ewItem.EWId == 0)
                    {
                        QuotationDetail quoteItemWithEx = this.QuotationDetail.Where(d => d.line_No == lineNoWithEx).FirstOrDefault();
                        if (quoteItemWithEx == null || (quoteItemWithEx != null && quoteItemWithEx.ItemType != (int)LineItemType.BTOSChild))
                        {
                            RemoveExtendWarranty(quoteItemWithEx);
                        }
                    }
                    else
                    {

                        var ewList = MyAdvantechDAL.GetExtendedWarrantyByPlant(this.Plant);
                        var exItem = ewList.FirstOrDefault(e => e.EW_PartNO == ewItem.Partno);
                        if (exItem != null)
                        {
                            QuotationDetail quoteItemWithEx = this.QuotationDetail.Where(d => d.line_No == lineNoWithEx).FirstOrDefault();
                            if (quoteItemWithEx == null || (quoteItemWithEx != null && quoteItemWithEx.ItemType != (int)LineItemType.BTOSChild))
                            {
                                RemoveExtendWarranty(quoteItemWithEx);
                                AddExtendWarranty(quoteItemWithEx);
                            }
                            // jump to next item because ExWarranty item is add by above function
                        }
                    }
                    this.ReorderQuotationDetail();
                }
            }
            catch(Exception ex) { errMsg = ex.Message; }
        }

        public bool AddExtendWarranty(QuotationDetail quoteItemWithEx)
        {
            try
            {
                if (quoteItemWithEx != null && quoteItemWithEx.EWPartNoX!=null)
                {
                    var ewQuoteItem = new QuotationDetail();

                    ewQuoteItem.quoteId = quoteItemWithEx.quoteId;
                    
                    ewQuoteItem.partNo = quoteItemWithEx.EWPartNoX.EW_PartNO;
                    ewQuoteItem.description = "Extended Warranty for " + quoteItemWithEx.EWPartNoX.EW_Month.ToString() + " Months";
                    ewQuoteItem.deliveryPlant = quoteItemWithEx.deliveryPlant;
                    ewQuoteItem.qty = quoteItemWithEx.qty;
                    if (quoteItemWithEx.ItemType == (int)LineItemType.BTOSParent)
                    {
                        ewQuoteItem.ItemType = (int)LineItemType.BTOSChild;
                        ewQuoteItem.HigherLevel = quoteItemWithEx.line_No;
                        ewQuoteItem.line_No = GetNextBTOSItemLineNo(quoteItemWithEx.line_No.Value);
                    }
                    else// it should be loose item
                    {
                        ewQuoteItem.ItemType = quoteItemWithEx.ItemType;
                        ewQuoteItem.HigherLevel = quoteItemWithEx.HigherLevel;
                        // get all items with same higer level, and add 1 for items which lineNo larger than partWithEx's lineNo
                        foreach(var relatedItem in this.QuotationDetail.Where(d => d.ItemType == (int)LineItemType.LooseItem && d.HigherLevel == quoteItemWithEx.HigherLevel && d.line_No > quoteItemWithEx.line_No).OrderByDescending(d => d.line_No))
                        {
                            relatedItem.line_No += 1;
                        }
                        ewQuoteItem.line_No = quoteItemWithEx.line_No + 1;
                    }
                    ewQuoteItem.listPrice = 0;
                    ewQuoteItem.unitPrice = 0;
                    ewQuoteItem.newUnitPrice = 0;
                    ewQuoteItem.itp = 0;
                    ewQuoteItem.newItp = 0;
                    ewQuoteItem.inventory = 0;
                    ewQuoteItem.canBeConfirmed = 1;
                    ewQuoteItem.reqDate = quoteItemWithEx.reqDate;
                    ewQuoteItem.dueDate = quoteItemWithEx.dueDate;
                    ewQuoteItem.ewFlag = 0;
                    ewQuoteItem.category = "";
                    ewQuoteItem.custMaterial = "";
                    ewQuoteItem.rohs = 0;
                    ewQuoteItem.satisfyFlag = 0;
                    ewQuoteItem.DMF_Flag = "";
                    ewQuoteItem.VirtualPartNo = "";
                    ewQuoteItem.RecyclingFee = 0;
                    ewQuoteItem.DELIVERYGROUP = "";
                    ewQuoteItem.ShipPoint = "";
                    ewQuoteItem.StorageLoc = "";
                    ewQuoteItem.RECFIGID = "";
                    this.QuotationDetail.Add(ewQuoteItem);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool RemoveExtendWarranty(QuotationDetail quoteItemWithEx)
        {
            try
            {
                var existedEWQuoteItem = new QuotationDetail();
                // Check if EW part existed, if yes, delete frist.
                if (quoteItemWithEx.ItemType == (int)LineItemType.BTOSParent)
                    existedEWQuoteItem = this.QuotationDetail.Where(d => d.HigherLevel == quoteItemWithEx.line_No && d.IsEWpartnoX == true).FirstOrDefault();
                else
                    existedEWQuoteItem = this.QuotationDetail.Where(d => d.HigherLevel == quoteItemWithEx.HigherLevel && d.line_No == quoteItemWithEx.line_No +1 && d.IsEWpartnoX == true).FirstOrDefault();

                if (existedEWQuoteItem != null)
                {
                    this.QuotationDetail.Remove(existedEWQuoteItem);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public void CalculateAllExtendWarrantyPrice()
        {
            try
            {
                foreach (var exQuoteItem in this.QuotationDetail.Where(d => d.IsEWpartnoX == true))
                {
                    var ewPart = MyAdvantechDAL.GetExtendedWarrantyPartNoAndPlant(exQuoteItem.partNo, this.Plant);
                    var partWithEx = new QuotationDetail();

                    if (exQuoteItem.ItemType == (int)LineItemType.BTOSChild)
                        partWithEx = this.QuotationDetail.FirstOrDefault(q => q.line_No == exQuoteItem.HigherLevel && q.ewFlag == ewPart.ID);
                    else
                        partWithEx = this.QuotationDetail.FirstOrDefault(q => q.line_No == exQuoteItem.line_No - 1 && q.ewFlag == ewPart.ID);

                    if (partWithEx != null)
                    {
                        var price = Decimal.Round(Convert.ToDecimal(GetExtendedWarrantyPrice(partWithEx)), 2);

                        exQuoteItem.listPrice = price;
                        exQuoteItem.unitPrice = price;
                        exQuoteItem.newUnitPrice = price;
                    }
                }
                this.InitializeQuotationDetail();
            }
            catch { }

        }

        public void CalculateExtendWarrantyPriceByLineNo(int lineNoWithEX)
        {
            try
            {
                var existedEWQuoteItem = new QuotationDetail();
                var quoteItemWithEx = this.QuotationDetail.FirstOrDefault(q => q.line_No == lineNoWithEX);
                if(quoteItemWithEx != null && ( quoteItemWithEx.ewFlag == null || quoteItemWithEx.ewFlag == 0))
                    quoteItemWithEx = this.QuotationDetail.FirstOrDefault(q => q.line_No == quoteItemWithEx.HigherLevel && q.ewFlag != null && q.ewFlag != 0);

                if (quoteItemWithEx != null)
                {

                    if (quoteItemWithEx.ItemType == (int)LineItemType.BTOSParent)
                        existedEWQuoteItem = this.QuotationDetail.Where(d => d.HigherLevel == quoteItemWithEx.line_No && d.IsEWpartnoX == true).FirstOrDefault();
                    else
                        existedEWQuoteItem = this.QuotationDetail.Where(d => d.HigherLevel == quoteItemWithEx.HigherLevel && d.line_No == quoteItemWithEx.line_No + 1 && d.IsEWpartnoX == true).FirstOrDefault();

                    if (existedEWQuoteItem != null)
                    {
                        var price = Decimal.Round(Convert.ToDecimal(GetExtendedWarrantyPrice(quoteItemWithEx)), 2);
                        existedEWQuoteItem.listPrice = price;
                        existedEWQuoteItem.unitPrice = price;
                        existedEWQuoteItem.newUnitPrice = price;
                    }

                }
                   



                //foreach (var exQuoteItem in this.QuotationDetail.Where(d => d.IsEWpartnoX == true))
                //{
                //    var ewPart = MyAdvantechDAL.GetExtendedWarrantyPartNoAndPlant(exQuoteItem.partNo, this.Plant);
                //    var partWithEx = new QuotationDetail();

                    //    if (exQuoteItem.ItemType == (int)LineItemType.BTOSChild)
                    //        partWithEx = this.QuotationDetail.FirstOrDefault(q => q.line_No == exQuoteItem.HigherLevel && q.ewFlag == ewPart.ID);
                    //    else
                    //        partWithEx = this.QuotationDetail.FirstOrDefault(q => q.line_No == exQuoteItem.line_No - 1 && q.ewFlag == ewPart.ID);

                    //    if (partWithEx!=null)
                    //    {
                    //        var price = Decimal.Round(Convert.ToDecimal(GetExtendedWarrantyPrice(partWithEx)), 2);

                    //        exQuoteItem.listPrice = price;
                    //        exQuoteItem.unitPrice = price;
                    //        exQuoteItem.newUnitPrice = price;
                    //    }
                    //}
                this.InitializeQuotationDetail();
            }
            catch { }
        }

        public decimal GetExtendedWarrantyPrice(QuotationDetail qd)
        {
            if (qd.ItemType == (int)LineItemType.BTOSParent)
            {
                
                var quoteBtosChildList = this.QuotationDetail.Where(q => q.HigherLevel == qd.line_No && q.quoteId == qd.quoteId).OrderBy(q => q.line_No).ToList();
                if (quoteBtosChildList.Count() > 0)
                    return qd.EWPartNoX.EW_Rate.Value * quoteBtosChildList.Where(q => SAPDAL.IsWarrantableV3(q.partNo, this.org) == true).Sum(q=> q.newUnitPrice.Value * (q.qty.Value / qd.qty.Value));
            }
            else if (qd.ItemType == (int)LineItemType.LooseItem)
            {
                return qd.EWPartNoX.EW_Rate.Value * qd.newUnitPrice.Value;
            }
            return 0;
        }

        public void UpdateQuotationDetail(int lineNo, string description, decimal newPrice, int qty, ref string errorMsg, bool isNCNR)
        {
            QuotationDetail item = this.QuotationDetail.Where(d => d.line_No == lineNo).FirstOrDefault();
            if (item != null)
            {
                switch ((LineItemType)item.ItemType)
                {
                    case LineItemType.BTOSParent:
                        int CurrentQTY = (int)item.qty;
                        item.description = description;
                        item.newUnitPrice = 0;
                        item.qty = qty;
                        this.UpdateBTOSQTY((int)item.line_No, CurrentQTY, qty);
                        break;

                    case LineItemType.BTOSChild:
                        int parentQty = (int)this.QuotationDetail.Where(d => d.line_No == item.HigherLevel).FirstOrDefault().qty;

                        if (qty % parentQty == 0)
                        {
                            item.description = description;
                            item.newUnitPrice = newPrice / (1 + this.tax.Value);
                            item.qty = qty;
                            this.InitializeQuotationDetail();
                        }
                        else
                        {
                            // Return false - "New qty is not a product of Parent QTY"
                            errorMsg = "Qty is invalid";
                        }
                        break;

                    case LineItemType.LooseItem:
                        item.description = description;
                        item.newUnitPrice = newPrice / (1 + this.tax.Value);
                        item.qty = qty;
                        item.NCNR = Convert.ToInt32(isNCNR);
                        this.InitializeQuotationDetail();
                        break;
                }
            }
        }

        public void UpdateQuotationDetailV2(int lineNo, string description, decimal preTaxQuotingPrice, int currentQty, int qty, ref string errorMsg, bool isNCNR)
        {
            QuotationDetail item = this.QuotationDetail.Where(d => d.line_No == lineNo).FirstOrDefault();
            if (item != null)
            {
                switch ((LineItemType)item.ItemType)
                {
                    case LineItemType.BTOSParent:
                        item.description = description;
                        item.newUnitPrice = 0;
                        item.qty = qty;
                        this.UpdateBTOSQTY((int)item.line_No, currentQty, qty);
                        break;

                    case LineItemType.BTOSChild:
                        int parentQty = (int)this.QuotationDetail.Where(d => d.line_No == item.HigherLevel).FirstOrDefault().qty;

                        if (qty % parentQty == 0)
                        {
                            item.description = description;
                            item.newUnitPrice = preTaxQuotingPrice;
                            item.qty = qty;
                            this.InitializeQuotationDetail();
                        }
                        else
                        {
                            // Return false - "New qty is not a product of Parent QTY"
                            errorMsg = "Qty is invalid";
                        }
                        break;

                    case LineItemType.LooseItem:
                        item.description = description;
                        item.newUnitPrice = preTaxQuotingPrice;
                        item.qty = qty;
                        item.NCNR = Convert.ToInt32(isNCNR);
                        this.InitializeQuotationDetail();
                        break;
                }
            }
        }

        public void ReorderQuotationDetail()
        {
            var reorderQuotationDetails = new List<QuotationDetail>();
            var LooseItems = this.QuotationDetail.Where(q => q.ItemType == (int)LineItemType.LooseItem).OrderBy(q => q.line_No).ToList();
            int lineNo = 1;
            foreach (var item in LooseItems)
            {
                item.line_No = lineNo;
                reorderQuotationDetails.Add(item);
                lineNo += 1;
            }


            var BTOSParentItems = this.QuotationDetail.Where(q => q.ItemType == (int)LineItemType.BTOSParent).OrderBy(q => q.line_No).ToList();

            foreach (var parentItem in BTOSParentItems)
            {
                lineNo = 1;
                reorderQuotationDetails.Add(parentItem);
                var BTOSChildItems = this.QuotationDetail.Where(q => q.ItemType == (int)LineItemType.BTOSChild && q.HigherLevel == parentItem.line_No).OrderBy(q => q.line_No).ToList();
                foreach (var item in BTOSChildItems)
                {
                    item.line_No = parentItem.line_No.Value + lineNo;
                    reorderQuotationDetails.Add(item);
                    lineNo += 1;
                }
            }
            this.QuotationDetail = reorderQuotationDetails;

        }

        public void UpdateBTOSParentPriceProperties()
        {
            // Set all BTOSParent viewing price properties
            foreach (QuotationDetail qd in this.QuotationDetail.Where(d => d.ItemType == (int)LineItemType.BTOSParent))
            {
                //qd.PostTaxListPrice = 0;
                //qd.PostTaxUnitPrice = 0;
                //qd.PostTaxNewUnitPrice = 0;
                //qd.PostTaxSubTotal = 0;
                //this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == qd.line_No)
                //                    .ToList()
                //                    .ForEach(d =>
                //                    {
                //                        qd.PostTaxListPrice += (Decimal)d.PostTaxListPrice;
                //                        qd.PostTaxUnitPrice += (Decimal)d.PostTaxUnitPrice;
                //                        qd.PostTaxNewUnitPrice += (Decimal)d.PostTaxNewUnitPrice;
                //                        qd.PostTaxSubTotal += (Decimal)d.PostTaxSubTotal;
                //                    });
            }
        }

        public void InitializeQuotationDetail()
        {
            this._QuotationDetail.ForEach(d =>
            {
                d.PostTaxListPrice = Math.Round((Decimal)(d.listPrice * (1 + this.tax)), 2, MidpointRounding.AwayFromZero);
                d.PostTaxUnitPrice = Math.Round((Decimal)(d.unitPrice * (1 + this.tax)), 2, MidpointRounding.AwayFromZero);
                d.PostTaxNewUnitPrice = Math.Round((Decimal)(d.newUnitPrice * (1 + this.tax)), 2, MidpointRounding.AwayFromZero);
                //d.PostTaxSubTotal = (Decimal)(d.SubTotal * (1 + this.tax));
                d.PostTaxSubTotal = d.PostTaxNewUnitPrice * d.qty.Value;

                //Ryan 20180214 Net ACN itp rate confirmed by Patty, OS Parts with 1.20, PTD with 1.22, else with 1.24
                //Alex 20180709 Set ACN 96PR Part's itp rate to 1.00
                if (d.isACNOSParts)
                    d.ACN_GeneralRate = 0.20m;
                else if (d.Is96PR)
                    d.ACN_GeneralRate = 0m;
                else if (d.IsPTD)
                    d.ACN_GeneralRate = 0.22m;
                else
                    d.ACN_GeneralRate = 0.24m;

                d.ITPWithGeneralAmount = (Decimal)(d.newItp.Value * (1 + d.ACN_GeneralRate));
                if (d.PostTaxNewUnitPrice > 0)
                {
                    d.ACNMargin = Math.Round((d.PostTaxNewUnitPrice - d.ITPWithGeneralAmount) / d.PostTaxNewUnitPrice, 4, MidpointRounding.AwayFromZero);
                }
                else
                    d.ACNMargin = 0;


            });
            //UpdateBTOSParentPriceProperties();
        }


        public decimal SumBTOChildPostTaxUnitPrice(int higerLevel)
        {
            return this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == higerLevel).Sum(d => d.PostTaxUnitPrice);
        }

        public decimal SumBTOChildPostTaxListPrice(int higerLevel)
        {
            return this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == higerLevel).Sum(d => d.PostTaxListPrice);
        }

        public decimal SumBTOChildPostTaxNewUnitPrice(int higerLevel)
        {
            return this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == higerLevel).Sum(d => d.PostTaxNewUnitPrice);
        }

        public decimal SumBTOChildPostTaxSubTotal(int higerLevel)
        {
            return this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == higerLevel).Sum(d => d.PostTaxSubTotal);
        }

        public decimal SumBTOChildPreTaxUnitPrice(int higerLevel)
        {
            return this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == higerLevel).Sum(d => d.unitPrice.Value);
        }

        public decimal SumBTOChildPreTaxListPrice(int higerLevel)
        {
            return this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == higerLevel).Sum(d => d.listPrice.Value);
        }

        public decimal SumBTOChildPreTaxNewUnitPrice(int higerLevel)
        {
            return this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == higerLevel).Sum(d => d.newUnitPrice.Value);
        }

        public decimal SumBTOChildPreTaxSubTotal(int higerLevel)
        {
            return this.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId) && d.HigherLevel == higerLevel).Sum(d => d.SubTotal);
        }

        public void UpdateBTOSQTY(int ParentLineNo, int CurrentBTOSQTY, int NewBTOSQTY)
        {
            this.QuotationDetail.Where(x => x.ItemType == (int)LineItemType.BTOSParent && x.line_No == ParentLineNo)
                .ToList()
                .ForEach(d => this._QuotationDetail.Where(c => c.HigherLevel == d.line_No)
                                                   .ToList()
                                                   .ForEach(b => b.qty = (b.qty / CurrentBTOSQTY) * NewBTOSQTY));
            InitializeQuotationDetail();
        }

        /// <summary>
        /// Re-processed quotation detail after org changed, remove un-orderable items and re-simulate price
        /// </summary>
        public void RemoveUnorderableItems()
        {
            if (this.QuotationDetail != null && this.QuotationDetail.Count > 0)
            {
                try
                {
                    for (int i = this.QuotationDetail.Count - 1; i >= 0; i--)
                    {
                        this.QuotationDetail[i].deliveryPlant = this.Plant;
                        String partno = this.QuotationDetail[i].partNo;
                        SAP_PRODUCT_STATUS_ORDERABLE SPSO = MyAdvantechContext.Current.SAP_PRODUCT_STATUS_ORDERABLE.Where(c => c.SALES_ORG.Equals(this.org) && c.PART_NO.Equals(partno)).FirstOrDefault();
                        if (SPSO == null)
                        {
                            if (this.QuotationDetail[i].ItemType == (int)LineItemType.BTOSParent)
                                this.QuotationDetail.RemoveAll(c => c.HigherLevel == this.QuotationDetail[i].line_No);

                            this.QuotationDetail.RemoveAt(i);
                        }
                    }
                    this.ReorderQuotationDetail(); // 刪除item後,重新定義lineNo，讓lineNo連續                   
                }
                catch (Exception ex) { }

            }
        }

        public void SaveAllChanges()
        {
            // Quotation Master
            if (eQuotationContext.Current.QuotationMaster.Where(d => d.quoteId.Equals(this.quoteId, StringComparison.OrdinalIgnoreCase)).Any())
            {
                eQuotationContext.Current.QuotationMaster.Attach(this);
                eQuotationContext.Current.Entry(this).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                eQuotationContext.Current.QuotationMaster.Add(this);
            }

            // Quotation Detail
            List<QuotationDetail> ExistQDs = eQuotationContext.Current.QuotationDetail.Where(d => d.quoteId.Equals(this.quoteId, StringComparison.OrdinalIgnoreCase)).ToList();
            eQuotationContext.Current.QuotationDetail.RemoveRange(ExistQDs);
            eQuotationContext.Current.QuotationDetail.AddRange(this.QuotationDetail);

            // Quotation Parner
            List<EQPARTNER> ExistQPs = eQuotationContext.Current.EQPARTNER.Where(d => d.QUOTEID.Equals(this.quoteId, StringComparison.OrdinalIgnoreCase)).ToList();
            eQuotationContext.Current.EQPARTNER.RemoveRange(ExistQPs);
            eQuotationContext.Current.EQPARTNER.AddRange(this.QuotationPartner);

            // ACN Quotation Extension
            List<QuotationExtensionNew> QuotationExtensionNew = eQuotationContext.Current.QuotationExtensionNew.Where(d => d.QuoteID.Equals(this.quoteId, StringComparison.OrdinalIgnoreCase)).ToList();
            eQuotationContext.Current.QuotationExtensionNew.RemoveRange(QuotationExtensionNew);
            eQuotationContext.Current.QuotationExtensionNew.Add(this.QuotationExtensionNew);


            // Quotation Opty
            if (eQuotationContext.Current.optyQuote.Where(d => d.quoteId.Equals(this.quoteId, StringComparison.OrdinalIgnoreCase)).Any())
            {
                eQuotationContext.Current.optyQuote.Attach(this.QuotationOpty);
                eQuotationContext.Current.Entry(this.QuotationOpty).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                if (this.QuotationOpty != null)
                {
                    eQuotationContext.Current.optyQuote.Add(this.QuotationOpty);
                }
            }

            // Siebel Active
            if (this.SiebelActive != null && this.SiebelActive.Count > 0)
            {
                eQuotationContext.Current.SiebelActive.AddRange(this.SiebelActive);
            }

            try
            {
                eQuotationContext.Current.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            //Alex 20180720: not good to pust this code here, wait to modify
            UpdateQuotationRemark();
        }

        public void UpdateQuotationRemark()
        {
            try
            {
                SqlProvider.dbExecuteScalar("EQ", string.Format(" delete from QuotationNote where quoteid = '{0}' and notetype = 'Remark' ", this.quoteId));
                if(!string.IsNullOrEmpty(this.Remark))
                    SqlProvider.dbExecuteScalar("EQ", string.Format(" insert into QuotationNote values('{0}','Remark','{1}')", this.quoteId, this.Remark));
            }
            catch { }
        }

        public string GetQuotationSalesRepresentative()
        {
            string salesRepresentative = String.Empty;
            string salesCode = String.Empty;

            salesRepresentative = this.QuotationPartner.Where(p => p.TYPE == "E").Select(p => p.NAME).FirstOrDefault();
            if (String.IsNullOrEmpty(salesRepresentative))
            {
                salesRepresentative = this.createdBy;
                if (String.IsNullOrEmpty(salesRepresentative))
                {
                    salesCode = this.QuotationPartner.Where(p => p.TYPE == "E").Select(p => p.ERPID).FirstOrDefault();
                    if (!String.IsNullOrEmpty(salesCode))
                    {
                        List<SAP_EMPLOYEE> se = new MyAdvantechDAL().GetSAPEmployeeBySalesCode(salesCode);
                        salesRepresentative = (from d in se select d.EMAIL).FirstOrDefault();
                    }
                }
            }
            return !String.IsNullOrEmpty(salesRepresentative) ? salesRepresentative : "";
        }

        public string GetQuotationSalesRepresentativeSalesCode()
        {
            string salesCode = this.QuotationPartner.Where(p => p.TYPE == "E").Select(p => p.ERPID).FirstOrDefault();
            if (String.IsNullOrEmpty(salesCode))
            {
                string salesRepresentative = this.QuotationPartner.Where(p => p.TYPE == "E").Select(p => p.NAME).FirstOrDefault();
                if (String.IsNullOrEmpty(salesRepresentative))
                {
                    salesRepresentative = this.createdBy;
                    List<SAP_EMPLOYEE> se = new MyAdvantechDAL().GetSAPEmployeeBySalesEmail(salesRepresentative);
                    salesCode = (from d in se select d.SALES_CODE).FirstOrDefault();
                }
            }

            return !String.IsNullOrEmpty(salesCode) ? salesCode : "";
        }

        public bool IsContainLoosItem
        {
            get
            {
                QuotationDetail quotationDetail = this.QuotationDetail.Where(d => d.line_No <= 99).FirstOrDefault();

                if (quotationDetail != null)
                    return true;
                return false;
            }
        }

        public List<List<QuotationDetail>> ConfigurationList
        {
            get
            {
                List<QuotationDetail> qutationDetails = this.QuotationDetail;
                return qutationDetails.GroupBy(d => d.HigherLevel.Value).Select(grp => grp.ToList()).ToList();
            }
        }

        private List<WorkFlowApproval> _waitingApprovals;
        public List<WorkFlowApproval> WaitingApprovals
        {
            get
            {
                if (_waitingApprovals == null)
                    return new List<WorkFlowApproval>();
                return _waitingApprovals;
            }

            set { _waitingApprovals = value; }
        }

        public string ApprovalStatus { get; set; }

        public string SimulatePriceErrorMessage { get; set; }
    }
}
