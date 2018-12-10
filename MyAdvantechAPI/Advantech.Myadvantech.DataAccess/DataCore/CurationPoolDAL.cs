using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Advantech.Myadvantech.DataAccess.DataCore
{
    class CurationPoolDAL
    {

        /// <summary>
        /// Get district list of sales
        /// </summary>
        /// <param name="sales_code">Sales code on SAP</param>
        /// <returns>district list in Datatable</returns>
        public static DataTable GetSalesDistricts(String sales_code)
        {
            string _sql = string.Format("select district from US_COMM_SALES_DISTRICT WHERE sales_id='{0}' And (eff_from <= GETDATE() and eff_to>= GETDATE())", sales_code);
            DataTable dt = SqlProvider.dbGetDataTable("CP", _sql);
            return dt;
        }

    }
}
