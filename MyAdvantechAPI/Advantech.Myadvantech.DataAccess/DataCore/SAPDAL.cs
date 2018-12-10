using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Threading;
using System.Runtime.CompilerServices;
using GET_MATERIAL_ATP;
using System.Globalization;
using ZPP_BOM_EXPL_MAT_V2_RFC_CKD;
using CreateSAPContact;

[assembly: InternalsVisibleTo("Advantech.Myadvantech.Business")]
namespace Advantech.Myadvantech.DataAccess
{

    public class SAPDAL
    {

        public static string FormatItmNumber(int ItemNumber)
        {
            int Zeros = 6 - ItemNumber.ToString().Length;
            if (Zeros == 0)
                return ItemNumber.ToString();
            string strItemNumber = ItemNumber.ToString();
            for (int i = 0; i <= Zeros - 1; i++)
            {
                strItemNumber = "0" + strItemNumber;
            }
            return strItemNumber;
        }
        public static string RemovePrecedingZeros(string str)
        {
            if (!str.StartsWith("0"))
                return str;
            if (str.Length > 1)
            {
                return RemovePrecedingZeros(str.Substring(1));
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Convert part number format to SAP's format
        /// </summary>
        /// <param name="str">part number</param>
        /// <returns>part number in SAP format</returns>
        public static string FormatToSAPPartNo(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            decimal outdec = 0;
            str = RemovePrecedingZeros(str);
            Nullable<bool> IsNumericPart = default(Nullable<bool>);
            for (int i = 0; i <= str.Length - 1; i++)
            {
                if (!decimal.TryParse(str.Substring(i, 1), out outdec))
                {
                    IsNumericPart = false;
                    break;
                }
                else
                {
                    IsNumericPart = true;
                }
            }
            if (IsNumericPart == true)
            {
                while (str.Length < 18)
                {
                    str = "0" + str;
                }
            }
            return str;
        }
        public static string GetAHighLevelItemForPricing(string org)
        {
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(" select top 1 a.PART_NO from SAP_PRODUCT a inner join SAP_PRODUCT_STATUS b on a.PART_NO=b.PART_NO " + " where a.MATERIAL_GROUP='PRODUCT' and b.PRODUCT_STATUS='A' and b.SALES_ORG=@ORG and a.PRODUCT_LINE like 'ADM%' and a.PART_NO like 'ADAM-%' ", new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString));
            cmd.Parameters.AddWithValue("ORG", org);
            cmd.Connection.Open();
            object pnObject = cmd.ExecuteScalar();
            cmd.Connection.Close();
            if (pnObject != null)
            {
                return pnObject.ToString();
            }


            //Dictionary<string, string> dicPNOrg = (Dictionary<string, string>)HttpContext.Current.Cache["High Level Pricing PN Org Pair"];
            //if (dicPNOrg == null)
            //{
            //    dicPNOrg = new Dictionary<string, string>();
            //    HttpContext.Current.Cache.Add("High Level Pricing PN Org Pair", dicPNOrg, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            //}
            //if (!dicPNOrg.ContainsKey(org))
            //{
            //    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(" select top 1 a.PART_NO from SAP_PRODUCT a inner join SAP_PRODUCT_STATUS b on a.PART_NO=b.PART_NO " + " where a.MATERIAL_GROUP='PRODUCT' and b.PRODUCT_STATUS='A' and b.SALES_ORG=@ORG and a.PRODUCT_LINE like 'ADM%' and a.PART_NO like 'ADAM-%' ", new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString));
            //    cmd.Parameters.AddWithValue("ORG", org);
            //    cmd.Connection.Open();
            //    object pnObject = cmd.ExecuteScalar();
            //    cmd.Connection.Close();
            //    if (pnObject != null)
            //    {
            //        dicPNOrg.Add(org, pnObject.ToString());
            //    }
            //}
            //if (dicPNOrg.ContainsKey(org))
            //{
            //    return dicPNOrg[org];
            //}
            return "ADAM-4520-EE";
        }

        public static void UnblockSOCreditCard(string SONO, Boolean istesting)
        {
            SONO = FormatToSAPPartNo(SONO.Trim().ToUpper());
            string SAPRFCconnection = "SAP_PRD"; if (istesting) SAPRFCconnection = "SAPConnTest";
            string SAPDbconnection = "SAP_PRD"; if (istesting) SAPDbconnection = "SAP_Test";
            string sql = string.Format("select * from saprdp.fpla where vbeln='{0}'", SONO);

            //var dtFPLA = OraDbUtil.dbGetDataTable(SAPDbconnection, string.Format("select * from saprdp.fpla where vbeln='{0}'", SONO));
            var dtFPLA = OracleProvider.GetDataTable(SAPDbconnection, sql.ToString());

            if (dtFPLA.Rows.Count > 0)
            {
                System.Data.DataRow drFPLA = dtFPLA.Rows[0];
                var FPLA_NEW = new ZBILLING_SCHEDULE_SAVE.FPLAVB()
                {
                    Mandt = "168",
                    Fplnr = drFPLA["Fplnr"].ToString(),
                    Aust1 = "B",
                    Aust5 = "",
                    Updkz = "U",
                    Selkz = "",
                    Dfksaf = "",
                    Netwrp = (decimal)0.0
                };

                FPLA_NEW.Fptyp = drFPLA["Fptyp"].ToString();
                FPLA_NEW.Fpart = drFPLA["Fpart"].ToString(); FPLA_NEW.Sortl = drFPLA["Sortl"].ToString();
                FPLA_NEW.Bedat = drFPLA["Bedat"].ToString(); FPLA_NEW.Endat = drFPLA["Endat"].ToString();
                FPLA_NEW.Horiz = drFPLA["Horiz"].ToString(); FPLA_NEW.Vbeln = drFPLA["Vbeln"].ToString();
                FPLA_NEW.Bedar = drFPLA["Bedar"].ToString(); FPLA_NEW.Endar = drFPLA["Endar"].ToString();
                FPLA_NEW.Perio = drFPLA["Perio"].ToString(); FPLA_NEW.Fplae = drFPLA["Fplae"].ToString();
                FPLA_NEW.Rfpln = drFPLA["Rfpln"].ToString(); FPLA_NEW.Lodat = drFPLA["Lodat"].ToString();
                FPLA_NEW.Autte = drFPLA["Autte"].ToString(); FPLA_NEW.Lodar = drFPLA["Lodar"].ToString();
                FPLA_NEW.Peraf = drFPLA["Peraf"].ToString(); FPLA_NEW.Fakca = drFPLA["Fakca"].ToString();
                FPLA_NEW.Tndat = drFPLA["Tndat"].ToString(); FPLA_NEW.Tndar = drFPLA["Tndar"].ToString();
                FPLA_NEW.Aufpl = drFPLA["Aufpl"].ToString(); FPLA_NEW.Aplzl = drFPLA["Aplzl"].ToString();
                FPLA_NEW.Rsnum = drFPLA["Rsnum"].ToString(); FPLA_NEW.Rspos = drFPLA["Rspos"].ToString();
                FPLA_NEW.Ebeln = drFPLA["Ebeln"].ToString(); FPLA_NEW.Fpltu = drFPLA["Fpltu"].ToString();
                FPLA_NEW.Aust2 = drFPLA["Aust2"].ToString(); FPLA_NEW.Aust3 = drFPLA["Aust3"].ToString();
                FPLA_NEW.Aust4 = drFPLA["Aust4"].ToString();
                if (drFPLA["Basiswrt"] != DBNull.Value) FPLA_NEW.Basiswrt = decimal.Parse(drFPLA["Basiswrt"].ToString());
                FPLA_NEW.Pspnr = drFPLA["Pspnr"].ToString();
                FPLA_NEW.Autkor = drFPLA["Autkor"].ToString();

                var FPLA_NEW_Table1 = new ZBILLING_SCHEDULE_SAVE.FPLAVBTable();
                FPLA_NEW_Table1.Add(FPLA_NEW);
                var FPLA_OLD_Table1 = new ZBILLING_SCHEDULE_SAVE.FPLAVBTable();
                var FPLT_NEW_Table1 = new ZBILLING_SCHEDULE_SAVE.FPLTVBTable();
                var FPLT_OLD_Table1 = new ZBILLING_SCHEDULE_SAVE.FPLTVBTable();
                var RFCClient1 = new ZBILLING_SCHEDULE_SAVE.ZBILLING_SCHEDULE_SAVE();
                RFCClient1.Connection = new SAP.Connector.SAPConnection(System.Configuration.ConfigurationManager.AppSettings[SAPRFCconnection]);
                RFCClient1.Connection.Open();
                RFCClient1.Zbilling_Schedule_Save(ref FPLA_NEW_Table1, ref FPLA_OLD_Table1, ref FPLT_NEW_Table1, ref FPLT_OLD_Table1);
                RFCClient1.Connection.Close();
            }

        }

        public static void AddCreditCardInfo2SAPSO(string SONO, string AuthCode, string TranId, string CardType, string CardNum, decimal AuthAmt, bool isTesting)
        {
            string SAPRFCconnection = "SAP_PRD"; if (isTesting) SAPRFCconnection = "SAPConnTest";
            string SAPDbconnection = "SAP_PRD"; if (isTesting) SAPDbconnection = "SAP_Test";
            var dtFPLT = OracleProvider.GetDataTable(SAPDbconnection, string.Format(
                @"
                select b.fplnr, b.fpltr, b.fpttp, b.tetxt, b.fkdat, b.fpfix, b.fareg, b.fproz, b.waers, b.kurfp, b.fakwr
                , b.faksp, b.fkarv, b.fksaf, b.perio, b.fplae, b.mlstn, b.mlbez, b.zterm, b.kunrg,
                b.taxk1, b.taxk2, b.taxk3, b.taxk4, b.taxk5, b.taxk6, b.taxk7, b.taxk8, b.taxk9
                , b.valdt, b.nfdat, b.teman, b.fakca, b.afdat, b.netwr, b.netpr, b.wavwr, 
                b.kzwi1, b.kzwi2, b.kzwi3, b.kzwi4, b.kzwi5, b.kzwi6, b.cmpre, b.skfbp, b.bonba, b.prsok
                , b.typzm, b.cmpre_flt, b.uelnr, b.ueltr, b.kurrf, b.ccact, b.korte, b.ofkdat, b.perop_beg, b.perop_end,
                c.ccins, c.ccnum, c.ccfol, c.datab, c.datbi, c.ccname, c.csour, c.autwr, c.ccwae, c.settl, c.aunum, c.autra
                , c.audat, c.autim, c.merch, c.locid, c.trmid, c.ccbtc, c.cctyp, c.ccard_guid,
                'A' as CCAUA, 'C' as CCALL, 'A' as REACT, c.autwv, c.ccold, c.ccval, c.ccpre
                , c.ueltr_a, c.rcavr, c.rcava, c.rcavz, c.rcrsp, c.rtext
                from saprdp.fpla a inner join saprdp.fplt b on a.fplnr=b.fplnr
                inner join saprdp.fpltc c on b.fplnr=c.fplnr and b.fpltr=c.fpltr
                where a.mandt='168' and b.mandt='168' and c.mandt='168' and a.vbeln='{0}' 
                order by b.fpltr ", SONO));

            if (dtFPLT.Rows.Count > 0)
            {
                DataRow drFPLT = dtFPLT.Rows[dtFPLT.Rows.Count - 1];
                var FPLT_NEW = new ZBILLING_SCHEDULE_SAVE.FPLTVB()
                {
                    Mandt = "168",
                    Fplnr = drFPLT["FPLNR"].ToString(),
                    Fpltr = drFPLT["FPLTR"].ToString(),
                    Fpttp = drFPLT["FPTTP"].ToString(),
                    Tetxt = drFPLT["TETXT"].ToString(),
                    Fkdat = drFPLT["FKDAT"].ToString(),
                    Fpfix = drFPLT["FPFIX"].ToString(),
                    Fareg = drFPLT["FAREG"].ToString(),
                    Fproz = decimal.Parse(drFPLT["FPROZ"].ToString()),
                    Waers = drFPLT["WAERS"].ToString(),
                    Kurfp = decimal.Parse(drFPLT["KURFP"].ToString()),
                    Fakwr = decimal.Parse(drFPLT["FAKWR"].ToString()),
                    Faksp = drFPLT["FAKSP"].ToString(),
                    Fkarv = drFPLT["FKARV"].ToString(),
                    Fksaf = drFPLT["FKSAF"].ToString(),
                    Perio = drFPLT["PERIO"].ToString(),
                    Fplae = drFPLT["FPLAE"].ToString(),
                    Mlstn = drFPLT["MLSTN"].ToString(),
                    Mlbez = drFPLT["MLBEZ"].ToString(),
                    Zterm = drFPLT["ZTERM"].ToString(),
                    Kunrg = drFPLT["KUNRG"].ToString(),
                    Taxk1 = drFPLT["TAXK1"].ToString(),
                    Taxk2 = drFPLT["TAXK2"].ToString(),
                    Taxk3 = drFPLT["TAXK3"].ToString(),
                    Taxk4 = drFPLT["TAXK4"].ToString(),
                    Taxk5 = drFPLT["TAXK5"].ToString(),
                    Taxk6 = drFPLT["TAXK6"].ToString(),
                    Taxk7 = drFPLT["TAXK7"].ToString(),
                    Taxk8 = drFPLT["TAXK8"].ToString(),
                    Taxk9 = drFPLT["TAXK9"].ToString(),
                    Valdt = drFPLT["VALDT"].ToString(),
                    Nfdat = drFPLT["NFDAT"].ToString(),
                    Teman = drFPLT["TEMAN"].ToString(),
                    Fakca = drFPLT["FAKCA"].ToString(),
                    Afdat = drFPLT["AFDAT"].ToString(),
                    Netwr = decimal.Parse(drFPLT["NETWR"].ToString()),
                    Netpr = decimal.Parse(drFPLT["NETPR"].ToString()),
                    Wavwr = decimal.Parse(drFPLT["WAVWR"].ToString()),
                    Kzwi1 = decimal.Parse(drFPLT["KZWI1"].ToString()),
                    Kzwi2 = decimal.Parse(drFPLT["KZWI2"].ToString()),
                    Kzwi3 = decimal.Parse(drFPLT["KZWI3"].ToString()),
                    Kzwi4 = decimal.Parse(drFPLT["KZWI4"].ToString()),
                    Kzwi5 = decimal.Parse(drFPLT["KZWI5"].ToString()),
                    Kzwi6 = decimal.Parse(drFPLT["KZWI6"].ToString()),
                    Cmpre = decimal.Parse(drFPLT["CMPRE"].ToString()),
                    Skfbp = decimal.Parse(drFPLT["SKFBP"].ToString()),
                    Bonba = decimal.Parse(drFPLT["BONBA"].ToString()),
                    Prsok = drFPLT["PRSOK"].ToString(),
                    Typzm = drFPLT["TYPZM"].ToString(),
                    Cmpre_Flt = double.Parse(drFPLT["CMPRE_FLT"].ToString()),
                    Uelnr = drFPLT["UELNR"].ToString(),
                    Ueltr = drFPLT["UELTR"].ToString(),
                    Kurrf = decimal.Parse(drFPLT["KURRF"].ToString()),
                    Ccact = drFPLT["CCACT"].ToString(),
                    Korte = drFPLT["KORTE"].ToString(),
                    Ofkdat = drFPLT["OFKDAT"].ToString(),
                    Perop_Beg = drFPLT["PEROP_BEG"].ToString(),
                    Perop_End = drFPLT["PEROP_END"].ToString(),
                    Ccins = drFPLT["CCINS"].ToString(),
                    Ccnum = drFPLT["CCNUM"].ToString(),
                    Ccfol = drFPLT["CCFOL"].ToString(),
                    Datab = drFPLT["DATAB"].ToString(),
                    Datbi = drFPLT["DATBI"].ToString(),
                    Ccname = drFPLT["CCNAME"].ToString(),
                    Csour = drFPLT["CSOUR"].ToString(),
                    Autwr = decimal.Parse(drFPLT["AUTWR"].ToString()),
                    Ccwae = drFPLT["CCWAE"].ToString(),
                    Settl = drFPLT["SETTL"].ToString(),
                    Aunum = drFPLT["AUNUM"].ToString(),
                    Autra = drFPLT["AUTRA"].ToString(),
                    Audat = drFPLT["AUDAT"].ToString(),
                    Autim = drFPLT["AUTIM"].ToString(),
                    Merch = drFPLT["MERCH"].ToString(),
                    Locid = drFPLT["LOCID"].ToString(),
                    Trmid = drFPLT["TRMID"].ToString(),
                    Ccbtc = drFPLT["CCBTC"].ToString(),
                    Cctyp = drFPLT["CCTYP"].ToString(),
                    Ccard_Guid = drFPLT["CCARD_GUID"].ToString(),
                    Ccaua = drFPLT["CCAUA"].ToString(),
                    Ccall = drFPLT["CCALL"].ToString(),
                    React = drFPLT["REACT"].ToString(),
                    Autwv = decimal.Parse(drFPLT["AUTWV"].ToString()),
                    Ccold = drFPLT["CCOLD"].ToString(),
                    Ccval = drFPLT["CCVAL"].ToString(),
                    Ccpre = drFPLT["CCPRE"].ToString(),
                    Ueltr_A = drFPLT["UELTR_A"].ToString(),
                    Rcavr = drFPLT["RCAVR"].ToString(),
                    Rcava = drFPLT["RCAVA"].ToString(),
                    Rcavz = drFPLT["RCAVZ"].ToString(),
                    Rcrsp = drFPLT["RCRSP"].ToString(),
                    Rtext = drFPLT["RTEXT"].ToString(),
                    Updkz = "I",
                    Selkz = ""
                };
                int Max_Fpltr = int.Parse(FPLT_NEW.Fpltr) + 1;
                FPLT_NEW.Fpltr = Max_Fpltr.ToString();
                //FPLT_NEW.Ccaua = "A"; FPLT_NEW.Ccall = "C"; FPLT_NEW.React = "A";
                FPLT_NEW.Ccins = CardType.ToString();
                FPLT_NEW.Ccnum = CardNum;
                FPLT_NEW.Aunum = AuthCode;
                FPLT_NEW.Autra = TranId;
                FPLT_NEW.Autwr = AuthAmt;
                FPLT_NEW.Fksaf = "A";

                var FPLA_NEW_Table1 = new ZBILLING_SCHEDULE_SAVE.FPLAVBTable();
                var FPLA_OLD_Table1 = new ZBILLING_SCHEDULE_SAVE.FPLAVBTable();
                var FPLT_NEW_Table1 = new ZBILLING_SCHEDULE_SAVE.FPLTVBTable();
                var FPLT_OLD_Table1 = new ZBILLING_SCHEDULE_SAVE.FPLTVBTable();
                FPLT_NEW_Table1.Add(FPLT_NEW);
                var RFCClient1 = new ZBILLING_SCHEDULE_SAVE.ZBILLING_SCHEDULE_SAVE();
                RFCClient1.Connection = new SAP.Connector.SAPConnection(System.Configuration.ConfigurationManager.AppSettings[SAPRFCconnection]);
                RFCClient1.Connection.Open();
                RFCClient1.Zbilling_Schedule_Save(ref FPLA_NEW_Table1, ref FPLA_OLD_Table1, ref FPLT_NEW_Table1, ref FPLT_OLD_Table1);
                RFCClient1.Connection.Close();
            }
        }

        /// <summary>
        /// Get Account PO Number
        /// </summary>
        /// <param name="ORGID"></param>
        /// <param name="ERPID"></param>
        /// <param name="PO"></param>
        /// <returns></returns>
        public static DataTable GetAccountPONumber(string ORGID, string ERPID, string PO)
        {

            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" select vbeln from saprdp.vbak where KNKLI='" + ERPID.Replace("'", "''") + "' and BSTNK='" + PO.Replace("'", "''") + "' and rownum<=20 and vkorg='" + ORGID.Replace("'", "''") + "'");
            sql.AppendLine("  and auart in ('ZOR','ZOR2') order by erdat desc ");

            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", sql.ToString());

            return dt;
        }

        public static DataTable GetSalesGroupOfficeDivisionDistrictByERPID(String _ERPID)
        {
            _ERPID = _ERPID.ToUpper().Trim();
            string strSql = " select E.VKBUR as SalesOffice, E.VKGRP as SalesGroup, E.SPART as division, E.BZIRK as District " + " from saprdp.knvv E " + " where rownum<=30 and E.mandt='168' and E.kunnr='" + _ERPID + "'";
            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", strSql);
            return dt;
        }

        public static DataTable ExpandSystemPartToBOM(string SystemPartNo, int MaxLevel)
        {
            ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZPP_BOM_EXPL_MAT_V2_RFC_CKD MyComBom =
                new ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZPP_BOM_EXPL_MAT_V2_RFC_CKD();
            DataTable BOMTable = new DataTable();

            try
            {
                MyComBom.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings["SAP_PRD"]);
                MyComBom.Connection.Open();
                SAPDAL.ExpandBOM(SystemPartNo.ToUpper(), "TWH1", 1, MaxLevel, ref MyComBom, ref BOMTable);
            }
            catch (Exception e)
            {
            }
            finally
            {
                MyComBom.Connection.Close();
            }
            return BOMTable;
        }

        /// <summary>
        /// Expand BOM
        /// </summary>
        /// <param name="ParentItem"></param>
        /// <param name="Plant"></param>
        /// <param name="CurrLevel"></param>
        /// <param name="MaxLevel"></param>
        /// <param name="RFCProxy"></param>
        /// <param name="dtBOM"></param>
        public static void ExpandBOM(string ParentItem, String Plant, int CurrLevel, int MaxLevel, ref ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZPP_BOM_EXPL_MAT_V2_RFC_CKD RFCProxy, ref DataTable dtBOM)
        {

            if (CurrLevel > MaxLevel) { return; }

            ParentItem = FormatToSAPPartNo(ParentItem);
            string strErr = string.Empty; //string Plant = "TWH1";
            ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60Table BOMTable = new ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60Table();
            if (MaxLevel == 1)
            {
                RFCProxy.Zpp_Bom_Expl_Mat_V2_Rfc("", "", ParentItem, Plant, out strErr, ref BOMTable);
                dtBOM = BOMTable.ToADODataTable();
                return;
            }
            else
            {
                //多層次的待測
                RFCProxy.Zpp_Bom_Expl_Mat_V2_Rfc("", "X", ParentItem, Plant, out strErr, ref BOMTable);
            }

            decimal BOMMaxLevel = 1;
            foreach (ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60 BomRow in BOMTable)
            {
                if (BomRow.Stufe > BOMMaxLevel) { BOMMaxLevel = BomRow.Stufe; }
                //Frank:If user defines max level, then set it to max level for bom expanding
                if (MaxLevel > 0 && BOMMaxLevel > MaxLevel) { BOMMaxLevel = MaxLevel; }
            }

            for (int CurrentProcLevel = 1; CurrentProcLevel <= MaxLevel; CurrentProcLevel++)
            {
                for (int BomIdx = 0; BomIdx < BOMTable.Count; BomIdx++)
                {
                    ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60 BomRow = BOMTable[BomIdx];
                    if (BomRow.Stufe == CurrentProcLevel + 1 & BomRow.Mandt == ParentItem)
                    {
                        int currentFindIdx = BomIdx;
                        while (currentFindIdx >= 0)
                        {
                            currentFindIdx -= 1;
                            ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60 prevBomRow = BOMTable[currentFindIdx];
                            if (prevBomRow.Stufe == CurrentProcLevel)
                            {
                                BomRow.Matnr = prevBomRow.Idnrk;
                                break;
                            }
                        }
                    }
                }

            }

            dtBOM = BOMTable.ToADODataTable();


            List<ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60> BomList = new List<ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60>();
            foreach (ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60 BomRow in BOMTable)
            {
                BomList.Add(BomRow);
            }


            foreach (ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60 BomRow in BOMTable)
            {
                //If no children then get child bom, otherwise don't get to avoid duplicate
                var hasChild = from q in BomList
                               where q.Matnr == BomRow.Idnrk
                               select q;

                if (hasChild.Count() == 0)
                {
                    DataTable BOMTableChild = new DataTable();
                    SAPDAL.ExpandBOM(BomRow.Idnrk, "", Convert.ToInt32(BomRow.Stufe), MaxLevel, ref RFCProxy, ref BOMTableChild);
                    foreach (DataRow childRow in BOMTableChild.Rows)
                    {
                        childRow["Stufe"] = Convert.ToDouble(childRow["Stufe"]);
                    }
                    dtBOM.Merge(BOMTableChild);
                }

            }


        }

        /// <summary>
        /// Get sales office code by sales id
        /// </summary>
        /// <param name="SalesCode">SalesCode</param>
        /// <returns></returns>
        public static String GetOfficeCodeBySalesID(String SalesCode)
        {

            StringBuilder chkSql = new StringBuilder();
            chkSql.AppendLine(" Select SALESOFFICE from SAP_EMPLOYEE ");
            chkSql.AppendLine(" Where SALES_CODE=@SALES_CODE ");
            String _SalesOffice = String.Empty;
            try
            {

                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(chkSql.ToString(),
                    new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString));
                cmd.Parameters.AddWithValue("SALES_CODE", SalesCode);
                cmd.Connection.Open();
                object officeObject = cmd.ExecuteScalar();
                cmd.Connection.Close();

                if (officeObject != null)
                {
                    _SalesOffice = officeObject.ToString();
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
            }
            finally
            {
            }

            return _SalesOffice;

        }

        /// <summary>
        /// Check BSMI Parts
        /// </summary>
        /// <param name="partnos"></param>
        /// <returns></returns>
        public static DataTable CheckBSMIParts(String[] partnos)
        {
            if (partnos == null || partnos.Length == 0)
            {
                return null;
            }

            try
            {
                string _stringin = string.Empty;

                foreach (string _partno in partnos)
                {
                    _stringin += "'" + _partno + "',";
                }
                _stringin = _stringin.Remove(_stringin.Length - 1);

                StringBuilder strSql = new StringBuilder();
                //chkSql.AppendLine(" Select  a.[id_sap] as sale_id, a.[id_sector], a.id_sbu, a.id_fact_entity, b.SALES_CODE, b.FULL_NAME, b.EMAIL ");
                //chkSql.AppendLine(" From EAI_IDMAP a inner join SAP_EMPLOYEE b on a.id_sap=b.SALES_CODE ");

                strSql.AppendLine(" Select a.ITEM_NUMBER ");
                strSql.AppendLine(" ,(Select Z.NAME from agile.NODETABLE Z where a.SUBCLASS = Z.ID) as Part_Cat ");
                strSql.AppendLine(" ,(Select d.ENTRYVALUE from agile.LISTENTRY d where a.list17 = d.ENTRYID and d.LANGID = 0) as BSMI_Certification ");
                strSql.AppendLine(" ,to_char(b.RELEASE_DATE + 8 / 24, 'YYYY/MM/DD HH24:Mi:SS') as Rev_RELEASE_DATE ");
                strSql.AppendLine(" from agile.item_p2 a, agile.rev b ");
                strSql.AppendLine(" where a.class= '10000' ");
                strSql.AppendLine(" and a.id = b.ITEM ");
                strSql.AppendLine(" and b.LATEST_FLAG = 1 ");
                strSql.AppendLine(" and a.list17 = 2475275 "); // --BSMI 選項三的 List ID
                strSql.AppendLine(" and b.RELEASE_DATE+8/24 >= SYSDATE-1 ");
                strSql.AppendLine(" and a.ITEM_NUMBER in (" + _stringin + ") ");

                DataTable dt = OracleProvider.GetDataTable("PLM_PRD", strSql.ToString());
                return dt;
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// Get Sales Employee list for Aonline's quote/order
        /// </summary>
        /// <param name="region"></param>
        /// <param name="ERPID"></param>
        /// <returns></returns>
        public static DataTable GetSalesEmployee(AOnlineRegion region, string ERPID)
        {
            StringBuilder chkSql = new StringBuilder();
            chkSql.AppendLine(" Select  a.[id_sap] as sale_id, a.[id_sector], a.id_sbu, a.id_fact_entity, b.SALES_CODE, b.FULL_NAME, b.EMAIL ");
            chkSql.AppendLine(" From EAI_IDMAP a inner join SAP_EMPLOYEE b on a.id_sap=b.SALES_CODE ");
            string orderby = string.Empty;
            string ORGID = string.Empty;
            //ICC 2015/1/6 For AUS AOnline & AOnline_IAG, we use sales office to distinguish these two groups.
            string salesOfiice = string.Empty;
            string salesGroup = string.Empty;
            //Dim STR As String = "SELECT top 1 sales_code FROM SAP_COMPANY_EMPLOYEE WHERE SALES_ORG='" & ORGID & "' AND PARTNER_FUNCTION='VE' AND COMPANY_ID='" & ERPID & "'"
            switch (region)
            {
                case AOnlineRegion.AUS_AOnline:
                    orderby = " FULL_NAME ";
                    ORGID = "US01";
                    salesOfiice = "2700";
                    //chkSql.AppendLine(" Where (a.id_fact_zone='North America' and a.id_sector like '%AOnline%' ");
                    //chkSql.AppendFormat(" and a.id_rbu='AOnline' and b.SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='USAOnline') ");
                    chkSql.Clear();
                    chkSql.AppendFormat(" Select SALES_CODE, FULL_NAME  from  SAP_EMPLOYEE WHERE SALESOFFICE = '{0}' AND PERS_AREA = '{1}' ", salesOfiice, ORGID);
                    chkSql.AppendLine(" and SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='USAOnline') ");
                    chkSql.AppendLine(" Order By " + orderby);
                    break;
                case AOnlineRegion.AUS_AOnline_IAG:
                    orderby = " FULL_NAME ";
                    ORGID = "US01";
                    salesOfiice = "2110";
                    salesGroup = "219";
                    //chkSql.AppendLine(" Where (a.id_fact_zone='North America' and a.id_sector like '%AOnline%' ");
                    //chkSql.AppendFormat(" and a.id_rbu='IA-AOnline' and b.SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='USAOnline') ");
                    chkSql.Clear();
                    chkSql.AppendFormat(" Select SALES_CODE, FULL_NAME  from  SAP_EMPLOYEE WHERE SALESOFFICE = '{0}' AND PERS_AREA = '{1}' AND SALESGROUP='{2}' ", salesOfiice, ORGID, salesGroup);
                    chkSql.AppendLine(" and SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='USAOnline') ");
                    chkSql.AppendLine(" Order By " + orderby);
                    break;
                case AOnlineRegion.AUS_AOnline_iSystem:
                    orderby = " FULL_NAME ";
                    ORGID = "US01";
                    salesOfiice = "2110";
                    salesGroup = "21A";
                    //chkSql.AppendLine(" Where (a.id_fact_zone='North America' and a.id_sector like '%AOnline%' ");
                    //chkSql.AppendFormat(" and a.id_rbu='IA-AOnline' and b.SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='USAOnline') ");
                    chkSql.Clear();
                    chkSql.AppendFormat(" Select SALES_CODE, FULL_NAME  from  SAP_EMPLOYEE WHERE SALESOFFICE = '{0}' AND PERS_AREA = '{1}' AND SALESGROUP='{2}' ", salesOfiice, ORGID, salesGroup);
                    chkSql.AppendLine(" and SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='USAOnline') ");
                    chkSql.AppendLine(" Order By " + orderby);
                    break;
                case AOnlineRegion.AUS_AENC:
                    orderby = " FULL_NAME ";
                    ORGID = "US01";
                    salesOfiice = "2300";
                    //chkSql.AppendLine(" Where (a.id_fact_zone='North America' and a.id_sector like '%AOnline%' ");
                    //chkSql.AppendFormat(" and a.id_rbu='IA-AOnline' and b.SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='USAOnline') ");
                    chkSql.Clear();
                    chkSql.AppendFormat(" Select SALES_CODE, FULL_NAME  from  SAP_EMPLOYEE WHERE SALESOFFICE = '{0}' AND PERS_AREA = '{1}' ", salesOfiice, ORGID);
                    chkSql.AppendLine(" and SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='USAENC') ");
                    chkSql.AppendLine(" Order By " + orderby);
                    break;
                case AOnlineRegion.ATW:
                    orderby = " b.SALES_CODE ";
                    ORGID = "TW01";
                    chkSql.AppendLine(" Where (a.id_fact_zone='Taiwan' ");
                    chkSql.AppendLine("    and b.SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='TW') ");
                    chkSql.AppendLine(" ) ");
                    chkSql.AppendLine(" or b.SALES_CODE in (SELECT top 1 sales_code FROM SAP_COMPANY_EMPLOYEE WHERE SALES_ORG='" + ORGID + "' AND PARTNER_FUNCTION='VE' AND COMPANY_ID='" + ERPID + "') ");
                    chkSql.AppendLine(" Order By " + orderby);
                    break;
                case AOnlineRegion.ATW_AOnline:
                    orderby = " b.SALES_CODE ";
                    ORGID = "TW01";
                    chkSql.AppendLine(" Where (a.id_fact_zone='Taiwan' and a.id_sector like '%AOnline%' ");
                    chkSql.AppendLine("    and b.SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST Where [GROUP]='TWAOnline') ");
                    chkSql.AppendLine(" ) ");
                    chkSql.AppendLine(" or b.SALES_CODE in (SELECT top 1 sales_code FROM SAP_COMPANY_EMPLOYEE WHERE SALES_ORG='" + ORGID + "' AND PARTNER_FUNCTION='VE' AND COMPANY_ID='" + ERPID + "') ");
                    chkSql.AppendLine(" Order By " + orderby);
                    break;
                default:
                    return null;
            }
            //ICC 2015/1/6 Move up these sql script.
            //chkSql.AppendLine("    and b.SALES_CODE not in (select SALES_CODE FROM SAP_EMPLOYEE_EXCLUDELIST) ");
            //chkSql.AppendLine(" ) ");
            //chkSql.AppendLine(" or b.SALES_CODE in (SELECT top 1 sales_code FROM SAP_COMPANY_EMPLOYEE WHERE SALES_ORG='" + ORGID + "' AND PARTNER_FUNCTION='VE' AND COMPANY_ID='" + ERPID + "') ");
            //chkSql.AppendLine(" Order By " + orderby);

            System.Data.SqlClient.SqlConnection sqlMA = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
            DataTable chkDt = new DataTable();
            System.Data.SqlClient.SqlDataAdapter sqlAptr = new System.Data.SqlClient.SqlDataAdapter(chkSql.ToString(), sqlMA);
            try
            {
                sqlAptr.Fill(chkDt);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                //errorMsg = ex.ToString();
                //return false;
            }
            finally
            {
                sqlMA.Close();
            }

            return chkDt;

        }

        /// <summary>
        /// Get part's inventory information
        /// </summary>
        /// <param name="_Parts"></param>
        /// <param name="PostponeDays"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static List<Inventory> GetInventory(List<Part> _Parts, int PostponeDays, ref string errorMsg)
        {

            GET_MATERIAL_ATP.GET_MATERIAL_ATP BAPIGetInventory =
                new GET_MATERIAL_ATP.GET_MATERIAL_ATP(ConfigurationManager.AppSettings["SAP_PRD"]);
            BAPIGetInventory.Connection.Open();

            List<Inventory> _returnval = new List<Inventory>();
            try
            {

                foreach (Part _Part in _Parts)
                {

                    GET_MATERIAL_ATP.BAPIRETURN _BAPIRETURN = new GET_MATERIAL_ATP.BAPIRETURN();
                    GET_MATERIAL_ATP.BAPIWMDVSTable retTb = new GET_MATERIAL_ATP.BAPIWMDVSTable();
                    GET_MATERIAL_ATP.BAPIWMDVETable atpTb = new GET_MATERIAL_ATP.BAPIWMDVETable();
                    short _s1 = 0;
                    decimal _Out1 = 0;
                    string _Out2 = string.Empty;
                    string _EndLeadTime = string.Empty;

                    //call Bapi_Material_Availability to get inventory information
                    BAPIGetInventory.Bapi_Material_Availability("", "A", "", _s1, "", "", ""
                        , FormatToSAPPartNo(_Part.PartNumber), _Part.PlantID.ToUpper(), "", "", "", "", "PC", ""
                        , out _Out1, out _Out2, out _EndLeadTime, out _BAPIRETURN, ref atpTb, ref retTb);

                    foreach (GET_MATERIAL_ATP.BAPIWMDVE _row in atpTb)
                    {

                        DateTime theDate;
                        bool result = DateTime.TryParseExact(
                            _row.Com_Date,
                            "yyyyMMdd",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out theDate);

                        if (result)
                        {
                            theDate = theDate.AddDays(PostponeDays);
                        }

                        Inventory _inven = new Inventory();
                        _inven.Plant = _Part.PlantID.ToUpper();
                        _inven.PartNumber = _Part.PartNumber;
                        _inven.AvailableDate = theDate;
                        _inven.AvailableQty = Convert.ToInt32(_row.Com_Qty);

                        _returnval.Add(_inven);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
            }
            finally
            {
                BAPIGetInventory.Connection.Close();
            }

            return _returnval;
        }

        public static PriceATP GetPriceATP(String _PartNo, String _CompanyID, String _ORGID, String _Plant, String _Currency, int _ConfigQty)
        {
            try
            {
                DateTime due_date = DateTime.MaxValue;
                Decimal culQty = 0;
                Decimal QtyMeetReqAtp = 0;

                if (_PartNo.ToUpper().StartsWith("AGS-EW"))
                {
                    due_date = DateTime.Now;
                }
                else
                {
                    GET_MATERIAL_ATP.GET_MATERIAL_ATP p1 = new GET_MATERIAL_ATP.GET_MATERIAL_ATP(ConfigurationManager.AppSettings["SAP_PRD"]);
                    DateTime ret_date = DateTime.Now.AddDays(-1);
                    int retqty = 0;
                    short s1 = 0;
                    decimal out1 = 0;
                    string out2 = string.Empty;
                    string endLeadTime = string.Empty;
                    _PartNo = FormatToSAPPartNo(_PartNo.ToUpper().Trim());
                    GET_MATERIAL_ATP.BAPIWMDVSTable retTb = new GET_MATERIAL_ATP.BAPIWMDVSTable();
                    GET_MATERIAL_ATP.BAPIWMDVETable atpTb = new GET_MATERIAL_ATP.BAPIWMDVETable();
                    GET_MATERIAL_ATP.BAPIWMDVS rOfretTb = new GET_MATERIAL_ATP.BAPIWMDVS();
                    GET_MATERIAL_ATP.BAPIRETURN BAPIRETURN = new GET_MATERIAL_ATP.BAPIRETURN();
                    rOfretTb.Req_Qty = 999;
                    rOfretTb.Req_Date = DateTime.Now.ToString("yyyyMMdd");
                    retTb.Add(rOfretTb);
                    p1.Connection.Open();
                    p1.Bapi_Material_Availability("", "A", "", s1, "", "", "", _PartNo, _Plant,
                                              "", "", "", "", "PC", "", out out1, out out2, out endLeadTime, out BAPIRETURN, ref atpTb, ref retTb);
                    p1.Connection.Close();

                    foreach (GET_MATERIAL_ATP.BAPIWMDVE atpRec in atpTb)
                    {
                        if (atpRec.Com_Qty > 0)
                        {
                            culQty += atpRec.Com_Qty;
                            if (culQty >= _ConfigQty && due_date == DateTime.MaxValue)
                            {
                                DateTime.TryParseExact(atpRec.Com_Date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out due_date);
                                QtyMeetReqAtp = culQty;
                            }
                        }
                    }
                }

                if (due_date == DateTime.MaxValue)
                {
                    due_date = DateTime.Now.AddDays(GetLeadTime(_PartNo, _Plant));
                }

                PriceATP result = new PriceATP();
                if (QtyMeetReqAtp > 9999)
                    QtyMeetReqAtp = 9999;

                if (_PartNo.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase))
                {
                    var EWRate = SqlProvider.dbExecuteScalar("MY", String.Format(" select top 1 EW_Rate from ExtendedWarrantyPartNo_V2 where EW_PartNO = '{0}' and Plant = '{1}'", _PartNo, _Plant));

                    if (EWRate != null && !string.IsNullOrEmpty(EWRate.ToString()))
                        result.Price = (Convert.ToDecimal(EWRate) * 100).ToString() + "%";
                    else
                        result.Price = "";

                    result.ATPDate = "";
                    result.ATPQty = 0;
                }
                else
                {
                    result.Price = GetPriceByPartNo(_PartNo, _CompanyID, _ORGID, _Plant, _Currency).ToString();
                    result.ATPDate = due_date.ToString("yyyy/MM/dd");
                    result.ATPQty = Convert.ToInt32(QtyMeetReqAtp);
                }

                return result;
            }
            catch (Exception e)
            {

            }
            return new PriceATP();
        }

        public static Decimal GetPriceByPartNo(String _PartNo, String _CompanyID, String _ORGID, String _Plant, String _Currency)
        {
            Decimal price = 0;

            Order _order = new Order();
            _order.Currency = _Currency;
            _order.OrgID = _ORGID;
            _order.DistChannel = "10";
            _order.Division = "00";
            if (_ORGID.StartsWith("CN"))
            {
                if (System.Configuration.ConfigurationManager.AppSettings["ACNTaxRate"] != null)
                    _order.Tax = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["ACNTaxRate"]);
                else
                    _order.Tax = (Decimal)0.16;
            }

            _order.AddLooseItem(_PartNo, _Plant, 1, 1);
            _order.SetOrderPartnet(new OrderPartner(_CompanyID, _ORGID, OrderPartnerType.SoldTo));
            _order.SetOrderPartnet(new OrderPartner(_CompanyID, _ORGID, OrderPartnerType.ShipTo));
            _order.SetOrderPartnet(new OrderPartner(_CompanyID, _ORGID, OrderPartnerType.BillTo));

            String _errMsg = String.Empty;
            Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref _order, ref _errMsg);

            if (_order != null && String.IsNullOrEmpty(_errMsg))
            {
                price = _order.LineItems[0].UnitPrice;
            }

            return price;
        }

        public static int GetLeadTime(String _PartNo, String _Plant)
        {
            int N = 0;
            string str = string.Format("select (PLANNED_DEL_TIME + GP_PROCESSING_TIME) as d from dbo.SAP_PRODUCT_ABC where PART_NO='{0}' AND PLANT='{1}'", _PartNo, _Plant);
            DataTable dt = new DataTable();
            dt = SqlProvider.dbGetDataTable("MY", str);
            if (dt.Rows.Count > 0)
            {
                Int32.TryParse(dt.Rows[0]["d"].ToString(), out N);
            }
            return N;
        }

        public static string GetCalendarIDbyOrg(string org)
        {
            String plant = org + "H1";
            string str = String.Format("select LAND1 from saprdp.t001w where WERKS='{0}' and mandt='168' and rownum=1", plant);
            Object CID = OracleProvider.ExecuteScalar("SAP_PRD", str);
            if (CID != null && !string.IsNullOrEmpty(CID.ToString()))
            {
                return CID.ToString();
            }
            return "TW";
        }

        public static String GetDeliveryPlant(String company_id, String org_id, String part_no, QuoteItemType type)
        {

            if (!String.IsNullOrEmpty(org_id))
            {
                if (org_id.Equals("TW01", StringComparison.OrdinalIgnoreCase))
                {
                    if (type == QuoteItemType.BtosParent || type == QuoteItemType.BtosPart)
                    {
                        return "TWH1";
                    }
                    else
                    {
                        String str = String.Format("select top 1 DELIVERYPLANT from SAP_PRODUCT_ORG where ORG_ID='TW01' and PART_NO = '{0}'", part_no);
                        DataTable dt = DataAccess.SqlProvider.dbGetDataTable("MY", str);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0][0].ToString();
                        else
                            return "TWH1";
                    }
                }
                else
                    return SAPDAL.GetPlantByOrg(org_id);
            }
            else
                return "TWH1";
        }


        public static string GetABRPartNCM(string partno)
        {

            try
            {
                String plant = "BRH1";
                string str = String.Format("select steuc from saprdp.marc where WERKS='" + plant + "' and mandt='168' and matnr='{0}'", partno);
                Object CID = OracleProvider.ExecuteScalar("SAP_PRD", str);
                if (CID != null && !string.IsNullOrEmpty(CID.ToString()))
                {
                    return CID.ToString();
                }
            }
            catch
            {

            }
            return "";
        }

        public static void Get_Next_WorkingDate_ByCode(ref DateTime iATPDate, decimal Loading_Days, string code)
        {
            Factory_Date_Conversion.Factory_Date_Conversion _Factory_Date_Conversion =
                new Factory_Date_Conversion.Factory_Date_Conversion(ConfigurationManager.AppSettings["SAP_PRD"]);

            Decimal factory_date_Number;

            CultureInfo provider1 = new CultureInfo("fr-FR", true);
            DateTime time1 = iATPDate;
            String iATPDateStr = time1.ToString("yyyyMMdd");
            String workingdate_indicator = string.Empty;
            try
            {
                _Factory_Date_Conversion.Connection.Open();

                _Factory_Date_Conversion.Date_Convert_To_Factorydate("+", code, out factory_date_Number, out workingdate_indicator, ref iATPDateStr);
                _Factory_Date_Conversion.Factorydate_Convert_To_Date(code, (factory_date_Number + Loading_Days), out iATPDateStr);

                DateTime time2 = DateTime.ParseExact(iATPDateStr, "yyyyMMdd", provider1);
                iATPDate = time2;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                _Factory_Date_Conversion.Connection.Close();
            }

        }

        /// <summary>
        /// Test if order has GP Block issue
        /// </summary>
        /// <param name="_Order"></param>
        /// <param name="ErrorMsg"></param>
        public static void SimulateOrderGPBlock(ref Order _Order, ref string ErrorMsg)
        {
            SAPRFC.SAPRFC _CheckGPObj = new SAPRFC.SAPRFC(ConfigurationManager.AppSettings["SAP_PRD"]);
            //Xuan mentioned that parameters CONVERT_AMOUNT_IDOC and SO_SIMULATE must be inputted
            //For CONVERT_AMOUNT_IDOC, the value must to be set as "Y", it means line item comes from outside
            String _CONVERT_AMOUNT_IDOC = "Y";
            String _No_Check_Reason = string.Empty;
            SAPRFC.KONVTable _konvtable = new SAPRFC.KONVTable();
            SAPRFC.VBAKTable _vbaktable = new SAPRFC.VBAKTable();
            SAPRFC.VBAPTable _vbaptable = new SAPRFC.VBAPTable();
            SAPRFC.VBEPTable _vbeptable = new SAPRFC.VBEPTable();
            // input table
            SAPRFC.ZSSD_34Table _order_simulate = new SAPRFC.ZSSD_34Table(); //*SO_SIMULATE
            // output table
            SAPRFC.ZSSD_GPTable _gp_result_table = new SAPRFC.ZSSD_GPTable();


            //Convert order to Z_SD_CTOS_SPLIT_PRICE.ZSSD_34Table
            foreach (Product _part in _Order.LineItems)
            {
                SAPRFC.ZSSD_34 _row = new SAPRFC.ZSSD_34();
                _row.Vkorg = _Order.OrgID; //sales org
                _row.Kunnr = _Order.GetOrderPartnet(OrderPartnerType.SoldTo).ErpID; //customer id
                _row.Waerk = _Order.Currency; //currency
                _row.Posnr = FormatItmNumber(_part.LineNumber);  //Item line number
                _row.Matnr = FormatToSAPPartNo(_part.PartNumber); //need to be converted to SAP material number format
                _row.Werks = _part.PlantID; //plant(delivery plant
                _row.Kwmeng = _part.Quantity; //quantity
                _row.Netpr = _part.UnitPrice; //selling price or quoting price
                _row.Uepos = FormatItmNumber(_part.ParentLineNumber); //Parent line number

                //_row.Netpr = 0;

                _order_simulate.Add(_row);
            }


            try
            {
                _CheckGPObj.Connection.Open();

                _CheckGPObj.Z_Sd_Check_Gp(_CONVERT_AMOUNT_IDOC
                    , DateTime.Now.ToString()
                    , ""
                    , out _No_Check_Reason
                    , ref _konvtable
                    , ref _vbaktable
                    , ref _vbaptable
                    , ref _vbeptable
                    , ref _order_simulate
                    , ref _gp_result_table);

                foreach (Product LineItem in _Order.LineItems)
                {
                    DataRow[] _row = _gp_result_table.ToADODataTable().Select("Matnr='" + LineItem.PartNumber + "'");
                    if (_row.Length > 0)
                    {
                        decimal _itp = 0;
                        decimal _minimumprice = 0;
                        decimal.TryParse(_row[0]["COST"].ToString(), out _itp);
                        decimal.TryParse(_row[0]["MINPRICE"].ToString(), out _minimumprice);
                        LineItem.ITP = _itp;
                        LineItem.MinimumPrice = _minimumprice;
                    }

                }

                //SAPRFC.ZSSD_GP[] array33 = new SAPRFC.ZSSD_GP[_gp_result_table.Count];
                //_gp_result_table.CopyTo(array33, 0);

                //for (int i = 0; i < array33.Length; i++)
                //{
                //    SAPRFC.ZSSD_GP item33 = array33[i];
                //    item33.Matnr = RemovePrecedingZeros(item33.Matnr);
                //    item33.Posnr = RemovePrecedingZeros(item33.Posnr);

                //}
            }
            catch (Exception e)
            {
                ErrorMsg = e.ToString();
            }
            finally
            {
                _CheckGPObj.Connection.Close();
            }



        }

        public static void SimulateOrder(ref Order _Order, ref string errorMsg)
        {

            BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE BAPISimulateOrder = new BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings["SAP_PRD"]);

            OrderPartner _SoldTo = _Order.GetOrderPartnet(OrderPartnerType.SoldTo);
            OrderPartner _ShipTo = _Order.GetOrderPartnet(OrderPartnerType.ShipTo);
            if (_ShipTo == null)
                _ShipTo = _Order.GetOrderPartnet(OrderPartnerType.SoldTo);

            if (_SoldTo.ErpID == "SAID")
                BAPISimulateOrder.ConnectionString = ConfigurationManager.AppSettings["SAPConnTest"];

            BAPI_SALESORDER_SIMULATE.BAPISDHEAD OrderHeader = new BAPI_SALESORDER_SIMULATE.BAPISDHEAD();
            BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable Partners = new BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable();
            BAPI_SALESORDER_SIMULATE.BAPIITEMINTable ItemsIn = new BAPI_SALESORDER_SIMULATE.BAPIITEMINTable();
            BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable ItemsOut = new BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable();
            BAPI_SALESORDER_SIMULATE.BAPICONDTable Conditions = new BAPI_SALESORDER_SIMULATE.BAPICONDTable();

            bool HasPhaseOutItem = false;
            bool RemoveAddedItem = false;
            bool OnlyHasZSWLItem = false;
            string AddedItemLineNo = "";
            Product _FistLineItem = _Order.LineItems.FirstOrDefault();
            string PlantID = _FistLineItem.PlantID;
            ArrayList phaseOutItems = new ArrayList();
            DataTable ZSWLItemSet = new DataTable();

            //ICC 2015/12/9 Add columns for ZSWLItemSet
            DataColumn dcPartNo = new DataColumn("PartNo", typeof(string));
            DataColumn dcQty = new DataColumn("Qty", typeof(int));
            DataColumn dcLineNo = new DataColumn("LineNo", typeof(string));
            dcPartNo.DefaultValue = string.Empty;
            dcQty.DefaultValue = 0;
            ZSWLItemSet.Columns.Add(dcPartNo);
            ZSWLItemSet.Columns.Add(dcQty);
            ZSWLItemSet.Columns.Add(dcLineNo);

            var _with2 = OrderHeader;
            _with2.Doc_Type = "ZOR";
            _with2.Sales_Org = _Order.OrgID;
            _with2.Distr_Chan = _Order.DistChannel;
            _with2.Division = _Order.Division;
            if (_Order.OrgID == "BR01")
            {
                switch (_Order.OrderType)
                {
                    case SAPOrderType.ZQTI:
                        _with2.Doc_Type = "ZQTI";
                        break;
                    case SAPOrderType.ZQTC:
                        _with2.Doc_Type = "ZQTC";
                        break;
                    case SAPOrderType.ZQTR:
                        _with2.Doc_Type = "ZQTR";
                        break;
                    default:
                        _with2.Doc_Type = "ZORR";
                        break;
                }

            }
            else
            {
                switch (_Order.OrderType)
                {
                    case SAPOrderType.AG:
                        _with2.Doc_Type = "AG";
                        break;
                    case SAPOrderType.QT:
                        _with2.Doc_Type = "QT";
                        break;
                }
            }

            if (!string.IsNullOrEmpty(_Order.Currency.Trim()))
                _with2.Currency = _Order.Currency;
            int LineNo = 1;
            System.Data.SqlClient.SqlConnection sqlMA = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
            sqlMA.Open();
            foreach (Product LineItem in _Order.LineItems)
            {
                //if (partitem.Qty == 0) partitem.Qty = 1;
                string chkSql = " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + " where a.part_no='" + LineItem.PartNumber.ToMyAdvantechPart() + "' and a.product_status in ('A','N','H','O','M1','C','P','S2','S5','T','V','') and a.sales_org='" + _Order.OrgID + "' ";
                DataTable chkDt = new DataTable();
                System.Data.SqlClient.SqlDataAdapter sqlAptr = new System.Data.SqlClient.SqlDataAdapter(chkSql, sqlMA);
                try
                {
                    sqlAptr.Fill(chkDt);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    sqlMA.Close();
                    errorMsg = ex.ToString();
                    //return false;
                }
                if (chkDt.Rows.Count > 0 && (_Order.OrgID != "TW01" | (_Order.OrgID == "TW01") & chkDt.Rows[0]["ProfitCenter"] != "N/A"))
                {
                    if (chkDt.Rows[0]["ITEM_CATEGORY_GROUP"].ToString() != "ZSWL")
                    {
                        BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
                        item.Itm_Number = FormatItmNumber(LineItem.LineNumber);
                        item.Material = FormatToSAPPartNo(LineItem.PartNumber.ToUpper());
                        item.Hg_Lv_Item = LineItem.ParentLineNumber.ToString();
                        //item.Req_Qty = partitem.QTY.ToString();
                        item.Req_Qty = (Convert.ToInt32(LineItem.Quantity) * 1000).ToString();
                        ItemsIn.Add(item);
                        //LineNo += 1;
                    }
                    else
                    {
                        DataRow zr = ZSWLItemSet.NewRow();
                        zr["PartNo"] = LineItem.PartNumber.ToUpper();
                        zr["Qty"] = LineItem.Quantity;
                        zr["LineNo"] = FormatItmNumber(LineItem.LineNumber);
                        ZSWLItemSet.Rows.Add(zr);
                    }
                }
                else
                {
                    phaseOutItems.Add(LineItem.PartNumber.ToUpper());
                }
            }
            sqlMA.Close();

            if (ItemsIn.Count == 0 && ZSWLItemSet.Rows.Count > 0)
            {
                BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
                item.Itm_Number = FormatItmNumber(1);
                item.Material = GetAHighLevelItemForPricing(_Order.OrgID);
                item.Req_Qty = "1";
                item.Req_Qty = (Convert.ToInt32(item.Req_Qty) * 1000).ToString();
                ItemsIn.Add(item);
                RemoveAddedItem = true;
                OnlyHasZSWLItem = true;
                AddedItemLineNo = LineNo.ToString();
                LineNo += 1;
            }
            if (ItemsIn.Count > 0 && ZSWLItemSet.Rows.Count > 0)
            {
                foreach (DataRow r in ZSWLItemSet.Rows)
                {
                    BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
                    //Alex 20170515 只有ZSWL 存在時，
                    if (OnlyHasZSWLItem)
                    {
                        item.Itm_Number = FormatItmNumber(LineNo);
                        LineNo += 1;
                    }
                    else
                        item.Itm_Number = r["LineNo"].ToString();
                    item.Material = FormatToSAPPartNo(r["PartNo"].ToString().Trim().ToUpper());
                    item.Req_Qty = r["Qty"].ToString();
                    item.Plant = PlantID;
                    item.Req_Qty = (Convert.ToInt32(item.Req_Qty) * 1000).ToString();
                    item.Hg_Lv_Item = "1";
                    ItemsIn.Add(item);

                }
            }
            BAPI_SALESORDER_SIMULATE.BAPIPARTNR SoldTo = new BAPI_SALESORDER_SIMULATE.BAPIPARTNR();
            BAPI_SALESORDER_SIMULATE.BAPIPARTNR ShipTo = new BAPI_SALESORDER_SIMULATE.BAPIPARTNR();
            BAPI_SALESORDER_SIMULATE.BAPIRET2Table retDt = new BAPI_SALESORDER_SIMULATE.BAPIRET2Table();
            SoldTo.Partn_Role = "AG";
            SoldTo.Partn_Numb = _SoldTo.ErpID.ToUpper();
            ShipTo.Partn_Role = "WE";
            ShipTo.Partn_Numb = _ShipTo.ErpID.ToUpper();
            Partners.Add(SoldTo);
            Partners.Add(ShipTo);
            BAPISimulateOrder.Connection.Open();

            try
            {
                DataTable dtItem = new DataTable();
                DataTable dtPartNr = new DataTable();
                DataTable dtcon = new DataTable();
                DataTable DTRET = new DataTable();

                dtItem = ItemsIn.ToADODataTable();
                dtPartNr = Partners.ToADODataTable();
                dtcon = Conditions.ToADODataTable();
                string outstr = "";
                BAPI_SALESORDER_SIMULATE.BAPIPAYER BAPIPAYER = new BAPI_SALESORDER_SIMULATE.BAPIPAYER();
                BAPI_SALESORDER_SIMULATE.BAPIRETURN BAPIRETURN = new BAPI_SALESORDER_SIMULATE.BAPIRETURN();
                BAPI_SALESORDER_SIMULATE.BAPISHIPTO BAPISHIPTO = new BAPI_SALESORDER_SIMULATE.BAPISHIPTO();
                BAPI_SALESORDER_SIMULATE.BAPISOLDTO BAPISOLDTO = new BAPI_SALESORDER_SIMULATE.BAPISOLDTO();
                BAPI_SALESORDER_SIMULATE.BAPIPAREXTable BAPIPAREXTable = new BAPI_SALESORDER_SIMULATE.BAPIPAREXTable();
                BAPI_SALESORDER_SIMULATE.BAPICCARDTable BAPICCARDTable = new BAPI_SALESORDER_SIMULATE.BAPICCARDTable();
                BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable BAPICCARD_EXTable = new BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable();
                BAPI_SALESORDER_SIMULATE.BAPICUBLBTable BAPICUBLBTable = new BAPI_SALESORDER_SIMULATE.BAPICUBLBTable();
                BAPI_SALESORDER_SIMULATE.BAPICUINSTable BAPICUINSTable = new BAPI_SALESORDER_SIMULATE.BAPICUINSTable();
                BAPI_SALESORDER_SIMULATE.BAPICUPRTTable BAPICUPRTTable = new BAPI_SALESORDER_SIMULATE.BAPICUPRTTable();
                BAPI_SALESORDER_SIMULATE.BAPICUCFGTable BAPICUCFGTable = new BAPI_SALESORDER_SIMULATE.BAPICUCFGTable();
                BAPI_SALESORDER_SIMULATE.BAPICUVALTable BAPICUVALTable = new BAPI_SALESORDER_SIMULATE.BAPICUVALTable();
                BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable BAPIINCOMPTable = new BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable();
                BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable BAPISDHEDUTable = new BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable();
                BAPI_SALESORDER_SIMULATE.BAPISCHDLTable BAPISCHDLTable = new BAPI_SALESORDER_SIMULATE.BAPISCHDLTable();
                BAPI_SALESORDER_SIMULATE.BAPIADDR1Table BAPIADDR1Table = new BAPI_SALESORDER_SIMULATE.BAPIADDR1Table();

                BAPISimulateOrder.Bapi_Salesorder_Simulate("",
                    OrderHeader,
                  out BAPIPAYER,
                  out BAPIRETURN,
                  out outstr,
                  out BAPISHIPTO,
                 out BAPISOLDTO,
                 ref BAPIPAREXTable,
                 ref retDt, ref BAPICCARDTable,
                 ref BAPICCARD_EXTable,
                 ref BAPICUBLBTable,
                ref BAPICUINSTable,
                ref BAPICUPRTTable,
                ref BAPICUCFGTable,
                ref BAPICUVALTable,
                ref Conditions,
                ref BAPIINCOMPTable,
                ref ItemsIn,
                ref ItemsOut,
                ref Partners,
                ref BAPISDHEDUTable,
                ref BAPISCHDLTable,
                ref BAPIADDR1Table);
                DataTable retAdoDt = retDt.ToADODataTable();


                foreach (DataRow retMsgRec in retAdoDt.Rows)
                {
                    if (retMsgRec["Type"].ToString() == "E")
                    {
                        HasPhaseOutItem = true;
                        errorMsg += string.Format("{0}", retMsgRec["Message"]);
                    }
                }

                DataTable ConditionOut = Conditions.ToADODataTable();
                DataTable PInDt = ItemsIn.ToADODataTable();
                DataTable POutDt = ItemsOut.ToADODataTable();

                DTRET = retDt.ToADODataTable();

                //ICMS Amount
                decimal _BX13 = 0;
                //IPI Amount
                decimal _BX23 = 0;
                //ICMS ST(Sub Trib Amount)
                decimal _BX41 = 0;
                decimal _ZMGP = 0;
                decimal _VPRS = 0;
                decimal _ConditionsPerUnit = 0;

                foreach (DataRow PIn in PInDt.Rows)
                {
                    //Alex 20170515 substrt 1 for lineno if only one ZSWL item existed and  line no < 100
                    var lineNo = PIn["Itm_Number"].ToString();
                    if (OnlyHasZSWLItem && Convert.ToInt32(PIn["Itm_Number"]) < 100)
                    {
                        lineNo = FormatItmNumber(Convert.ToInt32(PIn["Itm_Number"]) - 1);
                    }


                    Part _part = _Order.LineItems.FirstOrDefault(p => p.PartNumber == RemovePrecedingZeros(PIn["Material"].ToString()) && p.LineNumber == Convert.ToInt32(RemovePrecedingZeros(lineNo)));
                    if (_part == null)
                        continue;
                    _part.MinimumPrice = -1;
                    decimal outdb = 0;
                    DataRow[] rs2 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "'");
                    foreach (DataRow r in rs2)
                    {
                        switch (r["Cond_Type"].ToString().ToUpper())
                        {

                            case "ZPN0":
                            case "ZPR0":
                                if (decimal.TryParse(_part.ListPrice.ToString(), out outdb) && decimal.TryParse(r["Cond_Value"].ToString(), out outdb))
                                {
                                    decimal _liseprice = decimal.Parse(r["Cond_Value"].ToString());
                                    if (_liseprice > _part.ListPrice)
                                    {
                                        _part.ListPrice = (decimal)_liseprice;
                                    }
                                }
                                break;
                            case "ZMIP":
                                decimal _minimumprice = -1;
                                decimal.TryParse(r["Cond_Value"].ToString(), out _minimumprice);
                                _part.MinimumPrice = _minimumprice;
                                break;
                            case "BX13":
                                decimal.TryParse(r["CondValue"].ToString(), out _BX13);
                                break;
                            case "BX23":
                                decimal.TryParse(r["CondValue"].ToString(), out _BX23);
                                break;
                            case "BX41":
                                decimal.TryParse(r["CondValue"].ToString(), out _BX41);
                                break;

                            case "ZHB0":
                                // poutRec.RECYCLE_FEE = Strings.FormatNumber(r["Cond_Value"], 2);
                                break;

                            case "ZMGP":
                                decimal.TryParse(r["Cond_Value"].ToString(), out _ZMGP);
                                break;

                            case "VPRS":
                                // Ryan 20170502 VPRS should be divided by Conditions_Per_Unit
                                // Ryan 20170221 Add new property "conditions" for Part class and set VPRS value here for AJP                                
                                decimal.TryParse(r["Cond_Value"].ToString(), out _VPRS);
                                decimal.TryParse(r["COND_P_UNT"].ToString(), out _ConditionsPerUnit);

                                if (_ConditionsPerUnit != 0)
                                    _part.Conditions.VPRS = _VPRS / _ConditionsPerUnit;
                                break;
                        }
                    }
                    DataRow[] POutRs = POutDt.Select("Itm_Number='" + PIn["Itm_Number"] + "'");
                    if (PIn["Material"].ToString().IsNumberPart())
                    {
                        if (_part.ListPrice <= 0 && POutRs.Length > 0)
                        {
                            _part.ListPrice = decimal.Parse(POutRs[0]["net_value1"].ToString()) / decimal.Parse(POutRs[0]["req_qty"].ToString());
                        }
                    }
                    if (POutRs.Length > 0)
                    {
                        _part.UnitPrice = decimal.Parse(POutRs[0]["Net_Value1"].ToString()) / decimal.Parse(POutRs[0]["req_qty"].ToString());
                        if (_Order.OrgID == "BR01")
                        {
                            _part.ListPrice = _part.UnitPrice;
                            switch (_Order.OrderType)
                            {
                                case SAPOrderType.ZQTI:
                                    _part.ListPrice = _part.ListPrice + _BX13 + _BX23;
                                    break;
                                case SAPOrderType.ZQTC:
                                    _part.ListPrice = _part.ListPrice + _BX13 + _BX23 + _BX41;
                                    break;
                                case SAPOrderType.ZQTR:
                                    _part.ListPrice = _part.ListPrice + _BX13 + _BX23 + _BX41;
                                    break;
                                default:
                                    _part.ListPrice = _part.ListPrice + _BX13 + _BX23 + _BX41;
                                    break;
                            }
                            _part.UnitPrice = _part.ListPrice;
                        }
                        else if (_Order.OrgID.StartsWith("CN"))
                        {
                            _part.UnitPrice = Decimal.Round(_part.UnitPrice * (1 + _Order.Tax), 2);
                        }
                    }

                    if (_part.ListPrice < _part.UnitPrice)
                    {
                        _part.ListPrice = _part.UnitPrice;
                    }

                }
                foreach (string itm in phaseOutItems)
                {
                    Part _part = _Order.LineItems.First(p => p.PartNumber.Equals(RemovePrecedingZeros(itm), StringComparison.OrdinalIgnoreCase));

                    _part.ListPrice = 0;
                    //    pout.RECYCLE_FEE = 0;
                    _part.UnitPrice = 0;
                    //    ProductOut.AddProductOutRow(pout);
                }
            }
            catch (Exception ex)
            {
                errorMsg += System.Environment.NewLine + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString();
                BAPISimulateOrder.Connection.Close();
                //return false;
            }
            BAPISimulateOrder.Connection.Close();
        }

