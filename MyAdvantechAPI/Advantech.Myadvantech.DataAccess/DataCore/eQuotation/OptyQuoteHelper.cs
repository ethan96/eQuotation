using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class OptyQuoteHelper:_eQuotationHelper
    {
        /// <summary>
        /// Get optyQuote by quoteID
        /// </summary>
        /// <param name="quoteID">quoteID</param>
        /// <returns>optyQuote</returns>
        public optyQuote GetOptyQuote(string quoteID)
        {
            return context.optyQuote.Where(x => x.quoteId == quoteID).FirstOrDefault();
        }
    }
}
