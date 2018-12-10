using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class QuotationMasterHelper : _eQuotationHelper
    {
        /// <summary>
        /// Get  QuotationMaster by quoteID
        /// </summary>
        /// <param name="quoteID">quoteID</param>
        /// <returns>QuotationMaster</returns>
        public QuotationMaster GetQuotationMaster(string quoteID)
        {
            return eQuotationContext.Current.QuotationMaster.Where(x => x.quoteId == quoteID).FirstOrDefault();
        }

        public static List<QuotationMaster> GetQuotationMastersByQuoteNo(string quoteNO)
        {
            return eQuotationContext.Current.QuotationMaster.Where(x => x.quoteNo == quoteNO).ToList();
        }

        public static List<QuotationMaster> GetQuotationMastersByAccountRowID(string rowid)
        {
            return eQuotationContext.Current.QuotationMaster.Where(x => x.quoteToRowId.Equals(rowid)).ToList();
        }

        /// <summary>
        /// Get QuotationMaster for eStore
        /// </summary>
        /// <param name="quoteNo">quoteNo</param>
        /// <param name="status">status</param>
        /// <returns>QuotationMaster</returns>
        public QuotationMaster GetQuotationMasterForeStore(string quoteID, QuoteDocStatus status = QuoteDocStatus.All)
        {
            var master = from quote in context.QuotationMaster
                         where quote.quoteId == quoteID
                         && quote.expiredDate != null
                         && DateTime.Compare(quote.expiredDate.Value, DateTime.Now) > 0
                         select quote;

            if (status == QuoteDocStatus.Finish)
                return (from quote in master
                        where quote.DOCSTATUS != null
                        && quote.DOCSTATUS.Value == (int)status
                        select quote).FirstOrDefault();
            else
                return master.FirstOrDefault();
        }

        /// <summary>
        /// Get all my quotes
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<QuotationMaster> GetMyQuotes(string UserID)
        {
            List<QuotationMaster> qml = new List<QuotationMaster>();
            qml.Add(context.QuotationMaster.Where(x => x.createdBy == UserID).OrderByDescending(x => x.createdDate).FirstOrDefault());
            return qml;
        }


        public static void UpdateQuotationStatus(String _QuoteID, OrderStatus _Status)
        {
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Where(d => d.quoteId.Equals(_QuoteID, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (QM != null)
            {
                QM.DOCSTATUS = (int)_Status;
                eQuotationContext.Current.SaveChanges();
            }
        }

        public static void UpdateQuotationFinishDate(String _QuoteID, DateTime date)
        {
            QuotationMaster QM = eQuotationContext.Current.QuotationMaster.Where(d => d.quoteId.Equals(_QuoteID, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (QM != null)
            {
                QM.QuotationExtensionNew.FinishDate = date;
                eQuotationContext.Current.SaveChanges();
            }
        }

        //public static List<QuotationMaster> GetQuotationMasterBySearchTerms(List<String> _RBUList, String _QuoteNo, String _Description, String _AccountName, String _AccountERPID, String _CreatedBy, String _CreatedFrom, String _CreatedTo, String _LastUpdatedFrom, String _LastUpdatedTo, QuoteDocStatus _Status)
        //{
        //    List<QuotationMaster> finalQM = new List<QuotationMaster>();
        //    try
        //    {
        //        // Parse Date value
        //        Boolean hasCreatedDate = false, hasUpdatedDate = false;
        //        DateTime CreatedFrom = new DateTime(), CreatedTo = new DateTime(), LastUpdatedFrom = new DateTime(), LastUpdatedTo = new DateTime();

        //        DateTime forParse = new DateTime();
        //        if ((!String.IsNullOrEmpty(_CreatedFrom) && DateTime.TryParse(_CreatedFrom, out forParse))
        //            || (!String.IsNullOrEmpty(_CreatedTo) && DateTime.TryParse(_CreatedTo, out forParse)))
        //        {
        //            hasCreatedDate = true;
        //            CreatedFrom = string.IsNullOrEmpty(_CreatedFrom) ? DateTime.Parse("1980-01-01") : DateTime.Parse(_CreatedFrom);
        //            CreatedTo = string.IsNullOrEmpty(_CreatedTo) ? DateTime.Parse("2050-12-31") : DateTime.Parse(_CreatedTo);
        //        }
        //        if ((!String.IsNullOrEmpty(_LastUpdatedFrom) && DateTime.TryParse(_LastUpdatedFrom, out forParse))
        //           || (!String.IsNullOrEmpty(_LastUpdatedTo) && DateTime.TryParse(_LastUpdatedTo, out forParse)))
        //        {
        //            hasUpdatedDate = true;
        //            LastUpdatedFrom = string.IsNullOrEmpty(_LastUpdatedFrom) ? DateTime.Parse("1980-01-01") : DateTime.Parse(_LastUpdatedFrom);
        //            LastUpdatedTo = string.IsNullOrEmpty(_LastUpdatedTo) ? DateTime.Parse("2050-12-31") : DateTime.Parse(_LastUpdatedTo);
        //        }


        //        // Use conditions in where clause.
        //        var result = (from m in eQuotationContext.Current.QuotationMaster
        //                                    join d in eQuotationContext.Current.QuotationDetail on m.quoteId equals d.quoteId into md
        //                                    from x in md.DefaultIfEmpty()
        //                                    join w in eQuotationContext.Current.WorkFlowApproval on m.quoteId equals w.TypeID into mw
        //                                    where m.createdBy.Equals(_CreatedBy, StringComparison.OrdinalIgnoreCase) &&
        //                                          (_RBUList.Contains(m.siebelRBU)) &&
        //                                          (String.IsNullOrEmpty(_QuoteNo) ? true : m.quoteNo.Equals(_QuoteNo, StringComparison.OrdinalIgnoreCase)) &&
        //                                          ((String.IsNullOrEmpty(_Description) ? true : m.customId.Contains(_Description)) || (String.IsNullOrEmpty(_Description) ? true : x.description.Contains(_Description))) &&
        //                                          (String.IsNullOrEmpty(_AccountName) ? true : m.quoteToName.Equals(_AccountName, StringComparison.OrdinalIgnoreCase)) &&
        //                                          (String.IsNullOrEmpty(_AccountERPID) ? true : m.quoteToErpId.Equals(_AccountERPID, StringComparison.OrdinalIgnoreCase)) &&
        //                                          (hasCreatedDate == true ? (CreatedFrom <= m.createdDate && m.createdDate <= CreatedTo) : true) &&
        //                                          (hasUpdatedDate == true ? (LastUpdatedFrom <= m.LastUpdatedDate && m.LastUpdatedDate <= LastUpdatedTo) : true) &&
        //                                          (_Status == QuoteDocStatus.All ? true : (_Status == QuoteDocStatus.Finish ? m.DOCSTATUS == 1 : m.DOCSTATUS == 0)) &&
        //                                          m.DOCSTATUS != 2
        //                                    select new
        //                                    {
        //                                        M = m,
        //                                        ApprovalStatus = m.DOCSTATUS == 1 ? "Finish" : m.DOCSTATUS == 0 ? mw.Where(w => w.Status == (int)QuoteApprovalStatus.Rejected).FirstOrDefault() != null ? "Rejected" : "Draft (Waiting for approval)" : "Draft"
        //                                    })
        //                                     .Distinct().ToList();                                            

        //        foreach (var item in result)
        //        {

        //            item.M.ApprovalStatus = item.ApprovalStatus;
        //            finalQM.Add(item.M);
        //        }               

        //    }
        //    catch (Exception e)
        //    {
        //        return new List<QuotationMaster>();
        //    }
        //    return finalQM.Distinct().OrderByDescending(x => x.quoteNo).ThenByDescending(x => x.Revision_Number).Take(500).ToList();
        //}


        //public static List<QuotationMaster> GetQuotationMastersBySearchTerms(List<String> _RBUList, String _QuoteNo, String _Description, String _AccountName, String _AccountERPID, String _CurrentMail, String _CreatedFrom, String _CreatedTo, String _LastUpdatedFrom, String _LastUpdatedTo, QuoteDocStatus _Status, bool isMyteam, string[] myTeamSalesCodes)
        //{
        //    List<QuotationMaster> finalQM = new List<QuotationMaster>();
        //    try
        //    {
        //        // Parse Date value
        //        Boolean hasCreatedDate = false, hasUpdatedDate = false;
        //        DateTime CreatedFrom = new DateTime(), CreatedTo = new DateTime(), LastUpdatedFrom = new DateTime(), LastUpdatedTo = new DateTime();
        //        string CreatedBy = isMyteam ? "" : _CurrentMail;


        //        DateTime forParse = new DateTime();
        //        if ((!String.IsNullOrEmpty(_CreatedFrom) && DateTime.TryParse(_CreatedFrom, out forParse))
        //            || (!String.IsNullOrEmpty(_CreatedTo) && DateTime.TryParse(_CreatedTo, out forParse)))
        //        {
        //            hasCreatedDate = true;
        //            CreatedFrom = string.IsNullOrEmpty(_CreatedFrom) ? DateTime.Parse("1980-01-01") : DateTime.Parse(_CreatedFrom);
        //            CreatedTo = string.IsNullOrEmpty(_CreatedTo) ? DateTime.Parse("2050-12-31") : DateTime.Parse(_CreatedTo);
        //        }
        //        if ((!String.IsNullOrEmpty(_LastUpdatedFrom) && DateTime.TryParse(_LastUpdatedFrom, out forParse))
        //           || (!String.IsNullOrEmpty(_LastUpdatedTo) && DateTime.TryParse(_LastUpdatedTo, out forParse)))
        //        {
        //            hasUpdatedDate = true;
        //            LastUpdatedFrom = string.IsNullOrEmpty(_LastUpdatedFrom) ? DateTime.Parse("1980-01-01") : DateTime.Parse(_LastUpdatedFrom);
        //            LastUpdatedTo = string.IsNullOrEmpty(_LastUpdatedTo) ? DateTime.Parse("2050-12-31") : DateTime.Parse(_LastUpdatedTo);
        //        }


        //        // Use conditions in where clause.
        //        var result = (from m in eQuotationContext.Current.QuotationMaster
        //                      join d in eQuotationContext.Current.QuotationDetail on m.quoteId equals d.quoteId into md
        //                      from x in md.DefaultIfEmpty()
        //                      join p in eQuotationContext.Current.EQPARTNER on m.quoteId equals p.QUOTEID
        //                      join w in eQuotationContext.Current.WorkFlowApproval on m.quoteId equals w.TypeID into mw
        //                      where
        //                                  ( isMyteam ? true : m.createdBy.Equals(CreatedBy, StringComparison.OrdinalIgnoreCase) ) &&
        //                                  (_RBUList.Contains(m.siebelRBU)) &&
        //                                  (String.IsNullOrEmpty(_QuoteNo) ? true : m.quoteNo.Equals(_QuoteNo, StringComparison.OrdinalIgnoreCase)) &&
        //                                  ((String.IsNullOrEmpty(_Description) ? true : m.customId.Contains(_Description)) || (String.IsNullOrEmpty(_Description) ? true : x.description.Contains(_Description))) &&
        //                                  (String.IsNullOrEmpty(_AccountName) ? true : m.quoteToName.Equals(_AccountName, StringComparison.OrdinalIgnoreCase)) &&
        //                                  (String.IsNullOrEmpty(_AccountERPID) ? true : m.quoteToErpId.Equals(_AccountERPID, StringComparison.OrdinalIgnoreCase)) &&
        //                                  (hasCreatedDate == true ? (CreatedFrom <= m.createdDate && m.createdDate <= CreatedTo) : true) &&
        //                                  (hasUpdatedDate == true ? (LastUpdatedFrom <= m.LastUpdatedDate && m.LastUpdatedDate <= LastUpdatedTo) : true) &&
        //                                  (_Status == QuoteDocStatus.All ? true : (_Status == QuoteDocStatus.Finish ? m.DOCSTATUS == 1 : m.DOCSTATUS == 0)) &&
        //                                  m.DOCSTATUS != 2 && p.TYPE == "E"
        //                      select new
        //                      {
        //                          M = m,
        //                          P = p,
        //                          ApprovalStatus = m.DOCSTATUS == 1 ? "Finish" : m.DOCSTATUS == 0 ? mw.Where(w => w.Status == (int)QuoteApprovalStatus.Rejected).FirstOrDefault() != null ? "Rejected" : mw.Where(w => w.Status == (int)QuoteApprovalStatus.Wait_for_review).FirstOrDefault() != null ? "Draft (Waiting for approval)" : "Draft": "Delete"
        //                      })
        //                                     .Distinct().ToList();

        //        //20180517 Alex 填入quote's approval status for performance issue
        //        foreach (var item in result)
        //        {
        //            item.M.ApprovalStatus = item.ApprovalStatus;

        //        }

        //        if (isMyteam)
        //        {               
        //            foreach (var salesCode in myTeamSalesCodes)
        //            {
        //                finalQM.AddRange(result.Where(r => r.P.ERPID == salesCode).Select(r => r.M).ToList());
        //            }

        //            //Get belong insidesales QM
        //            var insideSalesQM = result.Where(q => String.Equals(q.M.salesEmail, _CurrentMail, StringComparison.CurrentCultureIgnoreCase)).Select(r => r.M).ToList();
        //            if (insideSalesQM != null)
        //                finalQM.AddRange(insideSalesQM);
        //        }
        //        else
        //        {
        //            foreach (var item in result)
        //            {
        //                finalQM.Add(item.M);
        //            }
        //        }



        //    }
        //    catch (Exception e)
        //    {
        //        return new List<QuotationMaster>();
        //    }
        //    return finalQM.Distinct().OrderByDescending(x => x.quoteNo).ThenByDescending(x => x.Revision_Number).Take(300).ToList();
        //}     
    }
}
