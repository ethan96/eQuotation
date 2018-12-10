using System;
using System.Collections.Generic;
using Advantech.Myadvantech.DataAccess;
using System.Data;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.Business
{
    public class GPControlBusinessLogic
    {
        public static Boolean AEUCartGPValidation(String _CartID, String _CompanyID, ref Decimal StandardMargin, ref Decimal PTDMargin)
        {
            // 0. No need GP validation if cartitems' price are never changed.
            if (!IsCartPriceUpdated(_CartID))
                return false;

            // 1. Get all cart items and exclude invalid type
            int StandardGPLevel = 0, PTDGPLevel = 0;

            List<string> InvalidType = new List<string> { "EPCS", "DIST", "ECBS", "EDOS", "ECMS", "ESMS" };
            List<Product> items = (from CartDetail in MyAdvantechContext.Current.cart_DETAIL_V2
                                   join SAPProduct in MyAdvantechContext.Current.SAP_PRODUCT on CartDetail.Part_No equals SAPProduct.PART_NO
                                   where CartDetail.Cart_Id.Equals(_CartID) &&
                                   (CartDetail.Line_No >= 100 ? !InvalidType.Contains(SAPProduct.PRODUCT_LINE) : true)
                                   select new Product
                                   {
                                       LineNumber = (int)CartDetail.Line_No,
                                       PartNumber = CartDetail.Part_No,
                                       UnitPrice = (Decimal)CartDetail.Unit_Price,
                                       ListPrice = (Decimal)CartDetail.List_Price,
                                       Quantity = (int)CartDetail.Qty,
                                       ITP = CartDetail.Itp == null ? 0 : (Decimal)CartDetail.Itp
                                   }).ToList();


            // 2. Get GP approver level
            List<GPBLOCK_LOGIC> GPitems = GetAEUApproverLevel(_CompanyID);

            // 3. Get Standard items margin
            // 3-1 Get Margin without AGS            
            StandardMargin = GetMarginWithoutAGS(items);
            // 3-2 Validate if Standard margin below GP (level?)
            foreach (GPBLOCK_LOGIC g in GPitems)
            {
                if (StandardMargin != -99999 && g.gp_level > Convert.ToDouble(StandardMargin))
                    StandardGPLevel++;
            }

            // 4. Get PTD items margin
            // 4-1 Get PTD Margin
            PTDMargin = GetMarginPTD(items);
            // 4-2 Validate if PTD margin below GP (level?)
            if (PTDMargin != -99999 && (0.05) > Convert.ToDouble(PTDMargin))
                PTDGPLevel++;

            // 5. return true if level > 0
            if (StandardGPLevel > 0 || PTDGPLevel > 0)
            {
                if (StandardMargin == -99999)
                    StandardMargin = 0;
                if (PTDMargin == -99999)
                    PTDMargin = 0;
                return true;
            }

            return false;
        }

        public static Boolean ACNLooseItemCartGPValidation(String _CartID, Decimal _TaxRate, ref Decimal TotalMargin)
        {
            // 0. No need GP validation if cartitems' price are never changed.
            if (!IsCartPriceUpdated(_CartID))
                return false;

            // 1. Get all cart items
            List<Product> items = (from CartDetail in MyAdvantechContext.Current.cart_DETAIL_V2
                                   where CartDetail.Cart_Id.Equals(_CartID) && CartDetail.Line_No < 100
                                   select new Product
                                   {
                                       LineNumber = (int)CartDetail.Line_No,
                                       PartNumber = CartDetail.Part_No,
                                       UnitPrice = (Decimal)CartDetail.Unit_Price,
                                       ListPrice = (Decimal)CartDetail.List_Price,
                                       Quantity = (int)CartDetail.Qty,
                                       ITP = CartDetail.Itp == null ? 0 : (Decimal)CartDetail.Itp
                                   }).ToList();

            // 2. Get total margin with specific logic (unit price * 1.17 for tax, os itp * 1.20, PTD * 1.22, else items itp * 1.24)            
            TotalMargin = GetACNPostTaxTotalMargin(items, _TaxRate);

            // 3. return true if level > 0
            if(TotalMargin < 0)            
                return true;
            else
                return false;
        }

        public static Decimal GetMarginWithoutAGS(List<Product> _items)
        {
            decimal r = -99999;
            decimal sumAmt = 0;
            decimal sumITP = 0;
            foreach (Product p in _items)
            {
                if (!Business.PartBusinessLogic.IsPTD(p.PartNumber) & !Business.PartBusinessLogic.IsNonStandardSensitiveITP(p.PartNumber))
                {
                    sumAmt += p.UnitPrice * p.Quantity;
                    sumITP += p.ITP * p.Quantity;
                }
            }
            if (sumAmt != 0)
            {
                r = (sumAmt - sumITP) / sumAmt;
            }
            return r;
        }


        public static Decimal GetMarginPTD(List<Product> _items)
        {
            decimal r = -99999;
            decimal sumAmt = 0;
            decimal sumITP = 0;
            foreach (Product p in _items)
            {
                if (Business.PartBusinessLogic.IsPTD(p.PartNumber))
                {
                    sumAmt += p.UnitPrice * p.Quantity;
                    sumITP += p.ITP * p.Quantity;
                }
            }
            if (sumAmt != 0)
            {
                r = (sumAmt - sumITP) / sumAmt;
            }
            return r;
        }

        public static Decimal GetMarginTotal(List<Product> _items)
        {
            decimal r = -99999;
            decimal sumAmt = 0;
            decimal sumITP = 0;
            foreach (Product p in _items)
            {
                sumAmt += p.UnitPrice * p.Quantity;
                sumITP += p.ITP * p.Quantity;
            }
            if (sumAmt != 0)
            {
                r = (sumAmt - sumITP) / sumAmt;
            }
            return r;
        }


        public static Boolean IsCartPriceUpdated(String _CartID)
        {
            MyAdvantechDAL my = new MyAdvantechDAL();
            List<cart_DETAIL_V2> list_cd = my.GetCartDetailV2ByCartID(_CartID);

            if (list_cd.Where(d => d.otype != (int)LineItemType.BTOSParent && d.oUnit_Price != d.Unit_Price).Any())
                return true;
            else
                return false;
        }

        public static List<GPBLOCK_LOGIC> GetAEUApproverLevel(String _CompanyID)
        {
            String office = String.Empty, group = String.Empty;
            GetOfficeGroupByERPID(_CompanyID, "EU10", ref office, ref group);

            List<GPBLOCK_LOGIC> list_gp = eQuotationContext.Current.GPBLOCK_LOGIC
                                            .Where(d => d.Office_code.Equals(office) && d.group_code.Equals(group) && d.Active == 1 && d.Type.Equals("GP"))
                                            .OrderBy(d => d.gp_level).ToList();

            return list_gp;
        }

        public static void GetOfficeGroupByERPID(String _CompanyID, String _Org, ref String _Office, ref String _Group)
        {
            SAP_DIMCOMPANY sd = MyAdvantechContext.Current.SAP_DIMCOMPANY.Where(d => d.ORG_ID.Equals(_Org) && d.COMPANY_ID.Equals(_CompanyID)).FirstOrDefault();

            if (sd != null)
            {
                _Office = sd.SALESOFFICE;
                _Group = sd.SALESGROUP;
            }
        }

        public static decimal GetACNConfigureMargin(List<Product> _items)
        {
            decimal margin = -99999;
            decimal sumUnitPtice = 0;
            decimal sumITP = 0;

            sumUnitPtice = _items.Sum(p => p.UnitPrice);
            sumITP = _items.Sum(p => p.ITP);

            if (sumUnitPtice != 0)
                margin = (sumUnitPtice - sumITP) / sumUnitPtice;
            return margin;

        }

        public static decimal GetACNMargin(Product _item)
        {
            decimal margin = -99999;
            if (_item.UnitPrice > 0 && _item.ITP > 0)
                margin = (_item.UnitPrice - _item.ITP) / _item.UnitPrice;
            return margin;
        }

        public static decimal GetACNPostTaxTotalMargin(List<Product> _items, Decimal _TaxRate)
        {
            decimal r = -99999;
            decimal sumAmt = 0;
            decimal sumITP = 0;
            foreach (Product p in _items)
            {
                sumAmt += p.UnitPrice * _TaxRate * p.Quantity;

                if(PartBusinessLogic.isACNOSParts(p.PartNumber))
                    sumITP += p.ITP * Convert.ToDecimal(1.20) * p.Quantity;
                else if(PartBusinessLogic.IsPTD(p.PartNumber))
                    sumITP += p.ITP * Convert.ToDecimal(1.22) * p.Quantity;
                else
                    sumITP += p.ITP * Convert.ToDecimal(1.24) * p.Quantity;
            }
            if (sumAmt != 0)
            {
                r = (sumAmt - sumITP) / sumAmt;
            }
            return r;

        }


        public static string[] FindAISCApprover(string salesEmail, decimal margin,string salesTeamName)
        {
            //DataTable _dt = eQuotationDAL.getGP_ParameterByMargin("AISC-iRetail", margin);
            DataTable _dt = eQuotationDAL.getGP_ParameterByMargin(salesTeamName, margin);
            String[] _s = new string[_dt.Rows.Count];
            int i = 0;
            foreach(DataRow _row in _dt.Rows){
                _s[i] = _row["Approver"].ToString();
                i++;
            }
            return _s;
        }

        public static string[] FindApproversByMarginAndSalesTeam(decimal margin, string salesTeamName)
        {
            DataTable _dt = eQuotationDAL.getGP_ParameterByMargin(salesTeamName, margin);
            String[] _s = new string[_dt.Rows.Count];
            int i = 0;
            foreach (DataRow _row in _dt.Rows)
            {
                _s[i] = _row["Approver"].ToString();
                i++;
            }
            return _s;
        }
        public static List<GPBLOCK_LOGIC> GetBBApprovalListByCompanyID(string companyID)
        {

            String office = String.Empty, group = String.Empty;
            if(string.IsNullOrEmpty(companyID))
            {
                office = "2900";
                group = "290";
            }
            else
                GetOfficeGroupByERPID(companyID, "US10", ref office, ref group);


            List<GPBLOCK_LOGIC> list_gp = eQuotationContext.Current.GPBLOCK_LOGIC
                                            .Where(d => d.Office_code.Equals(office) && d.group_code.Equals(group) && d.Active == 1 && d.Type.Equals("GP"))
                                            .OrderByDescending(d => d.gp_level).ToList();


            return list_gp;
        }

        public static List<GPBLOCK_LOGIC> GetASGApprovalListByCompanyID(string companyID)
        {

            String office = String.Empty, group = String.Empty;
            //if (string.IsNullOrEmpty(companyID))
            //{
            //    office = "2900";
            //    group = "290";
            //}
            //else
            //    GetOfficeGroupByERPID(companyID, "US10", ref office, ref group);

            office = "2900";
            group = "290";
            List<GPBLOCK_LOGIC> list_gp = eQuotationContext.Current.GPBLOCK_LOGIC
                                            .Where(d => d.Office_code.Equals(office) && d.group_code.Equals(group) && d.Active == 1 && d.Type.Equals("GP"))
                                            .OrderByDescending(d => d.gp_level).ToList();


            return list_gp;
        }

        public static List<GPBLOCK_LOGIC> GetBBApprovalListByCompanyIDSalesEmailAndSalesCode(string companyID,string salesEmail,string salesCode)
        {

            String office = String.Empty, group = String.Empty;
            if (string.IsNullOrEmpty(companyID))
            {
                office = "2900";
                var sales  =  MyAdvantechContext.Current.SAP_EMPLOYEE.Where(s => s.EMAIL == salesEmail && s.SALES_CODE == salesCode).FirstOrDefault();
                if (sales != null)
                    group = sales.SALESGROUP;
                else
                    group = "290";
            }
            else
                GetOfficeGroupByERPID(companyID, "US10", ref office, ref group);



            List<GPBLOCK_LOGIC> list_gp = eQuotationContext.Current.GPBLOCK_LOGIC
                                            .Where(d => d.Office_code.Equals(office) && d.group_code.Equals(group) && d.Active == 1 && d.Type.Equals("GP"))
                                            .OrderByDescending(d => d.gp_level).ToList();

            //20180411 Alex: Per Tracy, Fanny’s quote will bypass Tim and goes to Jerry directly
            if (salesEmail.Equals("FSCARGLE@ADVANTECH-BB.COM", StringComparison.CurrentCultureIgnoreCase))
            {
                list_gp = eQuotationContext.Current.GPBLOCK_LOGIC
                    .Where(d => d.Office_code.Equals(office) && d.group_code.Equals(group) && d.Active == 1 
                                && d.Type.Equals("GP") && d.approver.Equals("jogorman@advantech-bb.com", StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            return list_gp;
        }

        public static string FindApproverBySalesAndApproveLevel(string salesEmail, int approveLevel, string sector)
        {

            var eQSales = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesEmail == salesEmail && s.Level == approveLevel && s.Sector == sector).FirstOrDefault();
            if (eQSales != null)
                return eQSales.ApproverEmail;
            return "";
        }

        public static string FindApproverBySalesCodeAndApproveLevel(string salesCode, int approveLevel, string sector)
        {

            var eQSales = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesCode == salesCode && s.Level == approveLevel && s.Sector == sector).FirstOrDefault();
            if (eQSales != null)
                return eQSales.ApproverEmail;
            return "";
        }


        public static List<string> FindPSMApproversByProductDivAndApproveLevel(string productDiv, int PSMLevel)
        {
            var EQ_PSM = eQuotationContext.Current.ACN_EQ_PSM.Where(s => s.ProductDivision == productDiv && s.Level == PSMLevel).ToList();

            if (EQ_PSM != null)
                return EQ_PSM.Select(p =>  p.PSM ).ToList(); 
            else
                return new List<string>();

        }

        public static ACN_EQ_PSM FindACNPSMRuleById(int id)
        {
            return eQuotationContext.Current.ACN_EQ_PSM.SingleOrDefault(s => s.Id == id );

        }

        public static List<ACN_EQ_PSM> FindACNPSMRulesByProductDiv(string productDiv)
        {
            return eQuotationContext.Current.ACN_EQ_PSM.Where(s => s.ProductDivision == productDiv).ToList();

        }


        public static int GetACNMaxPSMApprovalLevel(string sector)
        {

            var regionParameterVaule = eQuotationDAL.GetRegionParameterValue("ACN", sector, "MaxPSMApprovalLevel", "0");

            return Convert.ToInt32(regionParameterVaule);
        }

        public static List<ACN_EQ_PSM> FindACNPSMApproversByApproveLevel(int PSMLevel)
        {
            return eQuotationContext.Current.ACN_EQ_PSM.Where(s =>  s.Level == PSMLevel).ToList();

        }

        public static List<WorkFlowApproval> GetQuoteWaitApprovalsByQuoteId(string quoteId)
        {
            var QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId && p.Status == (int)QuoteApprovalStatus.Wait_for_review).OrderBy(p => p.LevelNum).ToList();
            return QAlist;
        }

        public static WorkFlowApproval GetQuoteApprovalByApproveId(string approveId)
        {
            var qa = eQuotationContext.Current.WorkFlowApproval.Where(a => a.UID == approveId).FirstOrDefault();
            return qa;
        }

        public static List<WorkFlowApproval> GetQuoteApprovedApprovalByQuoteId(string quoteId)
        {
            var QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId && p.Status == (int)QuoteApprovalStatus.Approved).OrderBy(p => p.LevelNum).ToList();
            return QAlist;
        }

        public static List<WorkFlowApproval> GetQuoteAllApprovalsByQuoteId(string quoteId)
        {
            var QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId ).OrderBy(p => p.LevelNum).ToList();
            return QAlist;
        }

        public static WorkFlowApproval GetQuoteWaitApprovalByApproveId(string approveId)
        {
            var qa = eQuotationContext.Current.WorkFlowApproval.Where(a => a.UID == approveId && a.Status == (int)QuoteApprovalStatus.Wait_for_review).FirstOrDefault();
            return qa;
        }

        public static WorkFlowApproval GetQuoteApprovalByMobileId(string mobileId)
        {
            var qa = eQuotationContext.Current.WorkFlowApproval
                .Where(a => (a.MobileYes == mobileId || a.MobileNo == mobileId)).FirstOrDefault();
            return qa;
        }

        public static void UpdateQuoteWaitApprovalWorkflowId(string quoteId, string newWorkflowId)
        {
            var qaList = eQuotationContext.Current.WorkFlowApproval
                .Where(a => a.TypeID == quoteId && a.Status == (int)QuoteApprovalStatus.Wait_for_review).ToList();
            if (qaList.Any())
            {
                foreach (var qa in qaList)
                {
                    qa.WorkFlowID = newWorkflowId;
                    qa.Update();
                }
            }
        }


        public static void RemoveQuoteWaitApprovals(string quoteId)
        {
            var QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId && p.Status == (int)QuoteApprovalStatus.Wait_for_review).OrderBy(p => p.LevelNum).ToList();
            if (QAlist.Any())
                foreach (var qa in QAlist)
                    qa.Remove();
        }

        public static void RemoveQuoteWaitApprovalsByLevel(string quoteId, decimal levelNum)
        {
            var QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId && p.Status == (int)QuoteApprovalStatus.Wait_for_review && p.LevelNum > levelNum).OrderBy(p => p.LevelNum).ToList();
            if (QAlist.Any())
                foreach (var qa in QAlist)
                    qa.Remove();
        }

        public static void AssignWorkFlowIdToApprovers(string quoteId, string workflowId)
        {
            var QAlist = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteId && p.Status == (int)QuoteApprovalStatus.Wait_for_review).OrderBy(p => p.LevelNum).ToList();
            if (QAlist.Any())
                foreach (var qa in QAlist)
                {
                    qa.WorkFlowID = workflowId;
                    qa.Update();
                }

        }

        public static void CreateQuoteApprover(string quoteID, double approveLevel, string workFlowID, string url, string approver, string approverType, bool viewGP, string mailBody, string finalMailBody)
        {
            if (!string.IsNullOrEmpty(approver))
            {
                WorkFlowApproval existApproval = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteID && p.Approver == approver && p.LevelNum == (decimal)approveLevel && p.WorkFlowID == workFlowID).FirstOrDefault();
                if (existApproval == null)
                {
                    WorkFlowApproval QA = new WorkFlowApproval();
                    QA.UID = System.Guid.NewGuid().ToString();
                    QA.Type = "Quote";
                    QA.TypeID = quoteID;
                    QA.ApproverType = approverType;
                    QA.LevelNum = (decimal)approveLevel;
                    QA.Approver = approver;
                    QA.Status = (int)QuoteApprovalStatus.Wait_for_review;
                    QA.WorkFlowID = workFlowID;
                    QA.IsSendSuccessfully = 0;
                    QA.MobileYes = "MY" + System.Guid.NewGuid().ToString();
                    QA.MobileNo = "MN" + System.Guid.NewGuid().ToString();
                    QA.CreatedDate = DateTime.Now;
                    QA.LastUpdatedDate = DateTime.Now;
                    //QA.Mailbody = getGPmailBodyForInterconApprover(quoteID);


                    if (mailBody == null)
                    {
                        var maxLevelApproved = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteID && p.WorkFlowID == workFlowID && p.Status == (int)QuoteApprovalStatus.Approved).OrderByDescending(p => p.LevelNum).FirstOrDefault();
                        if (maxLevelApproved != null)
                            QA.Mailbody = maxLevelApproved.Mailbody;
                        else
                            QA.Mailbody = "";

                    }
                    else
                        QA.Mailbody = mailBody;


                    if (finalMailBody == null)
                    {
                        var maxLevelApproved = eQuotationContext.Current.WorkFlowApproval.Where(p => p.TypeID == quoteID && p.WorkFlowID == workFlowID && p.Status == (int)QuoteApprovalStatus.Approved).OrderByDescending(p => p.LevelNum).FirstOrDefault();
                        if (maxLevelApproved != null)
                            QA.FinalMailBody = maxLevelApproved.FinalMailBody;
                        else
                            QA.FinalMailBody = "";
                    }
                    else
                        QA.FinalMailBody = finalMailBody;


                    QA.Url = url;
                    QA.ViewGP = viewGP ? 1 : 0;
                    QA.Add();
                }
            }
        }

        public static int GetPSMApproverLevelPriority(string region, string sector)
        {
            if (sector.StartsWith("AISC"))
                sector = "AISC";
            var regionParameterVaule = eQuotationDAL.GetRegionParameterValue(region, sector,"PSMApprovalLevel","0");

            return Convert.ToInt32(regionParameterVaule);


        }

        public static decimal GenerateNewApproverLevel(int approverPrimaryLevel, int approverSecondaryLevel)
        {
            return approverPrimaryLevel + approverSecondaryLevel / 10m;
        }

        public static decimal GenerateNewSecondaryApproverLevel(decimal approverLevel)
        {
            return approverLevel + 0.1m;
        }

    }
}