        public static void SimulateABROrder(ref Order _Order, ref string errorMsg)
        {

            BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE BAPISimulateOrder = new BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings["SAP_PRD"]);

            OrderPartner _SoldTo = _Order.GetOrderPartnet(OrderPartnerType.SoldTo);
            OrderPartner _ShipTo = _Order.GetOrderPartnet(OrderPartnerType.ShipTo);
            if (_ShipTo == null)
                _ShipTo = _Order.GetOrderPartnet(OrderPartnerType.SoldTo);

            if (_SoldTo.ErpID == "SAID")
                BAPISimulateOrder.ConnectionString = ConfigurationManager.AppSettings["SAPConnTest"];

            BAPI_SALESORDER_SIMULATE.BAPISDHEAD OrderHeader = new BAPI_SALESORDER_SIMULATE.BAPISDHEAD();
            BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable Partners = new BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable();
            BAPI_SALESORDER_SIMULATE.BAPIITEMINTable ItemsIn = new BAPI_SALESORDER_SIMULATE.BAPIITEMINTable();
            BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable ItemsOut = new BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable();
            BAPI_SALESORDER_SIMULATE.BAPICONDTable Conditions = new BAPI_SALESORDER_SIMULATE.BAPICONDTable();

            bool HasPhaseOutItem = false;
            bool RemoveAddedItem = false;
            string AddedItemLineNo = "";
            Product _FistLineItem = _Order.LineItems.FirstOrDefault();
            string PlantID = _FistLineItem.PlantID;
            ArrayList phaseOutItems = new ArrayList();
            DataTable ZSWLItemSet = new DataTable();

