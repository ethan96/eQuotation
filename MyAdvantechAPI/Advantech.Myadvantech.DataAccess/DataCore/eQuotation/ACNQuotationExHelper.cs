using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    class ACNQuotationExHelper : _eQuotationHelper
    {
        /// <summary>
        /// Get ACNQuotationEx by quoteID
        /// </summary>
        /// <param name="quoteID">quoteID</param>
        /// <returns>List<QuotationDetail></returns>
        public ACN_EQ_QuotationEx GetACNQuotationEx(string quoteID)
        {
            return eQuotationContext.Current.ACN_EQ_QuotationEx.Where(x => x.QuoteID == quoteID).FirstOrDefault();
        }



        /// <summary>
        /// Update ACNQuotationEx
        /// </summary>
        /// <param name="detail">QuotationDetail</param>
        /// <returns>bool</returns>
        public bool UpdateACNQuotationEx(ACN_EQ_QuotationEx quoteEx)
        {
            try
            {
                context.Entry(quoteEx).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
