using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech
{
    public class SAPCompanyHelper
    {
        public static List<SAP_DIMCOMPANY> GetSAPDIMCompanyByID(String _CompanyID)
        {
            return MyAdvantechContext.Current.SAP_DIMCOMPANY.Where(d => d.COMPANY_ID.Equals(_CompanyID)).ToList();
        }

        public static List<SAP_DIMCOMPANY> GetSAPDIMCompanyByID(String _CompanyID, String _ORGID)
        {
            return MyAdvantechContext.Current.SAP_DIMCOMPANY.Where(d => d.COMPANY_ID.Equals(_CompanyID, StringComparison.OrdinalIgnoreCase) && d.ORG_ID.Equals(_ORGID, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<SAP_DIMCOMPANY> GetSAPDIMCompanyShiptoByID(String _SoldtoID, String _ORGID, String _ShiptoID, String _CompanyName)
        {
            if (String.IsNullOrEmpty(_SoldtoID))
                return new List<SAP_DIMCOMPANY>();

            List<string> CompanyType = new List<string> { "Z001", "Z002" };
            List<SAP_DIMCOMPANY> result = (from SAPCompanyPartners in MyAdvantechContext.Current.SAP_COMPANY_PARTNERS
                                           join SAPDimCompany in MyAdvantechContext.Current.SAP_DIMCOMPANY on SAPCompanyPartners.PARENT_COMPANY_ID equals SAPDimCompany.COMPANY_ID
                                           where SAPCompanyPartners.COMPANY_ID.Equals(_SoldtoID) &&
                                                 SAPCompanyPartners.ORG_ID.Equals(_ORGID) &&
                                                 SAPCompanyPartners.PARTNER_FUNCTION.Equals("WE") &&
                                                 CompanyType.Contains(SAPDimCompany.COMPANY_TYPE) &&
                                                 (String.IsNullOrEmpty(_ShiptoID) ? true : SAPCompanyPartners.PARENT_COMPANY_ID.Contains(_ShiptoID)) &&
                                                 (String.IsNullOrEmpty(_CompanyName) ? true : SAPDimCompany.COMPANY_NAME.Contains(_CompanyName))
                                           orderby SAPCompanyPartners.DEFPA descending
                                           select SAPDimCompany).ToList();

            return result;
        }

        public static List<SAP_DIMCOMPANY> GetSAPDIMCompanyBilltoByID(String _SoldtoID, String _ORGID, String _BilltoID, String _CompanyName)
        {
            if (String.IsNullOrEmpty(_SoldtoID))
                return new List<SAP_DIMCOMPANY>();

            List<string> CompanyType = new List<string> { "Z001", "Z003" };
            List<SAP_DIMCOMPANY> result = (from SAPCompanyPartners in MyAdvantechContext.Current.SAP_COMPANY_PARTNERS
                                           join SAPDimCompany in MyAdvantechContext.Current.SAP_DIMCOMPANY on SAPCompanyPartners.PARENT_COMPANY_ID equals SAPDimCompany.COMPANY_ID
                                           where SAPCompanyPartners.COMPANY_ID.Equals(_SoldtoID) &&
                                                 SAPCompanyPartners.ORG_ID.Equals(_ORGID) &&
                                                 SAPCompanyPartners.PARTNER_FUNCTION.Equals("RE") &&
                                                 CompanyType.Contains(SAPDimCompany.COMPANY_TYPE) &&
                                                 (String.IsNullOrEmpty(_BilltoID) ? true : SAPCompanyPartners.PARENT_COMPANY_ID.Contains(_BilltoID)) &&
                                                 (String.IsNullOrEmpty(_CompanyName) ? true : SAPDimCompany.COMPANY_NAME.Contains(_CompanyName))
                                           orderby SAPDimCompany.CREATEDDATE
                                           select SAPDimCompany).ToList();

            return result;
        }

        public static List<SAP_DIMCOMPANY> GetSAPDIMCompanyEndCustomerByID(String _SoldtoID, String _ORGID, String _EndCustomerID, String _EndCustomerName)
        {
            if (String.IsNullOrEmpty(_SoldtoID))
                return new List<SAP_DIMCOMPANY>();

            List<SAP_DIMCOMPANY> result = new List<SAP_DIMCOMPANY>();
            //List<String> EndCustomers = (from SAPDimCompany in MyAdvantechContext.Current.SAP_DIMCOMPANY
            //                             join SAPCompanyPartners in MyAdvantechContext.Current.SAP_COMPANY_PARTNERS on SAPDimCompany.COMPANY_ID equals SAPCompanyPartners.COMPANY_ID
            //                             where SAPDimCompany.COMPANY_ID.Equals(_SoldtoID, StringComparison.OrdinalIgnoreCase) &&
            //                                   SAPCompanyPartners.PARTNER_FUNCTION.Equals("EM") &&
            //                                   !String.IsNullOrEmpty(SAPCompanyPartners.PARENT_COMPANY_ID) &&
            //                                   (String.IsNullOrEmpty(_ORGID) ? true : SAPCompanyPartners.ORG_ID.Equals(_ORGID, StringComparison.OrdinalIgnoreCase))
            //                             orderby SAPCompanyPartners.DEFPA descending
            //                             select "'" + SAPCompanyPartners.PARENT_COMPANY_ID + "'").ToList();


            String sql = " SELECT distinct A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME, " +
                    " D.street || ' ' || D.city1 || ' ' || D.region || ' ' || D.post_code1 || ' ' || (select e.landx from saprdp.t005t e where e.land1 = B.land1 and e.spras = 'E' and rownum = 1) AS Address, " +
                    " B.Land1 AS COUNTRY, B.Ort01 AS CITY, B.STRAS as STREET, " +
                    " B.PSTLZ AS ZIP_CODE, D.region AS STATE, B.TELF1 AS TEL_NO, B.TELFX AS FAX_NO, D.NAME_CO as Attention, " +
                    " '' as PARTNER_FUNCTION, " +
                    " E.VKBUR as SalesOffice, E.VKGRP As SalesGroup, E.SPART As division, D.STR_SUPPL3, A.DEFPA" +
                    " From saprdp.kna1 B " +
                    " Left join saprdp.adr6 C on B.adrnr = C.addrnumber " +
                    " inner join  saprdp.knvp A on A.KUNN2 = B.KUNNR " +
                    " inner join saprdp.adrc D on D.country = B.land1 And D.addrnumber = B.adrnr " +
                    " inner join saprdp.knvv E on B.KUNNR = E.KUNNR " +
                    " where B.loevm <> 'X' AND A.PARVW ='EM' " +
                    " AND A.KUNNR = '" + _SoldtoID + "' AND A.VKORG = '" + _ORGID + "'";
            if (!String.IsNullOrEmpty(_EndCustomerID)) sql += String.Format(" AND A.KUNN2 like '%{0}%' ", _EndCustomerID.Replace("'", "''").Trim());
            if (!String.IsNullOrEmpty(_EndCustomerName)) sql += String.Format(" and (Upper(B.NAME1) LIKE '%{0}%' or B.NAME2 like '%{0}%') ", _EndCustomerName.Replace("'", "''").Trim());
            sql += " ORDER BY A.DEFPA desc, A.KUNN2";

            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                if (dt.Select("DEFPA = 'X'").Count() > 0)
                {
                    dr = dt.Select("DEFPA = 'X'")[0];
                }
                else if (dt.Select("company_id = '" + _SoldtoID + "'").Count() > 0)
                {
                    dr = dt.Select("company_id = '" + _SoldtoID + "'")[0];
                }
                else
                {
                    dr = dt.Rows[0];
                }



                SAP_DIMCOMPANY sd = new SAP_DIMCOMPANY();
                sd.COMPANY_ID = dr["company_id"].ToString();
                sd.COMPANY_NAME = dr["COMPANY_NAME"].ToString();
                sd.ADDRESS = dr["Address"].ToString();
                sd.CITY = dr["CITY"].ToString();
                sd.REGION_CODE = dr["STATE"].ToString();
                sd.ZIP_CODE = dr["ZIP_CODE"].ToString();
                sd.COUNTRY = dr["COUNTRY"].ToString();
                sd.ATTENTION = dr["Attention"].ToString();
                sd.TEL_NO = dr["TEL_NO"].ToString();

                result.Add(sd);
            }

            return result;
        }
    }
}