            //ICC 2015/12/9 Add columns for ZSWLItemSet
            DataColumn dcPartNo = new DataColumn("PartNo", typeof(string));
            DataColumn dcQty = new DataColumn("Qty", typeof(int));
            dcPartNo.DefaultValue = string.Empty;
            dcQty.DefaultValue = 0;
            ZSWLItemSet.Columns.Add(dcPartNo);
            ZSWLItemSet.Columns.Add(dcQty);

            var _with2 = OrderHeader;
            _with2.Doc_Type = "ZOR";
            _with2.Sales_Org = _Order.OrgID;
            _with2.Distr_Chan = _Order.DistChannel;
            _with2.Division = _Order.Division;

            switch (_Order.OrderType)
            {
                case SAPOrderType.ZQTI:
                    _with2.Doc_Type = "ZQTI";
                    break;
                case SAPOrderType.ZQTC:
                    _with2.Doc_Type = "ZQTC";
                    break;
                case SAPOrderType.ZQTR:
                    _with2.Doc_Type = "ZQTR";
                    break;
                default:
                    _with2.Doc_Type = "ZORR";
                    break;
            }


            if (!string.IsNullOrEmpty(_Order.Currency.Trim()))
                _with2.Currency = _Order.Currency;
            int LineNo = 1;
            System.Data.SqlClient.SqlConnection sqlMA = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
            sqlMA.Open();
            foreach (Product LineItem in _Order.LineItems)
            {
                //if (partitem.Qty == 0) partitem.Qty = 1;
                string chkSql = " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + " where a.part_no='" + LineItem.PartNumber.ToMyAdvantechPart() + "' and a.product_status in ('A','N','H','O','S5','V','M1','') and a.sales_org='" + _Order.OrgID + "' ";
                DataTable chkDt = new DataTable();
                System.Data.SqlClient.SqlDataAdapter sqlAptr = new System.Data.SqlClient.SqlDataAdapter(chkSql, sqlMA);
                try
                {
                    sqlAptr.Fill(chkDt);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    sqlMA.Close();
                    errorMsg = ex.ToString();
                    //return false;
                }
                if (chkDt.Rows.Count > 0 && (_Order.OrgID != "TW01" | (_Order.OrgID == "TW01") & chkDt.Rows[0]["ProfitCenter"].ToString() != "N/A"))
                {
                    if (chkDt.Rows[0]["ITEM_CATEGORY_GROUP"].ToString() != "ZSWL")
                    {
                        BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
                        item.Itm_Number = FormatItmNumber(LineItem.LineNumber);
                        item.Material = FormatToSAPPartNo(LineItem.PartNumber.ToUpper());
                        //item.Req_Qty = partitem.QTY.ToString();
                        item.Req_Qty = (Convert.ToInt32(LineItem.Quantity) * 1000).ToString();
                        item.Hg_Lv_Item = LineItem.ParentLineNumber.ToString();
                        if (LineItem.LineNumber % 100 != 0)
                        {
                            item.Cond_Type = "ZK06";
                            //Frank and Ryan 20160823 control the discount or markup by front end
                            //item.Cond_Val1 = _Order.Discount * -1;
                            item.Cond_Val1 = _Order.Discount;
                        }

                        ItemsIn.Add(item);
                        LineNo += 1;
                    }
                    else
                    {
                        DataRow zr = ZSWLItemSet.NewRow();
                        zr["PartNo"] = LineItem.PartNumber.ToUpper();
                        zr["Qty"] = LineItem.Quantity;
                        ZSWLItemSet.Rows.Add(zr);
                    }
                }
                else
                {
                    phaseOutItems.Add(LineItem.PartNumber.ToUpper());
                }


                //if (_Order.Discount > 0)
                //{
                //    //dddd
                //    string _cond_type = string.Empty;
                //    if (LineItem.LineNumber < 100)
                //    {
                //        _cond_type = "ZK06";
                //    }
                //    else if (LineItem.LineNumber >= 100 && (LineItem.LineNumber % 100) == 0)
                //    {
                //        _cond_type = "ZKB6";
                //    }
                //    if (!string.IsNullOrEmpty(_cond_type))
                //    {
                //        BAPI_SALESORDER_SIMULATE.BAPICOND S_ConditionRow = new BAPI_SALESORDER_SIMULATE.BAPICOND();
                //        S_ConditionRow.Itm_Number = FormatItmNumber(LineItem.LineNumber);
                //        S_ConditionRow.Cond_Type = _cond_type;
                //        S_ConditionRow.Cond_Value = _Order.Discount;
                //        //系統下的料號discount要對ZPR0去discount之後，其它的稅就會一起變了
                //        //S_ConditionRow.Currency = "%";
                //        Conditions.Add(S_ConditionRow);

                //        DataTable ConditionOut1 = Conditions.ToADODataTable();
                //        int aaa = 1;
                //    }

                //}

            }
            sqlMA.Close();

            //            If Not IsNothing(ConditionDT) AndAlso ConditionDT.Rows.Count > 0 Then
            //    For Each r As SalesOrder.ConditionRow In ConditionDT.Rows
            //        Dim S_ConditionRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND
            //        With r
            //            S_ConditionRow.Itm_Number = "000000" : S_ConditionRow.Cond_Type = .TYPE : S_ConditionRow.Currency = .CURRENCY : S_ConditionRow.Cond_Value = .VALUE
            //        End With
            //        S_ConditionDT.Add(S_ConditionRow)
            //    Next
            //End If

            //if (Conditions != null && Conditions.Count > 0)
            //{
            //    foreach (salesor r in Conditions.ToADODataTable().Rows)
            //    {
            //        BAPI_SALESORDER_SIMULATE.BAPICOND S_ConditionRow = new BAPI_SALESORDER_SIMULATE.BAPICOND();
            //        S_ConditionRow.Itm_Number = "000000";
            //        S_ConditionRow.Cond_Type = r.Cond_Type;
            //        S_ConditionRow.Currency = r.CURRENCY() ;
            //        S_ConditionRow.Cond_Value = r.VALUE;
            //        Conditions.Add(S_ConditionRow);

            //    }
            //}


            if (ItemsIn.Count == 0 && ZSWLItemSet.Rows.Count > 0)
            {
                BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
                item.Itm_Number = FormatItmNumber(LineNo);
                item.Material = GetAHighLevelItemForPricing(_Order.OrgID);
                item.Req_Qty = "1";
                item.Req_Qty = (Convert.ToInt32(item.Req_Qty) * 1000).ToString();
                ItemsIn.Add(item);
                RemoveAddedItem = true;
                AddedItemLineNo = LineNo.ToString();

                LineNo += 1;
            }
            if (ItemsIn.Count > 0 && ZSWLItemSet.Rows.Count > 0)
            {
                foreach (DataRow r in ZSWLItemSet.Rows)
                {
                    BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
                    item.Itm_Number = FormatItmNumber(LineNo);
                    item.Material = FormatToSAPPartNo(r["PartNo"].ToString().Trim().ToUpper());
                    item.Req_Qty = r["Qty"].ToString();
                    item.Plant = PlantID;
                    item.Req_Qty = (Convert.ToInt32(item.Req_Qty) * 1000).ToString();
                    item.Hg_Lv_Item = "1";
                    ItemsIn.Add(item);
                    LineNo += 1;
                }
            }
            BAPI_SALESORDER_SIMULATE.BAPIPARTNR SoldTo = new BAPI_SALESORDER_SIMULATE.BAPIPARTNR();
            BAPI_SALESORDER_SIMULATE.BAPIPARTNR ShipTo = new BAPI_SALESORDER_SIMULATE.BAPIPARTNR();
            BAPI_SALESORDER_SIMULATE.BAPIRET2Table retDt = new BAPI_SALESORDER_SIMULATE.BAPIRET2Table();
            SoldTo.Partn_Role = "AG";
            SoldTo.Partn_Numb = _SoldTo.ErpID;
            ShipTo.Partn_Role = "WE";
            ShipTo.Partn_Numb = _ShipTo.ErpID;
            Partners.Add(SoldTo);
            Partners.Add(ShipTo);
            BAPISimulateOrder.Connection.Open();

            try
            {
                DataTable dtItem = new DataTable();
                DataTable dtPartNr = new DataTable();
                DataTable dtcon = new DataTable();
                DataTable DTRET = new DataTable();

                dtItem = ItemsIn.ToADODataTable();
                dtPartNr = Partners.ToADODataTable();
                dtcon = Conditions.ToADODataTable();
                string outstr = "";
                BAPI_SALESORDER_SIMULATE.BAPIPAYER BAPIPAYER = new BAPI_SALESORDER_SIMULATE.BAPIPAYER();
                BAPI_SALESORDER_SIMULATE.BAPIRETURN BAPIRETURN = new BAPI_SALESORDER_SIMULATE.BAPIRETURN();
                BAPI_SALESORDER_SIMULATE.BAPISHIPTO BAPISHIPTO = new BAPI_SALESORDER_SIMULATE.BAPISHIPTO();
                BAPI_SALESORDER_SIMULATE.BAPISOLDTO BAPISOLDTO = new BAPI_SALESORDER_SIMULATE.BAPISOLDTO();
                BAPI_SALESORDER_SIMULATE.BAPIPAREXTable BAPIPAREXTable = new BAPI_SALESORDER_SIMULATE.BAPIPAREXTable();
                BAPI_SALESORDER_SIMULATE.BAPICCARDTable BAPICCARDTable = new BAPI_SALESORDER_SIMULATE.BAPICCARDTable();
                BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable BAPICCARD_EXTable = new BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable();
                BAPI_SALESORDER_SIMULATE.BAPICUBLBTable BAPICUBLBTable = new BAPI_SALESORDER_SIMULATE.BAPICUBLBTable();
                BAPI_SALESORDER_SIMULATE.BAPICUINSTable BAPICUINSTable = new BAPI_SALESORDER_SIMULATE.BAPICUINSTable();
                BAPI_SALESORDER_SIMULATE.BAPICUPRTTable BAPICUPRTTable = new BAPI_SALESORDER_SIMULATE.BAPICUPRTTable();
                BAPI_SALESORDER_SIMULATE.BAPICUCFGTable BAPICUCFGTable = new BAPI_SALESORDER_SIMULATE.BAPICUCFGTable();
                BAPI_SALESORDER_SIMULATE.BAPICUVALTable BAPICUVALTable = new BAPI_SALESORDER_SIMULATE.BAPICUVALTable();
                BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable BAPIINCOMPTable = new BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable();
                BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable BAPISDHEDUTable = new BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable();
                BAPI_SALESORDER_SIMULATE.BAPISCHDLTable BAPISCHDLTable = new BAPI_SALESORDER_SIMULATE.BAPISCHDLTable();
                BAPI_SALESORDER_SIMULATE.BAPIADDR1Table BAPIADDR1Table = new BAPI_SALESORDER_SIMULATE.BAPIADDR1Table();

                BAPISimulateOrder.Bapi_Salesorder_Simulate("",
                    OrderHeader,
                  out BAPIPAYER,
                  out BAPIRETURN,
                  out outstr,
                  out BAPISHIPTO,
                 out BAPISOLDTO,
                 ref BAPIPAREXTable,
                 ref retDt, ref BAPICCARDTable,
                 ref BAPICCARD_EXTable,
                 ref BAPICUBLBTable,
                ref BAPICUINSTable,
                ref BAPICUPRTTable,
                ref BAPICUCFGTable,
                ref BAPICUVALTable,
                ref Conditions,
                ref BAPIINCOMPTable,
                ref ItemsIn,
                ref ItemsOut,
                ref Partners,
                ref BAPISDHEDUTable,
                ref BAPISCHDLTable,
                ref BAPIADDR1Table);
                DataTable retAdoDt = retDt.ToADODataTable();

                BAPISimulateOrder.Connection.Close();

                foreach (DataRow retMsgRec in retAdoDt.Rows)
                {
                    if (retMsgRec["Type"].ToString() == "E")
                    {
                        HasPhaseOutItem = true;
                        errorMsg += string.Format("{0}", retMsgRec["Message"]);
                    }
                }

                DataTable ConditionOut = Conditions.ToADODataTable();
                DataTable PInDt = ItemsIn.ToADODataTable();
                DataTable POutDt = ItemsOut.ToADODataTable();

                DTRET = retDt.ToADODataTable();
                //BR ICMS Normal Base
                decimal _BX10 = 0;

                //ICMS Amount
                decimal _BX13 = 0;
                //IPI Amount
                decimal _BX23 = 0;

                //BR SubTrib Base
                decimal _BX40 = 0;

                //ICMS ST(Sub Trib Amount)
                decimal _BX41 = 0;

                decimal _BX72 = 0;

                decimal _BX82 = 0;
                decimal _BX94 = 0;
                decimal _BX95 = 0;
                decimal _BX96 = 0;

                //Freight
                decimal _FK00 = 0;

                //ICVA Tax Rate
                decimal _ICVA = 0;

                //IPI Tax Rate
                decimal _IPVA = 0;

                //COFINS Rate
                decimal _BCO1 = 0;

                //PIS Rate
                decimal _BPI1 = 0;

                //ZPN0
                decimal _ZPN0 = 0;

                //ZPR0
                decimal _ZPR0 = 0;

                //ZCBR
                decimal _ZCBR = 0;


                //ICMI
                decimal _ICMI = 0;

                //ISIC ICMS Rate SF=ST
                decimal _ISIC = 0;

                //ISTS Subtrib Surcharge
                decimal _ISTS = 0;


                //rate Base
                decimal _RateBase = 0;
                //rate Base of calculation
                decimal _RateBaseCalculation = 0;

                decimal _RateBaseParameter1 = 4;

                //Decimal _ICMS_DIFAL_ORIGEM = 0.12M;
                //Decimal _ICMS_DIFAL_DESTINO = 0.12M;
                //Decimal _ICMS_ST = 0.18M;

                List<Tuple<int, Boolean>> no_bx41 = new List<Tuple<int, bool>> { Tuple.Create(0, false) };

                foreach (DataRow PIn in PInDt.Rows)
                {
                    int _lineNo = int.Parse(PIn["Itm_Number"].ToString());
                    //Part _part = _Order.LineItems.First(p => p.PartNumber == RemovePrecedingZeros(PIn["Material"].ToString()) && p.LineNumber == _lineNo);
                    Product _part = _Order.LineItems.First(p => p.PartNumber == RemovePrecedingZeros(PIn["Material"].ToString()) && p.LineNumber == _lineNo);
                    _part.MinimumPrice = -1;
                    decimal outdb = 0;
                    DataRow[] rs2 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "'");
                    DataRow[] rs3 = null;
                    //foreach (DataRow r in rs2)
                    //{

                    //int _lineNo = int.Parse(PIn["Itm_Number"].ToString());
                    bool _IsBtoParent = false;
                    if (_lineNo >= 100 && (_lineNo % 100 == 0))
                    {
                        _IsBtoParent = true;
                    }

                    //if (_IsBtoParent)
                    //{
                    //    //ICVA Tax Rate
                    //    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ICVA' ");
                    //    if (rs3.Length > 0)
                    //    {
                    //        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _ICVA);
                    //    }

                    //    //IPI Tax Rate
                    //    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='IPVA' ");
                    //    if (rs3.Length > 0)
                    //    {
                    //        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _IPVA);
                    //    }

                    //    //COFINS Rate
                    //    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BCO1' ");
                    //    if (rs3.Length > 0)
                    //    {
                    //        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _BCO1);
                    //    }

                    //    //PIS Rate
                    //    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BPI1' ");
                    //    if (rs3.Length > 0)
                    //    {
                    //        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _BPI1);
                    //    }

                    //    //ISIC Rate
                    //    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ISIC' ");
                    //    if (rs3.Length > 0)
                    //    {
                    //        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _ISIC);
                    //    }

                    //    //ISTS Rate
                    //    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ISTS' ");
                    //    if (rs3.Length > 0)
                    //    {
                    //        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _ISTS);
                    //    }


                    //    switch (_Order.OrderType)
                    //    {
                    //        case SAPOrderType.ZQTI:
                    //            _RateBase = 1 - ((_BPI1 + _BCO1 + _RateBaseParameter1) / 100);
                    //            break;
                    //        case SAPOrderType.ZQTC:
                    //            _RateBase = 1 - ((_BPI1 + _BCO1 + _RateBaseParameter1 + (_RateBaseParameter1 * (_IPVA / 100))) / 100);

                    //            //Ryan 20161102 Validate if bx41 is needed.
                    //            DataTable dt_region = SqlProvider.dbGetDataTable("MY", string.Format("SELECT COMPANY_ID,REGION_CODE FROM SAP_DIMCOMPANY WHERE COMPANY_ID='{0}'", _SoldTo.ErpID));
                    //            DataTable dt_controlcode = OracleProvider.GetDataTable("SAP_PRD", string.Format("select matnr,STEUC from saprdp.MARC where matnr='{0}' and werks='BRH1'", _part.PartNumber));
                    //            if ((dt_region != null && dt_region.Rows.Count > 0) && (dt_controlcode != null && dt_controlcode.Rows.Count > 0))
                    //            {
                    //                DataTable dt_bx41validation = OracleProvider.GetDataTable("SAP_PRD", string.Format("select * from saprdp.J_1BTXST3 where shipto='{0}' and GRUOP='60' and value='{1}'", dt_region.Rows[0]["REGION_CODE"].ToString(), dt_controlcode.Rows[0]["STEUC"].ToString()));
                    //                if (dt_bx41validation != null && dt_bx41validation.Rows.Count == 0)
                    //                    no_bx41.Add(Tuple.Create((_lineNo / 100), true));
                    //            }

                    //            break;
                    //        case SAPOrderType.ZQTR:
                    //            _RateBase = 1 - ((_BPI1 + _BCO1 + _RateBaseParameter1) / 100);
                    //            break;
                    //        default:
                    //            _RateBase = 1 - ((_BPI1 + _BCO1 + _RateBaseParameter1) / 100);
                    //            break;
                    //    }
                    //}


                    //ICVA Tax Rate
                    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ICVA' ");
                    if (rs3.Length > 0)
                    {
                        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _ICVA);
                    }

