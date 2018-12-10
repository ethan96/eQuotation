using Advantech.Myadvantech.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace eQuotation.DataAccess
{
    public class SAPDAL
    {
        static int _SAPWSTimeOut = 60000;

        public static List<SelectListItem> GetORGList(string ERPID, string SiebelRBU)
        {
            var ORGList = new List<SelectListItem>();
            var SAPList = new List<SAP_DIMCOMPANY>();
            SAPList = MyAdvantechContext.Current.SAP_DIMCOMPANY.Where(d => d.COMPANY_ID.Equals(ERPID)).ToList();

            if (SAPList != null)
            {
                if (SAPList.Count > 1)
                {
                    // Ryan 20170927 For ACN - Check if SAP_COMPANY_ORG has been setup, if not, do insert sql immediately.
                    //Alex 20180629 Move ACN set org function to Myadvantech API
                    if (SAPList.Where(d => d.ORG_ID.StartsWith("CN")).Any())
                    {
                        //if (Convert.ToInt32(SqlProvider.dbExecuteScalar("MY", "select count(*) as count from SAP_COMPANY_ORG where COMPANY_ID = '" + ERPID + "'")) == 0)
                        //{
                        //    SqlProvider.dbExecuteNoQuery("MY", String.Format("insert into SAP_COMPANY_ORG values ('{0}','CN10','CN10','1')", ERPID));
                        //    SqlProvider.dbExecuteNoQuery("MY", String.Format("insert into SAP_COMPANY_ORG values ('{0}','CN30','CN30','0')", ERPID));
                        //}
                        Advantech.Myadvantech.DataAccess.SAPDAL.SetACNMultiOrg(ERPID);

                    }

                    string _sql = " select isnull(ORG_ID,'') as ORG_ID,isnull(ORG_NAME,'') as ORG_NAME,IS_DEFAULT from SAP_COMPANY_ORG ";
                    _sql += string.Format(" where COMPANY_ID = '{0}' and ORG_ID not in {1} ", ERPID.Trim().Replace("'", "''"), WebConfigurationManager.AppSettings["InvalidOrg"]);
                    DataTable ORG = DBUtil.dbGetDataTable("MY", _sql);
                    if (ORG != null && ORG.Rows.Count > 0)
                    {
                        for (int i = 0; i <= ORG.Rows.Count - 1; i++)
                        {
                            ORGList.Add(new SelectListItem()
                            {
                                Text = ORG.Rows[i][1].ToString(),
                                Value = ORG.Rows[i][0].ToString(),
                                Selected = (bool)ORG.Rows[i][2]
                            });
                        }
                    }
                    else
                    {
                        ORGList.Add(new SelectListItem()
                        {
                            Text = SAPList.FirstOrDefault().ORG_ID.ToString(),
                            Value = SAPList.FirstOrDefault().ORG_ID.ToString(),
                            Selected = true
                        });
                    }
                }
                else if (SAPList.Count == 1)
                {
                    ORGList.Add(new SelectListItem()
                    {
                        Text = SAPList[0].ORG_ID,
                        Value = SAPList[0].ORG_ID
                    });
                }
                else
                {
                    //Default value
                    ORGList.Add(new SelectListItem()
                    {
                        Text = GetDefaultOrgByRBU(SiebelRBU),
                        Value = GetDefaultOrgByRBU(SiebelRBU)
                    });
                }
            }
            else
            {
                //Default value
                ORGList.Add(new SelectListItem()
                {
                    Text = GetDefaultOrgByRBU(SiebelRBU),
                    Value = GetDefaultOrgByRBU(SiebelRBU)
                });
            }

            return ORGList;
        }

        public static DataTable GetDefaultSalesFromSiebel(string AccountRowID)
        {
            var dt = new DataTable();
            if (!string.IsNullOrEmpty(AccountRowID))
            {
                String sql = String.Format(@" select b.id_sap as SALES_CODE, a.PRIMARY_SALES_EMAIL as EMAIL from SIEBEL_ACCOUNT a inner join EAI_IDMAP b on a.PRIMARY_SALES_EMAIL = b.id_email  inner join EZ_EMPLOYEE c on b.id_email = c.EMAIL_ADDR where a.ROW_ID = '{0}' order by b.id_sap", AccountRowID);
                dt = DBUtil.dbGetDataTable("MY", sql);
            }

            return dt;
        }

        public static DataTable GetDefaultSalesFromSAP(string ERPID)
        {
            var dt = new DataTable();
            if (!string.IsNullOrEmpty(ERPID))
            {
                String sql = String.Format(@" select b.id_sap as SALES_CODE, b.id_email as EMAIL from SAP_COMPANY_EMPLOYEE a inner join EAI_IDMAP b on a.SALES_CODE = b.id_sap  inner join EZ_EMPLOYEE c on b.id_email = c.EMAIL_ADDR 
                                              where a.COMPANY_ID = '{0}' and a.PARTNER_FUNCTION = 'VE'", ERPID);
                dt = DBUtil.dbGetDataTable("MY", sql);
            }

            return dt;
        }

        public static DataTable GetSalesInfo(string Email)
        {
            var DT = new DataTable();
            if (!string.IsNullOrEmpty(Email))
            {
                string _sql = "  select top 1 (isnull(FIRST_NAME,'') + ' ' + isnull(LAST_NAME,'')) as FULL_NAME,isnull(TEL,'') as TEL,isnull(EMAIL,'') as EMAIL from SAP_EMPLOYEE ";
                _sql += string.Format(" where EMAIL='{0}' ", Email.Trim().Replace("'", "''"));
                DT = DBUtil.dbGetDataTable("MY", _sql);
            }

            return DT;
        }

        public static DataTable GetSalesInfoBySalesCode(string salesCode)
        {
            var DT = new DataTable();
            if (!string.IsNullOrEmpty(salesCode))
            {
                string _sql = "  select top 1 (isnull(FIRST_NAME,'') + ' ' + isnull(LAST_NAME,'')) as FULL_NAME,isnull(TEL,'') as TEL,isnull(EMAIL,'') as EMAIL from SAP_EMPLOYEE ";
                _sql += string.Format(" where SALES_CODE ='{0}' ", salesCode.Trim().Replace("'", "''"));
                DT = DBUtil.dbGetDataTable("MY", _sql);
            }

            return DT;
        }

        public static String GetDefaultOrgByRBU(String _RBU)
        {
            switch (_RBU.ToUpper())
            {
                case "ACN":
                    return "CN10";
                case "ABB":
                    return "US10";
                case "ASG":
                    return "SG01";
                case "AAU":
                    return "AU01";
                case "ICG":
                    return "TW01";
                case "ADLOG":
                    return "EU80";
                default:
                    return "";
            }
        }
    }
}