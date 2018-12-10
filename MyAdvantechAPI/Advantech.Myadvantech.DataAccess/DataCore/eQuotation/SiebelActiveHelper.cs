using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    /// <summary>
    /// eQuotation.dbo.SiebelActive
    /// </summary>
    internal partial class SiebelActiveHelper: _eQuotationHelper
    {
        /// <summary>
        /// Get New SiebelActive entity object
        /// </summary>
        /// <returns>new SiebelActive</returns>
        internal static SiebelActive GetNewSiebelActive()
        {
            return new SiebelActive();
        }
        /// <summary>
        /// Save SiebelActive data
        /// </summary>
        /// <param name="active">SiebelActive object</param>
        /// <returns>int</returns>
        internal int Save(SiebelActive active)
        {
            if (active == null || string.IsNullOrEmpty(active.Status)) return -1;

            try
            {
                context.SiebelActive.Add(active);
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
