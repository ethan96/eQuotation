using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class QuotationDetail
    {

        public bool IsEWpartnoX
        {
            get
            {

                if (this.partNo.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase)) return true;
                return false;
            }
        }

        /// <summary>
        /// Is Service Part
        /// </summary>
        public bool IsServicePartX
        {
            get
            {

                if (this.partNo.StartsWith("AGS-", StringComparison.CurrentCultureIgnoreCase)) return true;
                return false;
            }
        }

        /// <summary>
        /// Is PTD
        /// </summary>
        public bool IsPTD
        {
            get
            {
                if (this.partNo.StartsWith("SQR") || (this.partNo.StartsWith("SQF") || this.partNo.StartsWith("EXM")))
                {
                    return false;
                }

                String STR = String.Format(("select count(*) from SAP_PRODUCT where " + (" ((PRODUCT_TYPE = \'ZPER\') " + (" OR " + (@" ((PRODUCT_TYPE = 'ZFIN' OR PRODUCT_TYPE = 'ZOEM') AND (PART_NO LIKE 'BT%' OR PART_NO LIKE 'DSD%' OR PART_NO LIKE 'ES%' OR PART_NO LIKE 'EWM%' OR PART_NO LIKE 'GPS%' OR PART_NO LIKE 'SQF%' OR PART_NO LIKE 'WIFI%' OR PART_NO LIKE 'PMM%' OR PART_NO LIKE 'Y%')) " + (" OR " + (" ((PRODUCT_TYPE = \'ZRAW\') AND (PART_NO LIKE \'206Q%\')) " + (" OR " + (" ((PRODUCT_TYPE = \'ZSEM\') AND (PART_NO LIKE \'968Q%\'))) " + " AND PART_NO = \'{0}\'")))))))), this.partNo);
                int count = Convert.ToInt32(SqlProvider.dbExecuteScalar("MY", STR));
                if (count > 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Is 96PR Part
        /// </summary>
        public bool Is96PR
        {
            get
            {
                if (this.partNo.StartsWith("96PR"))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Identify quote line type
        /// </summary>
        public QuoteItemType ItemTypeX
        {
            get
            {

                //if (Enum.IsDefined(typeof(QuoteItemType), this.ItemType))
                //{
                //    return (QuoteItemType)Enum.ToObject(typeof(QuoteItemType), this.ItemType);
                //}
                if (this.ItemType == 1) return QuoteItemType.BtosParent;
                if (this.ItemType == 0)
                {
                    if (this.HigherLevel != null && this.HigherLevel > 0) return QuoteItemType.BtosPart;
                }
                else
                {
                    return QuoteItemType.Part;
                }
                return QuoteItemType.Part;
            }
        }

        public Decimal MinimumPrice { get; set; }
        public Decimal Margin
        {
            get
            {
                if (this.newUnitPrice.Value > 0)
                {
                    return Math.Round((this.newUnitPrice.Value - this.newItp.Value) / this.newUnitPrice.Value * 100, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    return 0;
                }
            }
        }

        public Decimal SubTotal
        {
            get
            {
                return (Decimal)(this.newUnitPrice * this.qty);
            }
        }

        public Decimal ITPSubTotal
        {
            get
            {
                return (Decimal)(this.newItp * this.qty);
            }
        }

        public string ProductDiv
        {
            get
            {
                String sql_str = string.Format("SELECT TOP 1 [product_division] " +
                    "FROM [MyAdvantechGlobal].[dbo].[EAI_PRODUCT_HIERARCHY]  where part_no = '{0}'", partNo);

                object productDivObject = DataAccess.SqlProvider.dbExecuteScalar("MY", sql_str);
                if (productDivObject != null)
                    return productDivObject.ToString();
                else
                    return "";
            }
        }

        private Decimal _PostTaxListPrice = 0;
        public Decimal PostTaxListPrice
        {
            get
            {
                return _PostTaxListPrice;
            }
            set
            {
                _PostTaxListPrice = value;
            }
        }

        private Decimal _PostTaxUnitPrice = 0;
        public Decimal PostTaxUnitPrice
        {
            get
            {
                return _PostTaxUnitPrice;
            }
            set
            {
                _PostTaxUnitPrice = value;
            }
        }

        private Decimal _PostTaxNewUnitPrice = 0;
        public Decimal PostTaxNewUnitPrice
        {
            get
            {
                return _PostTaxNewUnitPrice;
            }
            set
            {
                _PostTaxNewUnitPrice = value;
            }
        }

        private Decimal _PostTaxSubTotal = 0;
        public Decimal PostTaxSubTotal
        {
            get
            {
                return _PostTaxSubTotal;
            }
            set
            {
                _PostTaxSubTotal = value;
            }
        }


        public Decimal DefaultDiscountRate
        {
            get
            {
                return (this.listPrice == 0) ? 0 : Decimal.Round((this.listPrice.Value - this.unitPrice.Value) / this.listPrice.Value, 4);
            }

        }

        public Decimal SalesDiscountRate
        {
            get
            {
                return (this.unitPrice == 0) ? 0 : Decimal.Round((this.unitPrice.Value - this.newUnitPrice.Value) / this.unitPrice.Value, 4);
            }

        }


        public Decimal OriginalSalesDiscountRate { get; set; }

        private Decimal _ITPWithGeneralAmount = 0;
        public Decimal ITPWithGeneralAmount
        {
            get
            {
                return _ITPWithGeneralAmount;
            }
            set
            {
                _ITPWithGeneralAmount = value;
            }
        }


        public string GPStatus { get; set; }


        public Decimal BelowSAPPriceRate
        {
            get
            {
                if (this.unitPrice.Value > 0)
                {
                    return Math.Round((this.unitPrice.Value - this.newUnitPrice.Value ) / this.unitPrice.Value, 4, MidpointRounding.AwayFromZero);
                }
                else
                {
                    return 0;
                }
            }
        }
        
        public Decimal ProduceBOMCost { get; set; }

        public Decimal End2EndGP
        {
            get
            {
                if (this.newUnitPrice != 0 && this.ProduceBOMCost > 0)
                {
                    return (Decimal)((this.newUnitPrice - this.ProduceBOMCost) / this.newUnitPrice);
                }
                return (Decimal)0.0;
            }
        }

        public Decimal ACNMargin { get; set; }

        public Decimal ACN_PCPPrice { get; set; }

        public Decimal ACN_GeneralRate { get; set; }

        public bool BelowACNPCPPrice
        {
            get
            {
                if (this.ACN_PCPPrice > 0 && this.newUnitPrice.Value > 0)
                {
                    if (this.ACN_PCPPrice - this.newUnitPrice.Value  > 0.1m) //do not use 0 because acn has tax rate transfer issue
                        return true;
                }
                return false;

            }
        }

        public Boolean isACNOSParts
        {
            get
            {
                if ((!String.IsNullOrEmpty(this.partNo)) && (this.partNo.StartsWith("968C") || this.partNo.StartsWith("968B") || this.partNo.StartsWith("209B")))
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public Boolean isACNNumberParts
        {
            get
            {
                String sql_str = string.Format("select PRODUCT_TYPE from  SAP_PRODUCT where PART_NO = '{0}'", partNo);

                object productTypeObject = DataAccess.SqlProvider.dbExecuteScalar("MY", sql_str);
                if (productTypeObject != null && productTypeObject.ToString() == "ZRAW" )
                    return true;
                return false;

            }
        }

        public Boolean isACNLocalNumberParts
        {
            get
            {
                if (isACNNumberParts)
                {
                    var _SpecialProcurement = Advantech.Myadvantech.DataAccess.SAPDAL.GetSpecialProcurement(partNo, deliveryPlant);
                    if (_SpecialProcurement !=null && _SpecialProcurement.StartsWith("F", StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
                return false;
            }

        }

        public Boolean isACNLocalPTDParts
        {
            get
            {
                if (IsPTD)
                {
                    var _SpecialProcurement = Advantech.Myadvantech.DataAccess.SAPDAL.GetSpecialProcurement(partNo, deliveryPlant);
                    if (_SpecialProcurement != null && _SpecialProcurement.Equals("20", StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
                return false;
            }

        }

        public ExtendedWarrantyPartNo_V2 EWPartNoX
        {
            get
            {
                if (this.ewFlag.HasValue && this.ewFlag > 0)
                    return MyAdvantechDAL.GetExtendedWarrantyById(this.ewFlag.Value);
                return null;
            }
        }

        public bool IsSoftwarePart
        {
            get
            {
                return SAPDAL.IsSoftwareV3(this.partNo);
            }
        }
    }
}
