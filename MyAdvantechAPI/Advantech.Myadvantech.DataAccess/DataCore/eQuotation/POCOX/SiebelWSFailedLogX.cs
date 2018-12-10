using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class SiebelWSFailedLog
    {
        private DbContext CurrentContext
        {
            get
            {
                return eQuotationContext.Current;
            }
        }

        internal void Update()
        {
            CurrentContext.Entry(this).State = EntityState.Modified;
            CurrentContext.SaveChanges();
        }

        internal void Add()
        {
            ((eQEntities)CurrentContext).SiebelWSFailedLog.Add(this);
            CurrentContext.Entry(this).State = EntityState.Added;
            CurrentContext.SaveChanges();
        }
    }
}
