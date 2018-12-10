using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class QuotationPartnerHelper
    {
        public List<EQPARTNER> GetQuotationPartner(String _QuoteID)
        {
            return eQuotationContext.Current.EQPARTNER.Where(d => d.QUOTEID.Equals(_QuoteID)).ToList();
        }

        /// <summary>
        /// 建立EQPartner表資料
        /// Sold_to(SOLD_TO), Ship_to(S), Bill_to(B) 三種type傳入ERPID後撈取地址資訊進行設定，相關設定與地址不可留空
        /// EndCustomer(EM)與Employee(E)僅需設定ERPID欄位即可，無設定時ERPID欄位留白
        /// </summary>
        /// <param name="_ERPID"></param>
        /// <param name="_QuoteID"></param>
        /// <param name="_PartnerTypes"></param>
        public static void CreateEQPartnerByERPID(String _ERPID, String _QuoteID, OrderPartnerType _PartnerTypes)
        {
            if (String.IsNullOrEmpty(_QuoteID))
                return;

            SAP_DIMCOMPANY SAPDimCompany = MyAdvantechContext.Current.SAP_DIMCOMPANY.Where(d => d.COMPANY_ID.Equals(_ERPID, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            DataTable dt = DataAccess.SAPDAL.GetSAPCompanyAddressByID(_ERPID);
            EQPARTNER e = new EQPARTNER();
            e.QUOTEID = _QuoteID;
            e.ERPID = _ERPID;
            e.ROWID = "";

            switch (_PartnerTypes)
            {
                case OrderPartnerType.SoldTo:
                case OrderPartnerType.ShipTo:
                case OrderPartnerType.BillTo:

                    // These three types are now allowed to leave fields blank.
                    if (SAPDimCompany == null || dt == null || dt.Rows.Count == 0)
                        return;

                    String type = String.Empty;

                    if (_PartnerTypes == OrderPartnerType.SoldTo)
                        type = "SOLDTO";
                    else if (_PartnerTypes == OrderPartnerType.ShipTo)
                        type = "S";
                    else if (_PartnerTypes == OrderPartnerType.BillTo)
                        type = "B";

                    e.TYPE = type;
                    e.NAME = SAPDimCompany.COMPANY_NAME;
                    e.ADDRESS = SAPDimCompany.ADDRESS;
                    e.ATTENTION = SAPDimCompany.ATTENTION;
                    e.TEL = SAPDimCompany.TEL_NO;
                    e.MOBILE = "";
                    e.ZIPCODE = SAPDimCompany.ZIP_CODE;
                    e.COUNTRY = SAPDimCompany.COUNTRY;
                    e.CITY = SAPDimCompany.CITY;
                    e.STREET = dt.Rows[0]["STREET"].ToString(); // STREET2 is not in SAP_DIMCOMPANY... use dt
                    e.STREET2 = dt.Rows[0]["STR_SUPPL3"].ToString(); // STREET2 is not in SAP_DIMCOMPANY... use dt
                    e.STATE = SAPDimCompany.REGION_CODE;
                    e.DISTRICT = "";                    
                    e.FAX = SAPDimCompany.FAX_NO;

                    break;
                case OrderPartnerType.EndCoutomer:
                    e.NAME = "";
                    e.ADDRESS = "";
                    e.ATTENTION = "";
                    e.TEL = "";
                    e.MOBILE = "";
                    e.ZIPCODE = "";
                    e.COUNTRY = "";
                    e.CITY = "";
                    e.STATE = "";
                    e.DISTRICT = "";
                    e.STREET = "";
                    e.STREET2 = "";
                    e.FAX = "";
                    e.TYPE = "EM";
                    break;
                case OrderPartnerType.Employee1:
                    e.NAME = "";
                    e.ADDRESS = "";
                    e.ATTENTION = "";
                    e.TEL = "";
                    e.MOBILE = "";
                    e.ZIPCODE = "";
                    e.COUNTRY = "";
                    e.CITY = "";
                    e.STATE = "";
                    e.DISTRICT = "";
                    e.STREET = "";                
                    e.STREET2 = "";
                    e.FAX = "";
                    e.TYPE = "E";
                    break;
            }
            eQuotationContext.Current.EQPARTNER.Add(e);
            eQuotationContext.Current.SaveChanges();
        }
    }
}