                    //IPI Tax Rate
                    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='IPVA' ");
                    if (rs3.Length > 0)
                    {
                        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _IPVA);
                    }

                    //COFINS Rate
                    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BCO1' ");
                    if (rs3.Length > 0)
                    {
                        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _BCO1);
                    }

                    //PIS Rate
                    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BPI1' ");
                    if (rs3.Length > 0)
                    {
                        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _BPI1);
                    }

                    //ISIC Rate
                    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ISIC' ");
                    if (rs3.Length > 0)
                    {
                        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _ISIC);
                    }

                    //ISTS Rate
                    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ISTS' ");
                    if (rs3.Length > 0)
                    {
                        decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _ISTS);
                    }


                    //ZPN0
                    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ZPN0' ");
                    if (rs3.Length > 0) decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _ZPN0);

                    //ZPR0
                    rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ZPR0' ");
                    if (rs3.Length > 0) decimal.TryParse(rs3[0]["Cond_Value"].ToString(), out _ZPR0);

                    //Frank 20161012 It seems loose item's discount price won't be returned as ZPNO.
                    //So need to discount/increase it by our-self
                    if (_lineNo < 100 && _Order.Discount != 0)
                    {
                        _ZPR0 = _ZPR0 + (_ZPR0 * (_Order.Discount / 100));
                    }

                    if (_lineNo > 100 && !_IsBtoParent)
                    {
                        //Frank Chris說取負數要再絕對值的邏輯怪怪的，之後再確認
                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='ZCBR' ");
                        if (rs3.Length > 0)
                        {
                            decimal.TryParse(rs3[0]["CondValue"].ToString(), out _ZCBR);
                            _ZPR0 = Math.Abs(_ZCBR);
                            if (_part.Quantity > 0)
                            {
                                _ZPR0 = _ZPR0 / _part.Quantity;
                            }
                        }

                        //_RateBaseCalculation = _ZPR0 / _RateBase;
                    }

                    if (_lineNo < 100 || _IsBtoParent)
                    {
                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX10' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX10);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX13' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX13);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX23' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX23);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX40' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX40);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX41' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX41);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX72' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX72);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX82' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX82);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX94' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX94);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX95' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX95);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='BX96' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _BX96);

                        rs3 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "' and Cond_Type='FK00' ");
                        if (rs3.Length > 0) decimal.TryParse(rs3[0]["CondValue"].ToString(), out _FK00);

                    }
                    else
                    {

                        _BX10 = 0;
                        _BX13 = 0;
                        _BX23 = 0;
                        _BX40 = 0;
                        _BX41 = 0;
                        _BX72 = 0;
                        _BX82 = 0;
                        _BX94 = 0;
                        _BX95 = 0;
                        _BX96 = 0;
                        _FK00 = 0;

                        //Frank 20180417，do not calcualte it by ourself, too difficult Q_Q
                        //_BX13 = (_ICVA / 100) * _RateBaseCalculation;
                        //_BX13 = Math.Round(_BX13, 2, MidpointRounding.AwayFromZero);

                        //_BX23 = (_IPVA / 100) * _RateBaseCalculation;
                        //_BX23 = Math.Round(_BX23, 2, MidpointRounding.AwayFromZero);

                        //_BX41 = 0;
                        //switch (_Order.OrderType)
                        //{
                        //    case SAPOrderType.ZQTC:

                        //        _BX13 = (_ICVA / 100) * (_RateBaseCalculation + _BX23);
                        //        _BX13 = Math.Round(_BX13, 2, MidpointRounding.AwayFromZero);



                        //        //_BX41 = (((_RateBaseCalculation + _BX23) * _ICMS_DIFAL_ORIGEM) - _BX13) * 0.6M;
                        //        //_BX41 = _BX41 + (((_RateBaseCalculation + _BX23) * _ICMS_DIFAL_DESTINO) - _BX13) * 0.4M;
                        //        //_ISIC

                        //        // Ryan 20161102 BX41 validation
                        //        if (no_bx41.Any(d => d.Item1 == _lineNo / 100 && d.Item2 == true))
                        //            _BX41 = 0;
                        //        else
                        //            _BX41 = ((_RateBaseCalculation + _BX23) * (_ISIC / 100)) - _BX13;
                        //        break;
                        //    case SAPOrderType.ZQTR:
                        //        _BX41 = _RateBaseCalculation + _BX23 + ((_RateBaseCalculation + _BX23) * (_ISTS / 100));
                        //        _BX41 = _BX41 * (_ISIC / 100) - _BX13;
                        //        break;
                        //    default:
                        //        _BX41 = 0;
                        //        break;
                        //}
                        //_BX41 = Math.Round(_BX41, 2, MidpointRounding.AwayFromZero);

                        //_BX82 = (_BPI1 / 100) * _RateBaseCalculation;
                        //_BX82 = Math.Round(_BX82, 2, MidpointRounding.AwayFromZero);

                        //_BX72 = (_BCO1 / 100) * _RateBaseCalculation;
                        //_BX72 = Math.Round(_BX72, 2, MidpointRounding.AwayFromZero);
                    }




                    //}

                    DataRow[] POutRs = POutDt.Select("Itm_Number='" + PIn["Itm_Number"] + "'");
                    if (PIn["Material"].ToString().IsNumberPart())
                    {
                        if (_part.ListPrice <= 0 && POutRs.Length > 0)
                        {
                            _part.ListPrice = decimal.Parse(POutRs[0]["net_value1"].ToString()) / decimal.Parse(POutRs[0]["req_qty"].ToString());
                        }
                    }
                    if (POutRs.Length > 0)
                    {
                        //_part.UnitPrice = decimal.Parse(POutRs[0]["Net_Value1"].ToString()) / decimal.Parse(POutRs[0]["req_qty"].ToString());
                        //_part.ListPrice = _part.UnitPrice;

                        decimal _PriceNoTax = _ZPR0;
                        if (_IsBtoParent)
                            _PriceNoTax = _ZPN0;

                        //switch (_Order.OrderType)
                        //{
                        //    case SAPOrderType.ZQTI:
                        //        _part.ListPrice = _PriceNoTax + _BX13 + _BX23 + _BX82 + _BX72;
                        //        break;
                        //    case SAPOrderType.ZQTC:
                        //        _part.ListPrice = _PriceNoTax + _BX13 + _BX23 + _BX82 + _BX72 + _BX41;
                        //        break;
                        //    case SAPOrderType.ZQTR:
                        //        _part.ListPrice = _PriceNoTax + _BX13 + _BX23 + _BX82 + _BX72 + _BX41;
                        //        break;
                        //    default:
                        //        _part.ListPrice = _PriceNoTax + _BX13 + _BX23 + _BX82 + _BX72 + _BX41;
                        //        break;
                        //}
                        _part.ListPrice = _PriceNoTax;
                        _part.BX10 = _BX10;
                        _part.BX13 = _BX13;
                        _part.BX23 = _BX23;
                        _part.BX40 = _BX40;
                        _part.BX41 = _BX41;
                        _part.BX72 = _BX72;
                        _part.BX82 = _BX82;
                        _part.BX94 = _BX94;
                        _part.BX95 = _BX95;
                        _part.BX96 = _BX96;
                        _part.FK00 = _FK00;
                        _part.ICVA = _ICVA;
                        _part.IPVA = _IPVA;
                        _part.ISIC = _ISIC;
                        _part.ISTS = _ISTS;
                        _part.BCO1 = _BCO1;
                        _part.BPI1 = _BPI1;

                        _part.UnitPrice = _part.ListPrice;


                        

                    }

                    if (_part.ListPrice < _part.UnitPrice)
                    {
                        _part.ListPrice = _part.UnitPrice;
                    }

                    _part.NCM = GetABRPartNCM(_part.PartNumber);

                }
                foreach (string itm in phaseOutItems)
                {
                    //Part _part = _Order.LineItems.First(p => p.PartNumber == RemovePrecedingZeros(itm));
                    Product _part = _Order.LineItems.First(p => p.PartNumber == RemovePrecedingZeros(itm));

                    _part.ListPrice = 0;
                    //    pout.RECYCLE_FEE = 0;
                    _part.UnitPrice = 0;
                    //    ProductOut.AddProductOutRow(pout);
                }
            }
            catch (Exception ex)
            {
                errorMsg += System.Environment.NewLine + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString();
                BAPISimulateOrder.Connection.Close();
                //return false;
            }
            BAPISimulateOrder.Connection.Close();
        }

        public static Decimal GetExchangerate(String C_FROM, String C_TO)
        {
            if (C_FROM.Equals(C_TO))
            {
                return 1;
            }
            Decimal _returnval = 0;
            Object temp = null;
            temp = SqlProvider.dbExecuteScalar("MY", "select top 1 UKURS from SAP_EXCHANGERATE where fCURR='" + C_FROM + "' and TCURR='" + C_TO + "' and EXCH_DATE<=GETDATE() order by exch_date desc");

            if (temp != null && !string.IsNullOrEmpty(temp.ToString()))
            {
                Decimal.TryParse(temp.ToString(), out _returnval);
            }

            return _returnval;
        }

        /// <summary>
        /// Get produce cost for quote items
        /// </summary>
        /// <param name="QuoteDetailList"></param>
        /// <param name="SAPCurrency"></param>
        public static void GetProductBOMCost(ref List<QuotationDetail> QuoteDetailList, String SAPCurrency, ref string errMsg)
        {
            try
            {
                Dictionary<string, string> dic_ItemNoCost = new Dictionary<string, string>();

                ZGET_PROD_PLANT_COST.ZMATNRTable _ZMATNRTable = new ZGET_PROD_PLANT_COST.ZMATNRTable();
                ZGET_PROD_PLANT_COST.ZPARTCOSTINFOTable _INFOTable = new ZGET_PROD_PLANT_COST.ZPARTCOSTINFOTable();

                //ZGET_PROD_PLANT_COST.ZMATNR _ZMATNR = new ZGET_PROD_PLANT_COST.ZMATNR() { Matnr = itemNo };
                //_ZMATNRTable.Add(_ZMATNR);
                foreach (var item in QuoteDetailList)
                {
                    ZGET_PROD_PLANT_COST.ZMATNR _ZMATNR = new ZGET_PROD_PLANT_COST.ZMATNR() { Matnr = item.partNo };
                    _ZMATNRTable.Add(_ZMATNR);

                }

                ZGET_PROD_PLANT_COST.ZGET_PROD_PLANT_COST obj = new ZGET_PROD_PLANT_COST.ZGET_PROD_PLANT_COST() { Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings["SAP_PRD"]) };
                obj.Connection.Open();
                obj.Zget_Prod_Plant_Cost(null, ref _ZMATNRTable, ref _INFOTable);
                obj.Connection.Close();

                Decimal _exchangerate = GetExchangerate("USD", SAPCurrency);


                foreach (ZGET_PROD_PLANT_COST.ZPARTCOSTINFO item in _INFOTable)
                {
                    //cost = item.Ugrp_Actcost.ToString();

                    var _matchparts = from _quoteitem in QuoteDetailList
                                      where _quoteitem.partNo == item.Matnr
                                      select _quoteitem;

                    foreach (var _part in _matchparts)
                    {
                        _part.ProduceBOMCost = item.Ugrp_Actcost * _exchangerate;
                    }

                }
                errMsg = "";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }



        }

        public static Boolean SharingConfigSystemPrice(ref Order _Order, Decimal _SystemAmount, int _ParentLineNo, ref String _errMsg)
        {
            Z_SD_CTOS_SPLIT_PRICE.Z_SD_CTOS_SPLIT_PRICE _SAPSharingPrice = new Z_SD_CTOS_SPLIT_PRICE.Z_SD_CTOS_SPLIT_PRICE(ConfigurationManager.AppSettings["SAP_PRD"]);
            string _NoSplitReason = string.Empty;
            decimal _outadmount = 0;

            Z_SD_CTOS_SPLIT_PRICE.ZSSD_33Table _33Table = new Z_SD_CTOS_SPLIT_PRICE.ZSSD_33Table();
            Z_SD_CTOS_SPLIT_PRICE.ZSSD_34Table _34Table = new Z_SD_CTOS_SPLIT_PRICE.ZSSD_34Table();

            BAPI_SALESORDER_SIMULATE.BAPIPAYER BAPIPAYER = new BAPI_SALESORDER_SIMULATE.BAPIPAYER();

            //Convert order to Z_SD_CTOS_SPLIT_PRICE.ZSSD_34Table
            foreach (Product _part in _Order.LineItems)
            {
                Z_SD_CTOS_SPLIT_PRICE.ZSSD_34 _row = new Z_SD_CTOS_SPLIT_PRICE.ZSSD_34();
                _row.Vkorg = _Order.OrgID;
                _row.Kunnr = _Order.GetOrderPartnet(OrderPartnerType.SoldTo).ErpID;
                _row.Waerk = _Order.Currency;

                _row.Uepos = FormatItmNumber(_part.ParentLineNumber); //Parent line number
                _row.Posnr = FormatItmNumber(_part.LineNumber);  //Item line number
                _row.Matnr = FormatToSAPPartNo(_part.PartNumber); //need to be converted to SAP material number format
                _row.Werks = _part.PlantID;
                _row.Kwmeng = _part.Quantity;
                //_row.Netpr = 0;

                _34Table.Add(_row);
            }

            try
            {
                _SAPSharingPrice.Connection.Open();
                _SAPSharingPrice.Z_Sd_Ctos_Split_Price(_SystemAmount, _ParentLineNo.ToString(), "", out _errMsg, out _outadmount, ref _34Table, ref _33Table);

                if (String.IsNullOrEmpty(_errMsg))
                {
                    Z_SD_CTOS_SPLIT_PRICE.ZSSD_33[] array33 = new Z_SD_CTOS_SPLIT_PRICE.ZSSD_33[_33Table.Count];
                    _33Table.CopyTo(array33, 0);

                    // If any item with negative price, return false
                    for (int i = 0; i < array33.Length; i++)
                    {
                        Z_SD_CTOS_SPLIT_PRICE.ZSSD_33 item33 = array33[i];
                        item33.Matnr = RemovePrecedingZeros(item33.Matnr);
                        item33.Posnr = RemovePrecedingZeros(item33.Posnr);

                        if (item33.Unitprice < 0)
                        {
                            _errMsg = "Invalid price, please check your input amount again.";
                            return false;
                        }
                    }

                    for (int i = 0; i < array33.Length; i++)
                    {
                        Z_SD_CTOS_SPLIT_PRICE.ZSSD_33 item33 = array33[i];
                        item33.Matnr = RemovePrecedingZeros(item33.Matnr);
                        item33.Posnr = RemovePrecedingZeros(item33.Posnr);

                        _Order.LineItems.Where(d => d.PartNumber.Equals(item33.Matnr, StringComparison.OrdinalIgnoreCase) && d.LineNumber.ToString().Equals(item33.Posnr)).ToList().ForEach(c => { c.UnitPrice = item33.Unitprice; c.ListPrice = item33.Unitprice; });
                    }
                }
                else
                {
                    _SAPSharingPrice.Connection.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                _errMsg = e.ToString();
                return false;
            }
            finally
            {
                _SAPSharingPrice.Connection.Close();
            }
            return true;
        }

        /// <summary>
        /// Convert order to SAP
        /// </summary>
        /// <param name="_Order"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static Boolean CreateOrder(ref Order _Order, DateTime LocalTime, Boolean IsTest, ref string ErrMsg)
        {

            if (_Order == null)
            {
                ErrMsg = "Order instance is null"; return false;
            }


            if (_Order.LineItems == null || _Order.LineItems.Count == 0)
            {
                ErrMsg = "This order does not have parts"; return false;
            }

            OrderPartner _SoldTo = _Order.GetOrderPartnet(OrderPartnerType.SoldTo);
            if (_SoldTo == null)
            {
                ErrMsg = "Sold-to party is not specified"; return false;
            }
            OrderPartner _ShipTo = _Order.GetOrderPartnet(OrderPartnerType.SoldTo);
            if (_ShipTo == null)
            {
                ErrMsg = "Ship-to party is not specified"; return false;
            }

            BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SALESORDER_CREATEFROMDAT2 BAPICreateOrder = new BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SALESORDER_CREATEFROMDAT2(ConfigurationManager.AppSettings["SAP_PRD"]);

            int FF = 0;

            try
            {

                BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1 OrderHeaderDT = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMTable OrderLineDT = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNRTable OrderPartnersDT = new BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNRTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDTable OrderConditionsDT = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXTTable OrderHeaderTextsDT = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXTTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLTable OrderScheLineDT = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARDTable OrderCreditCardDT = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARDTable();

                OrderHeaderDT.Doc_Type = _Order.OrderType.ToString();
                OrderHeaderDT.Sales_Org = _Order.OrgID;
                OrderHeaderDT.Distr_Chan = _Order.DistChannel;
                OrderHeaderDT.Division = _Order.Division;

                if (!String.IsNullOrEmpty(_Order.SalesGroup) && (!String.IsNullOrEmpty(_Order.SalesOffice)))
                {
                    OrderHeaderDT.Sales_Grp = _Order.SalesGroup;
                    OrderHeaderDT.Sales_Off = _Order.SalesOffice;
                }

                //Order is converted from which quote, need to be implement
                //if(! String.IsNullOrEmpty(.Ref_Doc.Trim)){
                //OrderHeader.Ref_Doc = .Ref_Doc
                //OrderHeader.Refdoc_Cat = "B"
                //}

                OrderHeaderDT.Currency = _Order.Currency;
                OrderHeaderDT.Doc_Date = LocalTime.ToString("yyyy/MM/dd");
                //OrderHeader.Price_Date = LocalTime.ToString("yyyy/MM/dd")   這一行在原來的版本就已被註解掉
                OrderHeaderDT.Incoterms1 = _Order.Incoterms1;
                OrderHeaderDT.Incoterms2 = _Order.Incoterms2;
                OrderHeaderDT.Taxdep_Cty = _Order.ShiptoCountry;
                OrderHeaderDT.Alttax_Cls = _Order.TaxClass;
                OrderHeaderDT.Eutri_Deal = _Order.TriangularIndicator;
                OrderHeaderDT.Ship_Cond = _Order.ShipCondition;
                OrderHeaderDT.Purch_No_C = _Order.CustomerPONumber;
                OrderHeaderDT.Purch_No_S = _Order.ShipToCustomerPONumber;
                if (_Order.RequireDate != null)
                {
                    OrderHeaderDT.Req_Date_H = string.Format("{0:yyyy/MM/dd}", _Order.RequireDate); //Need to be checked again
                }
                if (_Order.PODate != null)
                {
                    OrderHeaderDT.Purch_Date = string.Format("{0:yyyy/MM/dd}", _Order.PODate);//Need to be checked again
                }
                if (_Order.PartialShipment)
                {
                    OrderHeaderDT.Compl_Dlv = "0";
                }
                else
                {
                    OrderHeaderDT.Compl_Dlv = "1";
                }

                OrderHeaderDT.S_Proc_Ind = _Order.EarlyShipment;
                OrderHeaderDT.Taxdep_Cty = _Order.TAXDEL_CTY; //需要了解用途再改變數名稱
                OrderHeaderDT.Taxdst_Cty = _Order.TAXDES_CTY; //需要了解用途再改變數名稱

                if (string.IsNullOrEmpty(_Order.Version))
                {
                    OrderHeaderDT.Version = _Order.Version;
                }
                if (string.IsNullOrEmpty(_Order.District))
                {
                    OrderHeaderDT.Sales_Dist = _Order.District;
                }
                if (string.IsNullOrEmpty(_Order.PaymentTerm))
                {
                    OrderHeaderDT.Pmnttrms = _Order.PaymentTerm;
                }

                foreach (Product LineItem in _Order.LineItems)
                {
                    BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITM S_OrderLineRow = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITM();
                    BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDL S_ScheLineRow = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDL();
                    BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND S_ConditionRow = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND();

                    //Order Line
                    S_OrderLineRow.Part_Dlv = "";
                    S_OrderLineRow.Material = LineItem.PartNumber.ToUpper();
                    S_OrderLineRow.Hg_Lv_Item = LineItem.HigherLevel.ToString();
                    S_OrderLineRow.Itm_Number = LineItem.LineNumber.ToString();
                    S_OrderLineRow.Dlv_Group = LineItem.DeliveryGroup;
                    S_OrderLineRow.Plant = LineItem.PlantID;
                    S_OrderLineRow.Cust_Mat35 = LineItem.CustomerMaterial;
                    S_OrderLineRow.Usage_Ind = LineItem.DMF_Flag;
                    S_OrderLineRow.Short_Text = LineItem.CustomDescription;
                    S_OrderLineRow.Ship_Point = LineItem.ShipPoint;
                    S_OrderLineRow.Store_Loc = LineItem.StorageLoc;
                    OrderLineDT.Add(S_OrderLineRow);

                    //Schedule Line
                    S_ScheLineRow.Itm_Number = LineItem.LineNumber.ToString();
                    S_ScheLineRow.Req_Qty = LineItem.Quantity;
                    if (LineItem.RequireDate != null)
                    {
                        S_ScheLineRow.Req_Date = string.Format("{0:yyyy/MM/dd}", LineItem.RequireDate); //Need to be checked again
                    }
                    OrderScheLineDT.Add(S_ScheLineRow);

                    //Condition Line
                    if (LineItem.SellingPrice > 0)
                    {
                        S_ConditionRow.Cond_Value = LineItem.SellingPrice;
                    }
                    S_ConditionRow.Itm_Number = LineItem.LineNumber.ToString();
                    S_ConditionRow.Cond_Type = "ZPN0";
                    S_ConditionRow.Currency = _Order.Currency;

                    //*****JJ 2014/2/26：當TW01的單子有968T開頭的料號時，將ZTB1塞入ItCa欄位*****
                    //Need to be checked again

                    //ICC 2014/10/24 Check ItemCategoryGroup. If it is ZTN3 then do not add to condition
                    if (S_OrderLineRow.Item_Categ == null && S_OrderLineRow.Item_Categ == "ZTN3")
                    {
                    }
                    else
                    {
                        OrderConditionsDT.Add(S_ConditionRow);
                    }
                }

                //*****Order partner*****
                List<OrderPartner> _OrderPartnets = _Order.GetAllOrderPartnet();
                if (_OrderPartnets != null && _OrderPartnets.Count > 0)
                {
                    foreach (OrderPartner _OPartnet in _OrderPartnets)
                    {
                        BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNR PartnerRow = new BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNR();

                        switch (_OPartnet.Type)
                        {
                            case OrderPartnerType.SoldTo:
                                PartnerRow.Partn_Role = "AG";
                                break;
                            case OrderPartnerType.ShipTo:
                                PartnerRow.Partn_Role = "WE";
                                break;
                            case OrderPartnerType.BillTo:
                                PartnerRow.Partn_Role = "RE";
                                break;
                            case OrderPartnerType.Employee1:
                                PartnerRow.Partn_Role = "VE";
                                break;
                            case OrderPartnerType.Employee2:
                                PartnerRow.Partn_Role = "Z2";
                                break;
                            case OrderPartnerType.Employee3:
                                PartnerRow.Partn_Role = "Z3";
                                break;
                            case OrderPartnerType.KeyInPerson:
                                PartnerRow.Partn_Role = "RE";
                                break;
                            case OrderPartnerType.EndCoutomer:
                                PartnerRow.Partn_Role = "EM";
                                break;
                            case OrderPartnerType.EmployeeResponse:
                                PartnerRow.Partn_Role = "ZM";
                                break;
                        }

                        PartnerRow.Partn_Numb = _OPartnet.ErpID;
                        OrderPartnersDT.Add(PartnerRow);
                    }
                }

                //*****Notes*****
                string oLine = string.Empty;
                string textid = string.Empty;
                int StartP = 1;
                int LongP = 100;
                for (int j = 1; j < 6; j++)
                {
                    StartP = 1;
                    switch (j)
                    {
                        case 1:
                            oLine = _Order.SalesNote;
                            textid = "0001";
                            break;
                        case 2:
                            oLine = _Order.OrderNote;
                            textid = "0002";
                            break;
                        case 3:
                            oLine = _Order.OPNote;
                            textid = "ZEOP";
                            break;
                        case 4:
                            oLine = _Order.PrjNote;
                            textid = "ZPRJ";
                            break;
                        case 5:
                            oLine = _Order.BillingInstructionInfo;
                            textid = "ZBIL";
                            break;
                    }

                    if (!string.IsNullOrEmpty(oLine))
                    {
                        oLine = oLine.Substring(StartP, LongP);
                        while (oLine.Trim().Length > 0)
                        {
                            BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXT S_HeaderTextsRow = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXT();
                            S_HeaderTextsRow.Doc_Number = _Order.OrderNumber;
                            S_HeaderTextsRow.Text_Id = textid;
                            S_HeaderTextsRow.Text_Line = oLine;
                            S_HeaderTextsRow.Langu = "EN";
                            OrderHeaderTextsDT.Add(S_HeaderTextsRow);
                            StartP = StartP + 100;
                            oLine = _Order.SalesNote.Substring(StartP, LongP);
                        }
                    }

                }

                //*****Freight*****
                if (_Order.Freight != null && _Order.Freight.Count > 0)
                {
                    foreach (Freight _freight in _Order.Freight)
                    {
                        BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND S_ConditionRow = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND();
                        S_ConditionRow.Itm_Number = "000000";
                        S_ConditionRow.Cond_Type = _freight.Type.ToString();
                        S_ConditionRow.Currency = _freight.Currency.ToString();
                        S_ConditionRow.Cond_Value = _freight.Price;
                        OrderConditionsDT.Add(S_ConditionRow);
                    }
                }


                //*****Credit Cart*****
                if (_Order.CreditCartInfo != null)
                {
                    BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARD S_CreditCardRow = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARD();
                    S_CreditCardRow.Cc_Name = _Order.CreditCartInfo.Holder;
                    S_CreditCardRow.Cc_Number = _Order.CreditCartInfo.Number;
                    S_CreditCardRow.Cc_Type = _Order.CreditCartInfo.Type;
                    S_CreditCardRow.Cc_Valid_T = _Order.CreditCartInfo.Expired;
                    S_CreditCardRow.Cc_Verif_Value = _Order.CreditCartInfo.VerificationValue;
                    OrderCreditCardDT.Add(S_CreditCardRow);
                }

                //Create SAP connection
                //Boolean IsTesting = true;
                if (!IsTest)
                {
                    //BAPICreateOrder.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings["SAP_PRD"]);
                }
                else
                {
                    //BAPICreateOrder.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings["SAP_Test"]);
                    BAPICreateOrder.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings["SAPConnTest"]);
                    //ConfigurationManager.ConnectionStrings["SAP_Test"].ConnectionString
                }
                BAPICreateOrder.Connection.Open();

                String strRelationType = "";
                String strPConvert = "";
                String strpintnumassign = "";
                String strPTestRun = "";
                BAPI_SALESORDER_CREATEFROMDAT2.BAPIRET2Table retTable = new BAPI_SALESORDER_CREATEFROMDAT2.BAPIRET2Table();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPIPAREXTable Parex = new BAPI_SALESORDER_CREATEFROMDAT2.BAPIPAREXTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICUBLBTable Cublb = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICUBLBTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICUINSTable Cuins = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICUINSTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICUPRTTable Cuprt = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICUPRTTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICUCFGTable Cucfg = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICUCFGTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICUREFTable Curef = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICUREFTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVALTable Cuval = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVALTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVKTable Cuvk = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVKTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDXTable Condx = new BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDXTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMXTable Sditmx = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMXTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPISDKEYTable Sdkey = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDKEYTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLXTable Schdlx = new BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLXTable();
                BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1Table Addr1 = new BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1Table();

                string refDoc_Number = _Order.OrderNumber;
                refDoc_Number = "";
                //*****Create SAP order*****
                BAPICreateOrder.Bapi_Salesorder_Createfromdat2(ErrMsg,
                strRelationType,
                strPConvert,
                strpintnumassign,
                new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDLS(),
                OrderHeaderDT,
                new BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1X(),
                refDoc_Number,
                new BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SENDER(),
                strPTestRun,
                out refDoc_Number,
                ref Parex,
                ref OrderCreditCardDT,
                ref Cublb,
                ref Cuins,
                ref Cuprt,
                ref Cucfg,
                ref Curef,
                ref Cuval,
                ref Cuvk,
                ref OrderConditionsDT,
                ref Condx,
                ref OrderLineDT,
                ref Sditmx,
                ref Sdkey,
                ref OrderPartnersDT,
                ref OrderScheLineDT,
                ref Schdlx,
                ref OrderHeaderTextsDT,
                ref Addr1,
                ref retTable);

                _Order.ConvertToOrderResult = retTable.ToADODataTable();
                foreach (DataRow _row in _Order.ConvertToOrderResult.Rows)
                {
                    if ((string)_row["Number"] == "311")
                    {
                        FF = 1;
                        break;
                    }
                }

                if (FF == 1)
                {
                    BAPICreateOrder.CommitWork();
                }
                BAPICreateOrder.Connection.Close();

                if (OrderCreditCardDT.Count > 0 && FF == 1)
                {

                    //20120726 TC: Try to sleep two seconds to see if this can tick authorization block successfully
                    ZSD_UPDATE_FPLA.ZSD_UPDATE_FPLA pAuthBlock = new ZSD_UPDATE_FPLA.ZSD_UPDATE_FPLA();
                    ZSD_UPDATE_FPLA.SWF_LINESTable DT_LOG = new ZSD_UPDATE_FPLA.SWF_LINESTable();
                    DataTable dt = new DataTable();
                    pAuthBlock.Connection = new SAP.Connector.SAPConnection(BAPICreateOrder.ConnectionString);
                    pAuthBlock.Connection.Open();

                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(1);
                        pAuthBlock.Zsd_Update_Fpla("X", refDoc_Number, ref DT_LOG);
                        dt = DT_LOG.ToADODataTable();
                        if (dt.Rows.Count > 0 && dt.Rows[0]["Line"].ToString().Contains("successful"))
                        {
                            break;
                        }
                    }

                    if (dt.Rows.Count > 0 && (!dt.Rows[0]["Line"].ToString().Contains("successful")))
                    {
                        //InsertMyErrLog("Failed to tick Authorization Block for SO " & refDoc_Number)
                    }

                }

            }
            catch (Exception mex)
            {
                if (BAPICreateOrder != null && BAPICreateOrder.Connection != null)
                {
                    BAPICreateOrder.Connection.Close();
                }
                ErrMsg = mex.ToString();
                return false;
            }

            if (FF == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get part's Special Procurement
        /// </summary>
        /// <param name="PartNumber"></param>
        /// <param name="Plant"></param>
        /// <returns></returns>
        public static string GetSpecialProcurement(string PartNumber, string Plant)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.Append(" Select a.SOBSL as SpecialProcurement From saprdp.MARC a ");
            _sql.Append(" Where a.mandt='168' and a.werks='" + Plant + "' ");
            _sql.Append(" And a.matnr='" + FormatToSAPPartNo(PartNumber) + "' ");
            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", _sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["SpecialProcurement"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Get part's shipping via condition by loading group
        /// </summary>
        /// <param name="PartNumber"></param>
        /// <param name="Plant"></param>
        /// <returns>0001=Air,0002=Sea</returns>
        public static string GetShippingVia(string PartNumber, string Plant)
        {
            StringBuilder _sql = new StringBuilder();
            //Frank: use Transportation Group
            //_sql.Append(" select TRAGR as ShipVia from saprdp.MARA b ");
            //_sql.Append(" WHERE b.MATNR='" + FormatToSAPPartNo(PartNumber) + "' ");
            //Option Code and Display Name Mapping table:select * from saprdp.TTGRT where MANDT=168 and SPRAS='E'

            //Ivy.Sit 20150723
            //1.	Please use loading group to determine air/ocean route
            //2.	Yes, please always use USH1 material master for the default ship mode.  The TWH1 or CKH2 ship mode only determines how parts are being ship to TW & CN
            //Frank use Loading Group
            _sql.Append(" SELECT LADGR as ShipVia from saprdp.MARC b ");
            _sql.Append(" WHERE b.MATNR='" + FormatToSAPPartNo(PartNumber) + "' ");
            _sql.Append(" AND b.WERKS='" + Plant + "'");
            //Option Code and Display Name Mapping table:select * from saprdp.TLGRT where MANDT=168 and SPRAS='E'

            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", _sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ShipVia"].ToString();
            }
            return string.Empty;
        }


        /// <summary>
        /// Unblock SO GP line by line
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="OrgID"></param>
        /// <param name="IsTesting"></param>
        /// <returns></returns>
        public static Boolean UnblockSOGPWithZMIPCheck(String OrderNo, String OrgID, Boolean IsTesting)
        {
            String sap_connstr = (IsTesting == true) ? "SAP_Test" : "SAP_PRD";
            String sap_appstr = (IsTesting == true) ? "SAPConnTest" : "SAP_PRD";

            String sqlSOGPBlockLines = " select POSNR, LSSTA from saprdp.vbup where LSSTA='C' and vbeln='" + OrderNo + "' ";
            DataTable dtSOGPLines = OracleProvider.GetDataTable(sap_connstr, sqlSOGPBlockLines);

            if (dtSOGPLines != null && dtSOGPLines.Rows.Count > 0)
            {
                Z_RELEASE_GP_ITEM.Z_RELEASE_GP_ITEM releaseGPBAI = new Z_RELEASE_GP_ITEM.Z_RELEASE_GP_ITEM(ConfigurationManager.AppSettings[sap_appstr]);
                releaseGPBAI.Connection.Open();

                //Frank 20180619 Get item's ZMIP
                String _errmsg = string.Empty;

                MyAdvantechDAL mydal = new MyAdvantechDAL();
                ORDER_MASTER _OMaster = MyAdvantechDAL.GetOrderMaster(OrderNo);

                List<ORDER_DETAIL> _ODetail = MyAdvantechDAL.GetOrderDetail(OrderNo);

                Order _order = new Order();
                _order.OrgID = OrgID;
                _order.Currency = _OMaster.CURRENCY;
                _order.DistChannel = "10";
                _order.Division = "00";
                _order.SetOrderPartnet(new OrderPartner(_OMaster.SOLDTO_ID, OrgID, OrderPartnerType.SoldTo));
                _order.SetOrderPartnet(new OrderPartner(_OMaster.SOLDTO_ID, OrgID, OrderPartnerType.ShipTo));
                _order.SetOrderPartnet(new OrderPartner(_OMaster.SOLDTO_ID, OrgID, OrderPartnerType.BillTo));

                StringBuilder _partstr = new StringBuilder();
                foreach (ORDER_DETAIL _LineItem in _ODetail)
                {
                    _order.AddLooseItem(_LineItem.PART_NO, _LineItem.DeliveryPlant, (int)_LineItem.LINE_NO, 1);
                    _partstr.Append("'" + _LineItem.PART_NO + "',");
                }
                char[] charsToTrim = { ',' };

                //Get line item which product type is ZOEM from SAP_PRODUCT
                DataTable _myadt = SqlProvider.dbGetDataTable("MY", "select PART_NO,PRODUCT_TYPE from SAP_PRODUCT where PRODUCT_TYPE='ZOEM' and PART_NO in (" + _partstr.ToString().TrimEnd(charsToTrim) + ")");

                //Call GP Block simulation RFC to get part's minimum price
                SimulateOrderGPBlock(ref _order, ref _errmsg);


                foreach (DataRow GPLineRow in dtSOGPLines.Rows)
                {
                    int _returnval = 0;
                    String _partno = String.Empty;
                    //Get line number from GP block table
                    String _lineNostr = RemovePrecedingZeros(GPLineRow["POSNR"].ToString());
                    int _lineNo = 1;
                    int.TryParse(_lineNostr, out _lineNo);
                    Product _selectedpart = _order.LineItems.Where(d => d.LineNumber == _lineNo).FirstOrDefault();
                    if (_selectedpart != null)
                    {
                        _partno = _selectedpart.PartNumber;
                        //If _MatchedRow's count is more then 0, then the part's product type is ZOEM
                        DataRow[] _MatchedRow = _myadt.Select("PART_NO='" + _partno + "'");
                        // if it is ZOEM and minimum price is 0 then ignore to unblock the part
                        if ((_MatchedRow != null && _MatchedRow.Length > 0) && _selectedpart.MinimumPrice == 0)
                        {
                            continue;
                        }
                    }

                    try
                    {
                        releaseGPBAI.Z_Release_Gp_Item(GPLineRow["POSNR"].ToString(), OrderNo, "", out _returnval);
                    }
                    catch (Exception ex)
                    {
                        releaseGPBAI.Connection.Close();
                        return false;
                    }
                }
                releaseGPBAI.Connection.Close();
            }
            return true;
        }


        //public static bool GetPrice(List<Part> parts, ref string errorMsg)
        //{
        //    if (parts.Count == 0) return false;
        //    Part part = parts.FirstOrDefault();
        //    string SoldToId = part.SoldToErpID;
        //    string Org = part.OrgID;
        //    string PlantID = part.PlantID;
        //    string Currency = part.Currency;
        //    if (string.IsNullOrEmpty(PlantID) || string.IsNullOrEmpty(Currency))
        //    {
        //        object OrgID = SqlProvider.dbExecuteScalar("MY", string.Format(" select top 1 isnull(ORG_ID,'') as OrgID  from  SAP_DIMCOMPANY  where COMPANY_ID='{0}'", SoldToId));
        //        if (OrgID != null && !string.IsNullOrEmpty(OrgID.ToString()))
        //        {
        //            if (string.IsNullOrEmpty(PlantID))
        //            {
        //                switch (OrgID.ToString().Trim())
        //                {
        //                    case "EU10":
        //                        PlantID = "EUH1";
        //                        break;
        //                    case "TW01":
        //                        PlantID = "TWH1";
        //                        break;
        //                    case "US01":
        //                        PlantID = "USH1";
        //                        break;
        //                    default:
        //                        PlantID = "EUH1";
        //                        break;
        //                }
        //            }
        //            if (string.IsNullOrEmpty(Currency))
        //            {
        //                switch (OrgID.ToString().Trim())
        //                {
        //                    case "EU10":
        //                        Currency = "EUR";
        //                        break;
        //                    case "TW01":
        //                        Currency = "NT";
        //                        break;
        //                    case "US01":
        //                        Currency = "USD";
        //                        break;
        //                    case "CN10":
        //                        Currency = "CNY";
        //                        break;
        //                    default:
        //                        Currency = "EUR";
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    string ShipToId = SoldToId;
        //    if (part.ShipToErpID != null && string.IsNullOrEmpty(part.ShipToErpID)) ShipToId = part.ShipToErpID;
        //    string strDistChann = "10";
        //    string strDivision = "00";
        //    if (Org == "US01")
        //    {
        //        if (SoldToId.Equals("UAON00001", System.StringComparison.OrdinalIgnoreCase))
        //        {
        //            strDistChann = "30";
        //            strDivision = "10";
        //        }
        //        else
        //        {
        //            var N = SqlProvider.dbExecuteScalar("MY", string.Format("select COUNT(COMPANY_ID) from SAP_DIMCOMPANY " + " where SALESOFFICE in ('2300','2700') and COMPANY_ID='{0}' and ORG_ID='US01'", SoldToId));
        //            if (Int32.Parse(N.ToString()) > 0)
        //            {
        //                strDistChann = "10";
        //                strDivision = "20";
        //            }
        //            else
        //            {
        //                strDistChann = "30";
        //                strDivision = "10";
        //            }
        //        }
        //    }

        //    bool HasPhaseOutItem = false;
        //    ArrayList phaseOutItems = new ArrayList();
        //    DataTable ZSWLItemSet = new DataTable();
        //    var _with1 = ZSWLItemSet.Columns;
        //    _with1.Add("PartNo");
        //    _with1.Add("Qty", typeof(int));
        //    bool RemoveAddedItem = false;
        //    string AddedItemLineNo = "";

        //    BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE proxy1 = new BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings["SAP_PRD"]);

        //    if (SoldToId == "SAID")
        //        proxy1.ConnectionString = ConfigurationManager.AppSettings["SAPConnTest"];
        //    BAPI_SALESORDER_SIMULATE.BAPISDHEAD OrderHeader = new BAPI_SALESORDER_SIMULATE.BAPISDHEAD();
        //    BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable Partners = new BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable();
        //    BAPI_SALESORDER_SIMULATE.BAPIITEMINTable ItemsIn = new BAPI_SALESORDER_SIMULATE.BAPIITEMINTable();
        //    BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable ItemsOut = new BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable();
        //    BAPI_SALESORDER_SIMULATE.BAPICONDTable Conditions = new BAPI_SALESORDER_SIMULATE.BAPICONDTable();
        //    var _with2 = OrderHeader;
        //    _with2.Doc_Type = "ZOR";
        //    _with2.Sales_Org = Org;
        //    _with2.Distr_Chan = strDistChann;
        //    _with2.Division = strDivision;
        //    if (Org == "BR01")
        //        _with2.Doc_Type = "ZORB";
        //    if (!string.IsNullOrEmpty(Currency.Trim()))
        //        _with2.Currency = Currency;
        //    int LineNo = 1;
        //    System.Data.SqlClient.SqlConnection sqlMA = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
        //    sqlMA.Open();
        //    foreach (Part partitem in parts)
        //    {
        //        //if (partitem.Qty == 0) partitem.Qty = 1;
        //        string chkSql = " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + " where a.part_no='" + partitem.PartNo.ToMyAdvantechPart() + "' and a.product_status in ('A','N','H','O','S5','V','M1','') and a.sales_org='" + Org + "' ";
        //        DataTable chkDt = new DataTable();
        //        System.Data.SqlClient.SqlDataAdapter sqlAptr = new System.Data.SqlClient.SqlDataAdapter(chkSql, sqlMA);
        //        try
        //        {
        //            sqlAptr.Fill(chkDt);
        //        }
        //        catch (System.Data.SqlClient.SqlException ex)
        //        {
        //            sqlMA.Close();
        //            errorMsg = ex.ToString();
        //            return false;
        //        }
        //        if (chkDt.Rows.Count > 0 && (Org != "TW01" | (Org == "TW01") & chkDt.Rows[0]["ProfitCenter"] != "N/A"))
        //        {
        //            if (chkDt.Rows[0]["ITEM_CATEGORY_GROUP"].ToString() != "ZSWL")
        //            {
        //                BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
        //                item.Itm_Number = FormatItmNumber(LineNo);
        //                item.Material = FormatToSAPPartNo(partitem.PartNo.ToUpper());
        //                //item.Req_Qty = partitem.QTY.ToString();
        //                item.Req_Qty = (Convert.ToInt32(partitem.Qty) * 1000).ToString();
        //                ItemsIn.Add(item);
        //                LineNo += 1;
        //            }
        //            else
        //            {
        //                DataRow zr = ZSWLItemSet.NewRow();
        //                zr["PartNo"] = partitem.PartNo.ToUpper();
        //                zr["Qty"] = partitem.Qty;
        //                ZSWLItemSet.Rows.Add(zr);
        //            }
        //        }
        //        else
        //        {
        //            phaseOutItems.Add(partitem.PartNo.ToUpper());
        //        }
        //    }
        //    sqlMA.Close();

        //    if (ItemsIn.Count == 0 && ZSWLItemSet.Rows.Count > 0)
        //    {
        //        BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
        //        item.Itm_Number = FormatItmNumber(LineNo);
        //        item.Material = GetAHighLevelItemForPricing(Org);
        //        item.Req_Qty = "1";
        //        item.Req_Qty = (Convert.ToInt32(item.Req_Qty) * 1000).ToString();
        //        ItemsIn.Add(item);
        //        RemoveAddedItem = true;
        //        AddedItemLineNo = LineNo.ToString();
        //        LineNo += 1;
        //    }
        //    if (ItemsIn.Count > 0 && ZSWLItemSet.Rows.Count > 0)
        //    {
        //        foreach (DataRow r in ZSWLItemSet.Rows)
        //        {
        //            BAPI_SALESORDER_SIMULATE.BAPIITEMIN item = new BAPI_SALESORDER_SIMULATE.BAPIITEMIN();
        //            item.Itm_Number = FormatItmNumber(LineNo);
        //            item.Material = FormatToSAPPartNo(r["PartNo"].ToString().Trim().ToUpper());
        //            item.Req_Qty = r["Qty"].ToString();
        //            item.Plant = PlantID;
        //            item.Req_Qty = (Convert.ToInt32(item.Req_Qty) * 1000).ToString();
        //            item.Hg_Lv_Item = "1";
        //            ItemsIn.Add(item);
        //            LineNo += 1;
        //        }
        //    }
        //    BAPI_SALESORDER_SIMULATE.BAPIPARTNR SoldTo = new BAPI_SALESORDER_SIMULATE.BAPIPARTNR();
        //    BAPI_SALESORDER_SIMULATE.BAPIPARTNR ShipTo = new BAPI_SALESORDER_SIMULATE.BAPIPARTNR();
        //    BAPI_SALESORDER_SIMULATE.BAPIRET2Table retDt = new BAPI_SALESORDER_SIMULATE.BAPIRET2Table();
        //    SoldTo.Partn_Role = "AG";
        //    SoldTo.Partn_Numb = SoldToId;
        //    ShipTo.Partn_Role = "WE";
        //    ShipTo.Partn_Numb = ShipToId;
        //    Partners.Add(SoldTo);
        //    Partners.Add(ShipTo);
        //    proxy1.Connection.Open();

        //    try
        //    {
        //        DataTable dtItem = new DataTable();
        //        DataTable dtPartNr = new DataTable();
        //        DataTable dtcon = new DataTable();
        //        DataTable DTRET = new DataTable();

        //        dtItem = ItemsIn.ToADODataTable();
        //        dtPartNr = Partners.ToADODataTable();
        //        dtcon = Conditions.ToADODataTable();
        //        string outstr = "";
        //        BAPI_SALESORDER_SIMULATE.BAPIPAYER BAPIPAYER = new BAPI_SALESORDER_SIMULATE.BAPIPAYER();
        //        BAPI_SALESORDER_SIMULATE.BAPIRETURN BAPIRETURN = new BAPI_SALESORDER_SIMULATE.BAPIRETURN();
        //        BAPI_SALESORDER_SIMULATE.BAPISHIPTO BAPISHIPTO = new BAPI_SALESORDER_SIMULATE.BAPISHIPTO();
        //        BAPI_SALESORDER_SIMULATE.BAPISOLDTO BAPISOLDTO = new BAPI_SALESORDER_SIMULATE.BAPISOLDTO();
        //        BAPI_SALESORDER_SIMULATE.BAPIPAREXTable BAPIPAREXTable = new BAPI_SALESORDER_SIMULATE.BAPIPAREXTable();
        //        BAPI_SALESORDER_SIMULATE.BAPICCARDTable BAPICCARDTable = new BAPI_SALESORDER_SIMULATE.BAPICCARDTable();
        //        BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable BAPICCARD_EXTable = new BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable();
        //        BAPI_SALESORDER_SIMULATE.BAPICUBLBTable BAPICUBLBTable = new BAPI_SALESORDER_SIMULATE.BAPICUBLBTable();
        //        BAPI_SALESORDER_SIMULATE.BAPICUINSTable BAPICUINSTable = new BAPI_SALESORDER_SIMULATE.BAPICUINSTable();
        //        BAPI_SALESORDER_SIMULATE.BAPICUPRTTable BAPICUPRTTable = new BAPI_SALESORDER_SIMULATE.BAPICUPRTTable();
        //        BAPI_SALESORDER_SIMULATE.BAPICUCFGTable BAPICUCFGTable = new BAPI_SALESORDER_SIMULATE.BAPICUCFGTable();
        //        BAPI_SALESORDER_SIMULATE.BAPICUVALTable BAPICUVALTable = new BAPI_SALESORDER_SIMULATE.BAPICUVALTable();
        //        BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable BAPIINCOMPTable = new BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable();
        //        BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable BAPISDHEDUTable = new BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable();
        //        BAPI_SALESORDER_SIMULATE.BAPISCHDLTable BAPISCHDLTable = new BAPI_SALESORDER_SIMULATE.BAPISCHDLTable();
        //        BAPI_SALESORDER_SIMULATE.BAPIADDR1Table BAPIADDR1Table = new BAPI_SALESORDER_SIMULATE.BAPIADDR1Table();

        //        proxy1.Bapi_Salesorder_Simulate("", OrderHeader,
        //          out   BAPIPAYER,
        //          out   BAPIRETURN,
        //          out   outstr,
        //          out  BAPISHIPTO,
        //         out   BAPISOLDTO,
        //         ref   BAPIPAREXTable,
        //         ref    retDt, ref BAPICCARDTable,
        //         ref   BAPICCARD_EXTable,
        //         ref   BAPICUBLBTable,
        //        ref   BAPICUINSTable,
        //        ref   BAPICUPRTTable,
        //        ref    BAPICUCFGTable,
        //        ref   BAPICUVALTable,
        //        ref   Conditions,
        //        ref   BAPIINCOMPTable,
        //        ref   ItemsIn,
        //        ref   ItemsOut,
        //        ref   Partners,
        //        ref   BAPISDHEDUTable,
        //        ref   BAPISCHDLTable,
        //        ref   BAPIADDR1Table);
        //        DataTable retAdoDt = retDt.ToADODataTable();


        //        foreach (DataRow retMsgRec in retAdoDt.Rows)
        //        {
        //            if (retMsgRec["Type"].ToString() == "E")
        //            {
        //                HasPhaseOutItem = true;
        //                errorMsg += string.Format("{0}", retMsgRec["Message"]);
        //            }
        //        }

        //        DataTable ConditionOut = Conditions.ToADODataTable();
        //        DataTable PInDt = ItemsIn.ToADODataTable();
        //        DataTable POutDt = ItemsOut.ToADODataTable();

        //        //gv2.DataSource = retAdoDt : gv2.DataBind()

        //        DTRET = retDt.ToADODataTable();

        //        //ProductOut = new SAPDALDS.ProductOutDataTable();
        //        foreach (DataRow PIn in PInDt.Rows)
        //        {

        //            //SAPDALDS.ProductOutRow poutRec = ProductOut.NewProductOutRow();
        //            //poutRec.PART_NO = RemovePrecedingZeros(PIn["Material"].ToString());
        //            //poutRec.LIST_PRICE = 0;
        //            //poutRec.RECYCLE_FEE = 0;

        //            Part _part = parts.First(p => p.PartNo == RemovePrecedingZeros(PIn["Material"].ToString()));
        //            decimal outdb = 0;
        //            DataRow[] rs2 = ConditionOut.Select("Itm_Number='" + PIn["Itm_Number"] + "'");
        //            foreach (DataRow r in rs2)
        //            {
        //                switch (r["Cond_Type"].ToString().ToUpper())
        //                {

        //                    case "ZPN0":
        //                    case "ZPR0":
        //                        if (decimal.TryParse(_part.ListPrice.ToString(), out outdb) && decimal.TryParse(r["Cond_Value"].ToString(), out outdb))
        //                        {
        //                            decimal _liseprice = decimal.Parse(r["Cond_Value"].ToString());
        //                            if (_liseprice > _part.ListPrice)
        //                            {
        //                                _part.ListPrice = (decimal)_liseprice;
        //                            }
        //                        }
        //                        break;
        //                    case "ZHB0":
        //                        // poutRec.RECYCLE_FEE = Strings.FormatNumber(r["Cond_Value"], 2);
        //                        break;
        //                }
        //            }
        //            DataRow[] POutRs = POutDt.Select("Itm_Number='" + PIn["Itm_Number"] + "'");
        //            if (PIn["Material"].ToString().IsNumberPart())
        //            {
        //                if (_part.ListPrice <= 0 && POutRs.Length > 0)
        //                {
        //                    _part.ListPrice = decimal.Parse(POutRs[0]["net_value1"].ToString()) / decimal.Parse(POutRs[0]["req_qty"].ToString());
        //                }
        //            }
        //            if (POutRs.Length > 0)
        //            {
        //                // poutRec.TAX = POutRs[0]["Tx_Doc_Cur"] / POutRs[0]["req_qty"];
        //                _part.UnitPrice = decimal.Parse(POutRs[0]["Net_Value1"].ToString()) / decimal.Parse(POutRs[0]["req_qty"].ToString());
        //                if (Org == "BR01")
        //                {
        //                    DataRow[] cond_rs = ConditionOut.Select("Cond_Type='ZPR0' AND Itm_Number='" + PIn["Itm_Number"] + "'");
        //                    if (cond_rs.Length > 0)
        //                    {
        //                        _part.ListPrice = decimal.Parse(cond_rs[0]["Cond_Value"].ToString());
        //                    }
        //                    //poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
        //                }
        //            }
        //            //if (!RemoveAddedItem | (RemoveAddedItem & RemovePrecedingZeros(PIn["Itm_Number"].ToString()) != AddedItemLineNo))
        //            //{
        //            //    ProductOut.Rows.Add(poutRec);
        //            //}

        //        }
        //        foreach (string itm in phaseOutItems)
        //        {
        //            Part _part = parts.First(p => p.PartNo == RemovePrecedingZeros(itm));

        //            _part.ListPrice = 0;
        //            //    pout.RECYCLE_FEE = 0;
        //            _part.UnitPrice = 0;
        //            //    ProductOut.AddProductOutRow(pout);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg += System.Environment.NewLine + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString();
        //        proxy1.Connection.Close();
        //        return false;
        //    }
        //    proxy1.Connection.Close();
        //    //if (HasPhaseOutItem)
        //    //{
        //    //    return GetEUPrice(Org, SoldToId, ShipToId, strDistChann, strDivision, ProductIn, ProductOut, ErrorMessage);
        //    //}
        //    //foreach (SAPDALDS.ProductOutRow pOutRow in ProductOut.Rows)
        //    //{
        //    //    if (Information.IsNumeric(pOutRow.LIST_PRICE) && Information.IsNumeric(pOutRow.UNIT_PRICE) && Convert.ToDouble(pOutRow.LIST_PRICE) < Convert.ToDouble(pOutRow.UNIT_PRICE))
        //    //    {
        //    //        pOutRow.LIST_PRICE = pOutRow.UNIT_PRICE;
        //    //    }
        //    //}
        //    //ProductOut.AcceptChanges();
        //    foreach (Part ipart in parts)
        //    {
        //        if (ipart.ListPrice < ipart.UnitPrice) ipart.ListPrice = ipart.UnitPrice;
        //    }
        //    if (string.IsNullOrEmpty(errorMsg) == false)
        //        return false;
        //    return true;
        //}
        /// <summary>
        ///  Create SAP Account
        /// </summary>
        /// <param name="applicationID"></param>
        /// <param name="type"></param>
        /// <param name="istesting"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string CreateSAPAccount(int applicationID, companyType type, bool istesting, ref string msg)
        {

            MyAdminDAL _MyAdminDAL = new MyAdminDAL();
            SA_APPLICATION ap = _MyAdminDAL.getApplicationByID(applicationID);

            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient("172.20.0.76");
            smtpClient1.Send("myadvantech@advantech.com", "myadvantech@advantech.com", string.Format("CreateSAPAccount:  AppNO:{0}  , Type:{1}", ap.AplicationNO, type.ToString()), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            int _type = (int)type;
            SA_APPLICATION2COMPANY A2C = ap.SA_APPLICATION2COMPANY.FirstOrDefault(p => p.CompanyType == _type);
            string strCountryCode = "TW";
            var p1 = new SAPCustomerRFC.SAPCustomerRFC();
            string SAPconnection = "SAP_PRD"; if (istesting) SAPconnection = "SAPConnTest";
            p1.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings[SAPconnection]);
            var I_Bapiaddr1 = new SAPCustomerRFC.BAPIADDR1();
            var I_Bapiaddr2 = new SAPCustomerRFC.BAPIADDR2();
            var I_Kna1 = new SAPCustomerRFC.KNA1();
            var I_Knb1 = new SAPCustomerRFC.KNB1();
            var I_Knvv = new SAPCustomerRFC.KNVV();
            var O_Kna1 = new SAPCustomerRFC.KNA1();
            var T_Upd_Txt = new SAPCustomerRFC.FKUNTXTTable(); var T_Xkn = new SAPCustomerRFC.FKNASTable();
            var T_Xknb5 = new SAPCustomerRFC.FKNB5Table(); var T_Xknbk = new SAPCustomerRFC.FKNBKTable();
            var T_Xknex = new SAPCustomerRFC.FKNEXTable(); var T_Xknva = new SAPCustomerRFC.FKNVATable();
            var T_Xknvd = new SAPCustomerRFC.FKNVDTable(); var T_Xknvi = new SAPCustomerRFC.FKNVITable();
            var T_Xknvk = new SAPCustomerRFC.FKNVKTable(); var T_Xknvl = new SAPCustomerRFC.FKNVLTable();
            var T_Xknvp = new SAPCustomerRFC.FKNVPTable(); var T_Xknza = new SAPCustomerRFC.FKNZATable();
            var T_Ykn = new SAPCustomerRFC.FKNASTable(); var T_Yknb5 = new SAPCustomerRFC.FKNB5Table();
            var T_Yknbk = new SAPCustomerRFC.FKNBKTable(); var T_Yknex = new SAPCustomerRFC.FKNEXTable();
            var T_Yknva = new SAPCustomerRFC.FKNVATable(); var T_Yknvd = new SAPCustomerRFC.FKNVDTable();
            var T_Yknvi = new SAPCustomerRFC.FKNVITable(); var T_Yknvk = new SAPCustomerRFC.FKNVKTable();
            var T_Yknvl = new SAPCustomerRFC.FKNVLTable(); var T_Yknvp = new SAPCustomerRFC.FKNVPTable();
            var T_Yknza = new SAPCustomerRFC.FKNZATable();
            var Pi_Add_On_Data = new SAPCustomerRFC.CUST_ADD_ON_DATA();
            string Pi_Cam_Changed = "";
            string Pi_Postflag = "";
            string E_Kunnr = ""; string E_Sd_Cust_1321_Done = "";
            string I_Maintain_Address_By_Kna1 = "X";
            string I_Knb1_Reference = "";
            string I_No_Bank_Master_Update = "";
            string I_Raise_No_Bte = "";
            string I_Customer_Is_Consumer = "";
            string I_Force_External_Number_Range = "";
            string I_From_Customermaster = "";
            string Taxid = ""; string AAG = "";
            GetCustomerAcctAssgmtGroupAndTaxClassification(strCountryCode, ref AAG, ref Taxid);
            ICollection<SA_FKNVI> knvi = A2C.SA_FKNVI;

            //foreach (SA_FKNVI item in knvi)
            //{
            //    var T_Xknvidr = new SAPCustomerRFC.FKNVI()
            //    {
            //        Tatyp = item.Tatyp,
            //        Aland = item.Aland,
            //        Kunnr = item.Kunnr,
            //        Mandt = item.Mandt,
            //        Taxkd = item.Taxkd,
            //        Kz=""
            //    };
            //    T_Xknvi.Add(T_Xknvidr);
            //}

            SA_BAPIADDR1 currADDR1 = A2C.SA_BAPIADDR1.FirstOrDefault();
            I_Bapiaddr1.Addr_No = currADDR1.Addr_No;
            I_Bapiaddr1.Adr_Notes = currADDR1.Adr_Notes;
            I_Bapiaddr1.Build_Long = currADDR1.Build_Long;
            I_Bapiaddr1.Building = currADDR1.Building;
            I_Bapiaddr1.C_O_Name = currADDR1.C_O_Name;
            I_Bapiaddr1.Chckstatus = currADDR1.Chckstatus;
            I_Bapiaddr1.City = currADDR1.City;
            I_Bapiaddr1.City_No = currADDR1.City_No;
            I_Bapiaddr1.Comm_Type = currADDR1.Comm_Type;
            I_Bapiaddr1.Country = currADDR1.Country;
            I_Bapiaddr1.Countryiso = currADDR1.Countryiso;
            I_Bapiaddr1.County = currADDR1.County;
            I_Bapiaddr1.County_Code = currADDR1.County_Code;
            I_Bapiaddr1.Deli_Serv_Number = currADDR1.Deli_Serv_Number;
            I_Bapiaddr1.Deli_Serv_Type = currADDR1.Deli_Serv_Type;
            I_Bapiaddr1.Deliv_Dis = currADDR1.Deliv_Dis;
            I_Bapiaddr1.Distrct_No = currADDR1.Distrct_No;
            I_Bapiaddr1.District = currADDR1.District;
            I_Bapiaddr1.Dont_Use_P = currADDR1.Dont_Use_P;
            I_Bapiaddr1.Dont_Use_S = currADDR1.Dont_Use_S;
            I_Bapiaddr1.E_Mail = currADDR1.E_Mail;
            I_Bapiaddr1.Fax_Extens = currADDR1.Fax_Extens;
            I_Bapiaddr1.Fax_Number = currADDR1.Fax_Number;
            I_Bapiaddr1.Floor = currADDR1.Floor;
            I_Bapiaddr1.Formofaddr = currADDR1.Formofaddr;
            I_Bapiaddr1.Home_City = currADDR1.Home_City;
            I_Bapiaddr1.Homecityno = currADDR1.Homecityno;
            I_Bapiaddr1.Homepage = currADDR1.Homepage;
            I_Bapiaddr1.House_No = currADDR1.House_No;
            I_Bapiaddr1.House_No2 = currADDR1.House_No2;
            I_Bapiaddr1.House_No3 = currADDR1.House_No3;
            I_Bapiaddr1.Langu = currADDR1.Langu;
            I_Bapiaddr1.Langu_Cr = currADDR1.Langu_Cr;
            I_Bapiaddr1.Langu_Iso = currADDR1.Langu_Iso;
            I_Bapiaddr1.Langucriso = currADDR1.Langucriso;
            I_Bapiaddr1.Location = currADDR1.Location;
            I_Bapiaddr1.Name = currADDR1.Name;
            I_Bapiaddr1.Name_2 = currADDR1.Name_2;
            I_Bapiaddr1.Name_3 = currADDR1.Name_3;
            I_Bapiaddr1.Name_4 = currADDR1.Name_4;
            I_Bapiaddr1.Pboxcit_No = currADDR1.Pboxcit_No;
            I_Bapiaddr1.Pcode1_Ext = currADDR1.Pcode1_Ext;
            I_Bapiaddr1.Pcode2_Ext = currADDR1.Pcode2_Ext;
            I_Bapiaddr1.Pcode3_Ext = currADDR1.Pcode3_Ext;
            I_Bapiaddr1.Po_Box = currADDR1.Po_Box;
            I_Bapiaddr1.Po_Box_Cit = currADDR1.Po_Box_Cit;
            I_Bapiaddr1.Po_Box_Lobby = currADDR1.Po_Box_Lobby;
            I_Bapiaddr1.Po_Box_Reg = currADDR1.Po_Box_Reg;
            I_Bapiaddr1.Po_Ctryiso = currADDR1.Po_Ctryiso;
            I_Bapiaddr1.Po_W_O_No = currADDR1.Po_W_O_No;
            I_Bapiaddr1.Pobox_Ctry = currADDR1.Pobox_Ctry;
            I_Bapiaddr1.Postl_Cod1 = currADDR1.Postl_Cod1;
            I_Bapiaddr1.Postl_Cod2 = currADDR1.Postl_Cod2;
            I_Bapiaddr1.Postl_Cod3 = currADDR1.Postl_Cod3;
            I_Bapiaddr1.Regiogroup = currADDR1.Regiogroup;
            I_Bapiaddr1.Region = currADDR1.Region;
            I_Bapiaddr1.Room_No = currADDR1.Room_No;
            I_Bapiaddr1.Sort1 = currADDR1.Sort1;
            I_Bapiaddr1.Sort2 = currADDR1.Sort2;
            I_Bapiaddr1.Str_Abbr = currADDR1.Str_Abbr;
            I_Bapiaddr1.Str_Suppl1 = currADDR1.Str_Suppl1;
            I_Bapiaddr1.Str_Suppl2 = currADDR1.Str_Suppl2;
            I_Bapiaddr1.Str_Suppl3 = currADDR1.Str_Suppl3;
            I_Bapiaddr1.Street = currADDR1.Street;
            I_Bapiaddr1.Street_Lng = currADDR1.Street_Lng;
            I_Bapiaddr1.Street_No = currADDR1.Street_No;
            I_Bapiaddr1.Taxjurcode = currADDR1.Taxjurcode;
            I_Bapiaddr1.Tel1_Ext = currADDR1.Tel1_Ext;
            I_Bapiaddr1.Tel1_Numbr = currADDR1.Tel1_Numbr;
            I_Bapiaddr1.Time_Zone = currADDR1.Time_Zone;
            I_Bapiaddr1.Title = currADDR1.Title;
            I_Bapiaddr1.Township = currADDR1.Township;
            I_Bapiaddr1.Township_Code = currADDR1.Township_Code;
            I_Bapiaddr1.Transpzone = currADDR1.Transpzone;
            I_Bapiaddr1.Uri_Type = currADDR1.Uri_Type;
            SA_BAPIADDR2 currADDR2 = A2C.SA_BAPIADDR2.FirstOrDefault();
            I_Bapiaddr2.Addr_No = currADDR2.Addr_No;
            I_Customer_Is_Consumer = ""; I_Force_External_Number_Range = "1"; I_From_Customermaster = "1";
            SA_KNA1 currKNA1 = A2C.SA_KNA1.FirstOrDefault();
            I_Kna1.Abrvw = currKNA1.Abrvw;
            //  I_Kna1.Adrnr = currKNA1.Adrnr;
            I_Kna1.Alc = currKNA1.Alc;
            I_Kna1.Anred = currKNA1.Anred;
            I_Kna1.Aufsd = currKNA1.Aufsd;
            I_Kna1.Bahne = currKNA1.Bahne;
            I_Kna1.Bahns = currKNA1.Bahns;
            I_Kna1.Bbbnr = currKNA1.Bbbnr;
            I_Kna1.Bbsnr = currKNA1.Bbsnr;
            I_Kna1.Begru = currKNA1.Begru;
            I_Kna1.Bran1 = currKNA1.Bran1;
            I_Kna1.Bran2 = currKNA1.Bran2;
            I_Kna1.Bran3 = currKNA1.Bran3;
            I_Kna1.Bran4 = currKNA1.Bran4;
            I_Kna1.Bran5 = currKNA1.Bran5;
            I_Kna1.Brsch = currKNA1.Brsch;
            I_Kna1.Bubkz = currKNA1.Bubkz;
            I_Kna1.Cassd = currKNA1.Cassd;
            I_Kna1.Ccc01 = currKNA1.Ccc01;
            I_Kna1.Ccc02 = currKNA1.Ccc02;
            I_Kna1.Ccc03 = currKNA1.Ccc03;
            I_Kna1.Ccc04 = currKNA1.Ccc04;
            I_Kna1.Cfopc = currKNA1.Cfopc;
            I_Kna1.Cityc = currKNA1.Cityc;
            I_Kna1.Civve = currKNA1.Civve;
            I_Kna1.Confs = currKNA1.Confs;
            I_Kna1.Counc = currKNA1.Counc;
            I_Kna1.Datlt = currKNA1.Datlt;
            I_Kna1.Dear1 = currKNA1.Dear1;
            I_Kna1.Dear2 = currKNA1.Dear2;
            I_Kna1.Dear3 = currKNA1.Dear3;
            I_Kna1.Dear4 = currKNA1.Dear4;
            I_Kna1.Dear5 = currKNA1.Dear5;
            I_Kna1.Dear6 = currKNA1.Dear6;
            I_Kna1.Dtams = currKNA1.Dtams;
            I_Kna1.Dtaws = currKNA1.Dtaws;
            I_Kna1.Duefl = currKNA1.Duefl;
            I_Kna1.Ekont = currKNA1.Ekont;
            I_Kna1.Erdat = currKNA1.Erdat;
            I_Kna1.Ernam = currKNA1.Ernam;
            I_Kna1.Etikg = currKNA1.Etikg;
            I_Kna1.Exabl = currKNA1.Exabl;
            I_Kna1.Faksd = currKNA1.Faksd;
            I_Kna1.Fiskn = currKNA1.Fiskn;
            I_Kna1.Fityp = currKNA1.Fityp;
            I_Kna1.Gform = currKNA1.Gform;
            I_Kna1.Hzuor = currKNA1.Hzuor;
            I_Kna1.Inspatdebi = currKNA1.Inspatdebi;
            I_Kna1.Inspbydebi = currKNA1.Inspbydebi;
            I_Kna1.J_1kfrepre = currKNA1.J_1kfrepre;
            I_Kna1.J_1kftbus = currKNA1.J_1kftbus;
            I_Kna1.J_1kftind = currKNA1.J_1kftind;
            I_Kna1.Jmjah = currKNA1.Jmjah;
            I_Kna1.Jmzah = currKNA1.Jmzah;
            I_Kna1.Katr1 = currKNA1.Katr1;
            I_Kna1.Katr10 = currKNA1.Katr10;
            I_Kna1.Katr2 = currKNA1.Katr2;
            I_Kna1.Katr3 = currKNA1.Katr3;
            I_Kna1.Katr4 = currKNA1.Katr4;
            I_Kna1.Katr5 = currKNA1.Katr5;
            I_Kna1.Katr6 = currKNA1.Katr6;
            I_Kna1.Katr7 = currKNA1.Katr7;
            I_Kna1.Katr8 = currKNA1.Katr8;
            I_Kna1.Katr9 = currKNA1.Katr9;
            string CondGrp1 = "L0"; string CondGrp2 = "L0"; string CondGrp3 = "L0"; string CondGrp4 = "L0"; string CondGrp5 = "R4";
            I_Kna1.Kdkg1 = currKNA1.Kdkg1;
            I_Kna1.Kdkg2 = currKNA1.Kdkg2;
            I_Kna1.Kdkg3 = currKNA1.Kdkg3;
            I_Kna1.Kdkg4 = currKNA1.Kdkg4;
            I_Kna1.Kdkg5 = currKNA1.Kdkg5;
            I_Kna1.Knazk = currKNA1.Knazk;
            I_Kna1.Knrza = currKNA1.Knrza;
            I_Kna1.Knurl = currKNA1.Knurl;
            I_Kna1.Konzs = currKNA1.Konzs;
            I_Kna1.Ktocd = currKNA1.Ktocd;
            I_Kna1.Ktokd = currKNA1.Ktokd;
            I_Kna1.Kukla = currKNA1.Kukla;
            I_Kna1.Kunnr = currKNA1.Kunnr;
            I_Kna1.Land1 = currKNA1.Land1;
            I_Kna1.Lifnr = currKNA1.Lifnr;
            I_Kna1.Lifsd = currKNA1.Lifsd;
            I_Kna1.Locco = currKNA1.Locco;
            I_Kna1.Loevm = currKNA1.Loevm;
            I_Kna1.Lzone = currKNA1.Lzone;
            I_Kna1.Mandt = currKNA1.Mandt;
            I_Kna1.Mcod1 = currKNA1.Mcod1;
            I_Kna1.Mcod2 = currKNA1.Mcod2;
            I_Kna1.Mcod3 = currKNA1.Mcod3;
            I_Kna1.Milve = currKNA1.Milve;
            I_Kna1.Name1 = currKNA1.Name1;
            I_Kna1.Name2 = currKNA1.Name2;
            I_Kna1.Name3 = currKNA1.Name3;
            I_Kna1.Name4 = currKNA1.Name4;
            I_Kna1.Niels = currKNA1.Niels;
            I_Kna1.Nodel = currKNA1.Nodel;
            I_Kna1.Ort01 = currKNA1.Ort01;
            I_Kna1.Ort02 = currKNA1.Ort02;
            I_Kna1.Periv = currKNA1.Periv;
            I_Kna1.Pfach = currKNA1.Pfach;
            I_Kna1.Pfort = currKNA1.Pfort;
            I_Kna1.Pmt_Office = currKNA1.Pmt_Office;
            I_Kna1.Psofg = currKNA1.Psofg;
            I_Kna1.Psohs = currKNA1.Psohs;
            I_Kna1.Psois = currKNA1.Psois;
            I_Kna1.Pson1 = currKNA1.Pson1;
            I_Kna1.Pson2 = currKNA1.Pson2;
            I_Kna1.Pson3 = currKNA1.Pson3;
            I_Kna1.Psoo1 = currKNA1.Psoo1;
            I_Kna1.Psoo2 = currKNA1.Psoo2;
            I_Kna1.Psoo3 = currKNA1.Psoo3;
            I_Kna1.Psoo4 = currKNA1.Psoo4;
            I_Kna1.Psoo5 = currKNA1.Psoo5;
            I_Kna1.Psost = currKNA1.Psost;
            I_Kna1.Psotl = currKNA1.Psotl;
            I_Kna1.Psovn = currKNA1.Psovn;
            I_Kna1.Pstl2 = currKNA1.Pstl2;
            I_Kna1.Pstlz = currKNA1.Pstlz;
            //   I_Kna1.Regio = currKNA1.Regio;
            I_Kna1.Rpmkr = currKNA1.Rpmkr;
            I_Kna1.Sortl = currKNA1.Sortl;
            I_Kna1.Sperr = currKNA1.Sperr;
            I_Kna1.Sperz = currKNA1.Sperz;
            I_Kna1.Spras = currKNA1.Spras;
            I_Kna1.Stcd1 = currKNA1.Stcd1;
            I_Kna1.Stcd2 = currKNA1.Stcd2;
            I_Kna1.Stcd3 = currKNA1.Stcd3;
            I_Kna1.Stcd4 = currKNA1.Stcd4;
            I_Kna1.Stcd5 = currKNA1.Stcd5;
            I_Kna1.Stcdt = currKNA1.Stcdt;
            I_Kna1.Stceg = currKNA1.Stceg;
            I_Kna1.Stkza = currKNA1.Stkza;
            I_Kna1.Stkzn = currKNA1.Stkzn;
            I_Kna1.Stkzu = currKNA1.Stkzu;
            I_Kna1.Stras = currKNA1.Stras;
            I_Kna1.Telbx = currKNA1.Telbx;
            I_Kna1.Telf1 = currKNA1.Telf1;
            I_Kna1.Telf2 = "1122";//currKNA1.Telf2;
            I_Kna1.Telfx = currKNA1.Telfx;
            I_Kna1.Teltx = currKNA1.Teltx;
            I_Kna1.Telx1 = currKNA1.Telx1;
            I_Kna1.Txjcd = currKNA1.Txjcd;
            I_Kna1.Txlw1 = currKNA1.Txlw1;
            I_Kna1.Txlw2 = currKNA1.Txlw2;
            I_Kna1.Umjah = currKNA1.Umjah;
            I_Kna1.Umsa1 = 0;
            I_Kna1.Umsat = 0;
            I_Kna1.Updat = currKNA1.Updat;
            I_Kna1.Uptim = currKNA1.Uptim;
            I_Kna1.Uwaer = currKNA1.Uwaer;
            I_Kna1.Vbund = currKNA1.Vbund;
            //  I_Kna1.Vso_R_Dpoint = currKNA1.Vso_R_Dpoint;
            //  I_Kna1.Vso_R_I_No_Lyr = currKNA1.Vso_R_I_No_Lyr;
            //   I_Kna1.Vso_R_Load_Pref = currKNA1.Vso_R_Load_Pref;
            //    I_Kna1.Vso_R_Matpal = currKNA1.Vso_R_Matpal;
            // I_Kna1.Vso_R_One_Mat = currKNA1.Vso_R_One_Mat;
            //     I_Kna1.Vso_R_One_Sort = currKNA1.Vso_R_One_Sort;
            //   I_Kna1.Vso_R_Pal_Ul = currKNA1.Vso_R_Pal_Ul;
            //    I_Kna1.Vso_R_Palhgt = 0;
            //  I_Kna1.Vso_R_Pk_Mat = currKNA1.Vso_R_Pk_Mat;
            //  I_Kna1.Vso_R_Uld_Side = currKNA1.Vso_R_Uld_Side;
            I_Kna1.Werks = currKNA1.Werks;
            I_Kna1.Xcpdk = currKNA1.Xcpdk;
            I_Kna1.Xicms = currKNA1.Xicms;
            I_Kna1.Xknza = currKNA1.Xknza;
            I_Kna1.Xsubt = currKNA1.Xsubt;
            I_Kna1.Xxipi = currKNA1.Xxipi;
            I_Kna1.Xzemp = currKNA1.Xzemp;

            SA_KNB1 currKNB1 = A2C.SA_KNB1.FirstOrDefault();
            //    I_Knb1.Ad_Hash = currKNB1.Ad_Hash;
            I_Knb1.Akont = "0000121001";// currKNB1.Akont;
            I_Knb1.Altkn = currKNB1.Altkn;
            //  I_Knb1.Avsnd = currKNB1.Avsnd;
            I_Knb1.Begru = currKNB1.Begru;
            I_Knb1.Blnkz = currKNB1.Blnkz;
            I_Knb1.Bukrs = currKNB1.Bukrs;
            I_Knb1.Busab = "01";// currKNB1.Busab;
            I_Knb1.Cession_Kz = currKNB1.Cession_Kz;
            I_Knb1.Confs = currKNB1.Confs;
            I_Knb1.Datlz = currKNB1.Datlz;
            I_Knb1.Eikto = currKNB1.Eikto;
            I_Knb1.Ekvbd = currKNB1.Ekvbd;
            I_Knb1.Erdat = currKNB1.Erdat;
            I_Knb1.Ernam = currKNB1.Ernam;
            I_Knb1.Fdgrv = currKNB1.Fdgrv;
            I_Knb1.Frgrp = currKNB1.Frgrp;
            I_Knb1.Gmvkzd = currKNB1.Gmvkzd;
            I_Knb1.Gricd = currKNB1.Gricd;
            I_Knb1.Gridt = currKNB1.Gridt;
            I_Knb1.Guzte = currKNB1.Guzte;
            I_Knb1.Hbkid = currKNB1.Hbkid;
            I_Knb1.Intad = currKNB1.Intad;
            I_Knb1.Knrzb = currKNB1.Knrzb;
            I_Knb1.Knrze = currKNB1.Knrze;
            I_Knb1.Kultg = 0;
            I_Knb1.Kunnr = currKNB1.Kunnr;
            I_Knb1.Kverm = currKNB1.Kverm;
            I_Knb1.Lockb = currKNB1.Lockb;
            I_Knb1.Loevm = currKNB1.Loevm;
            I_Knb1.Mandt = currKNB1.Mandt;
            I_Knb1.Mgrup = currKNB1.Mgrup;
            I_Knb1.Nodel = currKNB1.Nodel;
            I_Knb1.Perkz = currKNB1.Perkz;
            I_Knb1.Pernr = currKNB1.Pernr;
            //  I_Knb1.Qland = currKNB1.Qland;
            I_Knb1.Remit = currKNB1.Remit;
            I_Knb1.Sperr = currKNB1.Sperr;
            I_Knb1.Sregl = currKNB1.Sregl;
            I_Knb1.Tlfns = currKNB1.Tlfns;
            I_Knb1.Tlfxs = currKNB1.Tlfxs;
            I_Knb1.Togru = currKNB1.Togru;
            I_Knb1.Updat = currKNB1.Updat;
            I_Knb1.Uptim = currKNB1.Uptim;
            I_Knb1.Urlid = currKNB1.Urlid;
            I_Knb1.Uzawe = currKNB1.Uzawe;
            I_Knb1.Verdt = currKNB1.Verdt;
            I_Knb1.Vlibb = 0;
            I_Knb1.Vrbkz = currKNB1.Vrbkz;
            I_Knb1.Vrsdg = currKNB1.Vrsdg;
            I_Knb1.Vrsnr = currKNB1.Vrsnr;
            I_Knb1.Vrspr = 0;
            I_Knb1.Vrszl = 0;
            I_Knb1.Vzskz = currKNB1.Vzskz;
            I_Knb1.Wakon = currKNB1.Wakon;
            I_Knb1.Wbrsl = currKNB1.Wbrsl;
            I_Knb1.Webtr = 0;
            I_Knb1.Xausz = currKNB1.Xausz;
            I_Knb1.Xdezv = currKNB1.Xdezv;
            I_Knb1.Xedip = currKNB1.Xedip;
            I_Knb1.Xknzb = currKNB1.Xknzb;
            I_Knb1.Xpore = currKNB1.Xpore;
            I_Knb1.Xverr = currKNB1.Xverr;
            I_Knb1.Xzver = currKNB1.Xzver;
            I_Knb1.Zahls = currKNB1.Zahls;
            I_Knb1.Zamib = currKNB1.Zamib;
            I_Knb1.Zamim = currKNB1.Zamim;
            I_Knb1.Zamio = currKNB1.Zamio;
            I_Knb1.Zamir = currKNB1.Zamir;
            I_Knb1.Zamiv = currKNB1.Zamiv;
            I_Knb1.Zgrup = currKNB1.Zgrup;
            I_Knb1.Zindt = currKNB1.Zindt;
            I_Knb1.Zinrt = currKNB1.Zinrt;
            I_Knb1.Zsabe = currKNB1.Zsabe;
            I_Knb1.Zterm = currKNB1.Zterm;
            I_Knb1.Zuawa = currKNB1.Zuawa;
            I_Knb1.Zwels = currKNB1.Zwels;
            /////
            SA_KNVV currKNVV = A2C.SA_KNVV.FirstOrDefault();
            I_Knvv.Agrel = currKNVV.Agrel;
            I_Knvv.Antlf = 9;
            I_Knvv.Aufsd = currKNVV.Aufsd;
            I_Knvv.Autlf = currKNVV.Autlf;
            I_Knvv.Awahr = currKNVV.Awahr;
            I_Knvv.Begru = currKNVV.Begru;
            I_Knvv.Bev1_Emlgforts = currKNVV.Bev1_Emlgforts;
            I_Knvv.Bev1_Emlgpfand = currKNVV.Bev1_Emlgpfand;
            I_Knvv.Blind = currKNVV.Blind;
            I_Knvv.Boidt = currKNVV.Boidt;
            I_Knvv.Bokre = currKNVV.Bokre;
            I_Knvv.Bzirk = currKNVV.Bzirk;
            //   I_Knvv.Carrier_Notif = currKNVV.Carrier_Notif;
            I_Knvv.Cassd = currKNVV.Cassd;
            I_Knvv.Chspl = currKNVV.Chspl;
            I_Knvv.Eikto = currKNVV.Eikto;
            I_Knvv.Erdat = currKNVV.Erdat;
            I_Knvv.Ernam = currKNVV.Ernam;
            I_Knvv.Faksd = " ";// currKNVV.Faksd;
            string strInco1 = "FOB"; string strInco2 = "";
            I_Knvv.Inco1 = currKNVV.Inco1;
            I_Knvv.Inco2 = currKNVV.Inco2;
            I_Knvv.Kabss = currKNVV.Kabss;
            I_Knvv.Kalks = currKNVV.Kalks;
            I_Knvv.Kdgrp = currKNVV.Kdgrp;
            I_Knvv.Kkber = currKNVV.Kkber;
            I_Knvv.Klabc = currKNVV.Klabc;
            I_Knvv.Konda = currKNVV.Konda;
            I_Knvv.Ktgrd = currKNVV.Ktgrd;
            I_Knvv.Kunnr = currKNVV.Kunnr;
            I_Knvv.Kurst = currKNVV.Kurst;
            I_Knvv.Kvakz = currKNVV.Kvakz;
            I_Knvv.Kvawt = 0;
            I_Knvv.Kvgr1 = currKNVV.Kvgr1;
            I_Knvv.Kvgr2 = currKNVV.Kvgr2;
            I_Knvv.Kvgr3 = currKNVV.Kvgr3;
            I_Knvv.Kvgr4 = currKNVV.Kvgr4;
            I_Knvv.Kvgr5 = currKNVV.Kvgr5;
            I_Knvv.Kzazu = currKNVV.Kzazu;
            I_Knvv.Kztlf = currKNVV.Kztlf;
            I_Knvv.Lifsd = currKNVV.Lifsd;
            I_Knvv.Loevm = currKNVV.Loevm;
            I_Knvv.Lprio = currKNVV.Lprio;
            I_Knvv.Mandt = currKNVV.Mandt;
            I_Knvv.Megru = currKNVV.Megru;
            I_Knvv.Mrnkz = currKNVV.Mrnkz;
            I_Knvv.Perfk = currKNVV.Perfk;
            I_Knvv.Perrl = currKNVV.Perrl;
            I_Knvv.Pltyp = currKNVV.Pltyp;
            I_Knvv.Podkz = currKNVV.Podkz;
            I_Knvv.Podtg = 0;
            I_Knvv.Prat1 = currKNVV.Prat1;
            I_Knvv.Prat2 = currKNVV.Prat2;
            I_Knvv.Prat3 = currKNVV.Prat3;
            I_Knvv.Prat4 = currKNVV.Prat4;
            I_Knvv.Prat5 = currKNVV.Prat5;
            I_Knvv.Prat6 = currKNVV.Prat6;
            I_Knvv.Prat7 = currKNVV.Prat7;
            I_Knvv.Prat8 = currKNVV.Prat8;
            I_Knvv.Prat9 = currKNVV.Prat9;
            I_Knvv.Prata = currKNVV.Prata;
            I_Knvv.Prfre = currKNVV.Prfre;
            I_Knvv.Pvksm = currKNVV.Pvksm;
            I_Knvv.Rdoff = currKNVV.Rdoff;
            I_Knvv.Spart = currKNVV.Spart;
            I_Knvv.Uebtk = currKNVV.Uebtk;
            I_Knvv.Uebto = 0;
            I_Knvv.Untto = 0;
            I_Knvv.Versg = currKNVV.Versg;
            I_Knvv.Vkbur = currKNVV.Vkbur;
            I_Knvv.Vkgrp = currKNVV.Vkgrp;
            I_Knvv.Vkorg = currKNVV.Vkorg;
            I_Knvv.Vsbed = currKNVV.Vsbed;
            I_Knvv.Vsort = currKNVV.Vsort;
            I_Knvv.Vtweg = currKNVV.Vtweg;
            I_Knvv.Vwerk = currKNVV.Vwerk;
            I_Knvv.Waers = currKNVV.Waers;
            I_Knvv.Zterm = currKNVV.Zterm;

            I_Maintain_Address_By_Kna1 = ""; I_No_Bank_Master_Update = ""; I_Raise_No_Bte = "";
            Pi_Cam_Changed = ""; Pi_Postflag = "";

            var VE = new SAPCustomerRFC.FKNVP();
            VE.Mandt = "168";
            VE.Kunnr = currKNA1.Kunnr;
            VE.Vkorg = "TW01";
            VE.Vtweg = "00";
            VE.Spart = "00";
            VE.Parvw = "VE";
            VE.Parza = "000";
            VE.Kunn2 = currKNA1.Kunnr;
            VE.Lifnr = "";
            VE.Pernr = A2C.SalesCode;
            VE.Parnr = "0000000000";
            VE.Knref = "";
            VE.Defpa = "";
            T_Xknvp.Add(VE);
            var ER = new SAPCustomerRFC.FKNVP();
            ER.Mandt = "168";
            ER.Kunnr = currKNA1.Kunnr;
            ER.Vkorg = "TW01";
            ER.Vtweg = "00";
            ER.Spart = "00";
            ER.Parvw = "ZM";
            ER.Parza = "000";
            ER.Kunn2 = "";
            ER.Lifnr = "";
            ER.Pernr = A2C.OPCode;
            ER.Parnr = "00000000";
            ER.Knref = "";
            ER.Defpa = "";
            T_Xknvp.Add(ER);

            try
            {
                p1.Zsd_Customer_Maintain_All(I_Bapiaddr1, I_Bapiaddr2, I_Customer_Is_Consumer,
                                I_Force_External_Number_Range, I_From_Customermaster,
                                I_Kna1, I_Knb1, I_Knb1_Reference, I_Knvv, I_Maintain_Address_By_Kna1,
                                I_No_Bank_Master_Update, I_Raise_No_Bte,
                                Pi_Add_On_Data, Pi_Cam_Changed, Pi_Postflag,
                            out E_Kunnr, out E_Sd_Cust_1321_Done, out O_Kna1, ref T_Upd_Txt,
                             ref T_Xkn, ref T_Xknb5, ref T_Xknbk, ref T_Xknex, ref T_Xknva, ref T_Xknvd, ref T_Xknvi,
                               ref T_Xknvk, ref T_Xknvl, ref T_Xknvp, ref T_Xknza, ref T_Ykn, ref T_Yknb5, ref T_Yknbk, ref T_Yknex, ref T_Yknva,
                               ref T_Yknvd, ref T_Yknvi, ref T_Yknvk, ref T_Yknvl, ref T_Yknvp, ref T_Yknza);
                p1.CommitWork();

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                smtpClient1.Send("myadvantech@advantech.com", "myadvantech@advantech.com", string.Format("CreateSAPAccount Failed:  AppNo:{0}  , Type:{1} ", ap.AplicationNO, type.ToString()), string.Format("error: {0}", msg));
            }

            p1.Connection.Close();
            return string.Empty;
        }

        public static void GetCustomerAcctAssgmtGroupAndTaxClassification(string countrycode, ref string AAG, ref string TC)
        {
            string strCountrys = "AT,BE,BG,CY,CZ,DE,DK,EE,GR,ES,FI,FR,GB,HR,HU,IE,IT,LT,LU,LV,MT,PL,PT,RO,SE,SI,SK";
            if (strCountrys.Contains(countrycode.ToUpper().Trim()))
            { AAG = "02"; TC = "8"; }
            else if (string.Equals(countrycode, "NL", StringComparison.CurrentCultureIgnoreCase))
            { AAG = "01"; TC = "7"; }
            else
            { AAG = "02"; TC = "9"; }
        }

        public static Boolean isERPIDExist(String _ERPID, Boolean _isTesting)
        {
            String SAP_Connection = _isTesting ? "SAP_Test" : "SAP_PRD";

            DataTable dt = OracleProvider.GetDataTable(SAP_Connection,
                "select kunnr from saprdp.kna1 where kunnr='" + _ERPID.Trim().Replace("'", "''").ToUpper() + "'");

            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public static Boolean IsABRNonContribuinte(String _ERPID, Boolean _isTesting)
        {
            String SAP_Connection = _isTesting ? "SAP_Test" : "SAP_PRD";

            DataTable dt = OracleProvider.GetDataTable(SAP_Connection,
                "select kunnr,BRSCH from saprdp.kna1 where mandt='168' and kunnr='" + _ERPID.Trim().Replace("'", "''").ToUpper() + "'");

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["BRSCH"].ToString().Equals("BRNC", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static void CreateUSShiptoAccount(DataAccess.SAPModel.SAPAccount _AccountInfo, Boolean _isTesting)
        {
            String ID_To_Create = _AccountInfo.ShiptoID;

            if (isERPIDExist(ID_To_Create, _isTesting))
                return;

            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);

            var p1 = new SAPCustomerRFC.SAPCustomerRFC();
            String SAPconnection = _isTesting ? "SAPConnTest" : "SAP_PRD";
            p1.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings[SAPconnection]);
            var I_Bapiaddr1 = new SAPCustomerRFC.BAPIADDR1();
            var I_Bapiaddr2 = new SAPCustomerRFC.BAPIADDR2();
            var I_Kna1 = new SAPCustomerRFC.KNA1();
            var I_Knb1 = new SAPCustomerRFC.KNB1();
            var I_Knvv = new SAPCustomerRFC.KNVV();
            var O_Kna1 = new SAPCustomerRFC.KNA1();
            var T_Upd_Txt = new SAPCustomerRFC.FKUNTXTTable(); var T_Xkn = new SAPCustomerRFC.FKNASTable();
            var T_Xknb5 = new SAPCustomerRFC.FKNB5Table(); var T_Xknbk = new SAPCustomerRFC.FKNBKTable();
            var T_Xknex = new SAPCustomerRFC.FKNEXTable(); var T_Xknva = new SAPCustomerRFC.FKNVATable();
            var T_Xknvd = new SAPCustomerRFC.FKNVDTable(); var T_Xknvi = new SAPCustomerRFC.FKNVITable();
            var T_Xknvk = new SAPCustomerRFC.FKNVKTable(); var T_Xknvl = new SAPCustomerRFC.FKNVLTable();
            var T_Xknvp = new SAPCustomerRFC.FKNVPTable(); var T_Xknza = new SAPCustomerRFC.FKNZATable();
            var T_Ykn = new SAPCustomerRFC.FKNASTable(); var T_Yknb5 = new SAPCustomerRFC.FKNB5Table();
            var T_Yknbk = new SAPCustomerRFC.FKNBKTable(); var T_Yknex = new SAPCustomerRFC.FKNEXTable();
            var T_Yknva = new SAPCustomerRFC.FKNVATable(); var T_Yknvd = new SAPCustomerRFC.FKNVDTable();
            var T_Yknvi = new SAPCustomerRFC.FKNVITable(); var T_Yknvk = new SAPCustomerRFC.FKNVKTable();
            var T_Yknvl = new SAPCustomerRFC.FKNVLTable(); var T_Yknvp = new SAPCustomerRFC.FKNVPTable();
            var T_Yknza = new SAPCustomerRFC.FKNZATable();
            var Pi_Add_On_Data = new SAPCustomerRFC.CUST_ADD_ON_DATA();
            string Pi_Cam_Changed = "";
            string Pi_Postflag = "";
            string E_Kunnr = ""; string E_Sd_Cust_1321_Done = "";
            string I_Maintain_Address_By_Kna1 = "X";
            string I_Knb1_Reference = "";
            string I_No_Bank_Master_Update = "";
            string I_Raise_No_Bte = "";
            string I_Customer_Is_Consumer = "";
            string I_Force_External_Number_Range = "";
            string I_From_Customermaster = "";
            string Taxid = ""; string AAG = "";


            //==============Common Settings=================  
            string _companyname = _AccountInfo.CompanyName;
            string _countrycode = _AccountInfo.CountryCode;
            string _orgid = _AccountInfo.OrgID;
            string _city = _AccountInfo.City;
            string _region = _AccountInfo.Region;
            string _postalcode = _AccountInfo.PostalCode;
            string _tel = _AccountInfo.TEL;
            string _vatnumber = _AccountInfo.VatNumber;
            string _currency = _AccountInfo.Currency;
            string _taxjurisdiction = _AccountInfo.TaxJurisdiction;
            string _creator = _AccountInfo.Creator;
            string _createdate = _AccountInfo.CreateDate;
            string _salesgroupcode = _AccountInfo.SalesGroupCode;
            string _salesofficecode = _AccountInfo.SalesOfficeCode;
            string _contactperson = _AccountInfo.ContactPerson;

            //Ryan 20160713 If length>40, split address to two parts and save to address2 which maps to I_Bapiaddr1.Str_Suppl3.
            string _address = _AccountInfo.Address;
            string _address2 = String.Empty;

            if (_address.Length > 40)
            {
                for (int i = _address.Length - 1; i > 0; i--)
                {
                    if (Char.IsWhiteSpace(_address[i]))
                    {
                        if (i <= 40)
                        {
                            _address2 = _address.Substring(i + 1, _address.Length - i - 1);
                            _address = _address.Substring(0, i);
                            break;
                        }
                    }
                }
            }
            //==============================================

            #region I_Bapiaddr1+2 values setting
            I_Bapiaddr1.Title = "Company";
            I_Bapiaddr1.Name = _companyname;
            I_Bapiaddr1.C_O_Name = String.IsNullOrEmpty(_contactperson.Trim()) ? _companyname : _contactperson;
            I_Bapiaddr1.Street = _address;
            I_Bapiaddr1.Str_Suppl3 = _address2;
            I_Bapiaddr1.Postl_Cod1 = _postalcode;
            I_Bapiaddr1.City = _city;
            I_Bapiaddr1.Country = _countrycode;
            I_Bapiaddr1.Region = _region;
            I_Bapiaddr1.Time_Zone = "PST";
            I_Bapiaddr1.Taxjurcode = _taxjurisdiction;

            I_Bapiaddr1.Langu = "EN";
            I_Bapiaddr1.Comm_Type = "INT";
            I_Bapiaddr1.Fax_Number = "";
            I_Bapiaddr1.Tel1_Numbr = _tel;
            I_Bapiaddr1.Location = "";
            I_Bapiaddr1.Addr_No = "";
            I_Bapiaddr1.E_Mail = "";
            I_Bapiaddr1.Fax_Number = "";
            I_Bapiaddr1.Homepage = "";

            I_Bapiaddr2.Addr_No = "";
            #endregion

            #region KNA1 values setting
            // KNA1 maps to SAP General Data
            I_Kna1.Mandt = "168";
            I_Kna1.Kunnr = ID_To_Create; // ERP ID     ======Will changed======
            I_Kna1.Land1 = _countrycode; // Country Code     ======Will changed======
            I_Kna1.Name1 = _companyname; // Company Name     ======Will changed======
            I_Kna1.Name2 = " ";
            I_Kna1.Ort01 = _city; // City     ======Will changed======
            I_Kna1.Pstlz = _postalcode; // Postal Code     ======Will changed======
            I_Kna1.Regio = _region; // Region     ======Will changed====== 
            I_Kna1.Sortl = " ";
            I_Kna1.Stras = _address; // Address, same as BappiAddr1's Street     ======Will changed======
            I_Kna1.Telf1 = _tel; // Tel Number, same as BappiAddr1's Tel1_Numbr     ======Will changed======
            I_Kna1.Telfx = " ";
            I_Kna1.Xcpdk = " ";

            //I_Kna1.Adrnr = " "; // !!!!!!!!!!!!!!!!!do not put this field, will causes error.!!!!!!!!!!!!!!!!!

            I_Kna1.Mcod1 = _companyname; // Put company name here.     ======Will changed======
            I_Kna1.Mcod2 = " ";
            I_Kna1.Mcod3 = _city; // Put City here.     ======Will changed======

            I_Kna1.Anred = "Company";  // !!!!!!!!!!!!!!!!!Add by myself, EU&TW didn't add this field!!!!!!!!!!!!!!!!

            I_Kna1.Aufsd = " ";
            I_Kna1.Bahne = " ";
            I_Kna1.Bahns = " ";
            I_Kna1.Bbbnr = "0000000";
            I_Kna1.Bbsnr = "00000";
            I_Kna1.Begru = " ";
            I_Kna1.Bubkz = "0";
            I_Kna1.Brsch = " ";
            I_Kna1.Datlt = " ";
            I_Kna1.Erdat = _createdate;  // Create Time
            I_Kna1.Ernam = _creator;  // Creator
            I_Kna1.Exabl = " ";
            I_Kna1.Faksd = " ";
            I_Kna1.Fiskn = " ";
            I_Kna1.Knazk = " ";
            I_Kna1.Knrza = " ";
            I_Kna1.Konzs = " ";
            I_Kna1.Ktokd = "Z002"; // Company Type Z002 represents ship-to
            I_Kna1.Kukla = " ";
            I_Kna1.Lifnr = " ";
            I_Kna1.Lifsd = " ";
            I_Kna1.Locco = " ";
            I_Kna1.Loevm = " ";
            I_Kna1.Name3 = " ";
            I_Kna1.Name4 = " ";
            I_Kna1.Niels = " ";
            I_Kna1.Ort02 = " ";
            I_Kna1.Pfach = " ";
            I_Kna1.Pstl2 = " ";
            I_Kna1.Counc = " ";
            I_Kna1.Cityc = " ";
            I_Kna1.Rpmkr = " ";
            I_Kna1.Sperr = " ";
            I_Kna1.Spras = "E"; // Language key.
            I_Kna1.Stcd1 = " ";
            I_Kna1.Stcd2 = " ";
            I_Kna1.Stkza = " ";
            I_Kna1.Stkzu = " ";
            I_Kna1.Telbx = " ";
            I_Kna1.Telf2 = " ";
            I_Kna1.Teltx = " ";
            I_Kna1.Telx1 = " ";
            I_Kna1.Lzone = " ";
            I_Kna1.Xzemp = " ";
            I_Kna1.Vbund = " ";
            I_Kna1.Stceg = _vatnumber; // VAT Number  ======Will changed======
            I_Kna1.Dear1 = " ";
            I_Kna1.Dear2 = " ";
            I_Kna1.Dear3 = " ";
            I_Kna1.Dear4 = " ";
            I_Kna1.Dear5 = " ";
            I_Kna1.Gform = " ";
            I_Kna1.Bran1 = " ";
            I_Kna1.Bran2 = " ";
            I_Kna1.Bran3 = " ";
            I_Kna1.Bran4 = " ";
            I_Kna1.Bran5 = " ";
            I_Kna1.Ekont = " ";
            I_Kna1.Umsat = 0;
            I_Kna1.Umjah = "0000";
            I_Kna1.Uwaer = " ";
            I_Kna1.Jmzah = "000000";
            I_Kna1.Jmjah = "0000";
            I_Kna1.Katr1 = " "; // Attr 1 
            I_Kna1.Katr2 = " "; // Attr 2
            I_Kna1.Katr3 = " "; // Attr 3
            I_Kna1.Katr4 = " "; // Attr 1 
            I_Kna1.Katr5 = " "; // Attr 1 
            I_Kna1.Katr6 = " "; // Attr 1 
            I_Kna1.Katr7 = " "; // Attr 1 
            I_Kna1.Katr8 = " "; // Attr 1 
            I_Kna1.Katr9 = " "; // Attr 1 
            I_Kna1.Katr10 = " "; // Attr 1 
            I_Kna1.Stkzn = " ";
            I_Kna1.Umsa1 = 0;
            I_Kna1.Txjcd = _taxjurisdiction; // Tax Jurisdiction    ======Will changed======
            I_Kna1.Periv = " ";
            I_Kna1.Abrvw = " ";
            I_Kna1.Inspbydebi = " ";
            I_Kna1.Inspatdebi = " ";
            I_Kna1.Ktocd = " ";
            I_Kna1.Pfort = " ";
            I_Kna1.Werks = " ";
            I_Kna1.Dtams = " ";
            I_Kna1.Dtaws = " ";
            I_Kna1.Duefl = "X";
            I_Kna1.Hzuor = "00";
            I_Kna1.Sperz = " ";
            I_Kna1.Etikg = " ";
            I_Kna1.Civve = "X";
            I_Kna1.Milve = " ";
            I_Kna1.Kdkg1 = " "; // Customer condition group 1
            I_Kna1.Kdkg2 = " "; // Customer condition group 2
            I_Kna1.Kdkg3 = " "; // Customer condition group 3
            I_Kna1.Kdkg4 = " "; // Customer condition group 4
            I_Kna1.Kdkg5 = " "; // Customer condition group 5
            I_Kna1.Xknza = " ";
            I_Kna1.Fityp = " ";
            I_Kna1.Stcdt = " ";
            I_Kna1.Stcd3 = " ";
            I_Kna1.Stcd4 = " ";
            I_Kna1.Stcd5 = " ";
            I_Kna1.Xicms = " ";
            I_Kna1.Xxipi = " ";
            I_Kna1.Xsubt = " ";
            I_Kna1.Cfopc = " ";
            I_Kna1.Txlw1 = " ";
            I_Kna1.Txlw2 = " ";
            I_Kna1.Ccc01 = " ";
            I_Kna1.Ccc02 = " ";
            I_Kna1.Ccc03 = " ";
            I_Kna1.Ccc04 = " ";
            I_Kna1.Cassd = " ";
            I_Kna1.Knurl = " "; // Company Website Url
            I_Kna1.J_1kfrepre = " ";
            I_Kna1.J_1kftbus = " ";
            I_Kna1.J_1kftind = " ";
            I_Kna1.Confs = " ";
            I_Kna1.Updat = "00000000";
            I_Kna1.Uptim = "000000";
            I_Kna1.Nodel = " ";
            I_Kna1.Dear6 = " ";

            // !!!!!!!!!!!!!!!!!Add by myself, EU&TW didn't add this field!!!!!!!!!!!!!!!!
            //I_Kna1.Vso_R_Palhgt = 0;
            //I_Kna1.Vso_R_Pal_Ul = " ";
            //I_Kna1.Vso_R_Pk_Mat = " ";
            //I_Kna1.Vso_R_Matpal = " ";
            //I_Kna1.Vso_R_I_No_Lyr = "00";
            //I_Kna1.Vso_R_One_Mat = " ";
            //I_Kna1.Vso_R_One_Sort = " ";
            //I_Kna1.Vso_R_Uld_Side = " ";
            //I_Kna1.Vso_R_Load_Pref = " ";
            //I_Kna1.Vso_R_Dpoint = " ";
            // END

            I_Kna1.Alc = " ";
            I_Kna1.Pmt_Office = " ";
            I_Kna1.Psofg = " ";
            I_Kna1.Psois = " ";
            I_Kna1.Pson1 = " ";
            I_Kna1.Pson2 = " ";
            I_Kna1.Pson3 = " ";
            I_Kna1.Psovn = " ";
            I_Kna1.Psotl = " ";
            I_Kna1.Psohs = " ";
            I_Kna1.Psost = " ";
            I_Kna1.Psoo1 = " ";
            I_Kna1.Psoo2 = " ";
            I_Kna1.Psoo3 = " ";
            I_Kna1.Psoo4 = " ";
            I_Kna1.Psoo5 = " ";

            // !!!!!!!!!!!!!!!!!Add by myself, EU&TW didn't add this field!!!!!!!!!!!!!!!!
            //I_Kna1.Suframa = " ";
            //I_Kna1.Rg = " ";
            //I_Kna1.Exp = " ";
            //I_Kna1.Uf = " ";
            //I_Kna1.Rgdate = "00000000";
            //I_Kna1.Ric = "00000000000";
            //I_Kna1.Rne = " ";
            //I_Kna1.Rnedate = "00000000";
            //I_Kna1.Cnae = " ";
            //I_Kna1.Legalnat = "0000";
            //I_Kna1.Crtn = " ";
            //I_Kna1.Icmstaxpay = " ";
            //I_Kna1.Indtyp = " ";
            //I_Kna1.Tdt = " ";
            //I_Kna1.Comsize = " ";
            //I_Kna1.Decregpc = " ";
            //end
            #endregion

            #region KNB1 values setting
            I_Knb1.Mandt = "168";
            I_Knb1.Kunnr = ID_To_Create; // Company Id
            I_Knb1.Bukrs = _orgid;
            I_Knb1.Pernr = "00000000";
            I_Knb1.Erdat = _createdate;  // Create Time
            I_Knb1.Ernam = _creator;  // Creator
            I_Knb1.Sperr = " ";
            I_Knb1.Loevm = " ";
            I_Knb1.Zuawa = " "; // Unsure, EU & TW puts 001, SAP CP puts 009 ?????????????????????????

            I_Knb1.Busab = " "; // Accounting clerk, TW puts EI, SAP CP puts CT?????????????????
            I_Knb1.Akont = "0000121001";
            I_Knb1.Vlibb = 0;
            I_Knb1.Fdgrv = " ";
            I_Knb1.Vrsnr = " ";

            I_Knb1.Begru = " ";
            I_Knb1.Knrze = " ";
            I_Knb1.Knrzb = " ";
            I_Knb1.Zamim = " ";
            I_Knb1.Zamiv = " ";
            I_Knb1.Zamir = " ";
            I_Knb1.Zamib = " ";
            I_Knb1.Zamio = " ";
            I_Knb1.Zwels = " ";
            I_Knb1.Xverr = " ";
            I_Knb1.Zahls = " ";
            I_Knb1.Zterm = "PPD";
            I_Knb1.Wakon = " ";
            I_Knb1.Vzskz = " ";
            I_Knb1.Zindt = "00000000";
            I_Knb1.Zinrt = "00";
            I_Knb1.Eikto = " ";
            I_Knb1.Zsabe = " ";
            I_Knb1.Kverm = " ";
            I_Knb1.Vrbkz = " ";
            I_Knb1.Vrszl = 0;
            I_Knb1.Vrspr = 0;
            I_Knb1.Verdt = "00000000";
            I_Knb1.Perkz = " ";
            I_Knb1.Xdezv = " ";
            I_Knb1.Xausz = " ";
            I_Knb1.Webtr = 0;
            I_Knb1.Remit = " ";
            I_Knb1.Datlz = "00000000";
            I_Knb1.Xzver = "X";
            I_Knb1.Togru = " ";
            I_Knb1.Kultg = 0;
            I_Knb1.Hbkid = " ";
            I_Knb1.Xpore = " ";
            I_Knb1.Blnkz = " ";
            I_Knb1.Altkn = " ";
            I_Knb1.Zgrup = " ";
            I_Knb1.Urlid = " ";
            I_Knb1.Mgrup = "01"; //Dunning group - currently only one option 01
            I_Knb1.Lockb = " ";
            I_Knb1.Uzawe = " ";
            I_Knb1.Ekvbd = " ";
            I_Knb1.Sregl = " ";
            I_Knb1.Xedip = " ";
            I_Knb1.Frgrp = " ";
            I_Knb1.Vrsdg = " ";
            I_Knb1.Tlfxs = " ";
            I_Knb1.Intad = " ";
            I_Knb1.Xknzb = " ";
            I_Knb1.Guzte = " ";
            I_Knb1.Gricd = " ";
            I_Knb1.Gridt = " ";
            I_Knb1.Wbrsl = " ";
            I_Knb1.Confs = " ";
            I_Knb1.Updat = "00000000";
            I_Knb1.Uptim = "000000";
            I_Knb1.Nodel = " ";
            I_Knb1.Tlfns = " ";
            I_Knb1.Cession_Kz = " ";
            I_Knb1.Gmvkzd = " ";
            #endregion

            #region KNVV values setting
            // KNVV maps SAP Sales Area Data
            I_Knvv.Mandt = "168";
            I_Knvv.Kunnr = ID_To_Create;  // ERP ID     ======Will changed======
            I_Knvv.Vkorg = _orgid;
            I_Knvv.Vtweg = "00"; // Distribution Channel
            I_Knvv.Spart = "20"; // Division
            I_Knvv.Erdat = _createdate;  // Create Time
            I_Knvv.Ernam = _creator;  // Creator
            I_Knvv.Begru = " ";
            I_Knvv.Loevm = " ";
            I_Knvv.Versg = " ";
            I_Knvv.Aufsd = " ";
            I_Knvv.Kalks = "1";
            I_Knvv.Kdgrp = " ";
            I_Knvv.Bzirk = " ";
            I_Knvv.Konda = " ";
            I_Knvv.Pltyp = " ";
            I_Knvv.Awahr = "100";
            I_Knvv.Inco1 = " ";
            I_Knvv.Inco2 = " ";
            I_Knvv.Lifsd = " ";
            I_Knvv.Autlf = " ";
            I_Knvv.Antlf = 9;
            I_Knvv.Kztlf = " ";
            I_Knvv.Kzazu = "X";
            I_Knvv.Chspl = " ";
            I_Knvv.Lprio = "01"; // Delivery Priority
            I_Knvv.Eikto = " ";
            I_Knvv.Vsbed = "05"; // Shipping Conditions
            I_Knvv.Faksd = " ";
            I_Knvv.Mrnkz = " ";
            I_Knvv.Perfk = " ";
            I_Knvv.Perrl = " ";
            I_Knvv.Kvakz = " ";
            I_Knvv.Kvawt = 0;
            I_Knvv.Waers = _currency; // Currency
            I_Knvv.Klabc = " ";
            I_Knvv.Ktgrd = " ";
            I_Knvv.Zterm = " ";
            I_Knvv.Vwerk = " ";
            I_Knvv.Vkgrp = _salesgroupcode; // Sales Group
            I_Knvv.Vkbur = _salesofficecode; // Sales Office
            I_Knvv.Vsort = " ";
            I_Knvv.Kvgr1 = " ";
            I_Knvv.Kvgr2 = " ";
            I_Knvv.Kvgr3 = " ";
            I_Knvv.Kvgr4 = " ";
            I_Knvv.Kvgr5 = " ";
            I_Knvv.Bokre = " ";
            I_Knvv.Boidt = "00000000";
            I_Knvv.Kurst = " ";
            I_Knvv.Prfre = " ";
            I_Knvv.Prat1 = " ";
            I_Knvv.Prat2 = " ";
            I_Knvv.Prat3 = " ";
            I_Knvv.Prat4 = " ";
            I_Knvv.Prat5 = " ";
            I_Knvv.Prat6 = " ";
            I_Knvv.Prat7 = " ";
            I_Knvv.Prat8 = " ";
            I_Knvv.Prat9 = " ";
            I_Knvv.Prata = " ";
            I_Knvv.Kabss = " ";
            I_Knvv.Kkber = " ";
            I_Knvv.Cassd = " ";
            I_Knvv.Rdoff = " ";
            I_Knvv.Agrel = " ";
            I_Knvv.Megru = " ";
            I_Knvv.Uebto = 0;
            I_Knvv.Untto = 0;
            I_Knvv.Uebtk = " ";
            I_Knvv.Pvksm = " ";
            I_Knvv.Podkz = " ";
            I_Knvv.Podtg = 0;
            I_Knvv.Blind = " ";
            I_Knvv.Bev1_Emlgforts = " ";
            I_Knvv.Bev1_Emlgpfand = " ";
            I_Knvv.Carrier_Notif = " "; // !!!!!!!!!!!!!!!!!Add by myself, EU&TW didn't add this field!!!!!!!!!!!!!!!!
            #endregion

            #region KNVP values setting

            var VE = new SAPCustomerRFC.FKNVP();
            VE.Mandt = "168";
            VE.Kunnr = ID_To_Create;
            VE.Vkorg = _orgid;
            VE.Vtweg = "00";
            VE.Spart = "20";
            VE.Parvw = "WE";
            VE.Parza = "000";
            VE.Kunn2 = ID_To_Create;
            VE.Lifnr = " ";
            VE.Pernr = "00000000";
            VE.Parnr = "0000000000";
            VE.Knref = " ";
            VE.Defpa = " ";
            T_Xknvp.Add(VE);
            #endregion

            #region KNVI values setting
            // KNVI maps SAP Sales Area Data > Billing Documents Tag > Taxes subtable
            var T_Xknvidr_1 = new SAPCustomerRFC.FKNVI()
            {
                Tatyp = "MWST",
                Aland = "CN",
                Kunnr = ID_To_Create,
                Mandt = "168",
                Taxkd = "0"
            };
            T_Xknvi.Add(T_Xknvidr_1);

            var T_Xknvidr_2 = new SAPCustomerRFC.FKNVI()
            {
                Tatyp = "MWST",
                Aland = "PL",
                Kunnr = ID_To_Create,
                Mandt = "168",
                Taxkd = "0"
            };
            T_Xknvi.Add(T_Xknvidr_2);

            var T_Xknvidr_3 = new SAPCustomerRFC.FKNVI()
            {
                Tatyp = "MWST",
                Aland = "TW",
                Kunnr = ID_To_Create,
                Mandt = "168",
                Taxkd = "0"
            };
            T_Xknvi.Add(T_Xknvidr_3);

            var T_Xknvidr_4 = new SAPCustomerRFC.FKNVI()
            {
                Tatyp = "UTXJ",
                Aland = "US",
                Kunnr = ID_To_Create,
                Mandt = "168",
                Taxkd = "0"
            };
            T_Xknvi.Add(T_Xknvidr_4);

            #endregion

            try
            {
                p1.Zsd_Customer_Maintain_All(I_Bapiaddr1, I_Bapiaddr2, I_Customer_Is_Consumer,
                                I_Force_External_Number_Range, I_From_Customermaster,
                                I_Kna1, I_Knb1, I_Knb1_Reference, I_Knvv, I_Maintain_Address_By_Kna1,
                                I_No_Bank_Master_Update, I_Raise_No_Bte,
                                Pi_Add_On_Data, Pi_Cam_Changed, Pi_Postflag,
                            out E_Kunnr, out E_Sd_Cust_1321_Done, out O_Kna1, ref T_Upd_Txt,
                             ref T_Xkn, ref T_Xknb5, ref T_Xknbk, ref T_Xknex, ref T_Xknva, ref T_Xknvd, ref T_Xknvi,
                               ref T_Xknvk, ref T_Xknvl, ref T_Xknvp, ref T_Xknza, ref T_Ykn, ref T_Yknb5, ref T_Yknbk, ref T_Yknex, ref T_Yknva,
                               ref T_Yknvd, ref T_Yknvi, ref T_Yknvk, ref T_Yknvl, ref T_Yknvp, ref T_Yknza);
                p1.CommitWork();

                //Send Success Email
                smtpClient1.Send("myadvantech@advantech.com", "Frank.Chung@advantech.com.tw,YL.Huang@advantech.com.tw",
                string.Format("Create SAP US ShipTo Account Success,  NewERPID: {0}", ID_To_Create),
                string.Format("NewERPID: {0}", ID_To_Create) + "\r\nTime: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                String errmsg = ex.Message.ToString();

                // Send Exception Mail
                smtpClient1.Send("myadvantech@advantech.com", "Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,YL.Huang@advantech.com.tw",
                    string.Format("Create SAP US ShipTo Account Failed,  SoldtoID: {0}", ID_To_Create),
                    string.Format("NewERPID: {0}", ID_To_Create) + "\r\nTime: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") +
                    "\r\nError: " + errmsg);
            }
            p1.Connection.Close();
            return;
        }

        public static void UpdateSAPSalesAreaData(String _SoldtoID, String _ShiptoID, String _OrgID, Boolean _isTesting)
        {
            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);

            if (!isERPIDExist(_SoldtoID, _isTesting) || !isERPIDExist(_ShiptoID, _isTesting))
                return;

            String SAP_Connection = _isTesting ? "SAPConnTest" : "SAP_PRD";

            var knvpTable = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVPTable();
            var salesRow = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVP();
            var opRow = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVP();
            var isRow = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVP();
            var ShipToRow = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVP();
            var BillingRow = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVP();

            // ------If is updating SHIP TO, salesRow opRow and isRow are not needed.-----
            // SalesRow Settings
            //salesRow.Defpa = ""; salesRow.Knref = ""; salesRow.Kunn2 = ""; salesRow.Kunnr = _SoldtoID;
            //salesRow.Lifnr = ""; salesRow.Mandt = "168"; salesRow.Parnr = "0000000000"; salesRow.Parvw = "VE";
            //salesRow.Parza = "000"; salesRow.Spart = "00"; salesRow.Vkorg = _OrgID; salesRow.Vtweg = "00"; salesRow.Kz = "I";

            //salesRow.Pernr = "Salescode"; // Should Input

            //// opRow Settings
            //opRow.Defpa = ""; opRow.Knref = ""; opRow.Kunn2 = ""; opRow.Kunnr = _SoldtoID;
            //opRow.Lifnr = ""; opRow.Mandt = "168"; opRow.Parnr = "0000000000"; opRow.Parvw = "ZM";
            //opRow.Parza = "000"; opRow.Spart = "00"; opRow.Vkorg = _OrgID; opRow.Vtweg = "00"; opRow.Kz = "I";

            //opRow.Pernr = "R.OPCODE"; // Should Input

            //// isRow Settings
            //isRow.Defpa = ""; isRow.Knref = ""; isRow.Kunn2 = ""; isRow.Kunnr = _SoldtoID;
            //isRow.Lifnr = ""; isRow.Mandt = "168"; isRow.Parnr = "0000000000"; isRow.Parvw = "Z2";
            //isRow.Parza = "001"; isRow.Spart = "00"; isRow.Vkorg = _OrgID; isRow.Vtweg = "00"; isRow.Kz = "I";

            //isRow.Pernr = "SalesCode"; // Should Input
            //------------------------------------------------------------------------------

            // Ship to Row Settings
            ShipToRow.Defpa = ""; ShipToRow.Knref = ""; ShipToRow.Kunn2 = _ShiptoID; ShipToRow.Kunnr = _SoldtoID;
            ShipToRow.Lifnr = ""; ShipToRow.Mandt = "168"; ShipToRow.Parnr = "0000000000"; ShipToRow.Parvw = "WE";
            ShipToRow.Parza = New_knvp_Parza(_SoldtoID, "WE", _isTesting); ShipToRow.Pernr = "00000000"; ShipToRow.Spart = "20";
            ShipToRow.Vkorg = _OrgID; ShipToRow.Vtweg = "00"; ShipToRow.Kz = "I";

            knvpTable.Add(ShipToRow);

            try
            {
                if ((knvpTable.Count > 0))
                {
                    for (int i = 0; (i <= 3); i++)
                    {
                        if (Advantech.Myadvantech.DataAccess.SAPDAL.isERPIDExist(_SoldtoID, _isTesting))
                            break;
                        if (i == 3)
                        {
                            smtpClient1.Send("myadvantech@advantech.com", "Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,YL.Huang@advantech.com.tw",
                                            string.Format("Find SAP ERPID Failed: {0}", _SoldtoID),
                                            string.Format("Find SAP ERPID Failed\r\n SoldtoID: {0}, ShiptoID: {1}", _SoldtoID, _ShiptoID) + "\r\nTime: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            break;
                        }
                        System.Threading.Thread.Sleep(1000);
                    }

                    ZCUSTOMER_UPDATE_SALES_AREA.ZCUSTOMER_UPDATE_SALES_AREA p1 = new ZCUSTOMER_UPDATE_SALES_AREA.ZCUSTOMER_UPDATE_SALES_AREA();

                    p1.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings[SAP_Connection]);
                    p1.Connection.Open();

                    ZCUSTOMER_UPDATE_SALES_AREA.FKNVDTable vd1 = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVDTable();
                    ZCUSTOMER_UPDATE_SALES_AREA.KNVVTable vv = new ZCUSTOMER_UPDATE_SALES_AREA.KNVVTable();
                    ZCUSTOMER_UPDATE_SALES_AREA.FKNVDTable vd2 = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVDTable();
                    ZCUSTOMER_UPDATE_SALES_AREA.FKNVPTable vp = new ZCUSTOMER_UPDATE_SALES_AREA.FKNVPTable();

                    p1.Zcustomer_Update_Sales_Area(ref vd1, ref knvpTable, ref vv, ref vd2, ref vp);

                    p1.CommitWork();
                    p1.Connection.Close();

                    //Send Success Email
                    smtpClient1.Send("myadvantech@advantech.com", "Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,YL.Huang@advantech.com.tw",
                    string.Format("Update SAP US SoldTo ShipTo Relation Success,  SoldtoID: {0}, ShiptoID: {1}", _SoldtoID, _ShiptoID),
                    string.Format("SoldtoID: {0}, ShiptoID: {1}", _SoldtoID, _ShiptoID) + "\r\nTime: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
            }
            catch (Exception ex)
            {
                String errmsg = ex.ToString();

                // Send Exception Mail
                smtpClient1.Send("myadvantech@advantech.com", "Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,YL.Huang@advantech.com.tw",
                    string.Format("Update SAP US SoldTo ShipTo Relation Failed,  SoldtoID: {0}, ShiptoID: {1}", _SoldtoID, _ShiptoID),
                    string.Format("SoldtoID: {0}, ShiptoID: {1}", _SoldtoID, _ShiptoID) + "\r\nTime: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") +
                    "\r\nError: " + errmsg);
            }

        }

        public static String New_knvp_Parza(String _CompanyId, String _Flag, Boolean _isTesting)
        {
            String tmpParza = "001";
            String SAP_Connection = _isTesting ? "SAP_Test" : "SAP_PRD";

            while (true)
            {
                DataTable knvp_dt = OracleProvider.GetDataTable(SAP_Connection, "select Kunnr from saprdp.knvp  where Kunnr ='" + _CompanyId + "' and Parza ='" + tmpParza + "' and PARVW ='" + _Flag + "' ");
                if (knvp_dt.Rows.Count == 0)
                    break;
                else
                    tmpParza = string.Format("{0:000}", (int.Parse(tmpParza) + 1));
            }
            return tmpParza;
        }

        public static void UpdateContactPerson(String _kunnr, String _parnr, String _firstname, String _lastname, String _title, String _email, Boolean _isTesting)
        {

            Bapi_Addrcontpart_Savereplica_dll.Bapi_Addrcontpart_Savereplica_dll dll = new Bapi_Addrcontpart_Savereplica_dll.Bapi_Addrcontpart_Savereplica_dll();
            String SAP_Connection = _isTesting ? "SAPConnTest" : "SAP_PRD";
            dll.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings[SAP_Connection]);
            dll.Connection.Open();

            String context = "0005";
            String iv_check_address = "";
            String iv_time_depent = "";
            String obj_id_c = _kunnr; // ERPID
            String obj_id_ext = "";
            String obj_id_p = _parnr; // Parnr
            String obj_type_c = "KNA1"; // Need
            String obj_type_p = "BUS1006001"; // Need
            String address_number = "";
            String person_number = "";
            Bapi_Addrcontpart_Savereplica_dll.BAPIRET2 ret2 = new Bapi_Addrcontpart_Savereplica_dll.BAPIRET2();
            Bapi_Addrcontpart_Savereplica_dll.BAPIAD3VLTable ad3vl = new Bapi_Addrcontpart_Savereplica_dll.BAPIAD3VLTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADFAXTable adfax = new Bapi_Addrcontpart_Savereplica_dll.BAPIADFAXTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADPAGTable adpag = new Bapi_Addrcontpart_Savereplica_dll.BAPIADPAGTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADPRTTable adprt = new Bapi_Addrcontpart_Savereplica_dll.BAPIADPRTTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADRFCTable adrfc = new Bapi_Addrcontpart_Savereplica_dll.BAPIADRFCTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADRMLTable adrml = new Bapi_Addrcontpart_Savereplica_dll.BAPIADRMLTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADSMTPTable adsmtp = new Bapi_Addrcontpart_Savereplica_dll.BAPIADSMTPTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADSSFTable adssf = new Bapi_Addrcontpart_Savereplica_dll.BAPIADSSFTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADTELTable adtel = new Bapi_Addrcontpart_Savereplica_dll.BAPIADTELTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADTLXTable adtlx = new Bapi_Addrcontpart_Savereplica_dll.BAPIADTLXTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADTTXTable adttx = new Bapi_Addrcontpart_Savereplica_dll.BAPIADTTXTable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADURITable aduri = new Bapi_Addrcontpart_Savereplica_dll.BAPIADURITable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADUSETable aduse = new Bapi_Addrcontpart_Savereplica_dll.BAPIADUSETable();
            Bapi_Addrcontpart_Savereplica_dll.BAPIADX400Table adx400 = new Bapi_Addrcontpart_Savereplica_dll.BAPIADX400Table();
            Bapi_Addrcontpart_Savereplica_dll.BAPICOMREMTable comrem = new Bapi_Addrcontpart_Savereplica_dll.BAPICOMREMTable();

            // Settings
            Bapi_Addrcontpart_Savereplica_dll.BAPIAD3VL _ad3vl = new Bapi_Addrcontpart_Savereplica_dll.BAPIAD3VL();
            _ad3vl.Firstname = _firstname;
            _ad3vl.Lastname = _lastname;
            _ad3vl.Title_P = _title;
            ad3vl.Add(_ad3vl);

            Bapi_Addrcontpart_Savereplica_dll.BAPIADSMTP _adsmtp = new Bapi_Addrcontpart_Savereplica_dll.BAPIADSMTP();
            _adsmtp.E_Mail = _email;
            adsmtp.Add(_adsmtp);

            dll.Bapi_Addrcontpart_Savereplica(context, iv_check_address, iv_time_depent, obj_id_c,
                obj_id_ext, obj_id_p, obj_type_c, obj_type_p, out address_number,
                out person_number, out ret2, ref ad3vl, ref adfax, ref adpag, ref adprt, ref adrfc, ref adrml, ref adsmtp,
                ref adssf, ref adtel, ref adtlx, ref adttx, ref aduri, ref aduse, ref adx400, ref comrem);

            dll.CommitWork();
            dll.Connection.Close();

        }


        public static CreditLimitData GetCustomerCreditExposure(String ERPID, CreditControlAreaOptions CCAO)
        {
            String strHorizonDate = DateTime.Now.AddMonths(1).ToString("yyyyMMdd");

            //Ryan 20170829 Set HorizonDate to max value for AJP
            if (CCAO == CreditControlAreaOptions.JP01)
                strHorizonDate = DateTime.MaxValue.ToString("yyyyMMdd");

            GetCreditExposure.GetCreditExposure p = new GetCreditExposure.GetCreditExposure(ConfigurationManager.AppSettings["SAP_PRD"]);
            String cmware = "";
            Decimal Delta2Limit = 0;
            GetCreditExposure.KNKK dtKnkk = null;
            String Knkli = "";
            Decimal OpenDelivery = 0;
            Decimal OpenDeliverySecure = 0;
            Decimal OpenInvoice = 0;
            Decimal OpenInvoiceSecure = 0;
            Decimal Receivables = 0;
            Decimal OpenOrders = 0;
            Decimal OpenOrderSecure = 0;
            Decimal SpecialLiability = 0;
            Decimal SumOpen = 0;
            String Percentage = "";
            Decimal CreditLimit = 0;

            p.Connection.Open();
            CreditLimitData cld = null;
            try
            {
                p.Zcredit_Exposure(
                      strHorizonDate
                    , CCAO.ToString()
                    , ERPID
                    , out cmware
                    , out CreditLimit
                    , out Delta2Limit
                    , out dtKnkk
                    , out Knkli
                    , out OpenDelivery
                    , out OpenDeliverySecure
                    , out OpenInvoice
                    , out OpenInvoiceSecure
                    , out Receivables
                    , out OpenOrders
                    , out OpenOrderSecure
                    , out SpecialLiability
                    , out Percentage
                    , out SumOpen);

                cld = new CreditLimitData();
                cld.ERPID = ERPID;
                cld.CreditControlAreaOption = CCAO;
                //cld.HorizonDate = strHorizonDate;
                cld.Currency = cmware;
                cld.CreditLimit = CreditLimit;
                cld.Delta2Limit = Delta2Limit;
                cld.OpenDelivery = OpenDelivery;
                cld.OpenDeliverySecure = OpenDeliverySecure;
                cld.OpenInvoice = OpenInvoice;
                cld.OpenInvoiceSecure = OpenInvoiceSecure;
                cld.OpenOrders = OpenOrders;
                cld.OpenOrderSecure = OpenOrderSecure;
                cld.Percentage = Percentage;
                cld.SumOpen = SumOpen;
                cld.Receivables = Receivables;
                cld.CreditExposure = CreditLimit - Delta2Limit;

                // Check if current currency needs extra mark up from SAP table
                var objCurrencyMarkUp = OracleProvider.ExecuteScalar("SAP_PRD", String.Format("SELECT CURRDEC FROM SAPRDP.TCURX WHERE CURRKEY = '{0}'", cld.Currency));
                int CurrencyMarkUp = 1;
                if (objCurrencyMarkUp != null && int.TryParse(objCurrencyMarkUp.ToString(), out CurrencyMarkUp))
                {
                    CurrencyMarkUp = (int)(100 * Math.Pow(10, CurrencyMarkUp));
                    cld.CreditLimit = cld.CreditLimit * CurrencyMarkUp;
                    cld.Delta2Limit = cld.Delta2Limit * CurrencyMarkUp;
                    cld.OpenOrders = cld.OpenOrders * CurrencyMarkUp;
                    cld.SumOpen = cld.SumOpen * CurrencyMarkUp;
                    cld.Receivables = cld.Receivables * CurrencyMarkUp;
                    cld.CreditExposure = cld.CreditExposure * CurrencyMarkUp;
                    SpecialLiability = SpecialLiability * CurrencyMarkUp;
                }

                if (cld.CreditLimit == 0)
                {
                    cld.CreditLimitUsed = 0;
                }
                else
                {
                    //cld.CreditLimitUsed = Math.Round(cld.CreditExposure / cld.CreditLimit, 4, MidpointRounding.AwayFromZero);
                    cld.CreditLimitUsed = cld.CreditExposure / cld.CreditLimit;
                }

                cld.SalesValue = cld.CreditExposure - cld.Receivables - SpecialLiability;
                cld.RiskCategory = dtKnkk.Ctlpc;
                if (dtKnkk.Crblb.Trim().Equals("X", StringComparison.InvariantCultureIgnoreCase))
                {
                    cld.Blocked = true;
                }
                else
                {
                    cld.Blocked = false;
                }
            }
            catch (Exception ex)
            {
                p.Connection.Close();
            }
            p.Connection.Close();
            return cld;
        }

        public static DataTable GetSAPCompanyAddressByID(String _CompanyID, Boolean _isProduction = true)
        {
            String conn_str = _isProduction == true ? "SAP_PRD" : "SAPConnTest";

            String str = String.Format(@" Select * FROM  
                        (select a.kunnr as PARTN_NUMB,a.anred as TITLE, a.NAME1 as NAME, a.NAME2 as NAME_2,
                        a.STRAS as STREET, a.LAND1 as COUNTRY, a.LAND1 as COUNTRY_ISO, a.PSTLZ as POSTL_CODE,
                        '' as POBX_PCD, ''as POBX_CTY,  a.TELF2 as TELEPHONE2, a.TELBX as TELEBOX,
                        a.TELFX as FAX_NUMBER,  a.TELF1 as TELEPHONE,  a.TELTX as TELEX_NO, a.SPRAS as LANGU,
                        '' as LANGU_ISO, '' as UNLOAD_PT,a.REGIO as REGION,  '' as ADDRESS, '' as PRIV_ADDR,
                        1 as ADDR_TYPE, '' as ADDR_ORIG, '' as ADDR_LINK, '' as REFOBJTYPE, '' as REFOBJKEY,
                        '' as REFLOGSYS,  a.ADRNR as ADRNR
                        from saprdp.kna1 a where a.KUNNR='{0}') T
                        Left Join
                        (select  b.NAME3 as NAME_3, b.NAME4 as NAME_4,  b.CITY1 as CITY, b.CITY2 as DISTRICT,
                        b.CITY_CODE, b.CITYP_CODE as Distrct_No, b.PO_BOX as PO_BOX, b.TEL_EXTENS, b.TRANSPZONE,
                        b.TAXJURCODE, b.name_co as Attention, b.time_zone, b.deflt_comm, b.addrnumber, b.BUILDING,
                        b.DONT_USE_P, b.DONT_USE_S, b.FAX_EXTENS, b.FLOOR, b.HOUSE_NUM1, b.HOUSE_NUM2, b.HOUSE_NUM3,
                        b.PO_BOX_NUM, b.PO_BOX_CTY, b.PO_BOX_REG, b.HOME_CITY, b.CITYH_CODE, b.POST_CODE1,
                        b.POST_CODE2, b.POST_CODE3, b.REGIOGROUP, b.ROOMNUMBER, b.STR_SUPPL1, b.STR_SUPPL2,
                        b.STR_SUPPL3, b.STREETCODE, b.LOCATION 
                        from saprdp.adrc b
                        where b.addrnumber=(select adrnr from saprdp.kna1 a where a.kunnr='{0}' and rownum=1)) M 
                        on T.ADRNR=M.addrnumber", _CompanyID);
            DataTable dt = OracleProvider.GetDataTable(conn_str, str);

            return dt;
        }

        public static String GetPlantByOrg(String _OrgID)
        {
            if (!String.IsNullOrEmpty(_OrgID))
            {
                switch (_OrgID.ToUpper())
                {
                    case "CN10":
                        return "CNH1";
                    case "CN30":
                        return "CNH3";
                    case "CN70":
                        return "CNH7";
                    case "US10":
                        return "UBH1";
                    case "EU80":
                        return "DLM1";
                    case "TW20":
                        return "ASH1";
                    default:
                        return _OrgID.Substring(0, 2) + "H1";
                }
            }
            else
            {
                return "TWH1";
            }
        }

        public static String GetDefaultERPIDforSimulation(String _OrgID, String _Currency)
        {
            switch (_OrgID.ToUpper())
            {
                case "TW01":
                case "TW20":
                    if (_Currency.Equals("USD"))
                        return "AILR001";
                    else
                        return "2NC00001";
                case "US01":
                    return "UEPP5001";
                case "EU10":
                    if (_Currency.Equals("USD"))
                        return "EFESAL01";
                    else if (_Currency.Equals("GBP"))
                        return "EKGBEC02";
                    else
                        return "EDDEVI07";
                case "CN10":
                case "CN30":
                case "CN70":
                    return "C100077";
                case "JP01":
                    if (_Currency.Equals("USD"))
                        return "UUAASC";
                    else
                        return "JJCBOM";
                case "KR01":
                    return "AKRCE0416";
                case "US10":
                    return "BBESTORE";
                case "SG01":
                    if (_Currency.Equals("USD"))
                        return "SZC001";
                    else if (_Currency.Equals("SGD"))
                        return "SSC001";
                    else
                        return "SZC001";
                default:
                    return "";
            }
        }

        public static void SetACNMultiOrg(string erpId)
        {
            List<string> ValidACNOrg = new List<string>(new string[] { "CN10", "CN30", "CN70" });
            List<SAP_DIMCOMPANY> listSD = MyAdvantechDAL.GetSAPDIMCompanyByERPID(erpId).Where(d => ValidACNOrg.Contains(d.ORG_ID.ToUpper())).ToList();

            // Only update db if SAP_Dimcompany has 2 or more records
            if (listSD != null && listSD.Count > 1)
            {
                List<SAP_COMPANY_ORG> listSCO = MyAdvantechDAL.GetSAPCompanyOrgList(erpId);
                if (listSCO != null && listSCO.Count == 0)
                {
                    foreach (SAP_DIMCOMPANY sd in listSD)
                    {
                        SAP_COMPANY_ORG sco = new SAP_COMPANY_ORG();
                        sco.COMPANY_ID = sd.COMPANY_ID;
                        sco.ORG_ID = sd.ORG_ID;
                        sco.ORG_NAME = sd.ORG_ID;
                        sco.IS_DEFAULT = false;
                        if (sd.ORG_ID.Equals("CN10"))
                        {
                            sco.IS_DEFAULT = true;
                        }
                        MyAdvantechDAL.AddSAPCompanyOrg(sco);
                    }
                }
                else if (listSCO != null && listSCO.Count != listSD.Count)
                {
                    foreach (SAP_DIMCOMPANY sd in listSD)
                    {
                        if (!listSCO.Where(d => d.ORG_ID.Equals(sd.ORG_ID)).Any())
                        {
                            SAP_COMPANY_ORG sco = new SAP_COMPANY_ORG();
                            sco.COMPANY_ID = sd.COMPANY_ID;
                            sco.ORG_ID = sd.ORG_ID;
                            sco.ORG_NAME = sd.ORG_ID;
                            sco.IS_DEFAULT = false;
                            MyAdvantechDAL.AddSAPCompanyOrg(sco);
                        }
                    }
                }
            }
        }

        public static DataTable GetSAPOrderMasterBySO(String _SONo)
        {
            String str = String.Format(@"select VBELN AS ORDNO,WAERK AS CURR,VKORG AS ORG, 
                                        (SELECT DISTINCT BEZEI FROM SAPRDP.TVKBT WHERE VKBUR=A.VKBUR AND ROWNUM=1) AS OFFICE, 
                                        KUNNR AS COMPANYID, 
                                        (SELECT KUNNR FROM SAPRDP.VBPA WHERE SAPRDP.VBPA.VBELN=A.VBELN AND SAPRDP.VBPA.PARVW='WE' AND ROWNUM=1) AS SHIPTOID,
                                        (SELECT KUNNR FROM SAPRDP.VBPA WHERE SAPRDP.VBPA.VBELN=A.VBELN AND SAPRDP.VBPA.PARVW='RE' AND ROWNUM=1) AS BILLTOID,
                                        (SELECT NAME1 FROM SAPRDP.KNA1 WHERE KUNNR=A.KUNNR AND ROWNUM=1) AS COMPANYNAME 
                                        , A.* from SAPRDP.VBAK A where A.VBELN ='{0}'", _SONo);
            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", str);
            return dt;
        }

        public static DataTable GetSAPOrderDetailBySONo(String _SONo)
        {
            String str = String.Format(@"select cast(VBAP.UEPOS as integer) AS HigherLevel, cast(VBAP.POSNR as integer) AS Lineno, VBAP.MATWA AS  Partno,  
                                         cast(VBAP.LSMENG as integer) AS  Qty, VBAP.ZZ_EDATU AS ReqDate, VBAP.NETPR AS UnitPrice,VBAP.NETWR AS  Amount ,VBAP.*
                                         from saprdp.VBAP where VBAP.VBELN ='{0}' order by Lineno", _SONo);
            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", str);
            return dt;
        }

        public static void UpdateSAPSOforRevenueSplitting(String _SONo, String _Attr8, Boolean _isTesting)
        {
            try
            {
                ZSD_SO_HEADER_UPDATE.ZSD_SO_HEADER_UPDATE z = new ZSD_SO_HEADER_UPDATE.ZSD_SO_HEADER_UPDATE();
                ZSD_SO_HEADER_UPDATE.BAPIRETURN1Table ReturnDT = new ZSD_SO_HEADER_UPDATE.BAPIRETURN1Table();

                String SAP_Connection = _isTesting ? "SAPConnTest" : "SAP_PRD";
                z.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings[SAP_Connection]);
                z.Connection.Open();
                z.Zsd_So_Header_Update(_SONo, _Attr8, ref ReturnDT);
                z.CommitWork();
                z.Connection.Close();

                if (ReturnDT != null && ReturnDT.Count > 0)
                {
                    foreach (ZSD_SO_HEADER_UPDATE.BAPIRETURN1 b in ReturnDT)
                    {
                        if (b.Type.ToUpper().Equals("E"))
                        {
                            Common.SendMailUtil.SendMail("YL.Huang@advantech.com.tw", "Update SO Attr8 for revenue splitting failed", String.Format("RFC Returned Error: {0}", b.Message));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Common.SendMailUtil.SendMail("YL.Huang@advantech.com.tw", "Update SO Attr8 for revenue splitting Exception", String.Format("Error: {0}", e.ToString()));
            }
        }

        public static DataTable GetSAPContact(String _ERPID, Boolean _isTesting)
        {
            String SAPConnection = (_isTesting) ? "SAP_Test" : "SAP_PRD";

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select concat(concat(b.namev, ' '), b.name1) as fullname, b.abtnr, d.vtext as department, b.pafkt, e.vtext, c.smtp_addr as email, b.PARNR as SAPContactID ");
            sql.AppendFormat(" from saprdp.kna1 a inner join saprdp.knvk b on a.kunnr=b.kunnr ");
            sql.AppendFormat(" inner join saprdp.adr6 c on a.adrnr=c.addrnumber and b.prsnr=c.persnumber ");
            sql.AppendFormat(" inner join saprdp.tsabt d on b.abtnr=d.abtnr inner join saprdp.TPFKT e on b.pafkt=e.pafkt ");
            sql.AppendFormat(" where a.kunnr = '{0}' and d.spras='E' and e.spras='E' ", _ERPID);
            sql.AppendFormat(" and a.mandt='168' and b.mandt='168' and d.mandt='168' and e.mandt='168' ");
            sql.AppendFormat(" order by b.namev, b.parnr ");

            DataTable dt = OracleProvider.GetDataTable(SAPConnection, sql.ToString());
            return dt;
        }

        public static String GetSAPContactRowID(String _ERPID, String _eMail, Boolean _isTesting)
        {
            String SAPConnection = (_isTesting) ? "SAP_Test" : "SAP_PRD";

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select b.PARNR as SAPContactID ");
            sql.AppendFormat(" from saprdp.kna1 a inner join saprdp.knvk b on a.kunnr=b.kunnr ");
            sql.AppendFormat(" inner join saprdp.adr6 c on a.adrnr=c.addrnumber and b.prsnr=c.persnumber ");
            sql.AppendFormat(" inner join saprdp.tsabt d on b.abtnr=d.abtnr ");
            //sql.AppendFormat(" inner join saprdp.TPFKT e on b.pafkt=e.pafkt ");
            sql.AppendFormat(" where a.kunnr = '{0}' and d.spras='E' ", _ERPID);
            sql.AppendFormat(" and a.mandt='168' and b.mandt='168' and d.mandt='168' ");
            //sql.AppendFormat(" and e.spras='E' and e.mandt = '168' ");
            sql.AppendFormat(" and upper(c.smtp_addr) = '{0}' and rownum = 1 ", _eMail.ToUpper());
            sql.AppendFormat(" order by b.namev, b.parnr ");

            var SAPContactID = OracleProvider.ExecuteScalar(SAPConnection, sql.ToString());

            if (SAPContactID != null && !String.IsNullOrEmpty(SAPContactID.ToString()))
            {
                return SAPContactID.ToString();
            }

            return String.Empty;
        }

        public static void CreateSAPContact(Boolean _isTesting, String _SoldtoID, String _FirstName, String _LastName, String _EMail, String _TEL, String _TELExt, String _DepartmentCode, String _JobFunctionCode)
        {
            var RC1 = new CreateSAPContact.CreateSAPContact();
            string SAPRFCconnection = "SAP_PRD"; if (_isTesting) SAPRFCconnection = "SAPConnTest";
            String SoldToId = _SoldtoID;
            String FirstName = _FirstName;
            String LastName = _LastName;
            String ContactEmail = _EMail;
            String Telephone = _TEL;
            String TelExt = _TELExt;
            String DepartmentCode = _DepartmentCode;
            String JobTitleCode = _JobFunctionCode;

            var CreationLogTable = new CreateSAPContact.ZSSD_07_LOGTable();
            RC1.Connection = new SAP.Connector.SAPConnection(System.Configuration.ConfigurationManager.AppSettings[SAPRFCconnection]);
            RC1.Connection.Open();
            RC1.Z_B2c_Contact_Create(DepartmentCode, SoldToId, ContactEmail, TelExt, FirstName, LastName, JobTitleCode, Telephone, ref CreationLogTable);
            RC1.Connection.Close();
        }

        public static bool isNCNRPart(String _partNo, String _plant)
        {
            SAP_PRODUCT sp = MyAdvantechDAL.GetSAPProduct(_partNo);
            SAP_PRODUCT_ABC spabc = MyAdvantechDAL.GetSAPProductABC(_partNo, _plant);

            if (spabc != null && !string.IsNullOrEmpty(spabc.ABC_INDICATOR))
            {
                if (spabc.ABC_INDICATOR.Equals("C") || spabc.ABC_INDICATOR.Equals("D") || spabc.ABC_INDICATOR.Equals("T") || spabc.ABC_INDICATOR.Equals("P"))
                {
                    return true;
                }
                else if (spabc.ABC_INDICATOR.Equals("X") || spabc.ABC_INDICATOR.Equals("Y"))
                {
                    if (sp != null && !String.IsNullOrEmpty(sp.PRODUCT_TYPE) && sp.PRODUCT_TYPE.Equals("ZPER"))
                        return true;
                }
            }

            return false;
        }



        /// <summary>
        /// Get OS part's ITP
        /// 若料號為968C開頭，則先檢查ZBMM129，若無，再檢查ZBMM0011
        ///若料號為209B或968B開頭，則不檢查ZBMM129，直接檢查ZBMM0011
        ///不管客戶為CN10還是CN30，都一律以CN10去ZBMM129及ZBMM0011撈成本
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="erpId"></param>
        /// <returns></returns>
        public static decimal GetACNOSPartITP(string partNo, string erpId)
        {
            decimal itp = 0m;
            var vendorId = "GL1601KR";
            StringBuilder _sql = new StringBuilder();
            if (partNo.StartsWith("968C", StringComparison.InvariantCultureIgnoreCase))
            {
                _sql.Append(" select netpr/peinh as itp from saprdp.ztmm_93  ");
                _sql.Append(" Where mandt='168' and ekorg='CN10' and waers='CNY' and to_char(sysdate,'YYYYMMDD') between date_f and date_t ");
                _sql.AppendFormat(" and matnr='{0}' and kunnr='{1}' ", partNo, erpId);
                DataTable dt = OracleProvider.GetDataTable("SAP_PRD", _sql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (decimal.TryParse(dt.Rows[0]["itp"].ToString(), out itp))
                        return itp;
                }
                vendorId = "GM0009B";
            }

            _sql = new StringBuilder();
            _sql.Append(" select b.netpr/b.peinh as itp from saprdp.eina a inner join saprdp.eine b on a.infnr=b.infnr ");
            _sql.Append(" where  a.mandt='168' and b.mandt='168' and b.prdat>=to_char(sysdate,'YYYYMMDD') and b.ekorg='CN10' and b.waers='CNY' ");
            _sql.AppendFormat(" and a.lifnr='{0}' and a.matnr='{1}' ", vendorId, partNo); //For 968C use GM0009B, for 968B or 209B use GL1601KR
            DataTable dt2 = OracleProvider.GetDataTable("SAP_PRD", _sql.ToString());
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                if (decimal.TryParse(dt2.Rows[0]["itp"].ToString(), out itp))
                    return itp;
            }


            return itp;
        }

        /// <summary>
        /// Get Periodic Cost
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="plant"></param>
        /// <param name="SAPCurrecny"></param>
        /// <returns></returns>
        public static decimal GetPeriodicCost(string partNo, string plant, string SAPCurrecny)
        {
            decimal itp = 0m;
            var RC1 = new GetSAPPeriodCost.GetSAPPeriodCost();
            try
            {
                GetSAPPeriodCost.ZGETCOST1 cost = new GetSAPPeriodCost.ZGETCOST1();
                RC1.Connection = new SAP.Connector.SAPConnection(ConfigurationManager.AppSettings["SAP_PRD"]);
                RC1.Connection.Open();
                RC1.Z_Get_Periodic_Cost("", partNo, "", plant, out cost);
                switch (SAPCurrecny)
                {
                    case "CNY":
                        itp = cost.Pvprs / cost.Peinh;
                        break;
                    case "USD":
                        itp = cost.G_Pvprs / cost.G_Peinh;
                        break;
                    default:
                        itp = 0m;
                        break;
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                RC1.Connection.Close();
            }

            return itp;
        }


        /// <summary>
        /// Get STO Plant
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="plant"></param>
        /// <returns></returns>
        public static string GetSTOPlant(string partNo, string plant)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.Append(" select b.SOBSL as SpecialProcurement, b.ltext as LongText from saprdp.marc a inner join saprdp.t460t b on a.werks = b.werks and a.sobsl = b.sobsl ");
            _sql.AppendFormat(" where a.matnr = '{0}' and a.werks = '{1}' and b.spras = 'E' ", partNo, plant);
            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", _sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                var STOPlant = dt.Rows[0]["LongText"].ToString().Substring(7, 4);
                if (STOPlant != null)
                    return STOPlant;
            }
            return String.Empty;
        }

        public static DataTable GetSAPCountryList()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(" select land1　as COUNTRY, landx as COUNTRYNAME from saprdp.t005t where mandt='168' and spras='E'  order by landx ");
            var dt = OracleProvider.GetDataTable("SAP_PRD", sb.ToString());
            return dt;
        }

        public static bool IsWarrantableV3(string partNo, string orgId)
        {
            if (string.Equals(orgId, "EU10", StringComparison.CurrentCultureIgnoreCase))
                return true;
            else if (string.Equals(orgId, "JP01", StringComparison.CurrentCultureIgnoreCase))
            {
                if (IsSoftwareV3(partNo))
                    return false;
                else if (partNo.StartsWith("XAJP"))
                    return false;
                else
                    return true;
            }
            else
            {
                if (IsSoftwareV3(partNo))
                    return false;
                else
                    return true;
            }
        }

        public static bool IsSoftwareV3(string partNo)
        {
            var dt = SqlProvider.dbGetDataTable("MY", String.Format("select * from SAP_PRODUCT where PART_NO = '{0}' ", partNo));
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["MATERIAL_GROUP"] != null &&
                    (
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "ZSRV" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "968MS" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "96SW" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "98" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "ZHD0" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "ZSPC" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "ZINC" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "206" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "207" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "968MS/SW" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "968OT" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "968WA" ||
                        dt.Rows[0]["MATERIAL_GROUP"].ToString() == "98DP"
                    )
                    || dt.Rows[0]["MATERIAL_GROUP"].ToString() == "20"
                )
                    return true;
                else if (dt.Rows[0]["PRODUCT_TYPE"].ToString() == "ZCON")
                    return true;
                else
                    return false;
            }
            else
                return false;

        }

    }



    public class CreditLimitData
    {
        public virtual string ERPID { get; set; }
        public virtual CreditControlAreaOptions CreditControlAreaOption { get; set; }
        public virtual DateTime HorizonDate { get; set; }
        public virtual String Currency { get; set; }
        public virtual Decimal CreditLimit { get; set; }

        public virtual Decimal CreditLimitUsed { get; set; }

        public virtual Decimal CreditExposure { get; set; }

        public virtual Decimal SalesValue { get; set; }

        public virtual Decimal Delta2Limit { get; set; }
        public virtual string Percentage { get; set; }
        public virtual Decimal Receivables { get; set; }
        public virtual Decimal SpecialLiability { get; set; }

        public virtual Decimal OpenDelivery { get; set; }
        public virtual Decimal OpenDeliverySecure { get; set; }
        public virtual Decimal OpenInvoice { get; set; }
        public virtual Decimal OpenInvoiceSecure { get; set; }
        public virtual Decimal OpenOrders { get; set; }
        public virtual Decimal OpenOrderSecure { get; set; }
        public virtual Decimal SumOpen { get; set; }


        public virtual String RiskCategory { get; set; }

        public virtual Boolean Blocked { get; set; }
    }

    public class PriceATP
    {
        public string Price { get; set; }
        public string ATPDate { get; set; }
        public int ATPQty { get; set; }
    }

}
